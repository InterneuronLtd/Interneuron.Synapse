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


﻿using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.Queries;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using Interneuron.Terminology.Model.Search;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Commands
{
    public partial class FormularyCommand : IFormularyCommands
    {
        public async Task InvokePostImportProcessForCodes(List<string> codes)
        {
            var allCodes = new List<string>();

            if (!codes.IsCollectionValid()) return;

            allCodes.AddRange(codes);

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var formularyBasicResultsRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyBasicSearchResultModel>)) as IFormularyRepository<FormularyBasicSearchResultModel>;

            var ancestors = await formularyBasicResultsRepo.GetFormularyAncestorForCodes(codes.ToArray());

            var descendents = await formularyBasicResultsRepo.GetFormularyDescendentForCodes(codes.ToArray());

            if (ancestors.IsCollectionValid())
            {
                var ancestorUniqueCodes = ancestors.Select(rec => rec.Code).Distinct().ToList();
                allCodes.AddRange(ancestorUniqueCodes);
            }

            if (descendents.IsCollectionValid())
            {
                var descendentUniqueCodes = descendents.Select(rec => rec.Code).Distinct().ToList();
                allCodes.AddRange(descendentUniqueCodes);
            }

            var allUniqueCodes = allCodes.Distinct().ToArray();
            var allWithDetails = formularyRepo.GetLatestFormulariesByCodes(allUniqueCodes).ToList();

            var vtms = allWithDetails.Where(rec => string.Compare(rec.ProductType, "vtm", true) == 0).ToList();

            var vmps = allWithDetails.Where(rec => string.Compare(rec.ProductType, "vmp", true) == 0).ToList();

            if (vtms.IsCollectionValid())
            {
                foreach (var vtm in vtms)
                {
                    var vmpsForVTM = allWithDetails.Where(rec => rec.ParentCode == vtm.Code).ToList();

                    if (vmpsForVTM.IsCollectionValid())
                    {
                        AssignVTMsWithVMPProps(vmpsForVTM, vtm, formularyRepo);

                        foreach (var vmp in vmpsForVTM)
                        {
                            //var ampsForVMP = allWithDetails.Where(rec => rec.ParentCode == vmp.Code && rec.RecStatusCode == TerminologyConstants.RECORDSTATUS_DRAFT).ToList();
                            var ampsForVMP = allWithDetails.Where(rec => rec.IsLatest == true && rec.ParentCode == vmp.Code && (rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED)).ToList();

                            if (ampsForVMP.IsCollectionValid())
                                await ProcessInhalationRule8(ampsForVMP, vtm, formularyRepo);
                        }
                    }
                }
            }

            if (vmps.IsCollectionValid())
            {
                vmps.Each(vmp =>
                {
                    //var ampsForVMP = allWithDetails.Where(rec => rec.ParentCode == vmp.Code && rec.RecStatusCode == TerminologyConstants.RECORDSTATUS_DRAFT).ToList();
                    var ampsForVMP = allWithDetails.Where(rec => rec.IsLatest == true && rec.ParentCode == vmp.Code && (rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED)).ToList();

                    if (ampsForVMP.IsCollectionValid())
                        AssignAMPsWithVMPProps(ampsForVMP, vmp, formularyRepo);
                });
            }

            formularyRepo.SaveChanges();
        }
        public async Task InvokePostImportProcess()
        {
            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            PopulateAMPFromVMPs();
            PopulateVTMFromVMPs();

            await ProcessInhalationRule8();

            await CreateCopyOfRoutes();

            await UpdateLicensedAndUnlicensedUse();

            formularyRepo.SaveChanges();
        }

        private void PopulateVTMFromVMPs()
        {
            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var allFormularyVTMCodes = formularyRepo.ItemsAsReadOnly
                .Where(rec => rec.ProductType.ToLower() == "vtm")
                .Select(rec => rec.Code)
                .ToList();

            if (!allFormularyVTMCodes.IsCollectionValid()) return;

            var allVtmsWithDetails = formularyRepo.GetLatestFormulariesByCodes(allFormularyVTMCodes.ToArray()).ToList();

            if (!allVtmsWithDetails.IsCollectionValid()) return;

            var vtmDictionary = new ConcurrentDictionary<string, FormularyHeader>();

            allVtmsWithDetails.AsParallel().Each(rec => { vtmDictionary[rec.Code] = rec; });

            var allvmpCodesForVtms = formularyRepo.ItemsAsReadOnly
                .Where(rec => vtmDictionary.Keys.Contains(rec.ParentCode))
                .Select(rec => rec.Code)
                .ToList();
            var allvmpsWithDetails = formularyRepo.GetLatestFormulariesByCodes(allvmpCodesForVtms.ToArray()).ToList();

            if (!allvmpsWithDetails.IsCollectionValid()) return;

            var vmpDictionary = new ConcurrentDictionary<string, List<FormularyHeader>>();
            allvmpsWithDetails.AsParallel().Each(rec =>
            {
                if (!vmpDictionary.ContainsKey(rec.ParentCode))
                    vmpDictionary[rec.ParentCode] = new List<FormularyHeader> { rec };
                else
                    vmpDictionary[rec.ParentCode].Add(rec);
            });

            vtmDictionary.Each(vtm =>
            {
                var vmpsForVTM = vmpDictionary.ContainsKey(vtm.Key) ? vmpDictionary[vtm.Key] : null;
                if (vmpsForVTM.IsCollectionValid())
                    AssignVTMsWithVMPProps(vmpsForVTM, vtm.Value, formularyRepo);
            });
        }

        private async Task ProcessInhalationRule8(List<FormularyHeader> ampsForVTM, FormularyHeader vtm, IFormularyRepository<FormularyHeader> formularyRepo)
        {
            if (!ampsForVTM.IsCollectionValid() || vtm == null && vtm.FormularyDetail.IsCollectionValid()) return;

            var vtmDetail = vtm.FormularyDetail.First();

            var dmdQueries = this._provider.GetService(typeof(IDMDQueries)) as IDMDQueries;

            var routesLookup = await dmdQueries.GetLookup<DmdLookupRouteDTO>(LookupType.DMDRoute);

            var inhalationRoute = routesLookup.Where(rec => string.Compare(rec.Desc, "inhalation", true) == 0).FirstOrDefault();

            if (inhalationRoute == null) return;

            //Check if any of the amp for this vtm has Inhalation as route - then mark it as not prescribable

            var hasInhalationRoute = false;
            foreach (var ampForVTM in ampsForVTM)
            {
                if (ampForVTM.FormularyRouteDetail.IsCollectionValid())
                {
                    hasInhalationRoute = ampForVTM.FormularyRouteDetail.Any(rec => rec.RouteCd == inhalationRoute.Cd);
                    if (hasInhalationRoute) break;
                }
            }

            if (hasInhalationRoute)
            {
                vtmDetail.Prescribable = false;
                vtmDetail.PrescribableSource = TerminologyConstants.DMD_DATA_SRC;
                formularyRepo.Update(vtm);
            }
        }

        private void AssignVTMsWithVMPProps(List<FormularyHeader> vmpsForVTM, FormularyHeader vtm, IFormularyRepository<FormularyHeader> formularyRepo)
        {
            if (!vmpsForVTM.IsCollectionValid()) return;

            var isWitnessRequired = false;
            var details = vmpsForVTM
                .Select(rec => rec.FormularyDetail.FirstOrDefault())?
                .Each(vmpDetail =>
                {
                    //Update witness required flag
                    isWitnessRequired = (vmpDetail != null && vmpDetail.ControlledDrugCategoryCd != "0");
                });

            var vtmDetail = vtm.FormularyDetail.FirstOrDefault();

            if (vtmDetail != null)
            {
                vtmDetail.WitnessingRequired = isWitnessRequired ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
                formularyRepo.Update(vtm);
            }
        }

        private void PopulateAMPFromVMPs()
        {
            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var allFormularyVMPCodes = formularyRepo.ItemsAsReadOnly
                .Where(rec => rec.ProductType.ToLower() == "vmp")
                .Select(rec => rec.Code)
                .ToList();

            if (!allFormularyVMPCodes.IsCollectionValid()) return;

            var allVmpsWithDetails = formularyRepo.GetLatestFormulariesByCodes(allFormularyVMPCodes.ToArray()).ToList();

            if (!allVmpsWithDetails.IsCollectionValid()) return;

            var vmpDictionary = new ConcurrentDictionary<string, FormularyHeader>();

            allVmpsWithDetails.AsParallel().Each(rec => { vmpDictionary[rec.Code] = rec; });

            var allAmpCodesForVmps = formularyRepo.ItemsAsReadOnly
                .Where(rec => vmpDictionary.Keys.Contains(rec.ParentCode))
                .Select(rec => rec.Code)
                .ToList();

            var allAmpsWithDetails = formularyRepo.GetLatestFormulariesByCodes(allAmpCodesForVmps.ToArray()).ToList();

            if (!allAmpsWithDetails.IsCollectionValid()) return;

            var ampDictionary = new ConcurrentDictionary<string, List<FormularyHeader>>();

            allAmpsWithDetails.AsParallel().Each(rec =>
            {
                if (!ampDictionary.ContainsKey(rec.ParentCode))
                {
                    ampDictionary[rec.ParentCode] = new List<FormularyHeader> { rec };
                }
                else
                {
                    ampDictionary[rec.ParentCode].Add(rec);
                }
            });

            vmpDictionary.Each(vmp =>
            {
                var ampsForVMP = ampDictionary.ContainsKey(vmp.Key) ? ampDictionary[vmp.Key] : null;

                if (ampsForVMP.IsCollectionValid())
                {
                    AssignAMPsWithVMPProps(ampsForVMP, vmp.Value, formularyRepo);
                }
            });
        }

        private void AssignAMPsWithVMPProps(List<FormularyHeader> ampsForVMP, FormularyHeader vmp, IFormularyRepository<FormularyHeader> formularyRepo)
        {
            ampsForVMP.Each(amp =>
            {
                PopulateFormularyDetailForAMPFromVMP(amp, vmp);
                PopulateFormularyAdditionalCodesForAMPFromVMP(amp, vmp);
                PopulateFormularyIngredientsForAMPFromVMP(amp, vmp);

                PopulateFormularyUnlicensedRouteForAMPFromVMP(amp, vmp);

                //After replicating to AMP Level - Update AMP to db
                formularyRepo.Update(amp);
            });
        }

        private void PopulateFormularyIngredientsForAMPFromVMP(FormularyHeader amp, FormularyHeader vmp)
        {
            amp.FormularyIngredient = amp.FormularyIngredient ?? new List<FormularyIngredient>();

            amp.FormularyIngredient?.Clear();

            if (vmp.FormularyIngredient.IsCollectionValid())
            {
                foreach (var ing in vmp.FormularyIngredient)
                {
                    var ampIng = _mapper.Map<FormularyIngredient>(ing);//Cloning
                    ampIng.FormularyVersionId = amp.FormularyVersionId;
                    ampIng.RowId = null;//Remove the primary key - should not get copied from vmp

                    amp.FormularyIngredient.Add(ampIng);
                }
            }
        }

        private void PopulateFormularyAdditionalCodesForAMPFromVMP(FormularyHeader amp, FormularyHeader vmp)
        {
            //Copy ATC Code only as BNF is already mapped at AMP level
            amp.FormularyAdditionalCode = amp.FormularyAdditionalCode ?? new List<FormularyAdditionalCode>();

            if (amp.FormularyAdditionalCode.IsCollectionValid())
            {
                var ampAtcs = amp.FormularyAdditionalCode.Where(rec => string.Compare(rec.AdditionalCodeSystem, "atc", true) == 0).ToList();

                ampAtcs?.Each(rec => amp.FormularyAdditionalCode.Remove(rec));
            }

            if (vmp.FormularyAdditionalCode.IsCollectionValid())
            {
                var vmpAtcs = vmp.FormularyAdditionalCode.Where(rec => string.Compare(rec.AdditionalCodeSystem, "atc", true) == 0);

                if (vmpAtcs.IsCollectionValid())
                {
                    foreach (var vmpAtc in vmpAtcs)
                    {
                        //var ampATC = new FormularyAdditionalCode
                        //{
                        //    FormularyVersionId = amp.FormularyVersionId,
                        //    AdditionalCode = vmpAtc.AdditionalCode,
                        //    AdditionalCodeDesc = vmpAtc.AdditionalCodeDesc,
                        //    AdditionalCodeSystem = vmpAtc.AdditionalCodeSystem,
                        //    Attr1 = vmpAtc.Attr1,
                        //    CodeType = vmpAtc.CodeType,
                        //    MetaJson = vmpAtc.MetaJson,
                        //    Source = vmpAtc.Source,
                        //    Tenant = vmpAtc.Tenant
                        //};// _mapper.Map<FormularyAdditionalCode>(atc);//Cloning

                        var ampATC = _mapper.Map<FormularyAdditionalCode>(vmpAtc);//Cloning
                        ampATC.FormularyVersionId = amp.FormularyVersionId;
                        ampATC.RowId = null;//Remove the primary key - should not get copied from vmp

                        amp.FormularyAdditionalCode.Add(ampATC);
                    }
                }
            }
        }

        private void PopulateFormularyDetailForAMPFromVMP(FormularyHeader ampForVMP, FormularyHeader vmp)
        {
            var vmpDetail = vmp.FormularyDetail.First();
            var ampDetail = ampForVMP.FormularyDetail.First();

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;
            var existingFormularies = formularyRepo.GetLatestFormulariesByCodes(new[] { ampForVMP.Code }).ToList();

            ampDetail.BasisOfPreferredNameCd = vmpDetail.BasisOfPreferredNameCd;
            ampDetail.ControlledDrugCategoryCd = vmpDetail.ControlledDrugCategoryCd;
            ampDetail.ControlledDrugCategorySource = vmpDetail.ControlledDrugCategorySource;
            ampDetail.DoseFormCd = vmpDetail.DoseFormCd;
            ampDetail.FormCd = vmpDetail.FormCd;
            ampDetail.CfcFree = vmpDetail.CfcFree;
            ampDetail.GlutenFree = vmpDetail.GlutenFree;
            ampDetail.PrescribingStatusCd = vmpDetail.PrescribingStatusCd;
            ampDetail.PreservativeFree = vmpDetail.PreservativeFree;
            ampDetail.SugarFree = vmpDetail.SugarFree;
            ampDetail.UnitDoseFormSize = vmpDetail.UnitDoseFormSize;
            ampDetail.UnitDoseFormUnits = vmpDetail.UnitDoseFormUnits;
            ampDetail.UnitDoseUnitOfMeasureCd = vmpDetail.UnitDoseUnitOfMeasureCd;

            if (existingFormularies.Any(x => x.RecStatusCode == TerminologyConstants.RECORDSTATUS_ACTIVE) && !existingFormularies.Any(x => x.RecStatusCode == TerminologyConstants.RECORDSTATUS_DRAFT))
            {
                if (ampDetail.ControlledDrugCategoryCd.IsEmpty() || ampDetail.ControlledDrugCategoryCd == "0")
                {
                    ampDetail.IsCustomControlledDrug = false;
                    ampDetail.IsPrescriptionPrintingRequired = false;
                    ampDetail.IsIndicationMandatory = false;
                }
                else
                {
                    ampDetail.IsCustomControlledDrug = true;
                    ampDetail.IsPrescriptionPrintingRequired = true;
                    ampDetail.IsIndicationMandatory = true;
                }
            }
            else if (!existingFormularies.Any(x => x.RecStatusCode == TerminologyConstants.RECORDSTATUS_ACTIVE) && existingFormularies.Any(x => x.RecStatusCode == TerminologyConstants.RECORDSTATUS_DRAFT))
            {
                if (ampDetail.ControlledDrugCategoryCd.IsEmpty() || ampDetail.ControlledDrugCategoryCd == "0")
                {
                    ampDetail.IsCustomControlledDrug = false;
                    ampDetail.IsPrescriptionPrintingRequired = false;
                    ampDetail.IsIndicationMandatory = false;
                }
                else
                {
                    ampDetail.IsCustomControlledDrug = true;
                    ampDetail.IsPrescriptionPrintingRequired = true;
                    ampDetail.IsIndicationMandatory = true;
                }
            }

            if (existingFormularies.Any(x => x.RecStatusCode == TerminologyConstants.RECORDSTATUS_ACTIVE) && !existingFormularies.Any(x => x.RecStatusCode == TerminologyConstants.RECORDSTATUS_DRAFT))
            {
                ampDetail.WitnessingRequired = vmpDetail.ControlledDrugCategoryCd != "0" ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
            else if (!existingFormularies.Any(x => x.RecStatusCode == TerminologyConstants.RECORDSTATUS_ACTIVE) && existingFormularies.Any(x => x.RecStatusCode == TerminologyConstants.RECORDSTATUS_DRAFT))
            {
                ampDetail.WitnessingRequired = vmpDetail.ControlledDrugCategoryCd != "0" ? TerminologyConstants.STRINGIFIED_BOOL_TRUE : TerminologyConstants.STRINGIFIED_BOOL_FALSE;
            }
        }

        private void PopulateFormularyUnlicensedRouteForAMPFromVMP(FormularyHeader amp, FormularyHeader vmp)
        {
            var vmpRoutes = vmp.FormularyRouteDetail;
            var ampRoutes = amp.FormularyRouteDetail;

            if (ampRoutes != null && ampRoutes.Count == 0 && vmpRoutes.IsCollectionValid())
            {
                foreach (var vmpRoute in vmpRoutes)
                {
                    FormularyRouteDetail unlicensedRoute = new FormularyRouteDetail();

                    unlicensedRoute.FormularyVersionId = amp.FormularyVersionId;
                    unlicensedRoute.RouteCd = vmpRoute.RouteCd;
                    unlicensedRoute.RouteFieldTypeCd = TerminologyConstants.ROUTEFIELDTYPE_UNLICENSED;

                    amp.FormularyRouteDetail.Add(unlicensedRoute);
                }
            }
        }

        private async Task ProcessInhalationRule8()
        {
            var dmdQueries = this._provider.GetService(typeof(IDMDQueries)) as IDMDQueries;

            var routesLookup = await dmdQueries.GetLookup<DmdLookupRouteDTO>(LookupType.DMDRoute);

            var inhalationRoute = routesLookup.Where(rec => string.Compare(rec.Desc, "inhalation", true) == 0).FirstOrDefault();

            if (inhalationRoute == null) return;

            //Rule 8: Inhaled dose forms
            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var routeRepo = this._provider.GetService(typeof(IReadOnlyRepository<FormularyRouteDetail>)) as IReadOnlyRepository<FormularyRouteDetail>;

            var inhalationRoutes = routeRepo.ItemsAsReadOnly.Where(rec => rec.RouteCd == inhalationRoute.Cd).ToList();

            if (!inhalationRoutes.IsCollectionValid()) return;

            var uniqueIds = inhalationRoutes.Select(rec => rec.FormularyVersionId).Distinct().ToList();

            var vtmCodes = GetVTMsForAMPIds(uniqueIds);

            var formularyCodesToUpdate = GetFormularyIdsToUpdate(vtmCodes);

            if (!formularyCodesToUpdate.IsCollectionValid()) return;

            var vtmFormulariesToUpdate = formularyRepo.GetLatestFormulariesByCodes(formularyCodesToUpdate.ToArray()).ToList();

            vtmFormulariesToUpdate.Each(rec =>
            {
                var detail = rec.FormularyDetail.FirstOrDefault();

                if (detail != null)
                {
                    detail.Prescribable = false;
                    detail.PrescribableSource = TerminologyConstants.DMD_DATA_SRC;
                    formularyRepo.Update(rec);
                }
            });
        }

        private ConcurrentBag<string> GetFormularyIdsToUpdate(List<string> vtmCodes)
        {
            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var formularyIdsToUpdate = new ConcurrentBag<string>();

            var vtmsWithItsForms = new ConcurrentDictionary<string, HashSet<string>>();

            if (!vtmCodes.IsCollectionValid()) return formularyIdsToUpdate;

            var vmpsForVTMs = formularyRepo.ItemsAsReadOnly
                .Where(rec => vtmCodes.Contains(rec.ParentCode))
                .Select(rec => new { parentCode = rec.ParentCode, detail = rec.FormularyDetail.FirstOrDefault() })
                .ToList();

            if (!vmpsForVTMs.IsCollectionValid()) return formularyIdsToUpdate;

            vmpsForVTMs.AsParallel().Each(vmp =>
            {
                if (vmp.detail != null && vmp.parentCode.IsNotEmpty())
                {
                    if (vtmsWithItsForms.ContainsKey(vmp.parentCode))
                    {
                        if (!vtmsWithItsForms[vmp.parentCode].Contains(vmp.detail.FormCd))
                        {
                            vtmsWithItsForms[vmp.parentCode].Add(vmp.detail.FormCd);
                        }
                    }
                    else
                    {
                        vtmsWithItsForms[vmp.parentCode] = new HashSet<string> { vmp.detail.FormCd };
                    }
                }
            });


            if (!vtmsWithItsForms.IsCollectionValid()) return formularyIdsToUpdate;

            vtmsWithItsForms.AsParallel().Each(vtmWithForm =>
            {
                if (vtmWithForm.Value != null && vtmWithForm.Value.Count > 1)
                {
                    formularyIdsToUpdate.Add(vtmWithForm.Key);
                }
            });

            return formularyIdsToUpdate;
        }

        private List<string> GetVTMsForAMPIds(List<string> uniqueIds)
        {
            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            if (!uniqueIds.IsCollectionValid()) return null;

            var formularyCodesForRoutes = formularyRepo.ItemsAsReadOnly
                 .Where(rec => uniqueIds.Contains(rec.FormularyVersionId))
                 .Select(rec => rec.Code)
                 .ToList();

            if (!formularyCodesForRoutes.IsCollectionValid()) return null;

            var vmpCodesForAMPs = formularyRepo.ItemsAsReadOnly
                .Where(rec => formularyCodesForRoutes.Contains(rec.Code))
                .Select(rec => rec.ParentCode)
                .ToList();

            if (!vmpCodesForAMPs.IsCollectionValid()) return null;

            var vtmCodes = formularyRepo.ItemsAsReadOnly
                .Where(rec => vmpCodesForAMPs.Contains(rec.Code))
                .Select(rec => rec.ParentCode)
                .ToList();

            return vtmCodes;
        }

        private async Task CreateCopyOfRoutes()
        {
            var formularyQueries = this._provider.GetService(typeof(IFormularyQueries)) as IFormularyQueries;

            var routes = await formularyQueries.GetFormulariesRoutes();

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyLocalRouteDetail>)) as IFormularyRepository<FormularyLocalRouteDetail>;

            routes.Each(rec =>
            {
                if (rec != null)
                {
                    rec.Source = TerminologyConstants.MANUAL_DATA_SRC;
                    formularyRepo.Add(_mapper.Map<FormularyLocalRouteDetail>(rec));
                }
            });
        }

        private async Task UpdateLicensedAndUnlicensedUse()
        {
            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyDetail>)) as IFormularyRepository<FormularyDetail>;

            var updateLicensedUses = repo.Items.Where(rec => rec.LicensedUse != null).ToList();

            var updateUnlicensedUses = repo.Items.Where(rec => rec.UnlicensedUse != null).ToList();

            updateLicensedUses?.Each(rec =>
            {
                if (rec.LicensedUse.IsNotEmpty())
                {
                    rec.LocalLicensedUse = rec.LicensedUse.Replace("\"Source\":\"FDB\"", "\"Source\":\"" + TerminologyConstants.MANUAL_DATA_SRC + "\"");

                    repo.Update(rec);
                }
            });

            updateUnlicensedUses?.Each(rec =>
            {
                if (rec.UnlicensedUse.IsNotEmpty())
                {
                    rec.LocalUnlicensedUse = rec.UnlicensedUse.Replace("\"Source\":\"FDB\"", "\"Source\":\"" + TerminologyConstants.MANUAL_DATA_SRC + "\"");

                    repo.Update(rec);
                }
            });

        }
    }
}
