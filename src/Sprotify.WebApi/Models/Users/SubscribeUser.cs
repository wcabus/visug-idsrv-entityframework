using System;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.WebApi.Models.Users
{
    public class SubscribeUser
    {
        [Required]
        public Guid SubscriptionId { get; set; }
    }
}
