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
                //.ForMember(dest => dest.Meta, opt => opt.MapFrom(src => GetMeta(src)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Visitnumber))
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom(src => GetEncounterIdentifiers(src)))
                .ForMember(dest => dest.Period, opt => opt.MapFrom(src => GetEncounterPeriod(src)))
                .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => GetReasonCodes(src)))
                .ForMember(dest => dest.Hospitalization, opt => opt.MapFrom(src => GetHospitalizationDetails(src)))
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => GetClass(src)))
                .ForMember(dest => dest.Status, opt => {
                    opt.PreCondition(src => !src.Episodestatuscode.IsEmpty());
                    opt.MapFrom(src => GetStatus(src)); 
                })
                .ForMember(dest => dest.Location, opt => {
                    opt.PreCondition(src => src.Assignedpatientlocationpointofcare.IsNotEmpty());
                    opt.MapFrom(src => GetPatientLocation(src));
                })
                .ForMember(dest => dest.Participant, opt => opt.MapFrom(src => GetParticipants(src)))
                .ForMember(dest => dest.Extension, opt => opt.MapFrom(src => GetEncounterExtensions(src)));

            CreateMap<entitystorematerialised_CorePersonidentifier, Encounter>()
                // .ForMember(dest => dest.Subject, opt => opt.MapFrom(src => GetSubject(src)));
                .ForMember(dest => dest.Subject, opt => opt.MapFrom<EncounterSubjectResolver>());
        }

        private List<Extension> GetEncounterExtensions(entitystorematerialised_CoreEncounter src)
        {
            var extensions = new List<Extension>();

            if (src.Hospitalservicecode.IsNotEmpty())
            {
                var extension = new Extension();
                extension.Url = "Specialty";
                extension.Value = new FhirString(src.Hospitalservicetext);
                extension.ElementId = src.Hospitalservicecode;

                extensions.Add(extension);
            }

            return extensions;
        }

        private List<ParticipantComponent> GetParticipants(entitystorematerialised_CoreEncounter src)
        {
            var participants = new List<ParticipantComponent>();

            if (src.Consultingdoctortext.IsNotEmpty())
            {
                var participant = new ParticipantComponent();
                participant.Type = new List<CodeableConcept>()
                {
                    new CodeableConcept
                    { 
                        Coding = new List<Coding>() 
                        {
                            new Coding
                            {
                                System = "http://hl7.org/fhir/v3/ParticipationType",
                                Code = "CON",
                                Display = "consultant"
                            }
                        }                    
                    }
                };
                participant.Individual = new ResourceReference
                {                    
                    Identifier = new Identifier 
                    {
                        Value = src.Consultingdoctorid
                    },
                    Display = src.Consultingdoctortext
                };
                participants.Add(participant);
            }

            if (src.Referringdoctortext.IsNotEmpty())
            {
                var participant = new ParticipantComponent();
                participant.Type = new List<CodeableConcept>()
                {
                    new CodeableConcept
                    {
                        Coding = new List<Coding>()
                        {
                            new Coding
                            {
                                System = "http://hl7.org/fhir/v3/ParticipationType",
                                Code = "REF",
                                Display = "referrer"
                            }
                        }
                    }
                };
                participant.Individual = new ResourceReference
                {
                    Identifier = new Identifier
                    {
                        Value = src.Referringdoctorid
                    },
                    Display = src.Referringdoctortext
                };
                participants.Add(participant);
            }

            if (src.Admittingdoctortext.IsNotEmpty())
            {
                var participant = new ParticipantComponent();
                participant.Type = new List<CodeableConcept>()
                {
                    new CodeableConcept
                    {
                        Coding = new List<Coding>()
                        {
                            new Coding
                            {
                                System = "http://hl7.org/fhir/v3/ParticipationType",
                                Code = "ADM",
                                Display = "admitter"
                            }
                        }
                    }
                };
                participant.Individual = new ResourceReference
                {
                    Identifier = new Identifier
                    {
                        Value = src.Admittingdoctorcode
                    },
                    Display = src.Admittingdoctortext
                };
                participants.Add(participant);
            }

            return participants;
        }

        private List<LocationComponent> GetPatientLocation(entitystorematerialised_CoreEncounter src)
        {
            var locations = new List<LocationComponent>();

            if (src.Assignedpatientlocationpointofcare.IsNotEmpty())
            {
                var location = new LocationComponent();
                location.Location = new ResourceReference();
                location.Location.Identifier = new Identifier { Value = src.Assignedpatientlocationpointofcare };
                location.Location.Display = src.Assignedpatientlocationpointofcare;
                location.Extension = new List<Extension>()
                {
                    new Extension
                    {
                        Value = new Identifier
                        {
                            Value = "Ward",                            
                        },
                        ElementId = "wa"
                    }
                };
                locations.Add(location);
            }

            if (src.Assignedpatientlocationbay.IsNotEmpty())
            {
                var location = new LocationComponent();
                location.Location = new ResourceReference();
                location.Location.Identifier = new Identifier { Value = src.Assignedpatientlocationbay };
                location.Location.Display = src.Assignedpatientlocationbay;
                location.Extension = new List<Extension>()
                {
                    new Extension
                    {
                        Value = new Identifier
                        {
                            Value = "Bay",
                        },
                        ElementId = "bay"
                    }
                };
                locations.Add(location);
            }

            if (src.Assignedpatientlocationbed.IsNotEmpty())
            {
                var location = new LocationComponent();
                location.Location = new ResourceReference();
                location.Location.Identifier = new Identifier { Value = src.Assignedpatientlocationbed };
                location.Location.Display = src.Assignedpatientlocationbed;
                location.Extension = new List<Extension>()
                {
                    new Extension
                    {
                        Value = new Identifier
                        {
                            Value = "Bed",
                        },
                        ElementId = "bd"
                    }
                };
                locations.Add(location);
            }

            return locations;
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

            period.Start = Convert.ToString(new FhirDateTime(coreEncounter.Admitdatetime.Value));
            if (coreEncounter.Dischargedatetime != null)
                period.End = Convert.ToString(new FhirDateTime(coreEncounter.Dischargedatetime.Value));

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
                System = "LocalCode",
                Code = coreEncounter.Admitreasoncode,
                Display = coreEncounter.Admitreasontext
            };

            reasonCode.Coding.Add(coding);

            reasonCodes.Add(reasonCode);

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
