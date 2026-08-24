using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Shifter.Application.DTOs.Identity;
using Shifter.Application.Interfaces.Auth;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Shifter.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auth");

        group.MapPost("/register", RegisterAsync).AllowAnonymous();
        group.MapGet("/verify-email", ConfirmEmailAsync).AllowAnonymous();
        group.MapPost("/login", LoginAsync).AllowAnonymous();
        group.MapPost("/verify-2fa", VerifyMfaAsync).RequireAuthorization("MfaPendingOnly");
        group.MapGet("/me", GetCurrentUserAsync);
        group.MapPost("/logout", Logout);
    }

    private static async Task<IResult> RegisterAsync([FromBody] Application.DTOs.Identity.RegisterRequest request, IAuthService authService)
    {
        var result = await authService.RegisterAsync(request);
        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }
        return Results.Ok("User registered successfully. Please check your email to confirm your account.");
    }

    private static async Task<IResult> ConfirmEmailAsync([FromQuery] Guid userId, [FromQuery] string token, IAuthService authService)
    {
        var result = await authService.ConfirmEmailAsync(userId, token);
        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }
        return Results.Ok("Email confirmed successfully.");
    }

    private static async Task<IResult> LoginAsync([FromBody] LoginRequest request, IAuthService authService, HttpContext context)
    {
        context.Response.Cookies.Delete("accessToken"); // Clear any existing token

        var user = await authService.ValidateUserAsync(request.Email, request.Password);
        if (user == null)
        {
            return Results.Unauthorized();
        }

        bool isTwoFactorEnabled = await authService.IsTwoFactorEnabledAsync(user);
        bool isSetupRequired = !isTwoFactorEnabled;
        var unformattedKey = await authService.GetAuthenticatorKeyAsync(user, isTwoFactorEnabled);        
        var qrCodeUri = await authService.GenerateQrCodeUri(user, unformattedKey);
        var mfaToken = authService.GenerateMfaToken(user);

        return Results.Ok(new 
        { 
            RequiresMfa = true, 
            MfaToken = mfaToken,
            SetupRequired = isSetupRequired,
            ManualEntryKey = isTwoFactorEnabled ? null : unformattedKey,
            QrCodeUri = qrCodeUri
        });
    }

    private static async Task<IResult> VerifyMfaAsync([FromBody] MfaRequest request, ClaimsPrincipal userPrincipal, IAuthService authService, HttpContext context)
    {
        // Ensure the token being used is an MFA challenge token and not a final JWT token
        var stage = userPrincipal.FindFirst("stage")?.Value;
        if (stage != "mfa_verification") return Results.BadRequest("Invalid token stage.");

        // Extract the user ID from the claims
        var userIdStr = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return Results.Unauthorized();

        var finalToken = await authService.VerifyMfaAndGenerateTokenAsync(userIdStr, request.Code);
        if (finalToken == null) return Results.BadRequest("Invalid 2FA code.");

        var cookieOptions = authService.ConfigureCookieOptions();
        context.Response.Cookies.Append("accessToken", finalToken, cookieOptions);

        var user = await authService.GetCurrentUserAsync(Guid.Parse(userIdStr));

        return Results.Ok(user);
    }

    private static async Task<IResult> GetCurrentUserAsync(ClaimsPrincipal userPrincipal, IAuthService authService)
    {
        var userIdStr = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return Results.Unauthorized();

        var userId = Guid.Parse(userIdStr);
        if (userId == Guid.Empty) return Results.Unauthorized();

        var user = await authService.GetCurrentUserAsync(userId);
        
        if (user == null) return Results.Unauthorized();

        return Results.Ok(user);
    }

    private static IResult Logout(HttpContext context)
    {
        // Clear the authentication cookie
        context.Response.Cookies.Delete("accessToken");
        return Results.Ok("Logged out successfully.");
    }
}
