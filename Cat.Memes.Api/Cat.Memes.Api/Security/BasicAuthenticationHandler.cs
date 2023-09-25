using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Cat.Memes.Api.Security;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration _configuration;

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IConfiguration configuration)
        : base(options, logger, encoder, clock)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!AuthenticationHeaderValue.TryParse(Request.Headers["Authorization"], out var header))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing credentials"));
        }

        if (header?.Parameter is null)
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing credentials"));
        }

        byte[] credentialBytes;

        try
        {
            credentialBytes = Convert.FromBase64String(header.Parameter);
        }
        catch (FormatException)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid encoding"));
        }

        var credentials = Encoding.UTF8
            .GetString(credentialBytes)
            .Split(new[] { ':' }, 2);

        if (credentials.Length != 2)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
        }

        var username = credentials[0];
        var password = credentials[1];

        if (username != _configuration["ApiUser"] || password != _configuration["ApiPassword"])
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
        }
        var claims = new[] {new Claim(ClaimTypes.Name, username)};
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        
        var ticket = new AuthenticationTicket(principal,
            new AuthenticationProperties(),
            Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}