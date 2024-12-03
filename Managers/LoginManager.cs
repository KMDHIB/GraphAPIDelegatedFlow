namespace GraphAPIDelegatedFlow.Managers
{
    /// <summary>
    /// Interface for managing login operations.
    /// </summary>
    public interface ILoginManager
    {
        /// <summary>
        /// Gets the token using the provided authorization code.
        /// </summary>
        /// <param name="code">The authorization code.</param>
        /// <returns>A new access token.</returns>
        Task<string> GetToken(string code);
        /// <summary>
        /// Refreshes the token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>A new access token.</returns>
        Task<string> RefeshToken(string refreshToken);
    }

    /// <summary>
    /// Class for managing login operations.
    /// </summary>
    public class LoginManager : ILoginManager
    {
        private readonly ILogger<LoginManager> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginManager"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="configuration">The configuration instance.</param>
        public LoginManager(ILogger<LoginManager> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Gets the token using the provided authorization code.
        /// </summary>
        /// <param name="code">The authorization code.</param>
        /// <returns>A new access token.</returns>
                public async Task<string> GetToken(string code)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "client_id", _configuration["ClientID"]! },
                    { "scope", _configuration["Scopes"]! },          
                    { "code", code },
                    { "redirect_uri", _configuration["CallBackUrl"]! },
                    { "grant_type", "authorization_code" },
                    { "client_secret", _configuration["ClientSecret"]! }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("https://login.microsoftonline.com/" + _configuration["TenantID"]! + "/oauth2/v2.0/token", content);

                return await response.Content.ReadAsStringAsync();
            }
        }

        /// <summary>
        /// Refreshes the token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>A new access token.</returns>
        public async Task<string> RefeshToken(string refreshToken)
        {
            using (var client = new HttpClient())
            {
                var values = new Dictionary<string, string>
                {
                    { "client_id", _configuration["ClientID"]! },
                    { "scope", _configuration["Scopes"]!  },
                    { "refresh_token", refreshToken },
                    { "redirect_uri", _configuration["CallBackUrl"]! },
                    { "grant_type", "refresh_token" },
                    { "client_secret", _configuration["ClientSecret"]! }
                };

                var content = new FormUrlEncodedContent(values);
                var response = await client.PostAsync("https://login.microsoftonline.com/" + _configuration["TenantID"]! + "/oauth2/v2.0/token", content);

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
