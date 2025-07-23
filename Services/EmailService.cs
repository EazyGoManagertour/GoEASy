using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GoEASy.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendContactConfirmationAsync(string toEmail, string userName)
        {
            var fromEmail = _config["EmailSettings:From"];
            var password = _config["EmailSettings:Password"];

            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail, "GoEASy Team"),
                Subject = "Xác nhận liên hệ từ GoEASy",
                Body = $"Xin chào {userName},\n\nChúng tôi đã nhận được liên hệ của bạn và sẽ phản hồi sớm nhất.",
                IsBodyHtml = false
            };
            mail.To.Add(toEmail);

            using var smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            await smtp.SendMailAsync(mail);
        }
    }
}
