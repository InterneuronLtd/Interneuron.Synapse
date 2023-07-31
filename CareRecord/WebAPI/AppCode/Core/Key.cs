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


﻿using Interneuron.Common.Extensions;
using System.Collections.Generic;

namespace Interneuron.CareRecord.API.AppCode.Core
{
    public class Key : IKey
    {
        public string Base { get; set; }
        public string TypeName { get; set; }
        public string ResourceId { get; set; }
        public string VersionId { get; set; }

        public Key() { }

        public Key(string _base, string type, string resourceid, string versionid)
        {
            this.Base = _base != null ? _base.TrimEnd('/') : null;
            this.TypeName = type;
            this.ResourceId = resourceid;
            this.VersionId = versionid;
        }

        public static Key Create(string type)
        {
            return new Key(null, type, null, null);
        }

        public static Key Create(string type, string resourceid)
        {
            return new Key(null, type, resourceid, null);
        }

        public static Key Create(string type, string resourceid, string versionid)
        {
            return new Key(null, type, resourceid, versionid);
        }

        public static Key Null
        {
            get
            {
                return default(Key);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (Key)obj;
            return ToUriString(this) == ToUriString(other);
        }

        public override int GetHashCode()
        {
            return ToUriString(this).GetHashCode();
        }

        public static Key ParseOperationPath(string path)
        {
            Key fhirParam = new Key();
            path = path.Trim('/');
            int indexOfQueryString = path.IndexOf('?');
            if (indexOfQueryString >= 0)
            {
                path = path.Substring(0, indexOfQueryString);
            }
            string[] segments = path.Split('/');
            if (segments.Length >= 1) fhirParam.TypeName = segments[0];
            if (segments.Length >= 2) fhirParam.ResourceId = segments[1];
            if (segments.Length == 4 && segments[2] == "_history") fhirParam.VersionId = segments[3];
            return fhirParam;
        }

        public override string ToString()
        {
            return ToUriString(this);
        }

        private static string ToUriString(IKey fhirParam)
        {
            var segments = GetSegments(fhirParam);
            return string.Join("/", segments);
        }

        private static IEnumerable<string> GetSegments(IKey key)
        {
            if (key.Base != null) yield return key.Base;
            if (key.TypeName != null) yield return key.TypeName;
            if (key.ResourceId != null) yield return key.ResourceId;
            if (key.VersionId != null)
            {
                yield return "_history";
                yield return key.VersionId;
            }
        }

        public bool HasResourceId()
        {
            return this.ResourceId.IsNotEmpty();
        }

        public bool HasVersionId()
        {
            return this.VersionId.IsNotEmpty();
        }
    }
}
