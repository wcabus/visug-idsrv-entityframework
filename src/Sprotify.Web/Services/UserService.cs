using System;
using System.Threading.Tasks;
using Sprotify.Web.Models;
using Sprotify.Web.Services.Core;
using System.Linq;

namespace Sprotify.Web.Services
{
    public class UserService : ApiServiceBase
    {
        public UserService(SprotifyHttpClient sprotifyclient) : base(sprotifyclient)
        {
        }

        public async Task EnsureUserExists(Guid userId, string name)
        {
            try
            {
                await Get<User>($"users/{userId}").ConfigureAwait(false);
            }
            catch (ResourceNotFoundException)
            {
                await Post<User>("users", new {id = userId, name = name}).ConfigureAwait(false);
            }
        }

        public async Task<bool> IsSubscribed(Guid userId)
        {
            var user = await Get<User>($"users/{userId}").ConfigureAwait(false);
            return user.Subscriptions?.Any(x => DateTimeOffset.UtcNow <= x.SubscriptionValidUntil) ?? false;
        }

        public async Task Subscribe(Guid userId, Guid subscription)
        {
            await Post<UserSubscription>($"users/{userId}/subscriptions", new { subscriptionId = subscription });
        }
    }
}