namespace GraphAPIDelegatedFlow.Models
{
    /// <summary>
    /// Reponse object containing authentication code from Callback.
    /// </summary>
    public record CallbackResponse
    {
        /// <summary>
        /// The authentication code which can be used to get a new Access Token within 10 minutes.
        /// </summary>
        public string Code { get; set; } = string.Empty;

    }
}
