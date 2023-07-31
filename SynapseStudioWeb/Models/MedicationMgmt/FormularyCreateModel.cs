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


﻿using SynapseStudioWeb.AppCode.Constants;
using SynapseStudioWeb.Models.MedicationMgmt.Validators;
using SynapseStudioWeb.Models.MedicinalMgmt;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SynapseStudioWeb.Models.MedicationMgmt
{
    public class FormularyCreateModel : IValidatableObject
    {
        public string ControlIdentifier { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Please select Product Type in Product Details")]
        public string ProductType { get; set; } = "AMP";

        public string Code { get; set; }

        public string CodeSystem { get; set; } = TerminologyConstants.PRIMARY_IDENTIFICATION_CODE_TYPE;

        public List<FormularyAdditionalCodeModel> FormularyClassificationCodes { get; set; }

        public List<FormularyAdditionalCodeModel> FormularyIdentificationCodes { get; set; }

        public string Status { get; set; } = TerminologyConstants.DRAFT_STATUS_CD;
        public string OriginalStatus { get; set; }
        public string Id { get; set; }

        [Required(ErrorMessage = "Please select Prescribable in Flags / Classification")]
        public bool Prescribable { get; set; }
        public bool OutpatientMedication { get; set; }
        public string RecStatuschangeMsg { get; set; }
        public string RecSource { get; set; } = TerminologyConstants.MANUAL_RECORD_SOURCE;
        public bool HighAlertMedication { get; set; }
        public string HighAlertMedicationSource { get; set; }

        public bool IgnoreDuplicateWarnings { get; set; }
        public List<CodeNameSelectorModel> Route { get; set; }
        public List<CodeNameSelectorModel> UnlicensedRoute { get; set; }

        public List<CodeNameSelectorModel> LocalLicensedRoute { get; set; }
        public List<CodeNameSelectorModel> LocalUnlicensedRoute { get; set; }
        public bool CriticalDrug { get; set; }
        public bool IVToOral { get; set; }
        public bool WitnessingRequired { get; set; }
        public List<CodeNameSelectorModel> Cautions { get; set; }
        public List<FormularyCustomWarningModel> CustomWarnings { get; set; }
        public List<FormularyReminderModel> Reminders { get; set; }
        public List<CodeNameSelectorModel> ContraIndications { get; set; }
        public List<CodeNameSelectorModel> SideEffects { get; set; }
        public List<CodeNameSelectorModel> SafetyMessages { get; set; }
        public List<string> Endorsements { get; set; }
        public List<CodeNameSelectorModel> LicensedUse { get; set; }
        public List<CodeNameSelectorModel> UnlicensedUse { get; set; }

        public List<CodeNameSelectorModel> LocalLicensedUse { get; set; }
        public List<CodeNameSelectorModel> LocalUnlicensedUse { get; set; }

        public bool BlackTriangle { get; set; }
        public string BlackTriangleSource { get; set; } = TerminologyConstants.MANUAL_DATA_SOURCE;
        public bool IsRecordStatusReadOnly { get; set; }
        public bool IsSaveSuccessful { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public string ParentProductType { get; set; }
        public bool? IsDuplicate { get; set; }
        public string DuplicateOfFormularyId { get; set; }
        public Dictionary<string, bool> ControlDisplayMetaDetails { get; set; }
        public bool IsBloodProduct { get; set; }
        public bool IsDiluent { get; set; }


        public string VirtualTherapeuticMoiety { get; set; }
        public string VirtualTherapeuticMoietyName { get; set; }

        public CodeNameSelectorModel FormCd { get; set; }
        public decimal? UnitDoseFormSize { get; set; }
        public CodeNameSelectorModel UnitDoseFormUnits { get; set; }
        public CodeNameSelectorModel UnitDoseUnitOfMeasure { get; set; }

        public List<FormularyIngredientModel> Ingredients { get; set; }

        public bool SugarFree { get; set; }
        public bool GlutenFree { get; set; }
        public bool PreservativeFree { get; set; }

        public bool CFCFree { get; set; }

        public string PrescribingStatusCd { get; set; }
        public string PrescribingStatusDesc { get; set; }
        public string VirtualMedicinalProduct { get; set; }
        public string VirtualMedicinalProductName { get; set; }


        //=================
        public string BasisOfPreferredNameCd { get; set; }
        public string BasisOfPreferredNameDesc { get; set; }
        public string RoundingFactorCd { get; set; }
        public string RoundingFactorDesc { get; set; }
        public string DoseFormCd { get; set; }
        public string DoseFormDesc { get; set; }

        public CodeNameSelectorModel Supplier { get; set; }
        public CodeNameSelectorModel TradeFamily { get; set; }
        public List<CodeNameSelectorModel> MedusaPreparationInstructions { get; set; }
        public string MedusaPreparationInstructionsEditable { get; set; }

        public bool ClinicalTrialMedication { get; set; }
        public List<CodeNameSelectorModel> ControlledDrugCategories { get; set; }
        public string ControlledDrugCategoriesEditableId { get; set; }
        public bool EmaAdditionalMonitoring { get; set; }
        public bool ExpensiveMedication { get; set; }
        public bool NotForPrn { get; set; }
        public string PrescribableSource { get; set; }
        public string RestrictionsOnAvailabilityCd { get; set; }
        public string RestrictionsOnAvailabilityDesc { get; set; }
        public List<CodeNameSelectorModel> TitrationTypes { get; set; }
        public string TitrationTypesEditableId { get; set; }

        public bool UnlicensedMedication { get; set; }
        public bool ParallelImport { get; set; }

        public string RnohFormularyStatuscd { get; set; }
        public string RnohFormularyStatusDesc { get; set; }
        public bool IsModifiedRelease { get; set; }
        public bool IsGastroResistant { get; set; }
        public string CurrentLicensingAuthorityCd { get; set; }
        public string CurrentLicensingAuthorityDesc { get; set; }

        public List<FormularyExcipientModel> Excipients { get; set; }

        public bool IsCustomControlledDrug { get; set; }

        public bool IsPrescriptionPrintingRequired { get; set; }

        public List<CodeNameSelectorModel> Diluents { get; set; }

        public bool IsIndicationMandatory { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = GetValidatorByType();
            var results = validator.Validate();
            return results;
        }

        private IFormularyCreateValidator GetValidatorByType()
        {
            return new AMPFormularyCreateValidator(this);
        }
    }
}






