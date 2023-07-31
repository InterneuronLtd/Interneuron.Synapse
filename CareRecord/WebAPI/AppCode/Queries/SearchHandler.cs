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


﻿using Hl7.Fhir.Model;
using Hl7.Fhir.Rest;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.CareRecord.API.AppCode.Extensions;
using Interneuron.CareRecord.HL7SynapseService.Interfaces;
using Interneuron.Common.Extensions;
using System;
using System.Linq;

namespace Interneuron.CareRecord.API.AppCode
{
    public partial class SearchHandler : ISearchHandler
    {
        private const string SearchWarningMsg = "bundle-search-warning";

        private IServiceProvider _provider;

        public SearchHandler(IServiceProvider provider)
        {
            this._provider = provider;
        }

        public FhirResponse Handle(IKey key, SearchParams searchParameters)
        {
            //Validate.TypeName(key.TypeName);

            Validate.HasSearchParameters(searchParameters);

            var hl7Type = key.TypeName.GetHl7ModelType();

            var modelFactory = this._provider.GetService(typeof(ResourceSearchHandlerFactory)) as ResourceSearchHandlerFactory;

            IResourceSearchHandler synapseModelHandler = modelFactory.GetHandler(hl7Type);

            var fhirParam = FHIRParam.Create(key.TypeName, key.ResourceId, key.VersionId);

            var searchResultData = synapseModelHandler.Handle(fhirParam, searchParameters);

            var fhirResponseFactory = this._provider.GetService(typeof(IFhirResponseFactory)) as IFhirResponseFactory;

            var resourcesData = searchResultData.ResourcesData;

            Bundle bundle = new Bundle()
            {
                Type = Bundle.BundleType.Searchset,
                Total = resourcesData.Count,
                Id = resourcesData.IsCollectionValid()? Guid.NewGuid().ToString() : SearchWarningMsg
            };
          
            bundle.Append(resourcesData);

            return fhirResponseFactory.GetFhirResponse(bundle);
        }
    }
}
