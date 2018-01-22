using System;

namespace Sprotify.Web.Models
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public bool ContainsExplicitLyrics { get; set; }

        public Guid BandId { get; set; }
        public string Band { get; set; }

        public Guid? AlbumId { get; set; }
        public string Album { get; set; }
        public string AlbumArt { get; set; }

        public int Position { get; set; }
    }
}
