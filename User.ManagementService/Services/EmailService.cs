using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using User.ManagementService.Models;

namespace User.ManagementService.Services;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfiguration;

    public EmailService(EmailConfiguration emailConfiguration)
    {
        _emailConfiguration = emailConfiguration;
    }

    public void SentEmail(Message message)
    {
        var emailMessage = CreateEmailMessage(message);
        Send(emailMessage);
    }

    private MimeMessage CreateEmailMessage(Message message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress("Warhammer Tournaments", _emailConfiguration.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        emailMessage.Body = new TextPart(TextFormat.Text) { Text = message.Content };

        return emailMessage;
    }

    private void Send(MimeMessage emailMessage)
    {
        using var client = new SmtpClient();

        try
        {
            client.Connect(_emailConfiguration.SmptServer, _emailConfiguration.Port, true);
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);

            client.Send(emailMessage);
        }
        catch (Exception e)
        {
            // Throw exception or report to log
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            client.Disconnect(true);
            client.Dispose();
        }
    }
}