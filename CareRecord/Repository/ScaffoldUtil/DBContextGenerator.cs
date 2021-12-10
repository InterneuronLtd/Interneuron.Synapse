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


﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Interneuron.CareRecord.Repository
{
    public class DBContextGenerator : CSharpDbContextGenerator
    {
        public DBContextGenerator(
          [NotNull] IProviderConfigurationCodeGenerator providerConfigurationCodeGenerator,
          [NotNull] IAnnotationCodeGenerator annotationCodeGenerator,
          [NotNull] ICSharpHelper cSharpHelper)
          : base(providerConfigurationCodeGenerator, annotationCodeGenerator, cSharpHelper)
        {
        }

        public override string WriteCode(
          IModel model,
          string contextName,
          string connectionString,
          string contextNamespace,
          string modelNamespace,
          bool useDataAnnotations,
          bool suppressConnectionStringWarning)
        {
            string code = base.WriteCode(model, contextName, connectionString, contextNamespace, modelNamespace, useDataAnnotations, suppressConnectionStringWarning).Replace("using Microsoft.EntityFrameworkCore;", "using Microsoft.EntityFrameworkCore;\nusing Interneuron.CareRecord.Model.DomainModels;");

            foreach (IEntityType entityType in model.GetEntityTypes())
            {
                string schema = entityType.GetSchema();

                //Regex regex = new Regex("[0-9]+$", RegexOptions.Compiled);

                string str2 = entityType.Name;// regex.Replace(entityType.Name, "");

                string str3 = entityType.GetDbSetName(); // regex.Replace(entityType.GetDbSetName(), "");

                string oldValue1 = "public virtual DbSet<" + entityType.Name + "> " + entityType.GetDbSetName() + " { get; set; }";
                string newValue1 = "public virtual DbSet<" + schema + "_" + str2 + "> " + schema + "_" + str3 + " { get; set; }";

                code = code.Replace(oldValue1, newValue1);

                string oldValue2 = "modelBuilder.Entity<" + entityType.Name + ">";

                string newValue2 = "modelBuilder.Entity<" + schema + "_" + str2 + ">";

                code = code.Replace(oldValue2, newValue2);
            }
            return code;
        }
    }
}
