using Respondia.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    /// <summary>
    /// Represents a feedback question within a feedback form.
    /// </summary>
    public class FeedbackQuestion : BaseEntity
    {
        /// <summary>
        /// Gets or sets the text of the feedback question.
        /// </summary>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of the feedback question.
        /// </summary>
        public QuestionType QuestionType { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated feedback form.
        /// </summary>
        public int FeedbackFormId { get; set; }

        /// <summary>
        /// Gets or sets the associated feedback form.
        /// </summary>
        public FeedbackForm FeedbackForm { get; set; } = default!;

        /// <summary>
        /// Gets or sets the JSON string containing multiple-choice options for the question.
        /// </summary>
        public string MultipleChoiceOptions { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of feedback answers associated with this question.
        /// </summary>
        public ICollection<FeedbackAnswer> FeedbackAnswers { get; set; } = new List<FeedbackAnswer>();
    }
}
