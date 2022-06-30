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
    public partial class entitystorematerialised_CoreProceduredetail : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string ProceduredetailId { get; set; }
        public string RowId { get; set; }
        public int? Sequenceid { get; set; }
        public string Contextkey { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdsource { get; set; }
        public string Createdmessageid { get; set; }
        public string Createdby { get; set; }
        public short? Recordstatus { get; set; }
        public string Timezonename { get; set; }
        public int? Timezoneoffset { get; set; }
        public string Tenant { get; set; }
        public string Skinincisionandapproachcode { get; set; }
        public string Skinincisionandapproachtext { get; set; }
        public string Assessmentofmargincode { get; set; }
        public string Assessmentofmargintext { get; set; }
        public string Deepclosurecode { get; set; }
        public string Deepclosuretext { get; set; }
        public string Skinclosurecode { get; set; }
        public string Skinclosuretext { get; set; }
        public string Skinrepairedwithcode { get; set; }
        public string Skinrepairedwithtext { get; set; }
        public string Drainlabel { get; set; }
        public string Drainplacement { get; set; }
        public string Drainwhentoremove { get; set; }
        public string Dressingcode { get; set; }
        public string Dressingtext { get; set; }
        public bool? Istorniquetapplied { get; set; }
        public string Torniquetpressure { get; set; }
        public string Localanaestheticinfiltration { get; set; }
        public string Procedurecomment { get; set; }
        public string ProcedureId { get; set; }
        public string Torniquettime { get; set; }
        public string Positioningcode { get; set; }
        public string Positioningtext { get; set; }
        public string Skinpreparationcode { get; set; }
        public string Skinpreparationtext { get; set; }
        public bool? Isdifferentteam { get; set; }
        public string Closuretext { get; set; }
        public decimal? Fluidloss { get; set; }
        public decimal? Bloodloss { get; set; }
        public string Skinpreparationothertext { get; set; }
        public string Laterality { get; set; }
        public bool? Candourthreshold { get; set; }
        public string Candourthresholdreached { get; set; }
        public bool? Armtable { get; set; }
        public bool? Beanbag { get; set; }
        public bool? Beachchair { get; set; }
        public bool? Jacksontable { get; set; }
        public bool? Equipmentother { get; set; }
        public string Equipmentothertext { get; set; }
        public string Positioningothertext { get; set; }
        public string Othercomments { get; set; }
        public string Unexpectedintraoperative { get; set; }
        public string Otherindication { get; set; }
        public string Otherindicationnotes { get; set; }
        public string Implantproceduretype { get; set; }
        public bool? Sidesupport { get; set; }
        public bool? Footsupport { get; set; }
        public bool? Skindrapingwithimpermeabledrapes { get; set; }
    }
}
