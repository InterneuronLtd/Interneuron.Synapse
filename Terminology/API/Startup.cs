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


using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using HealthChecks.UI.Client;
using Interneuron.Infrastructure.Web.Exceptions.Handlers;
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.API.AppCode.Filters;
using Interneuron.Terminology.API.AppCode.Infrastructure.AutofacBootstrap;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

namespace Interneuron.Terminology.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public static ILifetimeScope AutofacContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthenticationToTerminology(Configuration);

            // services.AddCustomFormatters();

            services.AddMvc(options =>
            {
                options.Filters.Add(new GlobalFilter());

                //Should un-comment this (below two lines) to enable it only for FHIR Parsers
                //options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
                //options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
            });

            //services.AddControllers();

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

            services.AddHttpContextAccessor();

            services.AddAutoMapperToTerminology();

            services.AddCachingToTerminology(Configuration);

            services.AddHealthChecks().AddCheck("self", () => HealthCheckResult.Healthy());

            services.AddSwaggerToTerminologyApp(Configuration);
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacBootstrapModule(Configuration));
            //builder.RegisterModule(new AutoMapperBootstrapModule()); //option 2
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        //public void Configure(IApplicationBuilder app)
        {
            AutofacContainer = app.ApplicationServices.GetAutofacRoot();

            app.UseSwaggerToTerminologyApp(Configuration, provider);

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

            app.UseHealthChecks("/liveness", new HealthCheckOptions
            {
                Predicate = r => r.Name.Contains("self")
            });

            app.UseHealthChecks("/hc", new HealthCheckOptions()
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            //This is required when hosted in IIS as in-proc - requestimeout in web.config does not work
            //Reference: https://www.seeleycoder.com/blog/asp-net-core-request-timeout-iis-in-process-mode
            app.UseMaximumRequestTimeout();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
        }

        private void LogException(Exception ex, string errorId)
        {
            if (ex.Message.StartsWith("cannot open database", StringComparison.InvariantCultureIgnoreCase) || ex.Message.StartsWith("a network", StringComparison.InvariantCultureIgnoreCase))
                Log.Logger.ForContext("ErrorId", errorId).Fatal(ex, ex.Message);
            else
                Log.Logger.ForContext("ErrorId", errorId).Error(ex, ex.Message);
        }
    }
}
