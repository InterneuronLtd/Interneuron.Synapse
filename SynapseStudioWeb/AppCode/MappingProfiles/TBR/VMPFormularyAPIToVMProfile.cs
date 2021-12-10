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
//using SynapseStudioWeb.DataService.APIModel;
//using SynapseStudioWeb.Helpers;
//using SynapseStudioWeb.Models.MedicinalMgmt;
//using System.Collections.Generic;
//using System.Linq;

//namespace SynapseStudioWeb.AppCode.MappingProfiles
//{
//    public class VMPFormularyAPIToVMProfile : Profile
//    {
//        public VMPFormularyAPIToVMProfile()
//        {
//            #region FormularyDetailAPIModel-VTMFormularyEdit
//            CreateMap<FormularyHeaderAPIModel, VMPFormularyEditModel>()
//               .ForMember(dest => dest.Route, opt => opt.MapFrom(src => GetRoutes(src, TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD)))
//               .ForMember(dest => dest.UnlicensedRoute, opt => opt.MapFrom(src => GetRoutes(src, TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED_CD)))
//               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RecStatusCode))
//               .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.FormularyIngredients));

//            CreateMap<FormularyDetailAPIModel, VMPFormularyEditModel>()
//                .ForMember(dest => dest.ContraIndications, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.ContraIndications, "fdb")))
//                .ForMember(dest => dest.Cautions, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.Cautions, "fdb")))
//                .ForMember(dest => dest.LicensedUse, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.LicensedUses, "fdb")))
//                .ForMember(dest => dest.UnlicensedUse, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.UnLicensedUses, "fdb")))
//                .ForMember(dest => dest.SideEffects, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.SideEffects, "fdb")))
//                .ForMember(dest => dest.SafetyMessages, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.SafetyMessages, "fdb")))
//                .ForMember(dest => dest.CustomWarnings, opt => opt.MapFrom(src => src.CustomWarnings))
//                .ForMember(dest => dest.Endorsements, opt => opt.MapFrom(src => src.Endorsements))


//                .ForMember(dest => dest.ControlledDrugCategory, opt => opt.MapFrom(src => src.ControlledDrugCategoryCd))
//                .ForMember(dest => dest.TitrationType, opt => opt.MapFrom(src => src.TitrationTypeCd))
//                .ForMember(dest => dest.RoundingFactor, opt => opt.MapFrom(src => src.RoundingFactorCd))

//                .ForMember(dest => dest.NotesForRestriction, opt => opt.MapFrom(src => src.RestrictionNote))
//                .ForMember(dest => dest.MaximumDoseNumerator, opt => opt.MapFrom(src => src.MaxDoseNumerator))

//                .ForMember(dest => dest.Prescribable, opt => opt.MapFrom(src => src.Prescribable))
//                .ForMember(dest => dest.FormularyStatus, opt => opt.MapFrom(src => src.RnohFormularyStatuscd))


//                .ForMember(dest => dest.BlackTriangle, opt => opt.MapFrom(src => CheckParse(src.BlackTriangle)))
//                .ForMember(dest => dest.CriticalDrug, opt => opt.MapFrom(src => CheckParse(src.CriticalDrug)))
//                .ForMember(dest => dest.Cytotoxic, opt => opt.MapFrom(src => CheckParse(src.Cytotoxic)))
//                .ForMember(dest => dest.HighAlertMedication, opt => opt.MapFrom(src => CheckParse(src.HighAlertMedication)))
//                .ForMember(dest => dest.IVToOral, opt => opt.MapFrom(src => CheckParse(src.IvToOral)))
//                .ForMember(dest => dest.IgnoreDuplicateWarnings, opt => opt.MapFrom(src => CheckParse(src.IgnoreDuplicateWarnings)))
//                .ForMember(dest => dest.WitnessingRequired, opt => opt.MapFrom(src => CheckParse(src.WitnessingRequired)))


//                .ForMember(dest => dest.OutpatientMedication, opt => opt.MapFrom(src => src.OutpatientMedicationCd))
//                .ForMember(dest => dest.Antibiotic, opt => opt.MapFrom(src => CheckParse(src.Antibiotic)))
//                .ForMember(dest => dest.Anticoagulant, opt => opt.MapFrom(src => CheckParse(src.Anticoagulant)))
//                .ForMember(dest => dest.Antipsychotic, opt => opt.MapFrom(src => CheckParse(src.Antipsychotic)))
//                .ForMember(dest => dest.Antimicrobial, opt => opt.MapFrom(src => CheckParse(src.Antimicrobial)))

//                .ForMember(dest => dest.BasisOfPreferredName, opt => opt.MapFrom(src => src.BasisOfPreferredNameCd))
//                .ForMember(dest => dest.FormCd, opt => opt.MapFrom(src => src.FormCd.ConvertToCodeNameModel(item => item, item => item)))
//                .ForMember(dest => dest.DoseForm, opt => opt.MapFrom(src => src.DoseFormCd))
//                .ForMember(dest => dest.UnitDoseFormSize, opt => opt.MapFrom(src => src.UnitDoseFormSize))
//                .ForMember(dest => dest.UnitDoseFormUnits, opt => opt.MapFrom(src => src.UnitDoseFormUnits.ConvertToCodeNameModel(item => item, item => item)))
//                .ForMember(dest => dest.UnitDoseUnitOfMeasure, opt => opt.MapFrom(src => src.UnitDoseUnitOfMeasureCd.ConvertToCodeNameModel(item => item, item => item)))
//                .ForMember(dest => dest.GlutenFree, opt => opt.MapFrom(src => CheckParse(src.GlutenFree)))
//                .ForMember(dest => dest.PreservativeFree, opt => opt.MapFrom(src => CheckParse(src.PreservativeFree)))
//                .ForMember(dest => dest.SugarFree, opt => opt.MapFrom(src => CheckParse(src.SugarFree)))
//                .ForMember(dest => dest.CFCFree, opt => opt.MapFrom(src => CheckParse(src.CfcFree)))
//                .ForMember(dest => dest.PrescribingStatus, opt => opt.MapFrom(src => src.PrescribingStatusCd));
               

//            #endregion FormularyDetailAPIModel-VTMFormularyEdit
//        }

//        private List<CodeNameSelectorModel> ConvertCodeDescAPIToCodeNameModelWithDefaults(List<FormularyLookupAPIModel> apiModel, string readOnlySource)
//        {
//            if (apiModel == null || !apiModel.IsCollectionValid()) return null;

//            var codeNameList = apiModel.Select(rec => rec.ConvertToCodeNameModel((item) => item.Cd, item => item.Desc, item => item.Source, item =>
//            {
//                return (item.Source.IsNotEmpty() && string.Compare(item.Source, readOnlySource, true) == 0);
//            })).ToList();

//            return codeNameList;
//        }

//        private List<CodeNameSelectorModel> GetRoutes(FormularyHeaderAPIModel src, string routeFieldType_Normal_Cd)
//        {
//            if (src == null || !src.FormularyRouteDetails.IsCollectionValid()) return null;

//            return src.FormularyRouteDetails
//                .Where(rec => rec.RouteFieldTypeCd == routeFieldType_Normal_Cd)
//                .Select(rec => rec.ConvertToCodeNameModel(val => val.RouteCd, val => val.RouteCd, val => val.Source)).ToList();
//        }

//        public bool CheckParse(string value)
//        {
//            if (value.IsEmpty() || value == "null" || value == "0")
//            {
//                return false;
//            }
//            else
//            {
//                return true;
//            }

//        }
//    }
//}
