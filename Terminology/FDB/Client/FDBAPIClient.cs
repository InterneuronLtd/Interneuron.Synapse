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
﻿using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using Interneuron.FDBAPI.Client.DataModels;
using System.Text;

namespace Interneuron.FDBAPI.Client
{
    public class FDBAPIClient
    {
        private string _baseUrl;

        public FDBAPIClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<List<string>> GetCautionsByCode(string productType, string productCode, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetCautionsByCode?productType={productType}&productCode={productCode}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<string>>(content);
                }
            }
        }

        public async Task<List<string>> GetCautionsByName(string productType, string productName, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetCautionsByName?productType={productType}&productName={productName}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<string>>(content);
                }
            }
        }

        public async Task<List<FDBIdText>> GetContraIndicationsByCode(string productType, string productCode, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetContraIndicationsByCode?productType={productType}&productCode={productCode}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<FDBIdText>>(content);
                }
            }
        }

        public async Task<List<FDBIdText>> GetContraIndicationsByName(string productType, string productName, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetContraIndicationsByName?productType={productType}&productName={productName}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<FDBIdText>>(content);
                }
            }
        }

        public async Task<List<string>> GetSideEffectsByCode(string productType, string productCode, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetSideEffectsByCode?productType={productType}&productCode={productCode}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<string>>(content);
                }
            }
        }

        public async Task<List<string>> GetSideEffectsByName(string productType, string productName, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetSideEffectsByName?productType={productType}&productName={productName}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<string>>(content);
                }
            }
        }

        public async Task<List<string>> GetSafetyMessagesByCode(string productType, string productCode, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetSafetyMessagesByCode?productType={productType}&productCode={productCode}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<string>>(content);
                }
            }
        }

        public async Task<List<string>> GetSafetyMessagesByName(string productType, string productName, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetSafetyMessagesByName?productType={productType}&productName={productName}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<string>>(content);
                }
            }
        }

        public async Task<List<FDBIdText>> GetLicensedUseByCode(string productType, string productCode, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetLicensedUseByCode?productType={productType}&productCode={productCode}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<FDBIdText>>(content);
                }
            }
        }

        public async Task<List<FDBIdText>> GetLicensedUseByName(string productType, string productName, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetLicensedUseByName?productType={productType}&productName={productName}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<FDBIdText>>(content);
                }
            }
        }

        public async Task<List<FDBIdText>> GetUnLicensedUseByCode(string productType, string productCode, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetUnLicensedUseByCode?productType={productType}&productCode={productCode}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<FDBIdText>>(content);
                }
            }
        }

        public async Task<List<FDBIdText>> GetUnLicensedUseByName(string productType, string productName, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetUnLicensedUseByName?productType={productType}&productName={productName}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<FDBIdText>>(content);
                }
            }
        }

        public async Task<bool?> GetAdverseEffectsFlagByCode(string productCode, string token)
        {
            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder($"{_baseUrl}/GetAdverseEffectsFlagByCode?productCode={productCode}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = await client.GetAsync(builder.Uri);

                bool? data;

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    if (content == null || string.IsNullOrEmpty(content.Trim())) data = default(bool?);
                    else data = JsonConvert.DeserializeObject<bool?>(content);
                }

                return data;
            }
        }



        public async Task<FDBAPIResourceModel<List<string>>> GetCautionsByCode1(string productType, string productCode, string token)
        {
            var endpoint = $"/GetCautionsByCode?productType={productType}&productCode={productCode}";

            return await InvokeService<List<string>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<Dictionary<string, List<string>>>> GetCautionsByCodes(List<FDBDataRequest> request, string token)
        {
            var endpoint = $"/GetCautionsByCodes";

            return await InvokeService<Dictionary<string, List<string>>>(endpoint, token, HttpMethod.Post, request);
        }

        public async Task<FDBAPIResourceModel<List<string>>> GetCautionsByName1(string productType, string productName, string token)
        {
            var endpoint = $"/GetCautionsByName?productType={productType}&productName={productName}";

            return await InvokeService<List<string>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<List<FDBIdText>>> GetContraIndicationsByCode1(string productType, string productCode, string token)
        {
            var endpoint = $"/GetContraIndicationsByCode?productType={productType}&productCode={productCode}";

            return await InvokeService<List<FDBIdText>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<Dictionary<string, List<FDBIdText>>>> GetContraIndicationsByCodes(List<FDBDataRequest> request, string token)
        {
            var endpoint = $"/GetContraIndicationsByCodes";

            return await InvokeService<Dictionary<string, List<FDBIdText>>>(endpoint, token, HttpMethod.Post, request);
        }


        public async Task<FDBAPIResourceModel<List<FDBIdText>>> GetContraIndicationsByName1(string productType, string productName, string token)
        {
            var endpoint = $"/GetContraIndicationsByName?productType={productType}&productName={productName}";

            return await InvokeService<List<FDBIdText>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<List<string>>> GetSideEffectsByCode1(string productType, string productCode, string token)
        {
            var endpoint = $"/GetSideEffectsByCode?productType={productType}&productCode={productCode}";

            return await InvokeService<List<string>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<Dictionary<string, List<string>>>> GetSideEffectsByCodes(List<FDBDataRequest> request, string token)
        {
            var endpoint = $"/GetSideEffectsByCodes";

            return await InvokeService<Dictionary<string, List<string>>>(endpoint, token, HttpMethod.Post, request);
        }

        public async Task<FDBAPIResourceModel<List<string>>> GetSideEffectsByName1(string productType, string productName, string token)
        {
            var endpoint = $"/GetSideEffectsByName?productType={productType}&productName={productName}";

            return await InvokeService<List<string>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<List<string>>> GetSafetyMessagesByCode1(string productType, string productCode, string token)
        {
            var endpoint = $"/GetSafetyMessagesByCode?productType={productType}&productCode={productCode}";

            return await InvokeService<List<string>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<Dictionary<string, List<string>>>> GetSafetyMessagesByCodes(List<FDBDataRequest> request, string token)
        {
            var endpoint = $"/GetSafetyMessagesByCodes";

            return await InvokeService<Dictionary<string, List<string>>>(endpoint, token, HttpMethod.Post, request);
        }

        public async Task<FDBAPIResourceModel<List<string>>> GetSafetyMessagesByName1(string productType, string productName, string token)
        {
            var endpoint = $"/GetSafetyMessagesByName?productType={productType}&productName={productName}";

            return await InvokeService<List<string>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<List<FDBIdText>>> GetLicensedUseByCode1(string productType, string productCode, string token)
        {
            var endpoint = $"/GetLicensedUseByCode?productType={productType}&productCode={productCode}";

            return await InvokeService<List<FDBIdText>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<Dictionary<string, List<FDBIdText>>>> GetLicensedUseByCodes(List<FDBDataRequest> request, string token)
        {
            var endpoint = $"/GetLicensedUseByCodes";

            return await InvokeService<Dictionary<string, List<FDBIdText>>>(endpoint, token, HttpMethod.Post, request);
        }

        public async Task<FDBAPIResourceModel<List<FDBIdText>>> GetLicensedUseByName1(string productType, string productName, string token)
        {
            var endpoint = $"/GetLicensedUseByName?productType={productType}&productName={productName}";

            return await InvokeService<List<FDBIdText>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<List<FDBIdText>>> GetUnLicensedUseByCode1(string productType, string productCode, string token)
        {
            var endpoint = $"/GetUnLicensedUseByCode?productType={productType}&productCode={productCode}";

            return await InvokeService<List<FDBIdText>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<Dictionary<string, List<FDBIdText>>>> GetUnLicensedUseByCodes(List<FDBDataRequest> request, string token)
        {
            var endpoint = $"/GetUnLicensedUseByCodes";

            return await InvokeService<Dictionary<string, List<FDBIdText>>>(endpoint, token, HttpMethod.Post, request);
        }

        public async Task<FDBAPIResourceModel<List<FDBIdText>>> GetUnLicensedUseByName1(string productType, string productName, string token)
        {
            var endpoint = $"/GetUnLicensedUseByName?productType={productType}&productName={productName}";

            return await InvokeService<List<FDBIdText>>(endpoint, token, HttpMethod.Get);
        }

        public async Task<FDBAPIResourceModel<Dictionary<string, bool>>> GetAdverseEffectsFlagByCodes(List<FDBDataRequest> request, string token)
        {
            var endpoint = $"/GetAdverseEffectsFlagByCodes";

            return await InvokeService<Dictionary<string, bool>>(endpoint, token, HttpMethod.Post, request);
        }

        public async Task<FDBAPIResourceModel<Dictionary<string, bool?>>> GetHighRiskFlagByCodes(List<FDBDataRequest> request, string token)
        {
            var endpoint = $"/GetHighRiskFlagByCodes";

            return await InvokeService<Dictionary<string, bool?>>(endpoint, token, HttpMethod.Post, request);
        }

        public async Task<FDBAPIResourceModel<Dictionary<string, (string, string)>>> GetTherapeuticClassificationGroupsByCodes(List<FDBDataRequest> request, string token)
        {
            var endpoint = $"/GetTherapeuticClassificationGroupsByCodes";

            return await InvokeService<Dictionary<string, (string, string)>>(endpoint, token, HttpMethod.Post, request);
        }

        public async Task<FDBAPIResourceModel<FDBAPIResponse>> GetFDBDetailByCodes(List<FDBDataRequest> request, string token)
        {
            var endpoint = $"/GetFDBDetailByCodes";

            return await InvokeService<FDBAPIResponse>(endpoint, token, HttpMethod.Post, request);
        }

        private async Task<FDBAPIResourceModel<T>> InvokeService<T>(string apiEndpoint, string token, HttpMethod method, dynamic payload = null, Func<string, bool> onError = null)
        {
            var response = new FDBAPIResourceModel<T>() { StatusCode = StatusCode.Success };

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(60);

                UriBuilder builder = new UriBuilder($"{_baseUrl}/{apiEndpoint}");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var requestMessage = new HttpRequestMessage(method, builder.Uri);

                if ((payload != null) && (method == HttpMethod.Post || method == HttpMethod.Put))
                {
                    var json = JsonConvert.SerializeObject(payload);

                    var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                    requestMessage.Content = stringContent;
                }

                var result = await client.SendAsync(requestMessage);

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();

                    if (!result.IsSuccessStatusCode)
                    {
                        response.StatusCode = StatusCode.Fail;// Error
                        response.ErrorMessages = new List<string> { content };

                        onError?.Invoke(content);

                        //var isErrorHandledInCallback = onError?.Invoke(content);

                        //if (isErrorHandledInCallback.GetValueOrDefault() == true)
                        //    return response;//default(T);
                    }
                    else
                    {
                        var data = JsonConvert.DeserializeObject<T>(content);
                        response.Data = data;
                    }
                }
                return response;
            }
        }

    }
}
