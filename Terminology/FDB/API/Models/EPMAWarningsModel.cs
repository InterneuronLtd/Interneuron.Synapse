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
using System.Linq;
using System.Web;

namespace InterneuronFDBAPI.Models
{
    public class EPMAWarnings
    {
        public string epma_warnings_id { get; set; }
        public string person_id { get; set; }
        public string encounter_id { get; set; }
        public string primarymedicationcode { get; set; }
        public string secondarymedicationcode { get; set; }
        public string primarymedicationname { get; set; }
        public string secondarymedicationame { get; set; }
        public string primaryprescriptionid { get; set; }
        public string secondaryprescriptionid { get; set; }
        public string fdbmessageid { get; set; }
        public string warningtype { get; set; }
        public string message { get; set; }
        public string severity { get; set; }
        public string fdbseverity { get; set; }
        public string overridemessage { get; set; }
        public bool? overriderequired { get; set; }
        public string allergencode { get; set; }
        public string allergeningredient { get; set; }
        public string drugingredient { get; set; }
        public string allergenmatchtype { get; set; }
        public string updatetrigger { get; set; }
        public string warningcategories { get; set; }
        public string msgtype { get; set; }
        public bool ispatientspecific { get; set; }
        public string alertmsgplaintext { get; set; }
    }

}