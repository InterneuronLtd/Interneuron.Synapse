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

namespace SynapseStudioWeb.Models
{
    public class WardBayModel
    {
        public string wardbay_id { get; set; }        
        [Required(ErrorMessage = "Please select ward code")]
        public string wardcode { get; set; }
        [Required(ErrorMessage = "Please enter ward display")]
        public string warddisplay { get; set; }
        [Required(ErrorMessage = "Please enter bay code")]
        public string baycode { get; set; }
        [Required(ErrorMessage = "Please enter bay display")]
        public string baydisplay { get; set; }
    }

    public class WardBayViewModel
    {
        public List<WardModel> Wards { get; set; }
        public List<WardBayModel> WardBays { get; set; }
        public string SelectedWardCode { get; set; }
    }

    public class AddEditWardBayViewModel
    {
        public List<WardModel> Wards { get; set; }
        public WardBayModel WardBay { get; set; }
    }
}
