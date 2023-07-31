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
using System.Diagnostics.CodeAnalysis;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_CoreProcedure : Interneuron.CareRecord.Infrastructure.Domain.EntityBase, IEquatable<entitystorematerialised_CoreProcedure>
    {
        public string ProcedureId { get; set; }
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
        public DateTime? Proceduredate { get; set; }
        public int? Duration { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public bool? Isprimary { get; set; }
        public string OperationId { get; set; }
        public string Name { get; set; }
        public string Anaesthesiacode { get; set; }
        public string Proceduremodifiercode { get; set; }
        public string Proceduremodifiertext { get; set; }
        public int? Setid { get; set; }
        public decimal? Anaesthesiaminutes { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public string Performedby { get; set; }
        public string ClinicalsummaryId { get; set; }
        public string Isdateapproximate { get; set; }
        public string Dateeffectiveperiod { get; set; }
        public string Effectivedatestring { get; set; }

        public bool Equals([AllowNull] entitystorematerialised_CoreProcedure other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return ProcedureId.Equals(other.ProcedureId);
        }

        public override int GetHashCode()
        {
            //Calculate the hash code for the product.
            return ProcedureId.GetHashCode();
        }
    }
}
