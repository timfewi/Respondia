using Respondia.Core.DTOs.FeedbackDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.DTOs.CampaignDTOs
{
    public class CampaignDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? EndDate { get; set; }
        public bool IsInactive { get; set; }
        public CampaignStatisticsDTO? Statistics { get; set; }
        public List<FeedbackFormShortDTO> FeedbackForms { get; set; } = new();
    }
}
