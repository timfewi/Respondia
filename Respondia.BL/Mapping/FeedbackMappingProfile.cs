using AutoMapper;
using Respondia.Core.DTOs.FeedbackDTOs;
using Respondia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.BL.Mapping
{
    public class FeedbackMappingProfile : Profile
    {
        public FeedbackMappingProfile()
        {
            // FeedbackFormCreateDTO -> FeedbackForm
            CreateMap<FeedbackFormCreateDTO, FeedbackForm>()
                .ForMember(dest => dest.FeedbackQuestions, opt => opt.Ignore())
                .ForMember(dest => dest.Campaign, opt => opt.Ignore())
                .ForMember(dest => dest.FeedbackSummery, opt => opt.Ignore());

            // FeedbackForm -> FeedbackFormOverviewDTO
            CreateMap<FeedbackForm, FeedbackFormOverviewDTO>()
                .ForMember(dest => dest.TotalQuestions, opt => opt.MapFrom(src => src.FeedbackQuestions.Count))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.DateCreated));

            // FeedbackForm -> FeedbackFormDetailsDTO
            CreateMap<FeedbackForm, FeedbackFormDetailsDTO>()
               .ForMember(dest => dest.Summery, opt => opt.MapFrom(src => src.FeedbackSummery))
               .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.FeedbackQuestions));

            // FeedbackQuestionCreateDTO -> FeedbackQuestion
            CreateMap<FeedbackQuestionCreateDTO, FeedbackQuestion>()
                .ForMember(dest => dest.FeedbackForm, opt => opt.Ignore())
                .ForMember(dest => dest.FeedbackAnswers, opt => opt.Ignore());

            // FeedbackSummery -> FeedbackSummeryDTO
            CreateMap<FeedbackSummery, FeedbackSummeryDTO>();

            // FeedbackQuestion -> FeedbackQuestionDTO
            CreateMap<FeedbackQuestion, FeedbackQuestionDTO>();
        }
    }
}
