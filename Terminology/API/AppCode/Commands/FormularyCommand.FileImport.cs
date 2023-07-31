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
﻿using Interneuron.Common.Extensions;
using Interneuron.FDBAPI.Client.DataModels;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary.Requests;
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.API.AppCode.Queries;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Commands
{
    public partial class FormularyCommand : IFormularyCommands
    {
        public async Task<ImportFormularyResultsDTO> FileImport(CreateEditFormularyRequest request)
        {
            var response = new ImportFormularyResultsDTO
            {
                Status = new StatusDTO { StatusCode = TerminologyConstants.STATUS_SUCCESS, StatusMessage = "", ErrorMessages = new List<string>() },
                Data = new List<FormularyDTO>()
            };

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var uniqueCodes = request.RequestsData.Select(req => req.Code).Distinct().ToArray();

            if (!uniqueCodes.IsCollectionValid())
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                response.Status.ErrorMessages.Add(INVALID_INPUT_MSG);
                return response;
            }

            //Query and get DMD information
            var dmdQueries = this._provider.GetService(typeof(IDMDQueries)) as IDMDQueries;

            var dmdResults = await dmdQueries.GetDMDFullDataForCodes(uniqueCodes.ToArray());

            if (!dmdResults.IsCollectionValid())
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                response.Status.ErrorMessages.Add(INVALID_INPUT_MSG);
                return response;
            }
            
            await PrefillDataForImport(dmdResults);

            PopulateFormularyDataForFileImport(request, dmdResults, response);

            return response;
        }

        private void PopulateFormularyDataForFileImport(CreateEditFormularyRequest request, List<DMDDetailResultDTO> dmdResults, ImportFormularyResultsDTO response)
        {
            var toBeSavedFormularies = new List<FormularyHeader>();

            var dmdCodes = request.RequestsData.Select(req => req.Code).Distinct().ToArray();

            var formularyHeaderRepo = this._provider.GetService(typeof(IRepository<FormularyHeader>)) as IRepository<FormularyHeader>;

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            //var existingFormulariesFromDB = formularyRepo.Items.Where(item => dmdCodes.Contains(item.Code)).ToList();
            //var existingFormulariesFromDB = formularyRepo.GetLatestFormulariesByCodes(dmdCodes.ToArray(), true).ToList();

            //var existingCodes = new HashSet<string>();

            //if (existingFormulariesFromDB.IsCollectionValid())
            //{
            //    //Consider non-archived and non-deleted records. Records returned from Db are only in non-deleted status only
            //    var onlyValidSameRecordsInDb = existingFormulariesFromDB.Where(rec => rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED).ToList();
            //    if (onlyValidSameRecordsInDb.IsCollectionValid())
            //    {
            //        existingFormulariesFromDB = onlyValidSameRecordsInDb;
            //        existingCodes = existingFormulariesFromDB.Select(item => item.Code).Distinct().ToHashSet();
            //    }
            //}

            //var formulariesToSave = PopulateFormulariesForImport(dmdResults, existingCodes, existingFormulariesFromDB, response);
            var formulariesToSave = PopulateFormulariesForImport(dmdResults, formularyHeaderRepo, response);

            OverrideFormularyForFileImport(request, formulariesToSave);

            SaveFormulariesForImport(formulariesToSave, formularyHeaderRepo);

            if (formulariesToSave.IsCollectionValid())
            {
                formulariesToSave.Each(saveFormulary =>
                {
                    PopulateDTO(saveFormulary, response);
                });
            }
        }

        private void OverrideFormularyForFileImport(CreateEditFormularyRequest request, List<FormularyHeader> formulariesToSave)
        {
            /*
             * These are the properties that will be considered from the request object
             * Formulary Status
             * Unlicensed Routes
             * Critical Drug
             * Blood Product
            */

            var uniqueObjs = request.RequestsData.Select(req => new { Code = req.Code, Obj = req }).Distinct(rec => rec.Code).ToDictionary(k => k.Code, v => v.Obj);

            if (formulariesToSave.IsCollectionValid() && uniqueObjs.IsCollectionValid())
            {
                formulariesToSave.AsParallel().Each(rec =>
                {
                    if (uniqueObjs.ContainsKey(rec.Code))
                    {
                        var reqObj = uniqueObjs[rec.Code];

                        var detail = rec.FormularyDetail.First();
                        var routes = rec.FormularyRouteDetail ?? new List<FormularyRouteDetail>();
                        rec.FormularyRouteDetail = routes;

                        if (reqObj.Detail != null)
                        {
                            detail.RnohFormularyStatuscd = reqObj.Detail.RnohFormularyStatuscd != null ? reqObj.Detail.RnohFormularyStatuscd : TerminologyConstants.FORMULARYSTATUS_FORMULARY;
                            detail.CriticalDrug = reqObj.Detail.CriticalDrug;
                            detail.IsBloodProduct = reqObj.Detail.IsBloodProduct;
                        }

                        if (reqObj.FormularyRouteDetails.IsCollectionValid())
                        {
                            var unlicensedRoutesInReq = reqObj.FormularyRouteDetails.Where(fr => fr.RouteFieldTypeCd == TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED).ToList();
                            if (unlicensedRoutesInReq.IsCollectionValid())
                            {
                                var unlicensedRoutesMapped = _mapper.Map<List<FormularyRouteDetail>>(unlicensedRoutesInReq);
                                unlicensedRoutesMapped.Each(ur =>
                                {
                                    rec.FormularyRouteDetail.Add(ur);
                                });
                            }
                        }

                        if (reqObj.FormularyLocalRouteDetails.IsCollectionValid())
                        {
                            var unlicensedRoutesInReq = reqObj.FormularyLocalRouteDetails.Where(fr => fr.RouteFieldTypeCd == TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED).ToList();
                            if (unlicensedRoutesInReq.IsCollectionValid())
                            {
                                var unlicensedRoutesMapped = _mapper.Map<List<FormularyLocalRouteDetail>>(unlicensedRoutesInReq);
                                unlicensedRoutesMapped.Each(ur =>
                                {
                                    rec.FormularyLocalRouteDetail.Add(ur);
                                });
                            }
                        }
                    }
                });
            }
        }
    }
}
