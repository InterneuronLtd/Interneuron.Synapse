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


﻿using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary.Requests;
using System;
using System.Collections.Generic;

namespace Interneuron.Terminology.API.AppCode.Validators
{
    public class EditFormularyRequestValidator : ITerminologyValidator
    {
        private const string NO_DATA_TO_SAVE_MSG = "No Data to save";
        private const string ROUTE_MISSING_MSG = "One of the route detail is missing in the data. Id {0}";
        private const string ADDNL_CODE_MISSING_MSG = "One of the additional code detail is missing in the data. Id {0}";
        private const string DETAIL_MISSING_MSG = "Key details are missing in the data. Id {0}";

        private CreateEditFormularyRequest _request;

        public EditFormularyRequestValidator(CreateEditFormularyRequest request)
        {
            _request = request;
        }

        public TerminologyValidationResult Validate()
        {
            var validationResult = CheckRequestHasData();
            if (!validationResult.IsValid)
                return validationResult;

            //Check for mandatory fields
            validationResult = CheckMandatory();

            return validationResult;
        }

        private TerminologyValidationResult CheckRequestHasData()
        {
            var validationResult = new TerminologyValidationResult();

            validationResult.IsValid = true;

            if (_request == null || !_request.RequestsData.IsCollectionValid())
            {
                validationResult.IsValid = false;
                validationResult.ValidationErrors = new List<string> { NO_DATA_TO_SAVE_MSG };
            }

            return validationResult;
        }

        private TerminologyValidationResult CheckMandatory()
        {
            var validationResult = new TerminologyValidationResult
            {
                IsValid = true,
                ValidationErrors = new List<string>()
            };

            foreach (var req in _request.RequestsData)
            {
                if (req == null)
                {
                    validationResult.IsValid = false;
                    validationResult.ValidationErrors.Add("'Empty' request object. Data cannot be saved.");

                    return validationResult;
                }

                var areHeaderFieldsEmpty = req.FormularyVersionId.IsEmpty();// || req.Code.IsEmpty();

                if (areHeaderFieldsEmpty)
                {
                    validationResult.ValidationErrors.Add("Key details are missing in the data.");

                    validationResult.IsValid = false;
                    return validationResult;
                }

                var areDetailFieldsEmpty = req.Detail.IsNull();

                if (areDetailFieldsEmpty)
                {
                    validationResult.ValidationErrors.Add(DETAIL_MISSING_MSG.ToFormat(req.FormularyVersionId));

                    validationResult.IsValid = false;
                    return validationResult;
                }

                if (!CheckAdditionalCodeDetails(req))
                {
                    validationResult.ValidationErrors.Add(ADDNL_CODE_MISSING_MSG.ToFormat(req.FormularyVersionId));

                    validationResult.IsValid = false;
                    return validationResult;
                }

                if (string.Compare(req.ProductType.Trim(), "amp", true) == 0)
                {
                    if (!CheckRouterDetails(req))
                    {
                        validationResult.ValidationErrors.Add(ROUTE_MISSING_MSG.ToFormat(req.FormularyVersionId));

                        validationResult.IsValid = false;
                        return validationResult;
                    }

                    if (!CheckLocalRouteDetails(req))
                    {
                        validationResult.ValidationErrors.Add(ROUTE_MISSING_MSG.ToFormat(req.FormularyVersionId));

                        validationResult.IsValid = false;
                        return validationResult;
                    }
                }

                return validationResult;
            }

            return null;
        }

        private bool CheckAdditionalCodeDetails(FormularyDTO req)
        {
            if (!req.FormularyAdditionalCodes.IsCollectionValid()) return true;

            foreach (var addl in req.FormularyAdditionalCodes)
            {
                if (addl.AdditionalCode.IsEmpty() || addl.AdditionalCodeSystem.IsEmpty()) return false;
            }

            return true;
        }
        

        private bool CheckRouterDetails(FormularyDTO req)
        {
            if (!req.FormularyRouteDetails.IsCollectionValid()) return true;

            foreach (var route in req.FormularyRouteDetails)
            {
                if (route.RouteCd.IsEmpty() || route.RouteFieldTypeCd.IsEmpty()) return false;
            }

            return true;
        }

        private bool CheckLocalRouteDetails(FormularyDTO req)
        {
            if (!req.FormularyLocalRouteDetails.IsCollectionValid()) return true;

            foreach (var route in req.FormularyLocalRouteDetails)
            {
                if (route.RouteCd.IsEmpty() || route.RouteFieldTypeCd.IsEmpty()) return false;
            }

            return true;
        }
    }
}
