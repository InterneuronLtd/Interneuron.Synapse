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
using Microsoft.AspNetCore.Http;
using SynapseStudioWeb.AppCode.Constants;
using SynapseStudioWeb.DataService.APIModel;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models.MedicationMgmt;
using SynapseStudioWeb.Models.MedicinalMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.AppCode.MappingProfiles.MedicationManagement
{
    public class FormularyAPIToFormularyCreateVMProfile : Profile
    {
        public FormularyAPIToFormularyCreateVMProfile()
        {
            CreateMap<FormularyHeaderAPIModel, FormularyCreateModel>()
                .ForMember(dest => dest.Route, opt => opt.MapFrom(src => GetRoutes(src, TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD)))
                .ForMember(dest => dest.UnlicensedRoute, opt => opt.MapFrom(src => GetRoutes(src, TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED_CD)))
                .ForMember(dest => dest.LocalLicensedRoute, opt => opt.MapFrom(src => GetLocalRoutes(src, TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD)))
                .ForMember(dest => dest.LocalUnlicensedRoute, opt => opt.MapFrom(src => GetLocalRoutes(src, TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED_CD)))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RecStatusCode))
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.FormularyIngredients))
                .ForMember(dest => dest.Excipients, opt => opt.MapFrom(src => src.FormularyExcipients))
                .ForMember(dest => dest.VirtualMedicinalProduct, opt => opt.MapFrom(src => src.VmpId))
                .ForMember(dest => dest.VirtualTherapeuticMoiety, opt => opt.MapFrom(src => src.VtmId))
                .ForMember(dest => dest.FormularyIdentificationCodes, opt => opt.ConvertUsing(new AdditonalCodeConverter("Identification"), src => src.FormularyAdditionalCodes))
                .ForMember(dest => dest.FormularyClassificationCodes, opt => opt.ConvertUsing(new AdditonalCodeConverter("Classification"), src => src.FormularyAdditionalCodes))
                .AfterMap<FormularyDetailResolveActionForCreate>()
                .AfterMap<OverrideDefaultMapAction>()
                .AfterMap<NameResolveActionForCreate>();

            CreateMap<FormularyDetailAPIModel, FormularyCreateModel>()
                .ForMember(dest => dest.ContraIndications, opt => opt.MapFrom(src => ConvertLookupAPIListToCodeNameModelWithDefaultsForCreate(src.ContraIndications)))
                .ForMember(dest => dest.Cautions, opt => opt.MapFrom(src => ConvertLookupAPIListToCodeNameModelWithDefaultsForCreate(src.Cautions)))
                .ForMember(dest => dest.LocalLicensedUse, opt => opt.MapFrom(src => ConvertLookupAPIListToCodeNameModelWithDefaultsForCreate(src.LocalLicensedUses)))
                .ForMember(dest => dest.LocalUnlicensedUse, opt => opt.MapFrom(src => ConvertLookupAPIListToCodeNameModelWithDefaultsForCreate(src.LocalUnLicensedUses)))
                .ForMember(dest => dest.LicensedUse, opt => opt.MapFrom(src => ConvertLookupAPIListToCodeNameModelWithDefaultsForCreate(src.LicensedUses)))
                .ForMember(dest => dest.UnlicensedUse, opt => opt.MapFrom(src => ConvertLookupAPIListToCodeNameModelWithDefaultsForCreate(src.UnLicensedUses)))
                .ForMember(dest => dest.SideEffects, opt => opt.MapFrom(src => ConvertLookupAPIListToCodeNameModelWithDefaultsForCreate(src.SideEffects)))
                .ForMember(dest => dest.SafetyMessages, opt => opt.MapFrom(src => ConvertLookupAPIListToCodeNameModelWithDefaultsForCreate(src.SafetyMessages)))
                .ForMember(dest => dest.CustomWarnings, opt => opt.MapFrom(src => src.CustomWarnings))
                .ForMember(dest => dest.Reminders, opt => opt.MapFrom(src => src.Reminders))
                .ForMember(dest => dest.Endorsements, opt => opt.MapFrom(src => src.Endorsements))


                .ForMember(dest => dest.BlackTriangle, opt => opt.MapFrom(src => CheckParse(src.BlackTriangle)))
                .ForMember(dest => dest.CriticalDrug, opt => opt.MapFrom(src => CheckParse(src.CriticalDrug)))
                .ForMember(dest => dest.HighAlertMedication, opt => opt.MapFrom(src => CheckParse(src.HighAlertMedication)))
                .ForMember(dest => dest.IVToOral, opt => opt.MapFrom(src => CheckParse(src.IvToOral)))
                .ForMember(dest => dest.IgnoreDuplicateWarnings, opt => opt.MapFrom(src => CheckParse(src.IgnoreDuplicateWarnings)))
                .ForMember(dest => dest.WitnessingRequired, opt => opt.MapFrom(src => CheckParse(src.WitnessingRequired)))

                .ForMember(dest => dest.OutpatientMedication, opt => opt.MapFrom(src => CheckParse(src.OutpatientMedicationCd)))

                .ForMember(dest => dest.FormCd, opt => opt.MapFrom(src => ConvertStringFromAPIToCodeNameModelWithDefaultsForCreate(src.FormCd, null)))
                .ForMember(dest => dest.UnitDoseFormSize, opt => opt.MapFrom(src => src.UnitDoseFormSize))
                .ForMember(dest => dest.UnitDoseFormUnits, opt => opt.MapFrom(src => ConvertStringFromAPIToCodeNameModelWithDefaultsForCreate(src.UnitDoseFormUnits, null)))
                .ForMember(dest => dest.UnitDoseUnitOfMeasure, opt => opt.MapFrom(src => ConvertStringFromAPIToCodeNameModelWithDefaultsForCreate(src.UnitDoseUnitOfMeasureCd, null)))
                .ForMember(dest => dest.GlutenFree, opt => opt.MapFrom(src => CheckParse(src.GlutenFree)))
                .ForMember(dest => dest.PreservativeFree, opt => opt.MapFrom(src => CheckParse(src.PreservativeFree)))
                .ForMember(dest => dest.SugarFree, opt => opt.MapFrom(src => CheckParse(src.SugarFree)))
                .ForMember(dest => dest.CFCFree, opt => opt.MapFrom(src => CheckParse(src.CfcFree)))

                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => ConvertStringFromAPIToCodeNameModelWithDefaultsForCreate(src.SupplierCd, src.SupplierName)))
                .ForMember(dest => dest.TradeFamily, opt => opt.MapFrom(src => ConvertStringFromAPIToCodeNameModelWithDefaultsForCreate(src.TradeFamilyCd, src.TradeFamilyName)))
                .ForMember(dest => dest.EmaAdditionalMonitoring, opt => opt.MapFrom(src => CheckParse(src.EmaAdditionalMonitoring)))
                .ForMember(dest => dest.ExpensiveMedication, opt => opt.MapFrom(src => CheckParse(src.ExpensiveMedication)))
                .ForMember(dest => dest.NotForPrn, opt => opt.MapFrom(src => CheckParse(src.NotForPrn)))
                .ForMember(dest => dest.ClinicalTrialMedication, opt => opt.MapFrom(src => CheckParse(src.ClinicalTrialMedication)))
                .ForMember(dest => dest.ParallelImport, opt => opt.MapFrom(src => CheckParse(src.ParallelImport)))
                .ForMember(dest => dest.UnlicensedMedication, opt => opt.MapFrom(src => CheckParse(src.UnlicensedMedicationCd)))
                .ForMember(dest => dest.MedusaPreparationInstructions, opt => opt.MapFrom(src => src.MedusaPreparationInstructions.ConvertToCodeNameModel(item => item, item => item, null, null)))
                .ForMember(dest => dest.ControlledDrugCategories, opt => opt.MapFrom(src => src.ControlledDrugCategories.ConvertToCodeNameModel(item => item.Cd, item => item.Desc, null, null)))
                .ForMember(dest => dest.TitrationTypes, opt => opt.MapFrom(src => src.TitrationTypes.ConvertToCodeNameModel(item => item.Cd, item => item.Desc, null, null)))

                .ForMember(dest => dest.Diluents, opt => opt.MapFrom(src => ConvertLookupAPIListToCodeNameModelWithDefaultsForCreate(src.Diluents)))
                .ForMember(dest => dest.IsDiluent, opt => opt.MapFrom(src => src.IsDiluent == true))
                .ForMember(dest => dest.IsBloodProduct, opt => opt.MapFrom(src => src.IsBloodProduct == true))
                .ForMember(dest => dest.IsCustomControlledDrug, opt => opt.MapFrom(src => src.IsCustomControlledDrug == true))
                .ForMember(dest => dest.IsPrescriptionPrintingRequired, opt => opt.MapFrom(src => src.IsPrescriptionPrintingRequired == true))
                .ForMember(dest => dest.IsModifiedRelease, opt => opt.MapFrom(src => src.IsModifiedRelease == true))
                .ForMember(dest => dest.IsGastroResistant, opt => opt.MapFrom(src => src.IsGastroResistant == true))
                .ForMember(dest => dest.IsIndicationMandatory, opt => opt.MapFrom(src => src.IsIndicationMandatory == true));

        }

        private List<CodeNameSelectorModel> ConvertLookupAPIListToCodeNameModelWithDefaultsForCreate(List<FormularyLookupAPIModel> apiModel)
        {
            if (apiModel == null || !apiModel.IsCollectionValid()) return null;

            var codeNameList = apiModel.Select(rec => rec.ConvertToCodeNameModel((item) => item.Cd, item => item.Desc, null)).ToList();

            return codeNameList;
        }

        private CodeNameSelectorModel ConvertStringFromAPIToCodeNameModelWithDefaultsForCreate(string cd, string desc)
        {
            if (cd.IsEmpty()) return null;

            var codeName = cd.ConvertToCodeNameModel((item) => item, item => desc ?? item, null);

            return codeName;
        }

        private List<CodeNameSelectorModel> GetRoutes(FormularyHeaderAPIModel src, string routeFieldType_Cd)
        {
            if (src == null || !src.FormularyRouteDetails.IsCollectionValid()) return null;
            return src.FormularyRouteDetails
                .Where(rec => rec.RouteFieldTypeCd == routeFieldType_Cd)
                .Select(rec => ConvertStringFromAPIToCodeNameModelWithDefaultsForCreate(rec.RouteCd, null)).ToList();
        }

        private List<CodeNameSelectorModel> GetLocalRoutes(FormularyHeaderAPIModel src, string routeFieldType_Cd)
        {
            if (src == null || !src.FormularyLocalRouteDetails.IsCollectionValid()) return null;
            return src.FormularyLocalRouteDetails
                .Where(rec => rec.RouteFieldTypeCd == routeFieldType_Cd)
                .Select(rec => ConvertStringFromAPIToCodeNameModelWithDefaultsForCreate(rec.RouteCd, null)).ToList();
        }

        public bool CheckParse(string value)
        {
            if (value.IsEmpty() || value == "null" || value == "0")
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public class AdditonalCodeConverter : IValueConverter<List<FormularyAdditionalCodeAPIModel>, List<FormularyAdditionalCodeModel>>
        {
            private string _codeType;

            public AdditonalCodeConverter()
            {

            }

            public AdditonalCodeConverter(string codeType)
            {
                _codeType = codeType;
            }

            public List<FormularyAdditionalCodeModel> Convert(List<FormularyAdditionalCodeAPIModel> sourceMember, ResolutionContext context)
            {
                if (sourceMember == null || !sourceMember.IsCollectionValid()) return null;

                var additonalCodesDTO = context.Mapper.Map<List<FormularyAdditionalCodeModel>>(sourceMember);

                return additonalCodesDTO.Where(ac => ac.CodeType.IsNotEmpty() && string.Compare(ac.CodeType, _codeType, true) == 0)?.ToList();
            }
        }

        private class OverrideDefaultMapAction : IMappingAction<FormularyHeaderAPIModel, FormularyCreateModel>
        {
            public void Process(FormularyHeaderAPIModel source, FormularyCreateModel destination, ResolutionContext context)
            {
                if (destination == null) return;

                //For AMPs there will be only one record
                if (source.ProductType.IsNotEmpty() && string.Compare(source.ProductType, "amp", true) == 0)
                {
                    destination.MedusaPreparationInstructionsEditable = destination.MedusaPreparationInstructions.FirstOrDefault() == null ? null : destination.MedusaPreparationInstructions[0].Id;
                    destination.ControlledDrugCategoriesEditableId = destination.ControlledDrugCategories.FirstOrDefault() == null ? null : destination.ControlledDrugCategories[0].Id;
                    destination.TitrationTypesEditableId = destination.TitrationTypes.FirstOrDefault() == null ? null : destination.TitrationTypes[0].Id;
                }

                if (destination.Ingredients.IsCollectionValid())
                {
                    destination.Ingredients.Each(rec =>
                    {
                        if (rec.Ingredient.IsNotNull()) rec.Ingredient.IsReadonly = false;
                        if (rec.StrengthValueDenominatorUnit.IsNotNull()) rec.StrengthValueDenominatorUnit.IsReadonly = false;
                        if (rec.StrengthValueNumeratorUnit.IsNotNull()) rec.StrengthValueNumeratorUnit.IsReadonly = false;
                    });
                }

                if (destination.Excipients.IsCollectionValid())
                {
                    destination.Excipients.Each(rec =>
                    {
                        if (rec.Ingredient.IsNotNull()) rec.Ingredient.IsReadonly = false;
                        if (rec.StrengthUnit.IsNotNull()) rec.StrengthUnit.IsReadonly = false;
                    });
                }
            }
        }

        private class NameResolveActionForCreate : IMappingAction<FormularyHeaderAPIModel, FormularyCreateModel>
        {
            private IHttpContextAccessor _httpContextAccessor;

            public NameResolveActionForCreate(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public void Process(FormularyHeaderAPIModel source, FormularyCreateModel destination, ResolutionContext context)
            {
                if (destination == null) return;

                destination.Route?
                    .Each(rec => rec.SafeAssignCodeNameFromSession(SynapseSession.RoutesLkpKey, _httpContextAccessor.HttpContext));

                destination.UnlicensedRoute?
                    .Each(rec => rec.SafeAssignCodeNameFromSession(SynapseSession.RoutesLkpKey, _httpContextAccessor.HttpContext));

                destination.LocalLicensedRoute?
                   .Each(rec => rec.SafeAssignCodeNameFromSession(SynapseSession.RoutesLkpKey, _httpContextAccessor.HttpContext));

                destination.LocalUnlicensedRoute?
                    .Each(rec => rec.SafeAssignCodeNameFromSession(SynapseSession.RoutesLkpKey, _httpContextAccessor.HttpContext));

                destination.TitrationTypes?
                    .Each(rec => rec.SafeAssignCodeNameFromSession(SynapseSession.TitrationType, _httpContextAccessor.HttpContext));

                destination.ControlledDrugCategories?
                    .Each(rec => rec.SafeAssignCodeNameFromSession(SynapseSession.ControlledDrugCategories, _httpContextAccessor.HttpContext));

                destination.FormCd?.SafeAssignCodeNameFromSession(SynapseSession.FormsLkpKey, _httpContextAccessor.HttpContext);

                destination.Supplier?.SafeAssignCodeNameFromSession(SynapseSession.SupplierLkpKey, _httpContextAccessor.HttpContext);

                destination.UnitDoseFormUnits?.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContextAccessor.HttpContext);

                destination.UnitDoseUnitOfMeasure?.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContextAccessor.HttpContext);

                destination.Ingredients?.Each(ing =>
                {
                    ing.Ingredient.SafeAssignCodeNameFromSession(SynapseSession.IngredientsLkpKey, _httpContextAccessor.HttpContext);
                    ing.StrengthValueDenominatorUnit?.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContextAccessor.HttpContext);
                    ing.StrengthValueNumeratorUnit?.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContextAccessor.HttpContext);
                });

                destination.Excipients?.Each(excp =>
                {
                    excp.Ingredient.SafeAssignCodeNameFromSession(SynapseSession.IngredientsLkpKey, _httpContextAccessor.HttpContext);
                    excp.StrengthUnit?.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContextAccessor.HttpContext);
                });
            }
        }

        private class FormularyDetailResolveActionForCreate : IMappingAction<FormularyHeaderAPIModel, FormularyCreateModel>
        {
            public void Process(FormularyHeaderAPIModel source, FormularyCreateModel destination, ResolutionContext context)
            {
                if (destination == null || source == null || source.Detail == null) return;

                context.Mapper.Map(source.Detail, destination);
            }
        }
    }
}

