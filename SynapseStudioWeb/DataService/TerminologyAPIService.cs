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


﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SynapseStudioWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using SynapseStudioWeb.DataService.APIModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.Models.MedicinalMgmt;
using SynapseStudioWeb.DataService.APIModel.Requests;
using Serilog;
using Interneuron.Common.Extensions;

namespace SynapseStudioWeb.DataService
{
    public class TerminologyAPIService
    {
        const string TerminologyAPI_URI = "api/terminology";
        const string Terminology_UTIL_API_URI = "api/util/Configuration";

        const string FormularyAPI_URI = "api/formulary";
        public static async Task<TerminologyAPIResponse<FormularySearchResultAPIModel>> SearchFormularies(FormularyFilterCriteriaAPIModel filterCriteria, string token)
        {
            var results = await InvokeService<FormularySearchResultAPIModel>($"{FormularyAPI_URI}/searchformularies/", token, HttpMethod.Post, filterCriteria);
            return results;
        }

        public static async Task<TerminologyAPIResponse<List<FormularyListAPIModel>>> SearchFormulariesAsList(FormularyFilterCriteriaAPIModel filterCriteria, string token)
        {
            var results = await InvokeService<List<FormularyListAPIModel>>($"{FormularyAPI_URI}/searchformulariesaslist/", token, HttpMethod.Post, filterCriteria);
            return results;
        }

        public static async Task<TerminologyAPIResponse<List<FormularyListAPIModel>>> GetLatestTopLevelFormulariesBasicInfo(string token)
        {
            var results = await InvokeService<List<FormularyListAPIModel>>($"{FormularyAPI_URI}/getlatesttoplevelformulariesbasicinfo/", token, HttpMethod.Post);
            return results;
        }

        public static async Task<TerminologyAPIResponse<List<FormularyListAPIModel>>> GetFormulariesAsDiluents(string token)
        {
            var result = await InvokeService<List<FormularyListAPIModel>>($"{FormularyAPI_URI}/getformulariesasdiluents", token, HttpMethod.Get);
            return result;
        }

        public static async Task<TerminologyAPIResponse<List<FormularyListAPIModel>>> GetFormularyDescendentForCodes(GetFormularyDescendentForCodesAPIRequest request, string token)
        {
            var results = await InvokeService<List<FormularyListAPIModel>>($"{FormularyAPI_URI}/getdescendentformulariesforcodes/", token, HttpMethod.Post, request);
            return results;
        }

        public static async Task<TerminologyAPIResponse<DMDSearchResultsWithHierarchy>> SearchDMDWithAllDescendents(string searchTxt, string token)
        {
            var results = await InvokeService<DMDSearchResultsWithHierarchy>($"{TerminologyAPI_URI}/searchdmdwithalldescendents?q={System.Web.HttpUtility.UrlEncode(searchTxt)}", token, HttpMethod.Get);
            return results;
        }

        public static async Task<TerminologyAPIResponse<DMDSearchResultsWithHierarchy>> SearchDMDNamesGetWithAllLevelNodes(string searchTxt, string token)
        {
            var results = await InvokeService<DMDSearchResultsWithHierarchy>($"{TerminologyAPI_URI}/searchdmdwithallnodes?q={System.Web.HttpUtility.UrlEncode(searchTxt)}", token, HttpMethod.Get);
            return results;
        }

        public static async Task<TerminologyAPIResponse<DMDSearchResultsWithHierarchy>> SearchDMDSyncLog(string searchTxt, string token)
        {
            var results = await InvokeService<DMDSearchResultsWithHierarchy>($"{TerminologyAPI_URI}/SearchDMDSyncLog?q={System.Web.HttpUtility.UrlEncode(searchTxt)}", token, HttpMethod.Get);
            return results;
        }


        public static async Task<List<FormularyLookupAPIModel>> GetRecordStatusLookup(string token)
        {
            var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getrecordstatuslookup", token, HttpMethod.Get);

            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<FormularyLookupAPIModel>> GetFormularyStatusLookup(string token)
        {
            var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getformularystatuslookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<TerminologyAPIResponse<ImportFormularyAPIModel>> ImportMeds(List<string> meds, string token, string formularyStatusCd = null, string recordStatusCd = null)
        {
            var url = $"{FormularyAPI_URI}/import";

            var urlParams = new List<string>();
            string urlParam = string.Empty;

            if (formularyStatusCd.IsNotEmpty())
                urlParams.Add($"formularyStatusCd={formularyStatusCd}");

            if (recordStatusCd.IsNotEmpty())
            {
                urlParams.Add($"recordStatusCd={recordStatusCd}");
            }

            if (urlParams.IsCollectionValid())
            {
                urlParam = urlParams[0];
                if (urlParams.Count > 1)
                    urlParam = string.Join("&", urlParams);
            }

            if (urlParam.IsNotEmpty())
                url = $"{url}?{urlParam}";

            var result = await InvokeService<ImportFormularyAPIModel>(url, token, HttpMethod.Post, meds);
            return result;
        }

        public static async Task<TerminologyAPIResponse<object>> ImportDeltas(string token)
        {
            var url = $"{FormularyAPI_URI}/importdeltas";

            var result = await InvokeService<object>(url, token, HttpMethod.Post);
            return result;
        }

        public static async Task<TerminologyAPIResponse<object>> ImportAllMedsFromDMDWithRules(string token)
        {
            var url = $"{FormularyAPI_URI}/importalldmdcodes";

            var result = await InvokeService<object>(url, token, HttpMethod.Post);
            return result;
        }

        public static async Task<TerminologyAPIResponse<object>> InvokePostImportProcess(string token, List<string> medCodes = null)
        {
            var url = $"{FormularyAPI_URI}/invokepostimportprocess";

            var result = await InvokeService<object>(url, token, HttpMethod.Post, medCodes);
            return result;
        }


        public static async Task<TerminologyAPIResponse<FormularyHeaderAPIModel>> GetFormularyDetailRuleBound(string id, string token)
        {
            var result = await InvokeService<FormularyHeaderAPIModel>($"{FormularyAPI_URI}/getformularydetailrulebound/{id}", token, HttpMethod.Get);
            return result;
        }

        public static async Task<TerminologyAPIResponse<List<FormularyHeaderAPIModel>>> GetLatestFormulariesHeaderOnlyByNameOrCode(string token, string productNameOrCode, string productType = null, bool isExactMatch = false)
        {
            var result = await InvokeService<List<FormularyHeaderAPIModel>>($"{FormularyAPI_URI}/getlatestformulariesheaderonlybynameorcode?nameOrCode={productNameOrCode}&productType={productType}&isExactSearch={isExactMatch}", token, HttpMethod.Get);
            return result;
        }



        //public static async Task<TerminologyAPIResponse<FormularyHeaderAPIModel>> GetFormularyDetail(string id, string token)
        //{
        //    var result = await InvokeService<FormularyHeaderAPIModel>($"{FormularyAPI_URI}/getformularydetail/{id}", token, HttpMethod.Get);
        //    return result;

        //    //string baseUrl = Environment.GetEnvironmentVariable("connectionString_TerminologyServiceBaseURL");

        //    //using (var client = new HttpClient())
        //    //{
        //    //    UriBuilder builder = new UriBuilder(baseUrl + "/api/getformularydetail/" + id);
        //    //    //builder.Path = id;

        //    //    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    //    var result = await client.GetAsync(builder.Uri);
        //    //    FormularyDetailAPIModel model = null;
        //    //    using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
        //    //    {
        //    //        string content = sr.ReadToEnd();
        //    //        model = JsonConvert.DeserializeObject<FormularyDetailAPIModel>(content);
        //    //    }

        //    //    return model;
        //    //}
        //}

        //public static async Task<List<MedicationTypeAPIModel>> GetMedicationTypes(string token)
        //{
        //    var apiResponse = await InvokeService<List<MedicationTypeAPIModel>>($"{FormularyAPI_URI}/getmedicationtypelookup", token, HttpMethod.Get);
        //    if (apiResponse.StatusCode != StatusCode.Success)
        //    {
        //        return null;
        //    }
        //    return apiResponse.Data;
        //}

        //public static async Task<List<FormularyLookupAPIModel>> GetMarkedModifiers(string token)
        //{
        //    var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getmodifierlookup", token, HttpMethod.Get);
        //    if (apiResponse.StatusCode != StatusCode.Success)
        //    {
        //        return null;
        //    }
        //    return apiResponse.Data;
        //}

        public static async Task<List<BasisOfPreferredNameAPIModel>> GetBasisOfPreferredName(string token)
        {
            var apiResponse = await InvokeService<List<BasisOfPreferredNameAPIModel>>($"{TerminologyAPI_URI}/getdmdbasisofnamelookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<LicensingAuthorityAPIModel>> GetLicensingAuthority(string token)
        {
            var apiResponse = await InvokeService<List<LicensingAuthorityAPIModel>>($"{TerminologyAPI_URI}/getdmdlicensingauthoritylookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<DoseFormAPIModel>> GetDoseForms(string token)
        {
            var apiResponse = await InvokeService<List<DoseFormAPIModel>>($"{TerminologyAPI_URI}/getdmddoseformlookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<FormularyLookupAPIModel>> GetRoundingFactor(string token)
        {
            var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getroundingfactorlookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<ControlledDrugCategoryAPIModel>> GetControlledDrugCategories(string token)
        {
            var apiResponse = await InvokeService<List<ControlledDrugCategoryAPIModel>>($"{TerminologyAPI_URI}/getdmdcontroldrugcategorylookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }



        //public static async Task<List<FormularyLookupAPIModel>> GetModifiedReleases(string token)
        //{
        //    var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getmodifiedreleaselookup", token, HttpMethod.Get);
        //    if (apiResponse.StatusCode != StatusCode.Success)
        //    {
        //        return null;
        //    }
        //    return apiResponse.Data;
        //}

        //public static async Task<List<FormularyLookupAPIModel>> GetOrderableStatuses(string token)
        //{
        //    var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getorderablestatuslookup", token, HttpMethod.Get);
        //    if (apiResponse.StatusCode != StatusCode.Success)
        //    {
        //        return null;
        //    }
        //    return apiResponse.Data;
        //}

        public static async Task<List<PrescribingStatusesAPIModel>> GetPrescribingStatuses(string token)
        {
            var apiResponse = await InvokeService<List<PrescribingStatusesAPIModel>>($"{TerminologyAPI_URI}/getdmdprescribingstatuslookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<RestrictionsOnAvailabilityAPIModel>> GetRestrictionsOnAvailability(string token)
        {
            var apiResponse = await InvokeService<List<RestrictionsOnAvailabilityAPIModel>>($"{TerminologyAPI_URI}/getdmdavailrestrictionslookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<FormularyLookupAPIModel>> GetTitrationType(string token)
        {
            var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/gettitrationtypelookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<FormularyLookupAPIModel>> GetFormularyStatuses(string token)
        {
            var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getformularystatuslookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<RouteLookup>> GetRoutes(string token)
        {
            var apiResponse = await InvokeService<List<RouteLookup>>($"{TerminologyAPI_URI}/getdmdroutelookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }


        //public static async Task<List<FormAndRouteAPIModel>> GetFormAndRoutes(string token)
        //{
        //    var apiResponse = await InvokeService<List<FormAndRouteAPIModel>>($"{TerminologyAPI_URI}/getdmdontformroutelookup", token, HttpMethod.Get);
        //    if (apiResponse.StatusCode != StatusCode.Success)
        //    {
        //        return null;
        //    }
        //    return apiResponse.Data;
        //}

        public static async Task<List<IngredientAPIModel>> GetIngredients(string token)
        {
            var apiResponse = await InvokeService<List<IngredientAPIModel>>($"{TerminologyAPI_URI}/getdmdingredientlookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<UOMAPIModel>> GetUOMs(string token)
        {
            var apiResponse = await InvokeService<List<UOMAPIModel>>($"{TerminologyAPI_URI}/getdmduomlookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<FormularyLookupAPIModel>> GetProductTypes(string token)
        {
            var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getproducttypelookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<FormularyLookupAPIModel>> GetClassificationCodeTypesLookup(string token)
        {
            var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getclassificationcodetypelookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<FormularyLookupAPIModel>> GetIdentificationCodeTypesLookup(string token)
        {
            var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getidentificationcodetypelookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<SupplierAPIModel>> GetSuppliers(string token)
        {
            var apiResponse = await InvokeService<List<SupplierAPIModel>>($"{TerminologyAPI_URI}/getdmdsupplierlookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        public static async Task<List<FormCodeAPIModel>> GetFormCodes(string token)
        {
            var apiResponse = await InvokeService<List<FormCodeAPIModel>>($"{TerminologyAPI_URI}/getdmdformlookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        //public static async Task<List<FormularyLookupAPIModel>> GetOrderFormTypes(string token)
        //{
        //    var apiResponse = await InvokeService<List<FormularyLookupAPIModel>>($"{FormularyAPI_URI}/getorderformtypelookup", token, HttpMethod.Get);
        //    if (apiResponse.StatusCode != StatusCode.Success)
        //    {
        //        return null;
        //    }
        //    return apiResponse.Data;
        //}

        public static async Task<List<BasisOfPharmaStrengthAPIModel>> GetBasisOfPharmaStrength(string token)
        {
            var apiResponse = await InvokeService<List<BasisOfPharmaStrengthAPIModel>>($"{TerminologyAPI_URI}/getdmdpharamceuticalstrengthlookup", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success)
            {
                return null;
            }
            return apiResponse.Data;
        }

        //public static async Task<List<DrugClassAPIModel>> GetDrugClassLookup(string token)
        //{
        //    var apiResponse = await InvokeService<List<DrugClassAPIModel>>($"{FormularyAPI_URI}/getdrugclasslookup", token, HttpMethod.Get);
        //    if (apiResponse.StatusCode != StatusCode.Success)
        //    {
        //        return null;
        //    }
        //    return apiResponse.Data;
        //}

        public static async Task<TerminologyAPIResponse<UpdateFormularyStatusAPIResponse>> UpdateFormularyStatus(UpdateFormularyStatusAPIRequest request, string token)
        {
            var results = await InvokeService<UpdateFormularyStatusAPIResponse>($"{FormularyAPI_URI}/updatestatus", token, HttpMethod.Put, request);
            return results;
        }

        public static async Task<TerminologyAPIResponse<UpdateFormularyStatusAPIResponse>> BulkUpdateFormularyStatus(UpdateFormularyStatusAPIRequest request, string token)
        {
            var results = await InvokeService<UpdateFormularyStatusAPIResponse>($"{FormularyAPI_URI}/bulkupdatestatus", token, HttpMethod.Put, request);
            return results;
        }

        public static async Task<SnomedCTSearchAPIModel> SearchSNOMEDData(string q, string semanticTag, string token)
        {
            var apiResponse = await InvokeService<SnomedCTSearchAPIModel>($"{TerminologyAPI_URI}/searchsnomed?q={System.Web.HttpUtility.UrlEncode(q)}&semanticTag={semanticTag}", token, HttpMethod.Get);
            if (apiResponse.StatusCode != StatusCode.Success) return null;
            return apiResponse.Data;
        }

        //public static async Task<TerminiologyAPIResponse<CreateFormularyAPIResponse>> CreateCustomMedication(List<CreateFormularyAPIRequest> formulary, string token)
        //{
        //    var request = new CreateFormularyAPIRequestWrapper();
        //    request.RequestsData = formulary;
        //    return await InvokeService<CreateFormularyAPIResponse>($"{FormularyAPI_URI}/create", token, HttpMethod.Post, request);
        //}

        public static async Task<TerminologyAPIResponse<CreateFormularyAPIResponse>> CreateCustomMedication(List<FormularyHeaderAPIModel> formularies, string token)
        {
            var request = new CreateFormularyAPIRequest
            {
                RequestsData = formularies
            };
            return await InvokeService<CreateFormularyAPIResponse>($"{FormularyAPI_URI}/create", token, HttpMethod.Post, request);
        }

        //public static async Task<TerminiologyAPIResponse<CreateFormularyAPIResponse>> FileImportMedication(List<CreateFormularyAPIRequest> formulary, string token)
        //{
        //    var request = new CreateFormularyAPIRequestWrapper();
        //    request.RequestsData = formulary;
        //    return await InvokeService<CreateFormularyAPIResponse>($"{FormularyAPI_URI}/fileimport", token, HttpMethod.Post, request);
        //}

        public static async Task<TerminologyAPIResponse<CreateFormularyAPIResponse>> FileImportMedication(List<FormularyHeaderAPIModel> formularies, string token)
        {
            var request = new CreateFormularyAPIRequest
            {
                RequestsData = formularies
            };
            return await InvokeService<CreateFormularyAPIResponse>($"{FormularyAPI_URI}/fileimport", token, HttpMethod.Post, request);
        }

        public static async Task<TerminologyAPIResponse<List<string>>> GetAllDMDCodes(string token)
        {
            return await InvokeService<List<string>>($"{TerminologyAPI_URI}/getalldmdcodes", token, HttpMethod.Get);
        }

        public static async Task<TerminologyAPIResponse<UpdateFormularyAPIResponse>> UpdateFormulary(List<FormularyHeaderAPIModel> formularies, string token)
        {
            var request = new UpdateFormularyAPIRequest
            {
                RequestsData = formularies
            };
            var test = JsonConvert.SerializeObject(request);
            return await InvokeService<UpdateFormularyAPIResponse>($"{FormularyAPI_URI}/update", token, HttpMethod.Put, request);
        }

        //public static async Task<TerminiologyAPIResponse<CreateFormularyAPIResponse>> UpdateCustomMedication(List<CreateFormularyAPIRequest> formulary, string token)
        //{
        //    var request = new CreateFormularyAPIRequestWrapper();
        //    request.RequestsData = formulary;
        //    var test = JsonConvert.SerializeObject(request);
        //    return await InvokeService<CreateFormularyAPIResponse>($"{FormularyAPI_URI}/update", token, HttpMethod.Put, request);
        //}

        public static async Task<TerminologyAPIResponse<List<FormularyBasicInfoModel>>> GetLatestFormulariesHeaderOnly(string token)
        {
            var results = await InvokeService<List<FormularyBasicInfoModel>>($"{FormularyAPI_URI}/getlatestformulariesheaderonly/", token, HttpMethod.Get);
            return results;
        }


        public static async Task<TerminologyAPIResponse<TerminologyConfigurationAPIModel>> GetTerminologyConfiguration(string configKeyName, string token)
        {
            var results = await InvokeService<TerminologyConfigurationAPIModel>($"{Terminology_UTIL_API_URI}/getconfiguration/{configKeyName}", token, HttpMethod.Get);
            return results;
        }

        public static async Task<TerminologyAPIResponse<DeriveProductNamesAPIModel>> DeriveProductName(DeriveProductNamesRequest apiRequest, string token)
        {
            var apiResponse = await InvokeService<DeriveProductNamesAPIModel>($"{FormularyAPI_URI}/deriveproductnames", token, HttpMethod.Post, apiRequest);
            return apiResponse;
        }

        public static async Task<TerminologyAPIResponse<CheckIfProductExistsAPIModel>> CheckIfProductExists(CheckIfProductExistsRequest apiRequest, string token)
        {
            var apiResponse = await InvokeService<CheckIfProductExistsAPIModel>($"{FormularyAPI_URI}/checkifproductexists", token, HttpMethod.Post, apiRequest);
            return apiResponse;
        }

        private static async Task<TerminologyAPIResponse<T>> InvokeService<T>(string apiEndpoint, string token, HttpMethod method, dynamic payload = null, Func<string, bool> onError = null)
        {
            var response = new TerminologyAPIResponse<T> { StatusCode = StatusCode.Success };

            string baseUrl = Environment.GetEnvironmentVariable("connectionString_TerminologyServiceBaseURL");

            using (var client = new HttpClient())
            {
                var url = baseUrl.EndsWith("/") ? $"{baseUrl}{apiEndpoint}" : $"{baseUrl}/{apiEndpoint}";

                UriBuilder builder = new UriBuilder(url);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.Timeout = TimeSpan.FromHours(24);

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
                        response.StatusCode = StatusCode.Fail;

                        response.ErrorMessages = new List<string> { content };

                        onError?.Invoke(content);

                        //try
                        //{
                        //    Log.Error(result.ToString());
                        //}
                        //catch { }

                        //var isErrorHandledInCallback = onError?.Invoke(content);

                        //if (isErrorHandledInCallback.GetValueOrDefault() == true)
                        //    return default(T);
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

        public static async Task<TerminologyAPIResponse<DmdSnomedVersionAPIModel>> GetDmdSnomedVersion(string token)
        {
            return await InvokeService<DmdSnomedVersionAPIModel>($"{TerminologyAPI_URI}/getdmdsnomedversion", token, HttpMethod.Get);
        }

        public static async Task<TerminologyAPIResponse<List<FormularyHistoryModel>>> GetHistoryOfFormularies(string token)
        {
            return await InvokeService<List<FormularyHistoryModel>>($"{FormularyAPI_URI}/gethistoryofformularies", token, HttpMethod.Get);
        }
    }
}

