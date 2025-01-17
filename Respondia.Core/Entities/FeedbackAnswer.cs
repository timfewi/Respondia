using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    public class FeedbackAnswer : BaseEntity
    {
        public int FeedbackQuestionId { get; set; }
        public FeedbackQuestion FeedbackQuestion { get; set; } = default!;
        public string Response { get; set; } = string.Empty;
        public DateTime SubmittedDate { get; set; }
        public string UserIdentifier { get; set; } = string.Empty;

    }
}
