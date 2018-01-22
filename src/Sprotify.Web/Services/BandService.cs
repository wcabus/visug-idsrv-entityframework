using Sprotify.Web.Models;
using Sprotify.Web.Services.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sprotify.Web.Services
{
    public class BandService : ApiServiceBase
    {
        public BandService(SprotifyHttpClient sprotifyclient) : base(sprotifyclient)
        {
        }

        public Task<IEnumerable<Band>> GetBands() => Get<IEnumerable<Band>>("bands");
        public async Task<Band> GetBandById(Guid id)
        {
            try
            {
                return await Get<Band>($"bands/{id}").ConfigureAwait(false);
            }
            catch (ResourceNotFoundException)
            {
            }

            return null;
        }

        public Task<Band> CreateBand(string name) => Post<Band>("bands", new { name = name });

        public Task UpdateBand(Guid id, string name) => Put<Band>($"bands/{id}", new { name = name });
    }
}
