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


﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SynapsePlatformServices.Models.STS;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SynapsePlatformServices.Services
{
    public class SynapseIdentityServerClient : ISynapseIdentityServerClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SynapseIdentityServerClient(HttpClient client, IConfiguration configuration)
        { 
            _httpClient = client;
            _configuration = configuration;
        }

        public async Task<IIdentityServerResponse> GetAccessToken(TokenParameter parameter)
        {
            string identityServerEndpoint = _configuration["Settings:IdentityServerUri"];
            var nameValueCollection = GetNameValueCollection(parameter);

            using (var content = new FormUrlEncodedContent(nameValueCollection))
            {
                content.Headers.Clear();
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                HttpResponseMessage response = await _httpClient.PostAsync(identityServerEndpoint, content);

                var responseData = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return System.Text.Json.JsonSerializer.Deserialize<IdentityServerToken>(responseData);
                }
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new IdentityServerError
                    {
                        Error = "Identity server endpoint not found"
                    };
                }

                return System.Text.Json.JsonSerializer.Deserialize<IdentityServerError>(responseData);
            }
        }

        private List<KeyValuePair<string, string>> GetNameValueCollection(TokenParameter parameter)
        {
            var nameValueCollection = new List<KeyValuePair<string, string>>();

            nameValueCollection.Add(new KeyValuePair<string, string>("client_id", parameter.ClientId));
            nameValueCollection.Add(new KeyValuePair<string, string>("grant_type", parameter.GrantType));

            if (parameter.GrantType == nameof(GrantTypes.password))
            {
                nameValueCollection.Add(new KeyValuePair<string, string>("password", parameter.Password));
                nameValueCollection.Add(new KeyValuePair<string, string>("username", parameter.UserName));
            }
            if (parameter.GrantType == nameof(GrantTypes.client_credentials))
            {
                nameValueCollection.Add(new KeyValuePair<string, string>("client_secret", parameter.ClientSecret));
            }

            if (parameter.Scope != null)
            {
                foreach (var scope in parameter.Scope)
                {
                    nameValueCollection.Add(new KeyValuePair<string, string>("scope", scope));
                }
            }
            else 
            {
                nameValueCollection.Add(new KeyValuePair<string, string>("scope", _configuration["Settings:DefaultScope"]));
            }

            return nameValueCollection;
        }
    }
}
