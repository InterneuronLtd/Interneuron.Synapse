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
using Hl7.Fhir.Model;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.CareRecord.API.AppCode.Extensions;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
using Interneuron.CareRecord.HL7SynapseService.Interfaces;
using Interneuron.CareRecord.HL7SynapseService.Models;
using Interneuron.Infrastructure.CustomExceptions;

namespace Interneuron.CareRecord.API.AppCode
{
    public class ReadQueryHandler : IQueryHandler
    {
        private IServiceProvider _provider;

        public ReadQueryHandler(IServiceProvider provider)
        {
            this._provider = provider;
        }

        public FhirResponse Handle(IKey key)
        {
            Validate.HasTypeName(key);
            Validate.HasResourceId(key);
            Validate.HasNoVersion(key);
            Validate.Key(key);

            var hl7Type = key.TypeName.GetHl7ModelType();

            //Startup.AutofacContainer.Resolve<ISynapseResourceService<CorePerson>>();

            var modelFactory = this._provider.GetService(typeof(ResourceQueryHandlerFactory)) as ResourceQueryHandlerFactory;
            
            IResourceQueryHandler synapseModelHandler = modelFactory.GetHandler(key.TypeName);

            var fhirParam = FHIRParam.Create(key.TypeName, key.ResourceId, key.VersionId);

            var resourceData = synapseModelHandler.Handle(fhirParam);

            var fhirResponseFactory = this._provider.GetService(typeof(IFhirResponseFactory)) as IFhirResponseFactory;

            return fhirResponseFactory.GetFhirResponse(resourceData, key);
        }

        public FhirResponse Handle(List<IKey> keys)
        {
            Dictionary<string, FhirResponse> fhirResponse = new Dictionary<string, FhirResponse>();

            foreach (var key in keys)
            {
                Validate.HasTypeName(key);
                Validate.HasResourceId(key);
                Validate.HasNoVersion(key);
                Validate.Key(key);

                var hl7Type = key.TypeName.GetHl7ModelType();

                var modelFactory = this._provider.GetService(typeof(ResourceQueryHandlerFactory)) as ResourceQueryHandlerFactory;

                IResourceQueryHandler synapseModelHandler = modelFactory.GetHandler(key.TypeName);

                var fhirParam = FHIRParam.Create(key.TypeName, key.ResourceId, key.VersionId);

                var resourceData = synapseModelHandler.Handle(fhirParam);

                var fhirResponseFactory = this._provider.GetService(typeof(IFhirResponseFactory)) as IFhirResponseFactory;

                fhirResponse[key.TypeName] = fhirResponseFactory.GetFhirResponse(resourceData, key);
            }

            return ProcessFhirResponses(fhirResponse);
        }

        private FhirResponse ProcessFhirResponses(Dictionary<string, FhirResponse> fhirResponse)
        {            
            if (fhirResponse.ContainsKey("Patient"))
            {
                if (fhirResponse["Patient"].Resource.TypeName == nameof(OperationOutcome))
                {
                    return fhirResponse["Patient"];
                }
            }

            Bundle bundle = new Bundle();
            bundle.Entry = new List<Bundle.EntryComponent>();

            foreach (var item in fhirResponse)
            {
                if (item.Value.Resource.TypeName == nameof(OperationOutcome))
                    continue;

                if (item.Value.Resource.TypeName == nameof(Bundle))
                {
                    foreach (var entry in (item.Value.Resource as Bundle).Entry)
                    {
                        bundle.Entry.Add(entry);
                    }
                }
                else 
                {
                    Bundle.EntryComponent bundleEntry = new Bundle.EntryComponent();
                    bundleEntry.Resource = item.Value.Resource;
                    bundle.Entry.Add(bundleEntry);
                }
            }

            return new FhirResponse(System.Net.HttpStatusCode.OK, bundle);
        }

        public FhirResponse Handle(IKey key, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public FhirResponse Handle(IKey key, IEnumerable<object> parameters = null)
        {
            throw new NotImplementedException();
        }

        public SynapseResourceData Handle(string personId)
        {
            throw new NotImplementedException();
        }
    }
}

