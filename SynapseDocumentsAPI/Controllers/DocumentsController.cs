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


﻿

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using SynapseDocumentsAPI.Model;

namespace SynapseDocumentsAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private IConfiguration _configuration;

        public DocumentsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("[Action]")]
        public string GetData()
        {
            return "Hello";
        }

        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> GeneratePdfDocument([FromBody] DocumentData documentData)
        {
            string path = _configuration.GetSection("AppSettings:ChromeExecutablePath").Value;

            try
            {
                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    Headless = true,
                    ExecutablePath = path,
                    IgnoreHTTPSErrors = true
                }))
                {
                    using (var page = await browser.NewPageAsync())
                    {
                        //await page.SetContentAsync(documentData.PdfBodyHTML);

                        Dictionary<string, string> headers = new Dictionary<string, string>();
                        headers.Add("Accept-Charset", "utf-8");
                        headers.Add("Content-Type", "text/html; charset=utf-8");

                        var response = await page.GoToAsync("data:text/html," + documentData.PdfBodyHTML, WaitUntilNavigation.Networkidle0);

                        await page.SetContentAsync(Encoding.UTF8.GetString(await response.BufferAsync()));

                        await page.AddStyleTagAsync(new AddTagOptions { Url = documentData.PdfCssUrl });
                        
                        await page.SetExtraHttpHeadersAsync(headers);

                        await page.EvaluateExpressionAsync("window.scrollBy(0, window.innerHeight);");

                        MarginOptions marginOption;
                        if (documentData.DocumentMargin == null)
                        {
                            marginOption = new MarginOptions
                            {
                                Top = _configuration.GetSection("AppSettings:Margin:Top").Value,
                                Bottom = _configuration.GetSection("AppSettings:Margin:Bottom").Value,
                                Left = _configuration.GetSection("AppSettings:Margin:Left").Value,
                                Right = _configuration.GetSection("AppSettings:Margin:Right").Value
                            };
                        }
                        else
                        {
                            marginOption = documentData.DocumentMargin;
                        }


                        byte[] pdfFile = await page.PdfDataAsync(new PdfOptions
                        {
                            MarginOptions = marginOption,
                            PrintBackground = true,
                            DisplayHeaderFooter = true,
                            HeaderTemplate = documentData.PdfHeaderHTML,
                            FooterTemplate = documentData.PdfFooterHTML
                        });

                        return File(pdfFile, "application/pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
