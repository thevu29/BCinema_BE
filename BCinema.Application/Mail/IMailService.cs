namespace BCinema.Application.Mail;

public interface IMailService
{
    Task<bool> SendMail(MailData mailData);
}