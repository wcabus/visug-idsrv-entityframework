using System;

namespace Sprotify.WebApi.Models.Users
{
    public class UserSubscription
    {
        public Guid Id { get; set; }

        public Guid SubscriptionId { get; set; }
        public DateTimeOffset SubscribedOn { get; set; }
        public DateTimeOffset SubscriptionValidUntil { get; set; }
    }
}
