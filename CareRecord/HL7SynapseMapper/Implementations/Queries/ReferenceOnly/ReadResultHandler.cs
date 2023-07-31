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


//using AutoMapper;
//using Hl7.Fhir.Model;
//using Interneuron.CareRecord.HL7SynapseService.Interfaces;
//using Interneuron.CareRecord.HL7SynapseService.Models;
//using Interneuron.CareRecord.Infrastructure.Domain;
//using Interneuron.CareRecord.Model.DomainModels;
//using Interneuron.Common.Extensions;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Linq;

//namespace Interneuron.CareRecord.HL7SynapseService.Implementations
//{
//    public class ReadResultHandler : IResourceQueryHandler
//    {
//        private IServiceProvider _provider;
//        private IMapper _mapper;
//        private IReadOnlyRepository<entitystore_CoreResult> _coreResultRepo;
//        private IReadOnlyRepository<entitystorematerialised_CoreResult1> _matCoreResultRepo;
//        private IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> _matCorePersonIdentifierRepo;
//        private IReadOnlyRepository<entitystorematerialised_CoreOrder1> _matCoreOrderRepo;

//        public ReadResultHandler(IServiceProvider provider, IMapper mapper, IReadOnlyRepository<entitystore_CoreResult> coreResultRepo, IReadOnlyRepository<entitystorematerialised_CoreResult1> matCoreResultRepo, IReadOnlyRepository<entitystorematerialised_CorePersonidentifier> matCorePersonIdentifierRepo,
//            IReadOnlyRepository<entitystorematerialised_CoreOrder1> matCoreOrderRepo)
//        {
//            this._provider = provider;
//            this._mapper = mapper;
//            this._coreResultRepo = coreResultRepo;
//            this._matCoreResultRepo = matCoreResultRepo;
//            this._matCorePersonIdentifierRepo = matCorePersonIdentifierRepo;
//            this._matCoreOrderRepo = matCoreOrderRepo;
//        }
//        public ResourceData Handle(IFHIRParam fhirParam)
//        {
//            var resourceData = new ResourceData(fhirParam);

//            var materializedCoreObservationResult = GetMaterializedCoreObservationResult(fhirParam);

//            if (materializedCoreObservationResult == null || materializedCoreObservationResult.ResultId.IsEmpty())
//            {
//                var storeCoreObservationResult = CheckInEntityStore(fhirParam);

//                if (storeCoreObservationResult == null || storeCoreObservationResult.ResultId == null) return resourceData;

//                if (storeCoreObservationResult.Recordstatus == 2) // Observation Result in Deleted State
//                {
//                    resourceData.Resource = null;
//                    resourceData.DeletedDate = storeCoreObservationResult.Createddate;
//                    resourceData.IsDeleted = true;

//                    return resourceData;
//                }

//                return resourceData;
//            }

//            var observation = CreateObservation(materializedCoreObservationResult);

//            resourceData.Resource = observation;

//            return resourceData;
//        }
//        private Observation CreateObservation(entitystorematerialised_CoreResult1 result)
//        {
//            IConfiguration configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

//            IConfigurationSection careRecordConfig = configuration.GetSection("CareRecordConfig");

//            string hospitalNumberReference = careRecordConfig.GetValue<string>("HospitalNumberReference"); ;

//            var obs = this._mapper.Map<Observation>(result);// result.GetResult();

//            AddIdentifier(result, obs, hospitalNumberReference);

//            AddIssuedDateTime(result, obs);

//            //AddPerformer(result, obs);

//            //AddReferenceRange(result, obs);

//            //AddInterpretation(result, obs);

//            //AddValue(result, obs);

//            return obs;
//        }
//        private entitystore_CoreResult CheckInEntityStore(IFHIRParam fhirParam)
//        {
//            return _coreResultRepo.ItemsAsReadOnly.Where(e => e.ResultId == fhirParam.ResourceId).OrderByDescending(x => x.Sequenceid).FirstOrDefault();

//        }
//        private entitystorematerialised_CoreResult1 GetMaterializedCoreObservationResult(IFHIRParam fhirParam)
//        {
//            return fhirParam.ResourceId.IsNotEmpty() ? _matCoreResultRepo.ItemsAsReadOnly
//                .Where(r => r.ResultId == fhirParam.ResourceId)
//                .OrderByDescending(r => r.Sequenceid)
//                .FirstOrDefault() : _matCoreResultRepo.ItemsAsReadOnly.FirstOrDefault();
//        }
//        private void AddIdentifier(entitystorematerialised_CoreResult1 result, Observation obs, string hospitalNumberReference)
//        {
//            entitystorematerialised_CorePersonidentifier personIdentifier = _matCorePersonIdentifierRepo.ItemsAsReadOnly.Where(x => x.PersonId == result.PersonId && x.Idtypecode == hospitalNumberReference).FirstOrDefault();

//            if (personIdentifier.IsNull()) return;

//            this._mapper.Map(personIdentifier, obs, typeof(entitystorematerialised_CorePersonidentifier), typeof(Observation));

//            //obs.Subject = new ResourceReference();
//            //obs.Subject.Identifier = new Identifier();

//            //obs.Subject.Reference = "Patient";
//            //obs.Subject.Identifier.Value = personIdentifier.Idnumber;
//            //obs.Subject.Identifier.System = hospitalNumberReference;
//        }
//        private void AddIssuedDateTime(entitystorematerialised_CoreResult1 result, Observation obs)
//        {
//            entitystorematerialised_CoreOrder1 order = _matCoreOrderRepo.ItemsAsReadOnly.Where(x => x.PersonId == result.PersonId && x.OrderId == result.OrderId).FirstOrDefault();

//            if (order.IsNull()) return;

//            this._mapper.Map(order, obs, typeof(entitystorematerialised_CoreOrder1), typeof(Observation));
//            //obs.Issued = order.Statuschangedatetime;
//        }

//        //private void AddPerformer(entitystorematerialised_CoreResult1 result, Observation obs)
//        //{
//        //    ResourceReference performer = new ResourceReference();

//        //    performer.Reference = "Practitioner";
//        //    performer.Display = result.Author;

//        //    obs.Performer.Add(performer);
//        //}
//        //private void AddReferenceRange(entitystorematerialised_CoreResult1 result, Observation obs)
//        //{
//        //    Observation.ReferenceRangeComponent referenceRange = new Observation.ReferenceRangeComponent();

//        //    SimpleQuantity simpleQuantityLow = new SimpleQuantity();

//        //    simpleQuantityLow.Value = Convert.ToDecimal(result.Referencerangelow);

//        //    SimpleQuantity simpleQuantityHigh = new SimpleQuantity();

//        //    simpleQuantityHigh.Value = Convert.ToDecimal(result.Referencerangehigh);

//        //    referenceRange.Low = simpleQuantityLow;

//        //    referenceRange.High = simpleQuantityHigh;

//        //    obs.ReferenceRange.Add(referenceRange);
//        //}
//        //private void AddInterpretation(entitystorematerialised_CoreResult1 result, Observation obs) 
//        //{
//        //    CodeableConcept code = new CodeableConcept();

//        //    if (result.Abnormalflag.IsNotEmpty())
//        //    {
//        //        if (result.Abnormalflag.ToLower() == "true")
//        //        {
//        //            code.Text = "Abnormal";
//        //        }
//        //        else
//        //        {
//        //            code.Text = "Normal";
//        //        }
//        //    }

//        //}
        
////        private void AddValue(entitystorematerialised_CoreResult1 result, Observation obs)
////        {
////            if (int.TryParse(result.Observationvalue, out int intValue))
////            {
////                Quantity valueQuantity = new Quantity();

////        valueQuantity.Code = result.Unitscode;
////                valueQuantity.Value = Convert.ToDecimal(intValue);
////                valueQuantity.Unit = result.Unitstext;

////                obs.Value = valueQuantity;
////            }
////            else if(bool.TryParse(result.Observationvalue, out bool boolValue))
////            {
////                obs.Value = new FhirBoolean(Convert.ToBoolean(boolValue));
////}
////            else if(decimal.TryParse(result.Observationvalue, out decimal decimalValue))
////            {
////                Quantity valueQuantity = new Quantity();

////valueQuantity.Code = result.Unitscode;
////                valueQuantity.Value = Convert.ToDecimal(decimalValue);
////                valueQuantity.Unit = result.Unitstext;

////                obs.Value = valueQuantity;
////            }
////            else
////            {
////                obs.Value = new FhirString(result.Observationvalue);
////            }
////        }
//    }
//}
