using AutoMapper;
using BuissnessLogicLayer.Filters;
using BuissnessLogicLayer.Models;
using DataAccessLayer.Auth;
using DataAccessLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLogicLayer.Services
{
    public class UsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public UsersService(IUserRepository userRepository, IPasswordHasher passwordHasher, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<UserDto> AddAsync(AddUserRequest addUserRequest)
        {
            if (await _userRepository.UserExistsAsync(addUserRequest.Email))
            {
                throw new Exception("Email already exists.");
            }
            DataAccessLayer.Models.User user = new DataAccessLayer.Models.User();
            user.LastName = addUserRequest.LastName;
            user.FirstName = addUserRequest.FirstName;
            user.Email = addUserRequest.Email;
            user.PasswordHash = _passwordHasher.Hash(addUserRequest.Password);
            var role = await _roleRepository.GetByTitleAsync(addUserRequest.Role);
            if (role is null)
            {
                throw new Exception("There is no role with this name.");
            }
            user.Role = role;
            user.RoleId = role.Id;
            await _userRepository.AddAsync(user);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if(user is null)
            {
                return null;
            }
            user.Role = await _roleRepository.GetAsync(user.RoleId);
            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<List<UserDto>> GetAllAsync(UserFilter userFilter)
        {
            
            var users = await _userRepository.GetAllAsync(userFilter.Email, userFilter.FirstName, userFilter.LastName, userFilter.PageNumber, userFilter.PageSize);
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<(UserDto user, ProfileAction profileAction)> UpdateAsync(Guid id, UpdateUserRequest updateUserRequest)
        {
            var user = await _userRepository.GetAsync(id);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var profileAction = ProfileAction.NONE;
            var currentRole = user.Role.Title;
            var newRoleTitle = updateUserRequest.Role;

            if (currentRole != newRoleTitle)
            {
                var newRole = await _roleRepository.GetByTitleAsync(newRoleTitle);
                if (newRole == null)
                {
                    throw new Exception($"Role '{newRoleTitle}' not found.");
                }
                user.Role = newRole;
                user.RoleId = newRole.Id;

                if (currentRole == "User" && newRoleTitle != "User")
                {
                    profileAction = ProfileAction.DELETE_PROFILE;
                }
                else if (currentRole != "User" && newRoleTitle == "User")
                {
                    profileAction = ProfileAction.CREATE_PROFILE;
                }
            }

            user.FirstName = updateUserRequest.FirstName;
            user.LastName = updateUserRequest.LastName;


            var updatedUser = await _userRepository.UpdateAsync(user);
            var userDto = _mapper.Map<UserDto>(updatedUser);
            return (userDto, profileAction);
        }

        public async Task<UpdateUserRequest> DeleteAsync(Guid id)
        {
            var user = await _userRepository.DeleteAsync(id);
            if (user is null)
            {
                return null;
            }
            return _mapper.Map<UpdateUserRequest>(user);
        }

        public async Task<UserDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user is null)
            {
                return null;
            }
            return _mapper.Map<UserDto>(user);
        }
    }
}
