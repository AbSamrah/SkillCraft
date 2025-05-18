using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface IRoleRepository
    {
        public Task<List<Role>> GetAllAsync();
        public Task<Role> GetAsync(Guid id);
        public Task<Role> GetByTitleAsync(string title);
        public Task<Role> DeleteAsync(Guid id);
        public Task<Role> AddAsync(Role role);
        public Task<Role> UpdateAsync(Guid id, Role role);

    }
}
