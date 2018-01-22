using Sprotify.Domain.Services;
using System;
using System.Collections.Generic;
using Sprotify.Domain.Models;
using System.Threading.Tasks;
using Sprotify.Domain.Repositories;
using Sprotify.DAL;
using Sprotify.Domain.Dto;

namespace Sprotify.Application.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISongRepository _songRepository;
        private readonly UnitOfWork _unitOfWork;

        public PlaylistService(
            IPlaylistRepository playlistRepository,
            IUserRepository userRepository,
            ISongRepository songRepository,
            UnitOfWork unitOfWork
        )
        {
            _playlistRepository = playlistRepository;
            _userRepository = userRepository;
            _songRepository = songRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PlaylistSong> AddSongToPlaylist(Playlist playlist, Song song, Guid userId)
        {
            await _playlistRepository.LoadSongs(playlist).ConfigureAwait(false);

            var playlistSong = playlist.AddSong(song, userId);
            await _unitOfWork.SaveChanges().ConfigureAwait(false);

            return playlistSong;
        }

        public async Task<Playlist> CreatePlaylist(Guid userId, string title, bool isPrivate, bool isCollaborative)
        {
            var playlist = new Playlist(userId, title, isPrivate, isCollaborative);
            _playlistRepository.Create(playlist);

            await _unitOfWork.SaveChanges().ConfigureAwait(false);

            return playlist;
        }
        
        public Task<Playlist> GetPlaylistById(Guid playlistId, Guid currentUserId)
        {
            return _playlistRepository.GetPlaylistById(playlistId, currentUserId);
        }

        public Task<IEnumerable<Playlist>> GetPlaylists(Guid userId)
        {
            return _playlistRepository.GetPlaylists(userId);
        }

        public Task<IEnumerable<Playlist>> GetPlaylistsForUser(Guid userId, bool includePrivate)
        {
            return _playlistRepository.GetPlaylistsForUser(userId, includePrivate);
        }

        public Task<SongResult> GetSongById(Guid id)
        {
            return _songRepository.GetSongById(id);
        }

        public Task<bool> UserExists(Guid userId)
        {
            return _userRepository.Exists(userId);
        }
    }
}
