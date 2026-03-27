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
            if (string.IsNullOrWhiteSpace(_settings.Key))
                throw new InvalidOperationException("JWT signing key is not configured. Set Jwt:Key in configuration.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.Key);

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
                Issuer = _settings.Issuer,
                Audience = _settings.Audience,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
