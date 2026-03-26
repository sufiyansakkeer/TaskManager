using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetByIdAsync(Guid id);

        Task<List<TaskItem>> GetByUserIdAsync(Guid userId);

        Task<List<TaskItem>> GetAllAsync();


        void Update(TaskItem taskItem);

        void Delete(TaskItem task);


        Task AddAsync(TaskItem taskItem);




    }
}