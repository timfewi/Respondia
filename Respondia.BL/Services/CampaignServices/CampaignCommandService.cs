using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Respondia.Core.Attributes;
using Respondia.Core.DTOs.CampaignDTOs;
using Respondia.Core.Entities;
using Respondia.Core.Exceptions;
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
    public class CampaignCommandService : ICampaignCommandService
    {
        private readonly ICampaignRepository _campaignRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CampaignCreateDTO> _campaignCreateValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<CampaignCommandService> _logger;

        public CampaignCommandService(
            ICampaignRepository campaignRepo,
            IUnitOfWork unitOfWork,
            IValidator<CampaignCreateDTO> campaignCreateValidator,
            IMapper mapper,
            ILogger<CampaignCommandService> logger
            )
        {
            _campaignRepo = campaignRepo;
            _unitOfWork = unitOfWork;
            _campaignCreateValidator = campaignCreateValidator;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<int> CreateCampaignAsync(CampaignCreateDTO dto, int userId)
        {
            var validatorResult = _campaignCreateValidator.Validate(dto);
            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            try
            {
                var campaign = _mapper.Map<Campaign>(dto);
                campaign.AppUserId = userId;

                var statistics = new CampaignStatistics
                {
                    Campaign = campaign,
                    AverageRating = 0,
                    TotalResponses = 0
                };

                await _campaignRepo.AddAsync(campaign);
                await _campaignRepo.AddCampaignStatisticsAsync(statistics);

                await _unitOfWork.CompleteAsync();

                return campaign.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in {Method}: {Message}", nameof(CreateCampaignAsync), ex.Message);
                throw new ServiceException($"An unexpected error occurred in {nameof(CreateCampaignAsync)}.", ex);
            }
        }

        /// <inheritdoc/>
        public Task SoftDeleteCampaignAsync(int id, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
