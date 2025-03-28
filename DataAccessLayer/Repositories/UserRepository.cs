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

        public UserRepository(UsersDbContext usersDbContext) {
            _usersDbContext = usersDbContext;
        }
        public async Task<User> AddAsync(User user)
        {
            user.Id = Guid.NewGuid();
            await _usersDbContext.AddAsync(user);
            await _usersDbContext.SaveChangesAsync();
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

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _usersDbContext.Users.ToListAsync();
        }

        public async Task<User> GetAsync(Guid id)
        {
            return await _usersDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> UpdateAsync(Guid id, User user)
        {
            var existingUser = await _usersDbContext.Users.FirstOrDefaultAsync(x=>x.Id == id);

            if (existingUser is null)
            {
                return null;
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PasswordHash = user.PasswordHash;

            await _usersDbContext.SaveChangesAsync();

            return existingUser;

        }
    }
}
