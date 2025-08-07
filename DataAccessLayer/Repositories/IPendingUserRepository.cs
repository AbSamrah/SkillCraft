using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersManagement.DataAccessLayer.Models;

namespace UsersManagement.DataAccessLayer.Repositories
{
    public interface IPendingUserRepository
    {
        Task AddPendingUserAsync(PendingUser pendingUser);
        Task<PendingUser> GetPendingUserAsync(string email, string token="");
        Task DeletePendingUser(PendingUser user);
    }
}
