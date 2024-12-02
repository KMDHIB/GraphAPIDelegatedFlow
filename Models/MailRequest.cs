
namespace GraphAPIDelegatedFlow.Models
{
/// <summary>
/// Represents a request to send an email.
/// </summary>
public record MailRequest
{
    /// <summary>
    /// Gets or sets the list of recipient email addresses.
    /// </summary>
    public List<string> mailTo { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets the subject of the email.
    /// </summary>
    public string mailSubject { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the body content of the email.
    /// </summary>
    public string mailBody { get; set; } = string.Empty;
}
}
