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


﻿

using Interneuron.Synapse;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Interneuron.SynapseDynamicAPIClient;
using Microsoft.Extensions.Configuration;

namespace InterneuronAutonomic.API
{
    public class DynamicAPIClient : SynapseDynamicAPIClient
    {
        private IConfiguration configuration;

        public DynamicAPIClient(IConfiguration configuration, HttpClient client, IHttpContextAccessor httpContextAccessor) : base(client)
        {
            this.configuration = configuration;

            var baseUrl = configuration.GetSection("DynamicAPISettings").GetValue<string>("uri");

            base._client.BaseAddress = new Uri(baseUrl);

            var bearerToken = httpContextAccessor.HttpContext.Request
                             .Headers["Authorization"]
                             .FirstOrDefault(h => h.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase));

            // Add authorization if found
            if (bearerToken != null)
                base._client.DefaultRequestHeaders.Add("Authorization", bearerToken);
        }
    }
}
