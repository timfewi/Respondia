using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.DTOs.FeedbackDTOs
{
    public class FeedbackFormDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public FeedbackSummeryDTO? Summery { get; set; }
        public List<FeedbackQuestionDTO>? Questions { get; set; } = new();

    }
}
