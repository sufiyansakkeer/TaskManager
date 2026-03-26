using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Application.Common.Settings;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Security
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtSettings _settings;

        public JwtGenerator(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }
        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.Secret);

            var claims = new List<Claim>
          {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email,user.Email),


          };

            var credentials = new SigningCredentials(
              new SymmetricSecurityKey(key),
              SecurityAlgorithms.HmacSha256

            );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}