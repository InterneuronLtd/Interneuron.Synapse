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


﻿using AutoMapper;
using Interneuron.Caching;
using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.API.AppCode.Infrastructure.Caching;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public class LookupFactory
    {
        private IServiceProvider _provider;
        private IMapper _mapper;
        private IConfiguration _configuration;

        public LookupFactory(IServiceProvider provider, IMapper mapper, IConfiguration configuration)
        {
            this._provider = provider;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        public async Task<List<T>> GetLookupDTO<T>(LookupType lookupType) where T : ILookupItemDTO
        {
            var dtos = new List<T>();

            var lookupData = CacheService.Get<List<T>>(lookupType.GetCacheKeyByLookupTypeName());

            if (lookupData.IsCollectionValid())
            {
                return lookupData;
            }

            var modelRepoType = GetModelRepoForLookupType(lookupType);

            if (modelRepoType == null) return null;

            dynamic repo = this._provider.GetService(modelRepoType);// as IReadOnlyRepository<DmdLookupControldrugcat>;

            var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

            if (lookupFromDb == null) return dtos;

            //var lookupFromDbAsList = lookupFromDb as List<T>;

            //lookupFromDbAsList = lookupFromDbAsList.Where(r => r.Recordstatus == 1);

            dtos = _mapper.Map<List<T>>(lookupFromDb);

            CacheService.Set(lookupType.GetCacheKeyByLookupTypeName(), dtos);

            return dtos;
        }

        //public async Task<dynamic> GetLookupDTO(LookupType lookupType)
        //{
        //    var modelRepoType = GetModelRepoForLookupType(lookupType);

        //    if (modelRepoType == null) return null;

        //    dynamic dtos = new List<DMDLookupDTO>();

        //    var repo = this._provider.GetService(modelRepoType) as IReadOnlyRepository<EntityBase>;

        //    var lookupFromDb = await repo.ItemsAsListAsyncReadOnly;

        //    if (!lookupFromDb.IsCollectionValid()) return dtos;

        //    var dtoType = typeof(List<DMDLookupDTO>);//to be revisited - tobe decided

        //    //var dtoType = Activator.CreateInstance(typeof(List<>).MakeGenericType("".GetType()), "")

        //    if (dtoType == null) return dtos;

        //    dtos = _mapper.Map(lookupFromDb, dtoType);

        //    return dtos;
        //}



        private Type GetModelRepoForLookupType(LookupType type)//to be moved to ioc
        {
            switch (type)
            {
                case LookupType.DMDAvailRestrictions:
                    return typeof(IReadOnlyRepository<DmdLookupAvailrestrict>);
                case LookupType.DMDBasisOfName:
                    return typeof(IReadOnlyRepository<DmdLookupBasisofname>);
                case LookupType.DMDControlDrugCategory:
                    return typeof(IReadOnlyRepository<DmdLookupControldrugcat>);
                case LookupType.DMDForm:
                    return typeof(IReadOnlyRepository<DmdLookupForm>);
                case LookupType.DMDLicensingAuthority:
                    return typeof(IReadOnlyRepository<DmdLookupLicauth>);
                case LookupType.DMDOntFormRoute:
                    return typeof(IReadOnlyRepository<DmdLookupOntformroute>);
                case LookupType.DMDPrescribingStatus:
                    return typeof(IReadOnlyRepository<DmdLookupPrescribingstatus>);
                case LookupType.DMDRoute:
                    return typeof(IReadOnlyRepository<DmdLookupRoute>);
                case LookupType.DMDSupplier:
                    return typeof(IReadOnlyRepository<DmdLookupSupplier>);
                case LookupType.DMDUOM:
                    return typeof(IReadOnlyRepository<DmdLookupUom>);
                case LookupType.DMDIngredient:
                    return typeof(IReadOnlyRepository<DmdLookupIngredient>);
                case LookupType.DMDDoseForm:
                    return typeof(IReadOnlyRepository<DmdLookupDrugformind>);
                case LookupType.DMDPharamceuticalStrength:
                    return typeof(IReadOnlyRepository<DmdLookupBasisofstrength>);
                case LookupType.ATCCode:
                    return typeof(IReadOnlyRepository<AtcLookup>);
                case LookupType.BNFCode:
                    return typeof(IReadOnlyRepository<BnfLookup>);
                case LookupType.FormularyMedicationType:
                case LookupType.FormularyRules:
                case LookupType.RecordStatus:
                case LookupType.FormularyStatus:
                case LookupType.OrderableStatus:
                case LookupType.TitrationType:
                case LookupType.RoundingFactor:
                case LookupType.Modifier:
                case LookupType.ProductType:
                case LookupType.ModifiedRelease:
                case LookupType.ClassificationCodeType:
                case LookupType.IdentificationCodeType:
                case LookupType.OrderFormType:
                case LookupType.RouteFieldType:
                case LookupType.DrugClass:
                    return typeof(IReadOnlyRepository<LookupCommon>);
                default:
                    return null;
            }
        }
    }

}
