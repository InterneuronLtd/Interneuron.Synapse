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
﻿//using SynapseStudioWeb.DataService.APIModel.Requests;
//using SynapseStudioWeb.Models.MedicinalMgmt;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AutoMapper;
//using Interneuron.Common.Extensions;
//using SynapseStudioWeb.AppCode.Constants;

//namespace SynapseStudioWeb.AppCode.MappingProfiles
//{
//    public class CreateCustomFormularyProfile : FormularycCommonVMToAPIProfile
//    {
//        public CreateCustomFormularyProfile()
//        {
//            CreateMap<FormularyAddEditModel, CreateFormularyAPIRequest>()
//                .ForMember(dest => dest.VtmId, opt => opt.MapFrom(src => src.VirtualTherapeuticMoiety))
//                .ForMember(dest => dest.VmpId, opt => opt.MapFrom(src => src.VirtualMedicinalProduct))
//                //.ForMember(dest => dest.FormularyOntologyForms, opt => opt.MapFrom(src => ConvertToOntology(src)))
//                .ForMember(dest => dest.RecStatusCode, opt => opt.MapFrom(src => src.Status))
//                .ForMember(dest => dest.FormularyRouteDetails, opt => opt.MapFrom(src=> ConvertRouteModelToAPIModel(src)))
//                .ForMember(dest => dest.RecSource, opt => opt.MapFrom(src => src.RecSource));

//            CreateMap<FormularyAddEditModel, CreateFormularyDetailAPIRequest>()
//                //.ForMember(dest => dest.MedicationTypeCode, opt => opt.MapFrom(src => src.MedicationType))
//                .ForMember(dest => dest.RnohFormularyStatuscd, opt => opt.MapFrom(src => src.FormularyStatus))
//                .ForMember(dest => dest.Prescribable, opt => opt.MapFrom(src => src.Prescribable))
//                //.ForMember(dest => dest.InpatientMedicationCd, opt => opt.MapFrom(src => src.InpatientMedication == true ? "1" : "0"))
//                .ForMember(dest => dest.OutpatientMedicationCd, opt => opt.MapFrom(src => src.OutpatientMedication == true ? "1" : "0"))
//                .ForMember(dest => dest.PrescribingStatusCd, opt => opt.MapFrom(src => src.PrescribingStatus))
//                //.ForMember(dest => dest.RulesCd, opt => opt.MapFrom(src => src.Rules))
//                .ForMember(dest => dest.UnlicensedMedicationCd, opt => opt.MapFrom(src => src.UnlicensedMedication == true ? "1" : null))
//                .ForMember(dest => dest.HighAlertMedication, opt => opt.MapFrom(src => src.HighAlertMedication == true ? "1" : null))
//                .ForMember(dest => dest.IgnoreDuplicateWarnings, opt => opt.MapFrom(src => src.IgnoreDuplicateWarnings == true ? "1" : null))
//                .ForMember(dest => dest.NotForPrn, opt => opt.MapFrom(src => src.NotForPRN == true ? "1" : null))
//                .ForMember(dest => dest.CriticalDrug, opt => opt.MapFrom(src => src.CriticalDrug == true ? "1" : null))
//                .ForMember(dest => dest.ControlledDrugCategoryCd, opt => opt.MapFrom(src => src.ControlledDrugCategory))
//                .ForMember(dest => dest.Cytotoxic, opt => opt.MapFrom(src => src.Cytotoxic == true ? "1" : null))
//                .ForMember(dest => dest.ClinicalTrialMedication, opt => opt.MapFrom(src => src.ClinicalTrialMedication == true ? "1" : null))
//                //.ForMember(dest => dest.Fluid, opt => opt.MapFrom(src => src.Fluid == true ? "1" : null))
//                .ForMember(dest => dest.Antibiotic, opt => opt.MapFrom(src => src.Antibiotic == true ? "1" : null))
//                .ForMember(dest => dest.Anticoagulant, opt => opt.MapFrom(src => src.Anticoagulant == true ? "1" : null))
//                .ForMember(dest => dest.Antipsychotic, opt => opt.MapFrom(src => src.Antipsychotic == true ? "1" : null))
//                .ForMember(dest => dest.Antimicrobial, opt => opt.MapFrom(src => src.Antimicrobial == true ? "1" : null))
//                //.ForMember(dest => dest.AddReviewReminder, opt => opt.MapFrom(src => src.ReviewReminder ? true: default(bool?)))
//                .ForMember(dest => dest.IvToOral, opt => opt.MapFrom(src => src.IVToOral == true ? "1" : null))
//                .ForMember(dest => dest.TitrationTypeCd, opt => opt.MapFrom(src => src.TitrationType))
//                .ForMember(dest => dest.RoundingFactorCd, opt => opt.MapFrom(src => src.RoundingFactor))
//                .ForMember(dest => dest.MaxDoseNumerator, opt => opt.MapFrom(src => CheckForDecimal(src.MaximumDoseNumerator)))
//                //.ForMember(dest => dest.MaximumDoseUnitCd, opt => opt.MapFrom(src => src.MaximumDoseUnit != null ? src.MaximumDoseUnit.Id : null))
//                .ForMember(dest => dest.WitnessingRequired, opt => opt.MapFrom(src => src.WitnessingRequired == true ? "1" : null))
//                //.ForMember(dest => dest.RestrictedPrescribing, opt => opt.MapFrom(src => src.RestrictedPrescribing == true ? "1" : null))
//                .ForMember(dest => dest.RestrictionNote, opt => opt.MapFrom(src => src.NotesForRestriction))
//                //.ForMember(dest => dest.OrderableFormtypeCd, opt => opt.MapFrom(src => src.OrderFormType))
//                //.ForMember(dest => dest.DrugClass, opt => opt.MapFrom(src => src.Class))
//                //.ForMember(dest => dest.MarkedModifierCd, opt => opt.MapFrom(src => src.MarkedModifier))
//                //.ForMember(dest => dest.Insulins, opt => opt.MapFrom(src => src.Insulins == true ? "1" : null))
//                //.ForMember(dest => dest.MentalHealthDrug, opt => opt.MapFrom(src => src.MentalHealthDrug == true ? "1" : null))
//                .ForMember(dest => dest.BasisOfPreferredNameCd, opt => opt.MapFrom(src => src.BasisOfPreferredName))
//                .ForMember(dest => dest.SugarFree, opt => opt.MapFrom(src => src.SugarFree == true ? "1" : null))
//                .ForMember(dest => dest.GlutenFree, opt => opt.MapFrom(src => src.GlutenFree == true ? "1" : null))
//                .ForMember(dest => dest.PreservativeFree, opt => opt.MapFrom(src => src.PreservativeFree == true ? "1" : null))
//                .ForMember(dest => dest.CfcFree, opt => opt.MapFrom(src => src.CFCFree == true ? "1" : null))
//                .ForMember(dest => dest.DoseFormCd, opt => opt.MapFrom(src => src.DoseForm))
//                .ForMember(dest => dest.UnitDoseFormSize, opt => opt.MapFrom(src => src.UnitDoseFormSize))
//                .ForMember(dest => dest.UnitDoseFormUnits, opt => opt.MapFrom(src => src.UnitDoseFormUnits != null ? src.UnitDoseFormUnits.Id : null))
//                .ForMember(dest => dest.UnitDoseUnitOfMeasureCd, opt => opt.MapFrom(src => src.UnitDoseUnitOfMeasure != null ? src.UnitDoseUnitOfMeasure.Id : null))
//                .ForMember(dest => dest.TradeFamilyCd, opt => opt.MapFrom(src => src.TradeFamily != null ? src.TradeFamily.Id : null))
//                .ForMember(dest => dest.TradeFamilyName, opt => opt.MapFrom(src => src.TradeFamily != null ? src.TradeFamily.Name : null))
//                //.ForMember(dest => dest.ModifiedReleaseCd, opt => opt.MapFrom(src => src.ModifiedRelease))
//                .ForMember(dest => dest.BlackTriangle, opt => opt.MapFrom(src => src.BlackTriangle == true ? "1" : null))
//                .ForMember(dest => dest.SupplierCd, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Id : null))
//                .ForMember(dest => dest.CurrentLicensingAuthorityCd, opt => opt.MapFrom(src => src.CurrentLicensingAuthority))
//                .ForMember(dest => dest.EmaAdditionalMonitoring, opt => opt.MapFrom(src => src.EMAAdditionalMonitoring == true ? "1" : null))
//                .ForMember(dest => dest.ParallelImport, opt => opt.MapFrom(src => src.ParallelImport == true ? "1" : null))
//                .ForMember(dest => dest.RestrictionsOnAvailabilityCd, opt => opt.MapFrom(src => src.RestrictionsOnAvailability))
//                .ForMember(dest => dest.SideEffects, opt => opt.MapFrom(src => ConvertValueToString(src.SideEffects)))
//                .ForMember(dest => dest.Cautions, opt => opt.MapFrom(src => ConvertValueToString(src.Cautions)))
//                .ForMember(dest => dest.SafetyMessages, opt => opt.MapFrom(src => ConvertValueToString(src.SafetyMessages)))
//                .ForMember(dest => dest.CustomWarnings, opt => opt.MapFrom(src => ConvertValueToString(src.CustomWarnings)))
//                .ForMember(dest => dest.Endorsements, opt => opt.MapFrom(src => src.Endorsements))
//                .ForMember(dest => dest.ContraIndications, opt => opt.MapFrom(src => ConvertToContraindications(src)))
//                .ForMember(dest => dest.FormCd, opt => opt.MapFrom(src => src.FormCd != null ? src.FormCd.Id : null))
//                .ForMember(dest => dest.LicensedUses, opt => opt.MapFrom(src => ConvertToLicensedOrUnlicensedUse(src.LicensedUse)))
//                .ForMember(dest => dest.UnLicensedUses, opt => opt.MapFrom(src => ConvertToLicensedOrUnlicensedUse(src.UnlicensedUse)))
//                .ForMember(dest => dest.ExpensiveMedication, opt => opt.MapFrom(src => src.ExpensiveMedication == true ? "1" : null));

           

//            //CreateMap<CodeNameSelectorModel, CreateFormularyRouteDetailAPIRequest>()
//            //    .ForMember(dest => dest.RouteCd, opt => opt.MapFrom(src => src.Id));

//        }

//        private List<CreateContraindicationAPIRequest> ConvertToContraindications(FormularyAddEditModel src)
//        {
//            if (!src.ContraIndications.IsCollectionValid())
//            {
//                return null;
//            }

//            return src.ContraIndications.Select(m => new CreateContraindicationAPIRequest { Cd = m.Id, Desc = m.Name }).ToList();
//        }

//        //private List<CreateFormularyOntologyFormAPIRequest> ConvertToOntology(FormularyAddEditModel src)
//        //{
//        //    if (!src.FormNRoute.IsCollectionValid())
//        //    {
//        //        return null;
//        //    }

//        //    return src.FormNRoute.Select(m => new CreateFormularyOntologyFormAPIRequest { FormCd = m.Id }).ToList();
//        //}

//        private new List<string> ConvertValueToString(List<CodeNameSelectorModel> src)
//        {
//            if (!src.IsCollectionValid())
//            {
//                return null;
//            }

//            return src.Select(x => x.Id).ToList();
//        }

//        private new decimal? CheckForDecimal(string src)
//        {
//            if (src.IsEmpty())
//            {
//                return null;
//            }

//            return Convert.ToDecimal(src);
//        }

//        //private long? CheckForLong(string src)
//        //{
//        //    if (src.IsEmpty())
//        //    {
//        //        return null;
//        //    }

//        //    return long.Parse(src);
//        //}

//        //private List<CreateFormularyRouteDetailAPIRequest> ConvertToRoutes(List<CodeNameSelectorModel> src, string routeField)
//        //{
//        //    if (!src.IsCollectionValid())
//        //    {
//        //        return null;
//        //    }

//        //    string routeFieldType = string.Empty;

//        //    switch (routeField)
//        //    {
//        //        case "Route":
//        //            routeFieldType = TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD;
//        //            break;
//        //        case "Additional":
//        //            routeFieldType = TerminologyConstants.ROUTEFIELDTYPE_ADDITONAL_CD;
//        //            break;
//        //        case "Discretionary":
//        //            routeFieldType = TerminologyConstants.ROUTEFIELDTYPE_DISCRETIONARY_CD;
//        //            break;
//        //        case "Unlicensed":
//        //            routeFieldType = TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED_CD;
//        //            break;
//        //        default:
//        //            routeFieldType = TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD;
//        //            break;
//        //    }


//        //    return src.Select(m => new CreateFormularyRouteDetailAPIRequest { RouteCd = m.Id, RouteFieldTypeCd = routeFieldType }).ToList();
//        //}

//        private List<CreateLicensedOrUnlicensedUseAPIRequest> ConvertToLicensedOrUnlicensedUse(List<CodeNameSelectorModel> src)
//        {
//            if (!src.IsCollectionValid())
//            {
//                return null;
//            }

//            return src.Select(m => new CreateLicensedOrUnlicensedUseAPIRequest { Cd = m.Id, Desc = m.Name }).ToList();
//        }

//        private List<CreateFormularyRouteDetailAPIRequest> ConvertRouteModelToAPIModel(FormularyAddEditModel src)
//        {
//            var formularyRoutes = new List<CreateFormularyRouteDetailAPIRequest>();

//            //if (src.AdditionalRoute.IsCollectionValid())
//            //{
//            //    src.AdditionalRoute.Each(ar =>
//            //    {
//            //        formularyRoutes.Add(new CreateFormularyRouteDetailAPIRequest { RouteCd = ar.Id, RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_ADDITONAL_CD });
//            //    });
//            //}
//            if (src.Route.IsCollectionValid())
//            {
//                src.Route.Each(ar =>
//                {
//                    formularyRoutes.Add(new CreateFormularyRouteDetailAPIRequest { RouteCd = ar.Id, RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD });
//                });
//            }
//            //if (src.DiscretionaryRoutes.IsCollectionValid())
//            //{
//            //    src.DiscretionaryRoutes.Each(ar =>
//            //    {
//            //        formularyRoutes.Add(new CreateFormularyRouteDetailAPIRequest { RouteCd = ar.Id, RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_DISCRETIONARY_CD });
//            //    });
//            //}
//            if (src.UnlicensedRoute.IsCollectionValid())
//            {
//                src.UnlicensedRoute.Each(ar =>
//                {
//                    formularyRoutes.Add(new CreateFormularyRouteDetailAPIRequest { RouteCd = ar.Id, RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED_CD });
//                });
//            }

//            return formularyRoutes;
//        }

//    }

//}
