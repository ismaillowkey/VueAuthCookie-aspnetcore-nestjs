using CookieAuthJwtRefresh.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace CookieAuthJwtRefresh.Services.EmailSenderService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(MessageEmail message)
        {
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress(_emailConfig.From, _emailConfig.From));
            msg.To.Add(new MailboxAddress(message.To, message.To));
            msg.Subject = message.Subject;
            msg.Body = new TextPart("html") { Text = message.BodyHtml };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, false);

                    // Note: only needed if the SMTP server requires authentication
                    //client.Authenticate("joey", "password");

                    client.Send(msg);
                }
                catch (Exception ex)
                {
                    //log an error message or throw an exception or both.
                    throw new Exception("Error when send message", ex);
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
