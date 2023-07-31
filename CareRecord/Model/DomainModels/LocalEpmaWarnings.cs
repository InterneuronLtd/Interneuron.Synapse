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
    public partial class entitystorematerialised_LocalEpmaWarnings : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string EpmaWarningsId { get; set; }
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
        public string Primarymedicationcode { get; set; }
        public string Secondarymedicationcode { get; set; }
        public string Primaryprescriptionid { get; set; }
        public string Secondaryprescriptionid { get; set; }
        public string Fdbmessageid { get; set; }
        public string Warningtype { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
        public string Fdbseverity { get; set; }
        public string Overridemessage { get; set; }
        public bool? Overriderequired { get; set; }
        public string Allergencode { get; set; }
        public string Allergeningredient { get; set; }
        public string Drugingredient { get; set; }
        public string Allergenmatchtype { get; set; }
        public string Updatetrigger { get; set; }
        public string Warningcategories { get; set; }
        public bool? Ispatientspecific { get; set; }
        public string Msgtype { get; set; }
        public string Primarymedicationname { get; set; }
        public string Secondarymedicationname { get; set; }
    }
}
