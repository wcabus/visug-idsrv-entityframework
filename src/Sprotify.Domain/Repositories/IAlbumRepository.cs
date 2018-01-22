using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sprotify.Domain.Models;

namespace Sprotify.Domain.Repositories
{
    public interface IAlbumRepository
    {
        Task<bool> Exists(Guid id);
        Task<Album> GetAlbumById(Guid id, bool includeSongs);
        Task<IEnumerable<Album>> GetAlbumsForBand(Guid bandId);
        Task<IEnumerable<Album>> GetAlbums(string filter);
    }
}
