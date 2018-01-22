using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sprotify.Domain.Models;
using Sprotify.Domain.Repositories;

namespace Sprotify.DAL.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly SprotifyDbContext _context;

        public SubscriptionRepository(SprotifyDbContext context)
        {
            _context = context;
        }

        public Subscription Create(Subscription subscription)
        {
            return _context.Set<Subscription>().Add(subscription).Entity;
        }

        public async Task<IEnumerable<Subscription>> Get()
        {
            return await _context.Set<Subscription>().ToListAsync().ConfigureAwait(false);
        }

        public Task<Subscription> GetById(Guid id)
        {
            return _context.Set<Subscription>().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}