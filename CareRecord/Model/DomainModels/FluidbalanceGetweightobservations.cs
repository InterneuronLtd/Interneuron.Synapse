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
    public partial class baseview_FluidbalanceGetweightobservations : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public DateTime? Datestarted { get; set; }
        public DateTime? Datefinished { get; set; }
        public string Addedby { get; set; }
        public string EncounterId { get; set; }
        public bool? Isamended { get; set; }
        public decimal? Observationfrequency { get; set; }
        public string ObservationscaletypeId { get; set; }
        public bool? Escalationofcare { get; set; }
        public string Reasonforamend { get; set; }
        public string Reasonfordelete { get; set; }
        public string Reasonforincompleteobservations { get; set; }
        public bool? Incomplete { get; set; }
        public DateTime? Observationeventdatetime { get; set; }
        public string ObservationId { get; set; }
        public DateTime? Timerecorded { get; set; }
        public string ObservationeventId { get; set; }
        public string ObservationtypeId { get; set; }
        public string Value { get; set; }
        public bool? Hasbeenammended { get; set; }
        public string Eventcorrelationid { get; set; }
        public string Method { get; set; }
    }
}
