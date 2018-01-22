using System;
using System.Collections.Generic;

namespace Sprotify.Web.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserSubscription> Subscriptions { get; set; }
    }

    public class UserSubscription
    {
        public Guid Id { get; set; }

        public Guid SubscriptionId { get; set; }
        public DateTimeOffset SubscribedOn { get; set; }
        public DateTimeOffset SubscriptionValidUntil { get; set; }
    }
}