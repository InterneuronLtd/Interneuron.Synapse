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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.DataService.APIModel
{
    //public class FormularyHeaderAPIModel
    //{
    //    //public string rowId { get; set; }
    //    //public DateTime? createddate { get; set; }
    //    //public string createdby { get; set; }
    //    //public string tenant { get; set; }
    //    //public string formularyId { get; set; }
    //    //public int? versionId { get; set; }
    //    //public string formularyVersionId { get; set; }
    //    //public string code { get; set; }
    //    //public string name { get; set; }
    //    //public string productType { get; set; }
    //    //public string parentCode { get; set; }
    //    //public string parentName { get; set; }
    //    //public string parentProductType { get; set; }
    //    //public string recStatusCode { get; set; }
    //    //public DateTime? recStatuschangeDate { get; set; }
    //    //public bool? isDuplicate { get; set; }
    //    //public bool? isLatest { get; set; }
    //    //public string recSource { get; set; }
    //    //public string vtmId { get; set; }
    //    //public string vmpId { get; set; }
    //   // public List<FormularyAdditionalCodeAPIModel> formularyAdditionalCodes { get; set; }
    //   // public DetailAPIModel detail { get; set; }
    //   // public List<FormularyIndicationAPIModel> formularyIndications { get; set; }
    //    //public List<FormularyIngredientAPIModel> formularyIngredients { get; set; }
    //    //public List<FormularyRouteDetailAPIModel> formularyRouteDetails { get; set; }


    //    //public string RowId { get; set; }
    //    //public DateTime? Createddate { get; set; }
    //    //public string Createdby { get; set; }
    //    //public DateTime? Updateddate { get; set; }
    //    //public string Updatedby { get; set; }
    //    //public string FormularyId { get; set; }
    //    //public int? VersionId { get; set; }
    //    //public string FormularyVersionId { get; set; }
    //    //public string Code { get; set; }
    //    //public string Name { get; set; }
    //    //public string ProductType { get; set; }
    //    //public string ParentCode { get; set; }
    //    //public string ParentName { get; set; }
    //    //public string ParentProductType { get; set; }
    //    //public string RecStatusCode { get; set; }
    //    //public DateTime? RecStatuschangeTs { get; set; }
    //    //public DateTime? RecStatuschangeDate { get; set; }
    //    //public string RecStatuschangeTzname { get; set; }
    //    //public int? RecStatuschangeTzoffset { get; set; }
    //    //public bool? IsDuplicate { get; set; }
    //    //public string RecStatuschangeMsg { get; set; }
    //    //public string DuplicateOfFormularyId { get; set; }

    //    //public bool? IsLatest { get; set; }
    //    //public string RecSource { get; set; }
    //    //public string VtmId { get; set; }
    //    //public string VmpId { get; set; }
    //    //public string CodeSystem { get; set; }

    //    //public List<FormularyAdditionalCodeAPIModel> FormularyAdditionalCodes { get; set; }
    //    //public FormularyDetailAPIModel Detail { get; set; }
    //    //public List<FormularyIndicationAPIModel> FormularyIndications { get; set; }
    //    //public List<FormularyIngredientAPIModel> FormularyIngredients { get; set; }
    //    //public List<FormularyRouteDetailAPIModel> FormularyRouteDetails { get; set; }
    //    //public List<FormularyOntologyFormAPIModel> FormularyOntologyForms { get; set; }

    //    ////public List<FormularySupplierDTO> FormularySuppliers { get; set; }


    //}

    public class FormularyHeaderAPIModel
    {
        public string RowId { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Updatedby { get; set; }
        public string FormularyId { get; set; }
        public long? VersionId { get; set; }
        public string FormularyVersionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ProductType { get; set; }
        public string ParentCode { get; set; }
        public string ParentName { get; set; }
        public string ParentProductType { get; set; }
        public string RecStatusCode { get; set; }
        public DateTime? RecStatuschangeTs { get; set; }
        public DateTime? RecStatuschangeDate { get; set; }
        public string RecStatuschangeTzname { get; set; }
        public long? RecStatuschangeTzoffset { get; set; }
        public bool? IsDuplicate { get; set; }
        public string RecStatuschangeMsg { get; set; }
        public string DuplicateOfFormularyId { get; set; }
        public bool? IsLatest { get; set; }
        public string RecSource { get; set; }
        public string VtmId { get; set; }
        public string VmpId { get; set; }
        public string CodeSystem { get; set; }
        public List<FormularyAdditionalCodeAPIModel> FormularyAdditionalCodes { get; set; }
        public FormularyDetailAPIModel Detail { get; set; }
        public List<FormularyIngredientAPIModel> FormularyIngredients { get; set; }
        public List<FormularyRouteDetailAPIModel> FormularyRouteDetails { get; set; }

        public List<FormularyLocalRouteDetailAPIModel> FormularyLocalRouteDetails { get; set; }
        public List<FormularyExcipientAPIModel> FormularyExcipients { get; set; }
    }

    public class FormularyDetailAPIModel
    {
        public string RowId { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Updatedby { get; set; }
        public string FormularyVersionId { get; set; }
        public string RnohFormularyStatuscd { get; set; }
        public string RnohFormularyStatusDesc { get; set; }
        public string OutpatientMedicationCd { get; set; }
        public string PrescribingStatusCd { get; set; }
        public string PrescribingStatusDesc { get; set; }
        public string UnlicensedMedicationCd { get; set; }
        public string DefinedDailyDose { get; set; }
        public string NotForPrn { get; set; }
        public string HighAlertMedication { get; set; }
        public string HighAlertMedicationSource { get; set; }
        public string IgnoreDuplicateWarnings { get; set; }
        public List<string> MedusaPreparationInstructions { get; set; }
        public string CriticalDrug { get; set; }
        public List<FormularyLookupAPIModel> ControlledDrugCategories { get; set; }
        public string Cytotoxic { get; set; }
        public string ClinicalTrialMedication { get; set; }
        public string IvToOral { get; set; }
        public List<FormularyLookupAPIModel> TitrationTypes { get; set; }
        public string RoundingFactorCd { get; set; }
        public string RoundingFactorDesc { get; set; }
        public long? MaxDoseNumerator { get; set; }
        public string WitnessingRequired { get; set; }
        public List<string> NiceTas { get; set; }
        public string BasisOfPreferredNameCd { get; set; }
        public string BasisOfPreferredNameDesc { get; set; }
        public string SugarFree { get; set; }
        public string GlutenFree { get; set; }
        public string PreservativeFree { get; set; }
        public string CfcFree { get; set; }
        public string DoseFormCd { get; set; }
        public string DoseFormDesc { get; set; }
        public decimal? UnitDoseFormSize { get; set; }
        public string UnitDoseFormUnits { get; set; }
        public string UnitDoseFormUnitsDesc { get; set; }
        public string UnitDoseUnitOfMeasureCd { get; set; }
        public string UnitDoseUnitOfMeasureDesc { get; set; }
        public string FormCd { get; set; }
        public string FormDesc { get; set; }
        public string TradeFamilyCd { get; set; }
        public string TradeFamilyName { get; set; }
        public string ExpensiveMedication { get; set; }
        public string BlackTriangle { get; set; }
        public string BlackTriangleSource { get; set; }
        public string SupplierCd { get; set; }
        public string SupplierName { get; set; }
        public string CurrentLicensingAuthorityCd { get; set; }
        public string CurrentLicensingAuthorityDesc { get; set; }
        public string EmaAdditionalMonitoring { get; set; }
        public string ParallelImport { get; set; }
        public string RestrictionsOnAvailabilityCd { get; set; }
        public string RestrictionsOnAvailabilityDesc { get; set; }
        public string DrugClass { get; set; }
        public string RestrictionNote { get; set; }
        public List<FormularyLookupAPIModel> SideEffects { get; set; }
        public List<FormularyLookupAPIModel> Cautions { get; set; }
        public List<FormularyLookupAPIModel> ContraIndications { get; set; }
        public List<FormularyLookupAPIModel> SafetyMessages { get; set; }
        public List<FormularyCustomWarningAPIModel> CustomWarnings { get; set; }
        public List<FormularyReminderAPIModel> Reminders { get; set; }
        public List<string> Endorsements { get; set; }
        public List<FormularyLookupAPIModel> LicensedUses { get; set; }
        public List<FormularyLookupAPIModel> UnLicensedUses { get; set; }

        public List<FormularyLookupAPIModel> LocalLicensedUses { get; set; }
        public List<FormularyLookupAPIModel> LocalUnLicensedUses { get; set; }
        public bool? IsBloodProduct { get; set; }
        public bool? IsDiluent { get; set; }
        public bool? IsModifiedRelease { get; set; }
        public bool? IsGastroResistant { get; set; }
        public bool? Prescribable { get; set; }
        public string PrescribableSource { get; set; }
        public bool? IsCustomControlledDrug { get; set; }
        public List<FormularyLookupAPIModel> Diluents { get; set; }
        public bool? IsIndicationMandatory { get; set; }

    }


    public class FormularyCustomWarningAPIModel
    {
        public string Warning { get; set; }
        public bool? NeedResponse { get; set; }
        public string Source { get; set; }
    }

    public class FormularyReminderAPIModel
    {
        public string Reminder { get; set; }
        public int Duration { get; set; }
        public bool? Active { get; set; }
        public string Source { get; set; }
    }

    public class FormularyAdditionalCodeAPIModel
    {
        public string RowId { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public string FormularyVersionId { get; set; }
        public string AdditionalCode { get; set; }
        public string AdditionalCodeSystem { get; set; }
        public string AdditionalCodeDesc { get; set; }
        public string Attr1 { get; set; }
        public string MetaJson { get; set; }
        public string Source { get; set; }
        public string CodeType { get; set; }
    }

    public class FormularyExcipientAPIModel
    {
        public string RowId { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public string Timezonename { get; set; }
        public long? Timezoneoffset { get; set; }
        public string Tenant { get; set; }
        public string FormularyVersionId { get; set; }
        public string IngredientCd { get; set; }
        public string Strength { get; set; }
        public string StrengthUnitCd { get; set; }
        public DateTime? Updatedtimestamp { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Updatedby { get; set; }
    }

    public class FormularyIngredientAPIModel
    {
        public string RowId { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Updatedby { get; set; }
        public string FormularyVersionId { get; set; }
        public string IngredientCd { get; set; }
        public string IngredientName { get; set; }
        public string BasisOfPharmaceuticalStrengthCd { get; set; }
        public string BasisOfPharmaceuticalStrengthDesc { get; set; }
        public string StrengthValueNumerator { get; set; }
        public string StrengthValueNumeratorUnitCd { get; set; }
        public string StrengthValueNumeratorUnitDesc { get; set; }
        public string StrengthValueDenominator { get; set; }
        public string StrengthValueDenominatorUnitCd { get; set; }
        public string StrengthValueDenominatorUnitDesc { get; set; }
    }

    public class FormularyRouteDetailAPIModel
    {
        public string RowId { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Updatedby { get; set; }
        public string FormularyVersionId { get; set; }
        public string RouteCd { get; set; }
        public string RouteDesc { get; set; }
        public string RouteFieldTypeCd { get; set; }
        public string RouteFieldTypeDesc { get; set; }
        public string Source { get; set; }
    }

    public class FormularyLocalRouteDetailAPIModel
    {
        public string RowId { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Updatedby { get; set; }
        public string FormularyVersionId { get; set; }
        public string RouteCd { get; set; }
        public string RouteDesc { get; set; }
        public string RouteFieldTypeCd { get; set; }
        public string RouteFieldTypeDesc { get; set; }
        public string Source { get; set; }
    }

}
