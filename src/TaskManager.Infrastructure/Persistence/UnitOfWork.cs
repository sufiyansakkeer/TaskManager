using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUserRepository Users { get; set; }
        public ITaskRepository Tasks { get; set; }
        public UnitOfWork(AppDbContext context, IUserRepository userRepository, ITaskRepository taskRepository)
        {
            _context = context;
            Users = userRepository;
            Tasks = taskRepository;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}