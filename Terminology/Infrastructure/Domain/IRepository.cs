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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.Infrastructure.Domain
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        IQueryable<TEntity> Items { get; }

        List<TEntity> ItemsAsList { get; }

        Task<List<TEntity>> ItemsAsListAsync { get; }

        IQueryable<TEntity> ItemsAsReadOnly { get; }

        List<TEntity> ItemsAsListReadOnly { get; }

        Task<List<TEntity>> ItemsAsListAsyncReadOnly { get; }

        string GetSql();

        string GetSql<T>(IQueryable<T> query) where T : class;

        bool AddLike(string matchExpression, string pattern);

        bool AddILike(string matchExpression, string pattern);

        IQueryable<T> AddILike<T>(IQueryable<T> query, Func<T, string> matchExpression, string pattern);

        TEntity Update(TEntity entity);
        void Add(TEntity entity);

        void AddRange(TEntity[] entities);

        void Remove(TEntity entity);
        void RemoveRange(TEntity[] entities);

        int SaveChanges();
        Task<int> SaveChangesAsync();
    }

}
