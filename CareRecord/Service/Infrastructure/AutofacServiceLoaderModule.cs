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
﻿using Autofac;
using Interneuron.CareRecord.Infrastructure.Domain;
using Interneuron.CareRecord.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Interneuron.CareRecord.Service.Infrastructure
{
    public class AutofacServiceLoaderModule : Autofac.Module
    {
        public AutofacServiceLoaderModule()
        {
           
        }

        protected override void Load(ContainerBuilder builder)
        {
            //builder.Register(c => new SynapseDBContext(configuration))
            //   .AsSelf()
            //   .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(ReadOnlyRepository<>))
               .As(typeof(IReadOnlyRepository<>))
               .InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(Repository<>))
               .As(typeof(IRepository<>))
               .InstancePerLifetimeScope();

            var assemblies = GetAssembliesFromBaseDirectory();

            // Add all the types as providing its own type as service + all implemented interfaces
            //Let it throw exception in case when no assembly
            builder.RegisterAssemblyTypes(assemblies)
                   .AsSelf()
                   .AsImplementedInterfaces();

            // register all the modules in the assemblies scanned
            var exceptCurrentAssemblyModule = assemblies.Except(new[] { typeof(AutofacServiceLoaderModule).Assembly }).ToArray();
            builder.RegisterAssemblyModules(exceptCurrentAssemblyModule);
        }

        private static Assembly[] GetAssembliesFromBaseDirectory()
        {
            //Load Assemblies
            //Get All assemblies.
            var refAssembyNames = Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies();

            if (refAssembyNames != null)
            {
                var refFilteredAssembyNames = refAssembyNames.Where(refAsm => refAsm.FullName.StartsWith("Interneuron.CareRecord"));

                return refFilteredAssembyNames
                .Select(Assembly.Load)
                .Union(new[] { Assembly.GetExecutingAssembly() })
                .ToArray();
            }

            return null;
        }
    }

}
