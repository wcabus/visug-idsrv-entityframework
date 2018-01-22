using System;

namespace Sprotify.Domain.Models
{
    public class PlaylistSong
    {
        public Guid Id { get; set; }

        public Guid PlaylistId { get; set; }
        public virtual Playlist Playlist { get; set; }

        public Guid SongId { get; set; }
        public virtual Song Song { get; set; }

        public Guid AddedById { get; set; }
        public virtual User AddedBy { get; set; }

        public DateTimeOffset AddedOn { get; set; }
        public int Index { get; set; }
    }
}