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


using AutoMapper;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Extensions;
using Interneuron.CareRecord.HL7SynapseService.Interfaces;
using Interneuron.CareRecord.HL7SynapseService.Models;
using Interneuron.CareRecord.Infrastructure.Search;
using Interneuron.CareRecord.Repository;
using Interneuron.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeExtensions = Interneuron.CareRecord.HL7SynapseHandler.Service.Extensions.TypeExtensions;

namespace Interneuron.CareRecord.HL7SynapseService.Implementations
{
    public abstract class BaseReadQueryHandler : IResourceQueryHandler
    {
        private IServiceProvider _provider;
        private IMapper _mapper;
        private IGenericSearchRepository _genericSearchRepo;

        protected string _defaultHospitalRefNo;

        public Func<ResourceData> OnNoSearchResults { get; set; }

        public Action<ResourceData> OnPostSuccessfullProcess { get; set; }

        public BaseReadQueryHandler(IServiceProvider provider, IMapper mapper, IGenericSearchRepository genericSearchRepo)
        {
            this._provider = provider;
            this._mapper = mapper;
            this._genericSearchRepo = genericSearchRepo;
            this._defaultHospitalRefNo = GetDefaultHospitalRefNo();
        }

        public virtual ResourceData Handle(IFHIRParam fhirParam)
        {
            string searchEntityIdentifier = GetSarchEntityIdentifier();

            List<SynapseSearchTerm> synapseSearchTerms = GetSynapseSearchTerms();

            var resourceData = new ResourceData(fhirParam);

            var searchResults = this._genericSearchRepo.Search(searchEntityIdentifier, synapseSearchTerms);

            if (searchResults.IsNull() || (searchResults.IsCollectionValid() && searchResults[0] == null))
                return OnNoSearchResults?.Invoke();

            ProcessResult(fhirParam, resourceData, searchResults);

            OnPostSuccessfullProcess?.Invoke(resourceData);

            return resourceData;
        } 

        public abstract string GetSarchEntityIdentifier();

        public abstract List<SynapseSearchTerm> GetSynapseSearchTerms();

        private void ProcessResult(IFHIRParam fhirParam, ResourceData resourceData, List<dynamic> searchResults)
        {
            var mainEntityInResult = searchResults.FirstOrDefault();

            if (mainEntityInResult == null) return;

            dynamic mainEntity = mainEntityInResult;

            //if (mainEntityInResult is System.Collections.IEnumerable)
            //    mainEntity = mainEntityInResult[0];
            //else
            //    mainEntity = mainEntityInResult;

            var resourceEntityType = fhirParam.TypeName.GetHL7ModelType();

            var dataType = TypeExtensions.ConvertToListOrType(resourceEntityType, mainEntity);

            var data = this._mapper.Map(mainEntity, mainEntity.GetType(), dataType);

            AppendWithSubsequentData(resourceEntityType, data, searchResults);

            if (data is System.Collections.IEnumerable && data.Count != 1)
            {
                Hl7.Fhir.Model.Bundle bundle = new Hl7.Fhir.Model.Bundle();

                foreach (var entry in data) 
                {
                    Hl7.Fhir.Model.Bundle.EntryComponent entryComponent = new Hl7.Fhir.Model.Bundle.EntryComponent();
                    entryComponent.Resource = entry;
                    bundle.Entry.Add(entryComponent);
                }

                resourceData.Resource = bundle;
                return;
            }

            resourceData.Resource = data[0];

        }

        private void AppendWithSubsequentData(Type resourceEntityType, dynamic dataItem, List<dynamic> searchResults)
        {
            foreach (var data in dataItem)
            {
                for (int resIndex = 1; resIndex < searchResults.Count; resIndex++)
                {
                    var result = searchResults[resIndex];

                    if (result != null)
                    {
                        if (result is System.Collections.IEnumerable)
                        {
                            foreach (var resultData in result)
                            {
                                if (resultData != null)
                                {
                                    var dataType = TypeExtensions.ConvertToListOrType(resourceEntityType, resultData);
                                    this._mapper.Map(resultData, data, resultData.GetType(), dataType);
                                }
                            }
                        }
                        else
                        {
                            var dataType = TypeExtensions.ConvertToListOrType(resourceEntityType, result);
                            this._mapper.Map(result, data, result.GetType(), dataType);
                        }
                    }
                }
            }
        }

        private string GetDefaultHospitalRefNo()
        {
            IConfiguration configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

            IConfigurationSection careRecordConfig = configuration.GetSection("CareRecordConfig");

            return careRecordConfig.GetValue<string>("HospitalNumberReference");
        }
    }
}
