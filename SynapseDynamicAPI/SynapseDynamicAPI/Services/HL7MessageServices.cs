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


﻿
using NHapi.Base.Parser;
using NHapi.Model.V24.Message;
using SynapseDynamicAPI.Models;
using System;

namespace SynapseDynamicAPI.Services
{
    public class HL7MessageServices
    {
        public static string GenerateInpatientTransferMessage(InpatientTransferMessageModel messageData, out string messageControlId)
        {
            string messageDateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            messageControlId = Guid.NewGuid().ToString();

            ADT_A02 adtMessage = new ADT_A02();
            adtMessage.MSH.FieldSeparator.Value = "|";
            adtMessage.MSH.EncodingCharacters.Value = "^~\\&";
            adtMessage.MSH.SendingApplication.NamespaceID.Value = Environment.GetEnvironmentVariable("OutboundInterface_SendingApplicationName");
            adtMessage.MSH.ReceivingApplication.NamespaceID.Value = "PCS_IN";
            adtMessage.MSH.DateTimeOfMessage.TimeOfAnEvent.Value = messageDateTime;
            adtMessage.MSH.MessageType.MessageType.Value = "ADT";
            adtMessage.MSH.MessageType.TriggerEvent.Value = "A08";
            adtMessage.MSH.MessageControlID.Value = messageControlId;
            adtMessage.MSH.ProcessingID.ProcessingID.Value = "P";
            adtMessage.MSH.VersionID.VersionID.Value = "2.4";
            adtMessage.MSH.AcceptAcknowledgmentType.Value = "NE";
            adtMessage.MSH.ApplicationAcknowledgmentType.Value = "AL";
            adtMessage.MSH.CountryCode.Value = string.Empty;

            adtMessage.EVN.EventTypeCode.Value = "A08";
            adtMessage.EVN.EventOccurred.TimeOfAnEvent.Value = messageDateTime;

            adtMessage.PID.PatientID.ID.Value = messageData.MRN;
            adtMessage.PID.PatientID.AssigningAuthority.NamespaceID.Value = "PAS";
            adtMessage.PID.PatientID.IdentifierTypeCode.Value = "PAS";
            var pid3 = adtMessage.PID.GetPatientIdentifierList(0);
            pid3.ID.Value = string.Empty;

            adtMessage.PV1.SetIDPV1.Value = "1";
            adtMessage.PV1.PatientClass.Value = messageData.PatinetClassCode;
            if (!string.IsNullOrEmpty(messageData.WardCode))
            {
                adtMessage.PV1.AssignedPatientLocation.PointOfCare.Value = messageData.WardCode;
                adtMessage.PV1.AssignedPatientLocation.Room.Value = string.IsNullOrEmpty(messageData.BayCode) ? "\"\"" : messageData.BayCode;
                adtMessage.PV1.AssignedPatientLocation.Bed.Value = string.IsNullOrEmpty(messageData.BedCode) ? "\"\"" : messageData.BedCode;
            }

            var consultingDoctor = adtMessage.PV1.GetConsultingDoctor(0);
            string[] consultingDocName = messageData.ConsultingDoctorName.Split(' ');
            consultingDoctor.IDNumber.Value = messageData.ConsultingDoctorGMCCode;
            consultingDoctor.FamilyName.Surname.Value = messageData.ConsultingDoctorName;
            consultingDoctor.DegreeEgMD.Value = messageData.ConsultingDoctorPASId;


            var hospitalServiceComp = adtMessage.PV1.HospitalService.ExtraComponents.getComponent(0);
            var compValue = new NHapi.Model.V23.Datatype.ST(adtMessage);
            compValue.Value = messageData.SpecialtyCode;
            hospitalServiceComp.Data = compValue;

            //adtMessage.PV1.HospitalService.Value = messageData.SpecialtyCode;
            adtMessage.PV1.PatientType.Value = messageData.PatinetClassCode.Equals("I") ? "1" : "2";
            adtMessage.PV1.VisitNumber.ID.Value = messageData.VisitNumber;
            adtMessage.PV1.AdmitDateTime.TimeOfAnEvent.Value = messageData.BedTransferDateTime == null ? null : ((DateTime)messageData.BedTransferDateTime).ToString("yyyyMMddHHmmss");

            adtMessage.PV2.ExpectedDischargeDateTime.TimeOfAnEvent.Value = messageData.ExpectedDischargeDate;

            PipeParser pipeParser = new PipeParser();

            return pipeParser.Encode(adtMessage);
        }
    }
}
