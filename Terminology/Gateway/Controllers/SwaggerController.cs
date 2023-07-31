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
﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Interneuron.CareRecord.API.Gateway.Controllers
{
    public class SwaggerController : Controller
    {
        private IConfiguration _configuration;

        public SwaggerController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public IActionResult Index()
        {
            var ServicesSwaggerSection = this._configuration.GetSection("ServicesSwagger");
            var urls = ServicesSwaggerSection.GetSection("Urls").Get<string[]>();

            ViewData["SwaggerUrls"] = urls;// "https://localhost:44356/swagger";
            return View();
        }
    }
}