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


﻿//using Interneuron.Common.Extensions;
//using Newtonsoft.Json;
//using SynapseStudioWeb.AppCode.Constants;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace SynapseStudioWeb.Models.MedicinalMgmt
//{
//    public class AMPFormularyEditModel : BaseFormularyEditModel, IValidatableObject
//    {
//        public string FormularyVersionId { get; set; }

//        [Required(ErrorMessage = "Please enter Name in Product Details")]
//        public string Name { get; set; }

//        [Required(ErrorMessage = "Please select Product Type in Product Details")]
//        public string ProductType { get; set; }

//        [Required(ErrorMessage = "Please enter Code in Product Details")]
//        public string Code { get; set; }

//        [Required(ErrorMessage = "Please enter Code System in Product Details")]
//        public string CodeSystem { get; set; } = TerminologyConstants.PRIMARY_IDENTIFICATION_CODE_TYPE;

//        public List<FormularyAdditionalCodeModel> FormularyClassificationCodes { get; set; }

//        public List<FormularyAdditionalCodeModel> FormularyIdentificationCodes { get; set; }

//        [Required(ErrorMessage = "Please select Status in History")]
//        public string Status { get; set; }
//        public string OriginalStatus { get; set; }
//        public string History { get; set; }
//        public string Id { get; set; }

//        [Required(ErrorMessage = "Please select Formulary Status in Preferences")]
//        public string FormularyStatus { get; set; }

//        [Required(ErrorMessage = "Please select Prescribable in Flags / Classification")]
//        public bool Prescribable { get; set; }
//        public bool OutpatientMedication { get; set; }
//        public string RecStatuschangeMsg { get; set; }
//        public string RecSource { get; set; }
//        public bool HighAlertMedication { get; set; }
//        public string HighAlertMedicationSource { get; set; }

//        public bool IgnoreDuplicateWarnings { get; set; }
//        public List<CodeNameSelectorModel> Route { get; set; }
//        public List<CodeNameSelectorModel> UnlicensedRoute { get; set; }
//        public string MedusaPreparationInstructions { get; set; }
//        public bool CriticalDrug { get; set; }
//        public string ControlledDrugCategory { get; set; }
//        public string ControlledDrugCategorySource { get; set; } = TerminologyConstants.MANUAL_DATA_SOURCE;
//        public bool Cytotoxic { get; set; }
//        public bool Antibiotic { get; set; }
//        public bool Anticoagulant { get; set; }
//        public bool Antipsychotic { get; set; }
//        public bool Antimicrobial { get; set; }
//        public bool IVToOral { get; set; }
//        public string TitrationType { get; set; }
//        public string RoundingFactor { get; set; }
//        public string MaximumDoseNumerator { get; set; }
//        public bool WitnessingRequired { get; set; }
//        public string NotesForRestriction { get; set; }
//        public List<CodeNameSelectorModel> Cautions { get; set; }
//        public List<FormularyCustomWarningModel> CustomWarnings { get; set; }
//        public List<CodeNameSelectorModel> ContraIndications { get; set; }
//        public List<CodeNameSelectorModel> SideEffects { get; set; }
//        public List<CodeNameSelectorModel> SafetyMessages { get; set; }
//        public List<string> Endorsements { get; set; }
//        public string NICETA { get; set; }
//        public List<CodeNameSelectorModel> LicensedUse { get; set; }
//        public List<CodeNameSelectorModel> UnlicensedUse { get; set; }
//        public bool BlackTriangle { get; set; }
//        public string BlackTriangleSource { get; set; } = TerminologyConstants.MANUAL_DATA_SOURCE;
//        public bool IsRecordStatusReadOnly { get; set; }
//        public bool IsSaveSuccessful { get; set; }
//        public string ParentCode { get; set; }
//        public string ParentName { get; set; }
//        public string ParentProductType { get; set; }
//        public bool? IsDuplicate { get; set; }
//        public string DuplicateOfFormularyId { get; set; }
//        public Dictionary<string, bool> ControlDisplayMetaDetails { get; set; }
//        public bool IsBloodProduct { get; set; }
//        public bool IsDiluent { get; set; }


//        public string CurrentLicensingAuthority { get; set; }
//        public CodeNameSelectorModel Supplier { get; set; }
//        public CodeNameSelectorModel TradeFamily { get; set; }
//        public string VirtualMedicinalProduct { get; set; }
//        public bool EMAAdditionalMonitoring { get; set; }
//        public bool ExpensiveMedication { get; set; }
//        public bool ClinicalTrialMedication { get; set; }
//        public string RestrictionsOnAvailability { get; set; }
//        public bool ParallelImport { get; set; }
//        public bool UnlicensedMedication { get; set; }



//        public string BasisOfPreferredName { get; set; }
//        public string VirtualTherapeuticMoiety { get; set; }
//        public CodeNameSelectorModel FormCd { get; set; }
//        public string DoseForm { get; set; }
//        public decimal? UnitDoseFormSize { get; set; }
//        public CodeNameSelectorModel UnitDoseFormUnits { get; set; }
//        public CodeNameSelectorModel UnitDoseUnitOfMeasure { get; set; }

//        public List<FormularyIngredientModel> Ingredients { get; set; }

//        public bool SugarFree { get; set; }
//        public bool GlutenFree { get; set; }
//        public bool PreservativeFree { get; set; }

//        public bool CFCFree { get; set; }

//        public string PrescribingStatus { get; set; }

//        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}



