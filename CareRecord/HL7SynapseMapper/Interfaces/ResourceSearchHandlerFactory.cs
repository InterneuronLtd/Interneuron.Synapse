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


using Interneuron.CareRecord.HL7SynapseHandler.Service.Implementations.Queries;
using System;

namespace Interneuron.CareRecord.HL7SynapseService.Interfaces
{
    public class ResourceSearchHandlerFactory
    {
        private IServiceProvider _provider;

        public ResourceSearchHandlerFactory(IServiceProvider provider)
        {
            this._provider = provider;
        }
        public IResourceSearchHandler GetHandler(Type fhirResourceType)
        {
            #region Just for reference
            //if (fhirResourceType == typeof(Patient))
            //{
            //    var searchHandler = this._provider.GetService(typeof(SearchPatientHandler)) as SearchPatientHandler;
            //    return searchHandler; 
            //}

            //if (fhirResourceType == typeof(Encounter))
            //{
            //    var searchHandler = this._provider.GetService(typeof(SearchEncounterHandler)) as SearchEncounterHandler;
            //    return searchHandler;
            //}


            //if (fhirResourceType == typeof(Observation))
            //{
            //    var searchHandler = this._provider.GetService(typeof(SearchResultHandler)) as SearchResultHandler;
            //    return searchHandler;
            //}
            //}
            //return null;
            #endregion Just for reference

            var searchHandler = this._provider.GetService(typeof(GenericSearchHandler)) as GenericSearchHandler;
            return searchHandler;

        }
    }
}
