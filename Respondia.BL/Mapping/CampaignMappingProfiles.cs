using AutoMapper;
using Respondia.Core.DTOs.CampaignDTOs;
using Respondia.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.BL.Mapping
{
    /// <summary>
    /// Defines the mapping profiles for Campaign related DTOs and entities.
    /// </summary>
    public class CampaignMappingProfiles : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignMappingProfiles"/> class.
        /// Configures the mappings between CampaignCreateDTO and Campaign.
        /// </summary>
        public CampaignMappingProfiles()
        {
            // CampaignCreateDTO -> Campaign
            CreateMap<CampaignCreateDTO, Campaign>()
                .ForMember(dest => dest.FeedbackForms, opt => opt.Ignore())
                .ForMember(dest => dest.IsInactive, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.DateCreated, opt => opt.Ignore())
                .ForMember(dest => dest.DateUpdated, opt => opt.Ignore());
        }
    }
}
