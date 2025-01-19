using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Respondia.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.BL.Services.EmailServices
{
    //[Injectable(ServiceLifetime.Singleton)]
    public class EmailProcessorService : BackgroundService
    {
        private readonly EmailQueueService _queue;
        private readonly EmailSenderService _sender;
        private readonly ILogger<EmailProcessorService> _logger;

        public EmailProcessorService(EmailQueueService queue, EmailSenderService sender, ILogger<EmailProcessorService> logger)
        {
            _queue = queue;
            _sender = sender;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var (toEmail, subject, htmlMessage) = await _queue.DequeueAsync(stoppingToken);
                    await _sender.SendEmailAsync(toEmail, subject, htmlMessage, stoppingToken);
                    _logger.LogInformation("Successfully sent email to {ToEmail} with subject: \"{Subject}\" at {Time}",
                   toEmail, subject, DateTime.Now);

                }
                catch (OperationCanceledException)
                {
                    _logger.LogWarning("Email processing was canceled at {Time}.", DateTime.UtcNow);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email. Error: {ErrorMessage} at {Time}.", ex.Message, DateTime.UtcNow);

                }
            }
            _logger.LogInformation("Email processing service is stopping at {Time}.", DateTime.UtcNow);

        }
    }
}
