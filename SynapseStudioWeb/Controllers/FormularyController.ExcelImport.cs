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
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExcelDataReader;
using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.DataService.APIModel;

namespace SynapseStudioWeb.Controllers
{
    public partial class FormularyController : Controller
    {
        [HttpGet]
        public IActionResult ImportFile()
        {
            return View("FormularyExcelImport");
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        [RequestSizeLimit(209715200)]
        public async Task<IActionResult> ExcelImport()
        {
            string token = HttpContext.Session.GetString("access_token");

            int.TryParse(_configuration["SynapseCore:Settings:FileImportBatchSize"], out int batSizeForFileFromConfig);

            var retries = 0;

            var responseStringBuilder = new StringBuilder();

            IFormFile xlFile = Request.Form.Files[0];

            var colmsConfigured = GetColumnDetailsFromConfig;

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var xlFileXtn = Path.GetExtension(xlFile.FileName).ToLower();

            if (xlFileXtn != ".xls" && xlFileXtn != ".xlsx") return StatusCode((int)HttpStatusCode.NotAcceptable);

            //var dirPath = CreateDirectory();

            if (xlFile.Length > 0)
            {
                //string fullPath = Path.Combine(dirPath, xlFile.FileName);

                //using (var stream = System.IO.File.Create(fullPath))
                //{
                //    await xlFile.CopyToAsync(stream);
                //}
                var requests = new List<FormularyHeaderAPIModel>();

                using (var reader = ExcelReaderFactory.CreateReader(xlFile.OpenReadStream()))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true,
                        }
                    }).Tables[0];// get the first sheet data with index 0.

                    var rowData = GetRowData(colmsConfigured);

                    //var totalBatches = (result.Rows.Count) / 500;

                    //List<int> source = Enumerable.Range(1, 19).ToList();
                    //int batchsize = 10;
                    //List<List<int>> batches = new List<List<int>>();
                    //for (int i = 0; i < source.Count; i += batchsize)
                    //{
                    //    var batch = source.Skip(i).Take(batchsize);
                    //    batches.Add(batch.ToList());
                    //}

                    var batchsize = batSizeForFileFromConfig;

                    var batchedRequests = new List<List<FormularyHeaderAPIModel>>();

                    for (var rowIndex = 0; rowIndex < result.Rows.Count; rowIndex++)
                    {
                        var row = result.Rows[rowIndex];

                        if (!CanProcessRow(row, colmsConfigured)) continue;

                        var requestRow = new FormularyHeaderAPIModel
                        {
                            Detail = new FormularyDetailAPIModel(),
                            FormularyRouteDetails = new List<FormularyRouteDetailAPIModel>()
                        };

                        colmsConfigured.Keys.Each(k =>
                        {
                            if (rowData.ContainsKey(k))
                                rowData[k](row, requestRow);
                        });

                        requests.Add(requestRow);
                    }

                    for (var reqIndex = 0; reqIndex < requests.Count; reqIndex += batchsize)
                    {
                        var batches = requests.Skip(reqIndex).Take(batchsize);
                        batchedRequests.Add(batches.ToList());
                    }

                    //for (var rowIndex = 0; rowIndex < result.Rows.Count; rowIndex++)
                    //for (var rowIndex = 0; rowIndex < result.Rows.Count; rowIndex += batchsize)
                    //{
                    //    var requests = new List<CreateFormularyAPIRequest>();

                    //    var row = result.Rows[rowIndex];

                    //    if (!CanProcessRow(row, colmsConfigured)) continue;

                    //    var requestRow = new CreateFormularyAPIRequest
                    //    {
                    //        Detail = new CreateFormularyDetailAPIRequest(),
                    //        FormularyRouteDetails = new List<CreateFormularyRouteDetailAPIRequest>()
                    //    };

                    //    colmsConfigured.Keys.Each(k =>
                    //    {
                    //        if (rowData.ContainsKey(k))
                    //            rowData[k](row, requestRow);
                    //    });

                    //    requests.Add(requestRow);

                    //    batchedRequests.Add(requests);

                    //    //var response = await DataService.TerminologyAPIService.FileImportMedication(requests, token);


                    //    //FormatResponseMessage(response, requestRow, responseStringBuilder);
                    //}

                    foreach (var batchReq in batchedRequests)
                    {
                        retries = 0;
                        try
                        {
                            var response = await DataService.TerminologyAPIService.FileImportMedication(batchReq, token);
                            FormatResponseMessageForBulk(response, responseStringBuilder);
                            await Task.Delay(200);

                            if (response.StatusCode == DataService.APIModel.StatusCode.Fail)
                                await RetryPosting(batchReq);
                        }
                        catch (Exception ex)
                        {
                            if (retries == 0)
                            {
                                await RetryPosting(batchReq);
                            }
                            else
                            {
                                throw ex;
                            }
                        }
                    }
                }

                async Task RetryPosting(List<FormularyHeaderAPIModel> batchReq)
                {
                    if (retries >= 3) throw new Exception("Exhausted re-tries during formulary file import");

                    try
                    {
                        retries++;
                        var response = await DataService.TerminologyAPIService.FileImportMedication(batchReq, token);
                        FormatResponseMessageForBulk(response, responseStringBuilder);

                        await Task.Delay(200);

                        if (response.StatusCode == DataService.APIModel.StatusCode.Fail)
                            await RetryPosting(batchReq);

                    }
                    catch (Exception ex)
                    {
                        if (retries <= 2)
                        {
                            await RetryPosting(batchReq);
                        }
                        else
                        {
                            throw ex;
                        }
                    }

                }

                var codesInExcel = new List<string>();

                if (requests.IsCollectionValid())
                {
                    codesInExcel = requests.Select(rec => rec.Code).ToList();
                }

                await UploadNonFormulariesInDMD(codesInExcel, token, responseStringBuilder);

                await DataService.TerminologyAPIService.InvokePostImportProcess(token);

            }

            //return Ok("Uploaded successfully");
            return Json(responseStringBuilder.ToString());
        }

        public async Task<IActionResult> UploadAllDMDToFormulary()
        {
            string token = HttpContext.Session.GetString("access_token");

            var response = await DataService.TerminologyAPIService.ImportAllMedsFromDMDWithRules(token);

            if (response == null || response.StatusCode == DataService.APIModel.StatusCode.Fail)
            {
                _toastNotification.AddErrorToastMessage("Error Importing DMD data to MMC System.");
                return Json("0");
            }

            return Json("1");
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 2097152000)]
        [RequestSizeLimit(2097152000)]
        public async Task<IActionResult> SyncDMDUsingFile()
        {
            var statusCode = 1;
            var processMsg = "";
            var processErrorMsg = "";
            try
            {
                var file = Request.Form.Files[0];

                var syncMode = Request.Form["syncMode"].ToString();

                if (file == null || file.Length == 0)
                {
                    _toastNotification.AddErrorToastMessage("file not selected");
                    return Json("file not selected");
                }

                if (string.Compare(Path.GetExtension(file.FileName), ".zip", true) != 0)
                {
                    _toastNotification.AddErrorToastMessage("Upload the DMD Zip file downloaded from TRUD");
                    return Json("Upload the DMD Zip file downloaded from TRUD");
                }

                if (!file.FileName.StartsWith("nhsbsa_dmd_", StringComparison.OrdinalIgnoreCase))
                {
                    _toastNotification.AddErrorToastMessage("Upload the correct DMD file downloaded from TRUD");
                    return Json("Upload the correct DMD file downloaded from TRUD");
                }

                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\DMDUploads");

                var pathWithFileName = Path.Combine(uploadDir, file.FileName);

                using (var stream = new FileStream(pathWithFileName, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                UnzipFile(uploadDir, file.FileName);

                //new TaskFactory().StartNew(() => ProcessDMDFile(uploadDir, file.FileName));
                (statusCode, processMsg, processErrorMsg) = await ProcessDMDFile(uploadDir, file.FileName, syncMode);
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.ToString());
                return Json(new { statusCd = 0, processText = processMsg, processError = processErrorMsg });
            }

            return Json(new { statusCd = statusCode, processText = processMsg, processError = processErrorMsg });
        }

        private void UnzipFile(string uploadDir, string fileName)
        {
            var pathWithFileName = Path.Combine(uploadDir, fileName);
            uploadDir = Path.Combine(uploadDir, Path.GetFileNameWithoutExtension(fileName));
            ZipFile.ExtractToDirectory(pathWithFileName, uploadDir, true);
        }

        private async Task<(int status, string processText, string processError)> ProcessDMDFile(string uploadDir, string fileName, string syncMode = "auto")
        {
            var dmdVersion = GetDMDVersion(fileName);

            var dmdFilePath = $@"{ Path.Combine(uploadDir, Path.GetFileNameWithoutExtension(fileName))}";
            dmdFilePath = dmdFilePath.Replace(@"\", "/");

            var dmdDb = _configuration["MMCSyncDMDDBConfig:dmdDb"];
            var dmdServer = _configuration["MMCSyncDMDDBConfig:dmdServer"];
            var dmdPort = _configuration["MMCSyncDMDDBConfig:dmdPort"]; ;
            var dmdSchema = _configuration["MMCSyncDMDDBConfig:dmdSchema"];
            var dmdUId = _configuration["MMCSyncDMDDBConfig:dmdUId"];
            var dmdPassword = _configuration["MMCSyncDMDDBConfig:dmdPassword"];
            var dmdStgDb = _configuration["MMCSyncDMDDBConfig:dmdStgDb"];
            var dmdStgServer = _configuration["MMCSyncDMDDBConfig:dmdStgServer"];
            var dmdStgPort = _configuration["MMCSyncDMDDBConfig:dmdStgPort"];
            var dmdStgSchema = _configuration["MMCSyncDMDDBConfig:dmdStgSchema"];
            var dmdStgUId = _configuration["MMCSyncDMDDBConfig:dmdStgUId"];
            var dmdStgPassword = _configuration["MMCSyncDMDDBConfig:dmdStgPassword"];

            var batchFileDir = Path.Combine(Directory.GetCurrentDirectory(), @"ETLJobs\DMDDeltaProcessor\dmd_delta_processor\dmd_delta_processor", "dmd_delta_processor_run.bat");

            var batArgs = $"--context_param dmd_version=\"{dmdVersion}\" --context_param dmd_db_additionalparams=  --context_param dmd_db_host=\"{dmdServer}\" --context_param dmd_db_name=\"{dmdDb}\" --context_param dmd_db_password=\"{dmdPassword}\" --context_param dmd_db_port={dmdPort} --context_param dmd_db_psql_path=  --context_param dmd_db_pwd_string=\"{dmdPassword}\" --context_param dmd_db_schema=\"{dmdSchema}\" --context_param dmd_db_script_path= --context_param dmd_db_user=\"{dmdUId}\" --context_param dmd_file_path=\"{dmdFilePath}\" --context_param dmd_db_stg_additionalparams=  --context_param dmd_db_stg_host=\"{dmdStgServer}\" --context_param dmd_db_stg_name=\"{dmdStgDb}\" --context_param dmd_db_stg_password=\"{dmdStgPassword}\" --context_param dmd_db_stg_port={dmdStgPort} --context_param dmd_db_stg_pwd_string=\"{dmdStgPassword}\" --context_param dmd_db_stg_schema=\"{dmdStgSchema}\" --context_param dmd_db_script_path= --context_param dmd_db_stg_user=\"{dmdStgUId}\"";


            //No need to execute the batch file
            //batchFileDir = Path.Combine(Directory.GetCurrentDirectory(), @"ETLJobs\DMDDeltaProcessor\dmd_delta_process.bat");
            //var command = $"dmd_delta_processor_run.bat {batArgs}";

            var psi = new ProcessStartInfo(batchFileDir)
            //var psi = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                //WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"ETLJobs\DMDDeltaProcessor\dmd_delta_processor\dmd_delta_processor"),
                Arguments = batArgs,//dmdFilePath,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = false,
                //CreateNoWindow = true
            };

            var batchProcess = new Process();

            var consoleOutput = new StringBuilder();
            var errorOutput = new StringBuilder();

            ////Debuggging only or can be for realtime later
            batchProcess.OutputDataReceived += (s, d) =>
            {
                consoleOutput.AppendLine(d.Data);
            };

            batchProcess.ErrorDataReceived += (s, d) =>
            {
                errorOutput.AppendLine(d.Data);
            };

            batchProcess.StartInfo = psi;
            batchProcess.Start();

            batchProcess.BeginOutputReadLine();
            batchProcess.BeginErrorReadLine();

            var status = 0;

            batchProcess.WaitForExit();

            if (batchProcess.HasExited)
            {
                var exitCode = batchProcess.ExitCode;

                if (string.Compare(syncMode, "auto", true) == 0 && exitCode == 0)
                {
                    status = await ImportDeltasToFormulary();
                }
            }

            batchProcess.Close();

            return (status, consoleOutput.ToString(), errorOutput.ToString());
        }

        private void BatchProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async Task<int> ImportDeltasToFormulary()
        {
            string token = HttpContext.Session.GetString("access_token");

            var response = await TerminologyAPIService.ImportDeltas(token);

            if (response == null || response.StatusCode == DataService.APIModel.StatusCode.Fail)
            {
                _toastNotification.AddErrorToastMessage("Error Importing DMD data to MMC System.");
                return 0;
            }

            return 1;
        }

        private string GetDMDVersion(string fileName)
        {
            var allVers = Regex.Split(fileName, @"[^0-9\.]+");

            if (allVers.IsCollectionValid())
            {
                var versionNo = allVers.FirstOrDefault(rec => rec.IsNotEmpty());
                return versionNo;
            }

            return "";
        }

        private async Task UploadNonFormulariesInDMD(List<string> codesInExcel, string token, StringBuilder responseStringBuilder)
        {
            int.TryParse(_configuration["SynapseCore:Settings:DMDCodeBatchSize"], out int batSizeForDMDFromConfig);

            var retries = 0;

            var allCodesInDMDResponse = await DataService.TerminologyAPIService.GetAllDMDCodes(token);

            if (allCodesInDMDResponse.StatusCode != DataService.APIModel.StatusCode.Success || !allCodesInDMDResponse.Data.IsCollectionValid()) return;

            var nonFormularyCodes = allCodesInDMDResponse.Data.Distinct().Except(codesInExcel).ToList();

            var batchsize = batSizeForDMDFromConfig;

            var batchedRequests = new List<List<string>>();

            for (var reqIndex = 0; reqIndex < nonFormularyCodes.Count; reqIndex += batchsize)
            {
                var batches = nonFormularyCodes.Skip(reqIndex).Take(batchsize);
                batchedRequests.Add(batches.ToList());
            }

            foreach (var batchReq in batchedRequests)
            {
                retries = 0;
                try
                {
                    var response = await DataService.TerminologyAPIService.ImportMeds(batchReq, token, "002", "003");//Non-formulary and Active
                    //FormatResponseMessageForBulk(response, responseStringBuilder);
                    await Task.Delay(200);

                    if (response.StatusCode == DataService.APIModel.StatusCode.Fail)
                        await RetryPosting(batchReq);
                }
                catch (Exception ex)
                {
                    if (retries == 0)
                    {
                        await RetryPosting(batchReq);
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }

            async Task RetryPosting(List<string> batchReq)
            {
                if (retries >= 3) throw new Exception("Exhausted re-tries during formulary file import");

                try
                {
                    retries++;
                    var response = await DataService.TerminologyAPIService.ImportMeds(batchReq, token, "002", "003");//Non-formulary and Active
                    //FormatResponseMessageForBulk(response, responseStringBuilder);
                    await Task.Delay(200);

                    if (response.StatusCode == DataService.APIModel.StatusCode.Fail)
                        await RetryPosting(batchReq);
                }
                catch (Exception ex)
                {
                    if (retries <= 2)
                    {
                        await RetryPosting(batchReq);
                    }
                    else
                    {
                        throw ex;
                    }
                }

            }
        }

        private void FormatResponseMessageForBulk(TerminologyAPIResponse<CreateFormularyAPIResponse> response, StringBuilder responseStringBuilder)
        {
            if (response.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                responseStringBuilder.AppendLine($"{string.Join('\n', response.ErrorMessages.ToArray())} \n");
            }

            else if (response.StatusCode == DataService.APIModel.StatusCode.Success)
            {
                if (response.Data.Status != null && response.Data.Status.ErrorMessages.IsCollectionValid())
                {
                    var errors = string.Join('\n', response.Data.Status.ErrorMessages.ToArray());

                    responseStringBuilder.AppendLine($"{errors} \n");
                }
                else
                {
                    if (response.Data != null && response.Data.Data != null)
                    {
                        response.Data.Data.Each(res =>
                        {
                            responseStringBuilder.AppendLine($"{res.Code}-Success \n");
                        });
                    }
                }
            }
        }

        private bool CanProcessRow(DataRow row, Dictionary<string, string> colsNames)
        {
            //if (row != null && (row[colsNames[ColumnKeyNames.FORMULARYSTATUS]].IsNotNull() && row[colsNames[ColumnKeyNames.FORMULARYSTATUS]].ToString().IsNotEmpty()) && (
            //    string.Compare(row[colsNames[ColumnKeyNames.FORMULARYSTATUS]].ToString().Trim(), "yes", true) == 0 || string.Compare(row[colsNames[ColumnKeyNames.FORMULARYSTATUS]].ToString().Trim(), "no", true) == 0))
            //    return true;

            //Can import both formulary and non-formulary
            if (row != null) return true;

            return false;
        }

        //private void FormatResponseMessage(TerminiologyAPIResponse<CreateFormularyAPIResponse> response, CreateFormularyAPIRequest requestRow, StringBuilder responseStringBuilder)
        //{
        //    if (response.StatusCode != DataService.APIModel.StatusCode.Success)
        //    {
        //        responseStringBuilder.AppendLine($"{requestRow.Code}-{string.Join('\n', response.ErrorMessages.ToArray())}");
        //    }

        //    else if (response.StatusCode == DataService.APIModel.StatusCode.Success)
        //    {
        //        if (response.Data.Status != null && response.Data.Status.ErrorMessages.IsCollectionValid())
        //        {
        //            var errors = string.Join('\n', response.Data.Status.ErrorMessages.ToArray());

        //            responseStringBuilder.AppendLine($"{requestRow.Code}-{errors}");
        //        }
        //        else
        //        {
        //            responseStringBuilder.AppendLine($"{requestRow.Code}-Success");
        //        }
        //    }

        //}

        private Dictionary<string, Action<DataRow, FormularyHeaderAPIModel>> GetRowData(Dictionary<string, string> colsNames)
        {
            const char DataDelimiter = '|';

            var dataDictionary = new Dictionary<string, Action<DataRow, FormularyHeaderAPIModel>>();

            dataDictionary.Add(ColumnKeyNames.Name, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.Name]].IsNotNull() && dr[colsNames[ColumnKeyNames.Name]].ToString().IsNotEmpty())
                    apiModel.Name = dr[colsNames[ColumnKeyNames.Name]]?.ToString()?.Trim();
            });
            dataDictionary.Add(ColumnKeyNames.Code, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.Code]].IsNotNull() && dr[colsNames[ColumnKeyNames.Code]].ToString().IsNotEmpty())
                    apiModel.Code = dr[colsNames[ColumnKeyNames.Code]]?.ToString()?.Trim();
            });
            dataDictionary.Add(ColumnKeyNames.Level, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.Level]].IsNotNull() && dr[colsNames[ColumnKeyNames.Level]].ToString().IsNotEmpty())
                    apiModel.ProductType = dr[colsNames[ColumnKeyNames.Level]]?.ToString()?.Trim();
            });
            dataDictionary.Add(ColumnKeyNames.ParentCode, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.ParentCode]].IsNotNull() && dr[colsNames[ColumnKeyNames.ParentCode]].ToString().IsNotEmpty())
                    apiModel.ParentCode = dr[colsNames[ColumnKeyNames.ParentCode]]?.ToString()?.Trim();
            });
            dataDictionary.Add(ColumnKeyNames.ParentLevel, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.ParentLevel]].IsNotNull() && dr[colsNames[ColumnKeyNames.ParentLevel]].ToString().IsNotEmpty())
                    apiModel.ParentProductType = dr[colsNames[ColumnKeyNames.ParentLevel]]?.ToString()?.Trim();
            });

            dataDictionary.Add(ColumnKeyNames.FormCode, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.FormCode]].IsNotNull() && dr[colsNames[ColumnKeyNames.FormCode]].ToString().IsNotEmpty())
                    apiModel.Detail.FormCd = dr[colsNames[ColumnKeyNames.FormCode]]?.ToString()?.Trim();
            });
            dataDictionary.Add(ColumnKeyNames.ControlledDrugStatusCode, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.ControlledDrugStatusCode]].IsNotNull() && dr[colsNames[ColumnKeyNames.ControlledDrugStatusCode]].ToString().IsNotEmpty())
                    apiModel.Detail.ControlledDrugCategories = new List<FormularyLookupAPIModel> { new FormularyLookupAPIModel { Cd = dr[colsNames[ColumnKeyNames.ControlledDrugStatusCode]]?.ToString()?.Trim() } };
            });

            dataDictionary.Add(ColumnKeyNames.PrescribingStatusCode, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.PrescribingStatusCode]].IsNotNull() && dr[colsNames[ColumnKeyNames.PrescribingStatusCode]].ToString().IsNotEmpty())
                    apiModel.Detail.PrescribingStatusCd = dr[colsNames[ColumnKeyNames.PrescribingStatusCode]]?.ToString()?.Trim();
            });

            dataDictionary.Add(ColumnKeyNames.SupplierCode, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.SupplierCode]].IsNotNull() && dr[colsNames[ColumnKeyNames.SupplierCode]].ToString().IsNotEmpty())
                    apiModel.Detail.SupplierCd = dr[colsNames[ColumnKeyNames.SupplierCode]]?.ToString()?.Trim();
            });
            dataDictionary.Add(ColumnKeyNames.FORMULARYSTATUS, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.FORMULARYSTATUS]].IsNotNull() && dr[colsNames[ColumnKeyNames.FORMULARYSTATUS]].ToString().IsNotEmpty())
                {
                    if (string.Compare(dr[colsNames[ColumnKeyNames.FORMULARYSTATUS]].ToString().Trim(), "yes", true) == 0)
                    {
                        apiModel.Detail.RnohFormularyStatuscd = "001";
                    }
                    else
                    {
                        apiModel.Detail.RnohFormularyStatuscd = "002";
                    }
                    //else if (string.Compare(dr[colsNames[ColumnKeyNames.FORMULARYSTATUS]].ToString().Trim(), "no", true) == 0)
                    //{
                    //    apiModel.Detail.RnohFormularyStatuscd = "002";
                    //}
                }
                else
                {
                    apiModel.Detail.RnohFormularyStatuscd = "002";
                }
                // apiModel.Detail.RnohFormularyStatuscd = string.Compare(dr[colsNames[ColumnKeyNames.FORMULARYSTATUS]].ToString().Trim(), "yes", true) == 0 ? "001" : "002";
            });

            dataDictionary.Add(ColumnKeyNames.LicensedRouteCodes, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.LicensedRouteCodes]].IsNotNull() && dr[colsNames[ColumnKeyNames.LicensedRouteCodes]].ToString().IsNotEmpty())
                {
                    var routeCodes = dr[colsNames[ColumnKeyNames.LicensedRouteCodes]].ToString().Split(DataDelimiter);

                    routeCodes.Each(routeCode =>
                    {

                        if (routeCode.IsNotEmpty())
                        {
                            var routeRequest = new FormularyRouteDetailAPIModel
                            {
                                RouteCd = routeCode,
                                RouteFieldTypeCd = "003"//Normal or Licensed
                            };
                            apiModel.FormularyRouteDetails.Add(routeRequest);
                        }
                    });
                }
            });

            dataDictionary.Add(ColumnKeyNames.UnlicensedRoutesCodes, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.UnlicensedRoutesCodes]].IsNotNull() && dr[colsNames[ColumnKeyNames.UnlicensedRoutesCodes]].ToString().IsNotEmpty())
                {
                    var routeCodes = dr[colsNames[ColumnKeyNames.UnlicensedRoutesCodes]].ToString().Split(DataDelimiter);

                    routeCodes.Each(routeCode =>
                    {

                        if (routeCode.IsNotEmpty())
                        {
                            var routeRequest = new FormularyRouteDetailAPIModel
                            {
                                RouteCd = routeCode,
                                RouteFieldTypeCd = "002"//UnLicensed
                            };
                            apiModel.FormularyRouteDetails.Add(routeRequest);
                        }
                    });
                }
            });

            dataDictionary.Add(ColumnKeyNames.CriticalDrug, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.CriticalDrug]].IsNotNull() && dr[colsNames[ColumnKeyNames.CriticalDrug]].ToString().IsNotEmpty())
                    apiModel.Detail.CriticalDrug = string.Compare(dr[colsNames[ColumnKeyNames.CriticalDrug]].ToString().Trim(), "yes", true) == 0 ? "1" : "0";
            });

            dataDictionary.Add(ColumnKeyNames.CYTOTOXIC, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.CYTOTOXIC]].IsNotNull() && dr[colsNames[ColumnKeyNames.CYTOTOXIC]].ToString().IsNotEmpty())
                    apiModel.Detail.Cytotoxic = string.Compare(dr[colsNames[ColumnKeyNames.CYTOTOXIC]].ToString().Trim(), "yes", true) == 0 ? "1" : "0";
            });

            dataDictionary.Add(ColumnKeyNames.BLACKTRIANGLE, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.BLACKTRIANGLE]].IsNotNull() && dr[colsNames[ColumnKeyNames.BLACKTRIANGLE]].ToString().IsNotEmpty())
                    apiModel.Detail.BlackTriangle = string.Compare(dr[colsNames[ColumnKeyNames.BLACKTRIANGLE]].ToString().Trim(), "yes", true) == 0 ? "1" : "0";
            });
            //dataDictionary.Add(ColumnKeyNames.RESTRICTEDPRESCRIBING, (dr, apiModel) =>
            //{
            //    if (dr[colsNames[ColumnKeyNames.RESTRICTEDPRESCRIBING]].IsNotNull() && dr[colsNames[ColumnKeyNames.RESTRICTEDPRESCRIBING]].ToString().IsNotEmpty())
            //        apiModel.Detail.RestrictedPrescribing = string.Compare(dr[colsNames[ColumnKeyNames.RESTRICTEDPRESCRIBING]].ToString().Trim(), "yes", true) == 0 ? "1" : "0";
            //});
            dataDictionary.Add(ColumnKeyNames.NOTESFORRESTRICTION, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.NOTESFORRESTRICTION]].IsNotNull() && dr[colsNames[ColumnKeyNames.NOTESFORRESTRICTION]].ToString().IsNotEmpty())
                    apiModel.Detail.RestrictionNote = dr[colsNames[ColumnKeyNames.NOTESFORRESTRICTION]]?.ToString()?.Trim();
            });

            dataDictionary.Add(ColumnKeyNames.MEDUSIVGUIDE, (dr, apiModel) =>
            {
                if (dr[colsNames[ColumnKeyNames.MEDUSIVGUIDE]].IsNotNull() && dr[colsNames[ColumnKeyNames.MEDUSIVGUIDE]].ToString().IsNotEmpty())
                    apiModel.Detail.MedusaPreparationInstructions = new List<string> { dr[colsNames[ColumnKeyNames.MEDUSIVGUIDE]]?.ToString()?.Trim() };
            });

            return dataDictionary;
        }

        //private string CreateDirectory()
        //{
        //    string folderName = "ImportedFiles";
        //    string webRootPath = _hostingEnvironment.WebRootPath;
        //    string newPath = Path.Combine(webRootPath, folderName);

        //    if (!Directory.Exists(newPath))
        //    {
        //        Directory.CreateDirectory(newPath);
        //    }

        //    return newPath;
        //}

        private Dictionary<string, string> GetColumnDetailsFromConfig => _configuration.GetSection("MMC_Excel_Import_Cols").GetChildren().ToList().ToDictionary(c => c.Key, c => c.Value);

        private class ColumnKeyNames
        {
            public const string Name = "Name";
            public const string Code = "Code";
            public const string Level = "Level";

            public const string FormCode = "FormCode";
            public const string ControlledDrugStatusCode = "ControlledDrugStatusCode";
            public const string PrescribingStatusCode = "PrescribingStatusCode";
            public const string SupplierCode = "SupplierCode";
            public const string ParentCode = "ParentCode";
            public const string ParentLevel = "ParentLevel";
            public const string FORMULARYSTATUS = "FORMULARYSTATUS";
            public const string LicensedRoute = "LicensedRoute";
            public const string LicensedRouteCodes = "LicensedRouteCodes";//New
            public const string AdditonalRoutes = "ADDITIONAL_ROUTE";
            public const string UnlicensedRoutes = "UNLICENSED_ROUTES";
            public const string UnlicensedRoutesCodes = "UnlicensedRoutesCodes";//New

            public const string CriticalDrug = "CRITICAL_DRUG";
            public const string CYTOTOXIC = "CYTOTOXIC";
            public const string BLACKTRIANGLE = "BLACKTRIANGLE";
            public const string RESTRICTEDPRESCRIBING = "RESTRICTEDPRESCRIBING";
            public const string NOTESFORRESTRICTION = "NOTESFORRESTRICTION";
            public const string MEDUSIVGUIDE = "MEDUSAIVGUIDE";


        }
    }
}
