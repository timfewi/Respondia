using Respondia.Core.DTOs.CampaignDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Interfaces.Services
{
    /// <summary>
    /// Provides methods for querying campaign data.
    /// </summary>
    public interface ICampaignQueryService
    {
        /// <summary>
        /// Retrieves a list of campaigns for overview purposes.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains a collection of <see cref="CampaignListDTO"/>.</returns>
        Task<IEnumerable<CampaignListDTO>> GetCampaignsForOverviewAsync();

        /// <summary>
        /// Retrieves the details of a specific campaign by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the campaign.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="CampaignDetailsDTO"/> if found; otherwise, null.</returns>
        Task<CampaignDetailsDTO?> GetCampaignDetailsAsync(int id);
    }
}
