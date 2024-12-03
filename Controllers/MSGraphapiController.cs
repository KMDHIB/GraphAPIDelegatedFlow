using System.Text;
using GraphAPIDelegatedFlow.Helpers;
using GraphAPIDelegatedFlow.Managers;
using GraphAPIDelegatedFlow.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraphAPIDelegatedFlow.Controllers
{
    /// <summary>
    /// Controller for handling Microsoft Graph API operations.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class MSGraphapiController : ControllerBase
    {
        private readonly ILogger<MSGraphapiController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILoginManager _loginManager;
        private readonly IMailManager _mailManager;
        private readonly IUserManager _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSGraphapiController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="loginManager">The login manager instance.</param>
        /// <param name="mailManager">The mail manager instance.</param>
        /// <param name="userManager">The user manager instance.</param>
        public MSGraphapiController(ILogger<MSGraphapiController> logger, IConfiguration configuration, ILoginManager loginManager, IMailManager mailManager, IUserManager userManager)
        {
            _logger = logger;
            _configuration = configuration;
            _loginManager = loginManager;
            _mailManager = mailManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Generates a link for obtaining an access code.
        /// </summary>
        /// <param name="state">The state parameter to include in the link.</param>
        /// <returns>An <see cref="AccessCodeResponse"/> containing the generated link.</returns>
        [HttpGet("AccessCodeLink")]
        public AccessCodeResponse GetLinkForAccessCode(string state)
        {
            var linkBuilder = new StringBuilder();

            linkBuilder.Append("https://login.microsoftonline.com/")
                       .Append(_configuration["TenantID"])
                       .Append("/oauth2/v2.0/authorize")
                       .Append("?client_id=").Append(_configuration["ClientID"])
                       .Append("&response_type=code")
                       .Append("&redirect_uri=").Append(_configuration["CallBackUrl"])
                       .Append("&response_mode=query")
                       .Append("&scope=").Append(_configuration["Scopes"])
                       .Append("&state=").Append(state);

            return new AccessCodeResponse()
            {
                Link = linkBuilder.ToString()
            };
        }

        /// <summary>
        /// Handles the callback from the authorization server.
        /// </summary>
        /// <param name="code">The authorization code received from the server.</param>
        /// <returns>A <see cref="CallbackResponse"/> containing the authorization code.</returns>
        [HttpGet("Callback")]
        public CallbackResponse GetCallBack(string code)
        {
            return new CallbackResponse()
            {
                Code = code
            };
        }

        /// <summary>
        /// Retrieves an access token using the provided authorization code.
        /// </summary>
        /// <param name="code">The authorization code.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the access token.</returns>
        [HttpGet("AccessToken")]
        public async Task<string> GetAccessToken(string code)
        {
            return await _loginManager.GetToken(code);
        }

        /// <summary>
        /// Refreshes the access token using the provided refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the new access token.</returns>
        [HttpGet("RefreshToken")]
        public async Task<string> RefreshToken(string refreshToken)
        {
            return await _loginManager.RefeshToken(refreshToken);
        }

        /// <summary>
        /// Retrieves user information using the provided bearer token.
        /// </summary>
        /// <param name="bearerToken">The bearer token.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user information.</returns>
        [HttpGet("User")]
        public async Task<string> GetUser([FromHeader(Name = "Authorization")] string bearerToken)
        {
            return await _userManager.GetUser(TokenHelper.Handle(bearerToken));
        }

        /// <summary>
        /// Sends an email using the provided bearer token and mail request.
        /// </summary>
        /// <param name="bearerToken">The bearer token.</param>
        /// <param name="mailRequest">The mail request containing email details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        [HttpPost("SendMail")]
        public async Task SendMail([FromHeader(Name = "Authorization")] string bearerToken, [FromBody] MailRequest mailRequest)
        {
            await _mailManager.SendMail(TokenHelper.Handle(bearerToken), mailRequest);
        }
    }
}
