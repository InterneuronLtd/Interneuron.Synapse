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
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using static Hl7.Fhir.Model.Encounter;

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.AutoMapperProfiles
{
    public class EncounterMapperProfile : Profile
    {
        public EncounterMapperProfile()
        {
            CreateMap<entitystorematerialised_CoreEncounter, Encounter>()
                .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => GetMeta(src)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.EncounterId))
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => GetEncounterIdentifiers(src)))
                .ForMember(dest => dest.Period, opt => opt.MapFrom(src => GetEncounterPeriod(src)))
                .ForMember(dest => dest.ReasonCode, opt => opt.MapFrom(src => GetReasonCodes(src)))
                .ForMember(dest => dest.Hospitalization, opt => opt.MapFrom(src => GetHospitalizationDetails(src)))
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => GetClass(src)))
                .ForMember(dest => dest.Status, opt => {
                    opt.PreCondition(src => !src.Episodestatuscode.IsEmpty());
                    opt.MapFrom(src => GetStatus(src)); 
                });

            CreateMap<entitystorematerialised_CorePersonidentifier, Encounter>()
                // .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => GetSubject(src)));
                .ForMember(dest => dest.Subject, opt => opt.MapFrom<EncounterSubjectResolver>());
        }

        private Meta GetMeta(entitystorematerialised_CoreEncounter src)
        {
            if (src.IsNull() || !src.Createdtimestamp.HasValue) return null;

            return new Meta
            {
                VersionId = $"{src.Sequenceid}",
                LastUpdated = src.Createdtimestamp
            };
        }

        //private ResourceReference GetSubject(entitystorematerialised_CorePersonidentifier personIdentifier)
        //{
        //    var subject = new ResourceReference
        //    {
        //        Identifier = new Identifier(),
        //        Reference = "Patient"
        //    };

        //    if (personIdentifier.IsNull()) return subject;

        //    subject.Identifier.Value = personIdentifier.Idnumber;
        //    subject.Identifier.System = personIdentifier.Idtypecode ?? "";// _defaultHospitalRefNo;

        //    return subject;
        //}

        private static EncounterStatus? GetStatus(entitystorematerialised_CoreEncounter coreEncounter)
        {
            if (coreEncounter.Episodestatuscode.IsEmpty()) return default(EncounterStatus?);

            switch (coreEncounter.Episodestatuscode.ToLower())
            {
                case "active":
                    return Encounter.EncounterStatus.InProgress;
                case "cancelled":
                    return Encounter.EncounterStatus.Cancelled;
            }

            return default(EncounterStatus?);
        }

        private Coding GetClass(entitystorematerialised_CoreEncounter coreEncounter)
        {
            Coding encounterClass = new Coding();

            if (coreEncounter.Patientclasscode.IsNull()) return encounterClass;

            encounterClass.Code = coreEncounter.Patientclasscode;

            switch (coreEncounter.Patientclasscode.ToLower())
            {
                case "ip":
                    encounterClass.Display = "Inpatient Encounter";
                    break;
                case "op":
                    encounterClass.Display = "Outpatient Encounter";
                    break;
            }

            return encounterClass;
        }

        private List<Identifier> GetEncounterIdentifiers(entitystorematerialised_CoreEncounter coreEncounter)
        {
            var identifiers = new List<Identifier>();

            if (coreEncounter.IsNull()) return identifiers;

            Identifier id = new Identifier
            {
                Value = coreEncounter.Visitnumber
            };

            identifiers.Add(id);

            return identifiers;
        }

        public Period GetEncounterPeriod(entitystorematerialised_CoreEncounter coreEncounter)
        {
            var period = new Period();

            if (coreEncounter.IsNull()) return period;

            period.Start = Convert.ToString(coreEncounter.Admitdatetime);
            period.End = Convert.ToString(coreEncounter.Dischargedatetime);

            return period;
        }

        public List<CodeableConcept> GetReasonCodes(entitystorematerialised_CoreEncounter coreEncounter)
        {
            var reasonCodes = new List<CodeableConcept>();

            if (coreEncounter.IsNull()) return reasonCodes;

            CodeableConcept reasonCode = new CodeableConcept();

            reasonCode.Text = coreEncounter.Admitreasontext;

            Coding coding = new Coding
            {
                Code = coreEncounter.Admitreasoncode
            };

            reasonCode.Coding.Add(coding);

            return reasonCodes;
        }

        public HospitalizationComponent GetHospitalizationDetails(entitystorematerialised_CoreEncounter coreEncounter)
        {
            var hospitalization = new HospitalizationComponent();

            if (coreEncounter.IsNull()) return hospitalization;

            hospitalization.AdmitSource = new CodeableConcept();

            hospitalization.DischargeDisposition = new CodeableConcept();

            hospitalization.AdmitSource.Text = coreEncounter.Referringdoctortext;

            Coding admitSourceCode = new Coding
            {
                Code = coreEncounter.Referringdoctorid
            };

            hospitalization.AdmitSource.Coding.Add(admitSourceCode);

            hospitalization.DischargeDisposition.Text = coreEncounter.Dischargetext;

            Coding dischargeCode = new Coding();

            dischargeCode.Code = coreEncounter.Dischargecode;

            hospitalization.DischargeDisposition.Coding.Add(dischargeCode);

            return hospitalization;
        }
    }

    public class EncounterSubjectResolver : IValueResolver<entitystorematerialised_CorePersonidentifier, Encounter, ResourceReference>
    {
        private IServiceProvider _provider;
        private string _defaultHospitalRefNo;

        public EncounterSubjectResolver(IServiceProvider provider)
        {
            this._provider = provider;
            this._defaultHospitalRefNo = GetDefaultHospitalRefNo();
        }
        public ResourceReference Resolve(entitystorematerialised_CorePersonidentifier personIdentifier, Encounter destination, ResourceReference destMember, ResolutionContext context)
        {
            var subject = new ResourceReference
            {
                Identifier = new Identifier(),
                Reference = "Patient"
            };

            if (personIdentifier.IsNull()) return subject;

            subject.Identifier.Value = personIdentifier.Idnumber;
            subject.Identifier.System = personIdentifier.Idtypecode ?? _defaultHospitalRefNo;

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
