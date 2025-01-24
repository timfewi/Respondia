using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.DTOs.CampaignDTOs
{
    /// <summary>
    /// Data Transfer Object for representing a campaign in a list.
    /// This DTO is used only for read operations.
    /// </summary>
    public class CampaignListDTO
    {
        /// <summary>
        /// Gets the unique identifier of the campaign.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the name of the campaign.
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// Gets the description of the campaign.
        /// </summary>
        public string Description { get; } = string.Empty;

        /// <summary>
        /// Gets the end date of the campaign, if any.
        /// </summary>
        public DateTime? EndDate { get; }

        /// <summary>
        /// Gets a value indicating whether the campaign is inactive.
        /// </summary>
        public bool IsInactive { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignListDTO"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the campaign.</param>
        /// <param name="name">The name of the campaign.</param>
        /// <param name="description">The description of the campaign.</param>
        /// <param name="endDate">The end date of the campaign, if any.</param>
        /// <param name="isInactive">A value indicating whether the campaign is inactive.</param>
        public CampaignListDTO(int id, string name, string description, DateTime? endDate, bool isInactive)
        {
            Id = id;
            Name = name;
            Description = description;
            EndDate = endDate;
            IsInactive = isInactive;
        }
    }
}
