using Sprotify.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Sprotify.Domain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Sprotify.DAL.Repositories
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly SprotifyDbContext _context;

        public PlaylistRepository(SprotifyDbContext context)
        {
            _context = context;
        }

        public Playlist Create(Playlist playlist)
        {
            return _context.Set<Playlist>().Add(playlist).Entity;
        }

        public async Task<Playlist> GetPlaylistById(Guid playlistId, Guid currentUserId)
        {
            var playlist = await _context.Set<Playlist>()
                .Include(x => x.Creator)
                .Where(x => x.CreatorId == currentUserId || x.IsPrivate == false)
                .FirstOrDefaultAsync(x => x.Id == playlistId)
                .ConfigureAwait(false);

            if (playlist == null)
            {
                return null;
            }

            await _context.Entry(playlist)
                .Collection(x => x.Songs)
                .Query()
                .Include(x => x.Song.Band)
                .LoadAsync()
                .ConfigureAwait(false);

            return playlist;
        }

        public async Task<IEnumerable<Playlist>> GetPlaylists(Guid userId)
        {
            var ownedPlaylists = await GetPlaylistsForUser(userId, true).ConfigureAwait(false);

            var publicPlaylists = await _context.Set<Playlist>()
                .Include(x => x.Creator)
                .Where(x => x.CreatorId != userId)
                .Where(x => x.IsPrivate == false)
                .ToListAsync()
                .ConfigureAwait(false);

            return ownedPlaylists.Union(publicPlaylists);
        }

        public async Task<IEnumerable<Playlist>> GetPlaylistsForUser(Guid userId, bool includePrivate)
        {
            var query = _context.Set<Playlist>()
                .Include(x => x.Creator)
                .Where(x => x.CreatorId == userId);

            if (!includePrivate)
            {
                query = query.Where(x => x.IsPrivate == false);
            }

            return await query.ToListAsync()
                .ConfigureAwait(false);
        }

        public Task LoadSongs(Playlist playlist)
        {
            return _context.Entry(playlist)
                .Collection(x => x.Songs)
                .LoadAsync();
        }
    }
}
