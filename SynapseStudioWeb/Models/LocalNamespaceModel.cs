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


﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Models
{ 
    public class LocalNamespaceModel
    {
        public string LocalNamespaceId { get; set; }
        [Required(ErrorMessage = "Please enter a name for the new local namespace")]
        [RegularExpression("^[a-z0-9 -]+$", ErrorMessage = "Namespace can be only non-alphanumeric and lower case")]
        [Remote(action: "VerifyLocalNamespace", controller: "EntityManager")]
        public string LocalNamespaceName { get; set; }
        public string LocalNamespaceDescription { get; set; }
        public List<LocalNamespaceDto> LocalNamespaceDto { get; set; }
    }
    public class LocalNamespaceDto
    {
        public string LocalNamespaceId { get; set; }
        public string LocalNamespaceName { get; set; }
        public string LocalNamespaceDescription { get; set; }
    }
}
