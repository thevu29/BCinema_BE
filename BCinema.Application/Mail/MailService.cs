using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BCinema.Application.Mail;

public class MailService(
    IConfiguration configuration,
    ILogger<MailService> logger) : IMailService
{
    public async Task<bool> SendMailAsync(MailData mailData, CancellationToken cancellationToken)
    {
        try
        {
            var mailSettings = configuration.GetSection("MailSettings");
            var email = mailSettings["Email"];
            var password = mailSettings["Password"];
            var host = mailSettings["Host"];
            var port = int.Parse(mailSettings["Port"]!);
            
            var smtpClient = new SmtpClient(host, port);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.EnableSsl = true;
            
            smtpClient.Credentials = new NetworkCredential(email, password);
            var mailMessage = new MailMessage(email!, mailData.EmailToId, mailData.EmailSubject, mailData.EmailBody);
            await smtpClient.SendMailAsync(mailMessage, cancellationToken);
            return true;
        }
        catch(Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return false;
        }
    }
}