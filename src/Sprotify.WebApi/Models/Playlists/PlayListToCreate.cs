using System.ComponentModel.DataAnnotations;

namespace Sprotify.WebApi.Models.Playlists
{
    public class PlayListToCreate
    {
        [Required, StringLength(100)]
        public string Title { get; set; }

        public bool IsPrivate { get; set; }
        public bool IsCollaborative { get; set; }
    }
}
