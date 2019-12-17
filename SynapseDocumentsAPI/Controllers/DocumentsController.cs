//Interneuron Synapse

//Copyright(C) 2019  Interneuron CIC

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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> GeneratePdfDocument([FromBody] DocumentData documentData)
        {
            string path = _configuration.GetSection("AppSettings:ChromeExecutablePath").Value;
            //await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
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
                    await page.GoToAsync("data:text/html," + documentData.PdfBodyHTML, WaitUntilNavigation.Networkidle0);
                    await page.AddStyleTagAsync(new AddTagOptions { Url = documentData.PdfCssUrl });

                    await page.EvaluateExpressionAsync("window.scrollBy(0, window.innerHeight);");

                    byte[] pdfFile = await page.PdfDataAsync(new PdfOptions {
                        MarginOptions = new MarginOptions {
                            Top = "2.54cm",
                            Left = "2.54cm",
                            Bottom = "2.54cm",
                            Right = "2.54cm"
                        },
                        PrintBackground = true
                    });

                    return File(pdfFile, "application/pdf");
                }
            }
        }
    }
}
