using System.Collections.Generic;
using System.Linq;

namespace Sprotify.Web.Models
{
    public class AlbumWithSongs : Album
    {
        public AlbumWithSongs()
        {

        }

        public AlbumWithSongs(IEnumerable<Song> songs)
        {
            var first = songs.FirstOrDefault();
            Band = first?.Band;
            Title = first?.Album;
            Art = first?.AlbumArt;

            Songs = songs;
        }

        public IEnumerable<Song> Songs { get; set; }
    }
}
