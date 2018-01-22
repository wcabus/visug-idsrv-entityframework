using Sprotify.Domain.Repositories;
using System.Collections.Generic;
using Sprotify.Domain.Dto;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Sprotify.Domain.Models;
using System;

namespace Sprotify.DAL.Repositories
{
    public class SongRepository : ISongRepository
    {
        private readonly SprotifyDbContext _context;

        public SongRepository(SprotifyDbContext context)
        {
            _context = context;
        }

        public async Task<SongResult> GetSongById(Guid id)
        {
            var query = from song in _context.Set<Song>()
                        join alSo in _context.Set<AlbumSong>() on song.Id equals alSo.SongId
                        join album in _context.Set<Album>() on alSo.AlbumId equals album.Id
                        join band in _context.Set<Band>() on album.BandId equals band.Id
                        where song.Id == id
                        select new SongResult
                        {
                            Song = song,
                            Album = album,
                            Band = band,
                            Position = alSo.Position
                        };

            return await query.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<SongResult>> GetSongs(string filter)
        {
            var query = from song in _context.Set<Song>()
                        join alSo in _context.Set<AlbumSong>() on song.Id equals alSo.SongId
                        join album in _context.Set<Album>() on alSo.AlbumId equals album.Id
                        join band in _context.Set<Band>() on album.BandId equals band.Id
                        where filter == null
                        || song.Title.StartsWith(filter)
                        || band.Name.StartsWith(filter)
                        || album.Title.StartsWith(filter)
                        select new SongResult
                        {
                            Song = song,
                            Album = album,
                            Band = band,
                            Position = alSo.Position
                        };

            return await query.Distinct().ToListAsync().ConfigureAwait(false);
        }
    }
}
