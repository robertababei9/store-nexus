

namespace Infrastructure.Email
{
    public interface IMailService
    {
        bool SendMail(MailData mailData);
    }
}
