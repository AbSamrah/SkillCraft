using BuissnessLogicLayer.Models;
using DataAccessLayer.Auth;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = BuissnessLogicLayer.Models.UserProfileDto;

namespace BuissnessLogicLayer.Services
{
    public class AuthService
    {
        private IUserRepository _userRepository;
        private IPasswordHasher _passwordHasher;
        private ITokenService _tokenService;
        private IRoleRepository _roleRepository;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _roleRepository = roleRepository;
        }

        public async Task<string> SignUpAsync(UserSignUp userSignUp)
        {
            if(await _userRepository.UserExistsAsync(userSignUp.Email))
            {
                throw new Exception("Email already exists.");
            }

            DataAccessLayer.Models.User user = new DataAccessLayer.Models.User();

            user.FirstName = userSignUp.FirstName;
            user.LastName = userSignUp.LastName;
            user.Email = userSignUp.Email;
            user.PasswordHash = _passwordHasher.Hash(userSignUp.Password);

            var role = await _roleRepository.GetByTitleAsync("User");
            user.Role = role;
            user.RoleId = role.Id;

            await _userRepository.AddAsync(user);

            var token = await _tokenService.GenerateToken(user);

            return token;
        }

        public async Task<string> LoginAsync(UserLogin userLogin)
        {
            if (!await _userRepository.UserExistsAsync(userLogin.Email))
            {
                throw new Exception("User not found.");
            }

            DataAccessLayer.Models.User user = await _userRepository.GetByEmailAsync(userLogin.Email);
            if (_passwordHasher.Verify(userLogin.Password, user.PasswordHash))
            {
                var token = await _tokenService.GenerateToken(user);
                return token;
            }
            else
            {
                throw new Exception("Password is incorrect.");
            }
        }
    }
}
