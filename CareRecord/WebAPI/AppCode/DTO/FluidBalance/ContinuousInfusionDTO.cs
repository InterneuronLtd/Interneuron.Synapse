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

namespace Interneuron.CareRecord.API.AppCode.DTO.FluidBalance
{
    public class ContinuousInfusionDTO
    {
		public string continuousinfusion_id { get; set; }
		public string fluidbalancesessionroute_id { get; set; }
		public string routetype_id { get; set; }
		public float totalvolume { get; set; }
		public float flowrate { get; set; }
		public string flowrateunit { get; set; }
		public string pumpnumber { get; set; }
		public string startdatetime { get; set; }
		public string finishdatetime { get; set; }
		public bool ispaused { get; set; }
		public float totaladministeredvolume { get; set; }
		public float totalremainingvolume { get; set; }
		public string eventcorrelationid { get; set; }
		public bool islineremovedoncompletion { get; set; }
		public string completioncomments { get; set; }
		public string addedby { get; set; }
		public string modifiedby { get; set; }
		public string fluidbalancesession_id { get; set; }
		public string notes { get; set; }
		public string route_id { get; set; }
		public string reasonforpause { get; set; }
	}
}