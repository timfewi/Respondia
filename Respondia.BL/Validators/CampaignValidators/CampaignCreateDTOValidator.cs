using FluentValidation;
using Respondia.Core.DTOs.CampaignDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.BL.Validators.CampaignValidators
{
    /// <summary>
    /// Validator for CampaignCreateDTO.
    /// Ensures that the campaign name, description, and end date meet specified criteria.
    /// </summary>
    public class CampaignCreateDTOValidator : AbstractValidator<CampaignCreateDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignCreateDTOValidator"/> class.
        /// Defines validation rules for the CampaignCreateDTO properties.
        /// </summary>
        public CampaignCreateDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Campaign name must not be empty.")
                .MaximumLength(200).WithMessage("Campaign name must be less than 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must be less than 1000 characters.");

            RuleFor(x => x.EndDate)
                .GreaterThan(DateTime.Now).WithMessage("EndDate must be in the future.")
                .When(x => x.EndDate.HasValue);
        }
    }
}
