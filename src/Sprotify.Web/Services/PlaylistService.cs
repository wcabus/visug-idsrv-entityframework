using Sprotify.Web.Models;
using Sprotify.Web.Models.Player;
using Sprotify.Web.Services.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprotify.Web.Services
{
    public class PlaylistService : ApiServiceBase
    {
        public PlaylistService(SprotifyHttpClient client) : base(client) { }

        public Task<IEnumerable<Playlist>> GetMyPlaylists(Guid userId)
        {
            return Get<IEnumerable<Playlist>>($"users/{userId}/playlists");
        }

        public Task<PlaylistWithSongs> GetPlaylistById(Guid playlistId)
        {
            return Get<PlaylistWithSongs>($"playlists/{playlistId}");
        }

        public Task<Playlist> CreatePlaylist(CreatePlaylistModel model)
        {
            return Post<Playlist>($"playlists", model);
        }

        public async Task<SearchResult> Search(string filter)
        {
            var bands = await Get<IEnumerable<SearchItem>>($"bands?filter={filter}").ConfigureAwait(false);
            var albums = await Get<IEnumerable<SearchItem>>($"albums?filter={filter}").ConfigureAwait(false);
            var songs = await Get<IEnumerable<SearchItem>>($"songs/search?filter={filter}").ConfigureAwait(false);

            return new SearchResult(filter, bands, albums, songs);
        }

        public Task AddSongToPlaylist(Guid playlistId, Guid id)
        {
            return Post<Song>($"playlists/{playlistId}/songs", new { id });
        }
    }
}
