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
    public partial class baseview_EboardsWardwaitingarea : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string Allocatedbedcode { get; set; }
        public string WardbaybedId { get; set; }
        public string Wardcode { get; set; }
        public string Warddisplay { get; set; }
        public string Currentwardcode { get; set; }
        public string Currentwarddisplay { get; set; }
        public string Baycode { get; set; }
        public string Baydisplay { get; set; }
        public string Bedcode { get; set; }
        public string Beddisplay { get; set; }
        public string Allocatedbeddisplay { get; set; }
        public int? Bedstatus { get; set; }
        public string Bedsortstring { get; set; }
        public string Bedbaystatusdisplay { get; set; }
        public bool? Bedenabled { get; set; }
        public string Statuscode { get; set; }
        public string Availability { get; set; }
        public string Statusdescription { get; set; }
        public string PersonId { get; set; }
        public string Nhsnumber { get; set; }
        public string Mrn { get; set; }
        public string Titletext { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Familyname { get; set; }
        public string Fullname { get; set; }
        public string Preferredname { get; set; }
        public string Dateofbirth { get; set; }
        public double? Ageinyears { get; set; }
        public TimeSpan? Agetext { get; set; }
        public string Dateofdeath { get; set; }
        public string Gender { get; set; }
        public bool? Deathindicator { get; set; }
        public string EncounterId { get; set; }
        public string Patienttypecode { get; set; }
        public string Patientclasstext { get; set; }
        public string Ward { get; set; }
        public string Bay { get; set; }
        public string Bed { get; set; }
        public string Wardbay { get; set; }
        public string Wardbaybed { get; set; }
        public string Admitdateformatted { get; set; }
        public DateTime? Dischargedatetime { get; set; }
        public string Intenteddischargedateformatted { get; set; }
        public string Episodestatuscode { get; set; }
        public string Episodestatustext { get; set; }
        public string Admitreasontext { get; set; }
        public double? Lengthofstay { get; set; }
        public string Visitnumber { get; set; }
        public string Consultingdoctorid { get; set; }
        public string Consultingdoctortext { get; set; }
        public string Scheduledfortheatrebadge { get; set; }
        public string TeamFlag { get; set; }
        public string OtFlag { get; set; }
        public string PtFlag { get; set; }
        public string TtaFlag { get; set; }
        public string VteFlag { get; set; }
        public string WardComments { get; set; }
        public string Wardinformationdetailed { get; set; }
        public string Wardinformationsimple { get; set; }
        public string Locatorboardtitleward { get; set; }
        public string Wardinformationformatted { get; set; }
        public string Wardnamelarge { get; set; }
        public string Wardcomment1 { get; set; }
        public string Patientbanner { get; set; }
        public string ColBed { get; set; }
        public string ColStatus { get; set; }
        public string ColPatient { get; set; }
        public string ColConsultant { get; set; }
        public string ColTheatredate { get; set; }
        public string ColExpecteddischargedate { get; set; }
        public string ColDays { get; set; }
        public string ColJuniorbleep { get; set; }
        public string ColNurse { get; set; }
        public string ColPttext { get; set; }
        public string ColOttext { get; set; }
        public string ColActions { get; set; }
        public string ColBadges { get; set; }
        public string ColRadiology { get; set; }
    }
}
