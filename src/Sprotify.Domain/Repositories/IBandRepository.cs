using Sprotify.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprotify.Domain.Repositories
{
    public interface IBandRepository
    {
        Task<IEnumerable<Band>> GetBands(string filter);
        Task<Band> GetBandById(Guid id);
        Band CreateBand(Band band);
        Task<bool> Exists(Guid id);
    }
}
