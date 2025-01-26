using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Respondia.BL.Validators.FeedbackValidators;
using Respondia.Core.Attributes;
using Respondia.Core.DTOs.FeedbackDTOs;
using Respondia.Core.Entities;
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
    public class FeedbackCommandService : IFeedbackCommandService
    {
        private readonly IFeedbackRepository _feedbackRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<FeedbackFormCreateDTO> _feedbackFormCreateValidator;
        private readonly IValidator<FeedbackQuestionCreateDTO> _feedbackQuestionCreateValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<FeedbackCommandService> _logger;

        public FeedbackCommandService(
            IFeedbackRepository feedbackRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<FeedbackFormCreateDTO> feedbackFormCreateValidator,
            IValidator<FeedbackQuestionCreateDTO> feedbackQuestionCreateValidator,
            ILogger<FeedbackCommandService> logger
            )
        {
            _feedbackRepo = feedbackRepo;
            _unitOfWork = unitOfWork;
            _feedbackFormCreateValidator = feedbackFormCreateValidator;
            _feedbackQuestionCreateValidator = feedbackQuestionCreateValidator;
            _mapper = mapper;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task<int> CreateFeedbackFormAsync(FeedbackFormCreateDTO dto, int userId)
        {
            var validatorResult = _feedbackFormCreateValidator.Validate(dto);
            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            try
            {
                var form = _mapper.Map<FeedbackForm>(dto);
                form.CampaignId = dto.CampaignId;

                await _feedbackRepo.AddAsync(form);
                await _unitOfWork.CompleteAsync(); ;
                _logger.LogInformation("FeedbackForm with Id {FormId} created by user {UserId}.", form.Id, userId);
                return form.Id;
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Error creating feedback form. Method: {Method}", nameof(CreateFeedbackFormAsync));
                throw new ServiceException("Unexpected error while creating feedback form.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating feedback form. Method: {Method}", nameof(CreateFeedbackFormAsync));
                throw new ServiceException("Unexpected error while creating feedback form.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task AddQuestionAsync(int formId, FeedbackQuestionCreateDTO questionDto, int userId)
        {
            var validatorResult = _feedbackQuestionCreateValidator.Validate(questionDto);
            if (!validatorResult.IsValid)
            {
                throw new ValidationException(validatorResult.Errors);
            }

            try
            {
                var form = await _feedbackRepo.GetByIdAsync(formId);
                if (form == null)
                {
                    throw new KeyNotFoundException($"FeedbackForm with ID {formId} not found.");
                }

                var question = _mapper.Map<FeedbackQuestion>(questionDto);
                question.FeedbackFormId = formId;

                await _feedbackRepo.AddQuestionAsync(question);
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation("Question with ID {QuestionId} added to form {FormId} by user {UserId}.", question.Id, formId, userId);
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Error adding question to form. Method: {Method}", nameof(AddQuestionAsync));
                throw new ServiceException("Unexpected error while adding question.", ex);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding question to form. Method: {Method}", nameof(AddQuestionAsync));
                throw new ServiceException("Unexpected error while adding question.", ex);
            }
        }


        /// <inheritdoc/>
        public Task<FeedbackLinkDTO> GenerateFeedbackLinkAsync(int formId, DateTime expiryDate, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
