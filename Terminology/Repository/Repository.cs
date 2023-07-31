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
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Repository.DBModelsContext;
using Interneuron.Terminology.Repository.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>, IDisposable where TEntity : EntityBase
    {
        private TerminologyDBContext _dbContext;
        private readonly DbSet<TEntity> _entities;
        private readonly IEnumerable<IEntityEventHandler> _entityEventHandlers;

        public Repository(TerminologyDBContext dbContext, IEnumerable<IEntityEventHandler> entityEventHandlers)
        {
            _dbContext = dbContext;
            _entities = dbContext.Set<TEntity>();
            _entityEventHandlers = entityEventHandlers;
        }

        #region Implementation of IRepository<T>

        public IQueryable<TEntity> Items => _entities;

        public List<TEntity> ItemsAsList => _entities.ToList();

        public Task<List<TEntity>> ItemsAsListAsync => _entities.ToListAsync();

        public IQueryable<TEntity> ItemsAsReadOnly => _entities.AsNoTracking();

        public List<TEntity> ItemsAsListReadOnly => _entities.AsNoTracking().ToList();

        public Task<List<TEntity>> ItemsAsListAsyncReadOnly => _entities.AsNoTracking().ToListAsync();

        public string GetSql()
        {
            return _entities.ToSql();
        }

        public string GetSql<T>(IQueryable<T> query) where T : class
        {
            return query.ToSql();
        }

        //public dynamic ExecuteRawQuery(string command)
        //{
        //    using (var cnn = this._dbContext.Database.GetDbConnection())
        //    {
        //        var cmm = cnn.CreateCommand();
        //        cmm.CommandType = System.Data.CommandType.Text;
        //        cmm.CommandText = "[dbo].[sp_MultiRecordSets]";
        //        cmm.Parameters.AddRange(param);
        //        cmm.Connection = cnn;
        //        cnn.Open();
        //        var reader = cmm.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            string studentName = Convert.ToString(reader["Name"]); // name from student table
        //        }
        //        reader.NextResult(); //move the next record set
        //        while (reader.Read())
        //        {
        //            string city = Convert.ToString(reader["City"]); // city from student address table
        //        }
        //    }
        //}

        public TEntity Update(TEntity entity)
        {
            //InvokeEventHandlers(entity, EventType.Update);
            return entity;
        }

        public bool AddLike(string matchExpression, string pattern)
        {
            return EF.Functions.Like(matchExpression, pattern);
        }

        public bool AddILike(string matchExpression, string pattern)
        {
            return EF.Functions.ILike(matchExpression, pattern);
        }

        public IQueryable<T> AddILike<T>(IQueryable<T> query, Func<T, string> matchExpression, string pattern)
        {
            return query.Where(con => EF.Functions.ILike(matchExpression(con), pattern));
        }

        public void Add(TEntity entity)
        {
            //InvokeEventHandlers(entity, EventType.Add);
            _entities.Add(entity);
        }

        public void AddRange(TEntity[] entities)
        {
            //entities.Each(entity =>
            //{
            //    InvokeEventHandlers(entity, EventType.Add);
            //});
            _entities.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            InvokeEventHandlers(entity, EventType.Delete);
            _entities.Remove(entity);
        }

        public void RemoveRange(TEntity[] entities)
        {
            //entities.Each(entity =>
            //{
            //    InvokeEventHandlers(entity, EventType.Delete);
            //});
            _entities.RemoveRange(entities);
        }


        public int SaveChanges()
        {
            InvokePreSaveHandler();
            return _dbContext.SaveChanges();
        }



        public Task<int> SaveChangesAsync()
        {
            InvokePreSaveHandler();
            return _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        #endregion

        private void InvokePreSaveHandler()
        {
            if (_dbContext.ChangeTracker.Entries().IsCollectionValid())
            {
                _dbContext.ChangeTracker.Entries().Each(entry =>
                {
                    if (entry.State != EntityState.Unchanged || entry.State != EntityState.Detached)
                    {
                        var entryEventType = GetEntryEventType(entry.State);

                        if (entryEventType != EventType.Nop)
                            InvokeEventHandlers(entry.Entity as EntityBase, entryEventType);
                    }
                });
            }
        }

        private void InvokeEventHandlers(EntityBase entity, EventType eventType)
        {
            if (_entityEventHandlers.IsCollectionValid())
                _entityEventHandlers.Each((entityEventHandler) => entityEventHandler.Execute(entity, eventType));
        }

        private EventType GetEntryEventType(EntityState entityState)
        {
            switch (entityState)
            {
                case EntityState.Added:
                    return EventType.Add;
                case EntityState.Modified:
                    return EventType.Update;
                case EntityState.Deleted:
                    return EventType.Delete;
            }
            return EventType.Nop;
        }
    }
}
