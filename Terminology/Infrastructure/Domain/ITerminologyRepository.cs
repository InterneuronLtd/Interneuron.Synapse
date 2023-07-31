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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interneuron.Terminology.Infrastructure.Domain
{
    public interface ITerminologyRepository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        IQueryable<TEntity> SearchSnomedTermBySemanticTag(string searchTerm, string semanticTag);

        Task<IEnumerable<TEntity>> SearchSnomedTermsGetWithAllDescendents(string searchTerm, string semanticTag);

        Task<IEnumerable<TEntity>> SearchSnomedTermsGetWithAllAncestors(string searchTerm, string semanticTag);

        Task<IEnumerable<TEntity>> GetSnomedDescendentForConceptIds(string[] conceptId);

        Task<IEnumerable<TEntity>> GetSnomedAncestorForConceptIds(string[] conceptId);

        IEnumerable<TEntity> SearchDMDName(string searchTerm);

        Task<IEnumerable<TEntity>> SearchDMDGetWithAllDescendents(string searchTerm);

        Task<IEnumerable<TEntity>> SearchDMDGetWithAllAncestors(string searchTerm);

        Task<IEnumerable<TEntity>> GetDMDDescendentForCodes(string[] codes);

        Task<IEnumerable<TEntity>> GetDMDAncestorForCodes(string[] codes);

        Task<IEnumerable<TEntity>> GetDMDFullDataForCodes(string[] codes);

        Task<IEnumerable<TEntity>> GetDMDAllDescendentsForCodes(string[] codes);

        Task<IEnumerable<TEntity>> GetDMDAllAncestorsForCodes(string[] codes);
    }

}
