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


﻿using NpgsqlTypes;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.PostgreSQL;
using System.Collections.Generic;
using System;
using Serilog.Context;
using Serilog.Exceptions;
using System.Configuration;
using System.Collections.Specialized;

namespace Interneuron.Web.Logging
{
    /// <summary>
    /// This logger service is a  wrapper around the Serilog Logger framework for the easy integration in the 
    /// Interneuron applications
    /// </summary>
    public class InterneuronSerilogLoggerService
    {
        const string SerilogConfigRootName = "SeriLog/Logs";
        NameValueCollection serliLogConfigSection;

        /// <summary>
        /// This creates a wrapper around the Serilog Logger
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="appName"></param>
        /// <returns></returns>
        public Serilog.ILogger CreateSerilogLogger(string appName)
        {
            serliLogConfigSection = ConfigurationManager.GetSection(SerilogConfigRootName) as NameValueCollection;

            LoggerConfiguration loggerConfig = GetLoggerConfiguration(appName);

            bool.TryParse(serliLogConfigSection[$"EnableDBLogging"], out bool isPgLogEnabled);

            if (isPgLogEnabled)
            {
                ConfigureWithPgLogger(loggerConfig);
            }

            return loggerConfig.CreateLogger();

        }


        protected virtual void ConfigureWithPgLogger(LoggerConfiguration loggerConfig)
        {
            var columnWriters = GetColumnWriters();

            var connectionstring = serliLogConfigSection[$"PostgresLogging_Connectionstring"];

            var tableSchema = serliLogConfigSection[$"PostgresLogging_TableSchema"];

            var tableName = serliLogConfigSection[$"PostgresLogging_TableName"];

            loggerConfig.WriteTo.PostgreSQL(connectionstring,
                tableName,
                columnWriters,
                restrictedToMinimumLevel: GetLogEventLevel(serliLogConfigSection["Level"]),
                needAutoCreateTable: true,
                schemaName: tableSchema);
        }

        protected virtual LoggerConfiguration GetLoggerConfiguration(string appName)
        {
            //Can be filtered later
            //var duplicateErrorRemoveNullErrorExprn = "(@Level = 'Error' or @Level = 'Fatal') and (ErrorId is null or ErrorId = '')";

            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .Enrich.WithProperty("ApplicationContext", appName)
                .Enrich.WithProperty("AppVersion", serliLogConfigSection["API_Version"])
                .Enrich.WithExceptionDetails()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName();
            //.Filter.ByExcluding(duplicateErrorRemoveNullErrorExprn);//Can be filtered later

        }

        protected virtual Dictionary<string, ColumnWriterBase> GetColumnWriters()
        {
            //Used columns (Key is a column name) 
            //Column type is writer's constructor parameter
            return new Dictionary<string, ColumnWriterBase>
                {
                    {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
                    {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
                    {"level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                    {"raise_date", new TimestampColumnWriter(NpgsqlDbType.Timestamp) },
                    {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
                    {"properties", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
                    {"props_test", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) },
                    {"request_id", new SinglePropertyColumnWriter("RequestId", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l" )},
                    {"machine_name", new SinglePropertyColumnWriter("MachineName", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l") },
                    {"error_id", new SinglePropertyColumnWriter("ErrorId", PropertyWriteMethod.ToString, NpgsqlDbType.Varchar, "l" )},
                    {"application_context", new SinglePropertyColumnWriter("ApplicationContext", PropertyWriteMethod.ToString, NpgsqlDbType.Varchar, "l" )},
                    {"app_version", new SinglePropertyColumnWriter("AppVersion", PropertyWriteMethod.ToString, NpgsqlDbType.Varchar, "l" )},
                    {"user_name", new SinglePropertyColumnWriter("UserName", PropertyWriteMethod.ToString, NpgsqlDbType.Varchar, "l" )},
                    {"client_details", new SinglePropertyColumnWriter("ClientDetails", PropertyWriteMethod.ToString, NpgsqlDbType.Text, "l" )}
                };
        }

        public static void PushProperty<T>(string name, T propValue, bool destructureObjects = false)
        {
            Log.Logger.ForContext(name, propValue);

            LogContext.PushProperty(name, propValue, destructureObjects);
        }

        public static LogEventLevel GetLogEventLevel(string logLevel)
        {
            if (Enum.TryParse<LogEventLevel>(logLevel, true, out LogEventLevel logEventLevel))
                return logEventLevel;
            return LogEventLevel.Information;
        }
    }
}
