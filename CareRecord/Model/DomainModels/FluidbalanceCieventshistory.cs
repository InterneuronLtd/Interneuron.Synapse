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


﻿using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_FluidbalanceCieventshistory : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string ContinuousinfusioneventId { get; set; }
        public string ContinuousinfusionId { get; set; }
        public string Eventtype { get; set; }
        public DateTime? Datetime { get; set; }
        public string Eventcorrelationid { get; set; }
        public string Pumpnumber { get; set; }
        public decimal? Totaladministeredvolume { get; set; }
        public decimal? Totalremainingvolume { get; set; }
        public decimal? Totalvolume { get; set; }
        public string Addedby { get; set; }
        public string Modifiedby { get; set; }
        public string Route { get; set; }
        public string Routetype { get; set; }
        public int? Sequenceid { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Fluidloss { get; set; }
        public decimal? Flowrate { get; set; }
        public decimal? Validatedvolume { get; set; }
        public DateTime? RmActualdatetime { get; set; }
        public string RmActualeventtype { get; set; }
        public string RmActualvalue { get; set; }
    }
}
