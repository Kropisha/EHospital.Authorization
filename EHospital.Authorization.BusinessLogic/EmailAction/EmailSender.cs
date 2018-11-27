namespace EHospital.Authorization.BusinessLogic
{
    using System.Threading.Tasks;

    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // TODO: write task for sending email
            return Task.CompletedTask;
        }
    }
}
