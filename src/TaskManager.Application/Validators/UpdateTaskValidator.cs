using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using TaskManager.Application.DTOs.TaskItem;

namespace TaskManager.Application.Validators
{
    public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Description).MaximumLength(1000);

        }
    }
}