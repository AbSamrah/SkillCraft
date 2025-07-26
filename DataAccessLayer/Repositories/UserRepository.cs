using DataAccessLayer.Auth;
using DataAccessLayer.Data;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UsersDbContext _usersDbContext;
        private readonly IPasswordHasher _passwordHasher;

        public UserRepository(UsersDbContext usersDbContext, IPasswordHasher passwordHasher) {
            _usersDbContext = usersDbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> AddAsync(User user) 
        {
            user.Id = Guid.NewGuid();
            await _usersDbContext.AddAsync(user);
            await _usersDbContext.SaveChangesAsync();
            user.PasswordHash = null;
            return user;
        }

        public async Task<User> DeleteAsync(Guid id)
        {
            var user = await _usersDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user is null)
            {
                return null;
            }

            _usersDbContext.Users.Remove(user);
            await _usersDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllAsync(string? email = null, string? firstName = null, string? lastName = null, int pageNumber = 0, int pageSize = 10)
        {
            var query = _usersDbContext.Users.OrderBy(u => u.FirstName)
                .ThenBy(u => u.LastName)
                .AsQueryable();
            if (!string.IsNullOrWhiteSpace(email))
            {
                query = query.Where(u => u.Email.Contains(email));
            }
            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query = query.Where(u => u.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query = query.Where(u => u.LastName.Contains(lastName));
            }

            query = query
                .Skip((pageNumber) * pageSize)
                .Take(pageSize)
                .Include(u => u.Role);

            return await query.ToListAsync();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _usersDbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _usersDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user is null)
            {
                return null;
            }
            user.Role = await _usersDbContext.Roles.FirstOrDefaultAsync(x => x.Id == user.RoleId);
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            var existingUser = await _usersDbContext.Users.FirstOrDefaultAsync(x=>x.Id == user.Id);

            if (existingUser is null)
            {
                return null;
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;
            existingUser.RoleId = user.RoleId;
            await _usersDbContext.SaveChangesAsync();

            existingUser.PasswordHash = null;
            return existingUser;

        }

        public async Task<bool> UserExistsAsync(string email)
        {
            var user = await _usersDbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user is null)
            {
                return false;
            }
            return true;
        }
    }
}
