using Sprotify.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprotify.Domain.Repositories
{
    public interface IPlaylistRepository
    {
        Task<IEnumerable<Playlist>> GetPlaylists(Guid userId);
        Task<IEnumerable<Playlist>> GetPlaylistsForUser(Guid userId, bool includePrivate);
        Task<Playlist> GetPlaylistById(Guid playlistId, Guid currentUserId);
        Playlist Create(Playlist playlist);
        Task LoadSongs(Playlist playlist);
    }
}
