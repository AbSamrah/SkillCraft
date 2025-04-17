using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Auth
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
    }
}
