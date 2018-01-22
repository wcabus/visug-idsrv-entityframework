using Sprotify.Domain.Repositories;
using System;
using System.Collections.Generic;
using Sprotify.Domain.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Sprotify.DAL.Repositories
{
    public class BandRepository : IBandRepository
    {
        private readonly SprotifyDbContext _context;

        public BandRepository(SprotifyDbContext context)
        {
            _context = context;
        }

        public Band CreateBand(Band band)
        {
            return _context.Set<Band>().Add(band).Entity;
        }

        public Task<bool> Exists(Guid id)
        {
            return _context.Set<Band>().AnyAsync(x => x.Id == id);
        }

        public Task<Band> GetBandById(Guid id)
        {
            return _context.Set<Band>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Band>> GetBands(string filter)
        {
            var query = _context.Set<Band>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(x => x.Name.StartsWith(filter));
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }
    }
}
