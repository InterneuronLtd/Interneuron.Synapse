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
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.SNOMED;
using Interneuron.Terminology.API.AppCode.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Interneuron.Terminology.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    //[Route("api")]
    [ApiController]
    public partial class TerminologyController : ControllerBase
    {
        private ILogger<TerminologyController> _logger;
        private IServiceProvider _provider;
        private ISnomedCTQueries _snomedCTQueries;
        private IDMDQueries _dmdQueries;

        public TerminologyController(ILogger<TerminologyController> logger, IServiceProvider provider, ISnomedCTQueries snomedCTQueries, IDMDQueries dmdQueries)
        {
            this._logger = logger;
            this._provider = provider;
            this._snomedCTQueries = snomedCTQueries;
            this._dmdQueries = dmdQueries;
        }



        [HttpGet, Route("snomedsemantics")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<SemanticTagDTO>> GetSNOMEDSemanticTags()
        {
            return await new TaskFactory<List<SemanticTagDTO>>().StartNew(() =>
            {
                return this._snomedCTQueries.GetSemanticTags();
            });
        }

        [HttpGet, Route("searchsnomed/{q}/{semanticTag}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<SnomedSearchResultsDTO> SearchSnomedPrefTermBySemanticTag(string q, string semanticTag)
        {
            return await new TaskFactory<SnomedSearchResultsDTO>().StartNew(() =>
            {
                return this._snomedCTQueries.SearchSnomedTermBySemanticTag(System.Web.HttpUtility.UrlDecode(q), System.Web.HttpUtility.UrlDecode(semanticTag));
            });
        }

        [HttpGet, Route("searchsnomed")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<SnomedSearchResultsDTO> SearchSnomedPrefTermBySemanticTagByQuery([FromQuery] string q, [FromQuery] string semanticTag)
        {
            return await new TaskFactory<SnomedSearchResultsDTO>().StartNew(() =>
            {
                return this._snomedCTQueries.SearchSnomedTermBySemanticTag(System.Web.HttpUtility.UrlDecode(q), System.Web.HttpUtility.UrlDecode(semanticTag));
            });
        }

        //[HttpGet, Route("searchsnomedwithalldescendents/{q}/{semanticTag}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedPrefTermBySemanticTagWithChildren(string q, string semanticTag)
        //{
        //    return await this._snomedCTQueries.SearchSnomedTermsGetWithAllDescendents(System.Web.HttpUtility.UrlDecode(q), System.Web.HttpUtility.UrlDecode(semanticTag));
        //}

        [HttpGet, Route("searchsnomedwithalldescendents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedPrefTermBySemanticTagWithChildren([FromQuery] string q, [FromQuery] string semanticTag)
        {
            return await this._snomedCTQueries.SearchSnomedTermsGetWithAllDescendents(System.Web.HttpUtility.UrlDecode(q), System.Web.HttpUtility.UrlDecode(semanticTag));
        }

        [HttpPost, Route("getsnomeddescendent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<SnomedSearchResultWithTreeDTO>> GetSnomedDescendentForConceptIds(string[] conceptIds)
        {
            return await this._snomedCTQueries.GetSnomedDescendentForConceptIds(conceptIds);
        }

        [HttpPost, Route("getsnomedancestor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<SnomedSearchResultWithTreeDTO>> GetSnomedAncestorForConceptIds(string[] conceptIds)
        {
            return await this._snomedCTQueries.GetSnomedAncestorForConceptIds(conceptIds);
        }

        //[HttpGet, Route("searchsnomedwithallparentnodes/{q}/{semanticTag}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedPrefTermBySemanticTagWithAncestors(string q, string semanticTag)
        //{
        //    return await this._snomedCTQueries.SearchSnomedTermsGetWithAllAncestors(System.Web.HttpUtility.UrlDecode(q), System.Web.HttpUtility.UrlDecode(semanticTag));
        //}

        [HttpGet, Route("searchsnomedwithallparentnodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedPrefTermBySemanticTagWithAncestors([FromQuery] string q, [FromQuery] string semanticTag)
        {
            return await this._snomedCTQueries.SearchSnomedTermsGetWithAllAncestors(System.Web.HttpUtility.UrlDecode(q), System.Web.HttpUtility.UrlDecode(semanticTag));
        }

        //[HttpGet, Route("searchsnomedwithallnodes/{q}/{semanticTag}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedPrefTermBySemanticTagWithAllNodes(string q, string semanticTag)
        //{
        //    return await this._snomedCTQueries.SearchSnomedTermsGetWithAllNodes(System.Web.HttpUtility.UrlDecode(q), System.Web.HttpUtility.UrlDecode(semanticTag));
        //}

        [HttpGet, Route("searchsnomedwithallnodes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedPrefTermBySemanticTagWithAllNodes([FromQuery] string q, [FromQuery] string semanticTag)
        {
            return await this._snomedCTQueries.SearchSnomedTermsGetWithAllNodes(System.Web.HttpUtility.UrlDecode(q), System.Web.HttpUtility.UrlDecode(semanticTag));
        }

        //[HttpGet, Route("{type}/{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<string> GetChildren(string snomedCode, string? semanticTag)
        //{
        //    return await new Task<string>(() => "Read Terminology");
        //}


        //In-efficient - Can use the search instead with 'disorder'semantic tag
        //[HttpGet, Route("getindicationlookup")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<List<SnomedIndicationLookupDTO>>> GetIndicationsLookup()
        //{
        //    var data = await new TaskFactory<List<SnomedIndicationLookupDTO>>().StartNew(() =>
        //    {
        //        var lookupData = this._snomedCTQueries.GetIndicationsLookup();

        //        return lookupData;
        //    });

        //    if (data.IsCollectionValid()) return Ok(data);

        //    return NoContent();

        //}


    }
}