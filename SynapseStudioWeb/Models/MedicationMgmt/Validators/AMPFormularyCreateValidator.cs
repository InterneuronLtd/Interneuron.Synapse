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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SynapseStudioWeb.Models.MedicationMgmt.Validators
{
    public class AMPFormularyCreateValidator : IFormularyCreateValidator
    {
        private List<ValidationResult> _results = new List<ValidationResult>();
        private FormularyCreateModel _model;

        public AMPFormularyCreateValidator(FormularyCreateModel model)
        {
            _model = model;
        }

        public List<ValidationResult> Validate()
        {
            if (_model == null) return _results;

            ValidateBasicInfo();

            ValidateClassificationCodes();

            ValidateRouteCodes();

            ValidateIndications();

            ValidateIngredients();

            ValidateExcipients();

            ValidateCustomWarnings();

            ValidateReminders();

            ValidateEndorsements();

            ValidateSideEffects();

            ValidateCautions();

            return _results;
        }

        private void ValidateBasicInfo()
        {
            if (_model.Name.IsEmpty())
                _results.Add(new ValidationResult("Please enter Name in Product Details.", new List<string> { nameof(_model.Name) }));
            if (_model.VirtualMedicinalProductName.IsEmpty())
                _results.Add(new ValidationResult("Please enter VMP Name in Product Details.", new List<string> { nameof(_model.VirtualMedicinalProductName) }));
            if (_model.VirtualTherapeuticMoietyName.IsEmpty())
                _results.Add(new ValidationResult("Please enter VTM Name in Product Details.", new List<string> { nameof(_model.VirtualTherapeuticMoietyName) }));
            if (_model.Code.IsEmpty())
                _results.Add(new ValidationResult("Please enter Code in Product Details.", new List<string> { nameof(_model.Code) }));
            if (_model.CodeSystem.IsEmpty())
                _results.Add(new ValidationResult("Please select Code System in Product Details.", new List<string> { nameof(_model.CodeSystem) }));
            if (_model.Status.IsEmpty())
                _results.Add(new ValidationResult("Please select Status in History.", new List<string> { nameof(_model.Status) }));
            if (_model.RnohFormularyStatuscd.IsEmpty())
                _results.Add(new ValidationResult("Please select Formulary Status in Product Details.", new List<string> { nameof(_model.RnohFormularyStatuscd) }));
            if (_model.Supplier == null || _model.Supplier.Id.IsEmpty())
                _results.Add(new ValidationResult("Please enter Supplier Name in Product Details.", new List<string> { nameof(_model.Supplier) }));
            if (_model.UnitDoseFormSize == null)
                _results.Add(new ValidationResult("Please enter Unit Dose Form Size in Posology.", new List<string> { nameof(_model.UnitDoseFormSize) }));
            if (_model.UnitDoseFormUnits == null || _model.UnitDoseFormUnits.Id.IsEmpty())
                _results.Add(new ValidationResult("Please enter Unit Dose Form Units in Posology.", new List<string> { nameof(_model.UnitDoseFormUnits) }));
            if (_model.UnitDoseUnitOfMeasure == null || _model.UnitDoseUnitOfMeasure.Id.IsEmpty())
                _results.Add(new ValidationResult("Please enter Unit Dose UOM in Posology.", new List<string> { nameof(_model.UnitDoseUnitOfMeasure) }));
            if (_model.ControlledDrugCategoriesEditableId.IsEmpty())
                _results.Add(new ValidationResult("Please select Controlled Drug Category in Guidance.", new List<string> { nameof(_model.ControlledDrugCategoriesEditableId) }));
        }

        private void ValidateClassificationCodes()
        {
            if (_model.FormularyClassificationCodes.IsCollectionValid())
            {
                var hasEmptyCodes = _model.FormularyClassificationCodes.Any(rec => rec.AdditionalCode.IsEmpty() || rec.AdditionalCodeSystem.IsEmpty());

                if (hasEmptyCodes)
                {
                    _results.Add(new ValidationResult("Classification Code or Classification Code System under Product Details cannot be empty.", new List<string> { nameof(_model.FormularyClassificationCodes) }));
                    return;
                }

                var addlnCodeSys = new Dictionary<string, int>();
                var hasDups = false;
                var addlnCodes = new Dictionary<string, int>();
                var hasDupCodes = false;

                _model.FormularyClassificationCodes.Each(rec =>
                {
                    if (addlnCodeSys.ContainsKey(rec.AdditionalCodeSystem))
                    {
                        hasDups = true;
                        addlnCodeSys[rec.AdditionalCodeSystem] = addlnCodeSys[rec.AdditionalCodeSystem] + 1;
                    }
                    else
                    {
                        addlnCodeSys[rec.AdditionalCodeSystem] = 1;
                    }

                    if (addlnCodes.ContainsKey(rec.AdditionalCode))
                    {
                        hasDupCodes = true;
                        addlnCodes[rec.AdditionalCode] = addlnCodes[rec.AdditionalCode] + 1;
                    }
                    else
                    {
                        addlnCodes[rec.AdditionalCode] = 1;
                    }
                });
                if (hasDups)
                {
                    _results.Add(new ValidationResult("Classification Code System under Product Details should be unique.", new List<string> { nameof(_model.FormularyClassificationCodes) }));
                }
                if (hasDupCodes)
                {
                    _results.Add(new ValidationResult("Classification Code under Product Details should be unique.", new List<string> { nameof(_model.FormularyClassificationCodes) }));
                }
            }
        }

        private void ValidateIngredients()
        {
            if (!_model.Ingredients.IsCollectionValid())
            {
                _results.Add(new ValidationResult("Ingredients under Posology cannot be empty.", new List<string> { nameof(_model.Ingredients) }));
            }

            if (_model.Ingredients.IsCollectionValid())
            {
                var hasEmptyCodes = _model.Ingredients.Any(rec => rec.Ingredient == null || rec.Ingredient.Id.IsEmpty());

                if (hasEmptyCodes)
                {
                    _results.Add(new ValidationResult("Ingredient Code under Posology cannot be empty.", new List<string> { nameof(_model.Ingredients) }));
                }

                hasEmptyCodes = _model.Ingredients.Any(rec => rec.StrengthValueNumeratorUnit == null || rec.StrengthValueNumeratorUnit.Id.IsEmpty());

                if (hasEmptyCodes)
                {
                    _results.Add(new ValidationResult("Ingredient Strength Value Numerator Unit under Posology cannot be empty.", new List<string> { nameof(_model.Ingredients) }));
                }

                hasEmptyCodes = _model.Ingredients.Any(rec => rec.StrengthValNumerator.IsEmpty());

                if (hasEmptyCodes)
                {
                    _results.Add(new ValidationResult("Ingredient Strength Value Numerator under Posology cannot be empty.", new List<string> { nameof(_model.Ingredients) }));
                }

                hasEmptyCodes = _model.Ingredients.Any(rec => (rec.StrengthValDenominator.IsEmpty() && (rec.StrengthValueDenominatorUnit != null && rec.StrengthValueDenominatorUnit.Id.IsNotEmpty())) || (rec.StrengthValDenominator.IsNotEmpty() && (rec.StrengthValueDenominatorUnit == null || rec.StrengthValueDenominatorUnit.Id.IsEmpty())));

                if (hasEmptyCodes)
                {
                    _results.Add(new ValidationResult("Enter both Ingredient Strength Value Denominator and Denominator Unit under Posology.", new List<string> { nameof(_model.Ingredients) }));
                }

                var ingCodes = new Dictionary<string, int>();
                var hasIngDups = false;

                _model.Ingredients.Each(rec =>
                {
                    if (rec.Ingredient != null && rec.Ingredient.Id.IsNotEmpty())
                    {
                        if (ingCodes.ContainsKey(rec.Ingredient.Id))
                        {
                            hasIngDups = true;
                            ingCodes[rec.Ingredient.Id] = ingCodes[rec.Ingredient.Id] + 1;
                        }
                        else
                        {
                            ingCodes[rec.Ingredient.Id] = 1;
                        }
                    }
                });
                if (hasIngDups)
                    _results.Add(new ValidationResult("Ingredient Name under Posology should be unique.", new List<string> { nameof(_model.Ingredients) }));

            }
        }

        private void ValidateExcipients()
        {   
            if (_model.Excipients.IsCollectionValid())
            {
                var hasEmptyCodes = _model.Excipients.Any(rec => rec.Ingredient == null || rec.Ingredient.Id.IsEmpty());

                if (hasEmptyCodes)
                {
                    _results.Add(new ValidationResult("Excipient Code under Posology cannot be empty.", new List<string> { nameof(_model.Excipients) }));
                }

                hasEmptyCodes = _model.Excipients.Any(rec => rec.StrengthUnit == null || rec.StrengthUnit.Id.IsEmpty());

                if (hasEmptyCodes)
                {
                    _results.Add(new ValidationResult("Excipient Strength Value Unit under Posology cannot be empty.", new List<string> { nameof(_model.Excipients) }));
                }

                hasEmptyCodes = _model.Excipients.Any(rec => rec.Strength.IsEmpty());

                if (hasEmptyCodes)
                {
                    _results.Add(new ValidationResult("Ingredient Strength Value under Posology cannot be empty.", new List<string> { nameof(_model.Excipients) }));
                }

                var ingCodes = new Dictionary<string, int>();
                var hasIngDups = false;

                _model.Excipients.Each(rec =>
                {
                    if (rec.Ingredient != null && rec.Ingredient.Id.IsNotEmpty())
                    {
                        if (ingCodes.ContainsKey(rec.Ingredient.Id))
                        {
                            hasIngDups = true;
                            ingCodes[rec.Ingredient.Id] = ingCodes[rec.Ingredient.Id] + 1;
                        }
                        else
                        {
                            ingCodes[rec.Ingredient.Id] = 1;
                        }
                    }
                });
                if (hasIngDups)
                    _results.Add(new ValidationResult("Excipient Name under Posology should be unique.", new List<string> { nameof(_model.Excipients) }));

            }
        }


        private void ValidateRouteCodes()
        {
            if (_model.Route.IsCollectionValid())
            {
                var hasEmptyCodes = _model.Route.Any(rec => rec.Id.IsEmpty());

                if (hasEmptyCodes)
                {
                    _results.Add(new ValidationResult("Route Code under Posology cannot be empty.", new List<string> { nameof(_model.Route) }));
                }

                var routeCodes = new Dictionary<string, int>();
                var hasRouteDups = false;

                _model.Route.Each(rec =>
                {
                    if (rec.Id.IsNotEmpty())
                    {
                        if (routeCodes.ContainsKey(rec.Id))
                        {
                            hasRouteDups = true;
                            routeCodes[rec.Id] = routeCodes[rec.Id] + 1;
                        }
                        else
                        {
                            routeCodes[rec.Id] = 1;
                        }
                    }
                });
                if (hasRouteDups)
                    _results.Add(new ValidationResult("Route Code under Posology should be unique.", new List<string> { nameof(_model.Route) }));
            }

            if (_model.UnlicensedRoute.IsCollectionValid())
            {
                var hasEmptyCodes = _model.UnlicensedRoute.Any(rec => rec.Id.IsEmpty());

                if (hasEmptyCodes)
                {
                    _results.Add(new ValidationResult("Unlicensed Route Code under Posology cannot be empty.", new List<string> { nameof(_model.UnlicensedRoute) }));
                    return;
                }

                var routeCodes = new Dictionary<string, int>();
                var hasRouteDups = false;

                _model.UnlicensedRoute.Each(rec =>
                {
                    if (rec.Id.IsNotEmpty())
                    {
                        if (routeCodes.ContainsKey(rec.Id))
                        {
                            hasRouteDups = true;
                            routeCodes[rec.Id] = routeCodes[rec.Id] + 1;
                        }
                        else
                        {
                            routeCodes[rec.Id] = 1;
                        }
                    }
                });
                if (hasRouteDups)
                    _results.Add(new ValidationResult("Unlicensed Route Code under Posology should be unique.", new List<string> { nameof(_model.UnlicensedRoute) }));
            }
        }

        private void ValidateIndications()
        {
            if (_model.LicensedUse.IsCollectionValid())
            {
                var indications = new Dictionary<string, int>();
                var hasIndcDups = false;

                _model.LicensedUse.Each(rec =>
                {
                    if (indications.ContainsKey(rec.Id))
                    {
                        hasIndcDups = true;
                        indications[rec.Id] = indications[rec.Id] + 1;
                    }
                    else
                    {
                        indications[rec.Id] = 1;
                    }
                });
                if (hasIndcDups)
                    _results.Add(new ValidationResult("Licensed Indications under Product Details should be unique.", new List<string> { nameof(_model.LicensedUse) }));
            }

            if (_model.UnlicensedUse.IsCollectionValid())
            {
                var indications = new Dictionary<string, int>();
                var hasIndcDups = false;

                _model.UnlicensedRoute.Each(rec =>
                {
                    if (indications.ContainsKey(rec.Id))
                    {
                        hasIndcDups = true;
                        indications[rec.Id] = indications[rec.Id] + 1;
                    }
                    else
                    {
                        indications[rec.Id] = 1;
                    }
                });
                if (hasIndcDups)
                    _results.Add(new ValidationResult("Unlicensed Indications under Product Details should be unique.", new List<string> { nameof(_model.UnlicensedUse) }));
            }
        }

        private void ValidateCustomWarnings()
        {
            if (_model.CustomWarnings.IsCollectionValid())
            {
                var hasEmpty = _model.CustomWarnings.Any(rec => rec.Warning.IsEmpty());

                if (hasEmpty)
                {
                    _results.Add(new ValidationResult("Custom Warning under Guidance cannot be empty.", new List<string> { nameof(_model.CustomWarnings) }));
                }
            }
        }

        private void ValidateReminders()
        {
            if (_model.Reminders.IsCollectionValid())
            {
                var hasEmptyReminder = _model.Reminders.Any(rec => rec.Reminder.IsEmpty());

                var hasEmptyDuration = _model.Reminders.Any(rec => rec.Duration.ToString().IsEmpty());

                var hasZeroDurationValue = _model.Reminders.Any(rec => rec.Duration.HasValue && rec.Duration.Value == 0);

                if (hasEmptyReminder)
                {
                    _results.Add(new ValidationResult("Reminder under Guidance cannot be empty.", new List<string> { nameof(_model.Reminders) }));
                }

                if (hasEmptyDuration)
                {
                    _results.Add(new ValidationResult("Duration under Guidance cannot be empty.", new List<string> { nameof(_model.Reminders) }));
                }

                if (!hasEmptyDuration && hasZeroDurationValue)
                {
                    _results.Add(new ValidationResult("Duration value under Guidance must be greater than 0.", new List<string> { nameof(_model.Reminders) }));
                }
            }
        }

        private void ValidateEndorsements()
        {
            if (_model.Endorsements.IsCollectionValid())
            {
                var hasEmpty = _model.Endorsements.Any(rec => rec.IsEmpty());

                if (hasEmpty)
                    _results.Add(new ValidationResult("Endorsement under Guidance cannot be empty.", new List<string> { nameof(_model.Endorsements) }));
            }
        }

        private void ValidateSideEffects()
        {
            if (_model.SideEffects.IsCollectionValid())
            {
                var sideEffs = new Dictionary<string, int>();
                var hasIndcDups = false;

                _model.SideEffects.Each(rec =>
                {
                    if (sideEffs.ContainsKey(rec.Id))
                    {
                        hasIndcDups = true;
                        sideEffs[rec.Id] = sideEffs[rec.Id] + 1;
                    }
                    else
                    {
                        sideEffs[rec.Id] = 1;
                    }
                });
                if (hasIndcDups)
                    _results.Add(new ValidationResult("Side Effects under Guidance should be unique.", new List<string> { nameof(_model.SideEffects) }));
            }
        }

        private void ValidateCautions()
        {
            if (_model.Cautions.IsCollectionValid())
            {
                var cautns = new Dictionary<string, int>();
                var hasIndcDups = false;

                _model.Cautions.Each(rec =>
                {
                    if (cautns.ContainsKey(rec.Id))
                    {
                        hasIndcDups = true;
                        cautns[rec.Id] = cautns[rec.Id] + 1;
                    }
                    else
                    {
                        cautns[rec.Id] = 1;
                    }
                });
                if (hasIndcDups)
                    _results.Add(new ValidationResult("Cautions under Guidance should be unique.", new List<string> { nameof(_model.Cautions) }));
            }
        }
    }
}

