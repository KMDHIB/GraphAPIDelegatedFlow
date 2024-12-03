using System.Net.Http.Headers;

namespace GraphAPIDelegatedFlow.Managers
{
    /// <summary>
    /// Interface for managing user-related operations.
    /// </summary>
    public interface IUserManager
    {
        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>The user information as a string.</returns>
        Task<string> GetUser(string token);
    }

    /// <summary>
    /// Class for managing user-related operations.
    /// </summary>
    public class UserManager : IUserManager
    {
        private readonly ILogger<UserManager> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public UserManager(ILogger<UserManager> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets the user information.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <returns>The user information as a string.</returns>
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
