using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.DTOs.CampaignDTOs
{
    public class CampaignCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? EndDate { get; set; }
    }
}
