namespace GloboticketWeb.Services;

/// <summary>
/// Defines the contract for an email service.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="recipient">The email address of the recipient.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body content of the email.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    Task SendEmailAsync(string recipient, string subject, string body);
}
