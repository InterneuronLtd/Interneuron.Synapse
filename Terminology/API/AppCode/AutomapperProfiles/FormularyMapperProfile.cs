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
using Interneuron.Common.Extensions;
using Interneuron.FDBAPI.Client.DataModels;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Config;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Infrastructure.Domain.DSLs;
using Interneuron.Terminology.Model.DomainModels;
using Interneuron.Terminology.Model.Search;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Interneuron.Terminology.Model.History;
using Interneuron.Terminology.Model.Other;

namespace Interneuron.Terminology.API.AppCode.AutomapperProfiles
{
    public class FormularyMapperProfile : Profile
    {
        public FormularyMapperProfile()
        {

            CreateMap<LookupCommon, FormularyLookupItemDTO>();

            CreateMap<FormularyBasicSearchResultModel, FormularySearchResultWithTreeDTO>()
                .ForMember(dest => dest.LogicalLevel, opt => opt.MapFrom(src => src.ProductType.GetDMDLogicalLevelByLevelCode().GetValueOrDefault()));

            CreateMap<FormularyBasicSearchResultModel, FormularySearchResultDTO>()
                .ForMember(dest => dest.LogicalLevel, opt => opt.MapFrom(src => src.ProductType.GetDMDLogicalLevelByLevelCode().GetValueOrDefault()));

            CreateMap<FormularyHeader, FormularyDTO>()
               .ForMember(dest => dest.CodeSystem, opt => opt.MapFrom(src => src.CodeSystem.IsNotEmpty() ? src.CodeSystem.Trim() : TerminologyConstants.DEFAULT_IDENTIFICATION_CODE_SYSTEM));

            CreateMap<FormularyDetail, FormularyDetailDTO>()
                .ForMember(dest => dest.Cautions, opt => opt.MapFrom((srcData) => FillCodeDescList(srcData.Caution)))
                .ForMember(dest => dest.SafetyMessages, opt => opt.MapFrom((srcData) => FillCodeDescList(srcData.SafetyMessage)))
                .ForMember(dest => dest.SideEffects, opt => opt.MapFrom((srcData) => FillCodeDescList(srcData.SideEffect)))
                .ForMember(dest => dest.ContraIndications, opt => opt.MapFrom((srcData) => FillCodeDescList(srcData.ContraIndication)))
                .ForMember(dest => dest.CustomWarnings, opt => opt.MapFrom((srcData) => FillCustomWarnings(srcData.CustomWarning)))
                .ForMember(dest => dest.Reminders, opt => opt.MapFrom((srcData) => FillReminders(srcData.Reminder)))
                .ForMember(dest => dest.Endorsements, opt => opt.MapFrom((srcData) => SplitAndFillList(srcData.Endorsement)))
                .ForMember(dest => dest.LocalLicensedUses, opt => opt.MapFrom((srcData) => FillCodeDescList(srcData.LocalLicensedUse)))
                .ForMember(dest => dest.LocalUnLicensedUses, opt => opt.MapFrom((srcData) => FillCodeDescList(srcData.LocalUnlicensedUse)))
                .ForMember(dest => dest.LicensedUses, opt => opt.MapFrom((srcData) => FillCodeDescList(srcData.LicensedUse)))
                .ForMember(dest => dest.UnLicensedUses, opt => opt.MapFrom((srcData) => FillCodeDescList(srcData.UnlicensedUse)))
                .ForMember(dest => dest.BlackTriangleSource, opt => opt.MapFrom((srcData) => (srcData.BlackTriangle.IsNotEmpty() && srcData.BlackTriangle != "1" && srcData.BlackTriangleSource.IsEmpty()) ? TerminologyConstants.MANUAL_DATA_SRC : srcData.BlackTriangleSource))
                .ForMember(dest => dest.HighAlertMedicationSource, opt => opt.MapFrom((srcData) => (srcData.HighAlertMedication.IsNotEmpty() && srcData.HighAlertMedication != "1" && srcData.HighAlertMedicationSource.IsEmpty()) ? TerminologyConstants.MANUAL_DATA_SRC : srcData.HighAlertMedicationSource))
                .ForMember(dest => dest.MedusaPreparationInstructions, opt => opt.MapFrom((srcData) => srcData.MedusaPreparationInstructions.IsNotEmpty() ? new List<string> { srcData.MedusaPreparationInstructions } : null))
                .ForMember(dest => dest.NiceTas, opt => opt.MapFrom((srcData) => srcData.NiceTa.IsNotEmpty() ? new List<string> { srcData.NiceTa } : null))
                .ForMember(dest => dest.TitrationTypes, opt => opt.MapFrom((srcData) => srcData.TitrationTypeCd.IsNotEmpty() ? new List<FormularyLookupItemDTO> { new FormularyLookupItemDTO { Cd = srcData.TitrationTypeCd } } : null))
                .ForMember(dest => dest.ControlledDrugCategories, opt => opt.MapFrom((srcData) => srcData.ControlledDrugCategoryCd.IsNotEmpty() ? new List<FormularyLookupItemDTO> { new FormularyLookupItemDTO { Cd = srcData.ControlledDrugCategoryCd, Source = srcData.ControlledDrugCategorySource.IsEmpty()? TerminologyConstants.MANUAL_DATA_SRC: srcData.ControlledDrugCategorySource } } : null))
                .ForMember(dest => dest.Diluents, opt => opt.MapFrom((srcData) => FillCodeDescList(srcData.Diluent)))
                .ForMember(dest => dest.ModifiedReleases, opt => opt.MapFrom((srcData) => srcData.ModifiedReleaseCd.IsNotEmpty() ? new List<string> { srcData.ModifiedReleaseCd } : null));

            CreateMap<FormularyIngredient, FormularyIngredientDTO>();

            //CreateMap<FormularyOntologyForm, FormularyOntologyFormDTO>();

            CreateMap<FormularyAdditionalCode, FormularyAdditionalCodeDTO>()
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source.IsEmpty() ? TerminologyConstants.MANUAL_DATA_SRC : src.Source.Trim()));

            //CreateMap<FormularyIndication, FormularyIndicationDTO>();

            CreateMap<FormularyRouteDetail, FormularyRouteDetailDTO>();

            CreateMap<FormularyExcipient, FormularyExcipientDTO>();

            CreateMap<FormularyRuleConfig, FormularyRuleConfigDTO>();

            CreateMap<FormularyRuleConfigRequest, FormularyRuleConfig>();

            CreateMap<FormularySearchFilterRequest, FormularyFilterCriteria>()
                .ForMember(dest => dest.FormularyStatusCd, opt => opt.MapFrom(src => src.FormularyStatusCd))
                .ForMember(dest => dest.RecStatusCds, opt => opt.MapFrom(src => src.RecStatusCds))
                .ForMember(dest => dest.SearchTerm, opt => opt.MapFrom(src => src.SearchTerm))
                .ForMember(dest => dest.HideArchived, opt => opt.MapFrom(src => src.HideArchived))
                .ForMember(dest => dest.ShowOnlyDuplicate, opt => opt.MapFrom(src => src.ShowOnlyDuplicate))
                .ForMember(dest => dest.IncludeDeleted, opt => opt.MapFrom(src => src.IncludeDeleted));

            CreateMap<FormularyHeader, FormularyHeader>()
                .ForMember(dest => dest.NameTokens, opt => opt.Ignore())
                .ForMember(dest => dest.ParentNameTokens, opt => opt.Ignore())
                .ForMember(dest => dest.CodeSystem, opt => opt.MapFrom(src => src.CodeSystem.IsNotEmpty() ? src.CodeSystem.Trim() : TerminologyConstants.DEFAULT_IDENTIFICATION_CODE_SYSTEM));

            CreateMap<FormularyDetail, FormularyDetail>();

            CreateMap<FormularyIngredient, FormularyIngredient>();

            CreateMap<FormularyExcipient, FormularyExcipient>();

            CreateMap<FormularyAdditionalCode, FormularyAdditionalCode>();

            CreateMap<FormularyIndication, FormularyIndication>();

            CreateMap<FormularyRouteDetail, FormularyRouteDetail>();

            CreateMap<FormularyOntologyForm, FormularyOntologyForm>();

            CreateMap<FormularyDTO, FormularyHeader>()
                .ForMember(dest => dest.CodeSystem, opt => opt.MapFrom(src => src.CodeSystem.IsNotEmpty() ? src.CodeSystem.Trim() : TerminologyConstants.DEFAULT_IDENTIFICATION_CODE_SYSTEM));

            CreateMap<FormularyDetailDTO, FormularyDetail>()
                .ForMember(dest => dest.RnohFormularyStatuscd, opt => opt.MapFrom(src => src.RnohFormularyStatuscd ?? TerminologyConstants.FORMULARYSTATUS_FORMULARY))
                .ForMember(dest => dest.Caution, opt => opt.MapFrom((srcData) => StringifyFDBCodeDescData(srcData.Cautions)))
                .ForMember(dest => dest.SafetyMessage, opt => opt.MapFrom((srcData) => StringifyFDBCodeDescData(srcData.SafetyMessages)))
                .ForMember(dest => dest.SideEffect, opt => opt.MapFrom((srcData) => StringifyFDBCodeDescData(srcData.SideEffects)))
                .ForMember(dest => dest.ContraIndication, opt => opt.MapFrom((srcData) => StringifyFDBCodeDescData(srcData.ContraIndications)))
                .ForMember(dest => dest.CustomWarning, opt => opt.MapFrom((srcData) => StringifyCustomWarnings(srcData.CustomWarnings)))
                .ForMember(dest => dest.Reminder, opt => opt.MapFrom((srcData) => StringifyReminders(srcData.Reminders)))
                .ForMember(dest => dest.Endorsement, opt => opt.MapFrom((srcData) => FlattenToString(srcData.Endorsements)))
                .ForMember(dest => dest.LocalLicensedUse, opt => opt.MapFrom((srcData) => StringifyFDBCodeDescData(srcData.LocalLicensedUses)))
                .ForMember(dest => dest.LocalUnlicensedUse, opt => opt.MapFrom((srcData) => StringifyFDBCodeDescData(srcData.LocalUnLicensedUses)))
                .ForMember(dest => dest.LicensedUse, opt => opt.MapFrom((srcData) => StringifyFDBCodeDescData(srcData.LicensedUses)))
                .ForMember(dest => dest.UnlicensedUse, opt => opt.MapFrom((srcData) => StringifyFDBCodeDescData(srcData.UnLicensedUses)))
                .ForMember(dest => dest.BlackTriangleSource, opt => opt.MapFrom((srcData) => (srcData.BlackTriangle.IsNotEmpty() && srcData.BlackTriangleSource.IsEmpty()) ? TerminologyConstants.MANUAL_DATA_SRC : srcData.BlackTriangleSource))
                .ForMember(dest => dest.HighAlertMedicationSource, opt => opt.MapFrom((srcData) => (srcData.HighAlertMedication.IsNotEmpty() && srcData.HighAlertMedicationSource.IsEmpty()) ? TerminologyConstants.MANUAL_DATA_SRC : srcData.HighAlertMedicationSource))
                .ForMember(dest => dest.ControlledDrugCategoryCd, opt => opt.MapFrom(srcData => srcData.ControlledDrugCategories.IsCollectionValid() ? srcData.ControlledDrugCategories.First().Cd : null))
                .ForMember(dest => dest.ControlledDrugCategorySource, opt => opt.MapFrom((srcData) => !srcData.ControlledDrugCategories.IsCollectionValid() ? TerminologyConstants.MANUAL_DATA_SRC : srcData.ControlledDrugCategories.First().Source))
                .ForMember(dest => dest.MedusaPreparationInstructions, opt => opt.MapFrom((srcData) => srcData.MedusaPreparationInstructions.FirstOrDefault()))
                .ForMember(dest => dest.NiceTa, opt => opt.MapFrom((srcData) => srcData.NiceTas.FirstOrDefault()))
                .ForMember(dest => dest.TitrationTypeCd, opt => opt.MapFrom((srcData) => srcData.TitrationTypes.IsCollectionValid() ? srcData.TitrationTypes.First().Cd : null))
                .ForMember(dest => dest.Diluent, opt => opt.MapFrom((srcData) => StringifyFDBCodeDescData(srcData.Diluents)));

            CreateMap<FormularyAdditionalCodeDTO, FormularyAdditionalCode>()
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source.IsEmpty() ? TerminologyConstants.MANUAL_DATA_SRC : src.Source.Trim()))
;
            CreateMap<FormularyRouteDetailDTO, FormularyRouteDetail>();

            CreateMap<FormularyIngredientDTO, FormularyIngredient>();

            CreateMap<FormularyExcipientDTO, FormularyExcipient>();

            CreateMap<DmdLookupRouteDTO, FormularyRouteDetail>()
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => src.Source.IsEmpty() ? TerminologyConstants.MANUAL_DATA_SRC : src.Source.Trim()))
                .ForMember(dest => dest.RouteCd, opt => opt.MapFrom(src => src.Cd))
                .ForMember(dest => dest.RouteFieldTypeCd, opt => opt.MapFrom(src => TerminologyConstants.ROUTEFIELDTYPE_NORMAL)); //Normal

            CreateMap<DmdAmpDrugrouteDTO, FormularyRouteDetail>()
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => TerminologyConstants.MANUAL_DATA_SRC))
                .ForMember(dest => dest.RouteCd, opt => opt.MapFrom(src => src.Routecd))
                .ForMember(dest => dest.RouteFieldTypeCd, opt => opt.MapFrom(src => TerminologyConstants.ROUTEFIELDTYPE_NORMAL)); //Normal

            CreateMap<DmdVmpDrugrouteDTO, FormularyRouteDetail>()
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => TerminologyConstants.MANUAL_DATA_SRC))
                .ForMember(dest => dest.RouteCd, opt => opt.MapFrom(src => src.Routecd))
                .ForMember(dest => dest.RouteFieldTypeCd, opt => opt.MapFrom(src => TerminologyConstants.ROUTEFIELDTYPE_ADDITONAL)); //Additional

            CreateMap<DmdAmpExcipientDTO, FormularyExcipient>()
                .ForMember(dest => dest.IngredientCd, opt => opt.MapFrom(src => src.Isid))
                .ForMember(dest => dest.Strength, opt => opt.MapFrom(src => src.Strnth))
                .ForMember(dest => dest.StrengthUnitCd, opt => opt.MapFrom(src => src.StrnthUomcd));

            CreateMap<DmdATCCodeDTO, FormularyAdditionalCode>()
                .ForMember(dest => dest.AdditionalCode, opt => opt.MapFrom(src => src.Cd))
                .ForMember(dest => dest.AdditionalCodeDesc, opt => opt.MapFrom(src => src.Desc))
                .ForMember(dest => dest.AdditionalCodeSystem, opt => opt.MapFrom(src => "ATC"))
                .ForMember(dest => dest.CodeType, opt => opt.MapFrom(src => TerminologyConstants.CODE_SYSTEM_CLASSIFICATION_TYPE))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => TerminologyConstants.DMD_DATA_SRC));

            CreateMap<DmdBNFCodeDTO, FormularyAdditionalCode>()
                .ForMember(dest => dest.AdditionalCode, opt => opt.MapFrom(src => src.Cd))
                .ForMember(dest => dest.AdditionalCodeDesc, opt => opt.MapFrom(src => src.Desc))
                .ForMember(dest => dest.AdditionalCodeSystem, opt => opt.MapFrom(src => "BNF"))
                .ForMember(dest => dest.CodeType, opt => opt.MapFrom(src => TerminologyConstants.CODE_SYSTEM_CLASSIFICATION_TYPE))
                .ForMember(dest => dest.Source, opt => opt.MapFrom(src => TerminologyConstants.DMD_DATA_SRC));

            CreateMap<FormularyRouteDetail, FormularyLocalRouteDetailDTO>();

            CreateMap<FormularyLocalRouteDetailDTO, FormularyLocalRouteDetail>();

            CreateMap<FormularyLocalRouteDetail, FormularyLocalRouteDetailDTO>();

            CreateMap<FormularyHistoryModel, FormularyHistoryDTO>();

            CreateMap<FormularyLocalLicensedUseModel, FormularyLocalLicensedUseDTO>();

            CreateMap<FormularyLocalUnlicensedUseModel, FormularyLocalUnlicensedUseDTO>();

            CreateMap<FormularyLocalLicensedRouteModel, FormularyLocalLicensedRouteDTO>();

            CreateMap<FormularyLocalUnlicensedRouteModel, FormularyLocalUnlicensedRouteDTO>();

            CreateMap<CustomWarningModel, CustomWarningDTO>();

            CreateMap<ReminderModel, ReminderDTO>();

            CreateMap<EndorsementModel, EndorsementDTO>();

            CreateMap<MedusaPreparationInstructionModel, MedusaPreparationInstructionDTO>();

            CreateMap<TitrationTypeModel, TitrationTypeDTO>();

            CreateMap<RoundingFactorModel, RoundingFactorDTO>();

            CreateMap<CompatibleDiluentModel, CompatibleDiluentDTO>();

            CreateMap<ClinicalTrialMedicationModel, ClinicalTrialMedicationDTO>();

            CreateMap<GastroResistantModel, GastroResistantDTO>();

            CreateMap<CriticalDrugModel, CriticalDrugDTO>();

            CreateMap<ModifiedReleaseModel, ModifiedReleaseDTO>();

            CreateMap<ExpensiveMedicationModel, ExpensiveMedicationDTO>();

            CreateMap<HighAlertMedicationModel, HighAlertMedicationDTO>();

            CreateMap<IVToOralModel, IVToOralDTO>();

            CreateMap<NotForPRNModel, NotForPRNDTO>();

            CreateMap<BloodProductModel, BloodProductDTO>();

            CreateMap<DiluentModel, DiluentDTO>();

            CreateMap<PrescribableModel, PrescribableDTO>();

            CreateMap<OutpatientMedicationModel, OutpatientMedicationDTO>();

            CreateMap<IgnoreDuplicateWarningModel, IgnoreDuplicateWarningDTO>();

            CreateMap<ControlledDrugModel, ControlledDrugDTO>();

            CreateMap<IndicationMandatoryModel, IndicationMandatoryDTO>();

            CreateMap<WitnessingRequiredModel, WitnessingRequiredDTO>();

            CreateMap<FormularyStatusModel, FormularyStatusDTO>();
        }

        private List<FormularyLookupItemDTO> FillCodeDescList(string dataAsString)
        {
            if (dataAsString.IsEmpty()) return null;

            var dataAsList = JsonConvert.DeserializeObject<List<FormularyLookupItemDTO>>(dataAsString);//id and text

            if (dataAsList == null) return null;

            return dataAsList;
        }

        private List<FormularyCustomWarningDTO> FillCustomWarnings(string dataAsString)
        {
            if (dataAsString.IsEmpty()) return null;

            var dataAsList = JsonConvert.DeserializeObject<List<FormularyCustomWarningDTO>>(dataAsString);//id and text

            if (dataAsList == null) return null;

            return dataAsList;
        }

        private List<FormularyReminderDTO> FillReminders(string dataAsString)
        {
            if (dataAsString.IsEmpty()) return null;

            var dataAsList = JsonConvert.DeserializeObject<List<FormularyReminderDTO>>(dataAsString);//id and text

            if (dataAsList == null) return null;

            return dataAsList;
        }

        private string StringifyCustomWarnings(List<FormularyCustomWarningDTO> warnings)
        {
            if (!warnings.IsCollectionValid()) return null;

            var sringified = JsonConvert.SerializeObject(warnings);
            return sringified;
        }

        private string StringifyReminders(List<FormularyReminderDTO> reminders)
        {
            if (!reminders.IsCollectionValid()) return null;

            var stringified = JsonConvert.SerializeObject(reminders);
            return stringified;
        }

        private string StringifyFDBCodeDescData(List<FormularyLookupItemDTO> codeDescList)
        {
            if (!codeDescList.IsCollectionValid()) return null;

            var sringified = JsonConvert.SerializeObject(codeDescList);
            return sringified;
        }

        protected List<string> SplitAndFillList(string dataAsJson)
        {
            if (dataAsJson.IsEmpty()) return null;

            return JsonConvert.DeserializeObject<List<string>>(dataAsJson);
        }

        protected string FlattenToString(List<string> list)
        {
            if (!list.IsCollectionValid()) return null;

            return JsonConvert.SerializeObject(list);
        }
    }
}

