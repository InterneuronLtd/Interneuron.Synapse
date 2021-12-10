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

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_TerminusFluidbalancechart : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string FluidbalancechartId { get; set; }
        public string FluidbalanceeventId { get; set; }
        public string Eventtime { get; set; }
        public int? Displayorder { get; set; }
        public string Oral { get; set; }
        public string Oralobsid { get; set; }
        public string Iv { get; set; }
        public string Ivobsid { get; set; }
        public string Intakeroute1 { get; set; }
        public string Intakeobsid1 { get; set; }
        public string Intakeroute2 { get; set; }
        public string Intakeobsid2 { get; set; }
        public string Intakeroute3 { get; set; }
        public string Intakeobsid3 { get; set; }
        public string Intakeroute4 { get; set; }
        public string Intakeobsid4 { get; set; }
        public decimal? Oralvolume { get; set; }
        public decimal? Ivvolume { get; set; }
        public decimal? Intakevolume1 { get; set; }
        public decimal? Intakevolume2 { get; set; }
        public decimal? Intakevolume3 { get; set; }
        public decimal? Intakevolume4 { get; set; }
        public int? Intakerunningtotal { get; set; }
        public string Urine { get; set; }
        public string Urineobsid { get; set; }
        public string Vomit { get; set; }
        public string Vomitobsid { get; set; }
        public string Outputroute1 { get; set; }
        public string Outputobsid1 { get; set; }
        public string Outputroute2 { get; set; }
        public string Outputobsid2 { get; set; }
        public string Outputroute3 { get; set; }
        public string Outputobsid3 { get; set; }
        public string Outputroute4 { get; set; }
        public string Outputobsid4 { get; set; }
        public decimal? Urinevolume { get; set; }
        public decimal? Vomitvolume { get; set; }
        public decimal? Outputvolume1 { get; set; }
        public decimal? Outputvolume2 { get; set; }
        public decimal? Outputvolume3 { get; set; }
        public decimal? Outputvolume4 { get; set; }
        public int? Outputrunningtotal { get; set; }
    }
}
