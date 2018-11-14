using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eHospital.Authorization.Service
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
