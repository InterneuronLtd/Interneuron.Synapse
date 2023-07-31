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
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using SynapseStudioWeb.Helpers;
using Microsoft.AspNetCore.Authorization; 
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using Microsoft.AspNetCore.Http;

namespace SynapseStudioWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {       
        public IActionResult Index()
        {
            try
            {
                string coreSQL = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid =  'd8851db1-68f8-45ee-be9a-628666512431' ORDER BY entityname; ";
                var paramList = new List<KeyValuePair<string, string>>()
                {
                };
                DataSet dsCore = DataServices.DataSetFromSQL(coreSQL, paramList);
                DataTable dtCore = dsCore.Tables[0];
                List<CoreEntityModel> CoreEntityModel = dtCore.ToList<CoreEntityModel>();

                string extendedSQL = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid =  'e8b78b52-641d-46eb-bb8b-16e2feb86fe7' ORDER BY entityname; ";
                DataSet dsExtended = DataServices.DataSetFromSQL(extendedSQL, paramList);
                DataTable dtExtended = dsExtended.Tables[0];
                List<ExtendedEntityModel> ExtendedEntityModel = dtExtended.ToList<ExtendedEntityModel>();

                string LocalSQL = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid =  '468ac87d-f6a3-4d20-8800-58742b8952b6' ORDER BY entityname; ";
                DataSet dsLocal = DataServices.DataSetFromSQL(LocalSQL, paramList);
                DataTable dtLocal = dsLocal.Tables[0];
                List<LocalEntityModel> LocalEntityModel = dtLocal.ToList<LocalEntityModel>();

                string metaSQL = "SELECT * FROM entitysettings.entitymanager WHERE synapsenamespaceid =  '76ce099d-0ab1-40c5-8b37-d0933b84ec8c' ORDER BY entityname; ";
                DataSet dsMeta = DataServices.DataSetFromSQL(metaSQL, paramList);
                DataTable dtMeta = dsMeta.Tables[0];
                List<MetaEntityModel> MetaEntityModel = dtMeta.ToList<MetaEntityModel>();

                EntityModel entityModel = new EntityModel();
                entityModel.CoreEntityModel = CoreEntityModel;
                entityModel.ExtendedEntityModel = ExtendedEntityModel;
                entityModel.LocalEntityModel = LocalEntityModel;
                entityModel.MetaEntityModel = MetaEntityModel;

                return View(entityModel);
            }
            catch(Exception ex)
            {
                Log.Error(ex, ex.Message);
                return View();
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            string errorID = Guid.NewGuid().ToString();

            var userName = HttpContext.Session.GetString(SynapseSession.UserID) ?? "UNKNOWNUSER";
            // (context != null && context.User != null && context.User.Identity != null && context.User.Identity.Name != null) ? context.User.Identity.Name : "StudioWeb User";

            var userClaimDetails = HttpContext.Session.GetString(SynapseSession.UserClaims) ?? null;

            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            try
            {
                Log.Logger.ForContext("ErrorId", errorID).ForContext("UserName", userName).ForContext("ClientDetails", userClaimDetails).Error(exceptionHandlerPathFeature.Error, exceptionHandlerPathFeature.Error.Message);
            }
            catch
            { }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier, ErrorId = errorID });
        }
    }
}
