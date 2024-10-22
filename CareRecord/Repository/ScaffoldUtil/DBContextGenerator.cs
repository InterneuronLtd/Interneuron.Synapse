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
using Microsoft.EntityFrameworkCore.Scaffolding;
//using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Interneuron.CareRecord.Repository
{
    public class DBContextGenerator //: CSharpDbContextGenerator
    {
        public DBContextGenerator(
          [NotNull] IProviderConfigurationCodeGenerator providerConfigurationCodeGenerator,
          [NotNull] IAnnotationCodeGenerator annotationCodeGenerator)
          //[NotNull] ICSharpHelper cSharpHelper)
          // : base(providerConfigurationCodeGenerator, annotationCodeGenerator, cSharpHelper)
        {
        }
    }
}
