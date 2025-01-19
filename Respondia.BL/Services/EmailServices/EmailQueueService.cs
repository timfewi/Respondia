using Microsoft.Extensions.DependencyInjection;
using Respondia.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.BL.Services.EmailServices
{
    //[Injectable(ServiceLifetime.Singleton)]
    public class EmailQueueService
    {
        private readonly Queue<(string ToEmail, string Subject, string HtmlMessage)> _emails = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void EnqueueEmail(string toEmail, string subject, string htmlMessage)
        {
            lock (_emails)
            {
                _emails.Enqueue((toEmail, subject, htmlMessage));
            }
            _signal.Release();
        }

        public async Task<(string ToEmail, string Subjedct, string HtmlMessage)> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);

            lock (_emails)
            {
                return _emails.Dequeue();
            }
        }
    }
}
