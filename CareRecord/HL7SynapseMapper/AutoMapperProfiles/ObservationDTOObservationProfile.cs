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
using Interneuron.CareRecord.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.AutoMapperProfiles
{
    public class ObservationDTOObservationProfile : Profile
    {
        public ObservationDTOObservationProfile()
        {
            CreateMap<ObservationDTO, entitystore_CoreObservation1>()
                .ForMember(dest => dest.Createdby, opt => opt.MapFrom(s => s._Createdby))
                .ForMember(dest => dest.ObservationeventId, opt => opt.MapFrom(s => s.Observationevent_Id))
                .ForMember(dest => dest.ObservationtypeId, opt => opt.MapFrom(s => s.Observationtype_Id))
                .ForMember(dest => dest.ObservationId, opt => opt.MapFrom(s => s.Observation_Id))
                .ForMember(dest => dest.ObservationtypemeasurementId, opt => opt.MapFrom(s => s.Observationtypemeasurement_Id))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(s => s.Value.ToString()))
                .ForMember(dest => dest.RowId, opt => opt.Ignore())
                .ForMember(dest => dest.Sequenceid, opt => opt.Ignore())
                .ForMember(dest => dest.Createdtimestamp, opt => opt.Ignore())
                .ForMember(dest => dest.Createddate, opt => opt.Ignore());

            CreateMap<ObservationEventDTO, entitystore_CoreObservationevent1>()
                .ForMember(dest => dest.Createdby, opt => opt.MapFrom(s => s._Createdby))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(s => s.Person_Id))
                .ForMember(dest => dest.ObservationeventId, opt => opt.MapFrom(s => s.Observationevent_Id))
                .ForMember(dest => dest.ObservationscaletypeId, opt => opt.MapFrom(s => s.Observationscaletype_Id))
                .ForMember(dest => dest.EncounterId, opt => opt.MapFrom(s => s.Encounter_Id))
                .ForMember(dest => dest.RowId, opt => opt.Ignore())
                .ForMember(dest => dest.Sequenceid, opt => opt.Ignore())
                .ForMember(dest => dest.Createdtimestamp, opt => opt.Ignore())
                .ForMember(dest => dest.Createddate, opt => opt.Ignore());

            CreateMap<ObservationDTO, entitystorematerialised_CoreObservation>()
                .ForMember(dest => dest.Createdby, opt => opt.MapFrom(s => s._Createdby))
                .ForMember(dest => dest.ObservationeventId, opt => opt.MapFrom(s => s.Observationevent_Id))
                .ForMember(dest => dest.ObservationtypeId, opt => opt.MapFrom(s => s.Observationtype_Id))
                .ForMember(dest => dest.ObservationId, opt => opt.MapFrom(s => s.Observation_Id))
                .ForMember(dest => dest.ObservationtypemeasurementId, opt => opt.MapFrom(s => s.Observationtypemeasurement_Id))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(s => s.Value.ToString()));

            CreateMap<ObservationEventDTO, entitystorematerialised_CoreObservationevent>()
                .ForMember(dest => dest.Createdby, opt => opt.MapFrom(s => s._Createdby))
                .ForMember(dest => dest.PersonId, opt => opt.MapFrom(s => s.Person_Id))
                .ForMember(dest => dest.ObservationeventId, opt => opt.MapFrom(s => s.Observationevent_Id))
                .ForMember(dest => dest.ObservationscaletypeId, opt => opt.MapFrom(s => s.Observationscaletype_Id))
                .ForMember(dest => dest.EncounterId, opt => opt.MapFrom(s => s.Encounter_Id));


            CreateMap<entitystore_CoreObservation1, ObservationDTO>();
                
            CreateMap<entitystore_CoreObservationevent1, ObservationEventDTO>();

            CreateMap<entitystorematerialised_CoreObservationevent, ObservationEventDTO>();
            CreateMap<entitystorematerialised_CoreObservation, ObservationDTO>();
        }
    }

    //public static class Mapping
    //{
    //    private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
    //    {
    //        var config = new MapperConfiguration(cfg => {
    //            cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
    //            cfg.AddProfile<ObservationDTOObservationProfile>();
    //        });
    //        var mapper = config.CreateMapper();
    //        return mapper;
    //    });
    //    private static IServiceProvider _provider;

    //    public static IMapper Mapper => Lazy.Value;
    //}

    //var destination = Mapping.Mapper.Map<Destination>(yourSourceInstance);



}
