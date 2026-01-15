using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ChecktonPld.RestAPI.Security
{
    /// <summary>
    /// class to handle api_key security.
    /// </summary>
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;

        /// <summary>
		/// scheme name for authentication handler.
		/// </summary>
		public const string SchemeName = "ApiKey";

        /// <summary>
        /// constructor.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public ApiKeyAuthenticationHandler(
            IConfiguration configuration,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// verify that require api key header exist and handle authorization.
        /// </summary>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("X-API-Key"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            if (!Request.Headers.ContainsKey("user"))
            {
                return AuthenticateResult.Fail("No user Guid provided in the header");
            }

            if (!Guid.TryParse(Request.Headers["X-API-Key"], out var apiKey))
            {
                return AuthenticateResult.Fail("Not a Guid key");
            }

            if (!Guid.TryParse(Request.Headers["user"], out var userGuid))
            {
                return AuthenticateResult.Fail("Not a Guid user");
            }

            try
            {
                var configuredApiKey = Guid.Parse(_configuration["API-Key"] ?? string.Empty);
                if (apiKey != configuredApiKey)
                {
                    return AuthenticateResult.Fail("Invalid API Key");
                }

            }
            catch (Exception e)
            {
                Logger.LogError(e, "Uncaught exception.");
                return AuthenticateResult.Fail("API-Key environment variable not set");
            }

            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(
                new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.Authentication, apiKey.ToString()),
                    new("Guid", userGuid.ToString()),
                }, SchemeName)),
                SchemeName)));
        }
    }
}