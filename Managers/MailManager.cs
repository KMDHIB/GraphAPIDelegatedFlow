using System.Net.Mail;
using GraphAPIDelegatedFlow.Models;
using Newtonsoft.Json;

namespace GraphAPIDelegatedFlow.Managers
{
    /// <summary>
    /// Interface for managing mail operations.
    /// </summary>
    public interface IMailManager
    {
        /// <summary>
        /// Sends an email using the provided token and mail request.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="mailRequest">The mail request containing email details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendMail(string token, MailRequest mailRequest);
    }

    /// <summary>
    /// Class for managing mail operations.
    /// </summary>
    public class MailManager : IMailManager
    {
        private readonly ILogger<MailManager> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MailManager"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public MailManager(ILogger<MailManager> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Sends an email using the provided token and mail request.
        /// </summary>
        /// <param name="token">The authentication token.</param>
        /// <param name="mailRequest">The mail request containing email details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendMail(string token, MailRequest mailRequest)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var email = new
            {
                Message = new
                {
                    Subject = mailRequest.mailSubject,
                    Body = new
                    {
                        ContentType = "Html",
                        Content = mailRequest.mailBody
                    },
                    ToRecipients = mailRequest.mailTo.Select(to => new
                    {
                        EmailAddress = new
                        {
                            Address = to
                        }
                    }).ToList(),
                    CcRecipients = new List<object>(),
                    Attachments = new List<object>()
                    
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
