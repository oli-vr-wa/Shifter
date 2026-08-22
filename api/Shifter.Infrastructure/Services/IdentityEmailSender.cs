using Microsoft.AspNetCore.Identity;
using MimeKit;
using Shifter.Application.Interfaces.Services.Emails;

namespace Shifter.Infrastructure.Services;

public class IdentityEmailSender(IEmailService emailService) : IEmailSender<IdentityUser>
{
    private readonly IEmailService _emailService = emailService;

    public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
    {
        string subject = "Confirm your Shifter Account";
        string body = $"Welcome to Shifter! Please confirm your account by clicking the following link: <a href='{confirmationLink}'>Confirm Account</a>";

        return _emailService.SendAsync(email, subject, body);
    }

    public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
    {
        string subject = "Reset your Shifter Account Password";
        string body = $"You requested a password reset. Use the following code to reset your password: {resetCode}";

        return _emailService.SendAsync(email, subject, body);
    }

    public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
    {
        string subject = "Reset your Shifter Account Password";
        string body = $"You requested a password reset. Click the following link to reset your password: <a href='{resetLink}'>Reset Password</a>";

        return _emailService.SendAsync(email, subject, body);
    }
}
