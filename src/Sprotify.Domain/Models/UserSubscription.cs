using System;

namespace Sprotify.Domain.Models
{
    public class UserSubscription
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public Guid SubscriptionId { get; set; }

        public virtual User User { get; set; }
        public virtual Subscription Subscription { get; set; }

        public DateTimeOffset SubscribedOn { get; set; }
        public DateTimeOffset SubscriptionValidUntil { get; set; }
    }
}