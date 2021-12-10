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
    public class UrineOutputHistoryDTO
    {
        public string fluidbalanceintakeoutput_id { get; set; }
        public string fluidbalanceescalation_id { get; set; }
        public string units { get; set; }
        public string datetime { get; set; }
        public float volume { get; set; }
        public int age { get; set; }
        public float personweight { get; set; }
        public float expectedvolume { get; set; }
        public float averagevolume { get; set; }
        public string sbar { get; set; }
        public string addedby { get; set; }
        public string modifiedby { get; set; }

        public string device { get; set; }
    }
}
