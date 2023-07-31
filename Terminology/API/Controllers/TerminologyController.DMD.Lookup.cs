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
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.DMD;
using Interneuron.Terminology.API.AppCode.Queries;
using Interneuron.Terminology.Model.DomainModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interneuron.Terminology.API.Controllers
{
    public partial class TerminologyController : ControllerBase
    {
        [HttpGet, Route("getdmdroutelookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupRouteDTO>> GetRouteLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupRouteDTO>(LookupType.DMDRoute);
        }

        [HttpGet, Route("getdmdformlookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupFormDTO>> GetFormLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupFormDTO>(LookupType.DMDForm);
        }

        [HttpGet, Route("getdmdprescribingstatuslookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupPrescribingstatusDTO>> GetPrescribingStatusLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupPrescribingstatusDTO>(LookupType.DMDPrescribingStatus);
        }

        [HttpGet, Route("getdmdcontroldrugcategorylookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupControldrugcatDTO>> GetControlDrugCategoryLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupControldrugcatDTO>(LookupType.DMDControlDrugCategory);
        }

        [HttpGet, Route("getdmdsupplierlookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupSupplierDTO>> GetSupplierLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupSupplierDTO>(LookupType.DMDSupplier);
        }

        [HttpGet, Route("getdmdlicensingauthoritylookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupLicauthDTO>> GetLicensingAuthorityLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupLicauthDTO>(LookupType.DMDLicensingAuthority);
        }

        [HttpGet, Route("getdmduomlookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupUomDTO>> GetDMDUOMLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupUomDTO>(LookupType.DMDUOM);
        }

        [HttpGet, Route("getdmdontformroutelookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupOntformrouteDTO>> GetDMDOntFormRouteLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupOntformrouteDTO>(LookupType.DMDOntFormRoute);
        }

        [HttpGet, Route("getdmdbasisofnamelookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupBasisofnameDTO>> GetDMDBasisOfNameLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupBasisofnameDTO>(LookupType.DMDBasisOfName);
        }

        [HttpGet, Route("getdmdavailrestrictionslookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupAvailrestrictDTO>> GetDMDAvailRestrictionsLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupAvailrestrictDTO>(LookupType.DMDAvailRestrictions);
        }

        [HttpGet, Route("getdmddoseformlookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupDrugformindDTO>> GetDMDDoseFormLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupDrugformindDTO>(LookupType.DMDDoseForm);
        }

        [HttpGet, Route("getdmdpharamceuticalstrengthlookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupBasisofstrengthDTO>> GetDMDPharamceuticalStrengthLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupBasisofstrengthDTO>(LookupType.DMDPharamceuticalStrength);
        }

        [HttpGet, Route("getdmdingredientlookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupIngredientDTO>> GetDMDIngredientLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupIngredientDTO>(LookupType.DMDIngredient);
        }

        [HttpGet, Route("getatclookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<AtcLookupDTO>> GetATCLookup()
        {
            return await this._dmdQueries.GetLookup<AtcLookupDTO>(LookupType.ATCCode);
        }

        [HttpGet, Route("getbnflookup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<List<DmdLookupBNFDTO>> GetBNFLookup()
        {
            return await this._dmdQueries.GetLookup<DmdLookupBNFDTO>(LookupType.BNFCode);
        }
    }
}
