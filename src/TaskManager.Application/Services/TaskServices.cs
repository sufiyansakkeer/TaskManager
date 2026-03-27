using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.TaskItem;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Interfaces.Service;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services
{
    public class TaskServices : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUnitOfWork _unitOfWork;
        public TaskServices(ITaskRepository taskRepository, IUnitOfWork unitOfWork)
        {
            _taskRepository = taskRepository;
            _unitOfWork = unitOfWork;

        }
        public async Task<TaskDto> CreateTaskAsync(Guid userId, CreateTaskDto request)
        {
            var task = new TaskItem(request.Title, request.Description, userId);
            await _taskRepository.AddAsync(task);
            await _unitOfWork.SaveChangesAsync();
            return MapToDto(task);

        }

        public async Task<bool> DeleteTaskAsync(Guid userId, Guid taskId)
        {

            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null || task.UserId != userId)
                return false;


            _taskRepository.Delete(task);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<List<TaskDto>> GetAllTaskAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return [.. tasks.Select(MapToDto)];
        }

        public async Task<TaskDto> GetTaskByIdAsync(Guid taskId, Guid userId)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null || task.Id == userId)
                throw new Exception("Task not found or unauthorized");

            return MapToDto(task);
        }



        public async Task<List<TaskDto>> GetUserTaskAsync(Guid userId)
        {
            var tasks = await _taskRepository.GetByUserIdAsync(userId);
            return [.. tasks.Select(MapToDto)];
        }

        public async Task UpdateTaskAsync(Guid userId, Guid taskId, UpdateTaskDto request)
        {
            var task = await _taskRepository.GetByIdAsync(taskId);
            if (task == null || task.UserId != userId)
                throw new Exception("Task not found or unauthorized");

            task.Update(request.Title, request.Description);

            _taskRepository.Update(task);

            await _unitOfWork.SaveChangesAsync();

        }

        private TaskDto MapToDto(TaskItem task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }
    }
}