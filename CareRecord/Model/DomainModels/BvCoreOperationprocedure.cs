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
    public partial class baseview_BvCoreOperationprocedure : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string ProcedureId { get; set; }
        public DateTime? Proceduredate { get; set; }
        public int? Duration { get; set; }
        public string PersonId { get; set; }
        public string EncounterId { get; set; }
        public bool? Isprimary { get; set; }
        public string OperationId { get; set; }
        public string Name { get; set; }
        public string Anaesthesiacode { get; set; }
        public string Proceduremodifiercode { get; set; }
        public string Proceduremodifiertext { get; set; }
        public int? Setid { get; set; }
        public decimal? Anaesthesiaminutes { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
    }
}
