using Sprotify.Web.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.Web.Areas.Administration.Models.Albums
{
    public class AlbumToCreate
    {
        [Required, StringLength(100)]
        public string Title { get; set; }

        [Display(Name = "Release date")]
        public DateTimeOffset? ReleaseDate { get; set; }

        [Display(Name = "Album art")]
        [DataType(DataType.ImageUrl), Url]
        public string Art { get; set; }
    }

    public class AlbumToEdit : AlbumToCreate
    {
        public AlbumToEdit()
        {

        }

        public AlbumToEdit(Album album)
        {
            Title = album.Title;
            ReleaseDate = album.ReleaseDate;
            Art = album.Art;
        }
    }
}
