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


namespace Interneuron.CareRecord.HL7SynapseService.Interfaces
{
    public class FHIRParam : IFHIRParam
    {
        public string Base { get; set; }
        public string TypeName { get; set; }
        public string ResourceId { get; set; }
        public string VersionId { get; set; }

        public FHIRParam() { }

        public FHIRParam(string _base, string type, string resourceid, string versionid)
        {
            this.Base = _base != null ? _base.TrimEnd('/') : null;
            this.TypeName = type;
            this.ResourceId = resourceid;
            this.VersionId = versionid;
        }

        public static FHIRParam Create(string type)
        {
            return new FHIRParam(null, type, null, null);
        }

        public static FHIRParam Create(string type, string resourceid)
        {
            return new FHIRParam(null, type, resourceid, null);
        }

        public static FHIRParam Create(string type, string resourceid, string versionid)
        {
            return new FHIRParam(null, type, resourceid, versionid);
        }

        public static FHIRParam Null
        {
            get
            {
                return default(FHIRParam);
            }
        }

        
    }
}
