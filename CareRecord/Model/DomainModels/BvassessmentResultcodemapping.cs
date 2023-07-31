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


using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_BvassessmentResultcodemapping : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public long? RowNumber { get; set; }
        public string ResultId { get; set; }
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
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public int? Setid { get; set; }
        public string Referencerange { get; set; }
        public DateTime? Analysisdatetime { get; set; }
        public string Identifiercode { get; set; }
        public string Identifiertext { get; set; }
        public string Abnormalflag { get; set; }
        public string OrderId { get; set; }
        public DateTime? Observationdatetime { get; set; }
        public string Observationidentifiercode { get; set; }
        public string Observationidentifiercodingsystem { get; set; }
        public string Observationidentifiertext { get; set; }
        public string Observationnotes { get; set; }
        public string Observationresultstatus { get; set; }
        public string Observationsubid { get; set; }
        public string Observationvalue { get; set; }
        public decimal? Observationvaluenumeric { get; set; }
        public string Referencerangehigh { get; set; }
        public string Referencerangelow { get; set; }
        public string Unitscode { get; set; }
        public string Unitstext { get; set; }
        public string Valuetype { get; set; }
        public string Author { get; set; }
        public DateTime? Creationdatetime { get; set; }
        public string Healthcarefacilitycode { get; set; }
        public string Healthcarefacilitytext { get; set; }
        public string Signedby { get; set; }
        public string Reporttitle { get; set; }
        public string Reporttypecode { get; set; }
        public string Reporttypetext { get; set; }
        public string Reportstatuscode { get; set; }
        public string Reportstatustext { get; set; }
        public DateTime? Reportexaminationdate { get; set; }
        public string Scantype { get; set; }
    }
}
