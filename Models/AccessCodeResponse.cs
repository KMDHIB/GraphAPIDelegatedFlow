namespace GraphAPIDelegatedFlow.Models
{
    /// <summary>
    /// Response object containg a link for logging into MS Graph API Tenant
    /// </summary>
    public class AccessCodeResponse
    {
        /// <summary>
        /// Link for logging into MS Graph API Tenant
        /// </summary>
        public string Link { get; set; } = string.Empty;
    }
}
