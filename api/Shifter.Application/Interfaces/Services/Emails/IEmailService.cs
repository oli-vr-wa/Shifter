namespace Shifter.Application.Interfaces.Services.Emails;

public interface IEmailService
{
    /// <summary>
    /// Sends an email asynchronously to the specified recipient with the given subject and HTML body.
    /// </summary>
    /// <param name="recipient">The email address of the recipient.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="htmlBody">The HTML content of the email body.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task SendAsync(string recipient, string subject, string htmlBody, CancellationToken cancellationToken = default);
}
