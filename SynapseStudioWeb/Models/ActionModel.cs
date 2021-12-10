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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SynapseStudioWeb.Models
{
    public class ActionModel
    {
        public ActionModel()
        {
            this.action_id = System.Guid.NewGuid().ToString();
        }
        public string action_id { get; set; }
        [Required(ErrorMessage = "Please enter the Action Name")]
        public string ActionName { get; set; }
        [Required(ErrorMessage = "Please enter the Action Description")]
        public string ActionDescription { get; set; }
        public bool isEndpoint { get; set; }

    }

    public class ActionentityAssociations
    {
        public string actionentityassociations_id { get; set; }
        public string actionname { get; set; }
        public string entityname { get; set; }
        public string permission_type { get; set; }

    }

    public class RollGridModel
    {
        public string roleprevilage_id { get; set; }
        public string objectname { get; set; }
        public string permission_type { get; set; }
        
    }
}
