 //Interneuron synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
﻿using AutoMapper;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.SNOMED;
using Interneuron.Terminology.Model.DomainModels;
using Interneuron.Terminology.Model.Search;

namespace Interneuron.Terminology.API.AppCode.AutomapperProfiles
{
    public class SNOMEDMapperProfile : Profile
    {
        public SNOMEDMapperProfile()
        {

            CreateMap<SNOMEDCTSearchResultModel, SnomedSearchResultDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ConceptId))
                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.PreferredTerm))
                .ForMember(dest => dest.FSN, opt => opt.MapFrom(src => src.FSN))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.GetValueOrDefault()));

            CreateMap<SNOMEDCTSearchResultModel, SnomedSearchResultWithTreeDTO>()
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.ConceptId))
                .ForMember(dest => dest.Term, opt => opt.MapFrom(src => src.PreferredTerm))
                .ForMember(dest => dest.FSN, opt => opt.MapFrom(src => src.FSN))
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.GetValueOrDefault()));

            CreateMap<SnomedctConceptLookupMat, SnomedIndicationLookupDTO>()
                .ForMember(dest => dest.Cd, opt => opt.MapFrom(src => src.Conceptid))
                .ForMember(dest => dest.Desc, opt => opt.MapFrom(src => src.Preferredterm))
                .ForMember(dest => dest.Recordstatus, opt => opt.MapFrom(src => 1));//Active only


            CreateMap<SnomedctTradefamiliesMat, SnomedTradeFamiliesDTO>();

            CreateMap<SnomedctModifiedReleaseMat, SnomedModifiedReleaseDTO>();

        }
    }
}

