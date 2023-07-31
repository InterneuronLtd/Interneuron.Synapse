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
        public async Task<List<FormularySearchResultDTO>> GetFormulariesAsDiluents()
        {
            //These are the VMPS any child of which is marked diluent

            //1. Get All AMPs which are marked as diluent
            var detailRepo = _provider.GetService(typeof(IFormularyRepository<FormularyDetail>)) as IFormularyRepository<FormularyDetail>;

            var forularyIdsAsDiluents = detailRepo.ItemsAsReadOnly.Where(rec => rec.IsDiluent == true).Select(rec => rec.FormularyVersionId).ToList();

            if (!forularyIdsAsDiluents.IsCollectionValid()) return null;

            var repo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var ampsAsDiluents = repo.ItemsAsReadOnly.Where(rec => rec.IsLatest == true && rec.ProductType == "AMP" && forularyIdsAsDiluents.Contains(rec.FormularyVersionId) && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED);

            if (!ampsAsDiluents.IsCollectionValid()) return null;

            //2. get vmps for these amps
            var ampsAsDiluentsCodes = ampsAsDiluents.Select(rec => rec.Code).Distinct().ToArray();

            var searchRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyBasicSearchResultModel>)) as IFormularyRepository<FormularyBasicSearchResultModel>;

            var vmpsForAmps = await searchRepo.GetFormularyAncestorForCodes(ampsAsDiluentsCodes);

            if (!vmpsForAmps.IsCollectionValid()) return null;

            //filter only to vmps
            vmpsForAmps = vmpsForAmps.Where(rec => string.Compare(rec.ProductType, "vmp", true) == 0).ToList();

            var vmpCodesForAmps = vmpsForAmps.Select(rec => rec.Code).ToArray();

            //3. get all amps to these vmps
            var ampsForVMPs = await searchRepo.GetFormularyDescendentForCodes(vmpCodesForAmps);

            if (!ampsForVMPs.IsCollectionValid()) return null;

            var uniqueAMpIds = ampsForVMPs.Select(rec => rec.FormularyVersionId).Distinct().ToList();

            //Get full details for these amps
            var ampDetailsForVMPs = repo.GetFormularyListForIds(uniqueAMpIds);

            if (!ampDetailsForVMPs.IsCollectionValid()) return null;

            var ampListForVMPKey = new ConcurrentDictionary<string, List<bool>>();

            ampDetailsForVMPs.AsParallel().Each(rec =>
            {
                if (rec.ParentCode.IsNotEmpty() && rec.FormularyDetail.IsCollectionValid())
                {
                    var detail = rec.FormularyDetail.First();

                    if (ampListForVMPKey.ContainsKey(rec.ParentCode))
                    {
                        ampListForVMPKey[rec.ParentCode].Add(detail.IsDiluent == true);
                    }
                    else
                    {
                        ampListForVMPKey[rec.ParentCode] = new List<bool> { detail.IsDiluent == true };
                    }
                }
            });

            var diluentVMPs = new ConcurrentBag<FormularyBasicSearchResultModel>();

            vmpsForAmps.AsParallel().Each(vmp =>
            {
                if (vmp.Code.IsNotEmpty())
                {
                    if (ampListForVMPKey.ContainsKey(vmp.Code) && IsVMPDiluent(ampListForVMPKey[vmp.Code]))
                    {
                        diluentVMPs.Add(vmp);
                    }
                }
            });

            var dto = _mapper.Map<List<FormularySearchResultDTO>>(diluentVMPs.ToList());

            return dto;
        }

        private bool IsVMPDiluent(List<bool> lists)
        {
            var cdAggRule = _configuration["Formulary_Rules:VMP_Is_Diluent_Agg"] ?? "all";

            if (string.Compare(cdAggRule, "all", true) == 0)
            {
                return lists?.All(rec => rec == true) == true;
            }
            else
            {
                return lists?.Any(rec => rec == true) == true;
            }
        }

    }
}
