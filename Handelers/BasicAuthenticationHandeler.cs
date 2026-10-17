using System.Security.Claims;
using System.Text.Encodings.Web;
using HotelListing.Api.Contracts;
using HotelListing.Api.DTOs.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

public class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IUserService userService
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValue))
        {
            return AuthenticateResult.Fail("Missing Authorization Header");
        }

        // Basic Base64(username:password)
        var authHeader = authorizationHeaderValue.ToString();
        if (
            string.IsNullOrEmpty(authHeader)
            || !authHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase)
        )
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        var token = authHeader["Basic ".Length..].Trim();
        string decoded;

        try
        {
            var credentialBytes = Convert.FromBase64String(token);
            //{username:password}
            decoded = System.Text.Encoding.UTF8.GetString(credentialBytes);
        }
        catch (FormatException)
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        var separatorIndex = decoded.IndexOf(':');
        if (separatorIndex == -1)   
        {
            return AuthenticateResult.Fail("Invalid Authorization Header");
        }

        var usernameOrEmail = decoded[..separatorIndex];
        var password = decoded[(separatorIndex + 1)..]; 

        var loginDto = new LoginUserDto
        {
            Email = usernameOrEmail,
            Password = password
        };

        var result = await userService.LoginAsync(loginDto);
        if (!result.IsSuccess)
        {
            return AuthenticateResult.Fail("Invalid username or password");     
        }

        var claims = new List<Claim> { new Claim(ClaimTypes.Name, usernameOrEmail) };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}

