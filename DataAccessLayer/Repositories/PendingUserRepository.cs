using DataAccessLayer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.DataAccessLayer.Models;

namespace UsersManagement.DataAccessLayer.Repositories
{
    public class PendingUserRepository : IPendingUserRepository
    {
        private readonly UsersDbContext _usersDbContext;

        public PendingUserRepository(UsersDbContext usersDbContext)
        {
            _usersDbContext = usersDbContext;
        }

        public async Task<PendingUser> GetPendingUserAsync(string email, string token = "")
        {
            PendingUser? user = new PendingUser();
            if (string.IsNullOrEmpty(token)) { 
                user = await _usersDbContext.PendingUsers.FirstOrDefaultAsync(p => p.Email == email);
            }
            else
            {
                user = await _usersDbContext.PendingUsers.FirstOrDefaultAsync(p => p.Email == email && p.VerificationToken == token);
            }
            return user;
        }

        public async Task DeletePendingUser(PendingUser user)
        {
            _usersDbContext.PendingUsers.Remove(user);
            await _usersDbContext.SaveChangesAsync();
        }

        public async Task AddPendingUserAsync(PendingUser pendingUser)
        {
            await _usersDbContext.PendingUsers.AddAsync(pendingUser);
            await _usersDbContext.SaveChangesAsync();
        }
    }
}
