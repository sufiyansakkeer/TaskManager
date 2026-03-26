using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Application.Common.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
    }
}