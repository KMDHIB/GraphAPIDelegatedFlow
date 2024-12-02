namespace GraphAPIDelegatedFlow.Helpers
{
    /// <summary>
    /// Login callback from Microsoft.
    /// </summary>
    public static class TokenHelper
    {
        public static string Handle(string bearerToken) {
            return bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
        }
    }
}
