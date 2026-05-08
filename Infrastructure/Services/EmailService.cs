using System;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class EmailService(IOptions<EmailSetting> settings, ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailSetting _settings= settings.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendAsync(string to, string subject, string body)
    {
        if (string.IsNullOrWhiteSpace(_settings.Host))
        {
            throw new InvalidOperationException("EmailSettings:Host is not configured.");
        }

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

        try
        {
            await client.SendMailAsync(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to send email to {Recipient} using SMTP host '{Host}' and port {Port}.",
                to,
                _settings.Host,
                _settings.Port);
            throw;
        }
    }
}
