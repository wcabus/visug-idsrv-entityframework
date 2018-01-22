using System;
using System.Collections.Generic;

namespace Sprotify.Domain.Models
{
    public class User
    {
        public User() { }

        public User(Guid id, string name)
        {
            Id = id;
            Name = name;
            Registered = DateTimeOffset.UtcNow;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Registered { get; set; }

        public virtual ICollection<UserSubscription> Subscriptions { get; set; }

        public virtual ICollection<PlaylistSubscription> Playlists { get; set; }

        public UserSubscription SubscribeTo(Subscription subscription)
        {
            if (Subscriptions == null)
            {
                Subscriptions = new List<UserSubscription>();
            }

            var userSubscription = new UserSubscription
            {
                User = this,
                Subscription = subscription,
                SubscribedOn = DateTimeOffset.UtcNow,
                SubscriptionValidUntil = DateTimeOffset.MaxValue
            };

            Subscriptions.Add(userSubscription);
            return userSubscription;
        }
    }
}