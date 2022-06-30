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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Commands.EditMergeHandlers
{
    public abstract class FormularyMergeHandler
    {
        private IMapper _mapper;

        public FormularyMergeHandler(IMapper mapper)
        {
            _mapper = mapper;
        }
        public abstract FormularyHeader Merge(FormularyHeader existingFormulary, FormularyDTO dto);

        protected virtual FormularyHeader CloneFormulary(FormularyHeader existingFormulary)
        {
            var newFormulary = _mapper.Map<FormularyHeader>(existingFormulary);
            var newFormularyVersionId = Guid.NewGuid().ToString();

            if (existingFormulary.FormularyAdditionalCode.IsCollectionValid())
            {
                newFormulary.FormularyAdditionalCode = _mapper.Map<List<FormularyAdditionalCode>>(existingFormulary.FormularyAdditionalCode);
                newFormulary.FormularyAdditionalCode?.Each(rec => { rec.FormularyVersionId = newFormularyVersionId; rec.RowId = null; });
            }
            if (existingFormulary.FormularyDetail.IsCollectionValid())
            {
                newFormulary.FormularyDetail = _mapper.Map<List<FormularyDetail>>(existingFormulary.FormularyDetail);
                newFormulary.FormularyDetail?.Each(rec => { rec.FormularyVersionId = newFormularyVersionId; rec.RowId = null; });
            }
            if (existingFormulary.FormularyRouteDetail.IsCollectionValid())
            {
                newFormulary.FormularyRouteDetail = _mapper.Map<List<FormularyRouteDetail>>(existingFormulary.FormularyRouteDetail);
                newFormulary.FormularyRouteDetail?.Each(rec => { rec.FormularyVersionId = newFormularyVersionId; rec.RowId = null; });
            }
            if (existingFormulary.FormularyIngredient.IsCollectionValid())
            {
                newFormulary.FormularyIngredient = _mapper.Map<List<FormularyIngredient>>(existingFormulary.FormularyIngredient);
                newFormulary.FormularyIngredient?.Each(rec => { rec.FormularyVersionId = newFormularyVersionId; rec.RowId = null; });
            }
            if (existingFormulary.FormularyExcipient.IsCollectionValid())
            {
                newFormulary.FormularyExcipient = _mapper.Map<List<FormularyExcipient>>(existingFormulary.FormularyExcipient);
                newFormulary.FormularyExcipient?.Each(rec => { rec.FormularyVersionId = newFormularyVersionId; rec.RowId = null; });
            }

            newFormulary.FormularyId = existingFormulary.FormularyId;
            newFormulary.VersionId = existingFormulary.VersionId + 1;
            newFormulary.FormularyVersionId = newFormularyVersionId;
            newFormulary.IsLatest = true;
            newFormulary.IsDuplicate = existingFormulary.IsDuplicate;// false;//Need to check
            newFormulary.DuplicateOfFormularyId = existingFormulary.DuplicateOfFormularyId;

            return newFormulary;
        }

        protected List<FormularyLookupItemDTO> HandleFormularyLookupItems(List<FormularyLookupItemDTO> sourceLookupItems, List<FormularyLookupItemDTO> destinationLookupItems)
        {
            destinationLookupItems = destinationLookupItems.IsCollectionValid() ? destinationLookupItems : new List<FormularyLookupItemDTO>();
            var customManualSourceItems = destinationLookupItems.Where(rec => rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty()).ToList();

            //Delete indications from custom source - manually entered
            if (customManualSourceItems.IsCollectionValid())
                customManualSourceItems.Each(rec => destinationLookupItems.Remove(rec));

            if (sourceLookupItems.IsCollectionValid())
            {
                var destinationLookupItemsCodes = destinationLookupItems.Select(rec => rec.Cd).ToList();

                //Get custom added indications
                var manualSourceLookupItems = sourceLookupItems.Where(rec => !destinationLookupItemsCodes.Contains(rec.Cd) && (rec.Source == TerminologyConstants.MANUAL_DATA_SRC || rec.Source.IsEmpty())).ToList();

                manualSourceLookupItems?.Each(rec =>
                {
                    rec.Source = TerminologyConstants.MANUAL_DATA_SRC;
                    destinationLookupItems.Add(rec);
                });
            }

            return destinationLookupItems;
        }
    }
}