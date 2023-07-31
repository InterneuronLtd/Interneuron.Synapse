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
﻿//using AutoMapper;
//using Interneuron.Common.Extensions;
//using SynapseStudioWeb.AppCode.Constants;
//using SynapseStudioWeb.DataService.APIModel;
//using SynapseStudioWeb.Helpers;
//using SynapseStudioWeb.Models.MedicinalMgmt;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SynapseStudioWeb.AppCode.MappingProfiles
//{
//    public class VTMFormularyAPIToVMProfile: Profile
//    {
//        public VTMFormularyAPIToVMProfile()
//        { 
//            #region FormularyDetailAPIModel-VTMFormularyEdit

//            CreateMap<FormularyHeaderAPIModel, VTMFormularyEditModel>()
//               .ForMember(dest => dest.Route, opt => opt.MapFrom(src => GetRoutes(src, TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD)))
//               .ForMember(dest => dest.UnlicensedRoute, opt => opt.MapFrom(src => GetRoutes(src, TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED_CD)))
//               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RecStatusCode));

//            CreateMap<FormularyDetailAPIModel, VTMFormularyEditModel>()
//                .ForMember(dest => dest.ContraIndications, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.ContraIndications, "fdb")))
//                .ForMember(dest => dest.Cautions, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.Cautions, "fdb")))
//                .ForMember(dest => dest.LicensedUse, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.LicensedUses, "fdb")))
//                .ForMember(dest => dest.UnlicensedUse, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.UnLicensedUses, "fdb")))
//                .ForMember(dest => dest.SideEffects, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.SideEffects, "fdb")))
//                .ForMember(dest => dest.SafetyMessages, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.SafetyMessages, "fdb")))
//                .ForMember(dest => dest.CustomWarnings, opt => opt.MapFrom(src => src.CustomWarnings))
//                .ForMember(dest => dest.Endorsements, opt => opt.MapFrom(src => src.Endorsements))

//                .ForMember(dest => dest.UnlicensedMedication, opt => opt.MapFrom(src => src.UnlicensedMedicationCd))
//                .ForMember(dest => dest.ControlledDrugCategory, opt => opt.MapFrom(src => src.ControlledDrugCategoryCd))
//                .ForMember(dest => dest.TitrationType, opt => opt.MapFrom(src => src.TitrationTypeCd))

//                .ForMember(dest => dest.NotesForRestriction, opt => opt.MapFrom(src => src.RestrictionNote))
//                .ForMember(dest => dest.MaximumDoseNumerator, opt => opt.MapFrom(src => src.MaxDoseNumerator))

//                .ForMember(dest => dest.Prescribable, opt => opt.MapFrom(src => src.Prescribable))
//                .ForMember(dest => dest.FormularyStatus, opt => opt.MapFrom(src => src.RnohFormularyStatuscd))

//                .ForMember(dest => dest.ControlledDrugCategory, opt => opt.MapFrom(src => src.ControlledDrugCategoryCd))

//                .ForMember(dest => dest.BlackTriangle, opt => opt.MapFrom(src => CheckParse(src.BlackTriangle)))
//                .ForMember(dest => dest.ClinicalTrialMedication, opt => opt.MapFrom(src => CheckParse(src.ClinicalTrialMedication)))
//                .ForMember(dest => dest.CriticalDrug, opt => opt.MapFrom(src => CheckParse(src.CriticalDrug)))
//                .ForMember(dest => dest.Cytotoxic, opt => opt.MapFrom(src => CheckParse(src.Cytotoxic)))
//                .ForMember(dest => dest.HighAlertMedication, opt => opt.MapFrom(src => CheckParse(src.HighAlertMedication)))
//                .ForMember(dest => dest.IVToOral, opt => opt.MapFrom(src => CheckParse(src.IvToOral)))
//                .ForMember(dest => dest.UnlicensedMedication, opt => opt.MapFrom(src => CheckParse(src.UnlicensedMedicationCd)))
//                .ForMember(dest => dest.IgnoreDuplicateWarnings, opt => opt.MapFrom(src => CheckParse(src.IgnoreDuplicateWarnings)))
//                .ForMember(dest => dest.WitnessingRequired, opt => opt.MapFrom(src => CheckParse(src.WitnessingRequired)));

//            #endregion FormularyDetailAPIModel-VTMFormularyEdit
//        }
        

//        private List<CodeNameSelectorModel> ConvertCodeDescAPIToCodeNameModelWithDefaults(List<CodeDescAPIModel> apiModel, string readOnlySource)
//        {
//            if (apiModel == null || !apiModel.IsCollectionValid()) return null;

//            var codeNameList = apiModel.Select(rec => rec.ConvertToCodeNameModel((item) => item.Cd, item => item.Desc, item => item.Source, item =>
//            {
//                return (item.Source.IsNotEmpty() && string.Compare(item.Source, readOnlySource, true) == 0);
//            })).ToList();

//            return codeNameList;
//        }

//        private List<CodeNameSelectorModel> GetRoutes(FormularyHeaderAPIModel src, string routeFieldType_Cd)
//        {
//            if (src == null || !src.FormularyRouteDetails.IsCollectionValid()) return null;

//            return src.FormularyRouteDetails
//                .Where(rec => rec.RouteFieldTypeCd == routeFieldType_Cd)
//                .Select(rec => rec.ConvertToCodeNameModel(val => val.RouteCd, val => val.RouteCd, val => val.Source)).ToList();
//        }

//        public bool CheckParse(string value)
//        {
//            return (value.IsEmpty() || value == "null" || value == "0")?false:true;

//        }
//    }
//}
