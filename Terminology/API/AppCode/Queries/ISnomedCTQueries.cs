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


﻿using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.SNOMED;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public interface ISnomedCTQueries
    {
        List<SemanticTagDTO> GetSemanticTags();

        SnomedSearchResultsDTO SearchSnomedTermBySemanticTag(string term, string semanticTag);

        Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedTermsGetWithAllDescendents(string searchTerm, string semanticTag);

        Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedTermsGetWithAllAncestors(string searchTerm, string semanticTag);

        Task<SnomedSearchResultsWithHierarchyDTO> SearchSnomedTermsGetWithAllNodes(string searchTerm, string semanticTag);

        Task<List<SnomedSearchResultWithTreeDTO>> GetSnomedAncestorForConceptIds(string[] conceptIds);

        Task<List<SnomedSearchResultWithTreeDTO>> GetSnomedDescendentForConceptIds(string[] conceptIds);

        List<SnomedIndicationLookupDTO> GetIndicationsLookup();

        List<SnomedTradeFamiliesDTO> GetTradeFamilyForConceptIds(List<string> conceptIds);

        List<SnomedModifiedReleaseDTO> GetModifiedReleaseForConceptIds(List<string> conceptIds);
    }
}
