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




using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Serilog;
using System.IO;
using Interneuron.Web.Logging;

namespace SynapseDynamicAPI
{
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

        public static IWebHost BuildWebHost(IConfiguration configuration, string[] args) =>
            WebHost.CreateDefaultBuilder(args)
               .UseKestrel()
                .UseStartup<Startup>()
                .UseIISIntegration()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseConfiguration(configuration)
                .UseSerilog()
                .Build();


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
