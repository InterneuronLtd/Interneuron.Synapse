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
﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SynapseStudioWeb.Models
{
    public class LocationModel
    {
        public string location_id { get; set; }
        [Required(ErrorMessage = "Please enter location code")]
        public string locationcode { get; set; }
        [Required(ErrorMessage = "Please enter location text")]
        public string locationtext { get; set; }
        [Required(ErrorMessage = "Please select location type")]
        public string locationtypecode { get; set; }
        [Required(ErrorMessage = "Please select location type")]
        public string locationtypetext { get; set; }
        [Required(ErrorMessage = "Please enter institution")]
        public string institution { get; set; }
        [Required(ErrorMessage = "Please enter building")]
        public string building { get; set; }
        [Required(ErrorMessage = "Please select status")]
        public string statuscode { get; set; }
        public string statustext { get; set; }
    }

    public class LocationType
    {
        public string locationtype_id { get; set; }
        public string locationtypecode { get; set; }
        public string locationtypetext { get; set; }
    }

    public class LocationAddEditViewModel
    {
        public LocationModel location { get; set; }
        public List<LocationType> locationTypes { get; set; }
    }
}
