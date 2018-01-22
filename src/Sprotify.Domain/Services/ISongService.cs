using Sprotify.Domain.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprotify.Domain.Services
{
    public interface ISongService
    {
        Task<IEnumerable<SongResult>> GetSongs();
        Task<IEnumerable<SongResult>> GetSongs(string filter);
    }
}
