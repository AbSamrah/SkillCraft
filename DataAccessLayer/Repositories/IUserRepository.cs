using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllAsync();
        public Task<User> GetAsync(Guid id);
        public Task<User> UpdateAsync(Guid id, User user);
        public Task<User> DeleteAsync(Guid id);
        public Task<User> AddAsync(User user);
    }
}
