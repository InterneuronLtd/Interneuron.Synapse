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
using Hl7.Fhir.Model;
using Interneuron.CareRecord.HL7SynapseService.Implementations;
using System;

namespace Interneuron.CareRecord.HL7SynapseService.Interfaces
{
    public class ResourceQueryHandlerFactory
    {
        private IServiceProvider _provider;

        public ResourceQueryHandlerFactory(IServiceProvider provider)
        {
            this._provider = provider;
        }
        public IResourceQueryHandler GetHandler(string fhirResourceType)
        {
            if (fhirResourceType == "Patient")
            {
                var readHandler = this._provider.GetService(typeof(ReadPatientHandler)) as ReadPatientHandler;
                return readHandler; // new ReadPatientHandler(this._provider);
            }

            if (fhirResourceType == "Encounter")
            {
                var readHandler = this._provider.GetService(typeof(ReadEncounterHandler)) as ReadEncounterHandler;
                return readHandler; // new ReadEncounterHandler(this._provider);
            }

            if (fhirResourceType == "Observation")
            {
                var readHandler = this._provider.GetService(typeof(ReadObservationHandler)) as ReadObservationHandler;
                return readHandler;
            }

            if (fhirResourceType == "Result")
            {
                var readHandler = this._provider.GetService(typeof(ReadResultHandler)) as ReadResultHandler;
                return readHandler;
            }

            if (fhirResourceType == "Report")
            {
                var readHandler = this._provider.GetService(typeof(ReadReportHandler)) as ReadReportHandler;
                return readHandler;
            }

            if (fhirResourceType == "Procedure")
            {
                var readHandler = this._provider.GetService(typeof(ReadProcedureHandler)) as ReadProcedureHandler;
                return readHandler;
            }
            return null;
        }
    }
}
