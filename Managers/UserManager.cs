using GraphAPIDelegatedFlow.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GraphAPIDelegatedFlow.Managers
{
    public interface IUserManager
    {
        Task<string> GetUser(string token);
    }

    public class UserManager : IUserManager
    {
        private readonly ILogger<UserManager> _logger;

        public UserManager(ILogger<UserManager> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetUser(string token)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await httpClient.GetAsync("https://graph.microsoft.com/v1.0/me");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }
            else
            {
                _logger.LogError($"Failed to get user: {response.ReasonPhrase}");
                throw new Exception("Failed to get user");
            }
        }
    }
}
