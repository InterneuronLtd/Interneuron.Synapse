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
using Hl7.Fhir.Serialization;
using InterneuronAutonomic.API;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Interneuron.CareRecord.API.AppCode.Infrastructure.AutofacBootstrap
{
    public class AutofacBootstrapModule : Autofac.Module
    {
        private IConfiguration configuration;
        private CareRecordAPISettings careRecordSettings;

        public AutofacBootstrapModule()
        {

        }
        public AutofacBootstrapModule(IConfiguration configuration, CareRecordAPISettings careRecordSettings)
        {
            this.configuration = configuration;
            this.careRecordSettings = careRecordSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = GetAssembliesFromBaseDirectory();

            builder.RegisterType<ReadQueryHandler>().Keyed<IQueryHandler>("Read");

            builder.Register(a => new HttpClient()).As<HttpClient>().InstancePerDependency();

            builder.Register(a =>
            {
                var settings = this.careRecordSettings; // a.Resolve<CareRecordAPISettings>();//GetSparkSettingsInstance(a);// 
                return new FhirJsonParser(settings.ParserSettings);
            }).AsSelf().InstancePerLifetimeScope();

            builder.Register(a =>
            {
                var settings = this.careRecordSettings; // a.Resolve<CareRecordAPISettings>();//GetSparkSettingsInstance(a);//
                return new FhirXmlParser(settings.ParserSettings);
            }).AsSelf().InstancePerLifetimeScope();

            builder.Register(a =>
            {
                var settings = this.careRecordSettings; // a.Resolve<CareRecordAPISettings>();// GetSparkSettingsInstance(a);// 
                return new FhirJsonSerializer(settings.SerializerSettings);
            }).AsSelf().InstancePerLifetimeScope();

            builder.Register(a =>
            {
                var settings = this.careRecordSettings; // a.Resolve<CareRecordAPISettings>();// GetSparkSettingsInstance(a);// 
                return new FhirXmlSerializer(settings.SerializerSettings);
            }).AsSelf().InstancePerLifetimeScope();

            //Not working should re-look
            builder.Register<CareRecordAPISettings>(a =>
            {
                CareRecordAPISettings settings = new CareRecordAPISettings();
                var configuration = a.Resolve<IConfiguration>();
                configuration.Bind("CareRecordSettings", settings);
                return settings;
            });

            builder.Register(a => this.careRecordSettings).AsSelf().SingleInstance();

            //builder.RegisterGeneric(typeof(SynapseResourceService<>))
            //   .As(typeof(ISynapseResourceService<>))
            //   .InstancePerLifetimeScope();

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
        }

        //private static CareRecordAPISettings GetSparkSettingsInstance(IComponentContext a)
        //{
        //    var settings = a.Resolve<CareRecordAPISettings>();

        //    if (settings.IsNull() || settings.Endpoint.IsNull())
        //    {
        //        var configuration = a.Resolve<IConfiguration>();
        //        configuration.Bind("CareRecordSettings", settings);
        //    }

        //    return settings;
        //}

        private static Assembly[] GetAssembliesFromBaseDirectory()
        {
            //Load Assemblies
            //Get All assemblies.
            var refAssembyNames = Assembly.GetExecutingAssembly()
                .GetReferencedAssemblies();

            if (refAssembyNames != null)
            {
                var refFilteredCareRecordAssembyNames = refAssembyNames.Where(refAsm => refAsm.FullName.StartsWith("Interneuron.CareRecord"));

                //Load referenced assemblies
                foreach (var assemblyName in refFilteredCareRecordAssembyNames)
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
                .Where(refAsm => refAsm.FullName.StartsWith("Interneuron.CareRecord") || refAsm.FullName.StartsWith("SynapseDynamicAPIClient")).ToArray() : null;

            //return Directory.EnumerateFiles(AppDomain.CurrentDomain.RelativeSearchPath, "*.dll")
            //         .Where(filename => Path.GetFileName(filename).StartsWith("Interneuron.CareRecord.", StringComparison.OrdinalIgnoreCase))
            //         .Select(Assembly.LoadFrom)
            //         .Union(new[] { Assembly.GetExecutingAssembly() })
            //         .ToArray();

            //return BuildManager.GetReferencedAssemblies().Cast<Assembly>();
        }
    }
}
