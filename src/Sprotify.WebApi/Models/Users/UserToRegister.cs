using System;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.WebApi.Models.Users
{
    public class UserToRegister
    {
        [Required]
        public Guid Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }
    }
}