using System;
using System.Collections.Generic;
using System.Linq;

namespace Sprotify.Domain.Models
{
    public class Playlist
    {
        public Playlist()
        {

        }

        public Playlist(Guid userId, string title, bool isPrivate, bool isCollaborative)
        {
            CreatorId = userId;
            CreatedOn = DateTimeOffset.UtcNow;
            LastUpdatedOn = DateTimeOffset.UtcNow;

            Title = title;
            IsPrivate = isPrivate;
            IsCollaborative = isCollaborative;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }

        public Guid CreatorId { get; set; }
        public virtual User Creator { get; set; }

        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset LastUpdatedOn { get; set; }
        public bool IsCollaborative { get; set; }
        public bool IsPrivate { get; set; }

        public virtual ICollection<PlaylistSong> Songs { get; set; } = new List<PlaylistSong>();

        public PlaylistSong AddSong(Song song, Guid userId)
        {
            var playlistSong = new PlaylistSong
            {
                AddedById = userId,
                AddedOn = DateTimeOffset.UtcNow,
                Song = song,
                Index = Songs.Count() + 1
            };

            Songs.Add(playlistSong);
            LastUpdatedOn = DateTimeOffset.UtcNow;

            return playlistSong;
        }
    }
}