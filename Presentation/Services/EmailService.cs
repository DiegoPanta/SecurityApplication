using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Presentation.Interfaces;

namespace Presentation.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;

        public EmailService(IConfiguration configuration)
        {
            var emailSettings = configuration.GetSection("EmailSettings");

            _smtpServer = emailSettings["SmtpServer"] ?? throw new InvalidOperationException("SMTP Server is missing.");
            _smtpPort = int.Parse(emailSettings["SmtpPort"] ?? "587");
            _smtpUser = emailSettings["SmtpUser"] ?? throw new InvalidOperationException("SMTP User is missing.");
            _smtpPassword = emailSettings["SmtpPassword"] ?? throw new InvalidOperationException("SMTP Password is missing.");
        }

        public async Task<bool> SendEmailAsync(string email, string token)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("SecurityTeste", _smtpUser));
                message.To.Add(new MailboxAddress("User", email));
                message.Subject = "Your Login Link";
                message.Body = new TextPart("plain")
                {
                    Text = $"Click here to login: http://localhost:5183/verify-login?token={token}"
                };

                using var client = new SmtpClient();

                //Ignore SSL certificate validation errors
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                //Connect securely with STARTTLS
                await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);

                //Authenticate using the App Password
                await client.AuthenticateAsync(new NetworkCredential(_smtpUser, _smtpPassword));

                //Send the email
                await client.SendAsync(message);

                //Disconnect cleanly
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
