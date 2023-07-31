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
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Commands.ImportMergeHandlers
{
    public class AMPImportMergeHandler : ImportMergeHandler
    {
        private IMapper _mapper;
        private DMDDetailResultDTO _dMDDetailResultDTO;
        private FormularyHeader _formularyDAO;
        private FormularyHeader _existingFormulary;

        public AMPImportMergeHandler(IMapper mapper, DMDDetailResultDTO dMDDetailResultDTO, FormularyHeader formularyDAO, FormularyHeader existingFormulary)
        {
            _mapper = mapper;
            _dMDDetailResultDTO = dMDDetailResultDTO;

            _formularyDAO = formularyDAO;
            _existingFormulary = existingFormulary;
        }

        public override void MergeFromExisting()
        {
            _formularyDAO.FormularyId = _existingFormulary.FormularyId;
            _formularyDAO.VersionId = _existingFormulary.VersionId + 1;
            _formularyDAO.RecStatusCode = TerminologyConstants.RECORDSTATUS_DRAFT;

            //Merge only if the previous record is active
            if (_existingFormulary.RecStatusCode == TerminologyConstants.RECORDSTATUS_ACTIVE)
            {
                _formularyDAO.RecStatusCode = TerminologyConstants.RECORDSTATUS_ACTIVE;

                MergeFormularyRoutes();
                MergeFormularyAdditionalCodes();
                MergeFormularyDetail();
            }
        }

        private void MergeFormularyRoutes()
        {
            //This addition is only for the un-licensed routes. Licensed routes is un-touched
            if (!_existingFormulary.FormularyRouteDetail.IsCollectionValid()) return;

            _formularyDAO.FormularyRouteDetail = _formularyDAO.FormularyRouteDetail.IsCollectionValid() ? _formularyDAO.FormularyRouteDetail : new List<FormularyRouteDetail>();

            var routeDTOsInDb = _mapper.Map<List<FormularyRouteDetailDTO>>(_existingFormulary.FormularyRouteDetail);
            var routeDTOsInSrc = _mapper.Map<List<FormularyRouteDetailDTO>>(_formularyDAO.FormularyRouteDetail);

            var manuallyAddedItemsInDb = routeDTOsInDb.Where(rec => (rec.RouteFieldTypeCd == TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED) && (rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty())).ToList();

            manuallyAddedItemsInDb?.Each(rec =>
            {
                rec.Source = TerminologyConstants.MANUAL_DATA_SRC;
                routeDTOsInSrc.Add(rec);
            });

            _formularyDAO.FormularyRouteDetail = _mapper.Map<List<FormularyRouteDetail>>(routeDTOsInSrc);
        }

        private void MergeFormularyAdditionalCodes()
        {
            if (!_existingFormulary.FormularyAdditionalCode.IsCollectionValid()) return;

            var existingFormularyAddtionalCodesDTO = _mapper.Map<List<FormularyAdditionalCodeDTO>>(_existingFormulary.FormularyAdditionalCode);

            if (!_formularyDAO.FormularyAdditionalCode.IsCollectionValid() && !existingFormularyAddtionalCodesDTO.IsCollectionValid()) return;

            _formularyDAO.FormularyAdditionalCode = _formularyDAO.FormularyAdditionalCode.IsCollectionValid() ? _formularyDAO.FormularyAdditionalCode : new List<FormularyAdditionalCode>();

            var additionalCodeDTOs = _mapper.Map<List<FormularyAdditionalCodeDTO>>(_formularyDAO.FormularyAdditionalCode);

            var customManualSourceItems = additionalCodeDTOs.Where(rec => (string.Compare(rec.CodeType, "identification", true) == 0) && (rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty())).ToList();

            //Delete codes from custom source - manually entered
            if (customManualSourceItems.IsCollectionValid())
                customManualSourceItems.Each(rec => additionalCodeDTOs.Remove(rec));

            if (existingFormularyAddtionalCodesDTO.IsCollectionValid())
            {
                var destinationItemsCodes = additionalCodeDTOs.Select(rec => rec.AdditionalCode).ToList();

                //Get custom added additional codes
                var manualSourceLookupItems = existingFormularyAddtionalCodesDTO.Where(rec => (!destinationItemsCodes.Contains(rec.AdditionalCode)) && (string.Compare(rec.CodeType, "identification", true) == 0) && (rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty())).ToList();

                manualSourceLookupItems?.Each(rec =>
                {
                    rec.Source = TerminologyConstants.MANUAL_DATA_SRC;
                    additionalCodeDTOs.Add(rec);
                });
            }

            _formularyDAO.FormularyAdditionalCode = _mapper.Map<List<FormularyAdditionalCode>>(additionalCodeDTOs);
        }

        private void MergeFormularyDetail()
        {
            if (!_existingFormulary.FormularyDetail.IsCollectionValid() || _formularyDAO.FormularyDetail.IsCollectionValid()) return;

            var existingDetailFromDb = _existingFormulary.FormularyDetail.First();

            var _formularyDetailFromSrc = _formularyDAO.FormularyDetail.First();

            var detailDTOFromSrc = _mapper.Map<FormularyDetailDTO>(_formularyDetailFromSrc);
            var detailDTOFromDb = _mapper.Map<FormularyDetailDTO>(existingDetailFromDb);

            detailDTOFromSrc.UnLicensedUses = HandleFormularyLookupItems(detailDTOFromDb.UnLicensedUses, detailDTOFromSrc.UnLicensedUses);

            detailDTOFromSrc.RoundingFactorCd = detailDTOFromDb.RoundingFactorCd;

            detailDTOFromSrc.CustomWarnings = detailDTOFromDb.CustomWarnings;
            detailDTOFromSrc.Reminders = detailDTOFromDb.Reminders;
            detailDTOFromSrc.Endorsements = detailDTOFromDb.Endorsements;
            detailDTOFromSrc.MedusaPreparationInstructions = detailDTOFromDb.MedusaPreparationInstructions;
            detailDTOFromSrc.NiceTas = detailDTOFromDb.NiceTas;
            detailDTOFromSrc.SideEffects = HandleFormularyLookupItems(detailDTOFromDb.SideEffects, detailDTOFromSrc.SideEffects);
            detailDTOFromSrc.Cautions = HandleFormularyLookupItems(detailDTOFromDb.Cautions, detailDTOFromSrc.Cautions);
            detailDTOFromSrc.ClinicalTrialMedication = detailDTOFromDb.ClinicalTrialMedication;
            detailDTOFromSrc.CriticalDrug = detailDTOFromDb.CriticalDrug;
            detailDTOFromSrc.IvToOral = detailDTOFromDb.IvToOral;
            detailDTOFromSrc.ExpensiveMedication = detailDTOFromDb.ExpensiveMedication;

            if (string.Compare(detailDTOFromSrc.HighAlertMedicationSource, TerminologyConstants.FDB_DATA_SRC, true) != 0)
            {
                detailDTOFromSrc.HighAlertMedication = detailDTOFromDb.HighAlertMedication;
                detailDTOFromSrc.HighAlertMedicationSource = TerminologyConstants.MANUAL_DATA_SRC;
            }

            detailDTOFromSrc.NotForPrn = detailDTOFromDb.NotForPrn;

            //not editable when set from dm+d  
            if (!(detailDTOFromSrc.Prescribable == false && string.Compare(detailDTOFromSrc.PrescribableSource, TerminologyConstants.DMD_DATA_SRC, true) == 0))
            {
                detailDTOFromSrc.Prescribable = detailDTOFromDb.Prescribable;
                detailDTOFromSrc.PrescribableSource = detailDTOFromDb.PrescribableSource;
            }

            detailDTOFromSrc.OutpatientMedicationCd = detailDTOFromDb.OutpatientMedicationCd;
            detailDTOFromSrc.TitrationTypes = HandleFormularyLookupItems(detailDTOFromDb.TitrationTypes, detailDTOFromSrc.TitrationTypes);
            detailDTOFromSrc.RnohFormularyStatuscd = detailDTOFromDb.RnohFormularyStatuscd;
            detailDTOFromSrc.IgnoreDuplicateWarnings = detailDTOFromDb.IgnoreDuplicateWarnings;
            detailDTOFromSrc.WitnessingRequired = detailDTOFromDb.WitnessingRequired;
            detailDTOFromSrc.IsBloodProduct = detailDTOFromDb.IsBloodProduct;
            detailDTOFromSrc.IsDiluent = detailDTOFromDb.IsDiluent;
            detailDTOFromSrc.IsCustomControlledDrug = detailDTOFromDb.IsCustomControlledDrug;
            detailDTOFromSrc.IsPrescriptionPrintingRequired = detailDTOFromDb.IsPrescriptionPrintingRequired;
            detailDTOFromSrc.Diluents = detailDTOFromDb.Diluents;
            detailDTOFromSrc.IsIndicationMandatory = detailDTOFromDb.IsIndicationMandatory;

            detailDTOFromSrc = _mapper.Map(detailDTOFromSrc, detailDTOFromSrc);
        }
    }
}
