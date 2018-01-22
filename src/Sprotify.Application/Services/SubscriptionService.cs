using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sprotify.DAL;
using Sprotify.Domain.Models;
using Sprotify.Domain.Repositories;
using Sprotify.Domain.Services;

namespace Sprotify.Application.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly UnitOfWork _unitOfWork;

        public SubscriptionService(
            ISubscriptionRepository subscriptionRepository, 
            UnitOfWork unitOfWork)
        {
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Subscription> CreateSubscription(string title, string description, decimal pricePerMonth,
            bool hasAdvertisements, bool canOnlyShuffle, bool canPlayOffline, bool hasHighQualityStreams)
        {
            var subscription = new Subscription(title, description, pricePerMonth, hasAdvertisements, canOnlyShuffle, canPlayOffline, hasHighQualityStreams);
            _subscriptionRepository.Create(subscription);
            await _unitOfWork.SaveChanges().ConfigureAwait(false);

            return subscription;
        }

        public Task<IEnumerable<Subscription>> GetSubscriptions()
        {
            return _subscriptionRepository.Get();
        }

        public Task<Subscription> GetSubscriptionById(Guid id)
        {
            return _subscriptionRepository.GetById(id);
        }
    }
}