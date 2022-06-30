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


﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class RoleManagerController : Controller
    {
        public IActionResult NewRole()
        {
            return View();
        }
        public IActionResult AddUser()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateUser(LocalUser model)
        {
            string userId = Guid.NewGuid().ToString();

            string hashedPassword = string.Empty;

            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            IdentityUser user = new IdentityUser(model.UserName);
            hashedPassword = passwordHasher.HashPassword(user, model.Password);
             
            string sql = "INSERT INTO \"AspNetUsers\"(\"Id\", \"UserName\", \"NormalizedUserName\", \"Email\", \"NormalizedEmail\", \"EmailConfirmed\", \"PasswordHash\", \"SecurityStamp\", \"ConcurrencyStamp\", \"PhoneNumberConfirmed\",                    \"TwoFactorEnabled\", \"LockoutEnabled\", \"AccessFailedCount\") "
                           + " VALUES(@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, false, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, false, false, false, 0)";

            var paramList = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Id", userId),
                    new KeyValuePair<string, string>("Username", model.UserName),
                    new KeyValuePair<string, string>("NormalizedUserName",  model.UserName.ToUpper()),
                    new KeyValuePair<string, string>("Email",  model.Email),
                    new KeyValuePair<string, string>("NormalizedEmail",  model.Email.ToUpper()),
                    new KeyValuePair<string, string>("PasswordHash", hashedPassword),
                    new KeyValuePair<string, string>("SecurityStamp", Guid.NewGuid().ToString()),
                    new KeyValuePair<string, string>("ConcurrencyStamp", Guid.NewGuid().ToString())
                };

            DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");

            string query = "INSERT INTO \"AspNetUserClaims\" (\"UserId\", \"ClaimType\", \"ClaimValue\") " +
                           "VALUES(@UserId, @ClaimType, @ClaimValue), " +
                           "(@UserId, @ClaimType1, @ClaimValue1), " +
                           "(@UserId, @ClaimType2, @ClaimValue2), " +
                           "(@UserId, @ClaimType3, @ClaimValue3), " +
                           "(@UserId, @ClaimType4, @ClaimValue4);";

            var parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("UserId", userId),
                    new KeyValuePair<string, string>("ClaimType", "name"),
                    new KeyValuePair<string, string>("ClaimValue", model.FirstName + ' ' + model.LastName),
                    new KeyValuePair<string, string>("ClaimType1", "given_name"),
                    new KeyValuePair<string, string>("ClaimValue1", model.FirstName),
                    new KeyValuePair<string, string>("ClaimType2", "family_name"),
                    new KeyValuePair<string, string>("ClaimValue2", model.LastName),
                    new KeyValuePair<string, string>("ClaimType3", "email"),
                    new KeyValuePair<string, string>("ClaimValue3", model.Email),
                    new KeyValuePair<string, string>("ClaimType4", "email_verified"),
                    new KeyValuePair<string, string>("ClaimValue4", "true")
                };

            DataServices.executeSQLStatement(query, parameters, "connectionString_SynapseIdentityStore");
            return RedirectToAction("SecurityManager", "Security");
        }

        [HttpPost]
        public ActionResult CreateRole(IFormCollection collection)
        {
            var RoleName = collection["RoleName"];
            string sql = "INSERT INTO \"AspNetRoles\"(\"Id\", \"Name\", \"NormalizedName\") VALUES (@id, @name, @nname);";

            string id = System.Guid.NewGuid().ToString();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id),
                new KeyValuePair<string, string>("name",RoleName),
                new KeyValuePair<string, string>("nname", RoleName.ToString().ToUpper())
            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("SecurityManager", "Security");
        }
        public IActionResult ManageRole(string id)
        {
            InitViewBag(id, new DataTable(), LoadUsersInRole(id));
            return View();
        }

        public ActionResult SearchLogin(IFormCollection collection)
        {
            var Logintext = collection["Logintext"];
            var roleid = collection["roleid"];
            string sql = "SELECT \"Id\", \"UserName\" FROM \"AspNetUsers\" where \"Id\" not in (select \"UserId\" from \"AspNetUserLogins\" ) and \"UserName\" like '" + Logintext.ToString() + "%'  ;";
            DataSet ds = DataServices.DataSetFromSQL(sql, null, "connectionString_SynapseIdentityStore");

            InitViewBag(roleid, ds.Tables[0], LoadUsersInRole(roleid.ToString()));
            ViewBag.SearchLogin = ds.Tables[0];
            ViewBag.Userroles = LoadUsersInRole(roleid.ToString());
            ViewBag.roleid = roleid;
            return View("ManageRole", new { id = roleid });
            
            //return RedirectToAction("ManageRole", "RoleManager", new { id = roleid });
        }
        private void InitViewBag(string id, DataTable searchLoginData, DataTable usersInRoleData)
        {
            ViewBag.SearchLogin = new DataTable();
            ViewBag.Userroles = LoadUsersInRole(id);
            ViewBag.roleid = id;
        }

        public DataTable LoadUsersInRole(string id)
        {
            string sql = "select \"UserName\", u.\"Id\" from \"AspNetUsers\" u inner join \"AspNetUserRoles\" ur on ur.\"UserId\" = u.\"Id\" and ur.\"RoleId\" = cast('" + id + "' as text)";
            DataSet ds = DataServices.DataSetFromSQL(sql, null, "connectionString_SynapseIdentityStore");
            return  ds.Tables[0];          
        }


        public IActionResult AddRole(string id,string roleid)
        {
            string sql = "insert into  \"AspNetUserRoles\" (\"UserId\",\"RoleId\") values (@userid,@roleid) ";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("userid",id),
                new KeyValuePair<string, string>("roleid",roleid)
            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList,  "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {
               
            }
            return RedirectToAction("ManageRole", "RoleManager", new { id = roleid });
        }

        public IActionResult RemoveRole(string id, string roleid)
        {
            string sql = "delete from  \"AspNetUserRoles\" where \"UserId\" = @userid and \"RoleId\" = @roleid;";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("userid",id),
                new KeyValuePair<string, string>("roleid",roleid)
            };
            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {

            }

            //return View("ManageRole", new { @id = roleid });
            return RedirectToAction("ManageRole", "RoleManager", new { id = roleid });
        }

        public IActionResult ManageRoleScope(string id)
        {
            string sql = " SELECT ir.\"Name\" , ir.\"Id\"  FROM \"ApiScopes\" ir";
            DataSet ds = DataServices.DataSetFromSQL(sql,null, "connectionString_SynapseIdentityStore");
            DataTable dt = ds.Tables[0];
            ViewBag.rolescope = ToSelectList(dt, "Id", "Name");
            ViewBag.ApiScopes = LoadScopesInRole(id);
            ViewBag.roleid = id;
            return View();
        }
        protected DataTable LoadScopesInRole(string id)
        {
            string sql = "select \"Name\", ap.\"Id\", u.\"Description\" from \"ApiScopes\" u inner join \"ApiScopePermissions\" ap on ap.\"ApiScopeId\" = u.\"Id\" where ap.\"RoleId\" = cast('" + id + "' as text)";
          
            DataSet ds = DataServices.DataSetFromSQL(sql, null, "connectionString_SynapseIdentityStore");
            DataTable dt = ds.Tables[0];

            return  dt;
           
        }
        public ActionResult MappRoleScope(IFormCollection collection)
        {
            var userid = collection["RoleScope"];
            var roleid = collection["roleid"];
            string sql = "insert into  \"ApiScopePermissions\" (\"ApiScopeId\",\"RoleId\",\"Id\") values (cast(@userid as int),@roleid ,@id) ";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("userid",userid ),
                  new KeyValuePair<string, string>("roleid", roleid),
                    new KeyValuePair<string, string>("id", Guid.NewGuid().ToString())
                            };
            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {
              
            }
            return RedirectToAction("ManageRoleScope", "RoleManager", new { id = roleid });
        }
     
        [NonAction]
        public SelectList ToSelectList(DataTable table, string valueField, string textField)
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

        public IActionResult RemoveScopeRole(string id, string roleid)
        {
            string sql = "delete from  \"ApiScopePermissions\" where \"Id\" = @id ";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("id", id)
                            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList,  "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {
               
            }

            return RedirectToAction("ManageRoleScope", "RoleManager", new { id = roleid });
        }


        public IActionResult ManageExternalusers(string id)
        {
            string sql = " SELECT ir.\"Name\" , ir.\"Id\"  FROM \"ApiScopes\" ir";
            DataSet ds = DataServices.DataSetFromSQL(sql, null, "connectionString_SynapseIdentityStore");
            DataTable dt = ds.Tables[0];
            ViewBag.rolescope = ToSelectList(dt, "Id", "Name");
            ViewBag.ExpernalUsers = LoadExterusermapped(id);
            ViewBag.roleid = id;
            return View("ManageExternalusers");
        }
        protected DataTable LoadExterusermapped(string id)
        {
            string sql = "select \"ExternalSubjectId\", u.\"Id\", u.\"Idp\" from \"UserRoles_ExternalProviders\" u where u.\"RoleId\" = cast('" + id + "' as text)";

            DataSet ds = DataServices.DataSetFromSQL(sql, null, "connectionString_SynapseIdentityStore");
            DataTable dt = ds.Tables[0];

            return dt;

        }

        public ActionResult AddExternaluser(IFormCollection collection)
        {
            var ExternalUser = collection["ExternalUser"];
            var provider = collection["provider"];
            var roleid = collection["roleid"];
            string sql = "insert into  \"UserRoles_ExternalProviders\" (\"ExternalSubjectId\",\"RoleId\",\"Idp\",\"Id\") values (@userid,@roleid, @idp,@id) ";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("userid",ExternalUser),
                  new KeyValuePair<string, string>("roleid",roleid),
                   new KeyValuePair<string, string>("idp",provider),
                    new KeyValuePair<string, string>("id", Guid.NewGuid().ToString())
                            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {

            }
            return RedirectToAction("ManageExternalusers", new { id = roleid });
        }

        public IActionResult RemoveExternaluser(string id, string roleid)
        {
            string sql = "delete from  \"UserRoles_ExternalProviders\" where \"Id\" = @id ";

            var paramList = new List<KeyValuePair<string, string>>() {

                new KeyValuePair<string, string>("id",id)
                            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList, "connectionString_SynapseIdentityStore");
            }
            catch (Exception ex)
            {

            }

            //return ManageExternalusers(roleid);
            return RedirectToAction("ManageExternalusers" , new { id = roleid });
        }
    }
}