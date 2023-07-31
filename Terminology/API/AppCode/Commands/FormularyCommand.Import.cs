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
﻿using AutoMapper;
using Interneuron.Common.Extensions;
using Interneuron.FDBAPI.Client;
using Interneuron.FDBAPI.Client.DataModels;
using Interneuron.Terminology.API.AppCode.Commands.ImportMergeHandlers;
using Interneuron.Terminology.API.AppCode.Commands.ImportRules;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.API.AppCode.DTOs.SNOMED;
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.API.AppCode.Queries;
using Interneuron.Terminology.Infrastructure;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Commands
{
    public partial class FormularyCommand : IFormularyCommands
    {
        const string INVALID_INPUT_MSG = "Invalid Input\n";

        const string DUPLICATE_MSG = "Record(s) with the same context details already exists in the System. Added the record as 'Duplicate'. Details - Code : {0}\n";

        const string COMPLETE_MATCHING_RECS_MSG = "Matching Code '{0}' already exists. The duplicate record has not been added";


        private IServiceProvider _provider;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private APIRequestContext _requestContext;
        private IFormularyQueries _formularyQueries;
        private IDMDQueries _dMDQueries;
        private string _defaultFormularyStatusCode = TerminologyConstants.FORMULARYSTATUS_FORMULARY;
        private string _defaultRecordStatusCode = TerminologyConstants.RECORDSTATUS_DRAFT;
        private Dictionary<string, List<string>> _cautionsForCodes;
        private Dictionary<string, List<string>> _sideEffectsForCodes;
        private Dictionary<string, List<string>> _safetyMessagesForCodes;
        private Dictionary<string, List<FDBIdText>> _contraIndicationsForCodes;
        private Dictionary<string, List<FDBIdText>> _licensedUsesForCodes;
        private Dictionary<string, List<FDBIdText>> _unLicensedUsesForCodes;
        private Dictionary<string, bool?> _highRiskFlagForCodes;
        private Dictionary<string, (string, string)> _therapeuticClassForCodes;

        private Dictionary<string, bool> _blackTriangleFlagForCodes;
        private ConcurrentDictionary<string, List<DmdAmpDrugrouteDTO>> _dmdAMPRouteMappings;
        private ConcurrentDictionary<string, List<DmdVmpDrugrouteDTO>> _dmdVMPRouteMappings;
        private ConcurrentDictionary<string, List<FormularyExcipient>> _dmdAMPExcipientsForCodes;
        private ConcurrentDictionary<string, List<FormularyAdditionalCode>> _dmdAdditionalCodesForCodes;
        private ConcurrentDictionary<string, SnomedTradeFamiliesDTO> _snomedTradeFamilyMappings;
        private ConcurrentDictionary<string, SnomedModifiedReleaseDTO> _snomedModifiedReleaseMappings;

        public FormularyCommand(IServiceProvider provider, IConfiguration configuration, IMapper mapper, APIRequestContext requestContext, IFormularyQueries formularyQueries, IDMDQueries dMDQueries)
        {
            _provider = provider;
            _mapper = mapper;
            _configuration = configuration;
            _requestContext = requestContext;
            _formularyQueries = formularyQueries;
            _dMDQueries = dMDQueries;
        }

        public async Task<ImportFormularyResultsDTO> ImportByCodes(List<string> dmdCodes, string defaultFormularyStatusCode = TerminologyConstants.FORMULARYSTATUS_FORMULARY, string defaultRecordStatusCode = TerminologyConstants.RECORDSTATUS_DRAFT)
        {
            _defaultFormularyStatusCode = defaultFormularyStatusCode;
            _defaultRecordStatusCode = defaultRecordStatusCode;

            var importFormularyDTO = new ImportFormularyResultsDTO()
            {
                Status = new StatusDTO { StatusCode = TerminologyConstants.STATUS_SUCCESS, StatusMessage = "", ErrorMessages = new List<string>() },
                DMDCodes = dmdCodes,
                Data = new List<FormularyDTO>()
            };//Status 2 = fail

            if (!dmdCodes.IsCollectionValid())
            {
                importFormularyDTO.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                importFormularyDTO.Status.ErrorMessages.Add(INVALID_INPUT_MSG);

                return importFormularyDTO;
            }
            var maxAllowedImport = _configuration.GetSection("TerminologyConfig").GetValue<int>("MaxAllowedImport");

            if (dmdCodes.Count > maxAllowedImport)
            {
                importFormularyDTO.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                const string MaxAllowedRecsMSG = "Cannot import more than {0} records.";
                importFormularyDTO.Status.ErrorMessages.Add(MaxAllowedRecsMSG.ToFormat(maxAllowedImport));
                return importFormularyDTO;
            }

            //Query and get DMD information
            var dmdQueries = this._provider.GetService(typeof(IDMDQueries)) as IDMDQueries;

            var dmdResults = await dmdQueries.GetDMDFullDataForCodes(dmdCodes.ToArray());

            if (!dmdResults.IsCollectionValid())
            {
                importFormularyDTO.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                importFormularyDTO.Status.ErrorMessages.Add(INVALID_INPUT_MSG);

                return importFormularyDTO;
            }

            await PrefillDataForImport(dmdResults);

            HandleImportSave(dmdResults, importFormularyDTO);

            return importFormularyDTO;
        }


        private async Task PrefillDataForImport(List<DMDDetailResultDTO> dmdResults)
        {
            AssignExcipients(dmdResults);

            await PrefillAdditionalCodesForDMDCodes(dmdResults);

            PrefillTradeFamiliesFromSNOMED(dmdResults);

            PrefillModifiedReleaseFromSNOMED(dmdResults);

            PrefillAllVMPMappedRoutesFromDMD(dmdResults);

            PrefillAllAMPMappedRoutesFromDMD(dmdResults);

            await PreFillFDBRecords1(dmdResults);
        }

        private void PrefillTradeFamiliesFromSNOMED(List<DMDDetailResultDTO> dmdResults)
        {
            _snomedTradeFamilyMappings = new ConcurrentDictionary<string, SnomedTradeFamiliesDTO>();
            var dmdCodes = dmdResults?.Select(rec => rec.Code).Distinct().ToList();

            if (!dmdCodes.IsCollectionValid()) return;

            var snomedCTQueries = this._provider.GetService(typeof(ISnomedCTQueries)) as ISnomedCTQueries;
            var tradeFamiliies = snomedCTQueries.GetTradeFamilyForConceptIds(dmdCodes);

            if (!tradeFamiliies.IsCollectionValid()) return;

            dmdCodes.AsParallel().Each(rec =>
            {
                var tradeFamilyForCode = tradeFamiliies?.FirstOrDefault(tf => tf.BrandedDrugId == rec);

                if (tradeFamilyForCode != null)
                {
                    _snomedTradeFamilyMappings[rec] = tradeFamilyForCode;
                }
            });
        }

        private void PrefillModifiedReleaseFromSNOMED(List<DMDDetailResultDTO> dmdResults)
        {
            _snomedModifiedReleaseMappings = new ConcurrentDictionary<string, SnomedModifiedReleaseDTO>();
            var dmdCodes = dmdResults?.Select(rec => rec.Code).Distinct().ToList();

            if (!dmdCodes.IsCollectionValid()) return;

            var snomedCTQueries = this._provider.GetService(typeof(ISnomedCTQueries)) as ISnomedCTQueries;
            var modifiedrelease = snomedCTQueries.GetModifiedReleaseForConceptIds(dmdCodes);

            if (!modifiedrelease.IsCollectionValid()) return;

            dmdCodes.AsParallel().Each(rec =>
            {
                var modifiedReleaseCode = modifiedrelease?.FirstOrDefault(tf => tf.DrugId == rec);

                if (modifiedReleaseCode != null)
                {
                    _snomedModifiedReleaseMappings[rec] = modifiedReleaseCode;
                }
            });
        }

        private async Task PrefillAdditionalCodesForDMDCodes(List<DMDDetailResultDTO> dmdResults)
        {
            _dmdAdditionalCodesForCodes = new ConcurrentDictionary<string, List<FormularyAdditionalCode>>();

            var dmdCodes = dmdResults?.Select(rec => rec.Code).Distinct().ToList();

            if (!dmdCodes.IsCollectionValid()) return;

            var dmdQueries = this._provider.GetService(typeof(IDMDQueries)) as IDMDQueries;

            var dmdATCCodes = await dmdQueries.GetAllATCCodesFromDMD();

            var dmdBNFCodes = await dmdQueries.GetAllBNFCodesFromDMD();

            if (!dmdATCCodes.IsCollectionValid() && !dmdBNFCodes.IsCollectionValid()) return;

            dmdCodes.AsParallel().Each(rec =>
            {
                var atcForDMD = dmdATCCodes?.Where(atc => atc.DmdCd == rec).ToList();
                var bnfsForDMD = dmdBNFCodes?.Where(atc => atc.DmdCd == rec).ToList();

                _dmdAdditionalCodesForCodes[rec] = new List<FormularyAdditionalCode>();

                if (atcForDMD.IsCollectionValid())
                {
                    var formularyAddnls = _mapper.Map<List<FormularyAdditionalCode>>(atcForDMD);
                    _dmdAdditionalCodesForCodes[rec].AddRange(formularyAddnls);
                }
                if (bnfsForDMD.IsCollectionValid())
                {
                    var formularyAddnls = _mapper.Map<List<FormularyAdditionalCode>>(bnfsForDMD);
                    _dmdAdditionalCodesForCodes[rec].AddRange(formularyAddnls);
                }
            });
        }

        private void AssignExcipients(List<DMDDetailResultDTO> dmdResults)
        {
            var dmdCodes = dmdResults.Where(rec => rec.LogicalLevel == 3)?.Select(rec => rec.Code).Distinct().ToList();//Only for AMPs

            _dmdAMPExcipientsForCodes = new ConcurrentDictionary<string, List<FormularyExcipient>>();

            if (!dmdCodes.IsCollectionValid()) return;

            var dmdQueries = this._provider.GetService(typeof(IDMDQueries)) as IDMDQueries;

            var dmdAMPExcipients = dmdQueries.GetAMPExcipientsForCodes(dmdCodes);

            if (dmdAMPExcipients.IsCollectionValid())
            {
                dmdAMPExcipients.AsParallel().Each(rec =>
                {
                    var formularyExcipient = _mapper.Map<FormularyExcipient>(rec);

                    if (_dmdAMPExcipientsForCodes.ContainsKey(rec.Apid))
                        _dmdAMPExcipientsForCodes[rec.Apid].Add(formularyExcipient);
                    else
                        _dmdAMPExcipientsForCodes[rec.Apid] = new List<FormularyExcipient> { formularyExcipient };
                });
            }
        }

        private void PrefillAllAMPMappedRoutesFromDMD(List<DMDDetailResultDTO> dmdResults)
        {
            var dmdCodes = dmdResults.Where(rec => rec.LogicalLevel == 3)?.Select(rec => rec.Code).Distinct().ToList();//Only for AMPs

            _dmdAMPRouteMappings = new ConcurrentDictionary<string, List<DmdAmpDrugrouteDTO>>();

            if (!dmdCodes.IsCollectionValid()) return;

            var dmdQueries = this._provider.GetService(typeof(IDMDQueries)) as IDMDQueries;

            var dmdAMPDrugRoutes = dmdQueries.GetAMPDrugRoutesForCodes(dmdCodes);

            if (!dmdAMPDrugRoutes.IsCollectionValid()) return;

            dmdAMPDrugRoutes.AsParallel().Each(rec =>
            {
                if (_dmdAMPRouteMappings.ContainsKey(rec.Apid))
                    _dmdAMPRouteMappings[rec.Apid].Add(rec);
                else
                    _dmdAMPRouteMappings[rec.Apid] = new List<DmdAmpDrugrouteDTO> { rec };
            });
        }

        private void PrefillAllVMPMappedRoutesFromDMD(List<DMDDetailResultDTO> dmdResults)
        {
            var dmdCodes = dmdResults.Where(rec => rec.LogicalLevel == 2)?.Select(rec => rec.Code).Distinct().ToList();//Only for VMPs

            _dmdVMPRouteMappings = new ConcurrentDictionary<string, List<DmdVmpDrugrouteDTO>>();

            if (!dmdCodes.IsCollectionValid()) return;

            var dmdQueries = this._provider.GetService(typeof(IDMDQueries)) as IDMDQueries;

            var dmdVMPDrugRoutes = dmdQueries.GetVMPDrugRoutesForCodes(dmdCodes);

            if (!dmdVMPDrugRoutes.IsCollectionValid()) return;

            dmdVMPDrugRoutes.AsParallel().Each(rec =>
            {
                if (_dmdVMPRouteMappings.ContainsKey(rec.Vpid))
                    _dmdVMPRouteMappings[rec.Vpid].Add(rec);
                else
                    _dmdVMPRouteMappings[rec.Vpid] = new List<DmdVmpDrugrouteDTO> { rec };
            });
        }

        private void HandleImportSave(List<DMDDetailResultDTO> dmdResults, ImportFormularyResultsDTO importFormularyDTO)
        {
            var formularyHeaderRepo = this._provider.GetService(typeof(IRepository<FormularyHeader>)) as IRepository<FormularyHeader>;

            var formulariesToSave = PopulateFormulariesForImport(dmdResults, formularyHeaderRepo, importFormularyDTO);

            SaveFormulariesForImport(formulariesToSave, formularyHeaderRepo);

            if (formulariesToSave.IsCollectionValid())
            {
                formulariesToSave.Each(saveFormulary =>
                {
                    PopulateDTO(saveFormulary, importFormularyDTO);
                });
            }
        }

        private List<FormularyHeader> GetExistingFormulariesByCodes(List<DMDDetailResultDTO> dmdResults)
        {
            //Get Both current DMD codes and also previous DMD codes
            var dmdCodes = dmdResults.Select(rec => rec.Code).ToList();
            var prevCodes = dmdResults.Where(rec => rec.PrevCode.IsNotEmpty()).Select(rec => rec.PrevCode).ToList();

            if (prevCodes.IsCollectionValid())
                dmdCodes.AddRange(prevCodes);

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;
            var existingFormularies = formularyRepo.GetLatestFormulariesByCodes(dmdCodes.ToArray()).ToList();

            existingFormularies = existingFormularies?.Where(rec => rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED).ToList();

            return existingFormularies;
        }

        private void SaveFormulariesForImport(List<FormularyHeader> formulariesToSave, IRepository<FormularyHeader> formularyHeaderRepo)
        {
            if (formulariesToSave.IsCollectionValid())
            {
                formulariesToSave.Each(saveFormulary =>
                {
                    formularyHeaderRepo.Add(saveFormulary);
                });

                formularyHeaderRepo.SaveChanges();
            }
        }

        private List<FormularyHeader> PopulateFormulariesForImport(List<DMDDetailResultDTO> dmdResults, IRepository<FormularyHeader> formularyHeaderRepo, ImportFormularyResultsDTO importFormularyDTO)
        {
            var formulariesToSave = new List<FormularyHeader>();

            var existingFormulariesForCodes = GetExistingFormulariesByCodes(dmdResults);

            foreach (var res in dmdResults)
            {
                var formularyHeader = CreateHeaderForImport(res);

                //check for duplicates
                var checkIfCodeExistsInDB = existingFormulariesForCodes.Where(x => x.Code == res.Code && x.IsLatest == true);

                string productType = res.LogicalLevel.GetDMDLevelCodeByLogicalLevel().ToLower();

                switch (productType)
                {
                    case "amp":
                        if (checkIfCodeExistsInDB.Count() == 0 || (checkIfCodeExistsInDB.Count() > 0 && checkIfCodeExistsInDB.FirstOrDefault().RecStatusCode != "001"))
                        {
                            PopulateFormularyDetailForImport(res, formularyHeader);

                            PopulateFormularyIngredientsForImport(res, formularyHeader);

                            PopulateFormularyExcipientsForImport(res, formularyHeader);

                            PopulateFormularyRouteDetailsForImport(res, formularyHeader);

                            PopulateFormularyAdditionalCodesForImport(res, formularyHeader);

                            ApplyRules(res, formularyHeader);

                            //OverwriteFromExisting(res, formularyHeader, existingFormulariesForCodes, formularyHeaderRepo);
                            CopyDataFromActiveRecord(res, formularyHeader);

                            formulariesToSave.Add(formularyHeader);
                        }
                        break;
                    case "vmp":
                        if (checkIfCodeExistsInDB.Count() == 0)
                        {
                            PopulateFormularyDetailForImport(res, formularyHeader);

                            PopulateFormularyIngredientsForImport(res, formularyHeader);

                            PopulateFormularyExcipientsForImport(res, formularyHeader);

                            PopulateFormularyRouteDetailsForVMPs(res, formularyHeader);

                            PopulateFormularyRouteDetailsForImport(res, formularyHeader);

                            PopulateFormularyAdditionalCodesForImport(res, formularyHeader);

                            ApplyRules(res, formularyHeader);

                            //OverwriteFromExisting(res, formularyHeader, existingFormulariesForCodes, formularyHeaderRepo);

                            formulariesToSave.Add(formularyHeader);
                        }
                        break;
                    case "vtm":
                        if (checkIfCodeExistsInDB.Count() == 0)
                        {
                            PopulateFormularyDetailForImport(res, formularyHeader);

                            PopulateFormularyIngredientsForImport(res, formularyHeader);

                            PopulateFormularyExcipientsForImport(res, formularyHeader);

                            PopulateFormularyRouteDetailsForImport(res, formularyHeader);

                            PopulateFormularyAdditionalCodesForImport(res, formularyHeader);

                            ApplyRules(res, formularyHeader);

                            //OverwriteFromExisting(res, formularyHeader, existingFormulariesForCodes, formularyHeaderRepo);

                            formulariesToSave.Add(formularyHeader);
                        }
                        break;
                    default:
                        break;
                }
            }

            return formulariesToSave;
        }

        private void OverwriteFromExisting(DMDDetailResultDTO resDTO, FormularyHeader formularyHeader, List<FormularyHeader> existingFormulariesForCodes, IRepository<FormularyHeader> formularyHeaderRepo)
        {
            //Check if there is exisitng record
            //if record (for AMP) exists and is active - overwrite the custom properties from existing and mark the old one as archived and new one as active
            //if record exists (for VTM and VMP) - overwrite the custom properties from existing
            //if record  (for AMP) exists and is in any other status (other than active) - overwrite the custom properties from existing and mark the old one as archived and new one as draft

            if (!existingFormulariesForCodes.IsCollectionValid()) return;

            var existingFormulary = existingFormulariesForCodes.Where(rec => (string.Compare(rec.ProductType, resDTO.LogicalLevel.GetDMDLevelCodeByLogicalLevel(), true) == 0) && (string.Compare(rec.Code, resDTO.Code, true) == 0 || string.Compare(rec.Code, resDTO.PrevCode, true) == 0)).FirstOrDefault();

            if (existingFormulary == null) return;


            var referencedFormulary = existingFormulary;

            FormularyHeader newVersionedFormulary = null;

            //Change the status to archived
            if (string.Compare(formularyHeader.ProductType, "amp", true) == 0)
            {
                newVersionedFormulary = existingFormulary.CloneFormulary(_mapper, Guid.NewGuid().ToString());
                newVersionedFormulary.VersionId = existingFormulary.VersionId + 1;

                referencedFormulary = newVersionedFormulary;
            }


            ImportMergeHandler mergeHandler = null;
            if (string.Compare(formularyHeader.ProductType, "vtm", true) == 0)
                mergeHandler = new VTMImportMergeHandler(_mapper, resDTO, formularyHeader, referencedFormulary);
            else if (string.Compare(formularyHeader.ProductType, "vmp", true) == 0)
                mergeHandler = new VMPImportMergeHandler(_mapper, resDTO, formularyHeader, referencedFormulary);
            else if (string.Compare(formularyHeader.ProductType, "amp", true) == 0)
                mergeHandler = new AMPImportMergeHandler(_mapper, resDTO, formularyHeader, referencedFormulary);
            else
                mergeHandler = new NullImportMergeHandler(_mapper, resDTO, formularyHeader, referencedFormulary);

            mergeHandler.MergeFromExisting();

            existingFormulary.IsLatest = false;
            formularyHeaderRepo.Update(existingFormulary);

            //Change the status to archived
            if (newVersionedFormulary != null)
            {
                newVersionedFormulary.RecStatusCode = TerminologyConstants.RECORDSTATUS_ARCHIVED;
                newVersionedFormulary.IsLatest = false;

                formularyHeaderRepo.Add(newVersionedFormulary);
            }
        }

        private void PopulateFormularyAdditionalCodesForImport(DMDDetailResultDTO res, FormularyHeader formularyHeader)
        {
            if ((!_dmdAdditionalCodesForCodes.IsCollectionValid() || !_dmdAdditionalCodesForCodes.ContainsKey(formularyHeader.Code)) &&
                (!_therapeuticClassForCodes.IsCollectionValid() || !_therapeuticClassForCodes.ContainsKey(formularyHeader.Code))) return;

            var addnlCodes = new List<FormularyAdditionalCode>();

            if (_dmdAdditionalCodesForCodes.IsCollectionValid() && _dmdAdditionalCodesForCodes.ContainsKey(formularyHeader.Code))
            {
                var dmdAddnlCodes = _dmdAdditionalCodesForCodes[formularyHeader.Code];

                dmdAddnlCodes = dmdAddnlCodes?.Where(rec => rec.AdditionalCode.IsNotEmpty()).ToList();

                dmdAddnlCodes?.Each(rec => addnlCodes.Add(rec));
            }

            if (_therapeuticClassForCodes.IsCollectionValid() && _therapeuticClassForCodes.ContainsKey(formularyHeader.Code))
            {
                var theraupeticClassificationCode = _therapeuticClassForCodes[formularyHeader.Code];
                if (theraupeticClassificationCode.Item1.IsNotEmpty())
                {
                    addnlCodes.Add(new FormularyAdditionalCode
                    {
                        CodeType = TerminologyConstants.CODE_SYSTEM_CLASSIFICATION_TYPE,
                        AdditionalCode = theraupeticClassificationCode.Item1,
                        AdditionalCodeDesc = theraupeticClassificationCode.Item2,
                        AdditionalCodeSystem = TerminologyConstants.FDB_DATA_SRC,
                        Source = TerminologyConstants.FDB_DATA_SRC
                    });
                }
            }

            addnlCodes.Each(rec =>
            {
                rec.FormularyVersionId = formularyHeader.FormularyVersionId;
            });

            formularyHeader.FormularyAdditionalCode = addnlCodes;
        }

        private void PopulateFormularyExcipientsForImport(DMDDetailResultDTO res, FormularyHeader formularyHeader)
        {
            if (!_dmdAMPExcipientsForCodes.IsCollectionValid() || !_dmdAMPExcipientsForCodes.ContainsKey(formularyHeader.Code)) return;

            var excipients = _dmdAMPExcipientsForCodes[formularyHeader.Code];

            if (!excipients.IsCollectionValid()) return;

            excipients.Each(rec =>
            {
                rec.FormularyVersionId = formularyHeader.FormularyVersionId;
            });

            formularyHeader.FormularyExcipient = excipients;
        }

        private void PopulateFormularyRouteDetailsForImport(DMDDetailResultDTO res, FormularyHeader formularyHeader)
        {
            PopulateFormularyRouteDetailsForAMPs(res, formularyHeader);
        }

        private void PopulateFormularyRouteDetailsForAMPs(DMDDetailResultDTO res, FormularyHeader formularyHeader)
        {
            if (!_dmdAMPRouteMappings.IsCollectionValid() || !_dmdAMPRouteMappings.ContainsKey(formularyHeader.Code)) return;

            formularyHeader.FormularyRouteDetail = formularyHeader.FormularyRouteDetail ?? new List<FormularyRouteDetail>();

            var dmdRouteForAMPs = _dmdAMPRouteMappings[formularyHeader.Code];

            var ampRoutes = _mapper.Map<List<FormularyRouteDetail>>(dmdRouteForAMPs);

            if (!ampRoutes.IsCollectionValid()) return;

            ampRoutes.Each(routeDetail =>
            {
                routeDetail.FormularyVersionId = formularyHeader.FormularyVersionId;
                routeDetail.RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_NORMAL; //Normal
                routeDetail.Source = TerminologyConstants.DMD_DATA_SRC;

                formularyHeader.FormularyRouteDetail.Add(routeDetail);
            });
        }

        private void PopulateFormularyRouteDetailsForVMPs(DMDDetailResultDTO res, FormularyHeader formularyHeader)
        {
            if (!_dmdVMPRouteMappings.IsCollectionValid() || !_dmdVMPRouteMappings.ContainsKey(formularyHeader.Code)) return;

            formularyHeader.FormularyRouteDetail = formularyHeader.FormularyRouteDetail ?? new List<FormularyRouteDetail>();

            var dmdRouteForVMPs = _dmdVMPRouteMappings[formularyHeader.Code];

            var vmpRoutes = _mapper.Map<List<FormularyRouteDetail>>(dmdRouteForVMPs);

            if (!vmpRoutes.IsCollectionValid()) return;

            vmpRoutes.Each(routeDetail =>
            {
                routeDetail.FormularyVersionId = formularyHeader.FormularyVersionId;
                routeDetail.RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_ADDITONAL; //Additional
                routeDetail.Source = TerminologyConstants.DMD_DATA_SRC;

                formularyHeader.FormularyRouteDetail.Add(routeDetail);
            });
        }

        private void PopulateFormularyIngredientsForImport(DMDDetailResultDTO res, FormularyHeader formularyHeader)
        {
            if (res.VMPIngredients.IsCollectionValid())
            {
                formularyHeader.FormularyIngredient = new List<FormularyIngredient>();

                res.VMPIngredients.Each(ing =>
                {
                    var ingredient = new FormularyIngredient();
                    ingredient.FormularyVersionId = formularyHeader.FormularyVersionId;

                    ingredient.BasisOfPharmaceuticalStrengthCd = ing.BasisStrntcd?.ToString();
                    ingredient.IngredientCd = ing.Isid?.ToString();
                    ingredient.StrengthValueNumerator = ing.StrntNmrtrVal?.ToString();
                    ingredient.StrengthValueNumeratorUnitCd = ing.StrntNmrtrUomcd?.ToString();
                    ingredient.StrengthValueDenominator = ing.StrntDnmtrVal?.ToString();
                    ingredient.StrengthValueDenominatorUnitCd = ing.StrntDnmtrUomcd?.ToString();

                    formularyHeader.FormularyIngredient.Add(ingredient);
                });
            }
        }

        private void PopulateFormularyDetailForImport(DMDDetailResultDTO res, FormularyHeader formularyHeader)
        {
            formularyHeader.FormularyDetail = new List<FormularyDetail>();

            var formularyDetail = new FormularyDetail();

            formularyDetail.FormularyVersionId = formularyHeader.FormularyVersionId;
            formularyDetail.RnohFormularyStatuscd = _defaultFormularyStatusCode ?? TerminologyConstants.FORMULARYSTATUS_NONFORMULARY;

            if (res.BasisOfName != null)
                formularyDetail.BasisOfPreferredNameCd = res.BasisOfName.Cd?.ToString();

            formularyDetail.CfcFree = res.CfcF;
            formularyDetail.GlutenFree = res.GluF;
            formularyDetail.PreservativeFree = res.PresF;
            formularyDetail.SugarFree = res.SugF;
            formularyDetail.UnitDoseFormSize = res.Udfs;

            if (res.UnitDoseFormSizeUOM != null)
                formularyDetail.UnitDoseFormUnits = res.UnitDoseFormSizeUOM.Cd?.ToString();

            if (res.UnitDoseUOM != null)
                formularyDetail.UnitDoseUnitOfMeasureCd = res.UnitDoseUOM.Cd?.ToString();

            if (res.DoseForm != null)
                formularyDetail.DoseFormCd = res.DoseForm.Cd?.ToString();

            formularyDetail.EmaAdditionalMonitoring = res.Ema;

            if (res.LicensingAuthority != null)
            {
                var licensingAuth = res.LicensingAuthority.Cd?.ToString();
                formularyDetail.CurrentLicensingAuthorityCd = licensingAuth;
                formularyDetail.UnlicensedMedicationCd = licensingAuth.IsNotEmpty() && licensingAuth == "0" ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;

                //For all the devices(2), the record status should be set to draft initially
                formularyHeader.RecStatusCode = licensingAuth.IsNotEmpty() && licensingAuth == "2" ? TerminologyConstants.RECORDSTATUS_DRAFT : formularyHeader.RecStatusCode;
            }

            formularyDetail.ParallelImport = res.ParallelImport;

            if (res.AvailableRestriction != null)
                formularyDetail.RestrictionsOnAvailabilityCd = res.AvailableRestriction.Cd?.ToString();

            if (res.ControlDrugCategory != null)
            {
                formularyDetail.ControlledDrugCategoryCd = res.ControlDrugCategory.Cd?.ToString();
                formularyDetail.ControlledDrugCategorySource = TerminologyConstants.DMD_DATA_SRC;
            }

            if (res.PrescribingStatus != null)
                formularyDetail.PrescribingStatusCd = res.PrescribingStatus.Cd?.ToString();

            if (res.SupplierCode != null)
                formularyDetail.SupplierCd = res.SupplierCode?.ToString();

            if (res.Form != null)
                formularyDetail.FormCd = res.Form.Cd?.ToString();

            if (formularyHeader.Code.IsNotEmpty() && _snomedTradeFamilyMappings.ContainsKey(formularyHeader.Code))
            {
                var tfDTO = _snomedTradeFamilyMappings[formularyHeader.Code];
                formularyDetail.TradeFamilyCd = tfDTO.TradeFamilyId;
                formularyDetail.TradeFamilyName = tfDTO.TradeFamilyTerm;
            }

            if (formularyHeader.Code.IsNotEmpty() && _snomedModifiedReleaseMappings.ContainsKey(formularyHeader.Code))
            {
                var tfDTO = _snomedModifiedReleaseMappings[formularyHeader.Code];
                formularyDetail.ModifiedReleaseCd = tfDTO.MrCd;
            }

            //This will be overridden by the rules later
            formularyDetail.Prescribable = true;
            formularyDetail.PrescribableSource = TerminologyConstants.DMD_DATA_SRC;

            formularyHeader.FormularyDetail.Add(formularyDetail);

            //await AddFDBDetailsForProductTypeAndCode(formularyHeader);
            AddFDBDetailsForProductTypeAndCode(formularyHeader);
        }

        private void ApplyRules(DMDDetailResultDTO dMDDetailResultDTO, FormularyHeader formularyHeader)
        {
            //Identity the product type and apply the rules
            IImportRule importRule = null;
            if (string.Compare(formularyHeader.ProductType, "vtm", true) == 0)
                importRule = new VTMImportRule(dMDDetailResultDTO, formularyHeader);
            else if (string.Compare(formularyHeader.ProductType, "vmp", true) == 0)
                importRule = new VMPImportRule(dMDDetailResultDTO, formularyHeader);
            else if (string.Compare(formularyHeader.ProductType, "amp", true) == 0)
                importRule = new AMPImportRule(dMDDetailResultDTO, formularyHeader);
            else
                importRule = new NullImportRule(dMDDetailResultDTO, formularyHeader);

            importRule.MutateByRules();
        }

        private FormularyHeader CreateHeaderForImport(DMDDetailResultDTO res)
        {
            var formularyHeader = new FormularyHeader();

            formularyHeader.FormularyId = Guid.NewGuid().ToString();
            formularyHeader.VersionId = 1;
            formularyHeader.FormularyVersionId = Guid.NewGuid().ToString();
            formularyHeader.IsLatest = true;
            formularyHeader.IsDuplicate = false;//Need to check

            formularyHeader.Code = res.Code;
            formularyHeader.CodeSystem = TerminologyConstants.DEFAULT_IDENTIFICATION_CODE_SYSTEM;

            formularyHeader.Name = res.Name;
            formularyHeader.ParentCode = res.ParentCode;
            formularyHeader.ParentName = null;
            formularyHeader.ParentProductType = res.LogicalLevel.GetDMDParentLevelCodeByLogicalLevel();
            formularyHeader.ProductType = res.LogicalLevel.GetDMDLevelCodeByLogicalLevel();

            formularyHeader.RecSource = TerminologyConstants.RECORD_SOURCE_IMPORT;// "Import";

            formularyHeader.RecStatusCode = _defaultRecordStatusCode; //(string.Compare(formularyHeader.ProductType, "amp", true) == 0) ? _defaultRecordStatusCode : null;// TerminologyConstants.RECORDSTATUS_DRAFT;//Draft

            formularyHeader.RecStatuschangeDate = DateTime.UtcNow;

            formularyHeader.VtmId = (string.Compare(formularyHeader.ParentProductType, "vtm", true) == 0) ? formularyHeader.ParentCode : null;

            formularyHeader.VmpId = (string.Compare(formularyHeader.ParentProductType, "vmp", true) == 0) ? formularyHeader.ParentCode : null;

            return formularyHeader;
        }

        private void PopulateDTO(FormularyHeader formularyHeader, ImportFormularyResultsDTO importFormularyDTO)
        {
            var headerDTO = _mapper.Map<FormularyDTO>(formularyHeader);

            if (formularyHeader.FormularyDetail.IsCollectionValid())
                headerDTO.Detail = _mapper.Map<FormularyDetailDTO>(formularyHeader.FormularyDetail.First());

            if (formularyHeader.FormularyAdditionalCode.IsCollectionValid())
            {
                headerDTO.FormularyAdditionalCodes = _mapper.Map<List<FormularyAdditionalCodeDTO>>(formularyHeader.FormularyAdditionalCode.ToList());
            }


            if (formularyHeader.FormularyIngredient.IsCollectionValid())
            {
                headerDTO.FormularyIngredients = _mapper.Map<List<FormularyIngredientDTO>>(formularyHeader.FormularyIngredient.ToList());
            }

            if (formularyHeader.FormularyRouteDetail.IsCollectionValid())
            {
                headerDTO.FormularyRouteDetails = _mapper.Map<List<FormularyRouteDetailDTO>>(formularyHeader.FormularyRouteDetail.ToList());
            }

            importFormularyDTO.Data.Add(headerDTO);
        }

        public void AddFDBDetailsForProductTypeAndCode(FormularyHeader recordToImport)
        {
            if (!recordToImport.FormularyDetail.IsCollectionValid()) return;

            recordToImport.FormularyDetail.Each(detail =>
            {
                detail.Caution = recordToImport.Code.SafeGetStringifiedCodeDescListForCode(_cautionsForCodes, TerminologyConstants.FDB_DATA_SRC);
                detail.SideEffect = recordToImport.Code.SafeGetStringifiedCodeDescListForCode(_sideEffectsForCodes, TerminologyConstants.FDB_DATA_SRC);
                detail.SafetyMessage = recordToImport.Code.SafeGetStringifiedCodeDescListForCode(_safetyMessagesForCodes, TerminologyConstants.FDB_DATA_SRC);
                detail.ContraIndication = recordToImport.Code.SafeGetStringifiedCodeDescListForCode(_contraIndicationsForCodes, TerminologyConstants.FDB_DATA_SRC);
                detail.LicensedUse = recordToImport.Code.SafeGetStringifiedCodeDescListForCode(_licensedUsesForCodes, TerminologyConstants.FDB_DATA_SRC);
                detail.UnlicensedUse = recordToImport.Code.SafeGetStringifiedCodeDescListForCode(_unLicensedUsesForCodes, TerminologyConstants.FDB_DATA_SRC);

                if (_highRiskFlagForCodes.IsCollectionValid() && _highRiskFlagForCodes.ContainsKey(recordToImport.Code))
                {
                    detail.HighAlertMedication = (_highRiskFlagForCodes[recordToImport.Code].HasValue && _highRiskFlagForCodes[recordToImport.Code].Value) ? "1" : null;
                    detail.HighAlertMedicationSource = (detail.HighAlertMedication == "1") ? TerminologyConstants.FDB_DATA_SRC : null;
                }

                if (_blackTriangleFlagForCodes.IsCollectionValid() && _blackTriangleFlagForCodes.ContainsKey(recordToImport.Code))
                {
                    detail.BlackTriangle = _blackTriangleFlagForCodes[recordToImport.Code] ? "1" : null;
                    detail.BlackTriangleSource = (detail.BlackTriangle == "1") ? TerminologyConstants.FDB_DATA_SRC : null;
                }
            });

        }

        private async Task<bool> PreFillFDBRecords1(List<DMDDetailResultDTO> dmdResults)
        {
            if (!dmdResults.IsCollectionValid()) return false;

            var codesAndProductTypes = dmdResults.Select(res => new FDBDataRequest()
            {
                ProductType = res.LogicalLevel.GetDMDLevelCodeByLogicalLevel(),
                ProductCode = res.Code
            }).ToList();

            if (!codesAndProductTypes.IsCollectionValid()) return false;

            var ampOnlyCodes = codesAndProductTypes.Where(rec => string.Compare(rec.ProductType, "amp", true) == 0).ToList();

            if (!ampOnlyCodes.IsCollectionValid()) return true;

            var baseFDBUrl = _configuration.GetSection("FDB").GetValue<string>("BaseURL");
            
            var token = _requestContext.AuthToken;

            var fdbClient = new FDBAPIClient(baseFDBUrl);


            var cautionsTask = await fdbClient.GetCautionsByCodes(ampOnlyCodes, token);
            var sideEffectsTask = await fdbClient.GetSideEffectsByCodes(ampOnlyCodes, token);
            var safetyMessagesTask = await fdbClient.GetSafetyMessagesByCodes(ampOnlyCodes, token);
            var contraIndicationsTask = await fdbClient.GetContraIndicationsByCodes(ampOnlyCodes, token);
            var licensedUsesTask = await fdbClient.GetLicensedUseByCodes(ampOnlyCodes, token);
            var unLicensedUsesTask = await fdbClient.GetUnLicensedUseByCodes(ampOnlyCodes, token);
            var blackTriangleFlagTask = await fdbClient.GetAdverseEffectsFlagByCodes(ampOnlyCodes, token);
            var highRiskFlagTask = await fdbClient.GetHighRiskFlagByCodes(ampOnlyCodes, token);
            var theraupeuticClassTask = await fdbClient.GetTherapeuticClassificationGroupsByCodes(ampOnlyCodes, token);

            var cautionsResult = cautionsTask; var sideEffectsResult = sideEffectsTask; var safetyMessagesResult = safetyMessagesTask; var contraIndicationsResult = contraIndicationsTask; var licensedUsesResult = licensedUsesTask; var unLicensedUsesResult = unLicensedUsesTask;
            var highRiskFlag = highRiskFlagTask;
            var theraupeuticClass = theraupeuticClassTask;

            if (cautionsResult.Data.IsCollectionValid())
            {
                _cautionsForCodes = cautionsResult.Data;
            }
            if (sideEffectsResult.Data.IsCollectionValid())
            {
                _sideEffectsForCodes = sideEffectsResult.Data;
            }
            if (safetyMessagesResult.Data.IsCollectionValid())
            {
                _safetyMessagesForCodes = safetyMessagesResult.Data;
            }

            if (contraIndicationsResult.Data.IsCollectionValid())
            {
                _contraIndicationsForCodes = contraIndicationsResult.Data;
            }

            if (licensedUsesResult.Data.IsCollectionValid())
            {
                _licensedUsesForCodes = licensedUsesResult.Data;
            }
            if (unLicensedUsesResult.Data.IsCollectionValid())
            {
                _unLicensedUsesForCodes = unLicensedUsesResult.Data;
            }

            var blackTriangleFlag = blackTriangleFlagTask;

            if (blackTriangleFlag.Data.IsCollectionValid())
            {
                _blackTriangleFlagForCodes = blackTriangleFlag.Data;
            }

            if (highRiskFlag.Data.IsCollectionValid())
            {
                _highRiskFlagForCodes = highRiskFlag.Data;
            }

            _therapeuticClassForCodes = theraupeuticClass?.Data;

            return true;
        }

        private void CopyDataFromActiveRecord(DMDDetailResultDTO res, FormularyHeader formularyHeader)
        {
            var formularyForCode = GetExistingFormulariesByCodes(new List<DMDDetailResultDTO>() { res });

            if (formularyForCode.Count == 0) return;

            var activeFormulary = formularyForCode.Where(x => x.RecStatusCode == TerminologyConstants.RECORDSTATUS_ACTIVE).FirstOrDefault();

            if (activeFormulary.IsNotNull())
            {
                var activeFormularyDetail = activeFormulary.FormularyDetail.FirstOrDefault();

                var activeFormularyLocalRoutes = activeFormulary.FormularyLocalRouteDetail;

                var formularyDetail = formularyHeader.FormularyDetail.FirstOrDefault();

                formularyHeader.FormularyLocalRouteDetail = formularyHeader.FormularyLocalRouteDetail ?? new List<FormularyLocalRouteDetail>();

                //var formularyDetail = new FormularyDetail();

                formularyDetail.RnohFormularyStatuscd = activeFormularyDetail.RnohFormularyStatuscd;
                formularyDetail.LocalLicensedUse = activeFormularyDetail.LocalLicensedUse;
                formularyDetail.LocalUnlicensedUse = activeFormularyDetail.LocalUnlicensedUse;
                formularyDetail.RoundingFactorCd = activeFormularyDetail.RoundingFactorCd;
                formularyDetail.CustomWarning = activeFormularyDetail.CustomWarning;
                formularyDetail.Reminder = activeFormularyDetail.Reminder;
                formularyDetail.Endorsement = activeFormularyDetail.Endorsement;
                formularyDetail.MedusaPreparationInstructions = activeFormularyDetail.MedusaPreparationInstructions;
                formularyDetail.TitrationTypeCd = activeFormularyDetail.TitrationTypeCd;
                formularyDetail.Diluent = activeFormularyDetail.Diluent;
                formularyDetail.ClinicalTrialMedication = activeFormularyDetail.ClinicalTrialMedication;
                formularyDetail.CriticalDrug = activeFormularyDetail.CriticalDrug;
                formularyDetail.IsGastroResistant = activeFormularyDetail.IsGastroResistant;
                formularyDetail.IsModifiedRelease = activeFormularyDetail.IsModifiedRelease;
                formularyDetail.ExpensiveMedication = activeFormularyDetail.ExpensiveMedication;
                formularyDetail.HighAlertMedication = activeFormularyDetail.HighAlertMedication;
                formularyDetail.HighAlertMedicationSource = activeFormularyDetail.HighAlertMedicationSource;
                formularyDetail.IvToOral = activeFormularyDetail.IvToOral;
                formularyDetail.NotForPrn = activeFormularyDetail.NotForPrn;
                formularyDetail.IsBloodProduct = activeFormularyDetail.IsBloodProduct;
                formularyDetail.IsDiluent = activeFormularyDetail.IsDiluent;
                formularyDetail.Prescribable = activeFormularyDetail.Prescribable == null ? false : activeFormularyDetail.Prescribable;
                formularyDetail.PrescribableSource = activeFormularyDetail.PrescribableSource;
                formularyDetail.OutpatientMedicationCd = activeFormularyDetail.OutpatientMedicationCd;
                formularyDetail.IgnoreDuplicateWarnings = activeFormularyDetail.IgnoreDuplicateWarnings;
                formularyDetail.IsCustomControlledDrug = activeFormularyDetail.IsCustomControlledDrug;
                formularyDetail.IsPrescriptionPrintingRequired = activeFormularyDetail.IsPrescriptionPrintingRequired;
                formularyDetail.IsIndicationMandatory = activeFormularyDetail.IsIndicationMandatory;
                formularyDetail.WitnessingRequired = activeFormularyDetail.WitnessingRequired;

                //formularyHeader.FormularyDetail.Add(formularyDetail);

                activeFormularyLocalRoutes.Each(res => {
                    var localRoute = new FormularyLocalRouteDetail();

                    localRoute.Createdby = res.Createdby;
                    localRoute.Createddate = DateTime.Now.ToUniversalTime();
                    localRoute.Createdtimestamp = DateTime.Now;
                    localRoute.FormularyVersionId = formularyDetail.FormularyVersionId;
                    localRoute.RouteCd = res.RouteCd;
                    localRoute.RouteFieldTypeCd = res.RouteFieldTypeCd;
                    localRoute.RowId = Guid.NewGuid().ToString();
                    localRoute.Source = res.Source;
                    localRoute.Updatedby = res.Updatedby;
                    localRoute.Updateddate = DateTime.Now.ToUniversalTime();
                    localRoute.Updatedtimestamp = DateTime.Now;

                    formularyHeader.FormularyLocalRouteDetail.Add(localRoute);
                });
            }
            else
            {
                return;
            }

        }
    }
}
