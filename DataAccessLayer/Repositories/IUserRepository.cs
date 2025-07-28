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
        public Task<List<User>> GetAllAsync(string? email=null, string? firstName=null, string? lastName=null, int pageNumber = 0, int pageSize = 10);
        public Task<User> GetAsync(Guid id);
        public Task<User> GetByEmailAsync(string email);
        public Task<User> UpdateAsync(User user);
        public Task<User> DeleteAsync(Guid id);
        public Task AddAsync(User user);

        public Task<bool> UserExistsAsync(String Email);
    }
}
