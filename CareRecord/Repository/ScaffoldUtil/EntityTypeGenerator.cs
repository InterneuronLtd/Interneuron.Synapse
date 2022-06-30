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
//using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace Interneuron.CareRecord.Repository
{
    public class EntityTypeGenerator //: CSharpEntityTypeGenerator
    {
        public EntityTypeGenerator()//ICSharpHelper helper)
          //: base(helper)
        {
        }

        /* 

        public override string WriteCode(IEntityType type, string @namespace, bool useDataAnnotations)
        {
            string code = base.WriteCode(type, @namespace, useDataAnnotations);

            string schema = type.GetSchema();

            var defaultSchema = type.Model.GetDefaultSchema();

            string oldValue = "public partial class " + type.Name;

            string str2 = type.Name;// new Regex("[0-9]+$", RegexOptions.Compiled).Replace(type.Name, "");

            string newValue = "public partial class " + schema + "_" + str2 + " : Interneuron.CareRecord.Infrastructure.Domain.EntityBase";

            code = code.Replace(oldValue, newValue).Replace("namespace " + @namespace, "namespace Interneuron.CareRecord.Model.DomainModels");

            code = GenerateConstructor(type, code);

            code = GenerateNavigationProperties(type, code);

            return code;
        }

        private string GenerateNavigationProperties([NotNull] IEntityType entityType, string code)
        {
            string schema = entityType.GetSchema();

            var defaultSchema = entityType.Model.GetDefaultSchema();

            List<INavigation> navigationList = entityType.GetNavigations().ToList<INavigation>();

            if (!navigationList.Any<INavigation>())
                return code;

            foreach (INavigation navigation in navigationList)
            {
                string name = navigation.GetTargetType().Name;

                if (navigation.IsCollection())
                {
                    //E.g. 
                    // public virtual ICollection<CorePersonOld> CorePersonOldGenderLkpNavigation { get; set; }

                    Regex entityRegex = new Regex("public virtual ICollection<.+> " + navigation.Name + " { get; set; }", RegexOptions.Compiled);

                    Match match = entityRegex.Match(code);

                    if (match.Success)
                    {
                        var entity = match.Value;

                        var entityNameRegex = new Regex("<.+>", RegexOptions.Compiled).Match(entity);

                        if (entityNameRegex.Success)
                        {
                            string entityTypeVal = entityNameRegex.Value.TrimStart('<').Trim('>');
                            //code = regex.Replace(code, "public virtual ICollection<" + schema + "_" + str2 + "> " + navigation.Name + " { get; set; }");
                            string newValue = "public virtual ICollection<" + schema + "_" + entityTypeVal + "> " + navigation.Name + " { get; set; }";
                            //code = regex.Replace(code, $"{schema}_{newTypeVal}");
                            code = entityRegex.Replace(code, newValue);
                        }
                    }

                    //Match match = new Regex("(?<=virtual ICollection<).*(?=" + navigation.Name + ")", RegexOptions.Compiled).Match(code);

                    //if (match.Success)
                    //{
                    //    string str = match.Value.TrimEnd('>');
                    //    string oldValue = "public virtual ICollection<" + str + "> " + navigation.Name + " { get; set; }";
                    //    string newValue = "public virtual ICollection<" + schema + "_" + str + "> " + navigation.Name + " { get; set; }";

                    //    code = code.Replace(oldValue, newValue);
                    //}
                }
                else
                {
                    //e.g         
                    //public virtual CoreLookup GenderLkpNavigation { get; set; }

                    //Match match = new Regex("(?<=virtual ).*(?=" + navigation.Name + ")", RegexOptions.Compiled).Match(code);

                    //if (match.Success)
                    //{
                    //    string str = match.Value;
                    //    string oldValue = "public virtual " + str + " " + navigation.Name + " { get; set; }";
                    //    string newValue = "public virtual " + schema + "_" + str + " " + navigation.Name + " { get; set; }";
                    //    code = code.Replace(oldValue, newValue);
                    //}

                    Regex entityRegex = new Regex("virtual.+" + navigation.Name, RegexOptions.Compiled);

                    Match match = entityRegex.Match(code);

                    if (match.Success)
                    {
                        var entityName = match.Value.Replace("virtual ", "").Replace($" {navigation.Name}", "");

                        string oldValue = "public virtual " + entityName + " " + navigation.Name + " { get; set; }";

                        string newValue = "public virtual " + schema + "_" + entityName + " " + navigation.Name + " { get; set; }";

                        code = code.Replace(oldValue, newValue);
                    }
                }
            }

            return code;
        }

        private string GenerateConstructor([NotNull] IEntityType entityType, string code)
        {
            string schema = entityType.GetSchema();

            var defaultSchema = entityType.Model.GetDefaultSchema();

            var collectionNavigations = entityType.GetNavigations().Where(n => n.IsCollection()).ToList();

            if (collectionNavigations.Count > 0)
            {
                string oldValue = $"public {entityType.Name}()";

                string newValue = $"public {schema}_{entityType.Name}()";

                code = code.Replace(oldValue, newValue);

                foreach (var navigation in collectionNavigations)
                {
                    //_sb.AppendLine($"{navigation.Name} = new HashSet<{navigation.TargetEntityType.Name}>();");
                    //e.g. CorePersonOldGenderLkpNavigation = new HashSet<CorePersonOld>();

                    //var oldHasSetValue = $"{navigation.Name} = new HashSet<{navigation.GetTargetType().Name}>();";
                    //var newHasSetValue = $"{navigation.Name} = new HashSet<{schema}_{navigation.GetTargetType().Name}>();";
                    //code = code.Replace(oldHasSetValue, newHasSetValue);

                    //CorePersonOldGenderLkpNavigation = new HashSet<CorePersonOld>();
                    Regex entityRegex = new Regex(navigation.Name + @" = new HashSet<.+>\(\)", RegexOptions.Compiled);

                    Match match = entityRegex.Match(code);

                    if (match.Success)
                    {
                        var entity = match.Value;

                        var entityNameRegex = new Regex("<.+>", RegexOptions.Compiled).Match(entity);

                        if (entityNameRegex.Success)
                        {
                            string entityTypeVal = entityNameRegex.Value.TrimStart('<').TrimEnd('>');

                            string newHasSetValue = navigation.Name + " = new HashSet<" + schema + "_" + entityTypeVal + ">()";

                            code = entityRegex.Replace(code, newHasSetValue);
                        }
                    }
                }
            }

            return code;
        }

        */
    }
}