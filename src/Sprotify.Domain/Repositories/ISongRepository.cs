using Sprotify.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Sprotify.Domain.Repositories
{
    public interface ISongRepository
    {
        Task<IEnumerable<SongResult>> GetSongs(string filter);
        Task<SongResult> GetSongById(Guid id);
    }
}
