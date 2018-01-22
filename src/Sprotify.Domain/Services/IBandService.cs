using Sprotify.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprotify.Domain.Services
{
    public interface IBandService
    {
        Task<IEnumerable<Band>> GetBands(string filter);
        Task<Band> GetBandById(Guid id);
        Task<Band> CreateBand(string name);
        Task<Band> UpdateBand(Band band);
    }
}
