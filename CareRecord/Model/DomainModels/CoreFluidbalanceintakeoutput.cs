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

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_CoreFluidbalanceintakeoutput : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string FluidbalanceintakeoutputId { get; set; }
        public string RowId { get; set; }
        public int? Sequenceid { get; set; }
        public string Contextkey { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdsource { get; set; }
        public string Createdmessageid { get; set; }
        public string Createdby { get; set; }
        public short? Recordstatus { get; set; }
        public string Timezonename { get; set; }
        public int? Timezoneoffset { get; set; }
        public string Tenant { get; set; }
        public string FluidbalancesessionrouteId { get; set; }
        public string RouteId { get; set; }
        public string RoutetypeId { get; set; }
        public decimal? Expectedvolume { get; set; }
        public decimal? Volume { get; set; }
        public string Units { get; set; }
        public DateTime? Datetime { get; set; }
        public string FluidcapturedeviceId { get; set; }
        public string FluidbalanceiotypeId { get; set; }
        public bool? Isamended { get; set; }
        public string Reasonforamend { get; set; }
        public string Reasonforremoval { get; set; }
        public bool? Isremoved { get; set; }
        public decimal? Personweight { get; set; }
        public string PersonId { get; set; }
        public string Addedby { get; set; }
        public string Modifiedby { get; set; }
        public string ContinuousinfusioneventId { get; set; }
        public string ContinuousinfusionvalidationId { get; set; }
        public bool? Isintake { get; set; }
        public string FluidbalancesessionId { get; set; }
        public string ContinuousinfusionId { get; set; }
        public string Otherroutetype { get; set; }
    }
}
