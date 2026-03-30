using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}