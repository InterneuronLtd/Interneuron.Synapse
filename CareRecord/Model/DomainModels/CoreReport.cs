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


﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_CoreReport : Interneuron.CareRecord.Infrastructure.Domain.EntityBase, IEquatable<entitystorematerialised_CoreReport>
    {
        public string ReportId { get; set; }
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
        public string Author { get; set; }
        public string Signedby { get; set; }
        public string Title { get; set; }
        public string Typecode { get; set; }
        public string Typetext { get; set; }
        public string Statuscode { get; set; }
        public string Statustext { get; set; }
        public string Scantype { get; set; }
        public DateTime? Creationdatetime { get; set; }
        public DateTime? Effectivedatetime { get; set; }
        public DateTime? Effectiveperiodstart { get; set; }
        public DateTime? Effectiveperiodend { get; set; }
        public DateTime? Reportissueddatetime { get; set; }
        public string Formatcode { get; set; }
        public string Formatcodetext { get; set; }
        public string Healthcarefacilitycode { get; set; }
        public string Healthcarefacilitytext { get; set; }
        public string Mimetypecode { get; set; }
        public string Mimetypetext { get; set; }
        public string Content { get; set; }
        public string OrderId { get; set; }

        public bool Equals([AllowNull] entitystorematerialised_CoreReport other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return ReportId.Equals(other.ReportId);
        }

        public override int GetHashCode()
        {
            //Calculate the hash code for the product.
            return ReportId.GetHashCode();
        }
    }
}
