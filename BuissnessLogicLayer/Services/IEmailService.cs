using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersManagement.BuissnessLogicLayer.Services
{
    public interface IEmailService
    {
        Task SendVerificationEmailAsync(string email, string verificationToken);
    }
}
