using System.Threading.Tasks;

namespace eHospital.Authorization.Service
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}