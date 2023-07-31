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
﻿using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary.Requests;
using System.Collections.Generic;

namespace Interneuron.Terminology.API.AppCode.Validators
{
    public class CheckIfProductExistsRequestValidator : ITerminologyValidator
    {
        private List<FormularyIngredientDTO> _ingredients;
        private string _unitDoseFormSize;
        private string _formulationName;
        private string _supplierName;
        private string _productType;
        TerminologyValidationResult _validationResult = new TerminologyValidationResult();
        private CheckIfProductExistsRequest _request;

        public CheckIfProductExistsRequestValidator(CheckIfProductExistsRequest request)
        {
            _request = request;
            _validationResult.IsValid = true;
            _validationResult.ValidationErrors = new List<string>();
        }

        public TerminologyValidationResult Validate()
        {
            if (_request == null)
            {
                _validationResult.IsValid = false;
                _validationResult.ValidationErrors.Add("Request cannot be empty");
                return _validationResult;
            }

            _ingredients = _request.Ingredients;
            _unitDoseFormSize = _request.UnitDoseFormSize;
            _formulationName = _request.FormulationName;
            _supplierName = _request.SupplierName;
            _productType = _request.ProductType;

            if (!_ingredients.IsCollectionValid())
            {
                _validationResult.IsValid = false;
                _validationResult.ValidationErrors.Add("Ingredients cannot be empty");
            }
            else
            {
                _ingredients.Each(rec =>
                {
                    if (rec.IngredientName.IsEmpty())
                    {
                        _validationResult.IsValid = false;
                        _validationResult.ValidationErrors.Add("Ingredient Name cannot be empty");
                    }
                    if (rec.StrengthValueNumerator.IsEmpty())
                    {
                        _validationResult.IsValid = false;
                        _validationResult.ValidationErrors.Add("Numerator cannot be empty");
                    }
                    if (rec.StrengthValueNumeratorUnitDesc.IsEmpty())
                    {
                        _validationResult.IsValid = false;
                        _validationResult.ValidationErrors.Add("Numerator Unit cannot be empty");
                    }
                });
            }
            if (string.Compare(_productType, "amp", true) == 0 || string.Compare(_productType, "all", true) == 0)
                ValidateForAMP();
            if (string.Compare(_productType, "vmp", true) == 0 || string.Compare(_productType, "all", true) == 0)
                ValidateForVMP();

            return _validationResult;
        }

        private void ValidateForAMP()
        {
            if (_unitDoseFormSize.IsEmpty())
            {
                _validationResult.IsValid = false;
                _validationResult.ValidationErrors.Add("Unit Dose cannot be empty");
            }

            if (_supplierName.IsEmpty())
            {
                _validationResult.IsValid = false;
                _validationResult.ValidationErrors.Add("Supplier Name cannot be empty");
            }

            if (_formulationName.IsEmpty())
            {
                _validationResult.IsValid = false;
                _validationResult.ValidationErrors.Add("Formulation Name cannot be empty");
            }
        }

        private void ValidateForVMP()
        {
            if (_unitDoseFormSize.IsEmpty())
            {
                _validationResult.IsValid = false;
                _validationResult.ValidationErrors.Add("Unit Dose cannot be empty");
            }

            if (_formulationName.IsEmpty())
            {
                _validationResult.IsValid = false;
                _validationResult.ValidationErrors.Add("Formulation Name cannot be empty");
            }
        }
    }
}
