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


using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_LocalAneEncounter : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string AneEncounterId { get; set; }
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
        public DateTime? Arrivaltime { get; set; }
        public string Presentingcomplaint { get; set; }
        public string Clinician { get; set; }
        public string Ratorsee { get; set; }
        public string Ct { get; set; }
        public string Referredtospecialty { get; set; }
        public string Triagecategory { get; set; }
        public string Anecategory { get; set; }
        public string Anelocation { get; set; }
        public bool? Triaged { get; set; }
        public DateTime? Triagedatetime { get; set; }
        public DateTime? Treatmentstartdatetime { get; set; }
        public DateTime? Referredtospecialtytime { get; set; }
        public string Ambulancereference { get; set; }
        public string Disposal { get; set; }
        public string Anestatus { get; set; }
        public DateTime? Treatmentcompletedatetime { get; set; }
        public bool? Actualdeparturenow { get; set; }
        public DateTime? Departuredatetime { get; set; }
        public DateTime? Effectivedischargedatetime { get; set; }
        public bool? Editbreachoveride { get; set; }
        public string Reasonforbreach { get; set; }
        public string Xray { get; set; }
        public string Dtaward { get; set; }
        public string Allergyinformation { get; set; }
        public string Latestlabresults { get; set; }
    }
}
