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


using Autofac;
using Autofac.Extensions.DependencyInjection;
using Interneuron.CareRecord.API.AppCode.Filters;
using Interneuron.CareRecord.API.AppCode.Infrastructure;
using Interneuron.CareRecord.API.AppCode.Infrastructure.AutofacBootstrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Formatters;
using Interneuron.CareRecord.API.AppCode.Extensions;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Interneuron.Infrastructure.Web.Exceptions.Handlers;
using System;
using Serilog;
using Interneuron.Web.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using InterneuronAutonomic.API;

namespace Interneuron.CareRecord.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public static ILifetimeScope AutofacContainer { get; private set; }

        private CareRecordAPISettings careRecordSettings = null;

        public void ConfigureServices(IServiceCollection services)
        {
            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // If using IIS:
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            // Bind to CareRecordAPISettings settings from appSettings.json
            careRecordSettings = new CareRecordAPISettings();
            Configuration.Bind("CareRecordSettings", careRecordSettings);
            services.AddSingleton<CareRecordAPISettings>(careRecordSettings);

            services.AddAuthenticationToCareRecord(Configuration);

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new GlobalFilter());
            //});

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });

            services.AddAutoMapperToCareRecord();

            // AddFhir also calls AddMvcCore
            services.AddFhir(careRecordSettings);
            services.AddFhirFormatters();

            //services.AddMvcCore(options =>
            services.AddMvc(options =>
            {
                options.Filters.Add(new GlobalFilter());

                //Should un-comment this (below two lines) to enable it only for FHIR Parsers
                options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
                options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
            });

            services.AddHttpContextAccessor();

            string SynapseDynamicAPIURI = Configuration.GetSection("DynamicAPISettings").GetSection("uri").Value;

            //services.AddHttpClient();

            //services.AddHttpClient<DynamicAPIClient>(clientConfig =>
            //{
            //    clientConfig.BaseAddress = new Uri(SynapseDynamicAPIURI);
            //});

            services.AddControllers();

            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                //config.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });

            services.AddVersionedApiExplorer(
                options =>
                {
                    // note: the specified format code will format the version as "'v'major[.minor]"
                    options.GroupNameFormat = "'v'V";
                    options.SubstituteApiVersionInUrl = true;
                    //options.SubstitutionFormat = "'v'V";
                });

            services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());

            services.AddSwaggerToCareRecord(Configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacBootstrapModule(Configuration, careRecordSettings));
            //builder.RegisterModule(new AutoMapperBootstrapModule()); //option 2
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            app.UseSwaggerToCareRecord(Configuration, provider);

            app.UseInterneuronExceptionHandler(options =>
            {
                options.OnExceptionHandlingComplete = (ex, errorId) =>
                {
                    LogException(ex, errorId);
                };
            });

            app.UseCors("AllowAllHeaders");

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMiddleware<InterneuronSerilogLoggingMiddleware>();

            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private static void LogException(Exception ex, string errorId)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) || ex.Message.StartsWith("a network", StringComparison.InvariantCultureIgnoreCase))
                Log.Logger.ForContext("ErrorId", errorId).Fatal(ex, ex.Message);
            else
                Log.Logger.ForContext("ErrorId", errorId).Error(ex, ex.Message);
        }

    }
}
