using Respondia.Core.DTOs.CampaignDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Interfaces.Services
{
  
/// <summary>
    /// Service interface for campaign write operations.
    /// </summary>
    public interface ICampaignCommandService
    {
        /// <summary>
        /// Creates a new campaign.
        /// </summary>
        /// <param name="dto">The data transfer object containing campaign details.</param>
        /// <param name="userId">The ID of the user creating the campaign.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the created campaign.</returns>
        Task<int> CreateCampaignAsync(CampaignCreateDTO dto, int userId);

        //Task UpdateCampaignAsync(CampaignUpdateDTO dto, int userId);

        Task SoftDeleteCampaignAsync(int id, int userId);
    }
}
