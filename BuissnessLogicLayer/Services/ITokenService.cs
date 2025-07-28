using BuissnessLogicLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuissnessLogicLayer.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(UserDto user);
    }
}
