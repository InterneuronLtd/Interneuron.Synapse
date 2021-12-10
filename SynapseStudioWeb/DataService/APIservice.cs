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


namespace SynapseStudioWeb.DataService
{
    public class APIservice
    {
        //public static string BaseUrl =  Environment.GetEnvironmentVariable("connectionString_ServiceBaseURL");


        /// <summary>
        /// GEt List 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<T> GetList<T>(string query, string token)
        {
            string BaseUrl = Environment.GetEnvironmentVariable("connectionString_ServiceBaseURL");

            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder(BaseUrl + "GetList");
                builder.Query = query;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync(builder.Uri).Result;

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
        }
        /// <summary>
        /// Get list by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<T> GetListById<T>(string id, string query, string token)
        {
            string BaseUrl = Environment.GetEnvironmentVariable("connectionString_ServiceBaseURL");

            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder(BaseUrl + "GetListByAttribute");
                builder.Query = query + id;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.GetAsync(builder.Uri).Result;

                using (StreamReader sr = new StreamReader(result.Content.ReadAsStreamAsync().Result))
                {
                    string content = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
        }
        /// <summary>
        /// Post object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string PostObject<T>(T t, string query, string token)
        {
            string BaseUrl = Environment.GetEnvironmentVariable("connectionString_ServiceBaseURL");

            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder(BaseUrl + "PostObject");
                builder.Query = query;

                var json = JsonConvert.SerializeObject(t);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

                try
                {

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var httpResponseMessage = client.PostAsync(builder.Uri, stringContent).Result;

                    return httpResponseMessage.StatusCode.ToString();

                }
                catch (OperationCanceledException) { return "error"; }

            }
        }
        /// <summary>
        /// Delete object
        /// </summary>
        /// <param name="id"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static string DeleteObject(string id, string query, string token)
        {
            string BaseUrl = Environment.GetEnvironmentVariable("connectionString_ServiceBaseURL");

            using (var client = new HttpClient())
            {
                UriBuilder builder = new UriBuilder(BaseUrl + "DeleteObject");
                builder.Query = query + id;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var result = client.DeleteAsync(builder.Uri).Result;

                return result.StatusCode.ToString();

            }
        }
    }
}
