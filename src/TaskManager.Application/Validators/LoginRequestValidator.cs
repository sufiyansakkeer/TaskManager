using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using TaskManager.Application.DTOs.Auth;

namespace TaskManager.Application.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequestDto>
    {

        public LoginRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty().EmailAddress();

            RuleFor(x => x.Password).NotEmpty();

        }
    }
}