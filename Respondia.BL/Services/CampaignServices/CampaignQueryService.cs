using Microsoft.Extensions.Logging;
using Respondia.Core.Attributes;
using Respondia.Core.DTOs.CampaignDTOs;
using Respondia.Core.DTOs.FeedbackDTOs;
using Respondia.Core.Interfaces.Repositories;
using Respondia.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.BL.Services.CampaignServices
{
    [Injectable(Microsoft.Extensions.DependencyInjection.ServiceLifetime.Scoped)]
    /// <inheritdoc/>
    public class CampaignQueryService : ICampaignQueryService
    {
        private readonly ILogger<CampaignQueryService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICampaignRepository _campaignRepo;

        public CampaignQueryService(
            ILogger<CampaignQueryService> logger,
            IUnitOfWork unitOfWork,
            ICampaignRepository campaignRepo
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _campaignRepo = campaignRepo;
        }    

        /// <inheritdoc/>
        public async Task<IEnumerable<CampaignListDTO>> GetCampaignsForOverviewAsync()
        {
            var campaigns = await _campaignRepo.GetAllAsync();

            return campaigns
                .Select(c => new CampaignListDTO(
                    c.Id,
                    c.Name,
                    c.Description,
                    c.EndDate,
                    c.IsInactive))
                .ToList();
        }

        /// <inheritdoc/>
        public async Task<CampaignDetailsDTO?> GetCampaignDetailsAsync(int campaignId)
        {
            var campaign = await _campaignRepo.GetCampaignWithStatisticsAsync(campaignId);
            if (campaign == null)
                return null;

            var details = new CampaignDetailsDTO
            {
                Id = campaign.Id,
                Name = campaign.Name,
                Description = campaign.Description,
                EndDate = campaign.EndDate,
                IsInactive = campaign.IsInactive,
                Statistics = campaign.Statistics == null ? null : new CampaignStatisticsDTO
                {
                    Id = campaign.Statistics.Id,
                    TotalResponses = campaign.Statistics.TotalResponses,
                    AverageRating = campaign.Statistics.AverageRating,
                    LastUpdated = campaign.Statistics.LastUpdated
                },
                FeedbackForms = campaign.FeedbackForms
                .Select(f => new FeedbackFormShortDTO
                {
                    Id = f.Id,
                    Name = f.Name
                })
                .ToList()
            };

            return details;
        }

    }
}
