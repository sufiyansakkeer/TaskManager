using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Auth;

namespace TaskManager.Application.Interfaces.Service
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

    }
}