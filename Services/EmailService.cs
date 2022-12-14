using Microsoft.AspNetCore.Identity.UI.Services;
using Azure;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using Microsoft.Extensions.Configuration;

namespace DoctorSystem.Services
{
    public class EmailService : IEmailSender
    {
        private readonly string _connectionString;
        private readonly string _sender;
        public EmailService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Email");
            _sender = configuration["EmailAddress"];
        }
        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            EmailClient emailClient = new(_connectionString);

            EmailContent emailContent = new(subject)
            {
                PlainText = htmlMessage,
                Html = htmlMessage
            };

            EmailRecipients emailRecipients = new(new List<EmailAddress> { new EmailAddress(email) });
            EmailMessage emailMessage = new(_sender, emailContent, emailRecipients);
            SendEmailResult emailResult = emailClient.Send(emailMessage, CancellationToken.None);

            return Task.FromResult(emailResult);
        }
    }
}
