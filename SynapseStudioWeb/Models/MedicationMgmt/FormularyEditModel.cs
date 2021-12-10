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
using SynapseStudioWeb.AppCode.Constants;
using SynapseStudioWeb.Models.MedicationMgmt.Validators;
using SynapseStudioWeb.Models.MedicinalMgmt;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SynapseStudioWeb.Models.MedicationMgmt
{
    [Serializable]
    public class FormularyEditModel : IValidatableObject
    {
        public List<string> FormularyVersionIds { get; set; }
        public bool IsImported { get; set; }

        public bool IsBulkEdit { get; set; } = false;

        public string ControlIdentifier { get; set; }

        public string Name { get; set; }

        [Required(ErrorMessage = "Please select Product Type in Product Details")]
        public string ProductType { get; set; }

        public string Code { get; set; }

        public string CodeSystem { get; set; } = TerminologyConstants.PRIMARY_IDENTIFICATION_CODE_TYPE;

        public List<FormularyAdditionalCodeModel> FormularyClassificationCodes { get; set; }

        public List<FormularyAdditionalCodeModel> FormularyIdentificationCodes { get; set; }

        public string Status { get; set; }
        public string OriginalStatus { get; set; }
        public string Id { get; set; }

        [Required(ErrorMessage = "Please select Prescribable in Flags / Classification")]
        public bool Prescribable { get; set; }

        public bool OriginalPrescribable { get; set; }

        public bool OutpatientMedication { get; set; }
        public string RecStatuschangeMsg { get; set; }
        public string RecSource { get; set; }
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

        public List<CodeNameSelectorModel> Diluents { get; set; }

        public bool IsIndicationMandatory { get; set; }

        public bool IsDiff { get; set; }
        public bool IsDiffBlackTriangle { get; set; }
        public bool IsDiffClinicalTrialMedication { get; set; }
        public bool IsDiffCriticalDrug { get; set; }
        public bool IsDiffEMAAddMontorng { get; set; }
        public bool IsDiffGastroResistant { get; set; }
        public bool IsDiffModifiedRelease { get; set; }
        public bool IsDiffExpensiveMedication { get; set; }
        public bool IsDiffHighAlertMedication { get; set; }
        public bool IsDiffIVtoOral { get; set; }
        public bool IsDiffNotforPRN { get; set; }
        public bool IsDiffBloodProduct { get; set; }
        public bool IsDiffDiluent { get; set; }
        public bool IsDiffPrescribable { get; set; }
        public bool IsDiffOutpatientMedication { get; set; }
        public bool IsDiffSugarFree { get; set; }
        public bool IsDiffGlutenFree { get; set; }
        public bool IsDiffPreservativeFree { get; set; }
        public bool IsDiffCFCFree { get; set; }
        public bool IsDiffUnlicensedMedication { get; set; }
        public bool IsDiffParallelImport { get; set; }
        public bool IsDiffIgnoreDuplicateWarnings { get; set; }
        public bool IsDiffControlledDrug { get; set; }
        public bool IsDiffIndicationIsMandatory { get; set; }
        public bool IsDiffWitnessingRequired { get; set; }

        public List<AnyDiff.Difference> Differences { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validator = GetValidatorByType();
            var results = validator.Validate();
            return results;
        }

        private IFormularyEditValidator GetValidatorByType()
        {
            switch (this.ProductType?.ToLower())
            {
                case "vtm":
                    return new VTMFormularyEditValidator(this);
                case "vmp":
                    return new VMPFormularyEditValidator(this);
                case "amp":
                    return new AMPFormularyEditValidator(this);
                default:
                    return new NullFormularyEditValidator();
            }
        }
    }


    public class BulkFormularyEditModel : FormularyEditModel
    {
        public bool? NullableClinicalTrialMedication { get; set; }
        public bool? NullableCriticalDrug { get; set; }
        public bool? NullableIVToOral { get; set; }
        public bool? NullableExpensiveMedication { get; set; }
        public bool? NullableHighAlertMedication { get; set; }
        public bool? NullableNotForPrn { get; set; }
        public bool? NullablePrescribable { get; set; }
        public bool? NullableOutpatientMedication { get; set; }
        public bool? NullableIgnoreDuplicateWarnings { get; set; }
        public bool? NullableWitnessingRequired { get; set; }
        public bool? NullableIsBloodProduct { get; set; }
        public bool? NullableIsDiluent { get; set; }
        public bool? NullableIsCustomControlledDrug { get; set; }
        public bool? NullableIsIndicationMandatory { get; set; }
        public bool? NullableIsGastroResistant { get; set; }
        public bool? NullableIsModifiedRelease { get; set; }

        public string SerializedOriginalObj { get; set; }


        /*
         * if (orginalObj.ClinicalTrialMedication == editedData.ClinicalTrialMedication)
            {
                editedData.ClinicalTrialMedication = modelFromDB.ClinicalTrialMedication;
            }
            if (orginalObj.CriticalDrug == editedData.CriticalDrug)
            {
                editedData.CriticalDrug = modelFromDB.CriticalDrug;
            }
            if (orginalObj.IVToOral == editedData.IVToOral)
            {
                editedData.IVToOral = modelFromDB.IVToOral;
            }
            if (orginalObj.ExpensiveMedication == editedData.ExpensiveMedication)
            {
                editedData.ExpensiveMedication = modelFromDB.ExpensiveMedication;
            }
            if (orginalObj.HighAlertMedication == editedData.HighAlertMedication)
            {
                editedData.HighAlertMedication = modelFromDB.HighAlertMedication;
            }
            if (orginalObj.NotForPrn == editedData.NotForPrn)
            {
                editedData.NotForPrn = modelFromDB.NotForPrn;
            }
            if (orginalObj.Prescribable == editedData.Prescribable)
            {
                editedData.Prescribable = modelFromDB.Prescribable;
            }
            if (orginalObj.OutpatientMedication == editedData.OutpatientMedication)
            {
                editedData.OutpatientMedication = modelFromDB.OutpatientMedication;
            }
            if (!editedData.TitrationTypes.IsCollectionValid())
            {
                editedData.TitrationTypes = modelFromDB.TitrationTypes;
            }
            if (orginalObj.RnohFormularyStatuscd == editedData.RnohFormularyStatuscd)
            {
                editedData.RnohFormularyStatuscd = modelFromDB.RnohFormularyStatuscd;
            }
            if (orginalObj.IgnoreDuplicateWarnings == editedData.IgnoreDuplicateWarnings)
            {
                editedData.IgnoreDuplicateWarnings = modelFromDB.IgnoreDuplicateWarnings;
            }
            if (orginalObj.WitnessingRequired == editedData.WitnessingRequired)
            {
                editedData.WitnessingRequired = modelFromDB.WitnessingRequired;
            }
         * 
         * 
         * 
         * 
         * 
         * 
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
            detailDTOFromSrc.Diluents = detailDTOFromDb.Diluents;
            detailDTOFromSrc.IsIndicationMandatory = detailDTOFromDb.IsIndicationMandatory;
         
         */

    }

}





