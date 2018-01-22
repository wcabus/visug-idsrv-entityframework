using Sprotify.Domain.Services;
using System;
using System.Collections.Generic;
using Sprotify.Domain.Models;
using System.Threading.Tasks;
using Sprotify.Domain.Repositories;
using Sprotify.DAL;

namespace Sprotify.Application.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IBandRepository _bandRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly UnitOfWork _unitOfWork;

        public AlbumService(
            IBandRepository bandRepository,
            IAlbumRepository albumRepository,
            UnitOfWork unitOfWork
        )
        {
            _bandRepository = bandRepository;
            _albumRepository = albumRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<bool> AlbumExists(Guid id)
        {
            return _albumRepository.Exists(id);
        }

        public Task<bool> BandExists(Guid id)
        {
            return _bandRepository.Exists(id);
        }

        public async Task<Album> CreateAlbum(Guid bandId, string title, DateTime? releaseDate, string art)
        {
            var band = await _bandRepository.GetBandById(bandId).ConfigureAwait(false);
            if (band == null)
            {
                return null;
            }

            var album = band.AddAlbum(title, releaseDate, art);
            await _unitOfWork.SaveChanges().ConfigureAwait(false);

            return album;
        }

        public async Task<Album> UpdateAlbum(Album album)
        {
            await _unitOfWork.SaveChanges().ConfigureAwait(false);

            return album;
        }

        public Task<Album> GetAlbumById(Guid id, bool includeSongs)
        {
            return _albumRepository.GetAlbumById(id, includeSongs);
        }

        public Task<IEnumerable<Album>> GetAlbumsForBand(Guid bandId)
        {
            return _albumRepository.GetAlbumsForBand(bandId);
        }

        public Task<IEnumerable<Album>> GetAlbums(string filter)
        {
            return _albumRepository.GetAlbums(filter);
        }
    }
}
