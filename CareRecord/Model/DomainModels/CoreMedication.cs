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


﻿using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_CoreMedication : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string MedicationId { get; set; }
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
        public string PrescriptionId { get; set; }
        public string Name { get; set; }
        public string Genericname { get; set; }
        public string Medicationtype { get; set; }
        public string Displayname { get; set; }
        public string Form { get; set; }
        public string Formcode { get; set; }
        public decimal? Strengthneumerator { get; set; }
        public decimal? Strengthdenominator { get; set; }
        public string Strengthneumeratorunit { get; set; }
        public string Strengthdenominatorunit { get; set; }
        public string Doseformunits { get; set; }
        public decimal? Doseformsize { get; set; }
        public string Doseformunitofmeasure { get; set; }
        public string Bnf { get; set; }
        public string Defineddailydose { get; set; }
        public string Doseform { get; set; }
        public string Doseperweight { get; set; }
        public string Doseperweightunit { get; set; }
        public decimal? Roundingfactor { get; set; }
        public string Actgroupcode { get; set; }
        public string Actgroupname { get; set; }
        public string Orderformtype { get; set; }
        public string Maxdoseperdayunit { get; set; }
        public decimal? Maxdoseperday { get; set; }
        public decimal? Maxdoseperweek { get; set; }
        public string Maxdoseperweekunit { get; set; }
        public string Titrationtype { get; set; }
        public string Producttype { get; set; }
        public bool? Isformulary { get; set; }
        public bool? Isblacktriangle { get; set; }
        public bool? Iscontrolled { get; set; }
        public bool? Iscritical { get; set; }
        public string Markedmodifier { get; set; }
        public decimal? Modifiedreleasehrs { get; set; }
        public decimal? Reviewreminderdays { get; set; }
        public bool? Isprimary { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public string Classification { get; set; }
        public bool? Isclinicaltrial { get; set; }
        public bool? Isexpensive { get; set; }
        public bool? Isunlicenced { get; set; }
        public bool? Ishighalert { get; set; }
        public string Customgroup { get; set; }
        public string Correlationid { get; set; }
        public bool? Isbloodproduct { get; set; }
        public bool? Isantimicrobial { get; set; }
    }
}
