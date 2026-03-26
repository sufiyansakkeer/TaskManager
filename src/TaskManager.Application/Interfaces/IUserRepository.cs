using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);

        Task<User?> GetByEmailAsync(string email);

        Task<bool> IsEmailAlreadyExistAsync(string email);

        Task AddAsync(User user);

    }
}