 //Interneuron synapse

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
﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Models
{
    public class AttributeModel
    {
        public string AttributeId { get; set; }
        [Required(ErrorMessage = "Please enter a new name for the attribute")]
        [RegularExpression("^[a-z _]+$", ErrorMessage = "Attribute name cannot start with capital letter/numeric characters")]
        [Remote(action: "VerifyAttribute", controller: "EntityManager")]
        public string AttributeName { get; set; }
        [Required(ErrorMessage = "Please select a data type")]
        public string DataTypeId { get; set; }
        public List<AttributeDto> AttributeDtos { get; set; }
    }
    public class AttributeDto
    {
        public string AttributeId { get; set; }
        public string AttributeName { get; set; }
        public string DataTypeDetails { get; set; }
    }
}
