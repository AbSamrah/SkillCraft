using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync(string? email=null, string? firstName=null, string? lastName=null, int pageNumber = 0, int pageSize = 10);
        Task<User> GetAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        Task<User> UpdateAsync(User user);
        Task<User> DeleteAsync(Guid id);
        Task AddAsync(User user);
        Task<bool> UserExistsAsync(string Email);
        
    }
}
