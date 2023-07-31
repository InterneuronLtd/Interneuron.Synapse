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
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using static Hl7.Fhir.Model.DiagnosticReport;

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.AutoMapperProfiles
{
    public class OrderMapperProfile : Profile
    {
        public OrderMapperProfile() 
        {
            CreateMap<entitystorematerialised_CoreOrder, DiagnosticReport>()
                //.ForMember(dest => dest.Meta, opt => opt.MapFrom(src => GetMeta(src)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.Status, opt =>
                {
                    opt.PreCondition(src => !src.Resultstatus.IsEmpty());
                    opt.MapFrom(src => GetStatus(src));
                })
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => GetOrderCode(src)))
                .ForMember(dest => dest.Effective, opt =>
                {
                    opt.PreCondition(src => src.Datetimeoftransaction.HasValue);
                    opt.MapFrom(src => GetEffective(src));
                })
                .ForMember(dest => dest.Category, opt => 
                {
                    opt.PreCondition(src => src.Diagnosticserviceid.IsNotEmpty());
                    opt.MapFrom(src => GetDiagnosticReportCategory(src.Diagnosticserviceid));
                });
                

            CreateMap<entitystorematerialised_CoreReport, DiagnosticReport>()
                .ForMember(dest => dest.PresentedForm, opt => opt.MapFrom<ReportPresentedFormResolver>());


            CreateMap<entitystorematerialised_CoreResult, DiagnosticReport>()
                .ForMember(dest => dest.Contained, opt => opt.MapFrom<ReportContainedResolver>());

            CreateMap<entitystorematerialised_CorePersonidentifier, DiagnosticReport>()
                .ForMember(dest => dest.Subject, opt => opt.MapFrom<ReportResultSubjectResolver>());

            CreateMap<entitystorematerialised_CoreNote, DiagnosticReport>()
                .ForMember(dest => dest.Contained, opt => opt.MapFrom<ReportContainedNoteResolver>());
        }

        private CodeableConcept GetDiagnosticReportCategory(string diagnosticServiceId)
        {
            CodeableConcept category = new CodeableConcept();
            category.Coding = new List<Coding> { 
                new Coding { 
                    System = "http://hl7.org/fhir/v2/0074"
                }
            };

            switch (diagnosticServiceId.ToLower().Trim())
            {
                case "bio":
                    category.Coding[0].Code = "LAB";
                    category.Coding[0].Display = "Laboratory";
                    category.Text = "Laboratory";
                    break;
                case "blbnk":
                    category.Coding[0].Code = "BLB";
                    category.Coding[0].Display = "Blood Bank";
                    category.Text = "Blood Bank";
                    break;
                case "c":
                    category.Coding[0].Code = "RAD";
                    category.Coding[0].Display = "Radiology";
                    category.Text = "Radiology";
                    break;
                case "d":
                    category.Coding[0].Code = "RAD";
                    category.Coding[0].Display = "Radiology";
                    category.Text = "Radiology";
                    break;
                case "e":
                    category.Coding[0].Code = "RAD";
                    category.Coding[0].Display = "Radiology";
                    category.Text = "Radiology";
                    break;
                case "f":
                    category.Coding[0].Code = "RAD";
                    category.Coding[0].Display = "Radiology";
                    category.Text = "Radiology";
                    break;
                case "hae":
                    category.Coding[0].Code = "HM";
                    category.Coding[0].Display = "Hematology";
                    category.Text = "Hematology";
                    break;
                case "haem":
                    category.Coding[0].Code = "HM";
                    category.Coding[0].Display = "Hematology";
                    category.Text = "Hematology";
                    break;
                case "histopathology":
                    category.Coding[0].Code = "PAT";
                    category.Coding[0].Display = "Pathology";
                    category.Text = "Pathology";
                    break;
                case "i":
                    category.Coding[0].Code = "RAD";
                    category.Coding[0].Display = "Radiology";
                    category.Text = "Radiology";
                    break;
                case "imu":
                    category.Coding[0].Code = "IMM";
                    category.Coding[0].Display = "Immunology";
                    category.Text = "Immunology";
                    break;
                case "m":
                    category.Coding[0].Code = "RAD";
                    category.Coding[0].Display = "Radiology";
                    category.Text = "Radiology";
                    break;
                case "mb":
                    category.Coding[0].Code = "MB";
                    category.Coding[0].Display = "Microbiology";
                    category.Text = "Microbiology";
                    break;
                case "n":
                    category.Coding[0].Code = "RAD";
                    category.Coding[0].Display = "Radiology";
                    category.Text = "Radiology";
                    break;
                case "poc":
                    category.Coding[0].Code = "HM";
                    category.Coding[0].Display = "Hematology";
                    category.Text = "Hematology";
                    break;
                case "r":
                    category.Coding[0].Code = "RAD";
                    category.Coding[0].Display = "Radiology";
                    category.Text = "Radiology";
                    break;
                case "tr":
                    category.Coding[0].Code = "LocalCode";
                    category.Coding[0].Display = diagnosticServiceId;
                    category.Text = diagnosticServiceId;
                    break;
                case "u":
                    category.Coding[0].Code = "RAD";
                    category.Coding[0].Display = "Radiology";
                    category.Text = "Radiology";
                    break;
                case "vir":
                    category.Coding[0].Code = "VR";
                    category.Coding[0].Display = "Virology";
                    category.Text = "Virology";
                    break;
                case "z":
                    category.Coding[0].Code = "RAD";
                    category.Coding[0].Display = "Radiology";
                    category.Text = "Radiology";
                    break;
                default:
                    category.Coding[0].System = "LocalCode";
                    category.Coding[0].Code = diagnosticServiceId;
                    category.Coding[0].Display = diagnosticServiceId;
                    category.Text = diagnosticServiceId;
                    break;
            }

            return category;
        }

        private object GetOrderCode(entitystorematerialised_CoreOrder coreOrder)
        {

            var obsCodes = new CodeableConcept();

            Coding obsCode = new Coding();

            obsCode.Code = coreOrder.Universalservicecode;
            obsCode.Display = coreOrder.Universalservicetext;

            obsCodes.Coding.Add(obsCode);

            return obsCodes;
        }

        private DiagnosticReportStatus? GetStatus(entitystorematerialised_CoreOrder coreObservationResult)
        {
            if (coreObservationResult.Resultstatus.IsEmpty()) return default(DiagnosticReportStatus);

            switch (coreObservationResult.Resultstatus.ToLower())
            {
                case "f":
                    return DiagnosticReportStatus.Final;

                case "cancelled":
                    return DiagnosticReportStatus.Cancelled;

                case "corrected":
                    return DiagnosticReportStatus.Corrected;

                case "enteredinerror":
                    return DiagnosticReportStatus.EnteredInError;

                case "final":
                    return DiagnosticReportStatus.Final;

                case "preliminary":
                    return DiagnosticReportStatus.Preliminary;

                case "registered":
                    return DiagnosticReportStatus.Registered;

                default:
                    return DiagnosticReportStatus.Unknown;
            }
        }

        private FhirDateTime GetEffective(entitystorematerialised_CoreOrder coreOrder)
        {
            if (coreOrder.Datetimeoftransaction.HasValue)
            {
                var elem = new FhirDateTime(coreOrder.Datetimeoftransaction.Value);
                elem.ElementId = "effectiveDateTime";
                return elem;
            }
            return null;
        }

        private object GetMeta(entitystorematerialised_CoreOrder src)
        {
            if (src.IsNull() || !src.Createdtimestamp.HasValue) return null;

            return new Meta
            {
                VersionId = $"{src.Sequenceid}",
                LastUpdated = src.Createdtimestamp
            };
        }
    }

    internal class ReportContainedNoteResolver : IValueResolver<entitystorematerialised_CoreNote, DiagnosticReport, List<Resource>>
    {
        public List<Resource> Resolve(entitystorematerialised_CoreNote coreNote, DiagnosticReport destination, List<Resource> destMember, ResolutionContext context)
        {
            var observations = new List<Resource>();

            if (destination.Contained.IsCollectionValid())
            {
                destination.Contained.Each(obs => {
                    if (obs.Id == coreNote.Parentid)
                    {
                        (obs as Observation).Comment = coreNote.Comment;
                    }
                    observations.Add(obs);
                });
            }

           return observations;
        }
    }

    internal class ReportContainedResolver : IValueResolver<entitystorematerialised_CoreResult, DiagnosticReport, List<Resource>>
    {
        public List<Resource> Resolve(entitystorematerialised_CoreResult coreResult, DiagnosticReport destination, List<Resource> destMember, ResolutionContext context)
        {
            var observations = new List<Resource>();

            if (destination.Contained.IsCollectionValid())
            {
                destination.Contained.Each(obs => observations.Add(obs));
            }

            if (coreResult.IsNull()) return observations;

            var observation = GetReportContained(coreResult, destination.Id);

            if (observation != null)
                observations.Add(observation);

            return observations;
        }

        private Observation GetReportContained(entitystorematerialised_CoreResult coreResult, string orderId)
        {
            Observation observation = null;

            if (coreResult.OrderId == orderId)
            {
                observation = new Observation();
                observation.Id = coreResult.ResultId;
                observation.Identifier = GetResultIdentifiers(coreResult);
                observation.Code = GetResultCode(coreResult);
                observation.Effective = GetEffective(coreResult);
                observation.Status = GetStatus(coreResult);
                observation.Performer = GetPerformer(coreResult);
                observation.ReferenceRange = GetReferenceRange(coreResult);
                observation.Interpretation = GetInterpretation(coreResult);
                observation.Value = GetValue(coreResult);
            }

            return observation;
        }

        private DataType GetValue(entitystorematerialised_CoreResult coreResult)
        {
            if (int.TryParse(coreResult.Observationvalue, out int intValue))
            {
                Quantity valueQuantity = new Quantity();

                valueQuantity.Code = coreResult.Unitscode;
                valueQuantity.Value = Convert.ToDecimal(intValue);
                valueQuantity.Unit = coreResult.Unitstext;

                return valueQuantity;
            }
            else if (bool.TryParse(coreResult.Observationvalue, out bool boolValue))
            {
                return new FhirBoolean(Convert.ToBoolean(boolValue));
            }
            else if (decimal.TryParse(coreResult.Observationvalue, out decimal decimalValue))
            {
                Quantity valueQuantity = new Quantity();

                valueQuantity.Code = coreResult.Unitscode;
                valueQuantity.Value = Convert.ToDecimal(decimalValue);
                valueQuantity.Unit = coreResult.Unitstext;

                return valueQuantity;
            }
            else
            {
                return new FhirString(coreResult.Observationvalue);
            }
        }

        private CodeableConcept GetInterpretation(entitystorematerialised_CoreResult coreResult)
        {
            CodeableConcept code = new CodeableConcept();

            if (coreResult.Abnormalflag.IsEmpty()) return code;

            if (coreResult.Abnormalflag.ToLower() == "true")
            {
                code.Text = "Abnormal";
            }
            else
            {
                code.Text = "Normal";
            }

            return code;
        }

        private List<Observation.ReferenceRangeComponent> GetReferenceRange(entitystorematerialised_CoreResult coreResult)
        {
            var referenceRanges = new List<Observation.ReferenceRangeComponent>();

            Observation.ReferenceRangeComponent referenceRange = new Observation.ReferenceRangeComponent();

            if (coreResult.Referencerangelow.IsNotEmpty())
            {
                referenceRange.Low = new Quantity
                {
                    Value = Decimal.TryParse(coreResult.Referencerangelow, out Decimal refLow) ? refLow : default(decimal?)
                };
            }

            if (coreResult.Referencerangehigh.IsNotEmpty())
            {
                referenceRange.High = new Quantity
                {
                    Value = Decimal.TryParse(coreResult.Referencerangehigh, out Decimal refHigh) ? refHigh : default(decimal?)
                };
            }

            referenceRanges.Add(referenceRange);
            return referenceRanges;
        }

        private List<ResourceReference> GetPerformer(entitystorematerialised_CoreResult coreResult)
        {
            var referenceResources = new List<ResourceReference>();

            if (coreResult.Author.IsNotEmpty())
            {
                ResourceReference performer = new ResourceReference
                {
                    Reference = "Practitioner",
                    Display = coreResult.Author
                };

                referenceResources.Add(performer);
            }
            return referenceResources;
        }

        private ObservationStatus? GetStatus(entitystorematerialised_CoreResult coreResult)
        {
            if (coreResult.Observationresultstatus.IsEmpty()) return default(ObservationStatus);

            switch (coreResult.Observationresultstatus.ToLower())
            {
                case "amended":
                    return ObservationStatus.Amended;

                case "cancelled":
                    return ObservationStatus.Cancelled;

                case "corrected":
                    return ObservationStatus.Corrected;

                case "enteredinerror":
                    return ObservationStatus.EnteredInError;

                case "final":
                    return ObservationStatus.Final;

                case "preliminary":
                    return ObservationStatus.Preliminary;

                case "registered":
                    return ObservationStatus.Registered;

                default:
                    return ObservationStatus.Unknown;
            }
        }

        private DataType GetEffective(entitystorematerialised_CoreResult coreResult)
        {
            if (coreResult.Observationdatetime.HasValue)
            {
                var elem = new FhirDateTime(coreResult.Observationdatetime.Value);
                elem.ElementId = "effectiveDateTime";
                return elem;
            }
            return null;
        }

        private CodeableConcept GetResultCode(entitystorematerialised_CoreResult src)
        {
            var obsCodes = new CodeableConcept();

            Coding obsCode = new Coding();

            obsCode.Code = src.Observationidentifiercode;
            obsCode.Display = src.Observationidentifiertext;

            obsCodes.Coding.Add(obsCode);

            return obsCodes;
        }

        private List<Identifier> GetResultIdentifiers(entitystorematerialised_CoreResult coreObservationResult)
        {
            var identifiers = new List<Identifier>();

            Identifier id = new Identifier
            {
                Value = coreObservationResult.ResultId
            };

            identifiers.Add(id);

            return identifiers;
        }
    }

    internal class ReportPresentedFormResolver : IValueResolver<entitystorematerialised_CoreReport, DiagnosticReport, List<Attachment>>
    {
        public List<Attachment> Resolve(entitystorematerialised_CoreReport coreReport, DiagnosticReport destination, List<Attachment> destMember, ResolutionContext context)
        {
            var attachments = new List<Attachment>();

            if (destination.PresentedForm.IsCollectionValid())
            {
                destination.PresentedForm.Each(addr => attachments.Add(addr));
            }

            if (coreReport.IsNull()) return attachments;

            var attachment = GetReportInPresentedForm(coreReport, destination.Id);

            if (attachment != null)
                attachments.Add(attachment);

            return attachments;
        }

        private Attachment GetReportInPresentedForm(entitystorematerialised_CoreReport coreReport, string orderId)
        {
            Attachment attachment = null;

            if (coreReport.OrderId == orderId)
            {
                attachment = new Attachment();
                attachment.Title = coreReport.Title;
                attachment.ContentType = coreReport.Mimetypecode;
                attachment.Data = Encoding.UTF8.GetBytes(coreReport.Content);                
            }

            return attachment;
        }
    }

    internal class ReportResultSubjectResolver : IValueResolver<entitystorematerialised_CorePersonidentifier, DiagnosticReport, ResourceReference>
    {
        private IServiceProvider _provider;
        private string _defaultHospitalRefNo;

        public ReportResultSubjectResolver(IServiceProvider provider)
        {
            this._provider = provider;
            this._defaultHospitalRefNo = GetDefaultHospitalRefNo();
        }

        public ResourceReference Resolve(entitystorematerialised_CorePersonidentifier personIdentifier, DiagnosticReport destination, ResourceReference destMember, ResolutionContext context)
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
