using Shifter.Application.Interfaces.Services.Emails;

namespace Shifter.Infrastructure.Services;

public class EmailService : IEmailService
{
    /// <inheritdoc />
    public Task SendAsync(string recipient, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("===============================================================");
        Console.WriteLine($"Sending email to: {recipient}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {htmlBody}");
        Console.WriteLine("===============================================================");
        return Task.CompletedTask;
    }
}
