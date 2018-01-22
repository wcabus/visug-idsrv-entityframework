using System;
using System.Collections.Generic;

namespace Sprotify.WebApi.Models.Albums
{
    public class AlbumWithSongs : Album
    {
        public ICollection<AlbumSong> Songs { get; set; }
    }

    public class AlbumSong
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public bool ContainsExplicitLyrics { get; set; }

        public int Position { get; set; }
    }
}
