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
using Interneuron.Terminology.API.AppCode.Commands;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Config;
using Interneuron.Terminology.API.AppCode.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Interneuron.Terminology.API.Controllers.Utility
{
    [Route("api/util/[controller]")]
    [ApiController]
    public class ConfigurationController : ControllerBase
    {
        private IConfigurationQueries _configurationQueries;
        private IConfigurationCommands _configurationCommands;

        public ConfigurationController(IConfigurationQueries configurationQueries, IConfigurationCommands configurationCommands)
        {
            _configurationQueries = configurationQueries;
            _configurationCommands = configurationCommands;
        }

        [HttpGet, Route("getconfiguration/{configName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormularyRuleConfigDTO>> GetConfigurationByName(string configName)
        {
            if (configName.IsEmpty()) { return BadRequest(); }

            var dtoTask = new TaskFactory<FormularyRuleConfigDTO>().StartNew(() =>
       {
           return _configurationQueries.GetConfigurationByName(configName);
       });

            return await dtoTask;
        }

        [HttpPost, Route("saveconfiguration")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status410Gone)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<FormularyRuleConfigDTO>> SaveConfiguration(FormularyRuleConfigRequest request)
        {
            if (request == null || request.Name.IsEmpty() || request.ConfigJson.IsEmpty())
            {
                return BadRequest();
            }
            return await _configurationCommands.SaveConfiguration(request);
        }
    }
}
