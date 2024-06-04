using eCommerceOrders.SendEmail.Services.Interfaces;
using System.Net.Mail;
using System.Net;

namespace eCommerceOrders.SendEmail.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService()
        {
            _smtpServer = "smtp.gmail.com";
            _smtpPort = 587; // (para TLS) ou 465 (para SSL)
            _smtpUser = ""; // e-mail Gmail
            _smtpPass = ""; // senha app Gmail
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = false)
        {
            using (var client = new SmtpClient(_smtpServer, _smtpPort))
            {
                client.Credentials = new NetworkCredential(_smtpUser, _smtpPass);
                client.EnableSsl = true; // Segurança

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUser),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isBodyHtml
                };
                mailMessage.To.Add(to);

                try
                {
                    await client.SendMailAsync(mailMessage);
                    Console.WriteLine("Email enviado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao enviar email: {ex.Message}");
                    throw;
                }
            }
        }
    }
}