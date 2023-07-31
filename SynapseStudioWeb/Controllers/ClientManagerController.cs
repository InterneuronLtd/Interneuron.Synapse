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


﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using NToastNotify;
using SynapseStudioWeb.AppCode.Filters;
using System.Security.Cryptography;
using System.Text;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class ClientManagerController : Controller
    {
        private IToastNotification toastNotification;

        public ClientManagerController(IToastNotification toastNotification)
        {
            this.toastNotification = toastNotification;
        }

        [StudioExceptionFilter()]
        public IActionResult ClientGrantTypes(string id)
        {
            ViewBag.grandtype = LoadClientgrandtypes(id);
            ViewBag.id = id;
            ViewBag.ClientId = GetClientId(id);

            return View();
        }

        [HttpPost]
        [StudioExceptionFilter()]
        public ActionResult AddClientGrantType(string grantType, string clientId)
        {
            string sql = "SELECT * FROM \"ClientGrantTypes\" WHERE \"ClientId\" = CAST(@ClientId AS INT) AND \"GrantType\" = @GrantType";

            var param = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("ClientId", clientId),
                    new KeyValuePair<string, string>("GrantType", grantType)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, param, "connectionString_SynapseIdentityStore");
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                this.toastNotification.AddErrorToastMessage("Client Grant Type already exists");
            }
            else
            {
                string insertSql = "INSERT INTO \"ClientGrantTypes\" (\"GrantType\", \"ClientId\") VALUES(@GrantType, CAST(@ClientId AS INT));";

                var paramList = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("GrantType", grantType),
                        new KeyValuePair<string, string>("ClientId", clientId)
                    };

                DataServices.ExcecuteNonQueryFromSQL(insertSql, paramList, "connectionString_SynapseIdentityStore");

                this.toastNotification.AddSuccessToastMessage("New Client Grant Type is added");
            }

            return RedirectToAction("ClientGrantTypes", "ClientManager", new { id = clientId });
        }

        [StudioExceptionFilter()]
        public ActionResult RemoveClientGrantType(string id, string clientId)
        {
            string sql = "DELETE FROM \"ClientGrantTypes\" WHERE \"Id\" = CAST(@Id AS INT);";

            var paramList = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("Id", id)
                    };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");

            this.toastNotification.AddSuccessToastMessage("Client Grant Type is removed");

            return RedirectToAction("ClientGrantTypes", "ClientManager", new { id = clientId });
        }

        [StudioExceptionFilter()]
        public IActionResult ClientClaims(string id)
        {
            string sql = "SELECT \"Id\", \"Type\", \"Value\", \"ClientId\" FROM public.\"ClientClaims\" WHERE \"ClientId\" = CAST(@ClientId AS INT) ORDER BY \"Id\";";

            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("ClientId", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");

            ViewBag.claims = ds.Tables[0];
            ViewBag.ClientId = GetClientId(id);
            ViewBag.id = id;
            return View();
        }

        [StudioExceptionFilter()]
        public ActionResult AddNewClientClaim(string claimType, string claimValue, string clientId)
        {
            string sql = "SELECT * FROM \"ClientClaims\" WHERE \"ClientId\" = CAST(@ClientId AS INT) AND \"Type\" = @type AND \"Value\" = @value";

            var param = new List<KeyValuePair<string, string>>() {
                    new KeyValuePair<string, string>("ClientId", clientId),
                    new KeyValuePair<string, string>("type", claimType),
                    new KeyValuePair<string, string>("Value", claimValue),
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, param, "connectionString_SynapseIdentityStore");
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                this.toastNotification.AddErrorToastMessage("Client Grant Type already exists");
            }
            else
            {
                string insertSql = "INSERT INTO \"ClientClaims\" (\"Type\", \"Value\", \"ClientId\") VALUES(@Type, @Value, CAST(@ClientId AS INT));";

                var paramList = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Type", claimType),
                    new KeyValuePair<string, string>("Value", claimValue),
                    new KeyValuePair<string, string>("ClientId", clientId)
                };

                DataServices.ExcecuteNonQueryFromSQL(insertSql, paramList, "connectionString_SynapseIdentityStore");

                this.toastNotification.AddSuccessToastMessage("New Client Claim is added");
            }

            return RedirectToAction("ClientClaims", "ClientManager", new { id = clientId });
        }

        [StudioExceptionFilter()]
        public ActionResult RemoveClientClaim(string id, string clientId)
        {
            string sql = "DELETE FROM \"ClientClaims\" WHERE \"Id\" = CAST(@Id AS INT);";

            var paramList = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("Id", id)
                    };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");

            this.toastNotification.AddSuccessToastMessage("Client Claim is removed");

            return RedirectToAction("ClientClaims", "ClientManager", new { id = clientId });
        }

        [StudioExceptionFilter()]
        public IActionResult ClientRedirectUris(string id)
        {
            List<string> uriType= new List<string>();

            uriType.Add("Redirect URI");
            uriType.Add("CORS origin URI");
            uriType.Add("Post logout redirect URI");

            string sql = " SELECT \"Id\", \"RedirectUri\" AS URI, \"ClientId\", 'Redirect URI' AS URIType FROM public.\"ClientRedirectUris\" WHERE \"ClientId\" = CAST(@ClientId AS INT)"
           + " UNION ALL "
           + " SELECT \"Id\", \"Origin\" AS URI, \"ClientId\", 'CORS origin URI' AS URIType FROM public.\"ClientCorsOrigins\" WHERE \"ClientId\" = CAST(@ClientId AS INT)"
           + " UNION ALL "
           + " SELECT \"Id\", \"PostLogoutRedirectUri\" AS URI, \"ClientId\", 'Post logout redirect URI' AS URIType FROM public.\"ClientPostLogoutRedirectUris\" WHERE \"ClientId\" = CAST(@ClientId AS INT)"
           + " ORDER BY URIType";

            var paramList = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("ClientId", id)
                };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");
            ViewBag.clientUri = ds.Tables[0];
            ViewBag.ClientId = GetClientId(id);
            ViewBag.uriType = uriType;
            ViewBag.id = id;
            return View();
        }

        [StudioExceptionFilter()]
        public ActionResult AddNewClientURI(string RedirectUri, string uritype, string clientId)
        {
            string sql = string.Empty;
            var paramList = new List<KeyValuePair<string, string>>();

            if (uritype == "Redirect URI")
            {
                sql = "INSERT INTO \"ClientRedirectUris\" (\"RedirectUri\", \"ClientId\") VALUES(@RedirectUri, CAST(@ClientId AS INT));";

                paramList = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("RedirectUri", RedirectUri),
                        new KeyValuePair<string, string>("ClientId", clientId)
                    };
            }
            else if (uritype == "CORS origin URI")
            {
                sql = "INSERT INTO \"ClientCorsOrigins\" (\"Origin\", \"ClientId\") VALUES(@Origin, CAST(@ClientId AS INT));";

                paramList = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("Origin", RedirectUri),
                        new KeyValuePair<string, string>("ClientId", clientId)
                    };
            }
            else if (uritype == "Post logout redirect URI")
            {
                sql = "INSERT INTO \"ClientPostLogoutRedirectUris\" (\"PostLogoutRedirectUri\", \"ClientId\") VALUES(@PostLogoutRedirectUri, CAST(@ClientId AS INT));";

                paramList = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("PostLogoutRedirectUri", RedirectUri),
                        new KeyValuePair<string, string>("ClientId", clientId)
                    };
            }

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");

            this.toastNotification.AddSuccessToastMessage("New Client Redirect URI is added");

            return RedirectToAction("ClientRedirectUris", "ClientManager", new { id = clientId });
        }

        [StudioExceptionFilter()]
        public ActionResult RemoveClientURI(string id, string type, string clientId)
        {
            string sql = string.Empty;

            string uriType = type.Split(" ")[0];

            var paramList = new List<KeyValuePair<string, string>>();

            if (uriType == "Redirect")
            {
                sql = "DELETE FROM \"ClientRedirectUris\" WHERE \"Id\" = CAST(@Id AS INT);";

                paramList = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("Id", id)
                        };

            }
            else if (uriType == "CORS")
            {
                sql = "DELETE FROM \"ClientCorsOrigins\" WHERE \"Id\" = CAST(@Id AS INT);";

                paramList = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("Id", id)
                        };
            }
            else if (uriType == "Post")
            {
                sql = "DELETE FROM \"ClientPostLogoutRedirectUris\" WHERE \"Id\" = CAST(@Id AS INT);";

                paramList = new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("Id", id)
                        };
            }

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");

            this.toastNotification.AddSuccessToastMessage("Client Redirect URI is removed");

            return RedirectToAction("ClientRedirectUris", "ClientManager", new { id = clientId });
        }

        [StudioExceptionFilter()]
        public IActionResult ClientScopes(string id)
        {
            string resourceSql = "SELECT \"Name\" FROM public.\"IdentityResources\" UNION SELECT \"Name\" FROM public.\"ApiScopes\" ORDER BY \"Name\";";

            DataSet resourceData = DataServices.DataSetFromSQL(resourceSql, null, "connectionString_SynapseIdentityStore");
            ViewBag.identityResources = ToSelectList(resourceData.Tables[0], "Name", "Name");

            string sql = "SELECT \"Id\", \"Scope\", \"ClientId\" FROM public.\"ClientScopes\" WHERE \"ClientId\" = CAST(@ClientId AS INT) ORDER BY \"Id\";";
            var paramList = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("ClientId", id)
                };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");
            ViewBag.clientScopes = ds.Tables[0];
            ViewBag.ClientId = GetClientId(id);
            ViewBag.id = id;

            return View();
        }

        [StudioExceptionFilter()]
        public ActionResult AddNewClientScope(string scope, string clientId)
        {
            string sql = "SELECT * FROM \"ClientScopes\" WHERE \"ClientId\" = CAST(@ClientId AS INT) AND \"Scope\" = @Scope";

            var param = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("ClientId", clientId),
                    new KeyValuePair<string, string>("Scope", scope)
                };

            DataSet ds = DataServices.DataSetFromSQL(sql, param, "connectionString_SynapseIdentityStore");
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                this.toastNotification.AddErrorToastMessage("Client Scope already exists");
            }
            else
            {
                string insertSql = "INSERT INTO \"ClientScopes\" (\"Scope\", \"ClientId\") VALUES(@Scope, CAST(@ClientId AS INT));";

                var paramList = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("Scope", scope),
                        new KeyValuePair<string, string>("ClientId", clientId)
                    };

                DataServices.ExcecuteNonQueryFromSQL(insertSql, paramList, "connectionString_SynapseIdentityStore");

                this.toastNotification.AddSuccessToastMessage("New Client Scope is added");
            }

            return RedirectToAction("ClientScopes", "ClientManager", new { id = clientId });
        }

        [StudioExceptionFilter()]
        public ActionResult RemoveClientScope(string id, string clientId)
        {
            string sql = "DELETE FROM \"ClientScopes\" WHERE \"Id\" = CAST(@Id AS INT);";

            var paramList = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("Id", id)
                    };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");

            this.toastNotification.AddSuccessToastMessage("Client Scope is removed");

            return RedirectToAction("ClientScopes", "ClientManager", new { id = clientId });
        }

        [StudioExceptionFilter()]
        public IActionResult ClientSecrets(string id)
        {
            string sql = "SELECT \"Id\", \"Type\", \"Value\", \"ClientId\" FROM public.\"ClientSecrets\" WHERE \"ClientId\" = CAST(@ClientId AS INT) ORDER BY \"Id\";";

            var paramList = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("ClientId", id)
                };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");
            ViewBag.clientSecrets = ds.Tables[0];
            ViewBag.ClientId = GetClientId(id);
            ViewBag.id = id;

            return View();
        }

        [StudioExceptionFilter()]
        public ActionResult AddNewClientSecret(string secretValue, string secretType, string clientId)
        {
            string sql = "INSERT INTO \"ClientSecrets\" (\"Value\", \"Type\", \"ClientId\") VALUES(@Value, @Type, CAST(@ClientId AS INT));";

            string hashedPassword = string.Empty;

            using (SHA256 shA256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(secretValue);
                hashedPassword = Convert.ToBase64String(((HashAlgorithm)shA256).ComputeHash(bytes));
            }

            var paramList = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Value", hashedPassword),
                    new KeyValuePair<string, string>("Type", secretType),
                    new KeyValuePair<string, string>("ClientId", clientId)
                };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");

            this.toastNotification.AddSuccessToastMessage("New Client Secret is added");

            return RedirectToAction("ClientSecrets", "ClientManager", new { id = clientId });
        }

        [StudioExceptionFilter()]
        public ActionResult RemoveClientSecret(string id, string clientId)
        {
            string sql = "DELETE FROM \"ClientSecrets\" WHERE \"Id\" = CAST(@Id AS INT);";

            var paramList = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("Id", id)
                    };

            DataServices.ExcecuteNonQueryFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");

            this.toastNotification.AddSuccessToastMessage("Client Secret is removed");

            return RedirectToAction("ClientSecrets", "ClientManager", new { id = clientId });
        }

        [NonAction]
        private DataTable LoadClientgrandtypes(string id)
        {
            string sql = "SELECT \"Id\", \"GrantType\", \"ClientId\" FROM public.\"ClientGrantTypes\" WHERE \"ClientId\" = CAST(@ClientId AS INT) ORDER BY \"Id\";";

            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("ClientId", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList, "connectionString_SynapseIdentityStore");

            return ds.Tables[0];
        }

        [NonAction]
        private string GetClientId(string id)
        {
            string sql = "SELECT \"ClientId\" FROM public.\"Clients\" WHERE \"Id\" = CAST(@Id AS INT);";
            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Id", id)
            };

            return DataServices.ExecuteScalar(sql, paramList, "connectionString_SynapseIdentityStore");
        }

        [NonAction]
        private SelectList ToSelectList(DataTable table, string valueField, string textField)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            foreach (DataRow row in table.Rows)
            {
                list.Add(new SelectListItem()
                {
                    Text = row[textField].ToString(),
                    Value = row[valueField].ToString()
                });
            }

            return new SelectList(list, "Value", "Text");
        }
    }
}