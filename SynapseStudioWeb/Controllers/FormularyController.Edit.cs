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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AnyDiff;
using AnyDiff.Extensions;
using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SynapseStudioWeb.AppCode.Constants;
using SynapseStudioWeb.Controllers.MedicationManagement.BulkEditMergeHandlers;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.DataService.APIModel;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models.MedicationMgmt;
using SynapseStudioWeb.Models.MedicinalMgmt;

namespace SynapseStudioWeb.Controllers
{
    public partial class FormularyController : Controller
    {

        [HttpPost]
        [Route("Formulary/LoadFormularyDetails")]
        public async Task<IActionResult> LoadFormularyDetails(string formularyVersionId)
        {
            const string MMC_CONFIG_KEY = "MMC_Control_Source";

            string token = HttpContext.Session.GetString("access_token");

            var responseTask = TerminologyAPIService.GetFormularyDetailRuleBound(formularyVersionId, token);// TerminologyAPIService.GetFormularyDetail(formularyVersionId, token);

            var terminologyConfigTask = TerminologyAPIService.GetTerminologyConfiguration(MMC_CONFIG_KEY, token);

            await Task.WhenAll(responseTask, terminologyConfigTask);

            var response = await responseTask;

            var terminologyConfigResponse = await terminologyConfigTask;

            if ((response == null || response.StatusCode == DataService.APIModel.StatusCode.Fail) || ((terminologyConfigResponse == null || terminologyConfigResponse.StatusCode == DataService.APIModel.StatusCode.Fail)))
            {
                const string ErrorResponse = "Error getting details for the selected record";
                var errors = "";
                if (response.ErrorMessages.IsCollectionValid())
                {
                    errors = string.Join('\n', ErrorResponse, response.ErrorMessages.ToArray());
                }
                if (terminologyConfigResponse.ErrorMessages.IsCollectionValid())
                {
                    errors = string.Join('\n', ErrorResponse, terminologyConfigResponse.ErrorMessages.ToArray());
                }
                _toastNotification.AddErrorToastMessage(errors);

                return null;
            }

            var formulariesfromAPI = response.Data;

            await FillLookupsToViewBag(formulariesfromAPI.RecStatusCode);

            FormularyEditModel vm = GetFormularyEditModel(formulariesfromAPI);

            return PartialView(GetFormularyEditView(formulariesfromAPI.ProductType), vm);
        }

        private string GetFormularyEditView(string productType)
        {
            if (productType.IsEmpty()) return null;

            if (string.Compare(productType, "vmp", true) == 0)
                return "_VMPFormularyEdit";
            if (string.Compare(productType, "amp", true) == 0)
                return "_AMPFormularyEdit";
            if (string.Compare(productType, "vtm", true) == 0)
                return "_VTMFormularyEdit";

            return null;
        }

        private string GetFormularyEditViewForBulkEdit(string productType)
        {
            if (productType.IsEmpty()) return null;

            if (string.Compare(productType, "vmp", true) == 0)
                return "_VMPFormularyBulkEdit";
            if (string.Compare(productType, "amp", true) == 0)
                return "_AMPFormularyBulkEdit";
            if (string.Compare(productType, "vtm", true) == 0)
                return "_VTMFormularyBulkEdit";

            return null;
        }

        private FormularyEditModel GetFormularyEditModel(FormularyHeaderAPIModel formulariesfromAPI)
        {
            var vmProvider = new FormularyEditViewModelProvider(this._mapper, this.HttpContext)
            {
                ApiModelProvider = () => formulariesfromAPI
            };
            return vmProvider.GetFormularyEditVM();
        }

        [HttpGet]
        [Route("Formulary/GetBulkEditSelectorPartial")]
        public IActionResult GetBulkEditSelectorPartial()
        {
            return PartialView("_FormularyBulkEditSelector");
        }

        [HttpGet]
        [Route("Formulary/GetProductTypeBulkEditPartial")]
        public async Task<IActionResult> GetProductTypeBulkEditPartial(string formularyVersionIds, string productType, string status)
        {
            var formularyVersionIdList = JsonConvert.DeserializeObject<List<string>>(formularyVersionIds);

            var editPartial = GetFormularyEditViewForBulkEdit(productType);

            var vm = new BulkFormularyEditModel
            {
                FormularyVersionIds = formularyVersionIdList,
                ProductType = productType,
                ControlIdentifier = "Edit_Formulary",
                IsBulkEdit = true
            };

            vm.SerializedOriginalObj = JsonConvert.SerializeObject(vm);

            string statusCode = "";

            switch (status.ToLower())
            {
                case "draft":
                    statusCode = "001";
                    break;
                case "active":
                    statusCode = "003";
                    break;
                case "archived":
                    statusCode = "004";
                    break;
                case "inactive":
                    statusCode = "005";
                    break;
                case "deleted":
                    statusCode = "006";
                    break;
                case "ready for review":
                    statusCode = "002";
                    break;
                
            }

            await FillLookupsToViewBag(statusCode);

            return PartialView(editPartial, vm);
        }

        [HttpPost]
        [Route("Formulary/UpdateBulkFormulary")]
        public async Task<IActionResult> UpdateBulkFormulary(BulkFormularyEditModel editedData)
        {
            return await UpdateFormulary(editedData);
        }

        [HttpPost]
        [Route("Formulary/UpdateFormulary")]
        public async Task<IActionResult> UpdateFormulary(FormularyEditModel editedData)
        {
            BulkFormularyEditModel bulkEditedData = null;

            string token = HttpContext.Session.GetString("access_token");

            var partialView = GetFormularyEditView(editedData.ProductType);

            if (editedData.IsBulkEdit)
            {
                bulkEditedData = editedData as BulkFormularyEditModel;
                partialView = GetFormularyEditViewForBulkEdit(editedData.ProductType);
            }


            if (!ModelState.IsValid)
            {
                await FillLookupsToViewBag(editedData.OriginalStatus);

                editedData.IsSaveSuccessful = false;
                return PartialView(partialView, editedData);
            }

            var apiModels = new List<FormularyHeaderAPIModel>();

            var uniqueIds = editedData.FormularyVersionIds;

            foreach (var rec in uniqueIds)
            {
                FormularyHeaderAPIModel apiModel = null;

                if (editedData.IsBulkEdit)
                {
                    var orginalObj = JsonConvert.DeserializeObject<BulkFormularyEditModel>(bulkEditedData.SerializedOriginalObj);

                    var edDataDeserializedObj = _mapper.Map<BulkFormularyEditModel>(bulkEditedData);

                    await MergeOnlyModified(edDataDeserializedObj, orginalObj, rec);

                    apiModel = _mapper.Map<FormularyHeaderAPIModel>(edDataDeserializedObj);
                }
                else
                {
                    apiModel = _mapper.Map<FormularyHeaderAPIModel>(editedData);
                }

                apiModel.FormularyVersionId = rec;
                apiModel.RecStatusCode = apiModel.RecStatusCode;  //editedData.IsBulkEdit ? null : apiModel.RecStatusCode;//For bulk-edit mode - Record status is not editable
                apiModels.Add(apiModel);
            }

            TerminologyAPIResponse<UpdateFormularyAPIResponse> response = await TerminologyAPIService.UpdateFormulary(apiModels, token);

            if (response.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                await FillLookupsToViewBag(editedData.OriginalStatus);

                _toastNotification.AddErrorToastMessage(string.Join('\n', response.ErrorMessages));

                editedData.IsSaveSuccessful = false;

                return PartialView(partialView, editedData);
            }

            if (response.StatusCode == DataService.APIModel.StatusCode.Success)
            {
                if (response.Data.Status != null && response.Data.Status.ErrorMessages.IsCollectionValid())
                {
                    var errors = string.Join('\n', response.Data.Status.ErrorMessages);

                    _toastNotification.AddInfoToastMessage(errors);

                    await FillLookupsToViewBag(editedData.OriginalStatus);

                    editedData.IsSaveSuccessful = true;

                    return PartialView(partialView, editedData);
                }
            }

            _toastNotification.AddSuccessToastMessage("Successfully Updated Formulary details");

            editedData.IsSaveSuccessful = true;

            return Json(null);
        }

        /// <summary>
        /// This method will check if the field is dirty after edit and will consider only those dirty fields for update - rest are ignored
        /// </summary>
        /// <param name="editedData"></param>
        /// <param name="orginalObj"></param>
        private async Task MergeOnlyModified(BulkFormularyEditModel editedData, BulkFormularyEditModel orginalObj, string currentFormularyVersionId)
        {
            string token = HttpContext.Session.GetString("access_token");

            var response = await TerminologyAPIService.GetFormularyDetailRuleBound(currentFormularyVersionId, token);

            var formulariesfromAPI = response.Data;

            await FillLookupsToViewBag(formulariesfromAPI.RecStatusCode);

            var modelFromDb = GetFormularyEditModel(formulariesfromAPI);

            IBulkEditMergeHandler mergeHandler = null;

            if (string.Compare(editedData.ProductType, "vtm", true) == 0)
                mergeHandler = new VTMBulkEditMergeHandler();
            else if (string.Compare(editedData.ProductType, "vmp", true) == 0)
                mergeHandler = new VMPBulkEditMergeHandler();
            else if (string.Compare(editedData.ProductType, "amp", true) == 0)
                mergeHandler = new AMPBulkEditMergeHandler();

            mergeHandler.Merge(editedData, orginalObj, modelFromDb);
        }

        [HttpPost]
        [Route("Formulary/LoadHistoryFormularyDetails")]
        public async Task<IActionResult> LoadHistoryFormularyDetails(string previousFormularyVersionId, string currentFormularyVersionId, string previousOrCurrent)
        {
            const string MMC_CONFIG_KEY = "MMC_Control_Source";

            string token = HttpContext.Session.GetString("access_token");

            var previousResponseTask = TerminologyAPIService.GetFormularyDetailRuleBound(previousFormularyVersionId, token);// TerminologyAPIService.GetFormularyDetail(formularyVersionId, token);

            var currentResponseTask = TerminologyAPIService.GetFormularyDetailRuleBound(currentFormularyVersionId, token);

            var terminologyConfigTask = TerminologyAPIService.GetTerminologyConfiguration(MMC_CONFIG_KEY, token);

            await Task.WhenAll(previousResponseTask, terminologyConfigTask);

            await Task.WhenAll(currentResponseTask, terminologyConfigTask);

            var previousResponse = await previousResponseTask;

            var currentResponse = await currentResponseTask;

            var terminologyConfigResponse = await terminologyConfigTask;

            if ((previousResponse == null || previousResponse.StatusCode == DataService.APIModel.StatusCode.Fail) || (currentResponse == null || currentResponse.StatusCode == DataService.APIModel.StatusCode.Fail) || ((terminologyConfigResponse == null || terminologyConfigResponse.StatusCode == DataService.APIModel.StatusCode.Fail)))
            {
                const string ErrorResponse = "Error getting details for the selected record";
                var errors = "";
                if (previousResponse.ErrorMessages.IsCollectionValid())
                {
                    errors = string.Join('\n', ErrorResponse, previousResponse.ErrorMessages.ToArray());
                }
                if (currentResponse.ErrorMessages.IsCollectionValid())
                {
                    errors = string.Join('\n', ErrorResponse, currentResponse.ErrorMessages.ToArray());
                }
                if (terminologyConfigResponse.ErrorMessages.IsCollectionValid())
                {
                    errors = string.Join('\n', ErrorResponse, terminologyConfigResponse.ErrorMessages.ToArray());
                }
                _toastNotification.AddErrorToastMessage(errors);

                return null;
            }

            var previousFormulariesfromAPI = previousResponse.Data;

            var currentFormulariesfromAPI = currentResponse.Data;

            await FillLookupsToViewBag(previousFormulariesfromAPI.RecStatusCode);

            await FillLookupsToViewBag(currentFormulariesfromAPI.RecStatusCode);

            FormularyEditModel previousVM = GetFormularyEditModel(previousFormulariesfromAPI);

            FormularyEditModel currentVM = GetFormularyEditModel(currentFormulariesfromAPI);

            var prevProductName = previousVM.Name;
            var prevProductType = previousVM.ProductType;
            var prevCode = previousVM.Code;
            var prevCodeSystem = previousVM.CodeSystem;

            var currProductName = currentVM.Name;
            var currProductType = currentVM.ProductType;
            var currCode = currentVM.Code;
            var currCodeSystem = currentVM.CodeSystem;

            var differences = previousVM.Diff(currentVM, ComparisonOptions.All | ComparisonOptions.AllowCollectionsToBeOutOfOrder | ComparisonOptions.AllowEqualsOverride, 
                ".FormularyVersionIds", ".Excipients.FormularyVersionId", ".FormularyClassificationCodes.FormularyVersionId", ".FormularyIdentificationCodes.FormularyVersionId", ".Ingredients.FormularyVersionId", 
                ".UnlicensedRoute.Source", ".UnlicensedRoute.SourceColor", ".Route.Source", ".Route.SourceColor", ".LocalLicensedRoute.Source", ".LocalLicensedRoute.SourceColor", ".LocalUnlicensedRoute.Source", ".LocalUnlicensedRoute.SourceColor",
                ".Cautions.Source", ".Cautions.SourceColor", ".ContraIndications.Source", ".ContraIndications.SourceColor", ".SideEffects.Source", ".SideEffects.SourceColor", ".SafetyMessages.Source", ".SafetyMessages.SourceColor",
                ".LicensedUse.Source", ".LicensedUse.SourceColor", ".UnlicensedUse.Source", ".UnlicensedUse.SourceColor", ".LocalLicensedUse.Source", ".LocalLicensedUse.SourceColor", ".LocalUnlicensedUse.Source", ".LocalUnlicensedUse.SourceColor",
                ".FormCd.Source", ".FormCd.SourceColor", ".UnitDoseFormUnits.Source", ".UnitDoseFormUnits.SourceColor", ".UnitDoseUnitOfMeasure.Source", ".UnitDoseUnitOfMeasure.SourceColor", ".Supplier.Source", ".Supplier.SourceColor",
                ".TradeFamily.Source", ".TradeFamily.SourceColor", ".MedusaPreparationInstructions.Source", ".MedusaPreparationInstructions.SourceColor", ".ControlledDrugCategories.Source", ".ControlledDrugCategories.SourceColor",
                ".TitrationTypes.Source", ".TitrationTypes.SourceColor", ".Diluents.Source", ".Diluents.SourceColor", ".HighAlertMedicationSource", ".BlackTriangleSource", ".PrescribableSource");

            if (differences.Count > 0)
            {

                List<KeyValuePair<string, object>> prevObjects = new List<KeyValuePair<string, object>>();

                foreach (var diff in differences)
                {
                    prevObjects.Add(new KeyValuePair<string, object>(diff.Property, diff.LeftValue));
                }

                FormularyEditModel prevVM = new FormularyEditModel();

                prevVM.Differences = new List<Difference>();

                prevVM.Differences.AddRange(differences);

                var prevVMType = prevVM.GetType();

                foreach (KeyValuePair<string, object> _pair in prevObjects)
                {
                    if(prevVMType.GetProperty(_pair.Key).PropertyType.Name == "List`1")
                    {
                        if(_pair.Value != null)
                        {
                            object instance = Activator.CreateInstance(prevVMType.GetProperty(_pair.Key).PropertyType);

                            IList list = (IList)instance;

                            list.Add(_pair.Value);

                            if (prevVM.GetType().GetProperty(_pair.Key).GetValue(prevVM, null).IsNotNull())
                            {
                                foreach (var value in (dynamic)prevVM.GetType().GetProperty(_pair.Key).GetValue(prevVM, null))
                                {
                                    list.Add(value);
                                }
                                prevVMType.GetProperty(_pair.Key).SetValue(prevVM, list, null);
                            }
                            else
                            {
                                prevVMType.GetProperty(_pair.Key).SetValue(prevVM, list, null);
                            }
                        }
                        else
                        {
                            object instance = Activator.CreateInstance(prevVMType.GetProperty(_pair.Key).PropertyType);

                            IList list = (IList)instance;

                            list.Add(_pair.Value);

                            prevVMType.GetProperty(_pair.Key).SetValue(prevVM, list, null);
                        }
                    }
                    else
                    {
                        prevVMType.GetProperty(_pair.Key).SetValue(prevVM, _pair.Value);
                    }
                }

                previousVM = prevVM;

                previousVM.IsDiff = true;

                previousVM.Name = prevProductName;
                previousVM.ProductType = prevProductType;
                previousVM.Code = prevCode;
                previousVM.CodeSystem = prevCodeSystem;
                
                List<KeyValuePair<string, object>> currObjects = new List<KeyValuePair<string, object>>();

                foreach (var diff in differences)
                {
                    currObjects.Add(new KeyValuePair<string, object>(diff.Property, diff.RightValue));
                }

                FormularyEditModel currVM = new FormularyEditModel();

                currVM.Differences = new List<Difference>();

                currVM.Differences.AddRange(differences);

                var currVMType = currVM.GetType();

                foreach (KeyValuePair<string, object> _pair in currObjects)
                {
                    if (currVMType.GetProperty(_pair.Key).PropertyType.Name == "List`1")
                    {
                        if(_pair.Value != null)
                        {
                            object instance = Activator.CreateInstance(currVMType.GetProperty(_pair.Key).PropertyType);

                            IList list = (IList)instance;

                            list.Add(_pair.Value);

                            if(currVM.GetType().GetProperty(_pair.Key).GetValue(currVM, null).IsNotNull())
                            {
                                foreach(var value in (dynamic)currVM.GetType().GetProperty(_pair.Key).GetValue(currVM, null))
                                {
                                    list.Add(value);
                                }
                                currVMType.GetProperty(_pair.Key).SetValue(currVM, list, null);
                            }
                            else
                            {
                                currVMType.GetProperty(_pair.Key).SetValue(currVM, list, null);
                            }
                        }
                        else
                        {
                            object instance = Activator.CreateInstance(currVMType.GetProperty(_pair.Key).PropertyType);

                            IList list = (IList)instance;

                            list.Add(_pair.Value);

                            currVMType.GetProperty(_pair.Key).SetValue(currVM, list, null);
                        }

                    }
                    else
                    {
                        currVMType.GetProperty(_pair.Key).SetValue(currVM, _pair.Value);
                    }

                }

                currentVM = currVM;

                currentVM.IsDiff = true;

                currentVM.Name = currProductName;
                currentVM.ProductType = currProductType;
                currentVM.Code = currCode;
                currentVM.CodeSystem = currCodeSystem;

            }
            else
            {
                previousVM = new FormularyEditModel();
                currentVM = new FormularyEditModel();

                previousVM.IsDiff = false;

                previousVM.Name = prevProductName;
                previousVM.ProductType = prevProductType;
                previousVM.Code = prevCode;
                previousVM.CodeSystem = prevCodeSystem;

                currentVM.IsDiff = false;

                currentVM.Name = currProductName;
                currentVM.ProductType = currProductType;
                currentVM.Code = currCode;
                currentVM.CodeSystem = currCodeSystem;
            }

            if(previousVM.BlackTriangle != currentVM.BlackTriangle)
            {
                previousVM.IsDiffBlackTriangle = true;
                currentVM.IsDiffBlackTriangle = true;
            }
            else
            {
                previousVM.IsDiffBlackTriangle = false;
                currentVM.IsDiffBlackTriangle = false;
            }

            if (previousVM.ClinicalTrialMedication != currentVM.ClinicalTrialMedication)
            {
                previousVM.IsDiffClinicalTrialMedication = true;
                currentVM.IsDiffClinicalTrialMedication = true;
            }
            else
            {
                previousVM.IsDiffClinicalTrialMedication = false;
                currentVM.IsDiffClinicalTrialMedication = false;
            }

            if (previousVM.CriticalDrug != currentVM.CriticalDrug)
            {
                previousVM.IsDiffCriticalDrug = true;
                currentVM.IsDiffCriticalDrug = true;
            }
            else
            {
                previousVM.IsDiffCriticalDrug = false;
                currentVM.IsDiffCriticalDrug = false;
            }

            if (previousVM.EmaAdditionalMonitoring != currentVM.EmaAdditionalMonitoring)
            {
                previousVM.IsDiffEMAAddMontorng = true;
                currentVM.IsDiffEMAAddMontorng = true;
            }
            else
            {
                previousVM.IsDiffEMAAddMontorng = false;
                currentVM.IsDiffEMAAddMontorng = false;
            }

            if (previousVM.IsGastroResistant != currentVM.IsGastroResistant)
            {
                previousVM.IsDiffGastroResistant = true;
                currentVM.IsDiffGastroResistant = true;
            }
            else
            {
                previousVM.IsDiffGastroResistant = false;
                currentVM.IsDiffGastroResistant = false;
            }

            if (previousVM.IsModifiedRelease != currentVM.IsModifiedRelease)
            {
                previousVM.IsDiffModifiedRelease = true;
                currentVM.IsDiffModifiedRelease = true;
            }
            else
            {
                previousVM.IsDiffModifiedRelease = false;
                currentVM.IsDiffModifiedRelease = false;
            }

            if (previousVM.ExpensiveMedication != currentVM.ExpensiveMedication)
            {
                previousVM.IsDiffExpensiveMedication = true;
                currentVM.IsDiffExpensiveMedication = true;
            }
            else
            {
                previousVM.IsDiffExpensiveMedication = false;
                currentVM.IsDiffExpensiveMedication = false;
            }

            if (previousVM.HighAlertMedication != currentVM.HighAlertMedication)
            {
                previousVM.IsDiffHighAlertMedication = true;
                currentVM.IsDiffHighAlertMedication = true;
            }
            else
            {
                previousVM.IsDiffHighAlertMedication = false;
                currentVM.IsDiffHighAlertMedication = false;
            }

            if (previousVM.IVToOral != currentVM.IVToOral)
            {
                previousVM.IsDiffIVtoOral = true;
                currentVM.IsDiffIVtoOral = true;
            }
            else
            {
                previousVM.IsDiffIVtoOral = false;
                currentVM.IsDiffIVtoOral = false;
            }

            if (previousVM.NotForPrn != currentVM.NotForPrn)
            {
                previousVM.IsDiffNotforPRN = true;
                currentVM.IsDiffNotforPRN = true;
            }
            else
            {
                previousVM.IsDiffNotforPRN = false;
                currentVM.IsDiffNotforPRN = false;
            }

            if (previousVM.IsBloodProduct != currentVM.IsBloodProduct)
            {
                previousVM.IsDiffBloodProduct = true;
                currentVM.IsDiffBloodProduct = true;
            }
            else
            {
                previousVM.IsDiffBloodProduct = false;
                currentVM.IsDiffBloodProduct = false;
            }

            if (previousVM.IsDiluent != currentVM.IsDiluent)
            {
                previousVM.IsDiffDiluent = true;
                currentVM.IsDiffDiluent = true;
            }
            else
            {
                previousVM.IsDiffDiluent = false;
                currentVM.IsDiffDiluent = false;
            }

            if (previousVM.Prescribable != currentVM.Prescribable)
            {
                previousVM.IsDiffPrescribable = true;
                currentVM.IsDiffPrescribable = true;
            }
            else
            {
                previousVM.IsDiffPrescribable = false;
                currentVM.IsDiffPrescribable = false;
            }

            if (previousVM.OutpatientMedication != currentVM.OutpatientMedication)
            {
                previousVM.IsDiffOutpatientMedication = true;
                currentVM.IsDiffOutpatientMedication = true;
            }
            else
            {
                previousVM.IsDiffOutpatientMedication = false;
                currentVM.IsDiffOutpatientMedication = false;
            }

            if (previousVM.SugarFree != currentVM.SugarFree)
            {
                previousVM.IsDiffSugarFree = true;
                currentVM.IsDiffSugarFree = true;
            }
            else
            {
                previousVM.IsDiffSugarFree = false;
                currentVM.IsDiffSugarFree = false;
            }

            if (previousVM.GlutenFree != currentVM.GlutenFree)
            {
                previousVM.IsDiffGlutenFree = true;
                currentVM.IsDiffGlutenFree = true;
            }
            else
            {
                previousVM.IsDiffGlutenFree = false;
                currentVM.IsDiffGlutenFree = false;
            }

            if (previousVM.PreservativeFree != currentVM.PreservativeFree)
            {
                previousVM.IsDiffPreservativeFree = true;
                currentVM.IsDiffPreservativeFree = true;
            }
            else
            {
                previousVM.IsDiffPreservativeFree = false;
                currentVM.IsDiffPreservativeFree = false;
            }

            if (previousVM.CFCFree != currentVM.CFCFree)
            {
                previousVM.IsDiffCFCFree = true;
                currentVM.IsDiffCFCFree = true;
            }
            else
            {
                previousVM.IsDiffCFCFree = false;
                currentVM.IsDiffCFCFree = false;
            }

            if (previousVM.UnlicensedMedication != currentVM.UnlicensedMedication)
            {
                previousVM.IsDiffUnlicensedMedication = true;
                currentVM.IsDiffUnlicensedMedication = true;
            }
            else
            {
                previousVM.IsDiffUnlicensedMedication = false;
                currentVM.IsDiffUnlicensedMedication = false;
            }

            if (previousVM.ParallelImport != currentVM.ParallelImport)
            {
                previousVM.IsDiffParallelImport = true;
                currentVM.IsDiffParallelImport = true;
            }
            else
            {
                previousVM.IsDiffParallelImport = false;
                currentVM.IsDiffParallelImport = false;
            }

            if (previousVM.IgnoreDuplicateWarnings != currentVM.IgnoreDuplicateWarnings)
            {
                previousVM.IsDiffIgnoreDuplicateWarnings = true;
                currentVM.IsDiffIgnoreDuplicateWarnings = true;
            }
            else
            {
                previousVM.IsDiffIgnoreDuplicateWarnings = false;
                currentVM.IsDiffIgnoreDuplicateWarnings = false;
            }

            if (previousVM.IsCustomControlledDrug != currentVM.IsCustomControlledDrug)
            {
                previousVM.IsDiffControlledDrug = true;
                currentVM.IsDiffControlledDrug = true;
            }
            else
            {
                previousVM.IsDiffControlledDrug = false;
                currentVM.IsDiffControlledDrug = false;
            }

            if (previousVM.IsIndicationMandatory != currentVM.IsIndicationMandatory)
            {
                previousVM.IsDiffIndicationIsMandatory = true;
                currentVM.IsDiffIndicationIsMandatory = true;
            }
            else
            {
                previousVM.IsDiffIndicationIsMandatory = false;
                currentVM.IsDiffIndicationIsMandatory = false;
            }

            if (previousVM.WitnessingRequired != currentVM.WitnessingRequired)
            {
                previousVM.IsDiffWitnessingRequired = true;
                currentVM.IsDiffWitnessingRequired = true;
            }
            else
            {
                previousVM.IsDiffWitnessingRequired = false;
                currentVM.IsDiffWitnessingRequired = false;
            }

            if (previousOrCurrent.IsEmpty()) return null;

            string viewName = string.Empty;

            if (string.Compare(previousOrCurrent, "previous", true) == 0)
            {
                viewName = "_FormularyPreviousDetails";
                return PartialView(viewName, previousVM);
            }
                
            if (string.Compare(previousOrCurrent, "current", true) == 0)
            {
                viewName = "_FormularyCurrentDetails";
                return PartialView(viewName, currentVM);
            }

            return null;
        }

    }

}
