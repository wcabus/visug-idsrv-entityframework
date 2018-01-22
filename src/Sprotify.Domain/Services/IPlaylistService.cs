using Sprotify.Domain.Dto;
using Sprotify.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprotify.Domain.Services
{
    public interface IPlaylistService
    {
        Task<IEnumerable<Playlist>> GetPlaylists(Guid userId);
        Task<IEnumerable<Playlist>> GetPlaylistsForUser(Guid userId, bool includePrivate);
        Task<Playlist> GetPlaylistById(Guid playlistId, Guid currentUserId);

        Task<Playlist> CreatePlaylist(Guid userId, string title, bool isPrivate, bool isCollaborative);
        Task<PlaylistSong> AddSongToPlaylist(Playlist playlist, Song song, Guid userId);
        Task<bool> UserExists(Guid userId);
        Task<SongResult> GetSongById(Guid id);
    }
}
