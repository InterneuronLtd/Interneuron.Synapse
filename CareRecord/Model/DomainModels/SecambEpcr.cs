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
    public partial class baseview_SecambEpcr : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string SecambEpcrId { get; set; }
        public string Pcrformid { get; set; }
        public DateTime? Incidentdate { get; set; }
        public string Incidentdateformatted { get; set; }
        public string Cadincidentnumber { get; set; }
        public string Callsignletter { get; set; }
        public string Callsignnumber { get; set; }
        public string Stationbase { get; set; }
        public string PersonId { get; set; }
        public string Locationofincident { get; set; }
        public DateTime? Calltime { get; set; }
        public string Calltimeformatted { get; set; }
        public DateTime? Atscene { get; set; }
        public string Atsceneformatted { get; set; }
        public DateTime? Atpatient { get; set; }
        public string Atpatientformatted { get; set; }
        public DateTime? Leftscene { get; set; }
        public string Leftsceneformatted { get; set; }
        public DateTime? Timeathospital { get; set; }
        public string Timeathospitalformatted { get; set; }
        public DateTime? Verbalhandover { get; set; }
        public string Verbalhandoverformatted { get; set; }
        public DateTime? Patienthandover { get; set; }
        public string Patienthandoverformatted { get; set; }
        public bool? Cardiacaspiringiven { get; set; }
        public bool? Cardiacgtngiven { get; set; }
        public bool? Cardiacclopidogrelgiven { get; set; }
        public bool? Cardiacphtgiven { get; set; }
        public bool? Cardiacaspirincontraindicated { get; set; }
        public bool? Cardiacgtncontraindicated { get; set; }
        public bool? Cardiacclopidogrelcontraindicated { get; set; }
        public bool? Cardiacphtcontraindicated { get; set; }
        public bool? Phtchecklistcompleted { get; set; }
        public bool? Cardiacdelays { get; set; }
        public bool? Cardiacecgmonitored { get; set; }
        public bool? Cardiacecg12lead { get; set; }
        public bool? Cardiacsuspectedstemi { get; set; }
        public bool? Cardiacsuspectedacsnstemi { get; set; }
        public bool? Cardiacecgwithpcr { get; set; }
        public bool? Cardiacppcipathway { get; set; }
        public bool? Cardiactelemetrysent { get; set; }
        public bool? Cardiactelemetryfailed { get; set; }
        public string Patientassessment { get; set; }
        public string Hospitalcentrecode { get; set; }
        public string Hospwarddept { get; set; }
        public string Qualitycheckinitials { get; set; }
        public string Auditoutcome { get; set; }
        public string Auditoutcometext { get; set; }
        public string Auditby { get; set; }
        public string Mitigatingreason { get; set; }
        public string Mitigatingreasoncomments { get; set; }
        public string Cardiacecginterpretation { get; set; }
        public bool? Manualcadmatch { get; set; }
        public bool? Isammeded { get; set; }
        public int? Calltoatscene { get; set; }
        public int? Calltoatpatient { get; set; }
        public int? Timeonscene { get; set; }
        public int? Timeonroute { get; set; }
        public int? Hospitalwaitingtime { get; set; }
        public int? Calltimetohandover { get; set; }
        public string Fullname { get; set; }
        public string Dateofbirthformatted { get; set; }
    }
}
