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
    public partial class baseview_EpmaReviewhistorydev : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public decimal? Titrationtargetmin { get; set; }
        public decimal? Titrationtargetmax { get; set; }
        public string Titrationtargetunits { get; set; }
        public string Correlationid { get; set; }
        public string PrescriptionId { get; set; }
        public string Indication { get; set; }
        public string Comments { get; set; }
        public decimal? Heparin { get; set; }
        public string Heparinunit { get; set; }
        public string PrescriptionadditionalconditionsId { get; set; }
        public decimal? Reminderdays { get; set; }
        public string Remindernotes { get; set; }
        public bool? Titration { get; set; }
        public string Titrationtype { get; set; }
        public decimal? Targetinr { get; set; }
        public decimal? Targetsaturation { get; set; }
        public string OxygendevicesId { get; set; }
        public string Orderformtype { get; set; }
        public bool? Isinfusion { get; set; }
        public string InfusiontypeId { get; set; }
        public bool? Ismedicinalgas { get; set; }
        public string PrescriptionsourceId { get; set; }
        public bool? Hasbeenmodified { get; set; }
        public string Reasonforediting { get; set; }
        public string Lastmodifiedby { get; set; }
        public string PrescriptionstatusId { get; set; }
        public string EpmaPrescriptioneventId { get; set; }
        public string Reasonforstopping { get; set; }
        public string Reasonforsuspending { get; set; }
        public bool? Allowsubstitution { get; set; }
        public string Substitutioncomments { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public DateTime? Createdon { get; set; }
        public string Createdby { get; set; }
        public DateTime? Lastmodifiedon { get; set; }
        public string Linkedinfusionid { get; set; }
        public DateTime? Startdatetime { get; set; }
        public string Posology { get; set; }
        public string Medications { get; set; }
        public string Routes { get; set; }
    }
}
