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


﻿using AutoMapper;
using Hl7.Fhir.Model;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using static Hl7.Fhir.Model.Procedure;


namespace Interneuron.CareRecord.HL7SynapseService.AutoMapMapperProfiles
{
    public class ProcedureMapperProfile : Profile
    {
        public ProcedureMapperProfile()
        {
            CreateMap<entitystorematerialised_CoreProcedure, Procedure>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GetStatus(src.Status)))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => GetProcedureCode(src)))
                .ForMember(dest => dest.Performed, opt => {
                    opt.PreCondition(src => src.Proceduredate.HasValue);
                    opt.MapFrom(src => GetProcedureDate(src));
                })
                .ForMember(dest => dest.Performer, opt => {
                    opt.PreCondition(src => src.Performedby.IsNotEmpty());
                    opt.MapFrom(src => GetPerformer(src));
                });


            CreateMap<entitystorematerialised_CorePersonidentifier, Procedure>()
                .ForMember(dest => dest.Subject, opt =>
                {
                    opt.MapFrom<ProcedureSubjectResolver>();
                });
        }

        private List<PerformerComponent> GetPerformer(entitystorematerialised_CoreProcedure src)
        {
            var performers = new List<PerformerComponent>();

            var performer = new PerformerComponent();
            performer.Actor = new ResourceReference();
            performer.Actor.Display = src.Performedby;

            performers.Add(performer);

            return performers;
        }

        private FhirDateTime GetProcedureDate(entitystorematerialised_CoreProcedure src)
        {
            if (src.Proceduredate.HasValue)
            {
                var elem = new FhirDateTime(src.Proceduredate.Value);
                elem.ElementId = "effectiveDateTime";
                return elem;
            }
            return null;
        }

        private object GetProcedureCode(entitystorematerialised_CoreProcedure procedure)
        {
            var procCodes = new CodeableConcept();

            Coding obsCode = new Coding();

            obsCode.Code = procedure.Code;
            obsCode.Display = procedure.Name;

            procCodes.Coding.Add(obsCode);

            return procCodes;
        }

        private EventStatus? GetStatus(string status)
        {
            if (!status.IsNotEmpty()) return null;

            return status.ToUpper() switch
            {
                "ON-HOLD" => EventStatus.Suspended,
                //"STOPPED" => EventStatus.,
                "ENTERED - IN - ERROR" => EventStatus.EnteredInError,
                "COMPLETED" => EventStatus.Completed,
                "IN - PROGRESS" => EventStatus.InProgress,
                //"NOT - DONE" => EventStatus.NotDone,
                "PREPARATION" => EventStatus.Preparation,
                _ => null,
            };
        }

        private Meta GetMeta(entitystorematerialised_CoreProcedure src)
        {
            if (src.IsNull() || !src.Createdtimestamp.HasValue) return null;

            return new Meta
            {
                VersionId = $"{src.Sequenceid}",
                LastUpdated = src.Createdtimestamp
            };
        }
    }

    public class ProcedureSubjectResolver : IValueResolver<entitystorematerialised_CorePersonidentifier, Procedure, ResourceReference>
    {
        private IServiceProvider _provider;
        private string _defaultHospitalRefNo;

        public ProcedureSubjectResolver(IServiceProvider provider)
        {
            this._provider = provider;
            this._defaultHospitalRefNo = GetDefaultHospitalRefNo();
        }

        public ResourceReference Resolve(entitystorematerialised_CorePersonidentifier personIdentifier, Procedure destination, ResourceReference destMember, ResolutionContext context)
        {
            var subject = new ResourceReference
            {
                Reference = $"Patient/{personIdentifier.Idnumber}"
            };

            return subject;
        }

        private string GetDefaultHospitalRefNo()
        {
            IConfiguration configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

            IConfigurationSection careRecordConfig = configuration.GetSection("CareRecordConfig");

            return careRecordConfig.GetValue<string>("HospitalNumberReference");
        }
    }
}
