using ProfilesManagement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProfilesManagement.DataAccessLayer.Interfaces
{
    public interface IProfileRepository: IDisposable
    {
        Task Add(UserProfile profile);
        Task<UserProfile> GetById(string id);
        Task<List<UserProfile>> GetAll();
        Task Update(UserProfile profile);
        Task Remove(string id);
    }
}
