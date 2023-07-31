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
﻿//using Interneuron.CareRecord.Infrastructure.Domain;
//using Interneuron.CareRecord.Service.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Threading.Tasks;

//namespace Interneuron.CareRecord.Service
//{
//    public class SynapseResourceService<T>: ISynapseResourceService<T> where T: EntityBase
//    {
//        private IReadOnlyRepository<T> _readonlyRepo;
//        private IRepository<T> _repository;
//        private IServiceProvider _provider;

//        public SynapseResourceService(IReadOnlyRepository<T> readonlyRepo, IRepository<T> repository, IServiceProvider provider)
//        {
//            this._readonlyRepo = readonlyRepo;

//            this._repository = repository;

//            this._provider = provider;
//        }

//        public IQueryable<T> GetQuerableWithCondition(Expression<Func<T, bool>> condition)
//        {
//            return this._readonlyRepo.ItemsAsReadOnly.Where(condition);
//        }

//        public IQueryable<T> GetQuerable()
//        {
//            return this._readonlyRepo.ItemsAsReadOnly;
//        }

//        public Task<IQueryable<T>> GetQuerableAsync()
//        {
//            return Task.Run(()=> this._readonlyRepo.ItemsAsReadOnly);
//        }

//        public Task<List<T>> GetEnityListAsync()
//        {
//            return this._readonlyRepo.ItemsAsListAsyncReadOnly;
//        }

//        /// <summary>
//        /// Any change for this entity will be tacked and saved to the store
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        public IQueryable<T> GetEditableQuerable(T entity)
//        {
//            return this._repository.Items;
//        }

//        /// <summary>
//        /// Any change for this entity will be tacked and saved to the store
//        /// </summary>
//        /// <param name="entity"></param>
//        /// <returns></returns>
//        public Task<IQueryable<T>> GetEditableQuerableAsync()
//        {
//            return Task.Run(() => this._repository.Items);
//        }

//        /// <summary>
//        /// This will add the entity to the store.
//        /// Need to invoke the Save method to complete the transaction
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <param name="entity"></param>
//        public void Add(T entity)
//        {
//            this._repository.Add(entity);
//        }

//        public void Update(T entity)
//        {
//            this._repository.Update(entity);
//        }

//        public void Delete(T entity)
//        {
//            this._repository.Remove(entity);
//        }

//        public int SaveChanges()
//        {
//            return this._repository.SaveChanges();
//        }

//        public Task<int> SaveChangesAsync()
//        {
//            return this._repository.SaveChangesAsync();
//        }
//    }
//}
