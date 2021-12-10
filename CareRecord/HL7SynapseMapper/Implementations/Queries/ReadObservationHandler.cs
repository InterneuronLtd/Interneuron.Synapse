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
using System.Text;

namespace Interneuron.CareRecord.HL7SynapseService.Implementations
{
    public class ReadObservationHandler : ISynapseQueryHandler
    {
        public SynapseResourceData Handle(string personId)
        {
            var observationRoots = GetObservations(personId);

            SynapseResourceData synResourceData = new SynapseResourceData();

            synResourceData.SynapseResources = new List<SynapseResource>();

            observationRoots.Each(o => {
                synResourceData.SynapseResources.Add(o);
            });

            return synResourceData;
        }

        private IServiceProvider _provider;
        private IMapper _mapper;

        private IRepository<entitystorematerialised_CoreObservationevent> _matCoreObservationEventRepo;
        private IRepository<entitystorematerialised_CoreObservation> _matCoreObservationRepo;

        public ReadObservationHandler(IServiceProvider provider, IMapper mapper, IRepository<entitystorematerialised_CoreObservationevent> matCoreObservationEventRepo, IRepository<entitystorematerialised_CoreObservation> matCoreObservationRepo)
        {
            this._provider = provider;
            this._mapper = mapper;
            this._matCoreObservationEventRepo = matCoreObservationEventRepo;
            this._matCoreObservationRepo = matCoreObservationRepo;
        }

        private List<ObservationRoot> GetObservations(string personId)
        {
            List<ObservationRoot> observationRoots = new List<ObservationRoot>();

            var observationEvents = _matCoreObservationEventRepo.ItemsAsReadOnly.Where(ev => ev.PersonId == personId).ToList();

            if (observationEvents == null || !observationEvents.IsCollectionValid()) return null;

            foreach (entitystorematerialised_CoreObservationevent observationEvent in observationEvents)
            {
                var observations = _matCoreObservationRepo.ItemsAsReadOnly.Where(ev => ev.ObservationeventId == observationEvent.ObservationeventId).ToList();

                observationRoots.Add(new ObservationRoot { ObservationEvent = this._mapper.Map<ObservationEventDTO>(observationEvent), Observations = this._mapper.Map<List<ObservationDTO>>(observations) });
            }

            return observationRoots;
        }
    }
}
