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


﻿using Interneuron.Terminology.API.AppCode.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.DTOs
{
    //public class DMDLookupDTO : TerminologyResource, ILookupItemDTO
    //{
    //    public string Cd { get; set; }
    //    public DateTime? Cddt { get; set; }
    //    public long? Cdprev { get; set; }
    //    public string Desc { get; set; }
    //    public short? Invalid { get; set; }
    //    public short? Recordstatus { get; set; }
    //}

    public class DmdLookupAvailrestrictDTO : TerminologyResource, ILookupItemDTO
    {
        public short? Recordstatus { get; set; }
        public long? Cd { get; set; }
        public string Desc { get; set; }
    }

    public class DmdLookupBasisofnameDTO : TerminologyResource, ILookupItemDTO
    {
        public short? Recordstatus { get; set; }
        public long? Cd { get; set; }
        public string Desc { get; set; }
    }

    public class DmdLookupLicauthDTO : TerminologyResource, ILookupItemDTO
    {
        public short? Recordstatus { get; set; }
        public long? Cd { get; set; }
        public string Desc { get; set; }
    }

    public partial class DmdLookupDrugformindDTO : TerminologyResource, ILookupItemDTO
    {
        public short? Recordstatus { get; set; }
        public long? Cd { get; set; }
        public string Desc { get; set; }
    }

    public class DmdLookupOntformrouteDTO : TerminologyResource, ILookupItemDTO
    {
        public short? Recordstatus { get; set; }
        public long? Cd { get; set; }
        public string Desc { get; set; }
    }

    public class DmdLookupUomDTO : TerminologyResource, ILookupItemDTO
    {
        public short? Recordstatus { get; set; }
        public string Cd { get; set; }
        public DateTime? Cddt { get; set; }
        public string Cdprev { get; set; }
        public string Desc { get; set; }
        public bool? IsLatest { get; set; }
    }

    public partial class DmdLookupIngredientDTO : TerminologyResource, ILookupItemDTO
    {
        public short? Recordstatus { get; set; }
        public string Isid { get; set; }
        public DateTime? Isiddt { get; set; }
        public string Isidprev { get; set; }
        public short? Invalid { get; set; }
        public string Nm { get; set; }
        public bool? IsLatest { get; set; }
    }

    public partial class DmdLookupBasisofstrengthDTO : TerminologyResource, ILookupItemDTO
    {
        public short? Recordstatus { get; set; }
        public long? Cd { get; set; }
        public string Desc { get; set; }
    }

    public class DmdLookupBNFDTO : TerminologyResource, ILookupItemDTO
    {
        public short? Recordstatus { get; set; }
        public string Cd { get; set; }
        public string Desc { get; set; }
    }

}
