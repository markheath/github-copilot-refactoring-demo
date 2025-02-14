namespace GloboticketWeb.Services;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;


/// <summary>
/// A basic implementation of the IEmailService interface.
/// </summary>
public class EmailService : IEmailService
{
    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        // Configure your SMTP settings here.
        using var smtpClient = new SmtpClient("smtp.example.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("your_username", "your_password"),
            EnableSsl = true
        };

        // Create the email message.
        var mailMessage = new MailMessage("sender@example.com", recipient, subject, body);

        // Send the email asynchronously.
        await smtpClient.SendMailAsync(mailMessage);
    }
}