using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    /// <summary>
    /// Represents a feedback form used to collect responses from users.
    /// </summary>
    public class FeedbackForm : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the feedback form.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the feedback form.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the feedback form is finalized (i.e. no further questions can be added).
        /// </summary>
        public bool IsFinalized { get; set; } = false; 

        /// <summary>
        /// Gets or sets the identifier of the associated campaign.
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        /// Shows the successful Feedbacks submits.
        /// </summary>
        public int SubmitCount { get; set; }

        /// <summary>
        /// Gets or sets the associated campaign.
        /// </summary>
        public Campaign Campaign { get; set; } = default!;

        /// <summary>
        /// Gets or sets the collection of feedback questions included in the form.
        /// </summary>
        public ICollection<FeedbackQuestion> FeedbackQuestions { get; set; } = new List<FeedbackQuestion>();

        /// <summary>
        /// Gets or sets the summary of feedback collected from the form.
        /// </summary>
        public FeedbackSummery FeedbackSummery { get; set; } = default!;

        /// <summary>
        /// Direct 1:1 Relation to Link 
        /// </summary>
        public FeedbackLink? FeedbackLink { get; set; }

    }
}
