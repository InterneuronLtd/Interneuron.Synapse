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
using Interneuron.Terminology.API.AppCode.DTOs.SNOMED;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public partial class SnomedCTQueries : ISnomedCTQueries
    {
        public List<SemanticTagDTO> GetSemanticTags()
        {
            var repo = this._provider.GetService(typeof(IReadOnlyRepository<SnomedctLookupSemantictag>)) as IReadOnlyRepository<SnomedctLookupSemantictag>;

            var semanticTagsModel = repo.ItemsAsListReadOnly.ToList();

            var tags = new List<SemanticTagDTO>();

            if (!semanticTagsModel.IsCollectionValid()) return tags;

            semanticTagsModel.OrderBy(d=> d.Domain).Each((d) =>
            {
                var tag = new SemanticTagDTO
                {
                    TagId = d.Id,
                    DomainName = d.Domain,
                    TagName = d.Tag,
                    AllTagNamesInDomain = semanticTagsModel.Where(s => s.Domain == d.Domain).Select(s=> s.Tag).ToList()
                };

                tags.Add(tag);
            });

            return tags;
        }

        public List<SnomedIndicationLookupDTO> GetIndicationsLookup()
        {
            const string IndicationSNOMEDTag = "disorder";

            var repo = this._provider.GetService(typeof(IReadOnlyRepository<SnomedctConceptLookupMat>)) as IReadOnlyRepository<SnomedctConceptLookupMat>;

            var indicationsFromDb = repo.ItemsAsReadOnly.Where(r => r.Semantictag.ToLower() == IndicationSNOMEDTag).ToList();

            if (!indicationsFromDb.IsCollectionValid()) return new List<SnomedIndicationLookupDTO>(); ;

            var lookupData = _mapper.Map<List<SnomedIndicationLookupDTO>>(indicationsFromDb);

            return lookupData;
        }
    }
}
