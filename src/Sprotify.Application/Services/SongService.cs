using Sprotify.Domain.Services;
using System.Collections.Generic;
using Sprotify.Domain.Dto;
using System.Threading.Tasks;
using Sprotify.Domain.Repositories;

namespace Sprotify.Application.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;

        public SongService(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        public Task<IEnumerable<SongResult>> GetSongs()
        {
            return _songRepository.GetSongs(null);
        }

        public Task<IEnumerable<SongResult>> GetSongs(string filter)
        {
            return _songRepository.GetSongs(filter);
        }
    }
}
