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
    public partial class baseview_BvCorePersonorder : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public string OrderId { get; set; }
        public DateTime? Observationdatetime { get; set; }
        public string Universalservicetext { get; set; }
        public string Resultstatus { get; set; }
        public string Category { get; set; }
        public string Ordertype { get; set; }
        public DateTime? Datetimeoftransaction { get; set; }
        public DateTime? Requesteddatetime { get; set; }
        public string Fillerordernumber { get; set; }
        public string Placerordernumber { get; set; }
        public string Orderingprovider { get; set; }
        public DateTime? Statuschangedatetime { get; set; }
        public string Specimentypecode { get; set; }
        public string Specimentypetext { get; set; }
        public DateTime? Specimendatetime { get; set; }
        public long? Abnormalflagcount { get; set; }
        public long? Totalviewcount { get; set; }
    }
}
