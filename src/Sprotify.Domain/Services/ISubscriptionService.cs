using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sprotify.Domain.Models;

namespace Sprotify.Domain.Services
{
    public interface ISubscriptionService
    {
        Task<Subscription> CreateSubscription(string title, string description, decimal pricePerMonth, bool hasAdvertisements, bool canOnlyShuffle, bool canPlayOffline, bool hasHighQualityStreams);

        Task<IEnumerable<Subscription>> GetSubscriptions();
        Task<Subscription> GetSubscriptionById(Guid id);
    }
}