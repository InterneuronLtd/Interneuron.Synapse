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

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_ClinicalsummaryGetdiagnoses : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public DateTime? Recordedby { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? Onsetdate { get; set; }
        public string Clinicalstatus { get; set; }
        public string Verificationstatus { get; set; }
        public DateTime? Resolveddate { get; set; }
        public string Reportedby { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public string ClinicalsummaryId { get; set; }
        public string DiagnosisId { get; set; }
        public string Isdateapproximate { get; set; }
        public string Dateeffectiveperiod { get; set; }
        public string Effectivedatestring { get; set; }
    }
}
