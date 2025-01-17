using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    public class CampaignStatistics : BaseEntity
    {
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; } = default!;
        public int TotalResponses { get; set; }
        public decimal AverageRating { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
