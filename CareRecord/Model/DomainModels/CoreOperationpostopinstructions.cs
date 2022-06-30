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
    public partial class entitystorematerialised_CoreOperationpostopinstructions : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string OperationpostopinstructionsId { get; set; }
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
        public string OperationId { get; set; }
        public string Observation { get; set; }
        public string Postoperativeantibiotics { get; set; }
        public string Rehabilitation { get; set; }
        public string Weightbearingstatuscode { get; set; }
        public string Weightbearingstatustext { get; set; }
        public string Weightbearingstatusfurtherinformation { get; set; }
        public DateTime? Anticipateddayofdischarge { get; set; }
        public string Woundcheck { get; set; }
        public string Clinicreview { get; set; }
        public string Anticipatedbloodloss { get; set; }
        public string Prophylaxis { get; set; }
        public string Careplanpathway { get; set; }
        public string Prophylaxistype { get; set; }
        public string Mechanicalsection { get; set; }
        public string Bedresttext { get; set; }
        public bool? Mechanicalted { get; set; }
        public bool? Mechanicalscd { get; set; }
        public string Mechanicaltedoption { get; set; }
        public string Mechanicalscdoption { get; set; }
    }
}
