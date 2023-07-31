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


using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_EboardsBcpexport : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string Patientclass { get; set; }
        public string Wardcode { get; set; }
        public string Warddisplay { get; set; }
        public string Admittedward { get; set; }
        public string Admittedbed { get; set; }
        public string Allocatedward { get; set; }
        public string Allocatedbed { get; set; }
        public string Mrn { get; set; }
        public string Nhsnumber { get; set; }
        public bool? SamenameBadge { get; set; }
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Familyname { get; set; }
        public string Preferredname { get; set; }
        public string Dateofbirth { get; set; }
        public double? Ageinyears { get; set; }
        public string Gender { get; set; }
        public string Admitdate { get; set; }
        public double? Lengthofstay { get; set; }
        public string Intendeddischargedate { get; set; }
        public double? Ddelay { get; set; }
        public string ColTheatredate { get; set; }
        public string Consultant { get; set; }
        public string Juniorbleep { get; set; }
        public string Nurse { get; set; }
        public string ReadyfordischargeStatus { get; set; }
        public string PtStatus { get; set; }
        public string PtText { get; set; }
        public string OtStatus { get; set; }
        public string OtText { get; set; }
        public string TtaStatus { get; set; }
        public string TtaText { get; set; }
        public string DeliriumStatus { get; set; }
        public string DementiaStatus { get; set; }
        public string HahStatus { get; set; }
        public string TransportStatus { get; set; }
        public string VteStatus { get; set; }
        public string S2Status { get; set; }
        public string S5Status { get; set; }
        public string PatientsurveyStatus { get; set; }
        public string MicroStatus { get; set; }
        public bool? ScheduledfortheatreBadge { get; set; }
        public bool? ToreturnformhduFlag { get; set; }
        public bool? HelpwithfeedingFlag { get; set; }
        public bool? HydrantdeviceFlag { get; set; }
        public bool? DementiaFlag { get; set; }
        public bool? DiabetesFlag { get; set; }
        public bool? DeliriumFlag { get; set; }
        public bool? AllergyFlag { get; set; }
        public bool? LatextallergyFlag { get; set; }
        public bool? NilbymouthFlag { get; set; }
        public bool? DysphagiaFlag { get; set; }
        public bool? FluidswitheldFlag { get; set; }
        public bool? ErppathwayFlag { get; set; }
        public bool? PrivatepatientFlag { get; set; }
        public bool? SarcomaFlag { get; set; }
        public bool? TvnFlag { get; set; }
        public bool? RecoverypathwayFlag { get; set; }
        public string RadiologyStatus { get; set; }
        public string WardComments { get; set; }
    }
}
