using Sprotify.WebApi.Models.Songs;
using System;
using System.Collections.Generic;

namespace Sprotify.WebApi.Models.Playlists
{
    public class Playlist
    {
        public Guid Id { get; set; }

        public Guid CreatorId { get; set; }
        public string CreatorName { get; set; }

        public string Title { get; set; }
    }

    public class PlaylistWithSongs : Playlist
    {
        public IEnumerable<Song> Songs { get; set; }
    }
}
