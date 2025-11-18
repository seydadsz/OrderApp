using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace OrderApp.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var settings = _config.GetSection("EmailSettings");

            string host = settings["Host"] ?? "smtp.gmail.com";
            int port = int.TryParse(settings["Port"], out var p) ? p : 587;
            bool enableSSL = bool.TryParse(settings["EnableSSL"], out var ssl) ? ssl : true;

            string username = settings["UserName"]
                ?? throw new Exception("Email UserName is missing!");
            string password = settings["Password"]
                ?? throw new Exception("Email Password is missing!");

            if (string.IsNullOrWhiteSpace(to))
                throw new Exception("Receiver email cannot be null.");

            using var smtp = new SmtpClient(host)
            {
                Port = port,
                EnableSsl = enableSSL,
                Credentials = new NetworkCredential(username, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            using var mail = new MailMessage
            {
                From = new MailAddress(username),
                Subject = subject ?? string.Empty,
                Body = body ?? string.Empty,
                IsBodyHtml = true
            };

            mail.To.Add(to);

            await smtp.SendMailAsync(mail);
        }
    }
}
