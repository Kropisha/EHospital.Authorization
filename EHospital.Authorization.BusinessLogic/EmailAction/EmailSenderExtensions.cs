using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace EHospital.Authorization.BusinessLogic.EmailAction
{
    /// <summary>
    /// For main letters
    /// </summary>
    public static class EmailSenderExtensions
    {
        /// <summary>
        /// Send confirmation link
        /// </summary>
        /// <param name="emailSender">an instance for interface</param>
        /// <param name="email">user's email</param>
        /// <param name="link">confirmation link</param>
        /// <returns>completed task</returns>
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(link)}'>clicking here</a>.");
        }

        /// <summary>
        /// Send reset password link
        /// </summary>
        /// <param name="emailSender">an instance for interface</param>
        /// <param name="email">user's email</param>
        /// <param name="callbackUrl">resetting link</param>
        /// <returns>completed task</returns>
        public static Task SendResetPasswordAsync(this IEmailSender emailSender, string email, string callbackUrl)
        {
            return emailSender.SendEmailAsync(
                email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
    }
}
