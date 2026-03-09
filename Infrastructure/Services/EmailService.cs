using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

public class EmailService(IOptions<EmailSetting> settings ) : IEmailService
{
    private readonly EmailSetting _settings= settings.Value;

    public async Task SendAsync(string to, string subject, string body)
    {
        var message = new MailMessage();
        message.To.Add(to);
        message.Subject = subject;
        message.Body = body;
        message.From = new MailAddress(_settings.Email);

        using var client = new SmtpClient(_settings.Host, _settings.Port)
        {
            Credentials = new NetworkCredential(
                _settings.Email,
                _settings.Password),
            EnableSsl = true
        };
        await client.SendMailAsync(message);
    }
}