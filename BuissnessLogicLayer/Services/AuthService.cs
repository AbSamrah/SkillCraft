using AutoMapper;
using BuissnessLogicLayer.Models;
using DataAccessLayer.Auth;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.BuissnessLogicLayer.Services;
using UsersManagement.DataAccessLayer.Models;
using UsersManagement.DataAccessLayer.Repositories;

namespace BuissnessLogicLayer.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPendingUserRepository _pendinguserRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AuthService(IUserRepository userRepository, IPendingUserRepository pendingUserRepository, IPasswordHasher passwordHasher, IRoleRepository roleRepository, IMapper mapper, IEmailService emailService)
        {
            _userRepository = userRepository;
            _pendinguserRepository = pendingUserRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task SignUpAsync(UserSignUp userSignUp)
        {
            if(await _userRepository.UserExistsAsync(userSignUp.Email))
            {
                throw new KeyNotFoundException("Email already exists.");
            }

            var existingPendingUser = await _pendinguserRepository.GetPendingUserAsync(userSignUp.Email);
            if (existingPendingUser != null)
            {
                await _pendinguserRepository.DeletePendingUser(existingPendingUser);
            }

            var pendingUser = new PendingUser
            {
                FirstName = userSignUp.FirstName,
                LastName = userSignUp.LastName,
                Email = userSignUp.Email,
                PasswordHash = _passwordHasher.Hash(userSignUp.Password),
                VerificationToken = Convert.ToHexString(RandomNumberGenerator.GetBytes(64)),
                ExpirationDate = DateTime.UtcNow.AddHours(24) 
            };

            await _pendinguserRepository.AddPendingUserAsync(pendingUser);

            await _emailService.SendVerificationEmailAsync(pendingUser.Email, pendingUser.VerificationToken);
        }

        public async Task<UserDto> LoginAsync(UserLogin userLogin)
        {
            var isPending = await _pendinguserRepository.GetPendingUserAsync(userLogin.Email);
            if (isPending is not null)
            {
                throw new Exception("Email is pending verification. Please check your inbox.");
            }
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

        public async Task<UserDto> VerifyEmailAsync(string email, string token)
        {
            var pendingUser = await _pendinguserRepository.GetPendingUserAsync(email, token);

            if (pendingUser == null)
            {
                throw new Exception("Invalid verification token.");
            }

            if (pendingUser.ExpirationDate < DateTime.UtcNow)
            {
                await _pendinguserRepository.DeletePendingUser(pendingUser);
                throw new Exception("Verification token has expired.");
            }

            var role = await _roleRepository.GetByTitleAsync("User");
            var user = new User
            {
                FirstName = pendingUser.FirstName,
                LastName = pendingUser.LastName,
                Email = pendingUser.Email,
                PasswordHash = pendingUser.PasswordHash,
                Role = role,
                RoleId = role.Id
            };

            await _userRepository.AddAsync(user);

            await _pendinguserRepository.DeletePendingUser(pendingUser);

            return _mapper.Map<UserDto>(user);
        }
    }
}
