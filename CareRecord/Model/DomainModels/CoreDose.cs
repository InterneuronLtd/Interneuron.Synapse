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
    public partial class entitystorematerialised_CoreDose : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string DoseId { get; set; }
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
        public string PrescriptionId { get; set; }
        public string PosologyId { get; set; }
        public DateTime? Dosestartdatetime { get; set; }
        public DateTime? Doseenddatatime { get; set; }
        public decimal? Strengthneumerator { get; set; }
        public decimal? Strengthdenominator { get; set; }
        public string Strengthneumeratorunit { get; set; }
        public string Strengthdenominatorunit { get; set; }
        public string Dosesize { get; set; }
        public string Doseunit { get; set; }
        public string Dosemeasure { get; set; }
        public string Descriptivedose { get; set; }
        public decimal? Infusionrate { get; set; }
        public decimal? Infusionduration { get; set; }
        public bool? Titration { get; set; }
        public DateTime? Titrateddoseconfirmedon { get; set; }
        public string Titrateddoseconfirmedby { get; set; }
        public string Lastmodifiedby { get; set; }
        public bool? Isadditionaladministration { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public string Dosetype { get; set; }
        public string Continuityid { get; set; }
        public bool? Isbolus { get; set; }
        public string Additionaladministrationcomment { get; set; }
        public string Correlationid { get; set; }
        public decimal? Strengthneumeratorrangemax { get; set; }
        public decimal? Strengthdenominatorrangemax { get; set; }
        public decimal? Dosesizerangemax { get; set; }
        public decimal? Dosestrength { get; set; }
        public string Dosestrengthunits { get; set; }
        public decimal? Dosestrengthrangemax { get; set; }
    }
}
