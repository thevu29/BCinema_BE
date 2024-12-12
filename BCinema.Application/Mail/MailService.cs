using System.Net;
using System.Net.Mail;
using System.Text;
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
            
            var mailMessage = new MailMessage
            {
                From = new MailAddress(email!, "BCinema"),
                Subject = mailData.EmailSubject,
                Body = mailData.EmailBody,
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8
            };
            mailMessage.To.Add(mailData.EmailToId);
        
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