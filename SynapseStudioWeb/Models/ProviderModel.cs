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
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SynapseStudioWeb.Models
{
    public class ProviderModel
    {
        public string provider_id { get; set; }
        [Required(ErrorMessage = "Please select prefix")]
        public string prefix { get; set; }
        [Required(ErrorMessage = "Please enter first name")]
        public string firstname { get; set; }
        public string middlename { get; set; }
        [Required(ErrorMessage = "Please enter last name")]
        public string lastname { get; set; }
        public string title { get; set; }
        public string emailid { get; set; }
        public string phonenumber { get; set; }
        public string suffix { get; set; }
        public string employer { get; set; }
        public string grade { get; set; }
        [Required(ErrorMessage = "Please select the role")]
        public string role { get; set; }
        public string userid { get; set; }
        public string notes { get; set; }
    }

    public class ProviderAddEditViewModel
    {
        public ProviderModel provider { get; set; }
        public Honorific honor { get; set; }
        public List<string> providerroles { get; set; }
    }

    public enum Honorific
    {
        Dr,
        Mr, 
        Miss,
        Mrs,
        Ms
    }

    public class ProviderRole
    {
        public string providerrole_id { get; set; }
        public string role { get; set; }
    }
}
