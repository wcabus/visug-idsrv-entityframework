using System;
using System.Threading.Tasks;
using Sprotify.DAL;
using Sprotify.Domain.Models;
using Sprotify.Domain.Repositories;
using Sprotify.Domain.Services;

namespace Sprotify.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly UnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            ISubscriptionRepository subscriptionRepository,
            UnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<Subscription> GetSubscriptionById(Guid id)
        {
            return _subscriptionRepository.GetById(id);
        }

        public Task<User> GetUserById(Guid id)
        {
            return _userRepository.GetById(id);
        }

        public async Task<User> RegisterUser(Guid id, string name)
        {
            var user = new User(id, name);
            _userRepository.Create(user);
            await _unitOfWork.SaveChanges().ConfigureAwait(false);

            return user;
        }

        public async Task<UserSubscription> Subscribe(User user, Subscription subscription)
        {
            var userSubscription = user.SubscribeTo(subscription);
            await _unitOfWork.SaveChanges().ConfigureAwait(false);

            return userSubscription;
        }
    }
}