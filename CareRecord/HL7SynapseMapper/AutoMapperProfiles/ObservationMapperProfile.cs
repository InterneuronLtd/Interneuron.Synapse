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


﻿using AutoMapper;
using Hl7.Fhir.Model;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.AutoMapperProfiles
{
    public class ObservationMapperProfile : Profile
    {
        public ObservationMapperProfile()
        {

            CreateMap<entitystorematerialised_CoreObservation, Observation>()
                //.ForMember(dest => dest.Meta, opt => opt.MapFrom(src => GetMeta(src)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ObservationId))
                .ForMember(dest => dest.Value, opt =>
                {
                    opt.PreCondition(src => !src.Value.IsNull());
                    opt.MapFrom(src => GetValue(src));
                })
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => GetObservationCode(src)))
                .ForMember(dest => dest.Effective, opt =>
                {
                    opt.PreCondition(src => src.Timerecorded.HasValue);
                    opt.MapFrom(src => GetEffective(src));
                });

            CreateMap<entitystorematerialised_CorePersonidentifier, Observation>()
                .ForMember(dest => dest.Subject, opt => opt.MapFrom<ObservationResultSubjectResolver>());
        }

        private object GetEffective(entitystorematerialised_CoreObservation src)
        {
            if (src.Timerecorded.HasValue)
            {
                var timeRecorded = new DateTime(src.Timerecorded.Value.Year,
                                                src.Timerecorded.Value.Month,
                                                src.Timerecorded.Value.Day,
                                                src.Timerecorded.Value.Hour,
                                                src.Timerecorded.Value.Minute,
                                                src.Timerecorded.Value.Second,
                                                src.Timerecorded.Value.Kind);
                var elem = new FhirDateTime(timeRecorded);
                elem.ElementId = "effectiveDateTime";
                return elem;
            }
            return null;
        }

        private object GetObservationCode(entitystorematerialised_CoreObservation src)
        {
            var obsCodes = new CodeableConcept();

            Coding obsCode = new Coding();

            switch (src.ObservationtypeId)
            {
                case "e2d6d5fa-2a38-448f-9e06-9a3e6b532bb5":
                    obsCode.Code = "271650006";
                    obsCode.Display = "Diastolic blood pressure";
                    break;
                case "83a4b253-5599-43d2-a377-9f8001e7dbac":
                    obsCode.Code = "50373000";
                    obsCode.Display = "Body height measure";
                    break;
                case "f05f980e-e29c-48ed-9cb7-51d47221e6a6": 
                    obsCode.Code = "364075005";
                    obsCode.Display = "Heart rate";
                    break;
                case "7e554e57-63fe-4924-a49b-646a6c6ef5ce":
                    obsCode.Code = "86290005";
                    obsCode.Display = "Respiratory rate";
                    break;
                case "af5dc4a2-3e40-4e09-9d4f-3ddb137158de":
                    obsCode.Code = "271649006";
                    obsCode.Display = "Systolic blood pressure";
                    break;
                case "d76e8bd1-158e-45e3-b4b0-d526e7268005":
                    obsCode.Code = "276885007";
                    obsCode.Display = "Core body temperature";
                    break;
                case "71d6a339-7d9e-4054-99d6-683da95331dc":
                    obsCode.Code = "27113001";
                    obsCode.Display = "Body weight";
                    break;
            }

            obsCodes.Coding.Add(obsCode);

            return obsCodes;
        }

        private object GetValue(entitystorematerialised_CoreObservation observation)
        {
            if (int.TryParse(observation.Value, out int intValue))
            {
                Quantity valueQuantity = new Quantity();

                valueQuantity.Code = observation.Units;
                valueQuantity.Value = Convert.ToDecimal(intValue);
                valueQuantity.Unit = observation.Units;

                return valueQuantity;
            }
            else if (bool.TryParse(observation.Value, out bool boolValue))
            {
                return new FhirBoolean(Convert.ToBoolean(boolValue));
            }
            else if (decimal.TryParse(observation.Value, out decimal decimalValue))
            {
                Quantity valueQuantity = new Quantity();

                valueQuantity.Code = observation.Units;
                valueQuantity.Value = Convert.ToDecimal(decimalValue);
                valueQuantity.Unit = observation.Units;

                return valueQuantity;
            }
            else
            {
                return new FhirString(observation.Value);
            }
        }

        private object GetMeta(entitystorematerialised_CoreObservation src)
        {
            if (src.IsNull() || !src.Createdtimestamp.HasValue) return null;

            return new Meta
            {
                VersionId = $"{src.Sequenceid}",
                LastUpdated = src.Createdtimestamp
            };
        }
    }
}
