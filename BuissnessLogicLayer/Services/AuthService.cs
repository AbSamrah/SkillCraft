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
        private IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> SignUpAsync(UserSignUp userSignUp)
        {
            if(await _userRepository.UserExistsAsync(userSignUp.Email))
            {
                throw new KeyNotFoundException("Email already exists.");
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

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> LoginAsync(UserLogin userLogin)
        {
            if (!await _userRepository.UserExistsAsync(userLogin.Email))
            {
                throw new KeyNotFoundException("User not found.");
            }

            User user = await _userRepository.GetByEmailAsync(userLogin.Email);
            if (_passwordHasher.Verify(userLogin.Password, user.PasswordHash))
            {
                return _mapper.Map<UserDto>(user);
            }
            else
            {
                throw new ArgumentException("Password is incorrect.");
            }
        }

        public async Task<UserDto> ChangePasswordAsync(ChangePasswordRequest request)
        {
            var user = await _userRepository.GetAsync(request.Id);
            if(user is null)
            {
                throw new KeyNotFoundException("User not found.");
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
                throw new ArgumentException("Password is incorrect.");
            }

        }
    }
}
