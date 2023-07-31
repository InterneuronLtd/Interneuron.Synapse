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
using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_LocalSecambEpcramended : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string SecambEpcramendedId { get; set; }
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
        public string SecambEpcrId { get; set; }
        public DateTime? Incidentdate { get; set; }
        public DateTime? Calltime { get; set; }
        public DateTime? Atscene { get; set; }
        public DateTime? Atpatient { get; set; }
        public DateTime? Leftscene { get; set; }
        public DateTime? Timeathospital { get; set; }
        public DateTime? Verbalhandover { get; set; }
        public DateTime? Patienthandover { get; set; }
        public bool? Cardiacaspiringiven { get; set; }
        public bool? Cardiacgtngiven { get; set; }
        public bool? Cardiacclopidogrelgiven { get; set; }
        public bool? Cardiacphtgiven { get; set; }
        public bool? Cardiacaspirincontraindicated { get; set; }
        public bool? Cardiacgtncontraindicated { get; set; }
        public bool? Cardiacclopidogrelcontraindicated { get; set; }
        public bool? Cardiacphtcontraindicated { get; set; }
        public bool? Phtchecklistcompleted { get; set; }
        public bool? Cardiacdelays { get; set; }
        public bool? Cardiacecgmonitored { get; set; }
        public bool? Cardiacecg12lead { get; set; }
        public bool? Cardiacsuspectedstemi { get; set; }
        public bool? Cardiacsuspectedacsnstemi { get; set; }
        public bool? Cardiacecgwithpcr { get; set; }
        public bool? Cardiacppcipathway { get; set; }
        public bool? Cardiactelemetrysent { get; set; }
        public bool? Cardiactelemetryfailed { get; set; }
        public string Cardiacecginterpretation { get; set; }
    }
}
