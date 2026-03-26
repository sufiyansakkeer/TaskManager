using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;


namespace TaskManager.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(User user)
        {
            await _context.AddAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<bool> IsEmailAlreadyExistAsync(string email)
        {
            return _context.Users.AnyAsync(x => x.Email == email);
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}