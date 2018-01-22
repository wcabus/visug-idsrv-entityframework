using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sprotify.Domain.Models;

namespace Sprotify.Domain.Repositories
{
    public interface ISubscriptionRepository
    {
        Subscription Create(Subscription subscription);

        Task<IEnumerable<Subscription>> Get();
        Task<Subscription> GetById(Guid id);
    }
}