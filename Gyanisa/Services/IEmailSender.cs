using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gyanisa.Models;

namespace Gyanisa.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendCustomEmailAsync(string email, string subject, string message);
        string ConfirmEmailRegistrationAsync(ApplicationUser user, string callbackUrl, string subject, string email);
        IEnumerable<string> GetClientIP(string localIp);
        void SendMail(string email, string subject, string messagebody);
    }
}
