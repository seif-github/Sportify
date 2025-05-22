using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using sportify.BLL.Services.Contracts;
using sportify.BLL.Settings;
using Microsoft.Extensions.Logging;
using MimeKit.Text;

namespace sportify.BLL.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(
        IOptions<SmtpSettings> settings,
        ILogger<EmailService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetLink)
    {
        var subject = "Reset your Sportify password";
        var emailTemplate = @"
        <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #111827; padding: 20px;"">
            <tr>
                <td align=""center"">
                    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px; background-color: #1f2937; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.35);"">
                        <tr>
                            <td style=""padding: 30px 40px; text-align: center;"">
                                <h1 style=""margin: 0; font-size: 24px; color: #10b981;"">Password Reset Request</h1>
                                <p style=""margin-top: 10px; font-size: 16px; color: #d1d5db;"">We received a request to reset your password. Click the button below to proceed.</p>
                            </td>
                        </tr>
                        <tr>
                            <td align=""center"" style=""padding: 20px;"">
                                <a href=""{0}"" target=""_blank""
                                   style=""display: inline-block; background: linear-gradient(135deg, #059669 0%, #064e3b 100%);
                                   color: #f3f4f6; text-decoration: none; padding: 12px 24px; border-radius: 6px; font-weight: bold; font-size: 16px;"">
                                    Reset Password
                                </a>
                            </td>
                        </tr>
                        <tr>
                            <td style=""padding: 0 40px 30px 40px; font-size: 14px; color: #9ca3af;"">
                                <p>If you didn't request this, please ignore this email.</p>
                                <p>If the button above doesn't work, copy and paste this link into your browser:</p>
                                <p style=""word-break: break-all;""><a href=""{0}"" style=""color: #10b981; text-decoration: underline;"">{0}</a></p>
                            </td>
                        </tr>
                        <tr>
                            <td style=""padding: 20px 40px; font-size: 12px; text-align: center; color: #6b7280;"">
                                © 2025 Sportify. All rights reserved.
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>";

        var message = string.Format(emailTemplate, resetLink);
        await SendEmailAsync(email, subject, message);
    }

    public async Task SendEmailVerificationAsync(string email, string confirmationLink)
    {
        var subject = "Verify your Sportify account";

        var emailTemplate = @"
        <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""background-color: #111827; padding: 20px;"">
            <tr>
                <td align=""center"">
                    <table role=""presentation"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width: 600px; background-color: #1f2937; border-radius: 8px; overflow: hidden; box-shadow: 0 4px 6px rgba(0,0,0,0.35);"">
                        <tr>
                            <td style=""padding: 30px 40px; text-align: center;"">
                                <h1 style=""margin: 0; font-size: 24px; color: #10b981;"">Welcome to Sportify!</h1>
                                <p style=""margin-top: 10px; font-size: 16px; color: #d1d5db;"">You're almost there! Please confirm your email address to complete your registration.</p>
                            </td>
                        </tr>
                        <tr>
                            <td align=""center"" style=""padding: 20px;"">
                                <a href=""{0}"" target=""_blank""
                                   style=""display: inline-block; background: linear-gradient(135deg, #059669 0%, #064e3b 100%);
                                   color: #f3f4f6; text-decoration: none; padding: 12px 24px; border-radius: 6px; font-weight: bold; font-size: 16px;"">
                                    Confirm Email
                                </a>
                            </td>
                        </tr>
                        <tr>
                            <td style=""padding: 0 40px 30px 40px; font-size: 14px; color: #9ca3af;"">
                                <p>If the button above doesn't work, copy and paste the following URL into your browser:</p>
                                <p style=""word-break: break-all;""><a href=""{0}"" style=""color: #10b981; text-decoration: underline;"">{0}</a></p>
                            </td>
                        </tr>
                        <tr>
                            <td style=""padding: 20px 40px; font-size: 12px; text-align: center; color: #6b7280;"">
                                © 2025 Sportify. All rights reserved.
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>";

        var message = string.Format(emailTemplate, confirmationLink);

        await SendEmailAsync(email, subject, message);
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
             
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName ?? string.Empty, _settings.FromEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;

            message.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _settings.SmtpHost,
                _settings.SmtpPort,
                MailKit.Security.SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                _settings.SmtpUsername,
                _settings.SmtpPassword);

            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            _logger.LogInformation($"Email sent to {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending email to {email}");
            throw;
        }
    }
}