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
    public partial class baseview_EpmaEncounters : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string Summary { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public string Assignedpatientlocationpointofcare { get; set; }
        public DateTime? Admitdatetime { get; set; }
        public DateTime? Dischargedatetime { get; set; }
        public string Consultingdoctorid { get; set; }
        public string Consultingdoctortext { get; set; }
        public string Episodestatustext { get; set; }
        public string Patientclasscode { get; set; }
        public string Patientclasstext { get; set; }
        public DateTime? Tcidate { get; set; }
        public DateTime? Intendedadmissiondate { get; set; }
        public string Intendedward { get; set; }
        public string Specialtytext { get; set; }
        public DateTime? Sortdate { get; set; }
    }
}
