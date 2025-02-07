using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Respondia.Core.Attributes;
using Respondia.Core.DTOs.FeedbackDTOs;
using Respondia.Core.Exceptions;
using Respondia.Core.Interfaces.Repositories;
using Respondia.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.BL.Services.FeedbackServices
{
    [Injectable(ServiceLifetime.Scoped)]
    /// <inheritdoc/>
    public class FeedbackQueryService : IFeedbackQueryService
    {
        private readonly IFeedbackRepository _feedbackRepo;
        private readonly ILogger<FeedbackQueryService> _logger;
        private readonly IMapper _mapper;

        public FeedbackQueryService(IFeedbackRepository feedbackRepo, ILogger<FeedbackQueryService> logger, IMapper mapper)
        {
            _feedbackRepo = feedbackRepo;
            _logger = logger;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<FeedbackFormOverviewDTO>> GetFeedbackFormsAsync(int campaignId)
        {
            try
            {
                var forms = await _feedbackRepo.GetFeedbackFormsByCampaignAsync(campaignId);
                return forms.Select(f => new FeedbackFormOverviewDTO
                {
                    Id = f.Id,
                    Name = f.Name,
                    TotalQuestions = f.FeedbackQuestions.Count,
                    DateCreated = f.DateCreated
                }).ToList();
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Data access error in {Method}.", nameof(GetFeedbackFormsAsync));
                throw new ServiceException("Error retrieving feedback form from campaign.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Method}", nameof(GetFeedbackFormsAsync));
                throw new ServiceException("Unexpected error while retrieving feedback forms from campaign.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<FeedbackFormDetailsDTO?> GetFeedbackFormDetailsAsync(int formId)
        {
            try
            {
                var form = await _feedbackRepo.GetFormWithDetailsAsync(formId);
                if (form == null)
                    return null;

                return _mapper.Map<FeedbackFormDetailsDTO>(form);
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Data access error in {Method}.", nameof(GetFeedbackFormDetailsAsync));
                throw new ServiceException("Error retrieving feedback form details.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in {Method}", nameof(GetFeedbackFormDetailsAsync));
                throw new ServiceException("Unexpected error while retrieving feedback form details.", ex);
            }

        }

        /// <inheritdoc/>
        public Task<FeedbackAnalysisDTO?> GetFeedbackAnalysisAsync(int formId)
        {
            throw new NotImplementedException();
        }

        
        /// <inheritdoc/>
        public async Task<PublicFeedbackFormDTO> GetFeedBackFormByTokenAsync(string token)
        {
            var feedbackLink = await _feedbackRepo.GetFeedbackLinkByTokenAsync(token);
            if (feedbackLink == null)
            {
                throw new KeyNotFoundException($"{nameof(GetFeedBackFormByTokenAsync)} - Link not found.");
            }

            if (feedbackLink.ExpiryDate < DateTime.Now || !feedbackLink.IsActive)
            {
                throw new ServiceException("Token expired.");
            }

            var form = await _feedbackRepo.GetByIdAsync(feedbackLink.FeedbackFormId);
            if (form == null)
            {
                throw new KeyNotFoundException("Feedback form not found.");
            }

            var question = await _feedbackRepo.GetQuestionsByFeedbackFormIdAsync(form.Id);

            var dto = new PublicFeedbackFormDTO
            {
                Id = form.Id,
                Name = form.Name,
                Description = form.Description,
                IsActive = feedbackLink.IsActive,
                ExpiryDate = feedbackLink.ExpiryDate,
                Questions = question.Select(q => new FeedbackQuestionDTO
                {
                    Id = q.Id,
                    QuestionText = q.QuestionText,
                    QuestionType = q.QuestionType
                })
                .ToList()
            };
            return dto;
        }
    }
}
