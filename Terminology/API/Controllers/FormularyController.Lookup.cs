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
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.API.AppCode.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Interneuron.Terminology.API.Controllers
{
    //[Authorize]
    public partial class FormularyController : ControllerBase
    {
        //[HttpGet, Route("getmedicationtypelookup")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetMedicationTypeLookup()
        //{
        //    //var response = new TerminologyResponse(System.Net.HttpStatusCode.NotFound);
        //    //var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.FormularyMedicationType);
        //    //var dataFromDb = data.Where(d => d.Type == LookupType.FormularyMedicationType.GetTypeName()).ToList();
        //    //if (dataFromDb.IsCollectionValid()) { response.Resource = new FormularyLookupDTO() { Items = dataFromDb }; return response; };
        //    //return response;

        //    var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.FormularyMedicationType);
        //    //return data.Where(d => d.Type == LookupType.FormularyMedicationType.GetTypeName()).ToList();

        //    var lookupData = data.Where(d => d.Type == LookupType.FormularyMedicationType.GetTypeName()).ToList();
        //    if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
        //    return NoContent();
        //}

        //[HttpGet, Route("getruleslookup")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetRulesLookup()
        //{
        //    //var response = new TerminologyResponse(System.Net.HttpStatusCode.NotFound);
        //    //var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.FormularyRules);
        //    //var dataFromDb = data.Where(d => d.Type == LookupType.FormularyRules.GetTypeName()).ToList();
        //    //if (dataFromDb.IsCollectionValid()) { response.Resource = new FormularyLookupDTO() { Items = dataFromDb }; return response; };
        //    //return response;


        //    var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.FormularyRules);
        //    //return data.Where(d => d.Type == LookupType.FormularyRules.GetTypeName()).ToList();
        //    var lookupData = data.Where(d => d.Type == LookupType.FormularyRules.GetTypeName()).ToList();
        //    if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
        //    return NoContent();
        //}

        [HttpGet, Route("getrecordstatuslookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetRecordStatusLookup()
        {
            var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.RecordStatus);
            var lookupData = data.Where(d => d.Type == LookupType.RecordStatus.GetTypeName()).ToList();
            if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
            return NoContent();
        }

        [HttpGet, Route("getformularystatuslookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetFormularyStatusLookup()
        {
            var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.FormularyStatus);
            var lookupData = data.Where(d => d.Type == LookupType.FormularyStatus.GetTypeName()).ToList();
            if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
            return NoContent();
        }

        //[HttpGet, Route("getorderablestatuslookup")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetOrderableStatusLookup()
        //{
        //    //var response = new TerminologyResponse(System.Net.HttpStatusCode.NotFound);
        //    //var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.OrderableStatus);
        //    //var dataFromDb = data.Where(d => d.Type == LookupType.OrderableStatus.GetTypeName()).ToList();
        //    //if (dataFromDb.IsCollectionValid()) { response.Resource = new FormularyLookupDTO() { Items = dataFromDb }; return response; };
        //    //return response;


        //    var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.OrderableStatus);
        //    //return data.Where(d => d.Type == LookupType.OrderableStatus.GetTypeName()).ToList();
        //    var lookupData = data.Where(d => d.Type == LookupType.OrderableStatus.GetTypeName()).ToList();
        //    if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
        //    return NoContent();
        //}

        [HttpGet, Route("gettitrationtypelookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetTitrationTypeLookup()
        {
            //var response = new TerminologyResponse(System.Net.HttpStatusCode.NotFound);
            //var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.TitrationType);
            //var dataFromDb = data.Where(d => d.Type == LookupType.TitrationType.GetTypeName()).ToList();
            //if (dataFromDb.IsCollectionValid()) { response.Resource = new FormularyLookupDTO() { Items = dataFromDb }; return response; };
            //return response;

            var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.TitrationType);
            var lookupData = data.Where(d => d.Type == LookupType.TitrationType.GetTypeName()).ToList();
            if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
            return NoContent();
        }

        [HttpGet, Route("getroundingfactorlookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetRoundingFactorLookup()
        {
            //var response = new TerminologyResponse(System.Net.HttpStatusCode.NotFound);
            //var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.RoundingFactor);
            //var dataFromDb = data.Where(d => d.Type == LookupType.RoundingFactor.GetTypeName()).ToList();
            //if (dataFromDb.IsCollectionValid()) { response.Resource = new FormularyLookupDTO() { Items = dataFromDb }; return response; };
            //return response;

            var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.RoundingFactor);
            //return data.Resource.Where(d => d.Type == LookupType.RoundingFactor.GetTypeName()).ToList();
            var lookupData = data.Where(d => d.Type == LookupType.RoundingFactor.GetTypeName()).ToList();
            if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
            return NoContent();
        }

        //[HttpGet, Route("getmodifierlookup")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetModifierLookup()
        //{
        //    //var response = new TerminologyResponse(System.Net.HttpStatusCode.NotFound);
        //    //var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.Modifier);
        //    //var dataFromDb = data.Where(d => d.Type == LookupType.Modifier.GetTypeName()).ToList();
        //    //if (dataFromDb.IsCollectionValid()) { response.Resource = new FormularyLookupDTO() { Items = dataFromDb }; return response; };
        //    //return response;

        //    var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.Modifier);
        //    var lookupData = data.Where(d => d.Type == LookupType.Modifier.GetTypeName()).ToList();
        //    if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
        //    return NoContent();
        //}


        //[HttpGet, Route("getmodifiedreleaselookup")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetModifiedReleaseLookup()
        //{
        //    //var response = new TerminologyResponse(System.Net.HttpStatusCode.NotFound);
        //    //var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.ModifiedRelease);
        //    //var dataFromDb = data.Where(d => d.Type == LookupType.ModifiedRelease.GetTypeName()).ToList();
        //    //if (dataFromDb.IsCollectionValid()) { response.Resource = new FormularyLookupDTO() { Items = dataFromDb }; return response; };
        //    //return response;

        //    var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.ModifiedRelease);
        //    //return data.Where(d => d.Type == LookupType.ModifiedRelease.GetTypeName()).ToList();
        //    var lookupData = data.Where(d => d.Type == LookupType.ModifiedRelease.GetTypeName()).ToList();
        //    if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
        //    return NoContent();
        //}

        [HttpGet, Route("getproducttypelookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetProductTypeLookup()
        {
            var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.ProductType);

            var lookupData = data.Where(d => d.Type == LookupType.ProductType.GetTypeName()).ToList();
            if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
            return NoContent();
        }

        [HttpGet, Route("getclassificationcodetypelookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetClassificationCodeTypeLookup()
        {
            var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.ClassificationCodeType);

            var lookupData = data.Where(d => d.Type == LookupType.ClassificationCodeType.GetTypeName()).ToList();
            if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
            return NoContent();
        }

        [HttpGet, Route("getidentificationcodetypelookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetIdentificationCodeTypeLookup()
        {
            var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.IdentificationCodeType);

            var lookupData = data.Where(d => d.Type == LookupType.IdentificationCodeType.GetTypeName()).ToList();
            if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
            return NoContent();
        }


        //[HttpGet, Route("getorderformtypelookup")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetOrdeFormTypeLookup()
        //{

        //    var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.OrderFormType);

        //    var lookupData = data.Where(d => d.Type == LookupType.OrderFormType.GetTypeName()).ToList();
        //    if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
        //    return NoContent();
        //}

        [HttpGet, Route("getroutefieldtypelookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetRouteFieldTypeLookup()
        {
            var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.RouteFieldType);

            var lookupData = data.Where(d => d.Type == LookupType.RouteFieldType.GetTypeName()).ToList();
            if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
            return NoContent();
        }

        //[HttpGet, Route("getdrugclasslookup")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status410Gone)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<ActionResult<List<FormularyLookupItemDTO>>> GetDrugClassLookup()
        //{
        //    var data = await this._formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.DrugClass);

        //    var lookupData = data.Where(d => d.Type == LookupType.DrugClass.GetTypeName()).ToList();
        //    if (lookupData.IsCollectionValid()) return Ok(lookupData.Where(d => d.Recordstatus == 1));
        //    return NoContent();
        //}
    }
}
