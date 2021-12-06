using CookieAuthJwtRefresh.Models;
using MimeKit;

namespace CookieAuthJwtRefresh.Services.EmailSenderService
{
    public interface IEmailSender
    {
        void SendEmail(MessageEmail message);
    }
}
