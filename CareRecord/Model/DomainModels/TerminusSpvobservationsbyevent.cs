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
    public partial class baseview_TerminusSpvobservationsbyevent : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string ObservationeventId { get; set; }
        public string PersonId { get; set; }
        public DateTime? Datestarted { get; set; }
        public DateTime? Datefinished { get; set; }
        public string Addedby { get; set; }
        public string Clvl { get; set; }
        public string ClvlSymbol { get; set; }
        public string Sbp { get; set; }
        public string SbpSymbol { get; set; }
        public string Dbp { get; set; }
        public string DbpSymbol { get; set; }
        public string Hr { get; set; }
        public string HrSymbol { get; set; }
        public string Resp { get; set; }
        public string RespSymbol { get; set; }
        public string Spo2 { get; set; }
        public string Spo2Symbol { get; set; }
        public string Height { get; set; }
        public string HeightSymbol { get; set; }
        public string Weight { get; set; }
        public string WeightSymbol { get; set; }
    }
}
