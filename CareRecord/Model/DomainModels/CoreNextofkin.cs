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
using System.Diagnostics.CodeAnalysis;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class entitystorematerialised_CoreNextofkin : Interneuron.CareRecord.Infrastructure.Domain.EntityBase, IEquatable<entitystorematerialised_CoreNextofkin>
    {
        public string NextofkinId { get; set; }
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
        public string Statuschangereasoncodesystem { get; set; }
        public string Relationshipcodesystemdescription { get; set; }
        public string Contactrolerelationshipcodesystemdescription { get; set; }
        public string Addressstreet2 { get; set; }
        public string Middlename { get; set; }
        public string Contactroletext { get; set; }
        public string Rolenormalized { get; set; }
        public string Addresspostalcode { get; set; }
        public string Addressstreet { get; set; }
        public string Addresstypecodesystemversion { get; set; }
        public string Statuscodesystem { get; set; }
        public string Statuschangereasoncode { get; set; }
        public string Addresscity { get; set; }
        public string Statuscode { get; set; }
        public string Addresscountry { get; set; }
        public string Statuschangereasontext { get; set; }
        public string Primarylanguagecode { get; set; }
        public string Personalcontactinfo { get; set; }
        public string Addresscountystateprovince { get; set; }
        public string Relationshiptext { get; set; }
        public string Givenname { get; set; }
        public string Suffix { get; set; }
        public string Relationship { get; set; }
        public string Contactrolecode { get; set; }
        public string Contactrolerelationshipcodesystemversion { get; set; }
        public string Primarylanguagecodesystemversion { get; set; }
        public string Addresstypecodesystem { get; set; }
        public string Prefix { get; set; }
        public string Familyname { get; set; }
        public string Relationshipcode { get; set; }
        public string Addresstypecode { get; set; }
        public string Relationshipnormalized { get; set; }
        public string Role { get; set; }
        public string Businesscontactinfo { get; set; }
        public string Fullname { get; set; }
        public string Addresstypedescription { get; set; }
        public string Primarylanguagetext { get; set; }
        public string Relationshipcodesystemversion { get; set; }
        public string Statuscodesystemversion { get; set; }
        public string Primarylanguagecodesystem { get; set; }
        public string Statuschangereasoncodesystemversion { get; set; }
        public string Statustext { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public DateTime? Startdate { get; set; }
        public decimal? Setid { get; set; }

        public bool Equals([AllowNull] entitystorematerialised_CoreNextofkin other)
        {
            //Check whether the compared object is null.
            if (Object.ReferenceEquals(other, null)) return false;

            //Check whether the compared object references the same data.
            if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal.
            return NextofkinId.Equals(other.NextofkinId);
        }

        public override int GetHashCode()
        {
            //Calculate the hash code for the product.
            return NextofkinId.GetHashCode();
        }
    }
}
