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
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.API.AppCode.Queries;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Generic;

namespace Interneuron.Terminology.API.AppCode.Commands.FormularyCreateHandlers
{
    public abstract class BaseCreateHandler
    {
        private FormularyDTO _request;
        private IMapper _mapper;
        private IFormularyQueries _formularyQueries;

        public BaseCreateHandler(IMapper mapper, FormularyDTO request)
        {
            _request = request;
            _mapper = mapper;
        }
        public abstract FormularyHeader CreateFormulary();

        


        protected virtual FormularyHeader CreateHeader()
        {
            var formularyHeader = new FormularyHeader();

            formularyHeader.FormularyId = Guid.NewGuid().ToString();
            formularyHeader.VersionId = 1;
            formularyHeader.FormularyVersionId = Guid.NewGuid().ToString();
            formularyHeader.IsLatest = true;
            formularyHeader.IsDuplicate = false;//Need to check

            formularyHeader.Code = Guid.NewGuid().ToString(); // request.Code?.Trim();
            formularyHeader.CodeSystem = _request.CodeSystem.IsNotEmpty() ? _request.CodeSystem : TerminologyConstants.Custom_IDENTIFICATION_CODE_SYSTEM;

            formularyHeader.Name = _request.Name?.Trim();
            formularyHeader.ParentCode = _request.ParentCode?.Trim();
            formularyHeader.ParentName = null;
            formularyHeader.ParentProductType = _request.ParentProductType?.Trim();
            formularyHeader.ProductType = _request.ProductType?.Trim();

            formularyHeader.RecSource = TerminologyConstants.MANUAL_DATA_SRC;// "Manual";
            formularyHeader.RecStatusCode = TerminologyConstants.RECORDSTATUS_DRAFT;//Draft
            formularyHeader.RecStatuschangeDate = DateTime.UtcNow;

            formularyHeader.VtmId = _request.VtmId.IsNotEmpty() ? _request.VtmId.Trim() : ((string.Compare(_request.ParentProductType, "vtm", true) == 0) ? _request.ParentCode : null);

            formularyHeader.VmpId = _request.VmpId.IsNotEmpty() ? _request.VmpId.Trim() : ((string.Compare(_request.ParentProductType, "vmp", true) == 0) ? _request.ParentCode : null);

            return formularyHeader;
        }

        protected virtual void PopulateFormularyDetail(FormularyHeader formularyHeader)
        {
            formularyHeader.FormularyDetail = new List<FormularyDetail>();

            var formularyDetail = _mapper.Map<FormularyDetail>(_request.Detail);

            formularyDetail.FormularyVersionId = formularyHeader.FormularyVersionId;

            formularyDetail.RnohFormularyStatuscd = formularyDetail.RnohFormularyStatuscd ?? TerminologyConstants.FORMULARYSTATUS_FORMULARY;

            formularyHeader.FormularyDetail.Add(formularyDetail);
        }

        protected virtual void PopulateFormularyAdditionalCodes(FormularyHeader formularyHeader)
        {
            if (_request.FormularyAdditionalCodes.IsCollectionValid())
            {
                formularyHeader.FormularyAdditionalCode = _mapper.Map<List<FormularyAdditionalCode>>(_request.FormularyAdditionalCodes);// new List<FormularyAdditionalCode>();

                formularyHeader.FormularyAdditionalCode.Each(detail =>
                {
                    detail.FormularyVersionId = formularyHeader.FormularyVersionId;
                    detail.Source = detail.Source ?? TerminologyConstants.MANUAL_DATA_SRC;//Should be manual since formulary is added manually
                });
            }
        }

        protected virtual void PopulateFormularyRouteDetails(FormularyHeader formularyHeader)
        {
            if (_request.FormularyRouteDetails.IsCollectionValid())
            {
                formularyHeader.FormularyRouteDetail = _mapper.Map<List<FormularyRouteDetail>>(_request.FormularyRouteDetails); // new List<FormularyRouteDetail>();

                formularyHeader.FormularyRouteDetail.Each(rt =>
                {
                    rt.FormularyVersionId = formularyHeader.FormularyVersionId;
                    rt.Source = rt.Source ?? TerminologyConstants.MANUAL_DATA_SRC;//Should be manual since formulary is added manually by RNOH
                });
            }
        }

        protected virtual void PopulateFormularyLocalRouteDetails(FormularyHeader formularyHeader)
        {
            if (_request.FormularyLocalRouteDetails.IsCollectionValid())
            {
                formularyHeader.FormularyLocalRouteDetail = _mapper.Map<List<FormularyLocalRouteDetail>>(_request.FormularyLocalRouteDetails); // new List<FormularyRouteDetail>();

                formularyHeader.FormularyLocalRouteDetail.Each(rt =>
                {
                    rt.FormularyVersionId = formularyHeader.FormularyVersionId;
                    rt.Source = rt.Source ?? TerminologyConstants.MANUAL_DATA_SRC;//Should be manual since formulary is added manually by RNOH
                });
            }
        }

        protected virtual void PopulateFormularyIngredients(FormularyHeader formularyHeader)
        {
            if (_request.FormularyIngredients.IsCollectionValid())
            {
                formularyHeader.FormularyIngredient = _mapper.Map<List<FormularyIngredient>>(_request.FormularyIngredients); // new List<FormularyRouteDetail>();

                formularyHeader.FormularyIngredient.Each(ing =>
                {
                    ing.FormularyVersionId = formularyHeader.FormularyVersionId;
                });
            }
        }

        protected virtual void PopulateFormularyExcipients(FormularyHeader formularyHeader)
        {
            if (_request.FormularyExcipients.IsCollectionValid())
            {
                formularyHeader.FormularyExcipient = _mapper.Map<List<FormularyExcipient>>(_request.FormularyExcipients); // new List<FormularyRouteDetail>();

                formularyHeader.FormularyExcipient.Each(excp =>
                {
                    excp.FormularyVersionId = formularyHeader.FormularyVersionId;
                });
            }
        }
    }
}
