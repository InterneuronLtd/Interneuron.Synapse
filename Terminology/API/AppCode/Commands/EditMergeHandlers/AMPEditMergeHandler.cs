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
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Commands.EditMergeHandlers
{
    public class AMPEditMergeHandler : FormularyMergeHandler
    {
        private IMapper _mapper;

        public AMPEditMergeHandler(IMapper mapper) : base(mapper)
        {
            _mapper = mapper;
        }

        public override FormularyHeader Merge(FormularyHeader existingFormulary, FormularyDTO dto)
        {
            if (existingFormulary == null || dto == null) return null;

            var newFormulary = CloneFormulary(existingFormulary);

            newFormulary.RecStatusCode = dto.RecStatusCode.IsEmpty() ? newFormulary.RecStatusCode : dto.RecStatusCode;

            MergeFormularyDetail(newFormulary, dto);

            MergeFormularyRoutes(newFormulary, dto);

            MergeFormularyLocalRoutes(newFormulary, dto);

            MergeFormularyAdditionalCodes(newFormulary, dto);

            return newFormulary;
        }

        private void MergeFormularyAdditionalCodes(FormularyHeader existingFormularyFromDb, FormularyDTO sourceDTO)
        {
            if (!existingFormularyFromDb.FormularyAdditionalCode.IsCollectionValid() && !sourceDTO.FormularyAdditionalCodes.IsCollectionValid()) return;

            existingFormularyFromDb.FormularyAdditionalCode = existingFormularyFromDb.FormularyAdditionalCode.IsCollectionValid() ? existingFormularyFromDb.FormularyAdditionalCode : new List<FormularyAdditionalCode>();

            var additionalCodeDTOsInDb = _mapper.Map<List<FormularyAdditionalCodeDTO>>(existingFormularyFromDb.FormularyAdditionalCode);
            var customManualSourceItems = additionalCodeDTOsInDb.Where(rec => rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty()).ToList();

            //Delete routes from custom source - manually entered
            if (customManualSourceItems.IsCollectionValid())
                customManualSourceItems.Each(rec => additionalCodeDTOsInDb.Remove(rec));

            if (sourceDTO.FormularyAdditionalCodes.IsCollectionValid())
            {
                var destinationItemsCodes = additionalCodeDTOsInDb.Select(rec => rec.AdditionalCode).ToList();

                //Get custom added additional codes
                var manualSourceLookupItems = sourceDTO.FormularyAdditionalCodes.Where(rec => !destinationItemsCodes.Contains(rec.AdditionalCode) && (rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty())).ToList();

                manualSourceLookupItems?.Each(rec =>
                {
                    rec.Source = TerminologyConstants.MANUAL_DATA_SRC;
                    additionalCodeDTOsInDb.Add(rec);
                });
            }

            existingFormularyFromDb.FormularyAdditionalCode = _mapper.Map<List<FormularyAdditionalCode>>(additionalCodeDTOsInDb);
        }

        private void MergeFormularyRoutes(FormularyHeader existingFormularyFromDb, FormularyDTO dto)
        {
            //This addition is only for the un-licensed routes. Licensed routes is un-touched
            if (!dto.FormularyRouteDetails.IsCollectionValid() && !existingFormularyFromDb.FormularyRouteDetail.IsCollectionValid()) return;

            existingFormularyFromDb.FormularyRouteDetail = existingFormularyFromDb.FormularyRouteDetail.IsCollectionValid() ? existingFormularyFromDb.FormularyRouteDetail : new List<FormularyRouteDetail>();

            var routeDTOsInDb = _mapper.Map<List<FormularyRouteDetailDTO>>(existingFormularyFromDb.FormularyRouteDetail);

            var customManualSourceItems = routeDTOsInDb.Where(rec => (rec.RouteFieldTypeCd == TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED) && (rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty())).ToList();

            //Delete routes from custom source - manually entered
            if (customManualSourceItems.IsCollectionValid())
                customManualSourceItems.Each(rec => routeDTOsInDb.Remove(rec));

            if (dto.FormularyRouteDetails.IsCollectionValid())
            {
                var destinationItemsCodes = routeDTOsInDb.Select(rec => rec.RouteCd).ToList();

                //Get custom added routes
                var manualSourceLookupItems = dto.FormularyRouteDetails.Where(rec => !destinationItemsCodes.Contains(rec.RouteCd) && ((rec.RouteFieldTypeCd == TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED) && (rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty()))).ToList();

                manualSourceLookupItems?.Each(rec =>
                {
                    rec.Source = TerminologyConstants.MANUAL_DATA_SRC;
                    routeDTOsInDb.Add(rec);
                });
            }

            existingFormularyFromDb.FormularyRouteDetail = _mapper.Map<List<FormularyRouteDetail>>(routeDTOsInDb);
        }

        private void MergeFormularyLocalRoutes(FormularyHeader existingFormularyFromDb, FormularyDTO dto)
        {
            //This addition is only for the un-licensed routes. Licensed routes is un-touched
            if (!dto.FormularyLocalRouteDetails.IsCollectionValid() && !existingFormularyFromDb.FormularyLocalRouteDetail.IsCollectionValid()) return;

            existingFormularyFromDb.FormularyLocalRouteDetail = existingFormularyFromDb.FormularyLocalRouteDetail.IsCollectionValid() ? existingFormularyFromDb.FormularyLocalRouteDetail : new List<FormularyLocalRouteDetail>();

            var localRouteDTOsInDb = _mapper.Map<List<FormularyLocalRouteDetailDTO>>(existingFormularyFromDb.FormularyLocalRouteDetail);

            var customManualSourceItems = localRouteDTOsInDb.Where(rec => (rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty())).ToList();

            //Delete routes from custom source - manually entered
            if (customManualSourceItems.IsCollectionValid())
                customManualSourceItems.Each(rec => localRouteDTOsInDb.Remove(rec));

            if (dto.FormularyLocalRouteDetails.IsCollectionValid())
            {
                var destinationItemsCodes = localRouteDTOsInDb.Select(rec => rec.RouteCd).ToList();

                //Get custom added routes
                var manualSourceLookupItems = dto.FormularyLocalRouteDetails.Where(rec => !destinationItemsCodes.Contains(rec.RouteCd) && ((rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty()))).ToList();

                manualSourceLookupItems?.Each(rec =>
                {
                    rec.Source = TerminologyConstants.MANUAL_DATA_SRC;
                    localRouteDTOsInDb.Add(rec);
                });
            }

            existingFormularyFromDb.FormularyLocalRouteDetail = _mapper.Map<List<FormularyLocalRouteDetail>>(localRouteDTOsInDb);
        }

        private void MergeFormularyDetail(FormularyHeader existingFormularyFromDb, FormularyDTO dto)
        {
            if (dto.Detail == null || !existingFormularyFromDb.FormularyDetail.IsCollectionValid()) return;

            var detailFromDb = existingFormularyFromDb.FormularyDetail.First();

            var detailFromDbAsDTO = _mapper.Map<FormularyDetailDTO>(detailFromDb);

            var detailSourceDTO = dto.Detail;

            //No need to consider licensed indications --Retain as is in DB
            //detailFromDbAsDTO.LicensedUses = HandleFormularyLookupItems(detailSourceDTO.LicensedUses, detailFromDbAsDTO.LicensedUses);
            detailFromDbAsDTO.UnLicensedUses = HandleFormularyLookupItems(detailSourceDTO.UnLicensedUses, detailFromDbAsDTO.UnLicensedUses);

            detailFromDbAsDTO.LocalLicensedUses = HandleFormularyLookupItems(detailSourceDTO.LocalLicensedUses, detailFromDbAsDTO.LocalLicensedUses);
            detailFromDbAsDTO.LocalUnLicensedUses = HandleFormularyLookupItems(detailSourceDTO.LocalUnLicensedUses, detailFromDbAsDTO.LocalUnLicensedUses);

            detailFromDbAsDTO.RoundingFactorCd = detailSourceDTO.RoundingFactorCd;

            detailFromDbAsDTO.CustomWarnings = detailSourceDTO.CustomWarnings;
            detailFromDbAsDTO.Reminders = detailSourceDTO.Reminders;
            detailFromDbAsDTO.Endorsements = detailSourceDTO.Endorsements;
            detailFromDbAsDTO.MedusaPreparationInstructions = detailSourceDTO.MedusaPreparationInstructions;
            detailFromDbAsDTO.NiceTas = detailSourceDTO.NiceTas;
            detailFromDbAsDTO.SideEffects = HandleFormularyLookupItems(detailSourceDTO.SideEffects, detailFromDbAsDTO.SideEffects);
            detailFromDbAsDTO.Cautions = HandleFormularyLookupItems(detailSourceDTO.Cautions, detailFromDbAsDTO.Cautions);
            detailFromDbAsDTO.ClinicalTrialMedication = detailSourceDTO.ClinicalTrialMedication;
            detailFromDbAsDTO.CriticalDrug = detailSourceDTO.CriticalDrug;
            detailFromDbAsDTO.IvToOral = detailSourceDTO.IvToOral;
            detailFromDbAsDTO.ExpensiveMedication = detailSourceDTO.ExpensiveMedication;

            if (string.Compare(detailFromDbAsDTO.HighAlertMedicationSource, TerminologyConstants.FDB_DATA_SRC, true) != 0)
            {
                detailFromDbAsDTO.HighAlertMedication = detailSourceDTO.HighAlertMedication;
                detailFromDbAsDTO.HighAlertMedicationSource = TerminologyConstants.MANUAL_DATA_SRC;
            }

            detailFromDbAsDTO.NotForPrn = detailSourceDTO.NotForPrn;

            //not editable when set from dm+d  
            if (!(detailFromDb.Prescribable == false && string.Compare(detailFromDb.PrescribableSource, TerminologyConstants.DMD_DATA_SRC, true) == 0))
            {
                detailFromDbAsDTO.Prescribable = detailSourceDTO.Prescribable;
                detailFromDbAsDTO.PrescribableSource = detailSourceDTO.PrescribableSource;
            }

            detailFromDbAsDTO.OutpatientMedicationCd = detailSourceDTO.OutpatientMedicationCd;
            detailFromDbAsDTO.TitrationTypes = HandleFormularyLookupItems(detailSourceDTO.TitrationTypes, detailFromDbAsDTO.TitrationTypes);
            detailFromDbAsDTO.RnohFormularyStatuscd = detailSourceDTO.RnohFormularyStatuscd;
            detailFromDbAsDTO.IgnoreDuplicateWarnings = detailSourceDTO.IgnoreDuplicateWarnings;
            detailFromDbAsDTO.WitnessingRequired = detailSourceDTO.WitnessingRequired;
            detailFromDbAsDTO.IsBloodProduct = detailSourceDTO.IsBloodProduct;
            detailFromDbAsDTO.IsDiluent = detailSourceDTO.IsDiluent;
            detailFromDbAsDTO.IsCustomControlledDrug = detailSourceDTO.IsCustomControlledDrug;
            detailFromDbAsDTO.IsPrescriptionPrintingRequired = detailSourceDTO.IsPrescriptionPrintingRequired;
            detailFromDbAsDTO.Diluents = detailSourceDTO.Diluents;
            detailFromDbAsDTO.IsIndicationMandatory = detailSourceDTO.IsIndicationMandatory;
            detailFromDbAsDTO.IsGastroResistant = detailSourceDTO.IsGastroResistant;
            detailFromDbAsDTO.IsModifiedRelease = detailSourceDTO.IsModifiedRelease;

            detailFromDb = _mapper.Map(detailFromDbAsDTO, detailFromDb);
        }
    }
}
