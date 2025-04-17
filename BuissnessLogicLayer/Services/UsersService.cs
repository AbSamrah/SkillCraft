using AutoMapper;
using BuissnessLogicLayer.Models;
using DataAccessLayer.Auth;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLogicLayer.Services
{
    public class UsersService
    {
        private IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private IRoleRepository _roleRepository;
        private IMapper _mapper;

        public UsersService(IUserRepository userRepository, IPasswordHasher passwordHasher, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<AddUserRequest> AddAsync(AddUserRequest addUserRequest)
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

            addUserRequest.Password = null;
            return addUserRequest;
        }

        public async Task<UserProfileDto> GetByIdAsync(Guid id)
        {
            var user = await _userRepository.GetAsync(id);
            if(user is null)
            {
                return null;
            }
            var userDto = _mapper.Map<UserProfileDto>(user);
            return userDto;
        }

        public async Task<List<UserProfileDto>> GetAllAsync(string email, string firstName, string lastName)
        {
            var users = await _userRepository.GetAllAsync(email,firstName,lastName);
            return _mapper.Map<List<UserProfileDto>>(users);
        }

        public async Task<UserDto> UpdateAsync(UserDto userDto)
        {
            var user = await _userRepository.GetAsync(userDto.Id);
            if( user is null)
            {
                return null;
            }
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.PasswordHash = _passwordHasher.Hash(userDto.Password);
            user = await _userRepository.UpdateAsync(user);
            if (user is null)
            {
                throw new Exception("Some thing went wrong.");
            }
            userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserProfileDto> DeleteAsync(Guid id)
        {
            var user = await _userRepository.DeleteAsync(id);
            if (user is null)
            {
                return null;
            }
            return _mapper.Map<UserProfileDto>(user);
        }

        public async Task<UserProfileDto> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user is null)
            {
                return null;
            }
            return _mapper.Map<UserProfileDto>(user);
        }
    }
}
