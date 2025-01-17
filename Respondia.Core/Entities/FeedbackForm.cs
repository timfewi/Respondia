using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    public class FeedbackForm : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; } = default!;
        public ICollection<FeedbackQuestion> FeedbackQuestions { get; set; } = new List<FeedbackQuestion>();
        public FeedbackSummery FeedbackSummery { get; set; } = default!;


    }
}
