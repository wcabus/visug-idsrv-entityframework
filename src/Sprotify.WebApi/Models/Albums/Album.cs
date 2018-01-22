using System;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.WebApi.Models.Albums
{
    public class Album
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Art { get; set; }

        public string Band { get; set; }
    }

    public class AlbumToCreate
    {
        [Required, StringLength(200)]
        public string Title { get; set; }

        public DateTime? ReleaseDate { get; set; }

        [StringLength(1000), Url]
        public string Art { get; set; }
    }

    public class AlbumToUpdate : AlbumToCreate {}
}
