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

namespace Interneuron.CareRecord.API.AppCode.DTO.FluidBalance
{
    public class ExpectedUrineOutputDTO
    {
		public string expectedurineoutput_id { get; set; }
		public string description { get; set; }
		public float agefrom { get; set; }
		public float ageto { get; set; }
		public float volume { get; set; }
		public string units { get; set; }
	}
}