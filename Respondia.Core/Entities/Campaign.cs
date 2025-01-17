using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    public class Campaign : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? EndDate { get; set; }
        public bool IsInactive { get; set; } = false;

        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; } = default!;

        public ICollection<FeedbackForm> FeedbackForms { get; set; } = new List<FeedbackForm>();
        public CampaignStatistics Statistics { get; set; } = default!;

    }
}
