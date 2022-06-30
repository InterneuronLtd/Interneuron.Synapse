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
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Commands.ImportMergeHandlers
{
    public class VTMImportMergeHandler : ImportMergeHandler
    {
        private IMapper _mapper;
        private DMDDetailResultDTO _dMDDetailResultDTO;
        private FormularyHeader _formularyDAO;
        private FormularyHeader _existingFormulary;

        public VTMImportMergeHandler(IMapper mapper, DMDDetailResultDTO dMDDetailResultDTO, FormularyHeader formularyDAO, FormularyHeader existingFormulary)
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
            _formularyDAO.RecStatusCode = null;
            // _existingFormulary.RecStatusCode == TerminologyConstants.RECORDSTATUS_ACTIVE ? TerminologyConstants.RECORDSTATUS_ACTIVE : TerminologyConstants.RECORDSTATUS_DRAFT;

            MergeFormularyAdditionalCodes();
            MergeFormularyDetail();
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
            if (!_existingFormulary.FormularyDetail.IsCollectionValid() || _formularyDAO .FormularyDetail.IsCollectionValid()) return;

            var existingDetailFromDb = _existingFormulary.FormularyDetail.First();

            var _formularyDetailFromSrc = _formularyDAO.FormularyDetail.First();

            if (!(existingDetailFromDb.Prescribable == false && string.Compare(existingDetailFromDb.PrescribableSource, TerminologyConstants.DMD_DATA_SRC, true) == 0))
            {
                _formularyDetailFromSrc.Prescribable = existingDetailFromDb.Prescribable;
                _formularyDetailFromSrc.PrescribableSource = existingDetailFromDb.PrescribableSource;
            }

            _formularyDetailFromSrc.WitnessingRequired = existingDetailFromDb.WitnessingRequired;
        }
    }
}
