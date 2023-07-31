//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

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
using System.Collections.Generic;

namespace Interneuron.Terminology.API.AppCode.Validators
{
    public class CreateFormularyRequestValidator : ITerminologyValidator
    {
        private const string NO_DATA_TO_SAVE_MSG = "No Data to save";
        private const string INGREDIENT_MISSING_MSG = "One of the ingredient detail is missing in the data.";
        private const string EXCIPIENT_INVALID_MSG = "One of the excipient detail is missing in the data.";

        private const string SUPPLIER_MISSING_MSG = "One of the AMP related detail is missing in the data.";
        private const string ROUTE_MISSING_MSG = "One of the route detail is missing in the data.";
        private const string DETAIL_MISSING_MSG = "Key details are missing in the data.";

        private CreateEditFormularyRequest _request;

        public CreateFormularyRequestValidator(CreateEditFormularyRequest request)
        {
            _request = request;
        }

        public TerminologyValidationResult Validate()
        {
            var validationResult = CheckRequestHasData();
            if (!validationResult.IsValid)
                return validationResult;

            //validate fields
            validationResult = ValidateFormularyAttributes();

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

        private TerminologyValidationResult ValidateFormularyAttributes()
        {
            var validationResult = new TerminologyValidationResult();
            validationResult.IsValid = true;
            validationResult.ValidationErrors = new List<string>();

            foreach (var req in _request.RequestsData)
            {
                if (req == null)
                {
                    validationResult.IsValid = false;
                    validationResult.ValidationErrors.Add("'Empty' request object. Data cannot be saved.");
                }
                else
                {
                    var areHeaderFieldsEmpty = req.ProductType.IsEmpty() || req.Name.IsEmpty();

                    if (areHeaderFieldsEmpty)
                    {
                        validationResult.ValidationErrors.Add("Key details are missing in the data.");
                        validationResult.IsValid = false;
                    }

                    var areDetailFieldsEmpty = req.Detail.IsNull();

                    if (areDetailFieldsEmpty)
                    {
                        validationResult.ValidationErrors.Add(DETAIL_MISSING_MSG);
                        validationResult.IsValid = false;
                    }
                    else
                    {
                        if (req.Detail.SupplierCd.IsEmpty())
                        {
                            validationResult.ValidationErrors.Add(SUPPLIER_MISSING_MSG);
                            validationResult.IsValid = false;
                        }
                        if (req.Detail.SupplierName.IsEmpty())
                        {
                            validationResult.ValidationErrors.Add(SUPPLIER_MISSING_MSG);
                            validationResult.IsValid = false;
                        }
                        if (!req.Detail.UnitDoseFormSize.HasValue)
                        {
                            validationResult.IsValid = false;
                            validationResult.ValidationErrors.Add("Unit Dose cannot be empty");
                        }

                        if (req.Detail.FormCd.IsEmpty())
                        {
                            validationResult.IsValid = false;
                            validationResult.ValidationErrors.Add("Formulation Name cannot be empty");
                        }
                    }

                    if (!CheckRouterDetails(req))
                    {
                        validationResult.ValidationErrors.Add(ROUTE_MISSING_MSG);

                        validationResult.IsValid = false;
                    }

                    if (!CheckLocalRouteDetails(req))
                    {
                        validationResult.ValidationErrors.Add(ROUTE_MISSING_MSG);

                        validationResult.IsValid = false;
                    }

                    if (!CheckIngredientDetails(req))
                    {
                        validationResult.ValidationErrors.Add(INGREDIENT_MISSING_MSG);

                        validationResult.IsValid = false;
                    }

                    if (!CheckExcipientDetails(req))
                    {
                        validationResult.ValidationErrors.Add(EXCIPIENT_INVALID_MSG);

                        validationResult.IsValid = false;
                    }
                }
                return validationResult;
            }

            return null;
        }

        private bool CheckIngredientDetails(FormularyDTO req)
        {
            if (!req.FormularyIngredients.IsCollectionValid()) return false;

            foreach (var ing in req.FormularyIngredients)
            {
                var ingEmpty = ing.IngredientCd.IsEmpty() || ing.StrengthValueNumeratorUnitCd.IsEmpty() || ing.StrengthValueNumerator.IsEmpty();

                if (ingEmpty) return false;

                var isDenIncorrect = (ing.StrengthValueDenominator.IsEmpty() && ing.StrengthValueDenominatorUnitCd.IsNotEmpty()) || (ing.StrengthValueDenominator.IsNotEmpty() && ing.StrengthValueDenominatorUnitCd.IsEmpty());

                if (isDenIncorrect) return false;

            }
            return true;
        }

        private bool CheckExcipientDetails(FormularyDTO req)
        {
            if (!req.FormularyExcipients.IsCollectionValid()) return true;

            foreach (var exp in req.FormularyExcipients)
            {
                var ingEmpty = exp.IngredientCd.IsEmpty() || exp.Strength.IsEmpty() || exp.StrengthUnitCd.IsEmpty();

                if (ingEmpty) return false;
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
