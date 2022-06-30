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


using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_AutonomicObservationevent : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string ObservationeventId { get; set; }
        public string PersonId { get; set; }
        public DateTime? Datestarted { get; set; }
        public DateTime? Datefinished { get; set; }
        public double? Resp { get; set; }
        public string Respunits { get; set; }
        public double? Spo2 { get; set; }
        public string Spo2units { get; set; }
        public string Onoxygen { get; set; }
        public double? Oxygenperc { get; set; }
        public double? Oxygenlpm { get; set; }
        public double? Bps { get; set; }
        public string Bpsunits { get; set; }
        public double? Pulse { get; set; }
        public string Pulseunits { get; set; }
        public string Acvpu { get; set; }
        public double? Temp { get; set; }
        public string Tempunits { get; set; }
        public string Scaletype { get; set; }
        public string ScoreId { get; set; }
        public double? Hr { get; set; }
        public string Concern { get; set; }
        public string Respdistress { get; set; }
    }
}
