namespace GraphAPIDelegatedFlow.Models
{
    /// <summary>
    /// Login callback from Microsoft.
    /// </summary>
    public class CallbackRequest
    {
        /// <summary>
        /// Authentication Code used for receiving an access token.
        /// </summary>
        public string Code {  get; set; } = string.Empty;
        /// <summary>
        /// Value which matches the 'State' of login request which was send.
        /// </summary>
        public string State { get; set; } = string.Empty;
        /// <summary>
        /// Error title (if there is an error).
        /// </summary>
        public string Error { get; set; } = string.Empty;
        /// <summary>
        /// Error details (if there is an error).
        /// </summary>
        public string Error_description { get; set; } = string.Empty;
        /// <summary>
        /// Error Uri (if there is an error).
        /// </summary>
        public string Error_uri { get; set; } = string.Empty;
    }
}
