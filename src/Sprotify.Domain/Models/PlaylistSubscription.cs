using System;

namespace Sprotify.Domain.Models
{
    public class PlaylistSubscription
    {
        public Guid UserId { get; set; }
        public Guid PlaylistId { get; set; }

        public virtual User User { get; set; }
        public virtual Playlist Playlist { get; set; }

        public DateTimeOffset SubscribedOn { get; set; }
        public int Index { get; set; }
    }
}