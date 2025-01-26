using FluentValidation;
using Respondia.Core.DTOs.FeedbackDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.BL.Validators.FeedbackValidators
{
    public class FeedbackFormCreateDTOValidator : AbstractValidator<FeedbackFormCreateDTO>
    {
        public FeedbackFormCreateDTOValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("FeedbackForm name must not be empty.")
                .MaximumLength(200).WithMessage("FeedbackForm name must be less than 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description must be less than 1000 characters.");
        }
    }
}
