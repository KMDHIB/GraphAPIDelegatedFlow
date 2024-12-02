namespace GraphAPIDelegatedFlow.Managers
{
    public interface ILoginManager
    {
        Task<string> GetToken(string code);
        Task<string> RefeshToken(string refreshToken);
    }

    public class LoginManager : ILoginManager
    {
        private readonly ILogger<LoginManager> _logger;
        private readonly IConfiguration _configuration;

        public LoginManager(ILogger<LoginManager> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

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
