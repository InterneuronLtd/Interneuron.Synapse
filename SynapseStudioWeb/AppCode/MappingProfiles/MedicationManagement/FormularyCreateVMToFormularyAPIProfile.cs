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
using SynapseStudioWeb.AppCode.Constants;
using SynapseStudioWeb.DataService.APIModel;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models.MedicationMgmt;
using SynapseStudioWeb.Models.MedicinalMgmt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SynapseStudioWeb.AppCode.MappingProfiles.MedicationManagement
{
    public class FormularyCreateVMToFormularyAPIProfile : Profile
    {
        public FormularyCreateVMToFormularyAPIProfile()
        {
            CreateMap<FormularyCreateModel, FormularyHeaderAPIModel>()
                .ForMember(dest => dest.FormularyRouteDetails, opt => opt.MapFrom(src => GetRoutes(src)))
                .ForMember(dest => dest.FormularyLocalRouteDetails, opt => opt.MapFrom(src => GetLocalRoutes(src)))
                .ForMember(dest => dest.RecStatusCode, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.FormularyAdditionalCodes, opt => opt.MapFrom<AdditonalCodeResolver>())
                .ForMember(dest => dest.Detail, opt => opt.ConvertUsing(new FormularyDetailConverter(), src => src))
                .ForMember(dest => dest.FormularyIngredients, opt => opt.ConvertUsing(new FormularyIngredientsConverter(), src => src))
                .ForMember(dest => dest.FormularyExcipients, opt => opt.ConvertUsing(new FormularyExcipientsConverter(), src => src));


            CreateMap<FormularyCreateModel, FormularyDetailAPIModel>()
                .ForMember(dest => dest.FormCd, opt => opt.MapFrom(src => src.FormCd != null ? src.FormCd.Id : null))
                .ForMember(dest => dest.FormDesc, opt => opt.MapFrom(src => src.FormCd != null ? src.FormCd.Name : null))
                .ForMember(dest => dest.LocalLicensedUses, opt => opt.MapFrom(src => ConvertCodeNameModelsWithDefaultsToCodeDescAPI(src.LocalLicensedUse)))
                .ForMember(dest => dest.LocalUnLicensedUses, opt => opt.MapFrom(src => ConvertCodeNameModelsWithDefaultsToCodeDescAPI(src.LocalUnlicensedUse)))
                .ForMember(dest => dest.LicensedUses, opt => opt.MapFrom(src => ConvertCodeNameModelsWithDefaultsToCodeDescAPI(src.LicensedUse)))
                .ForMember(dest => dest.UnLicensedUses, opt => opt.MapFrom(src => ConvertCodeNameModelsWithDefaultsToCodeDescAPI(src.UnlicensedUse)))
                .ForMember(dest => dest.CustomWarnings, opt => opt.MapFrom(src => src.CustomWarnings))
                .ForMember(dest => dest.Reminders, opt => opt.MapFrom(src => src.Reminders))
                .ForMember(dest => dest.Endorsements, opt => opt.MapFrom(src => src.Endorsements))
                .ForMember(dest => dest.ContraIndications, opt => opt.MapFrom(src => ConvertCodeNameModelsWithDefaultsToCodeDescAPI(src.ContraIndications)))
                .ForMember(dest => dest.MedusaPreparationInstructions, opt => opt.MapFrom(src => AssignMedusaPreparationInstructions(src)))
                .ForMember(dest => dest.SideEffects, opt => opt.MapFrom(src => ConvertCodeNameModelsWithDefaultsToCodeDescAPI(src.SideEffects)))
                .ForMember(dest => dest.Cautions, opt => opt.MapFrom(src => ConvertCodeNameModelsWithDefaultsToCodeDescAPI(src.Cautions)))
                .ForMember(dest => dest.CriticalDrug, opt => opt.MapFrom(src => ConvertToBoolString(src.CriticalDrug)))
                .ForMember(dest => dest.ExpensiveMedication, opt => opt.MapFrom(src => ConvertToBoolString(src.ExpensiveMedication)))
                .ForMember(dest => dest.HighAlertMedication, opt => opt.MapFrom(src => ConvertToBoolString(src.HighAlertMedication)))
                .ForMember(dest => dest.NotForPrn, opt => opt.MapFrom(src => ConvertToBoolString(src.NotForPrn)))
                .ForMember(dest => dest.Prescribable, opt => opt.MapFrom(src => src.Prescribable))
                .ForMember(dest => dest.OutpatientMedicationCd, opt => opt.MapFrom(src => ConvertToBoolString(src.OutpatientMedication)))
                .ForMember(dest => dest.TitrationTypes, opt => opt.MapFrom(src => AssignTitrationTypes(src)))
                .ForMember(dest => dest.RnohFormularyStatuscd, opt => opt.MapFrom(src => src.RnohFormularyStatuscd))
                .ForMember(dest => dest.IgnoreDuplicateWarnings, opt => opt.MapFrom(src => ConvertToBoolString(src.IgnoreDuplicateWarnings)))
                .ForMember(dest => dest.WitnessingRequired, opt => opt.MapFrom(src => ConvertToBoolString(src.WitnessingRequired)))
                .ForMember(dest => dest.IvToOral, opt => opt.MapFrom(src => ConvertToBoolString(src.IVToOral)))
                .ForMember(dest => dest.IsBloodProduct, opt => opt.MapFrom(src => src.IsBloodProduct))
                .ForMember(dest => dest.IsDiluent, opt => opt.MapFrom(src => src.IsDiluent))
                .ForMember(dest => dest.SupplierCd, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Id : null))
                .ForMember(dest => dest.SupplierName, opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
                .ForMember(dest => dest.SugarFree, opt => opt.MapFrom(src => ConvertToBoolString(src.SugarFree)))
                .ForMember(dest => dest.GlutenFree, opt => opt.MapFrom(src => ConvertToBoolString(src.GlutenFree)))
                .ForMember(dest => dest.PreservativeFree, opt => opt.MapFrom(src => ConvertToBoolString(src.PreservativeFree)))
                .ForMember(dest => dest.CfcFree, opt => opt.MapFrom(src => ConvertToBoolString(src.CFCFree)))
                .ForMember(dest => dest.UnitDoseFormUnits, opt => opt.MapFrom(src => src.UnitDoseFormUnits != null ? src.UnitDoseFormUnits.Id : null))
                .ForMember(dest => dest.UnitDoseFormUnitsDesc, opt => opt.MapFrom(src => src.UnitDoseFormUnits != null ? src.UnitDoseFormUnits.Name : null))
                .ForMember(dest => dest.UnitDoseUnitOfMeasureCd, opt => opt.MapFrom(src => src.UnitDoseUnitOfMeasure != null ? src.UnitDoseUnitOfMeasure.Id : null))
                .ForMember(dest => dest.UnitDoseUnitOfMeasureDesc, opt => opt.MapFrom(src => src.UnitDoseUnitOfMeasure != null ? src.UnitDoseUnitOfMeasure.Name : null))
                .ForMember(dest => dest.BlackTriangle, opt => opt.MapFrom(src => ConvertToBoolString(src.BlackTriangle)))
                .ForMember(dest => dest.BlackTriangleSource, opt => opt.MapFrom(src => TerminologyConstants.MANUAL_DATA_SOURCE))
                .ForMember(dest => dest.ClinicalTrialMedication, opt => opt.MapFrom(src => ConvertToBoolString(src.ClinicalTrialMedication)))
                .ForMember(dest => dest.ControlledDrugCategories, opt => opt.MapFrom(src => AssignControlledDrugCategories(src)))
                .ForMember(dest => dest.EmaAdditionalMonitoring, opt => opt.MapFrom(src => ConvertToBoolString(src.EmaAdditionalMonitoring)))
                .ForMember(dest => dest.UnlicensedMedicationCd, opt => opt.MapFrom(src => ConvertToBoolString(src.UnlicensedMedication)))
                .ForMember(dest => dest.ParallelImport, opt => opt.MapFrom(src => ConvertToBoolString(src.ParallelImport)))
                .ForMember(dest => dest.Diluents, opt => opt.MapFrom(src => ConvertCodeNameModelsWithDefaultsToCodeDescAPI(src.Diluents)))
                .ForMember(dest => dest.IsCustomControlledDrug, opt => opt.MapFrom(src => src.IsCustomControlledDrug == true))
                .ForMember(dest => dest.IsPrescriptionPrintingRequired, opt => opt.MapFrom(src => src.IsPrescriptionPrintingRequired == true))
                .ForMember(dest => dest.IsModifiedRelease, opt => opt.MapFrom(src => src.IsModifiedRelease == true))
                .ForMember(dest => dest.IsGastroResistant, opt => opt.MapFrom(src => src.IsGastroResistant == true))
                .ForMember(dest => dest.IsIndicationMandatory, opt => opt.MapFrom(src => src.IsIndicationMandatory == true));
        }

        private List<string> AssignMedusaPreparationInstructions(FormularyCreateModel src)
        {
            if (src.ProductType.IsNotEmpty() && (string.Compare(src.ProductType, "amp", true) == 0))
            {
                return src.MedusaPreparationInstructionsEditable != null? new List<string> { src.MedusaPreparationInstructionsEditable }: null;
            }
            else if (src.MedusaPreparationInstructions != null)
            {
                return src.MedusaPreparationInstructions.Select(rec => rec.Id).ToList();
            }
            return null;
        }

        private List<FormularyLookupAPIModel> AssignControlledDrugCategories(FormularyCreateModel src)
        {
            if (src.ProductType.IsNotEmpty() && (string.Compare(src.ProductType, "amp", true) == 0))
            {
                return src.ControlledDrugCategoriesEditableId != null ? new List<FormularyLookupAPIModel> { ConvertCodeNameModelWithDefaultsToCodeDescAPI(src.ControlledDrugCategoriesEditableId) } : null;
            }
            else if (src.ControlledDrugCategories != null)
            {
                return ConvertCodeNameModelsWithDefaultsToCodeDescAPI(src.ControlledDrugCategories);
            }
            return null;
        }

        private List<FormularyLookupAPIModel> AssignTitrationTypes(FormularyCreateModel src)
        {
            if (src.ProductType.IsNotEmpty() && (string.Compare(src.ProductType, "amp", true) == 0))
            {
                return src.TitrationTypesEditableId != null? new List<FormularyLookupAPIModel> { ConvertCodeNameModelWithDefaultsToCodeDescAPI(src.TitrationTypesEditableId) }: null;
            }
            else if (src.TitrationTypes != null)
            {
                return ConvertCodeNameModelsWithDefaultsToCodeDescAPI(src.TitrationTypes);
            }
            return null;
        }

        private string ConvertToBoolString(bool val)
        {
            return val ? "1" : "0";
        }

        private FormularyLookupAPIModel ConvertCodeNameModelWithDefaultsToCodeDescAPI(string code)
        {
            if (code == null) return null;

            return new FormularyLookupAPIModel
            {
                Cd = code,
                Desc = null,
                Source = TerminologyConstants.MANUAL_DATA_SOURCE
            };
        }


        private List<FormularyLookupAPIModel> ConvertCodeNameModelsWithDefaultsToCodeDescAPI(List<CodeNameSelectorModel> codeNameModel)
        {
            if (codeNameModel == null || !codeNameModel.IsCollectionValid()) return null;

            var codeNameList = codeNameModel.Select(rec =>
            {
                return new FormularyLookupAPIModel
                {
                    Cd = rec.Id,
                    Desc = rec.Name,
                    Source = rec.Source
                };
            }).ToList();

            return codeNameList;
        }

        private List<FormularyRouteDetailAPIModel> GetRoutes(FormularyCreateModel src)
        {
            var routesAPI = new List<FormularyRouteDetailAPIModel>();
            if (src == null || (!src.Route.IsCollectionValid() && !src.UnlicensedRoute.IsCollectionValid())) return null;

            if (src.Route.IsCollectionValid())
            {
                src.Route.Each(rec =>
                {
                    var routeAPIModel = new FormularyRouteDetailAPIModel
                    {
                        RouteCd = rec.Id,
                        RouteDesc = rec.Name,
                        RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD,
                        Source = rec.Source
                    };
                    routesAPI.Add(routeAPIModel);
                });
            }

            if (src.UnlicensedRoute.IsCollectionValid())
            {
                src.UnlicensedRoute.Each(rec =>
                {
                    var routeAPIModel = new FormularyRouteDetailAPIModel
                    {
                        RouteCd = rec.Id,
                        RouteDesc = rec.Name,
                        RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED_CD,
                        Source = rec.Source
                    };
                    routesAPI.Add(routeAPIModel);
                });
            }
            return routesAPI;
        }

        private List<FormularyLocalRouteDetailAPIModel> GetLocalRoutes(FormularyCreateModel src)
        {
            var localRoutesAPI = new List<FormularyLocalRouteDetailAPIModel>();
            if (src == null || (!src.LocalLicensedRoute.IsCollectionValid() && !src.LocalUnlicensedRoute.IsCollectionValid())) return null;

            if (src.LocalLicensedRoute.IsCollectionValid())
            {
                src.LocalLicensedRoute.Each(rec =>
                {
                    var routeAPIModel = new FormularyLocalRouteDetailAPIModel
                    {
                        RouteCd = rec.Id,
                        RouteDesc = rec.Name,
                        RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_NORMAL_CD,
                        Source = rec.Source
                    };
                    localRoutesAPI.Add(routeAPIModel);
                });
            }

            if (src.LocalUnlicensedRoute.IsCollectionValid())
            {
                src.LocalUnlicensedRoute.Each(rec =>
                {
                    var routeAPIModel = new FormularyLocalRouteDetailAPIModel
                    {
                        RouteCd = rec.Id,
                        RouteDesc = rec.Name,
                        RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED_CD,
                        Source = rec.Source
                    };
                    localRoutesAPI.Add(routeAPIModel);
                });
            }
            return localRoutesAPI;
        }

        public class AdditonalCodeResolver : IValueResolver<FormularyCreateModel, FormularyHeaderAPIModel, List<FormularyAdditionalCodeAPIModel>>
        {
            public List<FormularyAdditionalCodeAPIModel> Resolve(FormularyCreateModel source, FormularyHeaderAPIModel destination, List<FormularyAdditionalCodeAPIModel> destMember, ResolutionContext context)
            {
                var additionalCodes = new List<FormularyAdditionalCodeAPIModel>();

                if (destination == null) return additionalCodes;

                if (source.FormularyClassificationCodes.IsCollectionValid())
                {
                    additionalCodes = context.Mapper.Map<List<FormularyAdditionalCodeAPIModel>>(source.FormularyClassificationCodes);
                    additionalCodes.Each(rec => rec.CodeType = "Classification");
                }
                if (source.FormularyIdentificationCodes.IsCollectionValid())
                {
                    var idenCodes = context.Mapper.Map<List<FormularyAdditionalCodeAPIModel>>(source.FormularyIdentificationCodes);
                    idenCodes.Each(rec => rec.CodeType = "Identification");
                    additionalCodes.AddRange(idenCodes);
                }

                return additionalCodes;
            }
        }

        private class FormularyDetailConverter : IValueConverter<FormularyCreateModel, FormularyDetailAPIModel>
        {
            public FormularyDetailAPIModel Convert(FormularyCreateModel sourceMember, ResolutionContext context)
            {
                if (sourceMember == null) return null;

                var detail = context.Mapper.Map<FormularyDetailAPIModel>(sourceMember);

                return detail;
            }
        }

        private class FormularyIngredientsConverter : IValueConverter<FormularyCreateModel, List<FormularyIngredientAPIModel>>
        {
            public List<FormularyIngredientAPIModel> Convert(FormularyCreateModel sourceMember, ResolutionContext context)
            {
                if (sourceMember == null && !sourceMember.Ingredients.IsCollectionValid()) return null;

                var ingredients = context.Mapper.Map<List<FormularyIngredientAPIModel>>(sourceMember.Ingredients);

                return ingredients;
            }
        }

        private class FormularyExcipientsConverter : IValueConverter<FormularyCreateModel, List<FormularyExcipientAPIModel>>
        {
            public List<FormularyExcipientAPIModel> Convert(FormularyCreateModel sourceMember, ResolutionContext context)
            {
                if (sourceMember == null && !sourceMember.Ingredients.IsCollectionValid()) return null;

                var excipients = context.Mapper.Map<List<FormularyExcipientAPIModel>>(sourceMember.Excipients);

                return excipients;
            }
        }
    }
}

