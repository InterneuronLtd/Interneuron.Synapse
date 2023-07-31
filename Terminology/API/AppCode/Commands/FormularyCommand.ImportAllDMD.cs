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
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Commands
{
    public partial class FormularyCommand : IFormularyCommands
    {
        public async Task<StatusDTO> ImportAllDMDCodes()
        {
            var status = new StatusDTO { StatusCode = TerminologyConstants.STATUS_SUCCESS, StatusMessage = "", ErrorMessages = new List<string>() };

            var dmdQueries = this._provider.GetService(typeof(IDMDQueries)) as IDMDQueries;

            //UnComment to test
            //var dmdResults = new List<string>();

            //Comment to test
            var dmdResults = dmdQueries.GetAllDMDCodes();

            //To be uncommented
            if (!dmdResults.IsCollectionValid())
            {
                status.StatusCode = TerminologyConstants.STATUS_FAIL;
                status.ErrorMessages.Add("No DMD records to import");
                return status;
            }

            //Un-Comment to test
            //dmdResults = GetTestCodes();

            await InvokeImportByCodes(dmdResults);

            await InvokePostImportProcess();

            return status;
        }

        private List<string> GetTestCodes()
        {
            //BNF codes
            //return new List<string> { "18595711000001104", "18595211000001106", "18594011000001100" };

            //ATC Codes
            //return new List<string> { "34558411000001109", "37501311000001106", "34555811000001102", "34554111000001105", "37495811000001106" };

            //TradeFamiliesTest
            //return new List<string> { "3136211000001100", "12516911000001104", "3136311000001108", "12517211000001105" };

            //Non-Scheduled drugs
            return new List<string> { "414984009", "36129811000001103", "19671011000001103", "37995711000001100", "37761311000001108" };

            //Modified-Release
            //return new List<string> { "33664007", "35891911000001105", "664611000001106", "24194011000001109" };
            //Rule 1: Multi-ingredient 
            //return new List<string> { "423037001", "9553411000001100", "9553411000001100" };

            //Rule 3: Co- medicines test
            //return new List<string> { "412556009", "322379008", "841911000001103" };

            //Rule 8 testing:
            //return new List<string> { "91143003", "320177008", "3385711000001103", "13566111000001109", "13533711000001109" };

            //Generic testing
            //return new List<string>
            //{
            //    "13445811000001109",
            //    "7724511000001105",
            //    "7724711000001100",
            //    "7724911000001103",
            //    "7725111000001102",
            //    "13445411000001107",
            //    "7663511000001107",
            //    "7663811000001105",
            //    "350309002",
            //    "348315009",
            //    "4929311000001103",
            //    "4929511000001109",
            //    "8334611000001100",
            //    "9482611000001106",
            //    "17223011000001106",
            //    "17987211000001100",
            //    "18677911000001109",
            //    "18749611000001102",
            //    "21531711000001100",
            //    "34965011000001107",
            //    "358501001",
            //    "4928511000001103",
            //    "11507911000001103",
            //    "19568611000001102",
            //    "23133011000001100",
            //    "28261911000001107",
            //    "31994611000001105",
            //    "31017211000001105",
            //    "4935111000001100",
            //    "4931111000001102"
            //};
        }

        private async Task InvokeImportByCodes(List<string> dmdCodes)
        {
            var configuredBatchSize = _configuration.GetSection("TerminologyConfig").GetValue<int>("BulkImportBatchSize");

            var batchsize = configuredBatchSize;

            var batchedRequests = new List<List<string>>();

            for (var reqIndex = 0; reqIndex < dmdCodes.Count; reqIndex += batchsize)
            {
                var batches = dmdCodes.Skip(reqIndex).Take(batchsize);
                batchedRequests.Add(batches.ToList());
            }

            foreach (var batchedReq in batchedRequests)
            {
                await this.ImportByCodes(batchedReq, defaultFormularyStatusCode: TerminologyConstants.FORMULARYSTATUS_NONFORMULARY, defaultRecordStatusCode: TerminologyConstants.RECORDSTATUS_ACTIVE);
            }
        }
    }


}
