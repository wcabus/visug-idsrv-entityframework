using System;

namespace Sprotify.Domain.Models
{
    public class AlbumSong
    {
        public Guid AlbumId { get; set; }
        public Guid SongId { get; set; }
        public int Position { get; set; }

        public virtual Album Album { get; set; }
        public virtual Song Song { get; set; }
    }
}