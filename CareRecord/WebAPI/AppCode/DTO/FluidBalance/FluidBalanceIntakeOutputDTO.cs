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

namespace Interneuron.CareRecord.API.AppCode.DTO.FluidBalance
{
    public class FluidBalanceIntakeOutputDTO
    {
		public string fluidbalanceintakeoutput_id { get; set; }
		public string fluidbalancesessionroute_id { get; set; }
		public string route_id { get; set; }
		public string routetype_id { get; set; }
		public float expectedvolume { get; set; }
		public float volume { get; set; }
		public string units { get; set; }
		public string datetime { get; set; }
		public string fluidcapturedevice_id { get; set; }
		public string fluidbalanceiotype_id { get; set; }
		public bool isamended { get; set; }
		public string reasonforamend { get; set; }
		public string reasonforremoval { get; set; }
		public bool isremoved { get; set; }
		public float personweight { get; set; }
		public string person_id { get; set; }
		public string addedby { get; set; }
		public string modifiedby { get; set; }
		public string continuousinfusionevent_id { get; set; }
		public string continuousinfusionvalidation_id { get; set; }
		public bool isintake { get; set; }
		public string fluidbalancesession_id { get; set; }
		public string continuousinfusion_id { get; set; }
		public string otherroutetype { get; set; }
	}
}
