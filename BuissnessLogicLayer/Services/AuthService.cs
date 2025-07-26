using AutoMapper;
using BuissnessLogicLayer.Models;
using DataAccessLayer.Auth;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLogicLayer.Services
{
    public class AuthService
    {
        private IUserRepository _userRepository;
        private IPasswordHasher _passwordHasher;
        private ITokenService _tokenService;
        private IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher,
            ITokenService tokenService, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<string> SignUpAsync(UserSignUp userSignUp)
        {
            if(await _userRepository.UserExistsAsync(userSignUp.Email))
            {
                throw new Exception("Email already exists.");
            }

            User user = new User();

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

            User user = await _userRepository.GetByEmailAsync(userLogin.Email);
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

        public async Task<UserDto> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _userRepository.GetAsync(request.Id);
            if(user is null)
            {
                throw new Exception("User not found.");
            }

            if (_passwordHasher.Verify(request.OldPassword, user.PasswordHash))
            {
                user.PasswordHash = _passwordHasher.Hash(request.NewPassword);
                user = await _userRepository.UpdateAsync(user);

                UserDto userDto = _mapper.Map<UserDto>(user);
                return userDto;
            }
            else
            {
                throw new Exception("Wrong Password.");
            }

        }
    }
}
