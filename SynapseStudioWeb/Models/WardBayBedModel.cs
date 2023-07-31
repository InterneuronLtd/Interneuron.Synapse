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


﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Models
{
    public class WardBayBedModel
    {
        public string wardbaybed_id { get; set; }
        [Required(ErrorMessage = "Please select ward code")]
        public string wardcode { get; set; }
        public string warddisplay { get; set; }
        [Required(ErrorMessage = "Please select bay code")]
        public string baycode { get; set; }
        public string baydisplay { get; set; }
        [Required(ErrorMessage = "Please enter bed code")]
        public string bedcode { get; set; }
        [Required(ErrorMessage = "Please enter bed display")]
        public string beddisplay { get; set; }
        [Required(ErrorMessage = "Please enter bed bay display")]
        public string bedbaydisplay { get; set; }
        [Required(ErrorMessage = "Please select bed status")]
        public int? bedstatus { get; set; }
        [Required(ErrorMessage = "Please select")]
        public int? acceptmale { get; set; }
        [Required(ErrorMessage = "Please select")]
        public int? acceptfemale { get; set; }
        [Required(ErrorMessage = "Please enter minimum age")]
        public int? acceptminimumage { get; set; }
        [Required(ErrorMessage = "Please enter maximum age")]
        public int? acceptmaximumage { get; set; }
        [Required(ErrorMessage = "Please enter bed type")]
        public string bedtype { get; set; }
        [Required(ErrorMessage = "Please enter bed sort string")]
        public string bedsortstring { get; set; }
    }

    public class WardBayBedIndexViewModel
    {
        public List<WardModel> Wards { get; set; }
        public List<WardBayModel> WardBays { get; set; }
        public List<WardBayBedModel> WardBayBeds { get; set; }
        public string selectedWardCode { get; set; }
        public string selectedBayCode { get; set; }
    }

    public class WardBayBedAddEditViewModel
    {
        public List<WardModel> Wards { get; set; }
        public List<WardBayModel> WardBays { get; set; }
        public WardBayBedModel WardBayBed { get; set; }
    }
}
