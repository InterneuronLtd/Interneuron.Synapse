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
using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interneuron.Terminology.API.Controllers
{
    public partial class TerminologyController : ControllerBase
    {
        [HttpGet, Route("searchdmd")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<DMDSearchResultsDTO> SearchDMD([FromQuery] string q)
        {
            return await this._dmdQueries.SearchDMD(System.Web.HttpUtility.UrlDecode(q));
        }

        
        [HttpGet, Route("searchdmdwithalldescendents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<DMDSearchResultsWithHierarchyDTO> SearchDMDNamesAndGetWithChildren([FromQuery] string q)
        {
            return await this._dmdQueries.SearchDMDNamesGetWithAllDescendents(System.Web.HttpUtility.UrlDecode(q));
        }

        [HttpGet, Route("searchdmdsyncLog")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<DMDSearchResultsWithHierarchyDTO> SearchDMDSyncLog([FromQuery] string q)
        {
            return await this._dmdQueries.SearchDMDSyncLog(System.Web.HttpUtility.UrlDecode(q));
        }

        [HttpGet, Route("searchdmdwithallnodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<DMDSearchResultsWithHierarchyDTO>> SearchDMDNamesGetWithAllLevelNodes([FromQuery] string q)
        {
            if (q.IsEmpty()) return BadRequest();

            var results = await this._dmdQueries.SearchDMDNamesGetWithAllLevelNodes(System.Web.HttpUtility.UrlDecode(q));
            return Ok(results);
        }

        [HttpPost, Route("getdmddescendent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DMDSearchResultWithTreeDTO>> GetDMDDescendentForCodes([FromBody] string[] codes)
        {
            return await this._dmdQueries.GetDMDDescendentForCodes(codes);
        }

        [HttpPost, Route("getdmdancestor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DMDSearchResultWithTreeDTO>> GetDMDAncestorForCodes([FromBody] string[] codes)
        {
            return await this._dmdQueries.GetDMDAncestorForCodes(codes);
        }

        [HttpGet, Route("searchdmdwithtopnodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<DMDSearchResultsWithHierarchyDTO> SearchDMDNamesWithTopNodes([FromQuery] string q)
        {
            return await this._dmdQueries.SearchDMDNamesGetWithTopNodes(System.Web.HttpUtility.UrlDecode(q));
        }


        [HttpGet, Route("getalldmdcodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetAllDMDCodes()
        {
            return await new TaskFactory<ActionResult>().StartNew(() =>
            {
                var results = this._dmdQueries.GetAllDMDCodes();

                if (!results.IsCollectionValid()) return NoContent();

                return Ok(results);
            });
        }

        [HttpGet, Route("getdmdsnomedversion")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetDmdSnomedVersion()
        {
            return await new TaskFactory<ActionResult>().StartNew(() =>
            {
                var results = this._dmdQueries.GetDmdSnomedVersion();

                if (results.IsNull()) return NoContent();

                return Ok(results);
            });
        }

    }
}
