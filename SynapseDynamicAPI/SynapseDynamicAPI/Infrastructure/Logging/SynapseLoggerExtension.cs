//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using System;

//namespace SynapseDynamicAPI.Infrastructure.Logging
//{
//    public static class SynapseLoggerExtension
//    {
//        public static void AddSynapseLogging(this IServiceCollection services, IConfiguration Configuration)
//        {
//            services.AddLogging(config =>
//            {
//                // clear the default configuration set during the webhost in program.cs
//                config.ClearProviders();

//                config.AddConfiguration(Configuration.GetSection("Logging"));
//                config.AddDebug();
//                config.AddEventSourceLogger();

//                // This has been added to improve the performance of the app
//                // Ref: https://weblog.west-wind.com/posts/2018/Dec/31/Dont-let-ASPNET-Core-Default-Console-Logging-Slow-your-App-down

//                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == EnvironmentName.Development)
//                {
//                    config.AddConsole();
//                }
//            });
//        }
//    }
//}
