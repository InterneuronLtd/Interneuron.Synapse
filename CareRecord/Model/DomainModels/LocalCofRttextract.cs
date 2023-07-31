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


using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_LocalCofRttextract : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string CofRttextractId { get; set; }
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
        public string EncounterId { get; set; }
        public string Cofkey { get; set; }
        public string Rttpathwayid { get; set; }
        public string Specialtycode { get; set; }
        public int? Rttcurrentwaittime { get; set; }
        public string Rttdescription { get; set; }
        public string Mrn { get; set; }
        public int? Rttwaittimeatappointment { get; set; }
        public string Cliniccode { get; set; }
        public string Clinicdescription { get; set; }
        public DateTime? Extractdatetime { get; set; }
        public DateTime? Rttstartdate { get; set; }
        public string Appointmentstatus { get; set; }
        public string Leadconsultantname { get; set; }
        public string Localrttcode { get; set; }
        public string Appointmenttime { get; set; }
        public DateTime? Appointmentdate { get; set; }
        public string Nationalrttcode { get; set; }
        public string Rttpathway { get; set; }
        public string Leadconsultantcode { get; set; }
        public DateTime? Rttbreachdate { get; set; }
        public string Clinicowner { get; set; }
    }
}
