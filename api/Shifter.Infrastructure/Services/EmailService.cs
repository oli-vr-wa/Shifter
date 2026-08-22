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
        Debug.WriteLine("===============================================================");
        Debug.WriteLine($"Sending email to: {recipient}");
        Debug.WriteLine($"Subject: {subject}");
        Debug.WriteLine($"Body: {htmlBody}");
        Debug.WriteLine("===============================================================");
        return Task.CompletedTask;
    }
}
