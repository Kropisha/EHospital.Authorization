using System.Threading.Tasks;

namespace EHospital.Authorization.BusinessLogic.EmailAction
{
    /// <summary>
    /// Class for verify by email
    /// </summary>
    public class EmailSender : IEmailSender
    {
        /// <summary>
        /// Send email to user
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="subject">topic of letter</param>
        /// <param name="message">link</param>
        /// <returns>response code</returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // TODO: write task for sending email
            return Task.CompletedTask;
        }
    }
}
