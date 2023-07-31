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
    public partial class entitystorematerialised_CorePosology : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PosologyId { get; set; }
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
        public DateTime? Prescriptionstartdate { get; set; }
        public DateTime? Prescriptionenddate { get; set; }
        public string Frequency { get; set; }
        public decimal? Frequencysize { get; set; }
        public string Infusiontypeid { get; set; }
        public decimal? Infusionrate { get; set; }
        public decimal? Infusionduration { get; set; }
        public string Daysofweek { get; set; }
        public string Dosingdaysfrequency { get; set; }
        public decimal? Dosingdaysfrequencysize { get; set; }
        public string Prescriptionduration { get; set; }
        public decimal? Prescriptiodurationsize { get; set; }
        public bool? Repeatlastday { get; set; }
        public decimal? Repeatprotocoltimes { get; set; }
        public decimal? Doseperkg { get; set; }
        public decimal? Dosepersa { get; set; }
        public DateTime? Antimicrobialstartdate { get; set; }
        public bool? Doctorsorder { get; set; }
        public bool? Prn { get; set; }
        public bool? Isadditionaladministration { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public string Classification { get; set; }
        public string Dosetype { get; set; }
        public DateTime? Repeatlastdayuntil { get; set; }
        public decimal? Totalinfusionvolume { get; set; }
        public string Correlationid { get; set; }
        public decimal? Totalquantity { get; set; }
        public string Infusionrateunits { get; set; }
        public decimal? Maxnumofdosesperday { get; set; }
        public string Totalquantitytext { get; set; }
        public bool? Titration { get; set; }
        public string Titrationtype { get; set; }
        public decimal? Titrationtargetmin { get; set; }
        public decimal? Titrationtargetmax { get; set; }
        public string Titrationtargetunits { get; set; }
        public string Titrationtypecode { get; set; }
        public bool? Iscurrent { get; set; }
    }
}
