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
﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace Interneuron.Terminology.Repository
{
    public class EntityTypeGenerator //: CSharpEntityTypeGenerator
    {
        public EntityTypeGenerator(ICSharpHelper helper)
          //: base(helper)
        {
        }

        //public override string WriteCode(IEntityType type, string @namespace, bool useDataAnnotations)
        //{
        //    string code = base.WriteCode(type, @namespace, useDataAnnotations);

        //    string schema = type.GetSchema();

        //    var defaultSchema = type.Model.GetDefaultSchema();

        //    string oldValue = "public partial class " + type.Name;

        //    string str2 = type.Name;// new Regex("[0-9]+$", RegexOptions.Compiled).Replace(type.Name, "");

        //    string newValue = "public partial class " + str2 + " : Interneuron.Terminology.Infrastructure.Domain.EntityBase";

        //    if (string.Compare(schema, "local_formulary", true) == 0)
        //    {
        //        newValue = "public partial class " + str2 + " : Interneuron.Terminology.Infrastructure.Domain.EntityBase,  Interneuron.Terminology.Infrastructure.Domain.IAuditable";
        //    }

        //    //string newValue = "public partial class " + str2 + " : Interneuron.Terminology.Infrastructure.Domain.EntityBase";

        //    code = code.Replace(oldValue, newValue).Replace("namespace " + @namespace, "namespace Interneuron.Terminology.Model.DomainModels");

        //    return code;
        //}

    }
}