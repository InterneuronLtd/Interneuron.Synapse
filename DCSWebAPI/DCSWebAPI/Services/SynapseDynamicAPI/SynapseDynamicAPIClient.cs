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


﻿using DCSWebAPI.Models;
using DCSWebAPI.Models.WebAPI.SynapseDynamicAPI;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace DCSWebAPI.Services.SynapseDynamicAPI
{
    public class SynapseDynamicAPIClient : ISynapseDynamicAPIClient
    {
        private readonly HttpClient _client;

        public SynapseDynamicAPIClient(HttpClient client, IHttpContextAccessor httpContextAccessor)
        {
            var bearerToken = httpContextAccessor.HttpContext.Request
                             .Headers["Authorization"]
                             .FirstOrDefault(h => h.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase));

            // Add authorization if found
            if (bearerToken != null)
                client.DefaultRequestHeaders.Add("Authorization", bearerToken);

            _client = client;
        }

        public string ExecuteNonSQL(SQLData sqlData)
        {
            var response = _client.PostAsJsonAsync("sql/ExecuteNonSQL/", sqlData).Result;

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            return result;
        }

        public string ExecuteSQL(SQLData sqlData)
        {
            var response = _client.PostAsJsonAsync("sql/ExecuteSQL/", sqlData).Result;

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            return result;
        }

        public string PostObject(string entityNamespace, string entityName, object data)
        {
            var response = _client.PostAsJsonAsync("PostObject/" + entityNamespace + "/" + entityName, data).Result;

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            return result;
        }

        public string PostObjectArray(string entityNamespace, string entityName, object data)
        {
            var response = _client.PostAsJsonAsync("PostObjectArray/" + entityNamespace + "/" + entityName, data).Result;

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            return result;
        }

        public string DeleteObjectByAttribute(string synapseNamespace, string synapseEntityName, string synapseAttributeName, string attributeValue)
        {
            var response = _client.DeleteAsync(string.Format("DeleteObjectByAttribute/{0}/{1}/{2}/{3}", synapseNamespace, synapseEntityName, synapseAttributeName, attributeValue)).Result;

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            return result;
        }

        public string GetListByAttribute(string synapseNamespace, string synapseEntityName, string synapseAttributeName, string attributeValue, bool returnSystemAttributes)
        {
            var response = _client.GetAsync(string.Format("GetListByAttribute/{0}/{1}/{2}/{3}/{4}", synapseNamespace, synapseEntityName, synapseAttributeName, attributeValue, returnSystemAttributes)).Result;

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            return result;
        }
    }
}