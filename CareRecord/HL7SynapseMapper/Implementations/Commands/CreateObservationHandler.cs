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


using AutoMapper;
using Hl7.Fhir.Model;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
using Interneuron.CareRecord.HL7SynapseService.Interfaces;
using Interneuron.CareRecord.HL7SynapseService.Models;
using Interneuron.CareRecord.Infrastructure.Domain;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.Common.Extensions;
using Interneuron.Infrastructure.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.Implementations
{
    public class CreateObservationHandler : IResourceCommandHandler
    {
        private IServiceProvider _provider;
        private IMapper _mapper;

        private IRepository<entitystorematerialised_CoreObservationevent> _coreObservationEventRepo;
        private IRepository<entitystorematerialised_CoreObservation> _coreObservationRepo;
        private IRepository<entitystorematerialised_CoreObservationevent> _matCoreObservationEventRepo;
        private IRepository<entitystorematerialised_CoreObservation> _matCoreObservationRepo;

        public CreateObservationHandler(IServiceProvider provider, IMapper mapper, IRepository<entitystorematerialised_CoreObservationevent> coreObservationEventRepo, IRepository<entitystorematerialised_CoreObservation> coreObservationRepo, IRepository<entitystorematerialised_CoreObservationevent> matCoreObservationEventRepo, IRepository<entitystorematerialised_CoreObservation> matCoreObservationRepo)
        {
            this._provider = provider;
            this._mapper = mapper;
            this._coreObservationEventRepo = coreObservationEventRepo;
            this._coreObservationRepo = coreObservationRepo;
            this._matCoreObservationEventRepo = matCoreObservationEventRepo;
            this._matCoreObservationRepo = matCoreObservationRepo;
        }

        public ResourceData Handle(IFHIRParam fhirParam, Resource resource)
        {
            throw new NotImplementedException();
        }

        private static bool IsAmended(ObservationRoot observationRoot)
        {
            return observationRoot.ObservationEvent.Isamended.HasValue ? observationRoot.ObservationEvent.Isamended.Value : false;
        }

        private static void ValidateObservation(SynapseResource resource)
        {
            if (resource.IsNull() || !(resource is ObservationRoot observationRoot))
                throw new InterneuronBusinessException(400, "Invalid observation object.");

            if (observationRoot.IsNull() || observationRoot.ObservationEvent.IsNull())
                throw new InterneuronBusinessException(400, "Should have observation event object.");

            if (!observationRoot.ObservationEvent.Datestarted.HasValue)
                throw new InterneuronBusinessException(400, "Should have observation event start date.");

            if (IsAmended(observationRoot) && observationRoot.ObservationEvent.Observationevent_Id.IsEmpty())
                throw new InterneuronBusinessException(400, "Amended observation event should have observation event identifier.");
        }

        public SynapseResource Handle(SynapseResource resource)
        {
            ValidateObservation(resource);

            var observationRoot = resource as ObservationRoot;

            if (IsAmended(observationRoot))
                HandleAmendedObservation(observationRoot);
            else
                HandleObservation(observationRoot);

            var savedStatus = _coreObservationEventRepo.SaveChanges();

            //var savedObsStatus = _observationService.SaveChanges();

            if (savedStatus > 0)
            {
                var newObservationRoot = GetSavedObservation(observationRoot);

                AddNewMaterialisedObservation(newObservationRoot);

                return newObservationRoot;
            }

            return null;
        }

        private void HandleObservation(ObservationRoot observationRoot)
        {
            var observationEventDTO = observationRoot.ObservationEvent;
            var observationsDTO = observationRoot.Observations;
            var eventId = observationEventDTO.Observationevent_Id = Guid.NewGuid().ToString();
            var correlationId = observationEventDTO.Eventcorrelationid = Guid.NewGuid().ToString();

            var eventModel = this._mapper.Map<entitystorematerialised_CoreObservationevent>(observationEventDTO);

            this._coreObservationEventRepo.Add(eventModel);

            if (observationsDTO.IsCollectionValid())
            {
                var observationsModel = this._mapper.Map<List<entitystorematerialised_CoreObservation>>(observationsDTO);

                observationsModel.Each(obs =>
                {
                    obs.ObservationId = Guid.NewGuid().ToString();
                    obs.ObservationeventId = eventId;
                    obs.Eventcorrelationid = correlationId;

                    //var observtaionModel = this._mapper.Map<entitystore_CoreObservation1>(obs);

                    this._coreObservationRepo.Add(obs);
                });
            }

            RemoveMaterialisedObservation(eventId);

            //AddNewMaterialisedObservation(observationRoot);
        }

        private void HandleAmendedObservation(ObservationRoot observationRoot)
        {
            var observationEventDTO = observationRoot.ObservationEvent;
            var observationsDTO = observationRoot.Observations;
            var eventId = observationEventDTO.Observationevent_Id; //.IsNotEmpty() ?  = Guid.NewGuid().ToString();
            var correlationId = observationEventDTO.Eventcorrelationid = Guid.NewGuid().ToString();

            var eventModel = this._mapper.Map<entitystorematerialised_CoreObservationevent>(observationEventDTO);

            this._coreObservationEventRepo.Add(eventModel);

            if (observationsDTO.IsCollectionValid())
            {
                var observationsModel = this._mapper.Map<List<entitystorematerialised_CoreObservation>>(observationsDTO);

                observationsModel.Each(obs =>
                {
                    obs.ObservationId = obs.Hasbeenammended.HasValue && obs.Hasbeenammended.Value ? obs.ObservationId : Guid.NewGuid().ToString();
                    obs.ObservationeventId = eventId;
                    obs.Eventcorrelationid = correlationId;

                    //var observtaionModel = this._mapper.Map<entitystore_CoreObservation1>(obs);

                    this._coreObservationRepo.Add(obs);
                });
            }

            RemoveMaterialisedObservation(eventId);

            //AddNewMaterialisedObservation(observationRoot);
        }

        private void AddNewMaterialisedObservation(ObservationRoot observationRoot)
        {
            var observationEventDTO = observationRoot.ObservationEvent;
            var observationsDTO = observationRoot.Observations;

            var eventModel = this._mapper.Map<entitystorematerialised_CoreObservationevent>(observationEventDTO);

            this._matCoreObservationEventRepo.Add(eventModel);

            if (observationsDTO.IsCollectionValid())
            {
                var observationsModel = this._mapper.Map<List<entitystorematerialised_CoreObservation>>(observationsDTO);

                observationsModel.Each(obs =>
                {
                    this._matCoreObservationRepo.Add(obs);
                });
            }
        }

        private void RemoveMaterialisedObservation(string eventId)
        {
            var materializedEvent = this._matCoreObservationEventRepo.Items.Where((c) => c.ObservationeventId == eventId).FirstOrDefault();

            var materializedObs = this._matCoreObservationRepo.Items.Where((c) => c.ObservationeventId == eventId).ToList();

            if (materializedEvent != null)
                this._matCoreObservationEventRepo.Remove(materializedEvent);

            if (materializedObs.IsCollectionValid())
            {
                materializedObs.Each(obs =>
                {
                    this._matCoreObservationRepo.Remove(obs);
                });
            }
        }

        private ObservationRoot GetSavedObservation(ObservationRoot observationRoot)
        {

            var observationEventDTO = observationRoot.ObservationEvent;
            var observationsDTO = observationRoot.Observations;
            var eventId = observationEventDTO.Observationevent_Id;
            var correlationId = observationEventDTO.Eventcorrelationid;

            var newObservationEventInStore = _coreObservationEventRepo.ItemsAsReadOnly.Where(ev => ev.ObservationeventId == eventId && ev.Eventcorrelationid == correlationId).FirstOrDefault();

            var newObservationsInStore = _coreObservationRepo.ItemsAsReadOnly.Where(ev => ev.ObservationeventId == eventId && ev.Eventcorrelationid == correlationId).ToList();

            if (newObservationEventInStore == null || !newObservationsInStore.IsCollectionValid()) return null;

            var newObservationEvent = this._mapper.Map<ObservationEventDTO>(newObservationEventInStore);

            var newObservations = this._mapper.Map<List<ObservationDTO>>(newObservationsInStore);

            observationRoot.ObservationEvent = newObservationEvent;
            observationRoot.Observations = newObservations;

            return observationRoot;
        }
    }
}
