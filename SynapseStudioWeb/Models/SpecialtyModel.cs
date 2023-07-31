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
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SynapseStudioWeb.Models
{
    public class SpecialtyModel
    {
        public string specialty_id { get; set; }
        [Required(ErrorMessage = "Please enter specialty code")]
        public string specialtycode { get; set; }
        [Required(ErrorMessage = "Please enter specialty text")]
        public string specialtytext { get; set; }
        [Required(ErrorMessage = "Please select status")]
        public string statuscode { get; set; }
        public string statustext { get; set; }
        public string parentid { get; set; }
        public string localspecialtycode { get; set; }
        public string localspecialtytext { get; set; }
    }
}
