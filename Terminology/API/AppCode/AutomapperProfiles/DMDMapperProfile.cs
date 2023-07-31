//Interneuron Synapse

//Copyright(C) 2021  Interneuron CIC

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
using Interneuron.Terminology.API.AppCode.DTOs.DMD;
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.Model.DomainModels;
using Interneuron.Terminology.Model.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.AutomapperProfiles
{

    public class DMDMapperProfile : Profile
    {
        public DMDMapperProfile()
        {
            CreateMap<DMDSearchResultModel, DMDSearchResultDTO>()
               .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LogicalLevel.GetDMDLevelCodeByLogicalLevel()));

            CreateMap<DMDSearchResultModel, DMDSearchResultWithTreeDTO>()
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LogicalLevel.GetDMDLevelCodeByLogicalLevel()));

            CreateMap<DmdLookupRoute, DmdLookupRouteDTO>()
                .ForMember(dest => dest.Cd, opt => opt.MapFrom(src => src.Cd))
                .ForMember(dest => dest.Cdprev, opt => opt.MapFrom(src => src.Cdprev));


            CreateMap<DmdLookupForm, DmdLookupFormDTO>()
                .ForMember(dest => dest.Cd, opt => opt.MapFrom(src => src.Cd))
                .ForMember(dest => dest.Cdprev, opt => opt.MapFrom(src => src.Cdprev));

            CreateMap<DmdLookupPrescribingstatus, DmdLookupPrescribingstatusDTO>()
                .ForMember(dest => dest.Cd, opt => opt.MapFrom(src => src.Cd));

            CreateMap<DmdLookupControldrugcat, DmdLookupControldrugcatDTO>()
                .ForMember(dest => dest.Cd, opt => opt.MapFrom(src => src.Cd));

            CreateMap<DmdLookupSupplier, DmdLookupSupplierDTO>()
                .ForMember(dest => dest.Cd, opt => opt.MapFrom(src => src.Cd))
                .ForMember(dest => dest.Cdprev, opt => opt.MapFrom(src => src.Cdprev));

            CreateMap<DmdLookupAvailrestrict, DmdLookupAvailrestrictDTO>()
                .ForMember(dest => dest.Cd, opt => opt.MapFrom(src => src.Cd));

            CreateMap<DmdLookupBasisofname, DmdLookupBasisofnameDTO>();

            CreateMap<DmdLookupDrugformind, DmdLookupDrugformindDTO>();

            CreateMap<DmdLookupLicauth, DmdLookupLicauthDTO>();

            CreateMap<DmdLookupOntformroute, DmdLookupOntformrouteDTO>();

            CreateMap<DmdLookupUom, DmdLookupUomDTO>();

            CreateMap<DmdLookupIngredient, DmdLookupIngredientDTO>();

            CreateMap<DmdLookupBasisofstrength, DmdLookupBasisofstrengthDTO>();

            CreateMap<DmdVmpIngredient, DmdVmpIngredientDTO>();

            CreateMap<DmdAmpExcipient, DmdAmpExcipientDTO>();

            CreateMap<DmdAmpDrugroute, DmdAmpDrugrouteDTO>();

            CreateMap<DmdVmpDrugroute, DmdVmpDrugrouteDTO>();

            CreateMap<DMDDetailResultModel, DMDDetailResultDTO>();


            CreateMap<AtcLookup, AtcLookupDTO>();

            CreateMap<BnfLookup, DmdLookupBNFDTO>()
                .ForMember(dest => dest.Desc, opt => opt.MapFrom(src => src.Name));

            CreateMap<DmdAtc, DmdATCCodeDTO>()
                .ForMember(dest => dest.Cd, opt => opt.MapFrom(src => src.AtcCd))
                .ForMember(dest => dest.ShortCd, opt => opt.MapFrom(src => src.AtcShortCd))
                .ForMember(dest => dest.DmdCd, opt => opt.MapFrom(src => src.DmdCd));


            CreateMap<DmdBnf, DmdBNFCodeDTO>()
                .ForMember(dest => dest.Cd, opt => opt.MapFrom(src => src.BnfCd))
                .ForMember(dest => dest.DmdCd, opt => opt.MapFrom(src => src.DmdCd));
        }
    }
}

