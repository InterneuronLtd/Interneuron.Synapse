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
    public partial class baseview_BvCoreClinicalsummary : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public string Fullname { get; set; }
        public string Dob { get; set; }
        public string Assignedpatientlocationpointofcare { get; set; }
        public string Gendertext { get; set; }
        public string EncounterId { get; set; }
        public string Location { get; set; }
        public string Bedinfo { get; set; }
        public string Consultingdoctorid { get; set; }
        public string Consultingdoctortext { get; set; }
        public string Idnumber { get; set; }
        public string Notes { get; set; }
        public string Lastoperationdate { get; set; }
        public decimal? Score { get; set; }
        public string Placeholder { get; set; }
        public string Patient { get; set; }
        public string Info { get; set; }
    }
}
