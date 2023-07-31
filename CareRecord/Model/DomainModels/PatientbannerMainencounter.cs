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
﻿using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_PatientbannerMainencounter : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public string Consultingdoctortext { get; set; }
        public DateTime? Admitdatetime { get; set; }
        public DateTime? Dischargedatetime { get; set; }
        public string Patientclasscode { get; set; }
        public string Wardcode { get; set; }
        public string Assignedpatientlocationpointofcare { get; set; }
        public string Warddisplay { get; set; }
        public string Warddisplayshort { get; set; }
        public string Assignedpatientlocationbay { get; set; }
        public string Assignedpatientlocationbed { get; set; }
        public string Bedcode { get; set; }
        public string Baydisplay { get; set; }
        public string Beddisplay { get; set; }
        public string Aliaslastname { get; set; }
        public string Aliasfirstname { get; set; }
    }
}
