namespace BCinema.Application.Mail;

public interface IMailService
{
    Task<bool> SendMailAsync(MailData mailData, CancellationToken cancellationToken);
}