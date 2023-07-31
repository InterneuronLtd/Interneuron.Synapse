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

namespace Interneuron.Terminology.Model.DomainModels
{
    public partial class DmdAmp : Interneuron.Terminology.Infrastructure.Domain.EntityBase
    {
        public string AmpId { get; set; }
        public string RowId { get; set; }
        public int Sequenceid { get; set; }
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
        public string Apid { get; set; }
        public short? Invalid { get; set; }
        public string Vpid { get; set; }
        public string Nm { get; set; }
        public string Abbrevnm { get; set; }
        public string Desc { get; set; }
        public DateTime? Nmdt { get; set; }
        public string NmPrev { get; set; }
        public string Suppcd { get; set; }
        public long? LicAuthcd { get; set; }
        public long? LicAuthPrevcd { get; set; }
        public long? LicAuthchangecd { get; set; }
        public DateTime? LicAuthchangedt { get; set; }
        public long? Combprodcd { get; set; }
        public long? Flavourcd { get; set; }
        public int? Ema { get; set; }
        public int? ParallelImport { get; set; }
        public long? AvailRestrictcd { get; set; }
        public Guid? ColValHash { get; set; }
    }
}
