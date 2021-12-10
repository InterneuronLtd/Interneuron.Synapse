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
    public partial class baseview_PatientbannerMainbanner : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public string Fullname { get; set; }
        public string Preferredname { get; set; }
        public string Born { get; set; }
        public string Gender { get; set; }
        public string Ethnicgroup { get; set; }
        public string Religion { get; set; }
        public string Maritalstatus { get; set; }
        public string Nhsnumber { get; set; }
        public string Hospitalnumber { get; set; }
        public string Address { get; set; }
        public string Contactdetails { get; set; }
        public string Consultingdoctortext { get; set; }
        public string Currentlyadmittedto { get; set; }
    }
}
