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


using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_LocalAneCoding : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string AneCodingId { get; set; }
        public string RowId { get; set; }
        public int? Sequenceid { get; set; }
        public string Contextkey { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdsource { get; set; }
        public string Createdmessageid { get; set; }
        public string Createdby { get; set; }
        public short? Recordstatus { get; set; }
        public string Timezonename { get; set; }
        public int? Timezoneoffset { get; set; }
        public string Tenant { get; set; }
        public string PersonId { get; set; }
        public string Diagnosisarray { get; set; }
        public string Investigationarray { get; set; }
        public string Treatmentsarray { get; set; }
        public string Injuriesarray { get; set; }
        public string Medicinearray { get; set; }
        public string Minorilnessarray { get; set; }
        public string Obsandgynaearray { get; set; }
        public string Ophthalmologyarray { get; set; }
        public string Orthopaedicsarray { get; set; }
        public string Othercategoryarray { get; set; }
        public string Psychiatryarray { get; set; }
        public string Surgeryarray { get; set; }
        public string Urologyarray { get; set; }
        public string Anatomicalareaarray { get; set; }
        public string Additionaltreatmentsarray { get; set; }
        public string Anaesthesiaarray { get; set; }
        public string Dischargearray { get; set; }
        public string Lifesupportarray { get; set; }
        public string Medicationarray { get; set; }
        public string Minortreatmentsarray { get; set; }
        public string Monitoringarray { get; set; }
        public string Proceduresarray { get; set; }
        public string Woundclosuresarray { get; set; }
        public string Headandneckarray { get; set; }
        public string Upperlimbarray { get; set; }
        public string Trunkarray { get; set; }
        public string Lowerlimbarray { get; set; }
        public string Laterallityarray { get; set; }
        public string Informationcategoryarray { get; set; }
        public string Seniorreviewarray { get; set; }
        public string Dischargeplanningarray { get; set; }
        public string Dischargenowarray { get; set; }
        public string Maindiagnosis { get; set; }
        public string Reviewingdoctor { get; set; }
        public string Disposal { get; set; }
        public string Disposalward { get; set; }
        public string Comments { get; set; }
    }
}
