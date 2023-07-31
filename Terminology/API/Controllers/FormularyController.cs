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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.Commands;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary.Requests;
using Interneuron.Terminology.API.AppCode.Queries;
using Interneuron.Terminology.API.AppCode.Validators;
using Interneuron.Terminology.Infrastructure.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace Interneuron.Terminology.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    //[Route("api")]
    [ApiController]
    public partial class FormularyController : ControllerBase
    {
        private ILogger<FormularyController> _logger;
        private IServiceProvider _provider;
        private IFormularyQueries _formularyQueries;
        private IFormularyCommands _formularyCommand;
        private IDMDQueries _dmdQueries;
        private IDMDCommand _dmdCommand;

        public FormularyController(ILogger<FormularyController> logger, IServiceProvider provider, IFormularyQueries formularyQueries, IFormularyCommands formularyCommand, IDMDQueries dmdQueries, IDMDCommand dmdCommand)
        {
            _logger = logger;
            _provider = provider;
            _formularyQueries = formularyQueries;
            _formularyCommand = formularyCommand;
            _dmdQueries = dmdQueries;
            _dmdCommand = dmdCommand;

        }

        [HttpPost, Route("searchformularies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormularySearchResultsWithHierarchyDTO>> SearchFormularies([FromBody] FormularySearchFilterRequest filterCriteria)
        {
            if (filterCriteria == null) return BadRequest();

            if (filterCriteria.HideArchived == false && filterCriteria.SearchTerm.IsEmpty() && !filterCriteria.RecStatusCds.IsCollectionValid() && !filterCriteria.Flags.IsCollectionValid() && !filterCriteria.FormularyStatusCd.IsCollectionValid() && filterCriteria.ShowOnlyDuplicate == false) return BadRequest();

            return await this._formularyQueries.GetFormularyHierarchyForSearchRequest(filterCriteria);
        }

        [HttpPost, Route("searchformulariesaslist")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<FormularySearchResultDTO>> SearchFormulariesAsList([FromBody] FormularySearchFilterRequest filterCriteria)
        {
            return await this._formularyQueries.GetFormularyAsFlatList(filterCriteria);
        }

        [HttpGet, Route("getlatestformulariesheaderonly")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLatestFormulariesHeaderOnly()
        {
            var results = await new TaskFactory().StartNew(() => this._formularyQueries.GetLatestFormulariesBriefInfo());

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpGet, Route("getlatestformulariesheaderonlybynameorcode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLatestFormulariesHeaderOnlyByNameOrCode(string nameOrCode, string productType = null, bool isExactSearch = false)
        {
            if (nameOrCode.IsEmpty()) return BadRequest();

            var results = await new TaskFactory().StartNew(() => this._formularyQueries.GetLatestFormulariesBriefInfoByNameOrCode(nameOrCode, productType, isExactSearch));

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getlatesttoplevelformulariesbasicinfo")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetLatestTopLevelFormulariesBasicInfoByStatusCodes()
        {
            var results = await this._formularyQueries.GetLatestTopLevelFormulariesBasicInfo();

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpGet, Route("getformulariesasdiluents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormularySearchResultDTO>> GetFormulariesAsDiluents()
        {
            var results = await _formularyQueries.GetFormulariesAsDiluents();

            return Ok(results);
        }

        [HttpPost, Route("getdescendentformulariesforcodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularySearchResultDTO>>> GetFormularyDescendentForCodes(GetFormularyDescendentForCodesRequest request)
        {
            if (request == null || !request.Codes.IsCollectionValid()) return BadRequest();

            var results = await this._formularyQueries.GetFormularyImmediateDescendentForCodes(request.Codes, request.OnlyNonDeleted);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        //Deprecated
        //[HttpGet, Route("getformularydetail/{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<IActionResult> GetFormularyDetail(string id)
        //{
        //    if (id.IsEmpty()) return BadRequest();

        //    var result = await this._formularyQueries.GetFormularyDetail(id);

        //    if (result == null) return NoContent();

        //    return Ok(result);
        //}




        [HttpPut, Route("updatestatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateFormularyRecordStatusDTO>> UpdateFormularyRecordStatus([FromBody] UpdateFormularyRecordStatusRequest request)
        {
            if (request == null || !request.RequestData.IsCollectionValid())
                return BadRequest();

            var result = await new TaskFactory<UpdateFormularyRecordStatusDTO>().StartNew(() => _formularyCommand.UpdateFormularyRecordStatus(request));

            if (result == null || result.Status == null || result.Status.StatusCode == TerminologyConstants.STATUS_BAD_REQUEST)
                return BadRequest(result?.Status?.ErrorMessages);

            if (result.Status.StatusCode == TerminologyConstants.STATUS_FAIL) return StatusCode(500, result?.Status?.ErrorMessages);

            if (result.Status.ErrorMessages.IsCollectionValid())
                return StatusCode(207, result);
            return Ok(result);
        }

        [HttpPut, Route("bulkupdatestatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UpdateFormularyRecordStatusDTO>> BulkUpdateFormularyRecordStatus([FromBody] UpdateFormularyRecordStatusRequest request)
        {
            if (request == null || !request.RequestData.IsCollectionValid())
                return BadRequest();

            var result = await new TaskFactory<UpdateFormularyRecordStatusDTO>().StartNew(() => _formularyCommand.BulkUpdateFormularyRecordStatus(request));

            if (result == null || result.Status == null || result.Status.StatusCode == TerminologyConstants.STATUS_BAD_REQUEST)
                return BadRequest(result?.Status?.ErrorMessages);

            if (result.Status.StatusCode == TerminologyConstants.STATUS_FAIL) return StatusCode(500, result?.Status?.ErrorMessages);

            if (result.Status.ErrorMessages.IsCollectionValid())
                return StatusCode(207, result);

            return Ok(result);
        }

        [HttpPost, Route("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateEditFormularyDTO>> CreateFormulary([FromBody] CreateEditFormularyRequest request)
        {
            var validationResult = new CreateFormularyRequestValidator(request).Validate();

            if (!validationResult.IsValid)
                return BadRequest(validationResult.ValidationErrors);

            var result = await _formularyCommand.CreateFormulary(request);

            if (result == null || result.Status == null || result.Status.StatusCode == TerminologyConstants.STATUS_BAD_REQUEST)
                return BadRequest(result?.Status?.ErrorMessages);

            if (result.Status.StatusCode == TerminologyConstants.STATUS_FAIL) return StatusCode(500, result?.Status?.ErrorMessages);

            if (result.Status.ErrorMessages.IsCollectionValid())
                return StatusCode(207, result);


            return Ok(result);
        }



        [HttpPut, Route("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreateEditFormularyDTO>> UpdateFormulary([FromBody] CreateEditFormularyRequest request)
        {
            //To be changed for Edit functionality
            var validationResult = new EditFormularyRequestValidator(request).Validate();

            if (!validationResult.IsValid)
                return BadRequest(validationResult.ValidationErrors);

            var result = await new TaskFactory<CreateEditFormularyDTO>().StartNew(() => _formularyCommand.UpdateFormulary(request));

            if (result == null || result.Status == null || result.Status.StatusCode == TerminologyConstants.STATUS_BAD_REQUEST)
                return BadRequest(result?.Status?.ErrorMessages);

            if (result.Status.StatusCode == TerminologyConstants.STATUS_FAIL) return StatusCode(500, result?.Status?.ErrorMessages);

            if (result.Status.ErrorMessages.IsCollectionValid())
                return StatusCode(207, result);

            return Ok(result);
        }


        [HttpPost, Route("fileimport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequestSizeLimit(1073741824)]
        public async Task<ActionResult<CreateEditFormularyDTO>> FileImport([FromBody] CreateEditFormularyRequest request)
        {
            //No validation will be performed

            var result = await _formularyCommand.FileImport(request);

            if (result == null || result.Status == null || result.Status.StatusCode == TerminologyConstants.STATUS_BAD_REQUEST)
                return BadRequest(result?.Status?.ErrorMessages);

            if (result.Status.StatusCode == TerminologyConstants.STATUS_FAIL) return StatusCode(500, result?.Status?.ErrorMessages);

            if (result.Status.ErrorMessages.IsCollectionValid())
                return StatusCode(207, result);


            return Ok(result);
        }

        //[HttpPost, Route("searchdmdandformularies")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<FormularySearchResultsWithHierarchyDTO>> SearchDMDAndFormularies(string searchTerm, string formularyStatusCode = null)
        //{
        //    if (searchTerm.IsEmpty()) return BadRequest();

        //    FormularySearchFilterRequest formularySearchFilter = new FormularySearchFilterRequest();

        //    formularySearchFilter.FormularyStatusCd = formularyStatusCode.IsNotEmpty() ? new List<string> { formularyStatusCode } : null;
        //    formularySearchFilter.SearchTerm = searchTerm;

        //    var result = await this._formularyQueries.GetDMDAndFormularyHierarchy(formularySearchFilter);

        //    return Ok(result);
        //}

        [HttpGet, Route("getformularydetailrulebound/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormularyDTO>> GetFormularyDetailRuleBound(string id)
        {
            if (id.IsEmpty()) return BadRequest();

            var formularyDTO = await this._formularyQueries.GetFormularyDetailRuleBound(id);

            if (formularyDTO == null) return NotFound();

            return Ok(formularyDTO);
        }

        [HttpGet, Route("getformularydetailruleboundbycode/{code}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormularyDTO>> GetActiveFormularyDetailRuleBoundByCode(string code)
        {
            if (code.IsEmpty()) return BadRequest();

            var formularyDTO = await this._formularyQueries.GetActiveFormularyDetailRuleBoundByCode(code);

            if (formularyDTO == null) return NotFound();

            return Ok(formularyDTO);
        }

        [HttpGet, Route("GetActiveFormularyDetailRuleBoundByCodeArray")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormularyDTO[]>> GetActiveFormularyDetailRuleBoundByCodeArray([FromQuery]string[] code)
        {
            if (code == null || code.Length == 0) return BadRequest();

            var dmdCodes = code.Distinct().ToArray();

            var formularyDTOList = await this._formularyQueries.GetActiveFormularyDetailRuleBoundByCodeArray(dmdCodes);

            if (formularyDTOList == null || formularyDTOList.Length == 0) return NotFound();

            return Ok(formularyDTOList);
        }

        [HttpPost, Route("deriveproductnames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DeriveProductNamesDTO>> DeriveProductNames(DeriveProductNamesRequest request)
        {
            var validationResult = new DeriveProductNamesRequestValidator(request).Validate();

            if (!validationResult.IsValid)
                return BadRequest(validationResult.ValidationErrors);

            var result = await new TaskFactory<DeriveProductNamesDTO>().StartNew(() => _formularyQueries.DeriveProductNames(request.Ingredients, request.UnitDoseFormSize, request.FormulationName, request.SupplierName, request.ProductType));

            return Ok(result);
        }

        [HttpPost, Route("checkifproductexists")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status207MultiStatus)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CheckIfProductExistsDTO>> CheckIfProductExists(CheckIfProductExistsRequest request)
        {
            var validationResult = new CheckIfProductExistsRequestValidator(request).Validate();

            if (!validationResult.IsValid)
                return BadRequest(validationResult.ValidationErrors);

            var result = await new TaskFactory<CheckIfProductExistsDTO>().StartNew(() => _formularyQueries.CheckIfProductExists(request.Ingredients, request.UnitDoseFormSize, request.FormulationName, request.SupplierName, request.ProductType));

            return Ok(result);
        }

        [HttpGet, Route("gethistoryofformularies")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyHistoryDTO>>> GetHistoryOfFormularies()
        {
            var results = await this._formularyQueries.GetHistoryOfFormularies();

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getlocallicenseduse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLocalLicensedUseDTO>>> GetLocalLicensedUse(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetLocalLicensedUse(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getlocalunlicenseduse")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLocalUnlicensedUseDTO>>> GetLocalUnlicensedUse(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetLocalUnlicensedUse(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getlocallicensedroute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLocalLicensedRouteDTO>>> GetLocalLicensedRoute(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetLocalLicensedRoute(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getlocalunlicensedroute")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLocalUnlicensedRouteDTO>>> GetLocalUnlicensedRoute(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetLocalUnlicensedRoute(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getcustomwarning")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CustomWarningDTO>>> GetCustomWarning(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetCustomWarning(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getreminder")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ReminderDTO>>> GetReminder(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetReminder(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getendorsement")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<EndorsementDTO>>> GetEndorsement(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetEndorsement(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getmedusapreparationinstruction")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<MedusaPreparationInstructionDTO>>> GetMedusaPreparationInstruction(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetMedusaPreparationInstruction(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("gettitrationtype")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<TitrationTypeDTO>>> GetTitrationType(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetTitrationType(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getroundingfactor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<RoundingFactorDTO>>> GetRoundingFactor(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetRoundingFactor(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getcompatiblediluent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CompatibleDiluentDTO>>> GetCompatibleDiluent(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetCompatibleDiluent(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getclinicaltrialmedication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ClinicalTrialMedicationDTO>>> GetClinicalTrialMedication(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetClinicalTrialMedication(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getgastroresistant")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<GastroResistantDTO>>> GetGastroResistant(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetGastroResistant(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getcriticaldrug")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<CriticalDrugDTO>>> GetCriticalDrug(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetCriticalDrug(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getmodifiedrelease")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ModifiedReleaseDTO>>> GetModifiedRelease(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetModifiedRelease(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getexpensivemedication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ExpensiveMedicationDTO>>> GetExpensiveMedication(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetExpensiveMedication(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("gethighalertmedication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<HighAlertMedicationDTO>>> GetHighAlertMedication(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetHighAlertMedication(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getivtooral")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<IVToOralDTO>>> GetIVToOral(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetIVToOral(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getnotforprn")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<NotForPRNDTO>>> GetNotForPRN(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetNotForPRN(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getbloodproduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<BloodProductDTO>>> GetBloodProduct(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetBloodProduct(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getdiluent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<DiluentDTO>>> GetDiluent(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetDiluent(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getprescribable")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PrescribableDTO>>> GetPrescribable(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetPrescribable(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getoutpatientmedication")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<OutpatientMedicationDTO>>> GetOutpatientMedication(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetOutpatientMedication(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getignoreduplicatewarning")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<IgnoreDuplicateWarningDTO>>> GetIgnoreDuplicateWarning(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetIgnoreDuplicateWarning(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getcontrolleddrug")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ControlledDrugDTO>>> GetControlledDrug(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetControlledDrug(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getprescriptionprintingrequired")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PrescriptionPrintingRequiredDTO>>> GetPrescriptionPrintingRequired(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetPrescriptionPrintingRequired(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getindicationmandatory")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<IndicationMandatoryDTO>>> GetIndicationMandatory(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetIndicationMandatory(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getwitnessingrequired")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<WitnessingRequiredDTO>>> GetWitnessingRequired(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetWitnessingRequired(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }

        [HttpPost, Route("getformularystatus")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyStatusDTO>>> GetFormularyStatus(List<string> formularyVersionIds)
        {
            var results = await this._formularyQueries.GetFormularyStatus(formularyVersionIds);

            if (!results.IsCollectionValid()) return NoContent();

            return Ok(results);
        }
    }
}
