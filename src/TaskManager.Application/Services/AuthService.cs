using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.DTOs.Auth;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Interfaces.Service;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        private readonly IJwtGenerator _jwtGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtGenerator jwtGenerator, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _jwtGenerator = jwtGenerator;
            _passwordHasher = passwordHasher;
        }
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Invalid credentials");

            var isValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

            if (!isValid)
                throw new Exception("Invalid credentials");

            var token = _jwtGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Email = user.Email,
                Token = token,
            };


        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var existingEmail = await _userRepository.IsEmailAlreadyExistAsync(request.Email);

            if (existingEmail)
                throw new Exception("User already exist");

            var hashedPassword = _passwordHasher.Hash(request.Password);

            var user = new User(request.Email, hashedPassword, request.FullName);

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var token = _jwtGenerator.GenerateToken(user);

            return new AuthResponseDto
            {
                Email = user.Email,

                Token = token
            };



        }
    }
}