using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace Engine.Notifications;

public class SmtpEmailSender : IEmailSender
{
    private readonly ILogger<SmtpEmailSender> _logger;
    private readonly EmailOptions _options;

    public SmtpEmailSender(EmailOptions options, ILogger<SmtpEmailSender> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task SendAsync(EmailMessage message, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_options.SenderAddress))
            throw new InvalidOperationException("Email sender address is not configured");

        using var smtpClient = BuildClient();
        using var mail = BuildMailMessage(message);

        cancellationToken.ThrowIfCancellationRequested();

        try
        {
            await smtpClient.SendMailAsync(mail);
            _logger.LogDebug("Email sent to {Recipient}", message.Recipient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}", message.Recipient);
            throw;
        }
    }

    private SmtpClient BuildClient()
    {
        var client = new SmtpClient(_options.Host, _options.Port)
        {
            EnableSsl = _options.EnableSsl
        };

        if (!string.IsNullOrWhiteSpace(_options.Username))
            client.Credentials = new NetworkCredential(_options.Username, _options.Password);

        return client;
    }

    private MailMessage BuildMailMessage(EmailMessage message)
    {
        var mail = new MailMessage
        {
            Subject = message.Subject,
            Body = message.Body,
            IsBodyHtml = message.IsHtml,
            From = string.IsNullOrWhiteSpace(_options.SenderDisplayName)
                ? new MailAddress(_options.SenderAddress)
                : new MailAddress(_options.SenderAddress, _options.SenderDisplayName)
        };

        mail.To.Add(message.Recipient);

        if (!string.IsNullOrWhiteSpace(message.ReplyTo)) mail.ReplyToList.Add(new MailAddress(message.ReplyTo));

        return mail;
    }
}