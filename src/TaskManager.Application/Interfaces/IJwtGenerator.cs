using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(User user);
    }
}