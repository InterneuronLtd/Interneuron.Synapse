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


﻿//using AutoMapper;
//using Interneuron.Common.Extensions;
//using SynapseStudioWeb.AppCode.Constants;
//using SynapseStudioWeb.DataService.APIModel.Requests;
//using SynapseStudioWeb.Models.MedicinalMgmt;
//using System;
//using System.Collections.Generic;

//namespace SynapseStudioWeb.AppCode.MappingProfiles
//{
//    public class VTMFormularyProfile : Profile
//    {
//        public VTMFormularyProfile()
//        {
//            CreateMap<VTMFormularyEditModel, CreateFormularyAPIRequest>()
//                .ForMember(dest => dest.VtmId, opt => opt.MapFrom(src => src.Code))
//                .ForMember(dest => dest.RecStatusCode, opt => opt.MapFrom(src => src.Status))
//                .ForMember(dest => dest.FormularyRouteDetails, opt => opt.MapFrom(src => ConvertRouteModelToAPIModel(src)))
//                .ForMember(dest => dest.FormularyAdditionalCodes, opt => opt.MapFrom(src => ConvertAdditionalCodesModelToAPIModel(src)))

//                .ForMember(dest => dest.RecSource, opt => opt.MapFrom(src => src.RecSource));

//            CreateMap<VTMFormularyEditModel, CreateFormularyDetailAPIRequest>()
//                .ForMember(dest => dest.RnohFormularyStatuscd, opt => opt.MapFrom(src => src.FormularyStatus))
//                .ForMember(dest => dest.Prescribable, opt => opt.MapFrom(src => src.Prescribable))
//                .ForMember(dest => dest.UnlicensedMedicationCd, opt => opt.MapFrom(src => src.UnlicensedMedication == true ? "1" : null))
//                .ForMember(dest => dest.HighAlertMedication, opt => opt.MapFrom(src => src.HighAlertMedication == true ? "1" : null))
//                .ForMember(dest => dest.IgnoreDuplicateWarnings, opt => opt.MapFrom(src => src.IgnoreDuplicateWarnings == true ? "1" : null))
//                .ForMember(dest => dest.CriticalDrug, opt => opt.MapFrom(src => src.CriticalDrug == true ? "1" : null))
//                .ForMember(dest => dest.ControlledDrugCategoryCd, opt => opt.MapFrom(src => src.ControlledDrugCategory))
//                .ForMember(dest => dest.Cytotoxic, opt => opt.MapFrom(src => src.Cytotoxic == true ? "1" : null))
//                .ForMember(dest => dest.ClinicalTrialMedication, opt => opt.MapFrom(src => src.ClinicalTrialMedication == true ? "1" : null))
//                .ForMember(dest => dest.IvToOral, opt => opt.MapFrom(src => src.IVToOral == true ? "1" : null))
//                .ForMember(dest => dest.TitrationTypeCd, opt => opt.MapFrom(src => src.TitrationType))
//                .ForMember(dest => dest.MaxDoseNumerator, opt => opt.MapFrom(src => CheckForDecimal(src.MaximumDoseNumerator)))
//                .ForMember(dest => dest.WitnessingRequired, opt => opt.MapFrom(src => src.WitnessingRequired == true ? "1" : null))
//                .ForMember(dest => dest.RestrictionNote, opt => opt.MapFrom(src => src.NotesForRestriction))
//                .ForMember(dest => dest.BlackTriangle, opt => opt.MapFrom(src => src.BlackTriangle == true ? "1" : null))
//                .ForMember(dest => dest.SideEffects, opt => opt.MapFrom(src => src.SideEffects))
//                .ForMember(dest => dest.Cautions, opt => opt.MapFrom(src => src.Cautions))
//                .ForMember(dest => dest.SafetyMessages, opt => opt.MapFrom(src => src.SafetyMessages))
//                .ForMember(dest => dest.CustomWarnings, opt => opt.MapFrom(src => src.CustomWarnings))
//                .ForMember(dest => dest.Endorsements, opt => opt.MapFrom(src => src.Endorsements))
//                .ForMember(dest => dest.ContraIndications, opt => opt.MapFrom(src => src.ContraIndications))
//                .ForMember(dest => dest.LicensedUses, opt => opt.MapFrom(src => src.LicensedUse))
//                .ForMember(dest => dest.UnLicensedUses, opt => opt.MapFrom(src => src.UnlicensedUse))
//                .ForMember(dest => dest.IsBloodProduct, opt => opt.MapFrom(src => src.IsBloodProduct))
//                .ForMember(dest => dest.IsDiluent, opt => opt.MapFrom(src => src.IsDiluent));
//        }

//        private List<CreateFormularyAdditionalCodeAPIRequest> ConvertAdditionalCodesModelToAPIModel(VTMFormularyEditModel src)
//        {
//            var addlCodes = new List<CreateFormularyAdditionalCodeAPIRequest>();

//            if (src.FormularyClassificationCodes.IsCollectionValid())
//            {
//                src.FormularyClassificationCodes.Each(ar =>
//                {
//                    addlCodes.Add(new CreateFormularyAdditionalCodeAPIRequest { AdditionalCode = ar.AdditionalCode, AdditionalCodeDesc = ar.AdditionalCodeDesc, AdditionalCodeSystem = ar.AdditionalCodeSystem, Attr1=ar.Attr1, CodeType=ar.CodeType?? TerminologyConstants.CLASSIFICATION_CODE_TYPE, MetaJson= ar.MetaJson,  Source = ar.Source?? TerminologyConstants.MANUAL_DATA_SOURCE });
//                });
//            }

//            if (src.FormularyIdentificationCodes.IsCollectionValid())
//            {
//                src.FormularyIdentificationCodes.Each(ar =>
//                {
//                    addlCodes.Add(new CreateFormularyAdditionalCodeAPIRequest { AdditionalCode = ar.AdditionalCode, AdditionalCodeDesc = ar.AdditionalCodeDesc, AdditionalCodeSystem = ar.AdditionalCodeSystem, Attr1 = ar.Attr1, CodeType = ar.CodeType ?? TerminologyConstants.IDENTIFICATION_CODE_TYPE, MetaJson = ar.MetaJson, Source = ar.Source ?? TerminologyConstants.MANUAL_DATA_SOURCE });
//                });
//            }

//            return addlCodes;
//        }

//        protected List<CreateFormularyRouteDetailAPIRequest> ConvertRouteModelToAPIModel(VTMFormularyEditModel src)
//        {
//            var formularyRoutes = new List<CreateFormularyRouteDetailAPIRequest>();

//            if (src.Route.IsCollectionValid())
//            {
//                src.Route.Each(ar =>
//                {
//                    formularyRoutes.Add(new CreateFormularyRouteDetailAPIRequest { RouteCd = ar.Id, RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD, Source = ar.Source });
//                });
//            }

//            if (src.UnlicensedRoute.IsCollectionValid())
//            {
//                src.UnlicensedRoute.Each(ar =>
//                {
//                    formularyRoutes.Add(new CreateFormularyRouteDetailAPIRequest { RouteCd = ar.Id, RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED_CD, Source = ar.Source });
//                });
//            }

//            return formularyRoutes;
//        }

//        protected decimal? CheckForDecimal(string src)
//        {
//            return src.IsEmpty() ? default(decimal?) : Convert.ToDecimal(src);
//        }
//    }
//}
