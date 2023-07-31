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
    public partial class baseview_ListsAlllists : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string RowId { get; set; }
        public int? Sequenceid { get; set; }
        public string Contextkey { get; set; }
        public string Contextid { get; set; }
        public DateTime? Createdtimestamp { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdsource { get; set; }
        public string Createdchannelid { get; set; }
        public string Createdmessageid { get; set; }
        public string Createdby { get; set; }
        public short? Recordstatus { get; set; }
        public DateTime? Expirydate { get; set; }
        public string Timezonename { get; set; }
        public int? Timezoneoffset { get; set; }
        public string ListId { get; set; }
        public string Listname { get; set; }
        public string Listdescription { get; set; }
        public string Listcontextkey { get; set; }
        public string BaseviewId { get; set; }
        public string Listnamespaceid { get; set; }
        public string Listnamespace { get; set; }
        public string Defaultcontext { get; set; }
        public string Defaultcontextfield { get; set; }
        public string Matchedcontextfield { get; set; }
        public string Tablecssstyle { get; set; }
        public string Defaultrowcssstyle { get; set; }
        public string Tableheadercssstyle { get; set; }
        public string Baseviewwardfield { get; set; }
        public string Patientbannerfield { get; set; }
        public string Rowcssfield { get; set; }
    }
}
