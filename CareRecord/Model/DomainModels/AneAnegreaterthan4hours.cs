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
    public partial class baseview_AneAnegreaterthan4hours : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string Mrn { get; set; }
        public string PersonId { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string Dtaward { get; set; }
        public string Referredtospecialty { get; set; }
        public string Anecategory { get; set; }
        public string Anelocation { get; set; }
        public double? DatePart { get; set; }
        public string Gender { get; set; }
        public string ArrivalTime { get; set; }
        public string Dynamiccss { get; set; }
        public double? Losminutes { get; set; }
        public string Loshours { get; set; }
        public string Los { get; set; }
        public string PresentingComplaint { get; set; }
        public string Clinician { get; set; }
        public string RatSee { get; set; }
        public string Ct { get; set; }
        public string ReferenceTo { get; set; }
        public string Triaged { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Triagedatetime { get; set; }
        public string Treatmentstartdatetime { get; set; }
        public string Referaltime { get; set; }
        public string Fullname { get; set; }
        public string Lessthen72 { get; set; }
    }
}
