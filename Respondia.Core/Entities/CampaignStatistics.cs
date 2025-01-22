using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    /// <summary>
    /// Represents the statistics of a campaign, including total responses, average rating, and last updated date.
    /// </summary>
    public class CampaignStatistics : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the associated campaign.
        /// </summary>
        public int CampaignId { get; set; }

        /// <summary>
        /// Gets or sets the associated campaign.
        /// </summary>
        public Campaign Campaign { get; set; } = default!;

        /// <summary>
        /// Gets or sets the total number of responses received for the campaign.
        /// </summary>
        public int TotalResponses { get; set; }

        /// <summary>
        /// Gets or sets the average rating of the campaign based on the responses.
        /// </summary>
        public decimal AverageRating { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the statistics were last updated.
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
