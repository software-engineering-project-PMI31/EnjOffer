using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace EnjOffer.UI.EmailConfiguration
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Website administration", "admin@metanit.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
            };
             
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration.GetValue<string>("EmailConfiguration:SmtpServer"),
                    _configuration.GetValue<int>("EmailConfiguration:Port"), true);
                await client.AuthenticateAsync(_configuration.GetValue<string>("EmailConfiguration:Username"),
                    _configuration.GetValue<string>("EmailConfiguration:Password"));
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
