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


﻿//using Hl7.Fhir.Model;
//using Interneuron.CareRecord.API.AppCode.Core;
//using Interneuron.CareRecord.API.AppCode.Extensions.ModelExtensions;
//using Interneuron.CareRecord.Model.DomainModels;
//using Interneuron.CareRecord.Service.Interfaces;
//using Interneuron.Common.Extensions;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace Interneuron.CareRecord.API.AppCode.Queries.ModelHandlers
//{
//    public class ReadEncounterHandler : IResourceSynapseModelHandler
//    {
//        private IServiceProvider _provider;

//        public ReadEncounterHandler(IServiceProvider provider)
//        {
//            this._provider = provider;
//        }

//        public ResourceData Handle(FHIRParam fhirParam)
//        {
//            var resourceData = new ResourceData(fhirParam);

//            var materializedCoreEncounter = GetMaterializedEncounter(fhirParam);

//            if (materializedCoreEncounter == null || materializedCoreEncounter.EncounterId.IsEmpty())
//            {
//                var storeCoreEncounter = CheckInEntityStore(fhirParam);

//                if (storeCoreEncounter == null || storeCoreEncounter.EncounterId == null) return resourceData;

//                if (storeCoreEncounter.Recordstatus == 2) // Encounter in Deleted State
//                {
//                    resourceData.Resource = null;
//                    resourceData.DeletedDate = storeCoreEncounter.Createddate;
//                    resourceData.IsDeleted = true;

//                    return resourceData;
//                }

//                return resourceData;
//            }

//            var encounter = CreateEncounter(materializedCoreEncounter);

//            resourceData.Resource = encounter;

//            return resourceData;
//        }

//        private Encounter CreateEncounter(entitystorematerialised_CoreEncounter encounter)
//        {
//            IConfiguration configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

//            string hospitalNumberReference = configuration["HospitalNumberReference"];

//            var enc = encounter.GetEncounter();

//            AddIdentifier(encounter, enc, hospitalNumberReference);

//            return enc;
//        }

//        private entitystore_CoreEncounter1 CheckInEntityStore(FHIRParam fhirParam)
//        {
//            ISynapseResourceService<entitystore_CoreEncounter1> storeEncounterSvc = this._provider.GetService(typeof(ISynapseResourceService<entitystore_CoreEncounter1>)) as ISynapseResourceService<entitystore_CoreEncounter1>;

//            return storeEncounterSvc.GetQuerable().Where(e => e.EncounterId == fhirParam.ResourceId).OrderByDescending(x => x.Sequenceid).FirstOrDefault();
//        }

//        private entitystorematerialised_CoreEncounter GetMaterializedEncounter(FHIRParam fhirParam)
//        {
//            var coreEncounterService = this._provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CoreEncounter>)) as ISynapseResourceService<entitystorematerialised_CoreEncounter>;

//            return fhirParam.ResourceId.IsNotEmpty() ? coreEncounterService.GetQuerable()
//                 .Where(ce => ce.EncounterId == fhirParam.ResourceId)
//                 .OrderByDescending(ce => ce.Sequenceid)
//                 .FirstOrDefault() : coreEncounterService.GetQuerable().FirstOrDefault();
//        }

//        private void AddIdentifier(entitystorematerialised_CoreEncounter encounter, Encounter enc, string hospitalNumberReference)
//        {
//            ISynapseResourceService<entitystorematerialised_CorePersonidentifier> corePersonIdentifier = this._provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersonidentifier>)) as ISynapseResourceService<entitystorematerialised_CorePersonidentifier>;

//            entitystorematerialised_CorePersonidentifier personIdentifier = corePersonIdentifier.GetQuerable().Where(x => x.PersonId == encounter.PersonId && x.Idtypecode == hospitalNumberReference).FirstOrDefault();

//            if (personIdentifier.IsNull()) return;

//            enc.Subject = new ResourceReference();
//            enc.Subject.Identifier = new Identifier();

//            enc.Subject.Reference = "Patient";
//            enc.Subject.Identifier.Value = personIdentifier.Idnumber;
//            enc.Subject.Identifier.System = hospitalNumberReference;
//        }
//    }
//}
