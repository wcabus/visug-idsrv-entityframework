using System;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.Web.Models
{
    public class Album
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        [Display(Name = "Release date")]
        public DateTimeOffset? ReleaseDate { get; set; }

        [Display(Name = "Album art")]
        [DataType(DataType.ImageUrl), Url]
        public string Art { get; set; }

        public string Band { get; set; }
    }
}
