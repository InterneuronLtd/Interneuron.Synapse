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
    public partial class entitystorematerialised_LocalEpmaPrescriptionreminders : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string EpmaPrescriptionremindersId { get; set; }
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
        public string MedicationId { get; set; }
        public DateTime? Activationdatetime { get; set; }
        public string Message { get; set; }
        public bool? Isacknowledged { get; set; }
        public string Ackmsg { get; set; }
        public string Acknowledgedby { get; set; }
        public string Lastmodifiedby { get; set; }
        public DateTime? Lastmodifiedon { get; set; }
        public bool? Isackmandatory { get; set; }
        public string Activationinhours { get; set; }
        public bool? Issystem { get; set; }
        public bool? Isivtooral { get; set; }
        public string Ackcomments { get; set; }
        public string Personid { get; set; }
        public string Encounterid { get; set; }
        public string Acknowledgedon { get; set; }
        public string Ackstatus { get; set; }
    }
}
