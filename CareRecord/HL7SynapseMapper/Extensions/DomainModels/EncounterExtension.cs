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
//using static Hl7.Fhir.Model.Encounter;

//namespace Interneuron.CareRecord.HL7SynapseService.Implementations.ModelExtensions
//{
//    public static class EncounterExtension
//    {
//        public static Encounter GetEncounter(this entitystorematerialised_CoreEncounter coreEncounter)
//        {
//            if (coreEncounter.IsNull()) return null;

//            var encounter = new Encounter
//            {
//                Id = coreEncounter.EncounterId,
//                Identifier = GetEncounterIdentifiers(coreEncounter),
//                Period = GetEncounterPeriod(coreEncounter),
//                ReasonCode = GetReasonCodes(coreEncounter),
//                Hospitalization = GetHospitalizationDetails(coreEncounter)
//            };

//            Coding encounterClass = new Coding();

//            encounterClass.Code = coreEncounter.Patientclasscode;

//            switch (coreEncounter.Patientclasscode.ToLower())
//            {
//                case "ip":
//                    encounterClass.Display = "Inpatient Encounter";
//                    break;
//                case "op":
//                    encounterClass.Display = "Outpatient Encounter";
//                    break;
//            }

//            encounter.Class = encounterClass;

//            if (coreEncounter.Episodestatuscode.IsEmpty()) return encounter;

//            switch (coreEncounter.Episodestatuscode.ToLower())
//            {
//                case "active":
//                    encounter.Status = Encounter.EncounterStatus.InProgress;
//                    break;
//                case "cancelled":
//                    encounter.Status = Encounter.EncounterStatus.Cancelled;
//                    break;
//            }

//            return encounter;
//        }

//        public static List<Identifier> GetEncounterIdentifiers(this entitystorematerialised_CoreEncounter coreEncounter)
//        {
//            var identifiers = new List<Identifier>();

//            if (coreEncounter.IsNull()) return identifiers;

//            Identifier id = new Identifier();

//            id.Value = coreEncounter.Visitnumber;

//            identifiers.Add(id);

//            return identifiers;
//        }

//        public static Period GetEncounterPeriod(this entitystorematerialised_CoreEncounter coreEncounter)
//        {
//            var period = new Period();

//            if (coreEncounter.IsNull()) return period;

//            period.Start = Convert.ToString(coreEncounter.Admitdatetime);
//            period.End = Convert.ToString(coreEncounter.Dischargedatetime);

//            return period;
//        }

//        public static List<CodeableConcept> GetReasonCodes(this entitystorematerialised_CoreEncounter coreEncounter)
//        {
//            var reasonCodes = new List<CodeableConcept>();

//            if (coreEncounter.IsNull()) return reasonCodes;

//            CodeableConcept reasonCode = new CodeableConcept();

//            reasonCode.Text = coreEncounter.Admitreasontext;

//            Coding coding = new Coding();

//            coding.Code = coreEncounter.Admitreasoncode;

//            reasonCode.Coding.Add(coding);

//            return reasonCodes;
//        }

//        public static HospitalizationComponent GetHospitalizationDetails(this entitystorematerialised_CoreEncounter coreEncounter)
//        {
//            var hospitalization = new HospitalizationComponent();

//            if (coreEncounter.IsNull()) return hospitalization;

//            hospitalization.AdmitSource = new CodeableConcept();

//            hospitalization.DischargeDisposition = new CodeableConcept();

//            hospitalization.AdmitSource.Text = coreEncounter.Referringdoctortext;

//            Coding admitSourceCode = new Coding();

//            admitSourceCode.Code = coreEncounter.Referringdoctorid;

//            hospitalization.AdmitSource.Coding.Add(admitSourceCode);

//            hospitalization.DischargeDisposition.Text = coreEncounter.Dischargetext;

//            Coding dischargeCode = new Coding();

//            dischargeCode.Code = coreEncounter.Dischargecode;

//            hospitalization.DischargeDisposition.Coding.Add(dischargeCode);

//            return hospitalization;
//        }

//    }

//}
