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
﻿

using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace Synapse.STS
{
    public class Program
    {
        private static IConfiguration _configuration;
        private static string _connectionString;
        private static string _logLevel;
        private static string _ConfigFilePath;
        public static void Main(string[] args)
        {
            _configuration = LoadConfiguration(args);
            var host = BuildWebHost(args);

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Debug()
               .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
               .MinimumLevel.Override("System", LogEventLevel.Error)
               .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
               .Enrich.FromLogContext()
               .WriteTo.PostgreSQL(_connectionString, "sislog", needAutoCreateTable: true, restrictedToMinimumLevel: getLogLevel())
               .WriteTo.File(_ConfigFilePath, fileSizeLimitBytes: 83886080, rollingInterval: RollingInterval.Day, restrictedToMinimumLevel: getLogLevel())
               //.WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
               .CreateLogger();

            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            var hostBuilder = WebHost.CreateDefaultBuilder(args).ConfigureLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog();
            }).UseConfiguration(_configuration);


            _connectionString = hostBuilder.GetSetting("Settings:ConnectionString");
            _logLevel = hostBuilder.GetSetting("Settings:LogLevel");
            _ConfigFilePath = hostBuilder.GetSetting("Settings:LogFilePath");
            hostBuilder.UseStartup<Startup>();

            return hostBuilder.Build();
        }

        public static LogEventLevel getLogLevel()
        {
            LogEventLevel l = new LogEventLevel();
            if (_logLevel == null || _logLevel == string.Empty)
                l = LogEventLevel.Verbose;
            else
                switch (_logLevel.ToLower().Trim())
                {
                    case "verbose":
                        l = LogEventLevel.Verbose;
                        break;
                    case "warning":
                        l = LogEventLevel.Warning;
                        break;
                    case "error":
                        l = LogEventLevel.Error;
                        break;
                    case "information":
                        l = LogEventLevel.Information;
                        break;
                    case "debug":
                        l = LogEventLevel.Debug;
                        break;
                    case "fatal":
                        l = LogEventLevel.Fatal;
                        break;
                    default:
                        l = LogEventLevel.Verbose;
                        break;
                }
            return l;
        }

        static IConfiguration LoadConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            //.AddEnvironmentVariables("ASPNETCORE_");
            if (args != null)
            {
                builder.AddCommandLine(args);
            }
            return builder.Build();
        }

    }
}
