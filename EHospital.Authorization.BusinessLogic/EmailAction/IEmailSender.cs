using System.Threading.Tasks;

namespace EHospital.Authorization.BusinessLogic.EmailAction
{
    /// <summary>
    /// An interface for sending emails
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Send email to user
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="subject">topic of letter</param>
        /// <param name="message">link</param>
        /// <returns>response code</returns>
        Task SendEmailAsync(string email, string subject, string message);
    }
}