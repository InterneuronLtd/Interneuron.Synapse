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


﻿using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.Model.DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public interface IDMDQueries
    {
        Task<DMDSearchResultsDTO> SearchDMD(string term);

        Task<DMDSearchResultsWithHierarchyDTO> SearchDMDNamesGetWithAllDescendents(string searchTerm);

        Task<DMDSearchResultsWithHierarchyDTO> SearchDMDNamesGetWithTopNodes(string searchTerm);

        Task<List<DMDSearchResultWithTreeDTO>> GetDMDAncestorForCodes(string[] codes);

        Task<List<DMDSearchResultWithTreeDTO>> GetDMDDescendentForCodes(string[] codes);

        Task<List<DMDDetailResultDTO>> GetDMDFullDataForCodes(string[] codes);

        Task<List<T>> GetLookup<T>(LookupType lookType) where T: ILookupItemDTO;

        List<string> GetAllDMDCodes();

        List<DmdAmpExcipientDTO> GetAMPExcipientsForCodes(List<string> dmdCodes);

        List<DmdAmpDrugrouteDTO> GetAMPDrugRoutesForCodes(List<string> dmdCodes);

        List<DmdVmpDrugrouteDTO> GetVMPDrugRoutesForCodes(List<string> dmdCodes);

        Task<List<DmdBNFCodeDTO>> GetAllBNFCodesFromDMD();

        Task<List<DmdATCCodeDTO>> GetAllATCCodesFromDMD();

        Task<DMDSearchResultsWithHierarchyDTO> SearchDMDNamesGetWithAllLevelNodes(string searchTerm);

        List<DmdSyncLog> GetDMDSyncLogs();

        List<DmdSyncLog> GetDMDPendingSyncLogs();

        Task<DMDSearchResultsWithHierarchyDTO> SearchDMDSyncLog(string searchTerm);

        DmdSnomedVersion GetDmdSnomedVersion();
    }
}
