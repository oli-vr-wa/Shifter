using Shifter.Application.Interfaces.Services.Emails;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
