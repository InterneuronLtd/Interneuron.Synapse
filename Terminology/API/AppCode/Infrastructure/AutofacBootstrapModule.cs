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


﻿using Autofac;
using Interneuron.Terminology.Infrastructure;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Repository;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;

namespace Interneuron.Terminology.API.AppCode.Infrastructure.AutofacBootstrap
{
    public class AutofacBootstrapModule : Autofac.Module
    {
        private IConfiguration configuration;

        public AutofacBootstrapModule()
        {

        }
        public AutofacBootstrapModule(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(FormularyRepository<>))
               .As(typeof(IFormularyRepository<>))
               .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(TerminologyRepository<>))
               .As(typeof(ITerminologyRepository<>))
               .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReadOnlyRepository<>))
               .As(typeof(IReadOnlyRepository<>))
               .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>))
               .As(typeof(IRepository<>))
               .InstancePerLifetimeScope();

            var assemblies = GetAssembliesFromBaseDirectory();

            //builder.RegisterAssemblyTypes(assemblies)
            //       .Where(t => t.BaseType == typeof(Profile))
            //       .As<Profile>();

            // Add all the types as providing its own type as service + all implemented interfaces
            builder.RegisterAssemblyTypes(assemblies)
                   .AsSelf()
                   .AsImplementedInterfaces();

            // register all the modules in the assemblies scanned
            var exceptCurrentAssemblyModule = assemblies.Except(new[] { Assembly.GetExecutingAssembly() }).ToArray();
            builder.RegisterAssemblyModules(exceptCurrentAssemblyModule);

            builder.Register<APIRequestContext>(r =>
            {
                APIRequestContext.APIRequestContextProvider = r.Resolve<Core.APIRequestContextProvider>().CreateAPIContext;
                return APIRequestContext.CurrentContext;
            }).InstancePerLifetimeScope();
        }

        private static Assembly[] GetAssembliesFromBaseDirectory()
        {
            //Load Assemblies
            //Get All assemblies.
            var refAssembyNames = Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies();

            if (refAssembyNames != null)
            {
                var refFilteredTerminologyAssembyNames = refAssembyNames.Where(refAsm => refAsm.FullName.StartsWith("Interneuron.Terminology"));

                //Load referenced assemblies
                foreach (var assemblyName in refFilteredTerminologyAssembyNames)
                {
                    Assembly.Load(assemblyName);
                }

                var refFilteredDynamicAssembyNames = refAssembyNames.Where(refAsm => refAsm.FullName.StartsWith("SynapseDynamicAPIClient"));

                //Load referenced assemblies
                foreach (var assemblyName in refFilteredDynamicAssembyNames)
                {
                    Assembly.Load(assemblyName);
                }
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies != null ? assemblies
                .Where(refAsm => refAsm.FullName.StartsWith("Interneuron.Terminology") || refAsm.FullName.StartsWith("SynapseDynamicAPIClient")).ToArray() : null;

            //return Directory.EnumerateFiles(AppDomain.CurrentDomain.RelativeSearchPath, "*.dll")
            //         .Where(filename => Path.GetFileName(filename).StartsWith("Interneuron.Terminology.", StringComparison.OrdinalIgnoreCase))
            //         .Select(Assembly.LoadFrom)
            //         .Union(new[] { Assembly.GetExecutingAssembly() })
            //         .ToArray();

            //return BuildManager.GetReferencedAssemblies().Cast<Assembly>();
        }
    }
}
