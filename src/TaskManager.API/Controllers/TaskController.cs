using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManager.Application.DTOs.TaskItem;
using TaskManager.Application.Interfaces.Service;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITaskService _taskService;

        public TaskController(ILogger<TaskController> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskDto request)
        {
            var userId = GetUserId();
            var result = await _taskService.CreateTaskAsync(userId, request);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTasks()
        {
            var userId = GetUserId();
            var result = await _taskService.GetUserTaskAsync(userId);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(Guid id)
        {
            var userId = GetUserId();
            var task = await _taskService.GetTaskByIdAsync(id, userId);
            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, UpdateTaskDto request)
        {
            var userId = GetUserId();
            await _taskService.UpdateTaskAsync(userId, id, request);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var userId = GetUserId();
            var deleted = await _taskService.DeleteTaskAsync(userId, id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }




        private Guid GetUserId()
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User.FindFirst("sub")?.Value;
            if (string.IsNullOrWhiteSpace(userIdStr))
                throw new UnauthorizedAccessException("User ID claim not found in token.");
            return Guid.Parse(userIdStr);
        }
    }
}