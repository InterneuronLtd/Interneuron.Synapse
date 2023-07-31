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
using Interneuron.Caching;
using Interneuron.Terminology.API.AppCode.Infrastructure.Caching;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interneuron.Terminology.API.Controllers.Utility
{
    //[Authorize]
    [Route("api/util/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        [HttpGet, Route("allcachekeys")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public List<string> GetAllCacheKeys()
        {
            return CacheKeys.GetAllCacheKeys();
        }

        [HttpGet, Route("clearcache")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public bool ClearCache()
        {
            CacheService.FlushAll();
            return true;
        }

        [HttpGet, Route("clearcache/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public bool ClearCache(string key)
        {
            CacheService.Remove(key);
            return true;
        }

        [HttpGet, Route("reloadcache")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public bool ReloadCache()
        {
            CacheService.FlushAll();
            return true;
        }

        [HttpGet, Route("reloadcache/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public bool ReloadCache(string key)
        {
            CacheService.Remove(key);
            return true;
        }
    }
}
