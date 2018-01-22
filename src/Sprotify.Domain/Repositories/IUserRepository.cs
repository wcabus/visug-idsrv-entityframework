using System;
using System.Threading.Tasks;
using Sprotify.Domain.Models;

namespace Sprotify.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetById(Guid id);

        User Create(User user);
        Task<bool> Exists(Guid userId);
    }
}