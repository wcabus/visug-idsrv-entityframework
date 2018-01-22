using System.Collections.Generic;
using System.Threading.Tasks;
using Sprotify.Web.Models;
using Sprotify.Web.Services.Core;

namespace Sprotify.Web.Services
{
    public class SubscriptionService : ApiServiceBase
    {
        public SubscriptionService(SprotifyHttpClient client) : base(client)
        {
        }

        public virtual Task<IEnumerable<Subscription>> GetSubscriptions()
        {
            return Get<IEnumerable<Subscription>>("subscriptions");
        }
    }
}