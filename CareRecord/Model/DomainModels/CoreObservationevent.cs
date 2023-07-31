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
    public partial class entitystorematerialised_CoreObservationevent : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string ObservationeventId { get; set; }
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
        public string Eventcorrelationid { get; set; }
        public bool? Incomplete { get; set; }
    }
}
