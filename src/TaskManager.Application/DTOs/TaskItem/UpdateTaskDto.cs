using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Application.DTOs.TaskItem
{
    public class UpdateTaskDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}