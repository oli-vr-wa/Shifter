using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Shifter.Application.DTOs.Identity;
using Shifter.Application.Interfaces.Auth;
using System.Security.Claims;

namespace Shifter.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/auth");

        group.MapPost("/login", LoginAsync);
        group.MapPost("/verify-mfa", VerifyMfaAsync).RequireAuthorization();
    }

    private static async Task<IResult> LoginAsync([FromBody] LoginRequest request, IAuthService authService)
    {
        var user = await authService.ValidateUserAsync(request.Email, request.Password);
        if (user == null)
        {
            return Results.Unauthorized();
        }
        var mfaToken = authService.GenerateMfaToken(user);
        return Results.Ok(new { RequiresMfa = true, MfaToken = mfaToken });
    }

    private static async Task<IResult> VerifyMfaAsync([FromBody] MfaRequest request, ClaimsPrincipal userPrincipal, IAuthService authService)
    {
        // Ensure the token being used is an MFA challenge token and not a final JWT token
        var stage = userPrincipal.FindFirst("stage")?.Value;
        if (stage != "mfa_verification") return Results.BadRequest("Invalid token stage.");

        // Extract the user ID from the claims
        var userIdStr = userPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr)) return Results.Unauthorized();

        var finalToken = await authService.VerifyMfaAndGenerateTokenAsync(userIdStr, request.Code);
        if (finalToken == null) return Results.BadRequest("Invalid 2FA code.");


        return Results.Ok(new LoginResponse(finalToken));
    }
}
