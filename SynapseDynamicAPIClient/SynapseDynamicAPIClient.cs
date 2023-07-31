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
﻿

using Interneuron.SynapseDynamicAPIClient.Models;
using System;
using System.Linq;
using System.Net.Http;

namespace Interneuron.Synapse
{
    public class SynapseDynamicAPIClient
    {
        protected readonly HttpClient _client;

        public SynapseDynamicAPIClient(HttpClient client)
        {
            _client = client;
        }

        /* SQLConnector Starts */

        public string ExecuteNonSQL(SQLData sqlData)
        {
            return PostData("sql/ExecuteNonSQL/", sqlData);
        }

        public string ExecuteSQL(SQLData sqlData)
        {
            return PostData("sql/ExecuteSQL/", sqlData);
        }

        /* SQLConnector Ends */

        /* DynamicController Starts */

        public string GetList(string synapseNamespace, string synapseEntityName, bool? returnSystemAttributes = null, string orderBy = null, string limit = null, string offset = null, string filter = null)
        {
            string endpoint = "GetList/" + synapseNamespace + "/" + synapseEntityName +
                            (returnSystemAttributes == null ? "" : "/" + returnSystemAttributes) +
                            (string.IsNullOrWhiteSpace(orderBy) ? "" : "/" + orderBy) +
                            (string.IsNullOrWhiteSpace(limit) ? "" : "/" + limit) +
                            (string.IsNullOrWhiteSpace(offset) ? "" : "/" + offset) +
                            (string.IsNullOrWhiteSpace(filter) ? "" : "/" + filter);

            return GetData(endpoint);
        }

        public string GetListByPost(string synapseNamespace, string synapseEntityName, object postData, bool? returnSystemAttributes = null, string orderBy = null, string limit = null, string offset = null, string filter = null)
        {
            string endpoint = "GetListByPost/" + synapseNamespace + "/" + synapseEntityName +
                            (returnSystemAttributes == null ? "" : "/" + returnSystemAttributes) +
                            (string.IsNullOrWhiteSpace(orderBy) ? "" : "/" + orderBy) +
                            (string.IsNullOrWhiteSpace(limit) ? "" : "/" + limit) +
                            (string.IsNullOrWhiteSpace(offset) ? "" : "/" + offset) +
                            (string.IsNullOrWhiteSpace(filter) ? "" : "/" + filter);

            return PostData(endpoint, postData);
        }

        public string GetObject(string synapseNamespace, string synapseEntityName, string id, bool? returnSystemAttributes = null)
        {
            string endpoint = "GetObject/" + synapseNamespace + "/" + synapseEntityName + "/" + id +
                            (returnSystemAttributes == null ? "" : "/" + returnSystemAttributes);

            return GetData(endpoint);
        }

        public string GetObjectWithInsert(string synapseNamespace, string synapseEntityName, string synapseAttributeName, string attributeValue, string keyValue, bool? returnSystemAttributes = null)
        {
            string endpoint = "GetObjectWithInsert/" + synapseNamespace + "/" + synapseEntityName + "/" + synapseAttributeName + "/" + attributeValue + "/" + keyValue +
                            (returnSystemAttributes == null ? "" : "/" + returnSystemAttributes);

            return GetData(endpoint);
        }

        public string GetListByAttribute(string synapseNamespace, string synapseEntityName, string synapseAttributeName, string attributeValue, bool? returnSystemAttributes = null, string orderBy = null, string limit = null, string offset = null, string filter = null)
        {
            string endpoint = "GetListByAttribute/" + synapseNamespace + "/" + synapseEntityName + "/" + synapseAttributeName + "/" + attributeValue +
                            (returnSystemAttributes == null ? "" : "/" + returnSystemAttributes) +
                            (string.IsNullOrWhiteSpace(orderBy) ? "" : "/" + orderBy) +
                            (string.IsNullOrWhiteSpace(limit) ? "" : "/" + limit) +
                            (string.IsNullOrWhiteSpace(offset) ? "" : "/" + offset) +
                            (string.IsNullOrWhiteSpace(filter) ? "" : "/" + filter);

            return GetData(endpoint);
        }

        public string PostObject(string synapseNamespace, string synapseEntityName, object postData)
        {
            string endpoint = "PostObject/" + synapseNamespace + "/" + synapseEntityName;

            return PostData(endpoint, postData);
        }

        public string PostObjectArray(string synapseNamespace, string synapseEntityName, object postData)
        {
            string endpoint = "PostObjectArray/" + synapseNamespace + "/" + synapseEntityName;

            return PostData(endpoint, postData);
        }

        public string PostObjectsInTransaction(object postData) {
            string endpoint = "PostObjectsInTransaction";

            return PostData(endpoint, postData);
        }

        public string DeleteObjectByAttribute(string synapseNamespace, string synapseEntityName, string synapseAttributeName, string attributeValue)
        {
            string endpoint = string.Format("DeleteObjectByAttribute/{0}/{1}/{2}/{3}", synapseNamespace, synapseEntityName, synapseAttributeName, attributeValue);

            return DeleteData(endpoint);
        }

        public string DeleteObject(string synapseNamespace, string synapseEntityName, string id)
        {
            string endpoint = string.Format("DeleteObject/{0}/{1}/{2}", synapseNamespace, synapseEntityName, id);

            return DeleteData(endpoint);
        }

        public string GetObjectHistory(string synapseNamespace, string synapseEntityName, string id)
        {
            string endpoint = "GetObjectHistory/" + synapseNamespace + "/" + synapseEntityName + "/" + id;

            return GetData(endpoint);
        }


        /* DynamicController Ends */

        /* Baseview Controller */
        public string GetBaseViewList(string baseviewName, string orderBy = null, string limit = null, string offset = null, string filter = null)
        {
            string endpoint = "GetBaseViewList/" + baseviewName +
                (string.IsNullOrWhiteSpace(orderBy) ? "" : "/" + orderBy) +
                (string.IsNullOrWhiteSpace(limit) ? "" : "/" + limit) +
                (string.IsNullOrWhiteSpace(offset) ? "" : "/" + offset) +
                (string.IsNullOrWhiteSpace(filter) ? "" : "/" + filter);

            return GetData(endpoint);
        }

        public string GetBaseViewListByAttribute(string baseviewName, string synapseAttributeName, string attributeValue, string orderBy = null, string limit = null, string offset = null, string filter = null)
        {
            string endpoint = "GetBaseViewListByAttribute/" + baseviewName + "/" + synapseAttributeName + "/" + attributeValue +
                            (string.IsNullOrWhiteSpace(orderBy) ? "" : "/" + orderBy) +
                            (string.IsNullOrWhiteSpace(limit) ? "" : "/" + limit) +
                            (string.IsNullOrWhiteSpace(offset) ? "" : "/" + offset) +
                            (string.IsNullOrWhiteSpace(filter) ? "" : "/" + filter);

            return GetData(endpoint);
        }

        public string GetBaseViewListObjectByAttribute(string baseviewName, string synapseAttributeName, string attributeValue, string filter = null)
        {
            string endpoint = "GetBaseViewListObjectByAttribute/" + baseviewName + "/" + synapseAttributeName + "/" + attributeValue +
                            (string.IsNullOrWhiteSpace(filter) ? "" : "/" + filter);

            return GetData(endpoint);
        }

        public string GetBaseViewListByPost(string baseviewName, object postData, string orderBy = null, string limit = null, string offset = null, string filter = null)
        {
            string endpoint = "GetBaseViewListByPost/" + baseviewName +
                            (string.IsNullOrWhiteSpace(orderBy) ? "" : "/" + orderBy) +
                            (string.IsNullOrWhiteSpace(limit) ? "" : "/" + limit) +
                            (string.IsNullOrWhiteSpace(offset) ? "" : "/" + offset) +
                            (string.IsNullOrWhiteSpace(filter) ? "" : "/" + filter);

            return PostData(endpoint, postData);
        }

        /* Baseview Controller ends */

        /* ListController Controller starts */

        public string GetListColumns(string listId)
        {
            return GetData("list/GetListColumns/" + listId);
        }

        public string GetListDetails(string listId)
        {
            return GetData("list/GetListDetails/" + listId);
        }

        public string GetListData(string listId)
        {
            return GetData("list/GetListData/" + listId);
        }

        public string GetListDataByPost(string listId, object postData)
        {
            return PostData("list/GetListDataByPost/" + listId, postData);
        }

        public string GetListByLocatorBoardID(string locatorBoardId)
        {
            return GetData("list/GetListByLocatorBoardID/" + locatorBoardId);
        }

        public string GetListQuestions(string listId, string contextValue)
        {
            return GetData("list/GetListQuestions/" + listId + "/" + contextValue);
        }

        public string GetListPersonBanner(string listId, string contextValue)
        {
            return GetData("list/GetListPersonBanner/" + listId + "/" + contextValue);
        }

        public string GetQuestionOptionCollection(string questionOptionCollectionId)
        {
            return GetData("list/GetQuestionOptionCollection/" + questionOptionCollectionId);
        }

        public string GetListSnapshot(string listId)
        {
            return GetData("list/GetListSnapshot/" + listId);
        }

        public string GetContext(string defaultcontext, string defaultcontextfield, string value)
        {
            return GetData("list/GetContext/" + defaultcontext + "/" + defaultcontextfield + "/" + value);
        }

        /* ListController Controller ends */

        /* AuthenticatorController Controller Begins */

        public string GetSmartCardToken(string userId)
        {
            return GetData("Authenticator/GetSmartCardToken?userId=" + userId);
        }

        public void RemoveSmartCardToken(string userId)
        {
            DeleteData("Authenticator/RemoveSmartCardToken?userId=" + userId);
        }

        /* AuthenticatorController Controller Ends */

        private string PostData(string endpoint, object postData)
        {
            var response = _client.PostAsJsonAsync(endpoint, postData).Result;

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            return result;
        }

        private string GetData(string endpoint)
        {
            var response = _client.GetAsync(endpoint).Result;

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            return result;
        }

        private string DeleteData(string endpoint)
        {
            var response = _client.DeleteAsync(endpoint).Result;

            response.EnsureSuccessStatusCode();

            var result = response.Content.ReadAsStringAsync().Result;

            return result;
        }
    }
}