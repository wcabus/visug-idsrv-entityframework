using Microsoft.EntityFrameworkCore;
using Sprotify.Domain.Models;
using Sprotify.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sprotify.DAL.Repositories
{
    public class AlbumRepository : IAlbumRepository
    {
        private readonly SprotifyDbContext _context;

        public AlbumRepository(SprotifyDbContext context)
        {
            _context = context;
        }

        public Task<bool> Exists(Guid id)
        {
            return _context.Set<Album>().AnyAsync(x => x.Id == id);
        }

        public async Task<Album> GetAlbumById(Guid id, bool includeSongs)
        {
            var album = await _context.Set<Album>()
                .Include(x => x.Band)
                .FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);

            if (album == null)
            {
                return null;
            }

            if (includeSongs)
            {
                await _context.Entry(album)
                    .Collection(x => x.Songs)
                    .Query()
                    .Include(x => x.Song)
                    .LoadAsync()
                    .ConfigureAwait(false);
            }

            return album;
        }

        public async Task<IEnumerable<Album>> GetAlbumsForBand(Guid bandId)
        {
            return await _context.Set<Album>()
                .Include(x => x.Band)
                .Where(x => x.BandId == bandId)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Album>> GetAlbums(string filter)
        {
            var query = _context.Set<Album>()
                    .Include(x => x.Band)
                    .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query
                    .Where(x => x.Title.StartsWith(filter) || x.Band.Name.StartsWith(filter));
            }

            return await query
                    .ToListAsync()
                    .ConfigureAwait(false);
        }
    }
}
