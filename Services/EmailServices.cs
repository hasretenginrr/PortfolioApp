using MailKit.Net.Smtp;  // SmtpClient için
using MailKit.Security; // SecureSocketOptions için
using Microsoft.Extensions.Configuration;
using MimeKit;          // MimeMessage, MailboxAddress, BodyBuilder için
using System.Threading.Tasks;

namespace PortfolioBackend.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendContactEmail(string fromName, string fromEmail, string subject, string messageBody)
        {
            // --- 1. AYARLARI ÇEK ---
            string toEmail = _config.GetValue<string>("EmailSettings:ToAddress");
            string toName = "Hasret Nisa Enginer";

            // *** YENİ ***
            // Maili gönderecek olan, kimliği doğrulanmış (authenticated) hesap
            string smtpUserEmail = _config.GetValue<string>("EmailSettings:SmtpUser");

            var email = new MimeMessage();

            // --- 2. MAİL BAŞLIKLARINI DÜZELT ---

            // KİMDEN (FROM): Mail, sizin GÜVENLİ GÖNDERİCİ hesabınızdan gelmeli.
            // Ziyaretçinin adı (fromName) görünebilir, ancak email adresi (smtpUserEmail) SİZİN olmalı.
            email.From.Add(new MailboxAddress(fromName, smtpUserEmail));

            // KİME (TO): Mail size gelmeli (toEmail).
            email.To.Add(new MailboxAddress(toName, toEmail));

            // *** YENİ ***
            // YANITLA (REPLY-TO): Gelen maile "Yanıtla" dediğinizde,
            // alıcı olarak ziyaretçinin adresi (fromEmail) otomatik eklenmeli.
            email.ReplyTo.Add(new MailboxAddress(fromName, fromEmail));

            email.Subject = $"Portfolyo İletişim Formu: {subject}";

            // --- 3. MAİL İÇERİĞİ (Aynı kaldı) ---
            var body = new BodyBuilder
            {
                HtmlBody = $@"
                        <h3>Portfolyo sitenizden yeni bir mesajınız var:</h3>
                        <p><strong>Gönderen:</strong> {fromName} ({fromEmail})</p>
                        <p><strong>Konu:</strong> {subject}</p>
                        <hr>
                        <p>{messageBody.Replace("\n", "<br>")}</p>"
            };
            email.Body = body.ToMessageBody();

            // --- 4. GÖNDERİM (Aynı kaldı) ---
            using var smtp = new SmtpClient();

            string host = _config.GetValue<string>("EmailSettings:SmtpHost");
            int port = _config.GetValue<int>("EmailSettings:SmtpPort");
            string user = _config.GetValue<string>("EmailSettings:SmtpUser");
            string pass = _config.GetValue<string>("EmailSettings:SmtpPass");

            await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(user, pass);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
