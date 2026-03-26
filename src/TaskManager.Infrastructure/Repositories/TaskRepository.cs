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
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(TaskItem taskItem)
        {
            await _context.Tasks.AddAsync(taskItem);
        }

        public void Delete(TaskItem task)
        {
            _context.Tasks.Remove(task);
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<TaskItem>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Tasks.Where(x => x.UserId == userId).ToListAsync();
        }

        public void Update(TaskItem taskItem)
        {
            _context.Tasks.Update(taskItem);
        }
    }
}