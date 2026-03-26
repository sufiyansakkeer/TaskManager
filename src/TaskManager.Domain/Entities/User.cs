using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }


        public string Email { get; private set; } = string.Empty;


        public string PasswordHash { get; private set; } = string.Empty;

        public string FullName { get; private set; } = string.Empty;

        public DateTime CreatedOn { get; private set; } = DateTime.UtcNow;

        public string Role { get; private set; } = "User";

        private User()
        {

        }
        public User(string email, string passwordHash, string fullName)
        {
            Id = Guid.NewGuid();
            Email = email;
            PasswordHash = passwordHash;
            FullName = fullName;
            CreatedOn = DateTime.UtcNow;
        }
    }
}