using Sprotify.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprotify.Domain.Services
{
    public interface IAlbumService
    {
        Task<IEnumerable<Album>> GetAlbums(string filter);

        Task<bool> BandExists(Guid id);
        Task<bool> AlbumExists(Guid id);
        Task<IEnumerable<Album>> GetAlbumsForBand(Guid bandId);
        Task<Album> GetAlbumById(Guid id, bool includeSongs);
        Task<Album> CreateAlbum(Guid bandId, string title, DateTime? releaseDate, string art);
        Task<Album> UpdateAlbum(Album album);
        
    }
}
