//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

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
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace SynapseStudioWeb.AppCode.MappingProfiles.MedicationManagement
//{
//    public class FormularyAddEditAPIVMMapperProfile : Profile
//    {
//        public FormularyAddEditAPIVMMapperProfile()
//        {
//            CreateMap<FormularyHeaderAPIModel, FormularyAddEditModel>()
//                .ForMember(dest => dest.Route, opt => opt.MapFrom(src => GetRoutes(src, TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD)))
//                .ForMember(dest => dest.UnlicensedRoute, opt => opt.MapFrom(src => GetRoutes(src, TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED_CD)))
//                //.ForMember(dest => dest.Indication, opt => opt.MapFrom(src => src.FormularyIndications.ConvertToCodeNameModel(val => val.IndicationCd, val => val.IndicationName, null, null)))
//                //.ForMember(dest => dest.FormNRoute, opt => opt.MapFrom(src => src.FormularyOntologyForms.ConvertToCodeNameModel(val => val.FormCd, val => val.FormCd, null, null)))
//                .ForMember(dest => dest.VirtualTherapeuticMoiety, opt => opt.MapFrom(src => src.VtmId))
//                .ForMember(dest => dest.VirtualMedicinalProduct, opt => opt.MapFrom(src => src.VmpId))
//                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.RecStatusCode));


//            CreateMap<FormularyDetailAPIModel, FormularyAddEditModel>()
//                .ForMember(dest => dest.ContraIndications, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.ContraIndications, "fdb")))
//                .ForMember(dest => dest.Cautions, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.Cautions, "fdb")))
//                .ForMember(dest => dest.LicensedUse, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.LicensedUses, "fdb")))
//                .ForMember(dest => dest.UnlicensedUse, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.UnLicensedUses, "fdb")))
//                .ForMember(dest => dest.SideEffects, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.SideEffects, "fdb")))
//                .ForMember(dest => dest.SafetyMessages, opt => opt.MapFrom(src => ConvertCodeDescAPIToCodeNameModelWithDefaults(src.SafetyMessages, "fdb")))
//                .ForMember(dest => dest.TradeFamily, opt => opt.MapFrom(src => src.TradeFamilyCd.ConvertToCodeNameModel(item => item, item => src.TradeFamilyName)))
//                .ForMember(dest => dest.FormCd, opt => opt.MapFrom(src => src.FormCd.ConvertToCodeNameModel(item => item, item => item)))
//                .ForMember(dest => dest.Supplier, opt => opt.MapFrom(src => src.SupplierCd.ConvertToCodeNameModel(item => item, item => item)))
//                //.ForMember(dest => dest.CustomWarning, opt => opt.MapFrom(src => src.CustomWarnings.ConvertToCodeNameModel(item => item, item => item, null, null)))
//                //.ForMember(dest => dest.Endorsements, opt => opt.MapFrom(src => src.Endorsements.ConvertToCodeNameModel(item => item, item => item, null, null)))
//                //.ForMember(dest => dest.MaximumDoseUnit, opt => opt.MapFrom(src => src.MaximumDoseUnitCd.ConvertToCodeNameModel(item => item, item => item)))
//                .ForMember(dest => dest.UnitDoseFormUnits, opt => opt.MapFrom(src => src.UnitDoseFormUnits.ConvertToCodeNameModel(item => item, item => item)))
//                .ForMember(dest => dest.UnitDoseUnitOfMeasure, opt => opt.MapFrom(src => src.UnitDoseUnitOfMeasureCd.ConvertToCodeNameModel(item => item, item => item)))

//                //.ForMember(dest => dest.InpatientMedication, opt => opt.MapFrom(src => src.InpatientMedicationCd))
//                .ForMember(dest => dest.OutpatientMedication, opt => opt.MapFrom(src => src.OutpatientMedicationCd))
//                .ForMember(dest => dest.PrescribingStatus, opt => opt.MapFrom(src => src.PrescribingStatusCd))
//                //.ForMember(dest => dest.Rules, opt => opt.MapFrom(src => src.RulesCd))
//                .ForMember(dest => dest.UnlicensedMedication, opt => opt.MapFrom(src => src.UnlicensedMedicationCd))
//                //.ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.DrugClass))
//                .ForMember(dest => dest.ControlledDrugCategory, opt => opt.MapFrom(src => src.ControlledDrugCategoryCd))
//                .ForMember(dest => dest.TitrationType, opt => opt.MapFrom(src => src.TitrationTypeCd))
//                .ForMember(dest => dest.RoundingFactor, opt => opt.MapFrom(src => src.RoundingFactorCd))


//                //.ForMember(dest => dest.MarkedModifier, opt => opt.MapFrom(src => src.MarkedModifierCd))
//                //.ForMember(dest => dest.MedicationType, opt => opt.MapFrom(src => src.MedicationTypeCode))
//                .ForMember(dest => dest.BasisOfPreferredName, opt => opt.MapFrom(src => src.BasisOfPreferredNameCd))
//                .ForMember(dest => dest.DoseForm, opt => opt.MapFrom(src => src.DoseFormCd))
//                .ForMember(dest => dest.UnitDoseFormSize, opt => opt.MapFrom(src => src.UnitDoseFormSize))
//                .ForMember(dest => dest.NotesForRestriction, opt => opt.MapFrom(src => src.RestrictionNote))
//                //.ForMember(dest => dest.ModifiedRelease, opt => opt.MapFrom(src => src.ModifiedReleaseCd))
//                .ForMember(dest => dest.MaximumDoseNumerator, opt => opt.MapFrom(src => src.MaxDoseNumerator))
//                .ForMember(dest => dest.CurrentLicensingAuthority, opt => opt.MapFrom(src => src.CurrentLicensingAuthorityCd))
//                .ForMember(dest => dest.RestrictionsOnAvailability, opt => opt.MapFrom(src => src.RestrictionsOnAvailabilityCd))

//                .ForMember(dest => dest.Prescribable, opt => opt.MapFrom(src => src.Prescribable.HasValue ? src.Prescribable.Value : false))
//                .ForMember(dest => dest.FormularyStatus, opt => opt.MapFrom(src => src.RnohFormularyStatuscd))

//                .ForMember(dest => dest.ControlledDrugCategory, opt => opt.MapFrom(src => src.ControlledDrugCategoryCd))
//                //.ForMember(dest => dest.OrderFormType, opt => opt.MapFrom(src => src.OrderableFormtypeCd))
//                .ForMember(dest => dest.Antibiotic, opt => opt.MapFrom(src => CheckParse(src.Antibiotic)))
//                .ForMember(dest => dest.Anticoagulant, opt => opt.MapFrom(src => CheckParse(src.Anticoagulant)))
//                .ForMember(dest => dest.Antipsychotic, opt => opt.MapFrom(src => CheckParse(src.Antipsychotic)))
//                .ForMember(dest => dest.Antimicrobial, opt => opt.MapFrom(src => CheckParse(src.Antimicrobial)))
//                .ForMember(dest => dest.BlackTriangle, opt => opt.MapFrom(src => CheckParse(src.BlackTriangle)))
//                .ForMember(dest => dest.ClinicalTrialMedication, opt => opt.MapFrom(src => CheckParse(src.ClinicalTrialMedication)))
//                .ForMember(dest => dest.CriticalDrug, opt => opt.MapFrom(src => CheckParse(src.CriticalDrug)))
//                .ForMember(dest => dest.Cytotoxic, opt => opt.MapFrom(src => CheckParse(src.Cytotoxic)))
//                .ForMember(dest => dest.GlutenFree, opt => opt.MapFrom(src => CheckParse(src.GlutenFree)))
//                .ForMember(dest => dest.HighAlertMedication, opt => opt.MapFrom(src => CheckParse(src.HighAlertMedication)))
//                .ForMember(dest => dest.EMAAdditionalMonitoring, opt => opt.MapFrom(src => CheckParse(src.EmaAdditionalMonitoring)))
//                //.ForMember(dest => dest.Insulins, opt => opt.MapFrom(src => checkParse(src.Insulins)))
//                .ForMember(dest => dest.IVToOral, opt => opt.MapFrom(src => CheckParse(src.IvToOral)))
//                //.ForMember(dest => dest.MentalHealthDrug, opt => opt.MapFrom(src => checkParse(src.MentalHealthDrug)))
//                .ForMember(dest => dest.NotForPRN, opt => opt.MapFrom(src => CheckParse(src.NotForPrn)))
//                .ForMember(dest => dest.OutpatientMedication, opt => opt.MapFrom(src => CheckParse(src.OutpatientMedicationCd)))
//                .ForMember(dest => dest.PreservativeFree, opt => opt.MapFrom(src => CheckParse(src.PreservativeFree)))
//                //.ForMember(dest => dest.RestrictedPrescribing, opt => opt.MapFrom(src => checkParse(src.RestrictedPrescribing)))
//                //.ForMember(dest => dest.ReviewReminder, opt => opt.MapFrom(src => src.AddReviewReminder))
//                .ForMember(dest => dest.SugarFree, opt => opt.MapFrom(src => CheckParse(src.SugarFree)))
//                .ForMember(dest => dest.UnlicensedMedication, opt => opt.MapFrom(src => CheckParse(src.UnlicensedMedicationCd)))
//                .ForMember(dest => dest.IgnoreDuplicateWarnings, opt => opt.MapFrom(src => CheckParse(src.IgnoreDuplicateWarnings)))
//                //.ForMember(dest => dest.InpatientMedication, opt => opt.MapFrom(src => checkParse(src.InpatientMedicationCd)))
//                .ForMember(dest => dest.WitnessingRequired, opt => opt.MapFrom(src => CheckParse(src.WitnessingRequired)))
//                .ForMember(dest => dest.ParallelImport, opt => opt.MapFrom(src => CheckParse(src.ParallelImport)))
//                //.ForMember(dest => dest.Fluid, opt => opt.MapFrom(src => checkParse(src.Fluid)))
//                .ForMember(dest => dest.CFCFree, opt => opt.MapFrom(src => CheckParse(src.CfcFree)))
//                .ForMember(dest => dest.ExpensiveMedication, opt => opt.MapFrom(src => CheckParse(src.ExpensiveMedication)));

//            CreateMap<FormularyAdditionalCodeAPIModel, FormularyAdditionalCodeModel>();

//            //CreateMap<FormularyIndicationAPIModel, CustomMedicationModel>()
//            //    .ForMember(dest => dest.Indication, opt => opt.MapFrom(src => src.IndicationCd));

//            CreateMap<FormularyIngredientAPIModel, FormularyIngredientModel>()
//                .ForMember(dest => dest.Ingredient, opt => opt.MapFrom(src => src.ConvertToCodeNameModel(val => val.IngredientCd, val => val.IngredientCd)))
//                .ForMember(dest => dest.BasisOfPharmaceuticalStrength, opt => opt.MapFrom(src => src.BasisOfPharmaceuticalStrengthCd))
//                .ForMember(dest => dest.StrengthValueDenominatorUnit, opt => opt.MapFrom(src => src.ConvertToCodeNameModel(val => val.StrengthValueDenominatorUnitCd, val => val.StrengthValueDenominatorUnitCd)))
//                .ForMember(dest => dest.StrengthValueNumeratorUnit, opt => opt.MapFrom(src => src.ConvertToCodeNameModel(val => val.StrengthValueNumeratorUnitCd, val => val.StrengthValueNumeratorUnitCd)))
//                .ForMember(dest => dest.StrengthValNumerator, opt => opt.MapFrom(src => src.StrengthValueNumerator))
//                .ForMember(dest => dest.StrengthValDenominator, opt => opt.MapFrom(src => src.StrengthValueDenominator));


//            CreateMap<FormularyCustomWarningAPIModel, FormularyCustomWarningModel>();
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
