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


﻿using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using Interneuron.Terminology.Model.Search;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public partial class FormularyQueries : IFormularyQueries
    {
        public async Task<FormularyDTO> GetFormularyDetailRuleBound(string id)
        {
            //get Current object by id

            //Create the complete DTO object for it

            //Verify the product type of the current object

            //Get Ancestors or descendents or both based on the product type

            //1.

            id = id.Trim();
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var resultsFromDB = repo.GetFormularyDetail(id);

            var resultObj = resultsFromDB.FirstOrDefault();

            if (resultObj == null) return null;

            var builderType = GetFormBuilderType(resultObj);

            var orchestrator = new RuleBoundFormularyBuilderOrchestrator(builderType);

            await orchestrator.BuildFormulary(resultObj);

            var formularyDTO = builderType.FormularyDTO;

            formularyDTO.Detail.LicensedUses.Sort((a, b) => a.Desc.CompareTo(b.Desc));

            formularyDTO.Detail.ContraIndications.Sort((a, b) => a.Desc.CompareTo(b.Desc));

            formularyDTO.Detail.SideEffects.Sort((a, b) => a.Desc.CompareTo(b.Desc));

            formularyDTO.Detail.Cautions.Sort((a, b) => a.Desc.CompareTo(b.Desc));

            return formularyDTO;

        }

        public async Task<FormularyDTO> GetActiveFormularyDetailRuleBoundByCode(string code)
        {
            //get Current object by code

            //Create the complete DTO object for it

            //Verify the product type of the current object

            //Get Ancestors or descendents or both based on the product type

            //1.

            code = code.Trim();

            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var currentActiveRecsForCode = repo.ItemsAsReadOnly.Where(rec => rec.IsLatest == true && rec.Code == code).ToList();

            if (!currentActiveRecsForCode.IsCollectionValid()) return null;

            currentActiveRecsForCode = currentActiveRecsForCode.Where(rec =>
            {
                if (string.Compare(rec.ProductType, "amp", true) != 0) return true;

                return rec.RecStatusCode == TerminologyConstants.RECORDSTATUS_ACTIVE;
            }).ToList();

            if (!currentActiveRecsForCode.IsCollectionValid()) return null;

            var idForCurrentActiveRecsForCode = currentActiveRecsForCode.Select(rec => rec.FormularyVersionId).First();

            var resultsFromDB = repo.GetFormularyDetail(idForCurrentActiveRecsForCode);

            var resultObj = resultsFromDB.FirstOrDefault();

            if (resultObj == null) return null;

            var builderType = GetFormBuilderType(resultObj);

            var orchestrator = new RuleBoundFormularyBuilderOrchestrator(builderType);

            await orchestrator.BuildFormulary(resultObj);

            if (builderType.FormularyDTO.FormularyRouteDetails != null && builderType.FormularyDTO.FormularyRouteDetails.Count > 0)
            {
                builderType.FormularyDTO.FormularyRouteDetails.Clear();
            }

            if (builderType.FormularyDTO.Detail.LicensedUses != null && builderType.FormularyDTO.Detail.LicensedUses.Count > 0)
            {
                builderType.FormularyDTO.Detail.LicensedUses.Clear();
            }

            if (builderType.FormularyDTO.Detail.UnLicensedUses != null && builderType.FormularyDTO.Detail.UnLicensedUses.Count > 0)
            {
                builderType.FormularyDTO.Detail.UnLicensedUses.Clear();
            }

            if (builderType.FormularyDTO.FormularyLocalRouteDetails.IsCollectionValid())
            {
                builderType.FormularyDTO.FormularyLocalRouteDetails.Where(x => x.RouteFieldTypeCd == TerminologyConstants.ROUTEFIELDTYPE_NORMAL).Each(rec => {
                    FormularyRouteDetailDTO formularyRouteDetailDTO = new FormularyRouteDetailDTO();

                    formularyRouteDetailDTO.Createdby = rec.Createdby;
                    formularyRouteDetailDTO.Createddate = rec.Createddate;
                    formularyRouteDetailDTO.FormularyVersionId = rec.FormularyVersionId;
                    formularyRouteDetailDTO.RouteCd = rec.RouteCd;
                    formularyRouteDetailDTO.RouteDesc = rec.RouteDesc;
                    formularyRouteDetailDTO.RouteFieldTypeCd = rec.RouteFieldTypeCd;
                    formularyRouteDetailDTO.RouteFieldTypeDesc = rec.RouteFieldTypeDesc;
                    formularyRouteDetailDTO.RowId = rec.RowId;
                    formularyRouteDetailDTO.Source = rec.Source;
                    formularyRouteDetailDTO.Updatedby = rec.Updatedby;
                    formularyRouteDetailDTO.Updateddate = rec.Updateddate;

                    if (builderType.FormularyDTO.FormularyRouteDetails.IsCollectionValid())
                    {
                        builderType.FormularyDTO.FormularyRouteDetails.Add(formularyRouteDetailDTO);
                    }
                    else
                    {
                        builderType.FormularyDTO.FormularyRouteDetails = new List<FormularyRouteDetailDTO>();

                        builderType.FormularyDTO.FormularyRouteDetails.Add(formularyRouteDetailDTO);
                    }

                });

                builderType.FormularyDTO.FormularyLocalRouteDetails.Where(x => x.RouteFieldTypeCd == TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED).Each(rec => {
                    FormularyRouteDetailDTO formularyRouteDetailDTO = new FormularyRouteDetailDTO();

                    formularyRouteDetailDTO.Createdby = rec.Createdby;
                    formularyRouteDetailDTO.Createddate = rec.Createddate;
                    formularyRouteDetailDTO.FormularyVersionId = rec.FormularyVersionId;
                    formularyRouteDetailDTO.RouteCd = rec.RouteCd;
                    formularyRouteDetailDTO.RouteDesc = rec.RouteDesc;
                    formularyRouteDetailDTO.RouteFieldTypeCd = rec.RouteFieldTypeCd;
                    formularyRouteDetailDTO.RouteFieldTypeDesc = rec.RouteFieldTypeDesc;
                    formularyRouteDetailDTO.RowId = rec.RowId;
                    formularyRouteDetailDTO.Source = rec.Source;
                    formularyRouteDetailDTO.Updatedby = rec.Updatedby;
                    formularyRouteDetailDTO.Updateddate = rec.Updateddate;

                    if (builderType.FormularyDTO.FormularyRouteDetails.IsCollectionValid())
                    {
                        builderType.FormularyDTO.FormularyRouteDetails.Add(formularyRouteDetailDTO);
                    }
                    else
                    {
                        builderType.FormularyDTO.FormularyRouteDetails = new List<FormularyRouteDetailDTO>();

                        builderType.FormularyDTO.FormularyRouteDetails.Add(formularyRouteDetailDTO);
                    }

                });
            }

            builderType.FormularyDTO.Detail.LocalLicensedUses.Each(rec => {
                FormularyLookupItemDTO formularyLookupItemDTO = new FormularyLookupItemDTO();

                formularyLookupItemDTO.AdditionalProperties = rec.AdditionalProperties;
                formularyLookupItemDTO.Cd = rec.Cd;
                formularyLookupItemDTO.Desc = rec.Desc;
                formularyLookupItemDTO.IsDefault = rec.IsDefault;
                formularyLookupItemDTO.Recordstatus = rec.Recordstatus;
                formularyLookupItemDTO.Source = rec.Source;
                formularyLookupItemDTO.Type = rec.Type;

                if (builderType.FormularyDTO.Detail.LicensedUses.IsCollectionValid())
                {
                    builderType.FormularyDTO.Detail.LicensedUses.Add(formularyLookupItemDTO);
                }
                else
                {
                    builderType.FormularyDTO.Detail.LicensedUses = new List<FormularyLookupItemDTO>();

                    builderType.FormularyDTO.Detail.LicensedUses.Add(formularyLookupItemDTO);
                }
            });

            builderType.FormularyDTO.Detail.LocalUnLicensedUses.Each(rec => {
                FormularyLookupItemDTO formularyLookupItemDTO = new FormularyLookupItemDTO();

                formularyLookupItemDTO.AdditionalProperties = rec.AdditionalProperties;
                formularyLookupItemDTO.Cd = rec.Cd;
                formularyLookupItemDTO.Desc = rec.Desc;
                formularyLookupItemDTO.IsDefault = rec.IsDefault;
                formularyLookupItemDTO.Recordstatus = rec.Recordstatus;
                formularyLookupItemDTO.Source = rec.Source;
                formularyLookupItemDTO.Type = rec.Type;

                if (builderType.FormularyDTO.Detail.UnLicensedUses.IsCollectionValid())
                {
                    builderType.FormularyDTO.Detail.UnLicensedUses.Add(formularyLookupItemDTO);
                }
                else
                {
                    builderType.FormularyDTO.Detail.UnLicensedUses = new List<FormularyLookupItemDTO>();

                    builderType.FormularyDTO.Detail.UnLicensedUses.Add(formularyLookupItemDTO);
                }

            });

            if (builderType.FormularyDTO.FormularyLocalRouteDetails != null && builderType.FormularyDTO.FormularyLocalRouteDetails.Count > 0)
            {
                builderType.FormularyDTO.FormularyLocalRouteDetails.Clear();
            }

            if (builderType.FormularyDTO.Detail.LocalLicensedUses != null && builderType.FormularyDTO.Detail.LocalLicensedUses.Count > 0)
            {
                builderType.FormularyDTO.Detail.LocalLicensedUses.Clear();
            }

            if (builderType.FormularyDTO.Detail.LocalUnLicensedUses != null && builderType.FormularyDTO.Detail.LocalUnLicensedUses.Count > 0)
            {
                builderType.FormularyDTO.Detail.LocalUnLicensedUses.Clear();
            }


            return builderType.FormularyDTO;

        }

        public async Task<FormularyDTO[]> GetActiveFormularyDetailRuleBoundByCodeArray(string[] code)
        {
            //get Current object by codes

            //Create the complete DTO object for it

            //Verify the product type of the current object

            //Get Ancestors or descendents or both based on the product type

            //1.

            //code = code.Trim();

            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var currentActiveRecsForCode = repo.ItemsAsReadOnly.Where(rec => rec.IsLatest == true && code.Contains(rec.Code)).ToList();

            if (!currentActiveRecsForCode.IsCollectionValid()) return null;

            currentActiveRecsForCode = currentActiveRecsForCode.Where(rec =>
            {
                if (string.Compare(rec.ProductType, "amp", true) != 0) return true;

                return rec.RecStatusCode == TerminologyConstants.RECORDSTATUS_ACTIVE;
            }).ToList();

            var FormularyDTOList = new List<FormularyDTO>();

            foreach (var activeRecsForCode in currentActiveRecsForCode)
            {
                var idForCurrentActiveRecsForCode = activeRecsForCode.FormularyVersionId;

                var resultsFromDB = repo.GetFormularyDetail(idForCurrentActiveRecsForCode);

                var resultObj = resultsFromDB.FirstOrDefault();

                if (resultObj != null)
                {
                    var builderType = GetFormBuilderType(resultObj);

                    var orchestrator = new RuleBoundFormularyBuilderOrchestrator(builderType);

                    await orchestrator.BuildFormulary(resultObj);

                    if (builderType.FormularyDTO.FormularyRouteDetails != null && builderType.FormularyDTO.FormularyRouteDetails.Count > 0)
                    {
                        builderType.FormularyDTO.FormularyRouteDetails.Clear();
                    }

                    if (builderType.FormularyDTO.Detail.LicensedUses != null && builderType.FormularyDTO.Detail.LicensedUses.Count > 0)
                    {
                        builderType.FormularyDTO.Detail.LicensedUses.Clear();
                    }

                    if (builderType.FormularyDTO.Detail.UnLicensedUses != null && builderType.FormularyDTO.Detail.UnLicensedUses.Count > 0)
                    {
                        builderType.FormularyDTO.Detail.UnLicensedUses.Clear();
                    }

                    if (builderType.FormularyDTO.FormularyLocalRouteDetails.IsCollectionValid())
                    {
                        builderType.FormularyDTO.FormularyLocalRouteDetails.Where(x => x.RouteFieldTypeCd == TerminologyConstants.ROUTEFIELDTYPE_NORMAL).Each(rec => {
                            FormularyRouteDetailDTO formularyRouteDetailDTO = new FormularyRouteDetailDTO();

                            formularyRouteDetailDTO.Createdby = rec.Createdby;
                            formularyRouteDetailDTO.Createddate = rec.Createddate;
                            formularyRouteDetailDTO.FormularyVersionId = rec.FormularyVersionId;
                            formularyRouteDetailDTO.RouteCd = rec.RouteCd;
                            formularyRouteDetailDTO.RouteDesc = rec.RouteDesc;
                            formularyRouteDetailDTO.RouteFieldTypeCd = rec.RouteFieldTypeCd;
                            formularyRouteDetailDTO.RouteFieldTypeDesc = rec.RouteFieldTypeDesc;
                            formularyRouteDetailDTO.RowId = rec.RowId;
                            formularyRouteDetailDTO.Source = rec.Source;
                            formularyRouteDetailDTO.Updatedby = rec.Updatedby;
                            formularyRouteDetailDTO.Updateddate = rec.Updateddate;

                            if (builderType.FormularyDTO.FormularyRouteDetails.IsCollectionValid())
                            {
                                builderType.FormularyDTO.FormularyRouteDetails.Add(formularyRouteDetailDTO);
                            }
                            else
                            {
                                builderType.FormularyDTO.FormularyRouteDetails = new List<FormularyRouteDetailDTO>();

                                builderType.FormularyDTO.FormularyRouteDetails.Add(formularyRouteDetailDTO);
                            }

                        });

                        builderType.FormularyDTO.FormularyLocalRouteDetails.Where(x => x.RouteFieldTypeCd == TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED).Each(rec => {
                            FormularyRouteDetailDTO formularyRouteDetailDTO = new FormularyRouteDetailDTO();

                            formularyRouteDetailDTO.Createdby = rec.Createdby;
                            formularyRouteDetailDTO.Createddate = rec.Createddate;
                            formularyRouteDetailDTO.FormularyVersionId = rec.FormularyVersionId;
                            formularyRouteDetailDTO.RouteCd = rec.RouteCd;
                            formularyRouteDetailDTO.RouteDesc = rec.RouteDesc;
                            formularyRouteDetailDTO.RouteFieldTypeCd = rec.RouteFieldTypeCd;
                            formularyRouteDetailDTO.RouteFieldTypeDesc = rec.RouteFieldTypeDesc;
                            formularyRouteDetailDTO.RowId = rec.RowId;
                            formularyRouteDetailDTO.Source = rec.Source;
                            formularyRouteDetailDTO.Updatedby = rec.Updatedby;
                            formularyRouteDetailDTO.Updateddate = rec.Updateddate;

                            if (builderType.FormularyDTO.FormularyRouteDetails.IsCollectionValid())
                            {
                                builderType.FormularyDTO.FormularyRouteDetails.Add(formularyRouteDetailDTO);
                            }
                            else
                            {
                                builderType.FormularyDTO.FormularyRouteDetails = new List<FormularyRouteDetailDTO>();

                                builderType.FormularyDTO.FormularyRouteDetails.Add(formularyRouteDetailDTO);
                            }

                        });
                    }

                    builderType.FormularyDTO.Detail.LocalLicensedUses.Each(rec => {
                        FormularyLookupItemDTO formularyLookupItemDTO = new FormularyLookupItemDTO();

                        formularyLookupItemDTO.AdditionalProperties = rec.AdditionalProperties;
                        formularyLookupItemDTO.Cd = rec.Cd;
                        formularyLookupItemDTO.Desc = rec.Desc;
                        formularyLookupItemDTO.IsDefault = rec.IsDefault;
                        formularyLookupItemDTO.Recordstatus = rec.Recordstatus;
                        formularyLookupItemDTO.Source = rec.Source;
                        formularyLookupItemDTO.Type = rec.Type;

                        if (builderType.FormularyDTO.Detail.LicensedUses.IsCollectionValid())
                        {
                            builderType.FormularyDTO.Detail.LicensedUses.Add(formularyLookupItemDTO);
                        }
                        else
                        {
                            builderType.FormularyDTO.Detail.LicensedUses = new List<FormularyLookupItemDTO>();

                            builderType.FormularyDTO.Detail.LicensedUses.Add(formularyLookupItemDTO);
                        }
                    });

                    builderType.FormularyDTO.Detail.LocalUnLicensedUses.Each(rec => {
                        FormularyLookupItemDTO formularyLookupItemDTO = new FormularyLookupItemDTO();

                        formularyLookupItemDTO.AdditionalProperties = rec.AdditionalProperties;
                        formularyLookupItemDTO.Cd = rec.Cd;
                        formularyLookupItemDTO.Desc = rec.Desc;
                        formularyLookupItemDTO.IsDefault = rec.IsDefault;
                        formularyLookupItemDTO.Recordstatus = rec.Recordstatus;
                        formularyLookupItemDTO.Source = rec.Source;
                        formularyLookupItemDTO.Type = rec.Type;

                        if (builderType.FormularyDTO.Detail.UnLicensedUses.IsCollectionValid())
                        {
                            builderType.FormularyDTO.Detail.UnLicensedUses.Add(formularyLookupItemDTO);
                        }
                        else
                        {
                            builderType.FormularyDTO.Detail.UnLicensedUses = new List<FormularyLookupItemDTO>();

                            builderType.FormularyDTO.Detail.UnLicensedUses.Add(formularyLookupItemDTO);
                        }

                    });

                    if (builderType.FormularyDTO.FormularyLocalRouteDetails != null && builderType.FormularyDTO.FormularyLocalRouteDetails.Count > 0)
                    {
                        builderType.FormularyDTO.FormularyLocalRouteDetails.Clear();
                    }

                    if (builderType.FormularyDTO.Detail.LocalLicensedUses != null && builderType.FormularyDTO.Detail.LocalLicensedUses.Count > 0)
                    {
                        builderType.FormularyDTO.Detail.LocalLicensedUses.Clear();
                    }

                    if (builderType.FormularyDTO.Detail.LocalUnLicensedUses != null && builderType.FormularyDTO.Detail.LocalUnLicensedUses.Count > 0)
                    {
                        builderType.FormularyDTO.Detail.LocalUnLicensedUses.Clear();
                    }

                    FormularyDTOList.Add(builderType.FormularyDTO);
                }                
            }

            return FormularyDTOList.ToArray();

        }

        private RuleBoundBaseFormularyBuilder GetFormBuilderType(FormularyHeader resultObj)
        {
            if (string.Compare(resultObj.ProductType, "vtm", true) == 0)
                return new RuleBoundVTMFormularyBuilder(this._provider);

            if (string.Compare(resultObj.ProductType, "vmp", true) == 0)
                return new RuleBoundVMPFormularyBuilder(this._provider);

            if (string.Compare(resultObj.ProductType, "amp", true) == 0)
                return new RuleBoundAMPFormularyBuilder(this._provider);

            return new RuleBoundNullFormularyBuilder(this._provider);
        }
    }

    
}
