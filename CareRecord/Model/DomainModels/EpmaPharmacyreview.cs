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
    public partial class baseview_EpmaPharmacyreview : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public string PrescriptionId { get; set; }
        public string Status { get; set; }
        public string Fullname { get; set; }
        public string Dateofbirth { get; set; }
        public int? Age { get; set; }
        public string Hospitalnumber { get; set; }
        public string Gendertext { get; set; }
        public string Assignedpatientlocationpointofcare { get; set; }
        public string Attendingdoctorcode { get; set; }
        public string Referringdoctorid { get; set; }
        public string Clinicalunit { get; set; }
        public string Nextdosevalue { get; set; }
        public string Lastchangeoftherapy { get; set; }
        public string Therapymodificationtype { get; set; }
        public decimal? Reminderdays { get; set; }
        public string Remindernotes { get; set; }
        public string Name { get; set; }
        public string Statusbadge { get; set; }
        public int? Displayorder { get; set; }
    }
}
