using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.DTOs.CampaignDTOs
{
    public class CampaignStatisticsDTO
    {
        public int Id { get; set; }
        public int TotalResponses { get; set; }
        public decimal AverageRating { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
