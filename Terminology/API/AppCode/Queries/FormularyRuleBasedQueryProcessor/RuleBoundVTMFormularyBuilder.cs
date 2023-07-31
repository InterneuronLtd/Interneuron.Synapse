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
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using Interneuron.Terminology.Model.Search;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public class RuleBoundVTMFormularyBuilder : RuleBoundBaseFormularyBuilder
    {
        private List<FormularyHeader> _childFormularies = new List<FormularyHeader>();
        private ConcurrentBag<FormularyDTO> _childFormulariesDTO = new ConcurrentBag<FormularyDTO>();
        private ConcurrentBag<FormularyCustomWarningDTO> _childCustomWarningsDTO = new ConcurrentBag<FormularyCustomWarningDTO>();
        private ConcurrentBag<FormularyReminderDTO> _childRemindersDTO = new ConcurrentBag<FormularyReminderDTO>();
        private ConcurrentBag<string> _childEndorsementsDTO = new ConcurrentBag<string>();
        private ConcurrentBag<string> _childFormulationsDTO = new ConcurrentBag<string>();

        private ConcurrentBag<string> _childBlackTrianglesDTO = new ConcurrentBag<string>();
        private ConcurrentBag<FormularyAdditionalCodeDTO> _childAdditionalCodesDTO = new ConcurrentBag<FormularyAdditionalCodeDTO>();
        private ConcurrentBag<FormularyRouteDetailDTO> _childRouteDetailsDTO = new ConcurrentBag<FormularyRouteDetailDTO>();
        private ConcurrentBag<FormularyLocalRouteDetailDTO> _childLocalRouteDetailsDTO = new ConcurrentBag<FormularyLocalRouteDetailDTO>();
        private ConcurrentBag<FormularyDetailDTO> _childFormulariesDetailDTO = new ConcurrentBag<FormularyDetailDTO>();
        private ConcurrentBag<FormularyIngredientDTO> _childIngredientsDTO = new ConcurrentBag<FormularyIngredientDTO>();
        private ConcurrentBag<FormularyLookupItemDTO> _childLicensedIndicationsDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentBag<FormularyLookupItemDTO> _childUnlicensedIndicationsDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentBag<FormularyLookupItemDTO> _childLocalLicensedIndicationsDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentBag<FormularyLookupItemDTO> _childLocalUnlicensedIndicationsDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentBag<FormularyLookupItemDTO> _childContraIndicationsDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentBag<string> _childMedusaPreparationInstructionsDTO = new ConcurrentBag<string>();
        private ConcurrentBag<string> _childNiceTADTO = new ConcurrentBag<string>();
        private ConcurrentBag<FormularyLookupItemDTO> _childSideEffectsDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentBag<bool> _childCriticalDrugFlagsDTO = new ConcurrentBag<bool>();
        private ConcurrentDictionary<string, List<bool>> _childIVTOOralFlagsDTO = new ConcurrentDictionary<string, List<bool>>();
        private ConcurrentBag<FormularyLookupItemDTO> _childCautionsDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentBag<bool> _childClinicalTrialFlagsDTO = new ConcurrentBag<bool>();
        private ConcurrentBag<bool> _childEMAAdditonalFlagsDTO = new ConcurrentBag<bool>();
        private ConcurrentBag<bool> _childExpensiveMedicationFlagsDTO = new ConcurrentBag<bool>();
        private ConcurrentBag<bool> _childUnlicensedMedicationFlagsDTO = new ConcurrentBag<bool>();
        private ConcurrentBag<bool> _childHighAlertMedicationFlagsDTO = new ConcurrentBag<bool>();
        private ConcurrentBag<bool> _childNotForPRNFlagsDTO = new ConcurrentBag<bool>();
        private ConcurrentBag<bool> _childOutpatientMedicationFlagsDTO = new ConcurrentBag<bool>();
        private ConcurrentBag<string> _childPrescribingstatusDTO = new ConcurrentBag<string>();
        private ConcurrentBag<bool> _childIgnoreDuplicateWarningsFlagsDTO = new ConcurrentBag<bool>();
        private ConcurrentBag<FormularyLookupItemDTO> _childTitrationTypeDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentBag<bool?> _childBloodProductFlagsDTO = new ConcurrentBag<bool?>();
        private ConcurrentBag<bool?> _childDiluentFlagsDTO = new ConcurrentBag<bool?>();
        private ConcurrentDictionary<string, List<bool?>> _childGastroResistantFlagsDTO = new ConcurrentDictionary<string, List<bool?>>();
        private ConcurrentDictionary<string, List<bool?>> _childMdifiedReleaseFlagsDTO = new ConcurrentDictionary<string, List<bool?>>();
        private ConcurrentBag<FormularyLookupItemDTO> _childSafetyMessagesDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentBag<FormularyLookupItemDTO> _childControlledDrugCategoriessDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentBag<bool?> _childCustomComtrolledDrugFlagsDTO = new ConcurrentBag<bool?>();
        private ConcurrentBag<bool?> _childPrescriptionPrintingRequiredFlagsDTO = new ConcurrentBag<bool?>();
        private ConcurrentBag<string> _childRecordsStatusDTO = new ConcurrentBag<string>();
        private ConcurrentBag<FormularyLookupItemDTO> _childDiluentsDTO = new ConcurrentBag<FormularyLookupItemDTO>();
        private ConcurrentDictionary<string, List<bool?>> _childIndicationMandatoryFlagsDTO = new ConcurrentDictionary<string, List<bool?>>();
        private ConcurrentBag<string> _childModifiedReleaseDTO = new ConcurrentBag<string>();

        public RuleBoundVTMFormularyBuilder(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task CreateBase(FormularyHeader formularyDAO)
        {
            await base.CreateBase(formularyDAO);

            await FillAMPProperties();

            //_formularyDTO.RecStatusCode = GetHighestPrecedenceRecordStatusCode();
        }

        //private string GetHighestPrecedenceRecordStatusCode()
        //{
        //    if (!_childRecordsStatusDTO.IsCollectionValid()) return TerminologyConstants.RECORDSTATUS_DRAFT;

        //    var uniqueRecStsCodes = _childRecordsStatusDTO.Distinct().ToHashSet();

        //    if(uniqueRecStsCodes.Contains(TerminologyConstants.RECORDSTATUS_ACTIVE)) return TerminologyConstants.RECORDSTATUS_ACTIVE;
        //    if (uniqueRecStsCodes.Contains(TerminologyConstants.RECORDSTATUS_APPROVED)) return TerminologyConstants.RECORDSTATUS_APPROVED;
        //    if (uniqueRecStsCodes.Contains(TerminologyConstants.RECORDSTATUS_DRAFT)) return TerminologyConstants.RECORDSTATUS_DRAFT;

        //    return TerminologyConstants.RECORDSTATUS_DRAFT;
        //}

        private async Task FillAMPProperties()
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyBasicSearchResultModel>)) as IFormularyRepository<FormularyBasicSearchResultModel>;

            var nodes = await repo.GetFormularyDescendentForCodes(new string[] { _formularyDAO.Code });

            if (nodes.IsCollectionValid())
            {

                var childIds = nodes.Where(rec => string.Compare(rec.ProductType, "AMP", true) == 0 && rec.RecStatusCode == TerminologyConstants.RECORDSTATUS_ACTIVE)?.Select(rec => rec.FormularyVersionId)?.ToList();

                if (childIds.IsCollectionValid())
                {
                    var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

                    _childFormularies = formularyRepo.GetFormularyListForIds(childIds)?.ToList();

                    _childFormularies.AsParallel().Each(child =>
                    {
                        var header = _mapper.Map<FormularyDTO>(child);

                        var detail = child.FormularyDetail.FirstOrDefault();

                        if (detail != null)
                        {
                            header.Detail = _mapper.Map<FormularyDetailDTO>(detail);

                            _childFormulariesDetailDTO.Add(header.Detail);

                            header.Detail.CustomWarnings?.Each(rec => { _childCustomWarningsDTO.Add(rec); });
                            header.Detail.Reminders?.Each(rec => { _childRemindersDTO.Add(rec); });
                            header.Detail.Endorsements?.Each(rec => { _childEndorsementsDTO.Add(rec); });

                            _childBlackTrianglesDTO.Add(detail.BlackTriangle);

                            header.Detail.LicensedUses?.Each(rec => _childLicensedIndicationsDTO.Add(rec));
                            header.Detail.UnLicensedUses?.Each(rec => _childUnlicensedIndicationsDTO.Add(rec));

                            header.Detail.LocalLicensedUses?.Each(rec => _childLocalLicensedIndicationsDTO.Add(rec));
                            header.Detail.LocalUnLicensedUses?.Each(rec => _childLocalUnlicensedIndicationsDTO.Add(rec));

                            header.Detail.ContraIndications?.Each(rec => _childContraIndicationsDTO.Add(rec));
                            header.Detail.SafetyMessages?.Each(rec => _childSafetyMessagesDTO.Add(rec));


                            header.Detail.MedusaPreparationInstructions?.Each(rec => _childMedusaPreparationInstructionsDTO.Add(rec));
                            header.Detail.NiceTas?.Each(rec => _childNiceTADTO.Add(rec));

                            header.Detail.Cautions?.Each(rec => _childCautionsDTO.Add(rec));

                            header.Detail.SideEffects?.Each(rec => _childSideEffectsDTO.Add(rec));

                            header.Detail.Diluents?.Each(rec => _childDiluentsDTO.Add(rec));

                            _childCriticalDrugFlagsDTO.Add(header.Detail.CriticalDrug == TerminologyConstants.STRINGIFIED_BOOL_TRUE);

                            if (header.ParentCode.IsNotEmpty())
                            {
                                if (_childIVTOOralFlagsDTO.ContainsKey(header.ParentCode))
                                    _childIVTOOralFlagsDTO[header.ParentCode].Add(header.Detail.IvToOral == TerminologyConstants.STRINGIFIED_BOOL_TRUE);
                                else
                                    _childIVTOOralFlagsDTO[header.ParentCode] = new List<bool> { header.Detail.IvToOral == TerminologyConstants.STRINGIFIED_BOOL_TRUE };

                                if (_childGastroResistantFlagsDTO.ContainsKey(header.ParentCode))
                                    _childGastroResistantFlagsDTO[header.ParentCode].Add(header.Detail.IsGastroResistant);
                                else
                                    _childGastroResistantFlagsDTO[header.ParentCode] = new List<bool?> { header.Detail.IsGastroResistant };

                                if (_childMdifiedReleaseFlagsDTO.ContainsKey(header.ParentCode))
                                    _childMdifiedReleaseFlagsDTO[header.ParentCode].Add(header.Detail.IsModifiedRelease);
                                else
                                    _childMdifiedReleaseFlagsDTO[header.ParentCode] = new List<bool?> { header.Detail.IsModifiedRelease };

                                if (_childIndicationMandatoryFlagsDTO.ContainsKey(header.ParentCode))
                                    _childIndicationMandatoryFlagsDTO[header.ParentCode].Add(header.Detail.IsIndicationMandatory);
                                else
                                    _childIndicationMandatoryFlagsDTO[header.ParentCode] = new List<bool?> { header.Detail.IsIndicationMandatory };

                            }

                            _childClinicalTrialFlagsDTO.Add(header.Detail.ClinicalTrialMedication == TerminologyConstants.STRINGIFIED_BOOL_TRUE);

                            header.Detail.ControlledDrugCategories?.Each(rec => _childControlledDrugCategoriessDTO.Add(rec));

                            _childEMAAdditonalFlagsDTO.Add(header.Detail.EmaAdditionalMonitoring == TerminologyConstants.STRINGIFIED_BOOL_TRUE);

                            _childExpensiveMedicationFlagsDTO.Add(header.Detail.ExpensiveMedication == TerminologyConstants.STRINGIFIED_BOOL_TRUE);

                            _childUnlicensedMedicationFlagsDTO.Add(header.Detail.UnlicensedMedicationCd == TerminologyConstants.STRINGIFIED_BOOL_TRUE);

                            _childHighAlertMedicationFlagsDTO.Add(header.Detail.HighAlertMedication == TerminologyConstants.STRINGIFIED_BOOL_TRUE);

                            _childNotForPRNFlagsDTO.Add(header.Detail.NotForPrn == TerminologyConstants.STRINGIFIED_BOOL_TRUE);

                            _childOutpatientMedicationFlagsDTO.Add(header.Detail.OutpatientMedicationCd == TerminologyConstants.STRINGIFIED_BOOL_TRUE);

                            if (header.Detail.PrescribingStatusCd.IsNotEmpty())
                                _childPrescribingstatusDTO.Add(header.Detail.PrescribingStatusCd);

                            header.Detail.TitrationTypes?.Each(rec => _childTitrationTypeDTO.Add(rec));

                            _childIgnoreDuplicateWarningsFlagsDTO.Add(header.Detail.IgnoreDuplicateWarnings == TerminologyConstants.STRINGIFIED_BOOL_TRUE);

                            _childBloodProductFlagsDTO.Add(header.Detail.IsBloodProduct);

                            _childDiluentFlagsDTO.Add(header.Detail.IsDiluent);

                            _childCustomComtrolledDrugFlagsDTO.Add(header.Detail.IsCustomControlledDrug);

                            _childPrescriptionPrintingRequiredFlagsDTO.Add(header.Detail.IsPrescriptionPrintingRequired);

                            _childRecordsStatusDTO.Add(header.RecStatusCode);

                            header.Detail.ModifiedReleases?.Each(rec => _childModifiedReleaseDTO.Add(rec));
                        }

                        if (child.FormularyIngredient.IsCollectionValid())
                        {
                            header.FormularyIngredients = _mapper.Map<List<FormularyIngredientDTO>>(child.FormularyIngredient);
                            header.FormularyIngredients.Each(rec => _childIngredientsDTO.Add(rec));
                        }

                        if (child.FormularyRouteDetail.IsCollectionValid())
                        {
                            header.FormularyRouteDetails = _mapper.Map<List<FormularyRouteDetailDTO>>(child.FormularyRouteDetail);
                            header.FormularyRouteDetails.Each(rec => _childRouteDetailsDTO.Add(rec));
                        }

                        if (child.FormularyLocalRouteDetail.IsCollectionValid())
                        {
                            header.FormularyLocalRouteDetails = _mapper.Map<List<FormularyLocalRouteDetailDTO>>(child.FormularyLocalRouteDetail);
                            header.FormularyLocalRouteDetails.Each(rec => _childLocalRouteDetailsDTO.Add(rec));
                        }

                        if (child.FormularyAdditionalCode.IsCollectionValid())
                        {
                            header.FormularyAdditionalCodes = _mapper.Map<List<FormularyAdditionalCodeDTO>>(child.FormularyAdditionalCode);
                            header.FormularyAdditionalCodes.Each(rec => _childAdditionalCodesDTO.Add(rec));
                        }

                        if (header.Detail.FormCd.IsNotEmpty())
                        {
                            _childFormulationsDTO.Add(header.Detail.FormCd);
                        }

                        _childFormulariesDTO.Add(header);
                    });
                }
            }
        }

        public override void CreateIngredients()
        {
            _formularyDTO.FormularyIngredients = _childIngredientsDTO?.Distinct(new FormularyIngredientDTOComparer()).ToList();
            // _childIngredientsDTO?.Distinct(rec => rec.IngredientCd)?.ToList();
        }

        public override void CreateAdditionalCodes()
        {
            _formularyDTO.FormularyAdditionalCodes = new List<FormularyAdditionalCodeDTO>();

            if (_childFormularies.IsCollectionValid())
            {
                ProjectClassificationCodesFromChildNodes();
            }

            //Add Additional Identity Codes
            if (!_formularyDAO.FormularyAdditionalCode.IsCollectionValid()) return;

            var addlIdentityCodes = _formularyDAO.FormularyAdditionalCode.Where(rec => string.Compare(rec.CodeType, TerminologyConstants.CODE_SYSTEM_IDENTIFICATION_TYPE, true) == 0)?.ToList();

            if (!addlIdentityCodes.IsCollectionValid()) return;
            _formularyDTO.FormularyAdditionalCodes.AddRange(this._mapper.Map<List<FormularyAdditionalCodeDTO>>(addlIdentityCodes));
        }

        public override void CreateRouteDetails()
        {
            //_formularyDTO.FormularyRouteDetails = new List<FormularyRouteDetailDTO>();

            //if (_childRouteDetailsDTO.IsCollectionValid())
            //{
            //    var distinctRoutes = _childRouteDetailsDTO.Distinct(rec => rec.RouteCd).ToList();

            //    _formularyDTO.FormularyRouteDetails.AddRange(distinctRoutes);
            //}

            if (!_childRouteDetailsDTO.IsCollectionValid()) return;

            var licensedRoute = _childRouteDetailsDTO.Where(x => x.RouteFieldTypeCd == "003").Distinct(rec => rec.RouteCd).ToList();

            var unlicensedRoute = _childRouteDetailsDTO.Where(x => x.RouteFieldTypeCd == "002").Distinct(rec => rec.RouteCd).ToList();

            List<FormularyRouteDetailDTO> tempUnlicensedRoute = new List<FormularyRouteDetailDTO>();

            tempUnlicensedRoute.AddRange(unlicensedRoute);

            for (int i = 0; i < unlicensedRoute.Count(); i++)
            {
                if (licensedRoute.Exists(x => x.RouteCd == unlicensedRoute[i].RouteCd))
                {
                    tempUnlicensedRoute.Remove(unlicensedRoute[i]);
                }
            }

            _childRouteDetailsDTO.Clear();

            unlicensedRoute.Clear();

            unlicensedRoute.AddRange(tempUnlicensedRoute);

            foreach (var route in licensedRoute)
            {
                _childRouteDetailsDTO.Add(route);
            }

            foreach (var uroute in unlicensedRoute)
            {
                _childRouteDetailsDTO.Add(uroute);
            }

            _formularyDTO.FormularyRouteDetails = _childRouteDetailsDTO.ToList();
        }

        public override void CreateLocalRouteDetails()
        {
            //_formularyDTO.FormularyLocalRouteDetails = new List<FormularyLocalRouteDetailDTO>();

            //if (_childLocalRouteDetailsDTO.IsCollectionValid())
            //{
            //    var distinctRoutes = _childLocalRouteDetailsDTO.Distinct(rec => rec.RouteCd).ToList();

            //    _formularyDTO.FormularyLocalRouteDetails.AddRange(distinctRoutes);
            //}

            if (!_childLocalRouteDetailsDTO.IsCollectionValid()) return;

            var localLicensedRoute = _childLocalRouteDetailsDTO.Where(x => x.RouteFieldTypeCd == "003").Distinct(rec => rec.RouteCd).ToList();

            var localUnlicensedRoute = _childLocalRouteDetailsDTO.Where(x => x.RouteFieldTypeCd == "002").Distinct(rec => rec.RouteCd).ToList();

            List<FormularyLocalRouteDetailDTO> tempLocalUnlicensedRoute = new List<FormularyLocalRouteDetailDTO>();

            tempLocalUnlicensedRoute.AddRange(localUnlicensedRoute);

            for (int i = 0; i < localUnlicensedRoute.Count(); i++)
            {
                if (localLicensedRoute.Exists(x => x.RouteCd == localUnlicensedRoute[i].RouteCd))
                {
                    tempLocalUnlicensedRoute.Remove(localUnlicensedRoute[i]);
                }
            }

            _childLocalRouteDetailsDTO.Clear();

            localUnlicensedRoute.Clear();

            localUnlicensedRoute.AddRange(tempLocalUnlicensedRoute);

            foreach (var route in localLicensedRoute)
            {
                _childLocalRouteDetailsDTO.Add(route);
            }

            foreach (var uroute in localUnlicensedRoute)
            {
                _childLocalRouteDetailsDTO.Add(uroute);
            }

            _formularyDTO.FormularyLocalRouteDetails = _childLocalRouteDetailsDTO.ToList();
        }

        public override void CreateDetails()
        {
            base.CreateDetails();

            var anyChildFormularyDetailDTO = _childFormulariesDetailDTO.FirstOrDefault() ?? new FormularyDetailDTO();

            if (!_childFormularies.IsCollectionValid()) return;

            //Overwrite VMP properties from AMP
            var detailDTO = _formularyDTO.Detail;
            detailDTO.LicensedUses = _childLicensedIndicationsDTO?.Distinct(rec => rec.Cd).ToList();
            detailDTO.UnLicensedUses = _childUnlicensedIndicationsDTO?.Distinct(rec => rec.Cd).ToList();

            detailDTO.LocalLicensedUses = _childLocalLicensedIndicationsDTO?.Distinct(rec => rec.Cd).ToList();
            detailDTO.LocalUnLicensedUses = _childLocalUnlicensedIndicationsDTO?.Distinct(rec => rec.Cd).ToList();

            List<FormularyLookupItemDTO> tempLocalUnlicensedUses = new List<FormularyLookupItemDTO>();

            tempLocalUnlicensedUses.AddRange(detailDTO.LocalUnLicensedUses);

            for (int i = 0; i < detailDTO.LocalUnLicensedUses.Count(); i++)
            {
                if (detailDTO.LocalLicensedUses.Exists(x => x.Cd == detailDTO.LocalUnLicensedUses[i].Cd))
                {
                    tempLocalUnlicensedUses.Remove(detailDTO.LocalUnLicensedUses[i]);
                }
            }

            detailDTO.LocalUnLicensedUses.Clear();

            detailDTO.LocalUnLicensedUses.AddRange(tempLocalUnlicensedUses);

            detailDTO.ChildFormulations = _childFormulationsDTO?.Distinct()?.Select(rec => new FormularyLookupItemDTO { Cd = rec }).ToList();

            detailDTO.ContraIndications = _childContraIndicationsDTO?.Distinct(rec => rec.Cd).ToList();

            AssignCustomWarnings();

            AssignReminders();

            detailDTO.Endorsements = _childEndorsementsDTO?.Distinct().ToList();

            detailDTO.SafetyMessages = _childSafetyMessagesDTO?.Distinct(rec => rec.Cd).ToList();

            detailDTO.MedusaPreparationInstructions = _childMedusaPreparationInstructionsDTO?.Distinct().ToList();

            detailDTO.NiceTas = _childNiceTADTO?.Distinct().ToList();

            detailDTO.SideEffects = _childSideEffectsDTO?.Distinct(rec => rec.Cd).ToList();

            detailDTO.Diluents = _childDiluentsDTO?.Distinct(rec => rec.Cd).ToList();

            AssignBlackTriangle(detailDTO);

            detailDTO.Cautions = _childCautionsDTO?.Distinct(rec => rec.Cd).ToList();

            AssignClinicalTrialFlag(detailDTO);

            AssignIVTOOralFlag(detailDTO);

            AssignCriticalDrug(detailDTO);

            //Controlled drug Category -- TBA
            detailDTO.ControlledDrugCategories = _childControlledDrugCategoriessDTO?.Distinct(rec => rec.Cd).ToList();

            //Retain what is set for the VTM --TBA
            //detailDTO.WitnessingRequired = (detailDTO.ControlledDrugCategoryCd == null || detailDTO.ControlledDrugCategoryCd != "0") ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;

            AssignEMAAdditionalFlag(detailDTO);

            AssignExpensiveMednFlag(detailDTO);

            AssignUnlicensedMednFlag(detailDTO);

            AssignHighAlertMednFlag(detailDTO);

            AssignNotForPRNFlag(detailDTO);

            AssignTitrationTypes(detailDTO);

            AssignIgnoreDuplicateWarningsFlag(detailDTO);

            AssignIsBloodProductFlag(detailDTO);

            AssignIsDiluentFlag(detailDTO);

            AssignIsModifiedReleaseFlag(detailDTO);

            AssignIsGastroResistantFlag(detailDTO);

            AssignFormularyStatusFlag(detailDTO);

            AssignIsCustomControlledDrugFlag(detailDTO);

            AssignIsPrescriptionPrintingRequiredFlag(detailDTO);

            AssignIsIndicationMandatoryFlag(detailDTO);

            detailDTO.ModifiedReleases = _childModifiedReleaseDTO.ToList();
        }

        private void AssignFormularyStatusFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Formulary_Status_Agg"] ?? "any";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.RnohFormularyStatuscd = _childFormulariesDetailDTO?.All(rec => rec.RnohFormularyStatuscd == TerminologyConstants.FORMULARYSTATUS_FORMULARY) == true ? TerminologyConstants.FORMULARYSTATUS_FORMULARY : TerminologyConstants.FORMULARYSTATUS_NONFORMULARY;
            }
            else
            {
                detailDTO.RnohFormularyStatuscd = _childFormulariesDetailDTO?.Any(rec => rec.RnohFormularyStatuscd == TerminologyConstants.FORMULARYSTATUS_FORMULARY) == true ? TerminologyConstants.FORMULARYSTATUS_FORMULARY : TerminologyConstants.FORMULARYSTATUS_NONFORMULARY;
            }
        }

        private void AssignIsGastroResistantFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Is_Gastro_Resistant_Agg"] ?? "all";

            var flagsForVMPs = new List<bool>();

            _childGastroResistantFlagsDTO?.Each(rec =>
            {
                if (rec.Value.IsCollectionValid())
                    flagsForVMPs.Add(GetIsGastroResistantFlagForParents(rec.Value));
            });

            if (string.Compare(cdAggRule, "all", true) == 0)
                detailDTO.IsGastroResistant = flagsForVMPs?.All(rec => rec == true) == true;
            else
                detailDTO.IsGastroResistant = flagsForVMPs?.Any(rec => rec == true) == true;
        }

        private bool GetIsGastroResistantFlagForParents(List<bool?> gastroResistantFlagsDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VMP_Is_Gastro_Resistant_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
                return gastroResistantFlagsDTO?.All(rec => rec == true) == true;
            else
                return gastroResistantFlagsDTO?.Any(rec => rec == true) == true;
        }

        private void AssignIsIndicationMandatoryFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Is_Indication_Mandatory_Agg"] ?? "all";

            var flagsForVMPs = new List<bool>();

            _childIndicationMandatoryFlagsDTO?.Each(rec =>
            {
                if (rec.Value.IsCollectionValid())
                    flagsForVMPs.Add(GetIsIndicationMandatoryFlagForParents(rec.Value));
            });

            if (string.Compare(cdAggRule, "all", true) == 0)
                detailDTO.IsIndicationMandatory = flagsForVMPs?.All(rec => rec == true) == true;
            else
                detailDTO.IsIndicationMandatory = flagsForVMPs?.Any(rec => rec == true) == true;
        }

        private bool GetIsIndicationMandatoryFlagForParents(List<bool?> indicationManadatoryFlagsDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VMP_Is_Indication_Mandatory_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
                return indicationManadatoryFlagsDTO?.All(rec => rec == true) == true;
            else
                return indicationManadatoryFlagsDTO?.Any(rec => rec == true) == true;
        }

        private void AssignIsModifiedReleaseFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Is_Modified_Release_Agg"] ?? "all";

            var flagsForVMPs = new List<bool>();

            _childMdifiedReleaseFlagsDTO?.Each(rec =>
            {
                if (rec.Value.IsCollectionValid())
                    flagsForVMPs.Add(GetIsModifiedReleasetFlagForParents(rec.Value));
            });

            if (string.Compare(cdAggRule, "all", true) == 0)
                detailDTO.IsModifiedRelease = flagsForVMPs?.All(rec => rec == true) == true;
            else
                detailDTO.IsModifiedRelease = flagsForVMPs?.Any(rec => rec == true) == true;
        }

        private bool GetIsModifiedReleasetFlagForParents(List<bool?> modifiedReleasesFlagsDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VMP_Is_Modified_Release_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
                return modifiedReleasesFlagsDTO?.All(rec => rec == true) == true;
            else
                return modifiedReleasesFlagsDTO?.Any(rec => rec == true) == true;
        }

        private void AssignIsDiluentFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Is_Diluent_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.IsDiluent = _childDiluentFlagsDTO?.All(rec => rec == true) == true;
            }
            else
            {
                detailDTO.IsDiluent = _childDiluentFlagsDTO?.Any(rec => rec == true) == true;
            }
        }

        private void AssignIsBloodProductFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Is_Blood_Product_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.IsBloodProduct = _childBloodProductFlagsDTO?.All(rec => rec == true) == true;
            }
            else
            {
                detailDTO.IsBloodProduct = _childBloodProductFlagsDTO?.Any(rec => rec == true) == true;
            }
        }

        private void AssignIgnoreDuplicateWarningsFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Ignore_Dup_warnings_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.IgnoreDuplicateWarnings = _childIgnoreDuplicateWarningsFlagsDTO?.All(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
            else
            {
                detailDTO.IgnoreDuplicateWarnings = _childIgnoreDuplicateWarningsFlagsDTO?.Any(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
        }

        private void AssignTitrationTypes(FormularyDetailDTO detailDTO)
        {
            detailDTO.TitrationTypes = new List<FormularyLookupItemDTO>();

            _childTitrationTypeDTO?.Distinct(rec => rec.Cd)?.Each(rec =>
            {
                detailDTO.TitrationTypes.Add(rec);
            });
        }


        private void AssignNotForPRNFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Not_For_PRN_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.NotForPrn = _childNotForPRNFlagsDTO?.All(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
            else
            {
                detailDTO.NotForPrn = _childNotForPRNFlagsDTO?.Any(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
        }

        private void AssignHighAlertMednFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_High_Alert_Med_Agg"] ?? "any";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.HighAlertMedication = _childHighAlertMedicationFlagsDTO?.All(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
            else
            {
                detailDTO.HighAlertMedication = _childHighAlertMedicationFlagsDTO?.Any(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
        }

        private void AssignExpensiveMednFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Expensive_Med_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.ExpensiveMedication = _childExpensiveMedicationFlagsDTO?.All(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
            else
            {
                detailDTO.ExpensiveMedication = _childExpensiveMedicationFlagsDTO?.Any(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
        }

        private void AssignUnlicensedMednFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Unlicensed_Med_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.UnlicensedMedicationCd = _childUnlicensedMedicationFlagsDTO?.All(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
            else
            {
                detailDTO.UnlicensedMedicationCd = _childUnlicensedMedicationFlagsDTO?.Any(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
        }

        private void AssignEMAAdditionalFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_EMA_Addnl_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.EmaAdditionalMonitoring = _childEMAAdditonalFlagsDTO?.All(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
            else
            {
                detailDTO.EmaAdditionalMonitoring = _childEMAAdditonalFlagsDTO?.Any(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
        }

        private void AssignIVTOOralFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_IV_TO_Oral_Agg"] ?? "any";

            var flagForVMPs = new List<bool>();

            _childIVTOOralFlagsDTO.Each(k =>
            {
                if (k.Value.IsCollectionValid())
                    flagForVMPs.Add(GetIVTOOralFlagForVMPs(k.Value));
            });

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.IvToOral = flagForVMPs?.All(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
            else
            {
                detailDTO.IvToOral = flagForVMPs?.Any(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
        }

        private void AssignCriticalDrug(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Critical_Drug_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                //If All child VMPs are set with critical then set to true
                detailDTO.CriticalDrug = _childCriticalDrugFlagsDTO?.All(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : null;
            }
            else
            {
                detailDTO.CriticalDrug = _childCriticalDrugFlagsDTO?.Any(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : null;
            }
        }

        private bool GetIVTOOralFlagForVMPs(List<bool> flagVals)
        {
            var cdAggRule = _configuration["Formulary_Rules:VMP_IV_TO_Oral_Agg"] ?? "any";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                return flagVals?.All(rec => rec) == true;
            }
            else
            {
                return flagVals?.Any(rec => rec) == true;
            }
        }

        private void AssignClinicalTrialFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Clinical_Trial_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.ClinicalTrialMedication = _childClinicalTrialFlagsDTO?.All(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
            else
            {
                detailDTO.ClinicalTrialMedication = _childClinicalTrialFlagsDTO?.Any(rec => rec) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
        }


        private void AssignBlackTriangle(FormularyDetailDTO detailDTO)
        {
            var btAggRule = _configuration["Formulary_Rules:VTM_Black_Triangle_Agg"] ?? "all";

            if (string.Compare(btAggRule, "all", true) == 0)
            {
                //If All child VMPs are set with blacktriangle then set to true
                detailDTO.BlackTriangle = _childBlackTrianglesDTO?.All(rec => rec == TerminologyConstants.STRINGIFIED_BOOL_TRUE) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : null;
            }
            else
            {
                detailDTO.BlackTriangle = _childBlackTrianglesDTO?.Any(rec => rec == TerminologyConstants.STRINGIFIED_BOOL_TRUE) == true ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : null;
            }
        }

        private void AssignCustomWarnings()
        {
            var aggRule = _configuration["Formulary_Rules:VTM_Custom_Warning_Agg"] ?? "all";

            if (string.Compare(aggRule, "all", true) == 0)
            {
                //Check if all AMPs has Custom warnings
                if (!_childFormularies.IsCollectionValid() || !_childFormulariesDetailDTO.IsCollectionValid()) return;

                if (_childFormularies.IsCollectionValid() && _childFormulariesDetailDTO.IsCollectionValid() && _childFormularies.Count != _childFormulariesDetailDTO.Count) return;

                //Check if all details has custom warnings
                var allVMPsHasWarnings = _childFormulariesDetailDTO.All(rec => rec.CustomWarnings.IsCollectionValid());

                if (!allVMPsHasWarnings) return;

                var detailDTO = _formularyDTO.Detail;

                detailDTO.CustomWarnings = _childCustomWarningsDTO?.GroupBy(cw => new { cw.Warning, cw.NeedResponse, cw.Source }).SelectMany(cw => cw.Skip(_childFormulariesDetailDTO.Count - 1)).ToList();
            }
            else
            {
                _formularyDTO.Detail.CustomWarnings = _childCustomWarningsDTO?.GroupBy(cw => new { cw.Warning, cw.NeedResponse, cw.Source }).Select(cw => cw.FirstOrDefault()).ToList();
            }
        }

        private void AssignReminders()
        {
            var aggRule = _configuration["Formulary_Rules:VTM_Reminder_Agg"] ?? "all";

            if (string.Compare(aggRule, "all", true) == 0)
            {
                //Check if all AMPs has Reminders
                if (!_childFormularies.IsCollectionValid() || !_childFormulariesDetailDTO.IsCollectionValid()) return;

                if (_childFormularies.IsCollectionValid() && _childFormulariesDetailDTO.IsCollectionValid() && _childFormularies.Count != _childFormulariesDetailDTO.Count) return;

                //Check if all details has reminders
                var allVMPsHasReminders = _childFormulariesDetailDTO.All(rec => rec.Reminders.IsCollectionValid());

                if (!allVMPsHasReminders) return;

                var detailDTO = _formularyDTO.Detail;

                detailDTO.Reminders = _childRemindersDTO?.GroupBy(rem => new { rem.Reminder, rem.Duration, rem.Active, rem.Source }).SelectMany(rem => rem.Skip(_childFormulariesDetailDTO.Count - 1)).ToList();
            }
            else
            {
                _formularyDTO.Detail.Reminders = _childRemindersDTO?.GroupBy(rem => new { rem.Reminder, rem.Duration, rem.Active, rem.Source }).Select(rem => rem.FirstOrDefault()).ToList();
            }
        }

        private void AssignIsCustomControlledDrugFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Is_Custom_Controlled"] ?? "any";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.IsCustomControlledDrug = _childCustomComtrolledDrugFlagsDTO?.All(rec => rec == true) == true;
            }
            else
            {
                detailDTO.IsCustomControlledDrug = _childCustomComtrolledDrugFlagsDTO?.Any(rec => rec == true) == true;
            }
        }

        private void AssignIsPrescriptionPrintingRequiredFlag(FormularyDetailDTO detailDTO)
        {
            var cdAggRule = _configuration["Formulary_Rules:VTM_Is_Prescription_Printing_Required"] ?? "any";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                detailDTO.IsPrescriptionPrintingRequired = _childPrescriptionPrintingRequiredFlagsDTO?.All(rec => rec == true) == true;
            }
            else
            {
                detailDTO.IsPrescriptionPrintingRequired = _childPrescriptionPrintingRequiredFlagsDTO?.Any(rec => rec == true) == true;
            }
        }

        private void ProjectClassificationCodesFromChildNodes()
        {
            if (!_childAdditionalCodesDTO.IsCollectionValid()) return;

            //Project the classification codes from AMP to VTM
            //Only if each classification type has only one record
            var codeSystemLkp = new ConcurrentDictionary<string, Dictionary<string, FormularyAdditionalCodeDTO>>();

            var classificationTypeCodes = _childAdditionalCodesDTO
                   .Where(rec => string.Compare(rec.CodeType, TerminologyConstants.CODE_SYSTEM_CLASSIFICATION_TYPE, true) == 0)?
                   .Distinct()?.ToList();

            //Check whether each type has only one record
            if (classificationTypeCodes.IsCollectionValid())
            {
                classificationTypeCodes.Each(rec =>
                {
                    if (codeSystemLkp.ContainsKey(rec.AdditionalCodeSystem))
                    {
                        if (!codeSystemLkp[rec.AdditionalCodeSystem].ContainsKey(rec.AdditionalCode))
                        {
                            codeSystemLkp[rec.AdditionalCodeSystem].Add(rec.AdditionalCode, rec);
                        }
                    }
                    else
                    {
                        codeSystemLkp[rec.AdditionalCodeSystem] = new Dictionary<string, FormularyAdditionalCodeDTO> { { rec.AdditionalCode, rec } };
                    }
                });
            }

            if (!codeSystemLkp.IsCollectionValid()) return;

            foreach (var csItem in codeSystemLkp)
            {
                //Consider that code system only if it doesn not has same code for that classification code system
                if (csItem.Value != null && csItem.Value.Count > 0)
                {
                    var codeData = csItem.Value.First().Value;
                    if (codeData != null)
                    {
                        if (codeData.AdditionalCodeSystem.ToLower() == "bnf")
                        {
                            codeData.AdditionalCode = codeData.AdditionalCode.Substring(0, 7);
                        }

                        _formularyDTO.FormularyAdditionalCodes.Add(codeData);
                    }
                }
            }
        }
    }


}
