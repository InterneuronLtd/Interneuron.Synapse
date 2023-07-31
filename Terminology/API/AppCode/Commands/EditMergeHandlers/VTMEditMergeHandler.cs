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
    public class VTMEditMergeHandler : FormularyMergeHandler
    {
        private IMapper _mapper;

        public VTMEditMergeHandler(IMapper mapper) : base(mapper)
        {
            _mapper = mapper;
        }

        public override FormularyHeader Merge(FormularyHeader existingFormulary, FormularyDTO dto)
        {
            if (existingFormulary == null || dto == null) return null;

            var newFormulary = base.CloneFormulary(existingFormulary);

            newFormulary.RecStatusCode = dto.RecStatusCode.IsEmpty() ? newFormulary.RecStatusCode : dto.RecStatusCode;

            MergeFormularyDetail(newFormulary, dto);

            MergeFormularyAdditionalCodes(newFormulary, dto);

            return newFormulary;
        }

        private void MergeFormularyAdditionalCodes(FormularyHeader existingFormularyFromDb, FormularyDTO sourceDTO)
        {
            if (!existingFormularyFromDb.FormularyAdditionalCode.IsCollectionValid() && !sourceDTO.FormularyAdditionalCodes.IsCollectionValid()) return;

            existingFormularyFromDb.FormularyAdditionalCode = existingFormularyFromDb.FormularyAdditionalCode.IsCollectionValid() ? existingFormularyFromDb.FormularyAdditionalCode : new List<FormularyAdditionalCode>();

            var additionalCodeDTOsInDb = _mapper.Map<List<FormularyAdditionalCodeDTO>>(existingFormularyFromDb.FormularyAdditionalCode);

            var customManualSourceItems = additionalCodeDTOsInDb.Where(rec => (string.Compare(rec.CodeType, "identification", true) == 0) && (rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty())).ToList();

            //Delete routes from custom source - manually entered
            if (customManualSourceItems.IsCollectionValid())
                customManualSourceItems.Each(rec => additionalCodeDTOsInDb.Remove(rec));

            if (sourceDTO.FormularyAdditionalCodes.IsCollectionValid())
            {
                var destinationItemsCodes = additionalCodeDTOsInDb.Select(rec => rec.AdditionalCode).ToList();

                //Get custom added additional codes
                var manualSourceLookupItems = sourceDTO.FormularyAdditionalCodes.Where(rec => (!destinationItemsCodes.Contains(rec.AdditionalCode)) && (string.Compare(rec.CodeType, "identification", true) == 0) && (rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty())).ToList();

                manualSourceLookupItems?.Each(rec =>
                {
                    rec.Source = TerminologyConstants.MANUAL_DATA_SRC;
                    additionalCodeDTOsInDb.Add(rec);
                });
            }

            existingFormularyFromDb.FormularyAdditionalCode = _mapper.Map<List<FormularyAdditionalCode>>(additionalCodeDTOsInDb);
        }

        private void MergeFormularyDetail(FormularyHeader existingFormularyFromDb, FormularyDTO dto)
        {
            if (dto.Detail == null || !existingFormularyFromDb.FormularyDetail.IsCollectionValid()) return;

            var detailFromDb = existingFormularyFromDb.FormularyDetail.First();

            var detailFromDbAsDTO = _mapper.Map<FormularyDetailDTO>(detailFromDb);

            var detailSourceDTO = dto.Detail;

            if (!(detailFromDb.Prescribable == false && string.Compare(detailFromDb.PrescribableSource, TerminologyConstants.DMD_DATA_SRC, true) == 0))
            {
                detailFromDbAsDTO.Prescribable = detailSourceDTO.Prescribable;
                detailFromDbAsDTO.PrescribableSource = detailSourceDTO.PrescribableSource;
            }

            detailFromDbAsDTO.WitnessingRequired = detailSourceDTO.WitnessingRequired;

            detailFromDb = _mapper.Map(detailFromDbAsDTO, detailFromDb);
        }
    }
}
