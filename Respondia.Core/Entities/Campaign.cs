using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    /// <summary>
    /// Represents a campaign entity used to manage and track feedback forms and their statistics.
    /// </summary>
    public class Campaign : BaseEntity
    {
        /// <summary>
        /// Gets or sets the name of the campaign.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the campaign.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end date of the campaign.
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the campaign is inactive.
        /// </summary>
        public bool IsInactive { get; set; } = false;

        /// <summary>
        /// Gets or sets the identifier of the associated application user.
        /// </summary>
        public int AppUserId { get; set; }

        /// <summary>
        /// Gets or sets the associated application user.
        /// </summary>
        public AppUser AppUser { get; set; } = default!;

        /// <summary>
        /// Gets or sets the collection of feedback forms associated with the campaign.
        /// </summary>
        public ICollection<FeedbackForm> FeedbackForms { get; set; } = new List<FeedbackForm>();

        /// <summary>
        /// Gets or sets the statistics of the campaign.
        /// </summary>
        public CampaignStatistics Statistics { get; set; } = default!;
    }
}
