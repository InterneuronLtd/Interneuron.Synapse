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


//using Hl7.Fhir.Model;
//using Interneuron.CareRecord.Model.DomainModels;
//using Interneuron.Common.Extensions;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Interneuron.CareRecord.HL7SynapseService.Implementations.ModelExtensions
//{
//    public static class ResultExtension
//    {
//        public static Observation GetResult(this entitystorematerialised_CoreResult1 coreObservationResult)
//        {
//            if (coreObservationResult.IsNull()) return null;

//            var observation = new Observation
//            {
//                Id = coreObservationResult.ResultId,
//                Identifier = GetResultIdentifiers(coreObservationResult),
//                Code = GetResultCode(coreObservationResult)
//            };

//            if (coreObservationResult.Observationdatetime.HasValue)
//            {
//                observation.Effective.ElementId = "effectiveDateTime";
//                observation.Effective = new FhirDateTime(coreObservationResult.Observationdatetime.Value);

//            }

//            if (coreObservationResult.Observationresultstatus.IsEmpty()) return observation;

//            switch (coreObservationResult.Observationresultstatus.ToLower())
//            {
//                case "amended":
//                    observation.Status = ObservationStatus.Amended;
//                    break;
//                case "cancelled":
//                    observation.Status = ObservationStatus.Cancelled;
//                    break;
//                case "corrected":
//                    observation.Status = ObservationStatus.Corrected;
//                    break;
//                case "enteredinerror":
//                    observation.Status = ObservationStatus.EnteredInError;
//                    break;
//                case "final":
//                    observation.Status = ObservationStatus.Final;
//                    break;
//                case "preliminary":
//                    observation.Status = ObservationStatus.Preliminary;
//                    break;
//                case "registered":
//                    observation.Status = ObservationStatus.Registered;
//                    break;
//                default:
//                    observation.Status = ObservationStatus.Unknown;
//                    break;
//            }

//            return observation;
//        }

//        public static CodeableConcept GetResultCode(this entitystorematerialised_CoreResult1 coreObservationResult)
//        {
//            var obsCodes = new CodeableConcept();

//            Coding obsCode = new Coding();

//            obsCode.Code = coreObservationResult.Observationidentifiercode;
//            obsCode.Display = coreObservationResult.Observationidentifiertext;

//            obsCodes.Coding.Add(obsCode);

//            return obsCodes;
//        }

//        public static List<Identifier> GetResultIdentifiers(this entitystorematerialised_CoreResult1 coreObservationResult)
//        {
//            List<Identifier> identifiers = new List<Identifier>();

//            Identifier id = new Identifier();

//            id.Value = coreObservationResult.Contextkey;

//            identifiers.Add(id);

//            return identifiers;
//        }

//    }
//}
