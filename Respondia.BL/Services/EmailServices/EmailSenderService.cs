using Respondia.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Respondia.Core.Entities;
using Respondia.Core.Utils;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Respondia.Core.Exceptions;
using Microsoft.Extensions.Logging;
using System.Text.Encodings.Web;

namespace Respondia.BL.Services.EmailServices
{
    [Injectable(lifetime: ServiceLifetime.Transient)]
    public class EmailSenderService : IEmailSender<AppUser>
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailSenderService> _logger;
        public EmailSenderService(IOptions<SmtpSettings> smtpSettings, ILogger<EmailSenderService> logger)
        {
            _smtpSettings = smtpSettings.Value;
            _logger = logger;
        }
        public Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink)
        {
            var subject = "Confirm your email";
            var body = $@"
                <p>Hi {user.UserName},</p>
                <p>Thank you for registering. Please confirm your account by clicking the link below:</p>
                <p><a href='{HtmlEncoder.Default.Encode(confirmationLink)}'>Confirm Email</a></p>
                <p>If you did not register, you can ignore this email.</p>";

            return SendEmailAsync(email, subject, body);
        }


        public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode)
        {
            return SendEmailAsync(email, "Reset your password", $"Please reset your password using the following code: {resetCode}");
        }

        public Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink)
        {
            return SendEmailAsync(email, "Reset your password", $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");
        }


        public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage, CancellationToken cancellationToken = default)
        {
            try
            {
                using (var client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port))
                {
                    client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                    client.EnableSsl = true;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
                        Subject = subject,
                        Body = htmlMessage,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail}", toEmail);
                throw new ServiceException($"Failed to send email to {toEmail}", new InvalidOperationException());
            }
        }
    }
}
