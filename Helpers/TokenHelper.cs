/// <summary>
/// Provides helper methods for handling tokens.
/// </summary>
namespace GraphAPIDelegatedFlow.Helpers
{
    public static class TokenHelper
    {
        /// <summary>
        /// Processes the provided bearer token by removing the "Bearer " prefix if it exists.
        /// </summary>
        /// <param name="bearerToken">The bearer token to process.</param>
        /// <returns>The token without the "Bearer " prefix if it was present; otherwise, the original token.</returns>
        public static string Handle(string bearerToken) {
            return bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
        }
    }
}
