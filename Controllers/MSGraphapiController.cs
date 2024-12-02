using System.Text;
using GraphAPIDelegatedFlow.Helpers;
using GraphAPIDelegatedFlow.Managers;
using GraphAPIDelegatedFlow.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraphAPIDelegatedFlow.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MSGraphapiController : ControllerBase
    {
        private readonly ILogger<MSGraphapiController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILoginManager _loginManager;
        private readonly IMailManager _mailManager;
        private readonly IUserManager _userManager;


        public MSGraphapiController(ILogger<MSGraphapiController> logger, IConfiguration configuration, ILoginManager loginManager, IMailManager mailManager, IUserManager userManager)
        {
            _logger = logger;
            _configuration = configuration;
            _loginManager = loginManager;
            _mailManager = mailManager;
            _userManager = userManager;
        }

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
                       .Append("&scope=").Append(_configuration["Scopes"]).Replace(" ", "%20").Replace("+", "%20")
                       .Append("&state=").Append(state);

            return new AccessCodeResponse()
            {
                Link = linkBuilder.ToString()
            };
        }

        [HttpGet("Callback")]
        public CallbackResponse GetCallBack(string code)
        {
            return new CallbackResponse()
            {
                Code = code
            };
        }

        [HttpGet("AccessToken")]
        public async Task<string> GetAccessToken(string code)
        {
            return await _loginManager.GetToken(code);
        }

        [HttpGet("RefreshToken")]
        public async Task<string> RefreshToken(string refreshToken)
        {
            return await _loginManager.RefeshToken(refreshToken);
        }

        [HttpGet("User")]
        public async Task<string> GetUser([FromHeader(Name = "Authorization")] string bearerToken)
        {
            return await _userManager.GetUser(TokenHelper.Handle(bearerToken));
        }

        [HttpGet("SendMail")]
        public async Task SendMail([FromHeader(Name = "Authorization")] string bearerToken)
        {
            await _mailManager.SendMail(TokenHelper.Handle(bearerToken));
        }
    }
}
