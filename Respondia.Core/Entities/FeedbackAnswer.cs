using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    /// <summary>
    /// Represents an answer to a feedback question.
    /// </summary>
    public class FeedbackAnswer : BaseEntity
    {
        /// <summary>
        /// Gets or sets the ID of the feedback question.
        /// </summary>
        public int FeedbackQuestionId { get; set; }

        /// <summary>
        /// Gets or sets the feedback question associated with this answer.
        /// </summary>
        public FeedbackQuestion FeedbackQuestion { get; set; } = default!;

        /// <summary>
        /// Gets or sets the response text provided by the user.
        /// </summary>
        public string Response { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date and time when the feedback was submitted.
        /// </summary>
        public DateTime SubmittedDate { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who submitted the feedback.
        /// </summary>
        public string UserIdentifier { get; set; } = string.Empty;
    }
}
