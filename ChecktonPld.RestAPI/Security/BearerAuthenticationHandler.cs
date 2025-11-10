using System;
using System.Net.Http.Headers;
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
	/// class to handle bearer authentication.
	/// </summary>
	public class BearerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;

        /// <summary>
		/// scheme name for authentication handler.
		/// </summary>
		public const string SchemeName = "Bearer";

        /// <summary>
        /// Handler for the bearer token
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public BearerAuthenticationHandler(
            IConfiguration configuration,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Authentication handler
        /// </summary>
        /// <returns></returns>
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            try
            {
                // TODO EMD: PENDIENTE IMPLEMENTAR
                /*var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var jwsToken = authHeader.Parameter;
                JWTBase refreshTokenInstance = new(
                    token: jwsToken,
                    // TODO: Review from where to take this information
                    issuer: "authenticationservice",
                    audience: "webui",
                    publicKey: new RSAKeyPair(
                        publicKeyString: _configuration["PublicKey"]).RSAPublicKey);*/
                ClaimsIdentity claimsIdentity = new([], Scheme.Name);//new(refreshTokenInstance.JWTTokenInstance.Claims, Scheme.Name);
                ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
                AuthenticationTicket authenticationTicket = new(
                    principal: claimsPrincipal,
                    authenticationScheme: Scheme.Name);
                return await Task.FromResult(AuthenticateResult.Success(authenticationTicket));
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Uncaught exception.");
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }
        }
    }
}
