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
using AutoMapper;
using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Infrastructure
{
    //public class AutoMapperBootstrapModule
    //{
    //    private IApplicationBuilder _app;
    //    private IServiceProvider _provider;

    //    public AutoMapperBootstrapModule(IApplicationBuilder app, IServiceProvider provider)
    //    {
    //        this._app = app;
    //        this._provider = provider;
    //    }

    //    public void ConfigureAutoMapper()
    //    {
    //        var config = new MapperConfiguration(cfg =>
    //        {
    //            var profiles = this._provider.GetService(typeof(List<Profile>)) as List<Profile>;

    //            if (profiles.IsCollectionValid())
    //                profiles.Each(cfg.AddProfile);
    //        });

    //        //new Mapper()(cfg =>
    //        //{
    //        //    cfg.AddProfile<MyApp.Model.ModelMappingProfile>();
    //        //    cfg.AddProfile<MyApp.Model.ApiMappingProfile>();
    //        //});
    //    }
    //}

    public class AutoMapperBootstrapModule : Autofac.Module
    {
        public AutoMapperBootstrapModule()
        {

        }

        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = GetAssembliesFromBaseDirectory();

            var config = new MapperConfiguration(cfg =>
            {
                // Add all profiles in current assembly
                cfg.AddMaps(assemblies);
            });
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

                //Load referenced assemblies
                foreach (var assemblyName in refFilteredAssembyNames)
                {
                    Assembly.Load(assemblyName);
                }
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies != null ? assemblies
                .Where(refAsm => refAsm.FullName.StartsWith("Interneuron.CareRecord")).ToArray() : null;

        }
    }

}