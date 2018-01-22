using System;
using System.ComponentModel.DataAnnotations;

namespace Sprotify.WebApi.Models.Playlists
{
    public class AddSongToPlaylist
    {
        [Required]
        public Guid Id { get; set; }
    }
}
