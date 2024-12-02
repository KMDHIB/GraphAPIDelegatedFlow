using Newtonsoft.Json;

namespace GraphAPIDelegatedFlow.Managers
{
    public interface IMailManager
    {
        Task SendMail(string token);
    }

    public class MailManager : IMailManager
    {
        private readonly ILogger<MailManager> _logger;

        public MailManager(ILogger<MailManager> logger)
        {
            _logger = logger;
        }

        public async Task SendMail(string token)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var email = new
            {
                Message = new
                {
                    Subject = "Test Mail",
                    Body = new
                    {
                        ContentType = "Text",
                        Content = "This is a test mail."
                    },
                    ToRecipients = new[]
                    {
                        new
                        {
                            EmailAddress = new
                            {
                                Address = "hib@kmd.dk"
                            }
                        }
                    }
                },
                SaveToSentItems = "true"
            };

            var json = JsonConvert.SerializeObject(email);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://graph.microsoft.com/v1.0/me/sendMail", content);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email sent successfully.");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Error sending email: {error}");
            }
        }
    }
}
