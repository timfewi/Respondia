using Respondia.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    public class FeedbackQuestion : BaseEntity
    {
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public int FeedbackFormId { get; set; }
        public FeedbackForm FeedbackForm { get; set; } = default!;
        // JSON for multiple-choice options
        public string MultipleChoiceOptions { get; set; } = string.Empty;
        public ICollection<FeedbackAnswer> FeedbackAnswers { get; set; } = new List<FeedbackAnswer>();
    }
}
