using System;

namespace Sprotify.Domain.Models
{
    public class Song
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public DateTime? ReleaseDate { get; set; }

        public bool ContainsExplicitLyrics { get; set; }

        public Guid BandId { get; set; }
        public virtual Band Band { get; set; }
    }
}