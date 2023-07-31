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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extensions.DependencyInjection;
using Interneuron.Web.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Interneuron.Terminology.API
{
    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        CreateHostBuilder(args).Build().Run();
    //    }

    //    public static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .ConfigureWebHostDefaults(webBuilder =>
    //            {
    //                webBuilder.UseStartup<Startup>();
    //            });
    //}

    public class Program
    {
        static readonly string Namespace = typeof(Program).Namespace;
        static readonly string AppName = Namespace;
        const string ProgramExceptionMsg = "Program terminated unexpectedly ({ApplicationContext})!";
        const string ProgramInitMsg = "Configuring web host ({ApplicationContext})...";
        const string ProgramStartMsg = "Starting web host ({ApplicationContext})...";

        public static int Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = new InterneuronSerilogLoggerService().CreateSerilogLogger(configuration, AppName);

            //CreateHostBuilder(args).Build().Run();
            try
            {
                Log.Information(ProgramInitMsg, AppName);
                var host = BuildWebHost(configuration, args);

                Log.Information(ProgramStartMsg, AppName);
                host.Run();

                return 0;
            }
            catch (System.Exception ex)
            {
                Log.Fatal(ex, ProgramExceptionMsg, AppName);
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

       
        public static IHost BuildWebHost(IConfiguration configuration, string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((c) => c.AddConfiguration(configuration))
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseIISIntegration();
                    webBuilder.ConfigureKestrel(opt => opt.Limits.KeepAliveTimeout = TimeSpan.FromHours(24));
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseSerilog();
                }).Build();

        private static IConfiguration GetConfiguration()
        {
            var environmentName = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
