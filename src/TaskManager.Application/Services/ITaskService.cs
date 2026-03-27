using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.TaskItem;

namespace TaskManager.Application.Interfaces.Service
{
    public interface ITaskService
    {
        Task<TaskDto> GetTaskByIdAsync(Guid taskId, Guid userId);

        Task<List<TaskDto>> GetUserTaskAsync(Guid userId);

        Task<List<TaskDto>> GetAllTaskAsync();

        Task<TaskDto> CreateTaskAsync(Guid userId, CreateTaskDto request);

        Task UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto request);


        Task<bool> DeleteTaskAsync(Guid userId, Guid taskId);


    }
}