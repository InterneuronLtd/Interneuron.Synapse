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
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.Infrastructure.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.Controllers
{
    public partial class FormularyController: ControllerBase
    {
        [HttpPost, Route("import")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ImportFormularyResultsDTO>> ImportDMD([FromBody] List<string> dmdCodes, string formularyStatusCd = TerminologyConstants.FORMULARYSTATUS_NONFORMULARY, string recordStatusCd = TerminologyConstants.RECORDSTATUS_DRAFT)
        {
            if (!dmdCodes.IsCollectionValid() || dmdCodes.Any(d => d.IsEmpty()))
                return BadRequest();

            var result = await this._formularyCommand.ImportByCodes(dmdCodes, formularyStatusCd, recordStatusCd);

            if (result == null || result.Status == null || result.Status.StatusCode == TerminologyConstants.STATUS_BAD_REQUEST)
                return BadRequest(result?.Status?.ErrorMessages);

            if (result.Status.StatusCode == TerminologyConstants.STATUS_FAIL) return StatusCode(500, result?.Status?.ErrorMessages);

            //Update the sync log - that formulary has been synced for these codes
            _dmdCommand.UpdateFormularySyncStatus(dmdCodes);

            return Ok(result);
        }

        [HttpPost, Route("importalldmdcodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ImportFormularyResultsDTO>> BulkImportDMD()
        {
            var result = await this._formularyCommand.ImportAllDMDCodes();

            if (result == null || result.StatusCode == TerminologyConstants.STATUS_BAD_REQUEST)
                return BadRequest(result?.ErrorMessages);

            if (result.StatusCode == TerminologyConstants.STATUS_FAIL) return StatusCode(500, result?.ErrorMessages);

            //Update the sync log - that formulary has been synced for these codes
            //_dmdCommand.UpdateFormularySyncStatusForAllRecords();

            return Ok(result);
        }

        [HttpPost, Route("invokepostimportprocess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> InvokePostImportProcess(List<string> codes)
        {
            if (codes.IsCollectionValid())
                await this._formularyCommand.InvokePostImportProcessForCodes(codes);
            else
                await this._formularyCommand.InvokePostImportProcess();

            return Ok();
        }

        [HttpPost, Route("importdeltas")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> SyncAllPendingDMD()
        {
            //Let it act a facade for now - Can be moved to mediator later
            var pendingLogs = _dmdQueries.GetDMDPendingSyncLogs();

            if (!pendingLogs.IsCollectionValid()) return Ok();

            var codes = pendingLogs.Select(rec => rec.DmdId).Distinct().ToList();

            if (!codes.IsCollectionValid()) return Ok();

            var importResponse = await this._formularyCommand.ImportByCodes(codes);

            if (importResponse == null || importResponse.Status == null || importResponse.Status.StatusCode == TerminologyConstants.STATUS_BAD_REQUEST)
                return BadRequest(importResponse?.Status?.ErrorMessages);

            if (importResponse.Status.StatusCode == TerminologyConstants.STATUS_FAIL) return StatusCode(500, importResponse?.Status?.ErrorMessages);

            await _formularyCommand.InvokePostImportProcessForCodes(codes);

            //Update the sync log - that formulary has been synced for these codes
            _dmdCommand.UpdateFormularySyncStatus(codes);

            return Ok();
        }
    }
}
