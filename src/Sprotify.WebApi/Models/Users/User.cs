using System;
using System.Collections.Generic;

namespace Sprotify.WebApi.Models.Users
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ICollection<UserSubscription> Subscriptions { get; set; }
    }
}