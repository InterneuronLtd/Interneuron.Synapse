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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.DataService.APIModel;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.DataService.APIModel.Requests;
using SynapseStudioWeb.Models.MedicinalMgmt;
using SynapseStudioWeb.AppCode.Constants;
using SynapseStudioWeb.Models.MedicationMgmt;

namespace SynapseStudioWeb.Controllers
{
    public partial class FormularyController : Controller
    {
        private const string UPDATE_MEDICATION_SUCCESS_MSG = "Successfully updated Medication";

        private async Task FillLookupsToViewBag(string? status = "")
        {
            string token = HttpContext.Session.GetString("access_token");

            //ViewBag.MedicationTypeList = (await _formularyLookupService.GetMedicationTypeLookup()).ToSelectList(DefaultDropDownText);
            ViewBag.BasisOfPreferredNameList = (await _formularyLookupService.GetBasisOfPreferredName()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            ViewBag.LicensingAuthorities = (await _formularyLookupService.GetLicensingAuthority()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            ViewBag.DoseForms = (await _formularyLookupService.GetDoseForms()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            ViewBag.RoundingFactors = (await _formularyLookupService.GetRoundingFactor()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            ViewBag.ControlledDrugCategories = (await _formularyLookupService.GetControlledDrugCategories()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            //ViewBag.MarkedModifiers = (await _formularyLookupService.GetMarkedModifiers()).ToSelectList(DefaultDropDownText);
            //ViewBag.ModifiedReleases = (await _formularyLookupService.GetModifiedReleases()).ToSelectList(DefaultDropDownText);
            //ViewBag.OrderableStatuses = (await _formularyLookupService.GetOrderableStatuses()).ToSelectList(DefaultDropDownText);
            ViewBag.PrescribingStatuses = (await _formularyLookupService.GetPrescribingStatuses()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            ViewBag.RestrictionsOnAvailabilities = (await _formularyLookupService.GetRestrictionsOnAvailability()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            ViewBag.TitrationTypes = (await _formularyLookupService.GetTitrationType()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            //ViewBag.DrugClasses = (await _formularyLookupService.GetDrugClassLookup()).ToSelectList(DefaultDropDownText);
            ViewBag.BasisOfPharmaStrengths = (await _formularyLookupService.GetBasisOfPharmaStrengths()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            ViewBag.ClassificationCodeTypes = (await _formularyLookupService.GetClassificationCodeTypes()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);

            var identificationTypes = (await _formularyLookupService.GetIdentificationCodeTypes());

            if (identificationTypes.IsCollectionValid())
            {
                ViewBag.IndentificationCodeTypes = identificationTypes?.ToDictionary(k => k.Key, v => v.Value).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            }

            ViewBag.RecordStatuses = await GetAssignableStatuses(status);

            //ViewBag.RecordStatuses = (await _formularyLookupService.GetRecordStatusLookup()).ToSelectList(DefaultDropDownText);
            ViewBag.FormularyStatuses = (await _formularyLookupService.GetFormularyStatuses()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            ViewBag.ProductTypes = (await _formularyLookupService.GetProductTypes()).ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT);
            //ViewBag.ClassificationCodeTypes = ToSelectList(codeSystems, "Cd", "Desc");
            //ViewBag.OrderFormTypes = (await _formularyLookupService.GetOrderFormTypes()).ToSelectList(DefaultDropDownText);
        }


        private bool GetByCondition(KeyValuePair<string, string> dictItem, string status)
        {
            return status switch
            {
                "001" => dictItem.Key == "001" || dictItem.Key == "002" || dictItem.Key == "004" || dictItem.Key == "006",
                "002" => dictItem.Key == "002" || dictItem.Key == "003" || dictItem.Key == "004" || dictItem.Key == "006",
                "003" => dictItem.Key == "002" || dictItem.Key == "003" || dictItem.Key == "004" || dictItem.Key == "006",
                "004" => dictItem.Key == "002" || dictItem.Key == "003" || dictItem.Key == "004" || dictItem.Key == "006",
                _ => dictItem.Key == "001"
            };
        }

        private async Task<SelectList> GetAssignableStatuses(string status, bool ignorePresentStatus = false)
        {
            var recordStatuses = await _formularyLookupService.GetRecordStatusLookup();//Let it throw the error if not exist
            var recordStatusesAsSelectedList = recordStatuses.Where(rec => GetByCondition(rec, status))?.ToDictionary(k => k.Key, v => v.Value)
                        .ToSelectList(TerminologyConstants.DEFAULT_DROPDOWN_TEXT, status, ignorePresentStatus);
            return recordStatusesAsSelectedList;
        }

        [HttpGet]
        [Route("Formulary/GetNextLevelStatuses")]
        public async Task<IActionResult> GetNextLevelStatuses(string status)
        {
            var recordStatuses = await GetAssignableStatuses(status, true);

            return Json(recordStatuses);
        }

        [HttpGet]
        [Route("Formulary/GetCustomMedContainerPartial")]
        public IActionResult GetCustomMedContainerPartial()
        {
            return PartialView("_FormularyCreateMedContainer");
        }

        [HttpPost]
        [Route("Formulary/DeriveProductName")]
        public async Task<IActionResult> DeriveProductName(List<FormularyIngredientModel> ingredients, string unitDoseSizeVal, string formulationName, string supplierName)
        {
            string token = HttpContext.Session.GetString("access_token");

            var apiRequest = new DeriveProductNamesRequest
            {
                Ingredients = ingredients.IsCollectionValid() ? _mapper.Map<List<FormularyIngredientAPIModel>>(ingredients) : null,
                FormulationName = formulationName,
                ProductType = "all",
                UnitDoseFormSize = unitDoseSizeVal,
                SupplierName = supplierName
            };

            var response = await TerminologyAPIService.DeriveProductName(apiRequest, token);
            if (response == null || response.StatusCode == DataService.APIModel.StatusCode.Fail || response.Data == null)
            {
                const string ErrorResponse = "Error getting details for the selected record";
                var errors = "";
                if (response.ErrorMessages.IsCollectionValid())
                    errors = string.Join('\n', ErrorResponse, response.ErrorMessages.ToArray());
                _toastNotification.AddErrorToastMessage(errors);
                return Json(null);
            }
            if (response.Data == null || !response.Data.ProductNameByType.IsCollectionValid()) return Json(null);

            return Json(response.Data.ProductNameByType);
        }

        [HttpPost]
        [Route("Formulary/CheckIfProductExists")]
        public async Task<IActionResult> CheckIfProductExists(List<FormularyIngredientModel> ingredients, string unitDoseSizeVal, string formulationName, string supplierName)
        {
            string token = HttpContext.Session.GetString("access_token");

            var apiRequest = new CheckIfProductExistsRequest
            {
                Ingredients = ingredients.IsCollectionValid() ? _mapper.Map<List<FormularyIngredientAPIModel>>(ingredients) : null,
                FormulationName = formulationName,
                ProductType = "amp",
                UnitDoseFormSize = unitDoseSizeVal,
                SupplierName = supplierName
            };

            var response = await TerminologyAPIService.CheckIfProductExists(apiRequest, token);
            if (response == null || response.StatusCode == DataService.APIModel.StatusCode.Fail || response.Data == null)
            {
                const string ErrorResponse = "Error getting details for the selected record";
                var errors = "";
                if (response.ErrorMessages.IsCollectionValid())
                    errors = string.Join('\n', ErrorResponse, response.ErrorMessages.ToArray());
                _toastNotification.AddErrorToastMessage(errors);
                return Json(null);
            }
            if (response.Data == null) return Json(null);

            return Json(response.Data);
        }

        [HttpGet]
        [Route("Formulary/GetCustomMedTemplatePartial")]
        public async Task<IActionResult> GetCustomMedTemplatePartial(string formularyVersionId)
        {
            await FillLookupsToViewBag("");

            if (formularyVersionId.IsNotEmpty())
            {
                string token = HttpContext.Session.GetString("access_token");

                var response = await TerminologyAPIService.GetFormularyDetailRuleBound(formularyVersionId, token);

                if (response == null || response.StatusCode == DataService.APIModel.StatusCode.Fail || response.Data == null)
                {
                    const string ErrorResponse = "Error getting details for the selected record";

                    var errors = "";

                    if (response.ErrorMessages.IsCollectionValid())
                    {
                        errors = string.Join('\n', ErrorResponse, response.ErrorMessages.ToArray());
                    }
                    _toastNotification.AddErrorToastMessage(errors);

                    var vm = new FormularyCreateModel
                    {
                        Code = Guid.NewGuid().ToString(),
                        ControlIdentifier = "Create_New_Custom_Medication"
                    };
                    return PartialView("_FormularyCustomCreate", vm);
                }

                var ampVM = MutateVMToCreateCustomMed(response.Data);
                return PartialView("_FormularyCustomCreate", ampVM);
            }
            else
            {
                var vm = new FormularyCreateModel
                {
                    Code = Guid.NewGuid().ToString(),
                    ControlIdentifier = "Create_New_Custom_Medication"
                };
                return PartialView("_FormularyCustomCreate", vm);
            }
        }

        private FormularyCreateModel MutateVMToCreateCustomMed(FormularyHeaderAPIModel data)
        {
            var ampVM = _mapper.Map<FormularyCreateModel>(data);
            ampVM.OriginalStatus = ampVM.Status;
            ampVM.CodeSystem = TerminologyConstants.CUSTOM_IDENTIFICATION_CODE_TYPE;
            ampVM.ControlIdentifier = "Create_New_Custom_Medication";
            ampVM.Code = Guid.NewGuid().ToString();
            return ampVM;
        }

        

        [HttpPost]
        [Route("Formulary/CreateFormulary")]
        public async Task<IActionResult> CreateFormulary(FormularyCreateModel createdData)
        {
            string token = HttpContext.Session.GetString("access_token");

            if (!ModelState.IsValid)
            {
                await FillLookupsToViewBag("");

                createdData.IsSaveSuccessful = false;
                return PartialView("_FormularyCustomCreate", createdData);
            }

            var apiModel = _mapper.Map<FormularyHeaderAPIModel>(createdData);

            var apiModels = new List<FormularyHeaderAPIModel>() { apiModel };

            TerminologyAPIResponse<CreateFormularyAPIResponse> response = await TerminologyAPIService.CreateCustomMedication(apiModels, token);

            if (response.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                _toastNotification.AddErrorToastMessage(string.Join('\n', response.ErrorMessages));

                await FillLookupsToViewBag("");

                createdData.IsSaveSuccessful = false;

                return PartialView("_FormularyCustomCreate", createdData);
            }

            if (response.StatusCode == DataService.APIModel.StatusCode.Success)
            {
                if (response.Data.Status != null && response.Data.Status.ErrorMessages.IsCollectionValid())
                {
                    var errors = string.Join('\n', response.Data.Status.ErrorMessages);

                    _toastNotification.AddInfoToastMessage(errors);

                    await FillLookupsToViewBag("");

                    createdData.IsSaveSuccessful = true;

                    return PartialView("_FormularyCustomCreate", createdData);
                }
            }

            _toastNotification.AddSuccessToastMessage("Successfully created formulary details");

            createdData.IsSaveSuccessful = true;

            return Json(null);
        }
    }


}
