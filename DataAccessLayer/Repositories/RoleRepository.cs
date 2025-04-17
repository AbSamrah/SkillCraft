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
    public class RoleRepository : IRoleRepository
    {
        private readonly UsersDbContext _usersDbContext;

        public RoleRepository(UsersDbContext usersDbContext)
        {
            _usersDbContext = usersDbContext;
        }
        public async Task<Role> DeleteAsync(Guid id)
        {
            var role = await _usersDbContext.Roles.FirstOrDefaultAsync(x => x.Id == id);

            if (role is null)
            {
                return null;
            }

            _usersDbContext.Remove(role);
            await _usersDbContext.SaveChangesAsync();

            return role;
        }

        public async Task<List<Role>> GetAllAsnc()
        {
            return await _usersDbContext.Roles.ToListAsync();
        }

        public async Task<Role> GetAsync(Guid id)
        {
            return await _usersDbContext.Roles.FirstAsync(x => x.Id == id);
        }

        public async Task<Role> UpdateAsync(Guid id, Role role)
        {
            var existingRole = await _usersDbContext.Roles.FirstOrDefaultAsync(x=>x.Id == role.Id);

            if (existingRole is null)
            {
                return null;
            }

            existingRole.Title = role.Title;

            return existingRole;
        }

        public async Task<Role> AddAsync(Role role)
        {
            role.Id = Guid.NewGuid();

            await _usersDbContext.AddAsync(role);
            await _usersDbContext.SaveChangesAsync();

            return role;
        }

        public async Task<Role> GetByTitleAsync(string title)
        {
            return await _usersDbContext.Roles.FirstOrDefaultAsync(x => x.Title == title);
        }
    }
}
