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
using Interneuron.Common.Extensions;
using SynapseStudioWeb.DataService.APIModel.Requests;
using SynapseStudioWeb.Models.MedicationMgmt;
using SynapseStudioWeb.Models.MedicinalMgmt;
using System.Collections.Generic;
using System.Linq;

namespace SynapseStudioWeb.AppCode.MappingProfiles
{
    public class FormularyCommonVMToAPIProfile : Profile
    {
        public FormularyCommonVMToAPIProfile()
        {
            //CreateMap<FormularyAdditionalCodeModel, CreateFormularyAdditionalCodeAPIRequest>();

            //CreateMap<FormularyIngredientModel, CreateFormularyIngredientAPIRequest>()
            //    .ForMember(dest => dest.BasisOfPharmaceuticalStrengthCd, opt => opt.MapFrom(src => src.BasisOfPharmaceuticalStrength))
            //    .ForMember(dest => dest.IngredientCd, opt => opt.MapFrom(src => src.Ingredient.Id))
            //    .ForMember(dest => dest.StrengthValueDenominatorUnitCd, opt => opt.MapFrom(src => src.StrengthValueDenominatorUnit.Id))
            //    .ForMember(dest => dest.StrengthValueNumeratorUnitCd, opt => opt.MapFrom(src => src.StrengthValueNumeratorUnit.Id))
            //    .ForMember(dest => dest.StrengthValueNumerator, opt => opt.MapFrom(src => src.StrengthValNumerator))
            //    .ForMember(dest => dest.StrengthValueDenominator, opt => opt.MapFrom(src => src.StrengthValDenominator));

            //CreateMap<CodeNameSelectorModel, CreateFormularyIndicationAPIRequest>()
            //    .ForMember(dest => dest.IndicationCd, opt => opt.MapFrom(src => src.Id))
            //    .ForMember(dest => dest.IndicationName, opt => opt.MapFrom(src => src.Name));

            //CreateMap<FormularyCustomWarningModel, CustomWarningAPIRequest>();

            CreateMap<BulkFormularyEditModel, BulkFormularyEditModel>();

            CreateMap<CodeNameSelectorModel, FormularyLookupItemAPIRequest>()
                .ForMember(dest => dest.Cd, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Desc, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source));
        }

        protected List<string> ConvertValueToString(List<CodeNameSelectorModel> src)
        {
            if (!src.IsCollectionValid())
            {
                return null;
            }

            return src.Select(x => x.Id).ToList();
        }
    }
}
