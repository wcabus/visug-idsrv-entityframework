using Sprotify.Web.Models;
using Sprotify.Web.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprotify.Web.Services
{
    public class SongService : ApiServiceBase
    {
        public SongService(SprotifyHttpClient sprotifyclient) : base(sprotifyclient)
        {
        }

        public Task<IEnumerable<Song>> GetAllSongs()
        {
            return Get<IEnumerable<Song>>("songs");
        }
    }
}
