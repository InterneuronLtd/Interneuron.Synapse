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


﻿

using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace SynapseStudio
{
    internal class DataServices
    {


        public static void executeSQLStatement(string sql, List<KeyValuePair<string, string>> parameters = null, SynapseHelpers.DBConnections dbConnection = SynapseHelpers.DBConnections.PGSQLConnection)
        {

            //Read Database Credentials from Web.config
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[dbConnection.ToString()].ConnectionString);

            using (con)
            {
                con.Open();

                // Insert some data
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = sql;
                    try
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    catch { }


                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (NpgsqlException ex)
                    {

                        string errorId = System.Guid.NewGuid().ToString();
                        string stacktrace = ex.StackTrace.ToString() ?? "";
                        string message = ex.Message.ToString() ?? "";
                        string source = ex.Source.ToString() ?? "";
                        string where = ((Npgsql.PostgresException)ex).Where ?? ""; // "Where"; // ex..ToString() ?? "";


                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<br />" + errorId);
                        sb.AppendLine("<hr />");
                        sb.AppendLine("Message: <br />" + message);
                        sb.AppendLine("<hr />");
                        sb.AppendLine("Statement: <br />" + where);
                        sb.AppendLine("<hr />");
                        sb.AppendLine("Stack Trace: <br />" + stacktrace);
                        LogSQLError(sql, stacktrace, message, source, errorId, where);
                        throw new Exception(sb.ToString());
                    }


                }


            }
        }

        private static void LogSQLError(string sql, string stacktrace, string message, string source, string errorlog_id, string ex_where, SynapseHelpers.DBConnections dbConnection = SynapseHelpers.DBConnections.PGSQLConnection)
        {
            string sqlInsert = "INSERT INTO log.errorlog (errorlog_sql, stacktrace, errorlog_message, errorlog_source, errorlog_id, ex_where) VALUES (@errorlog_sql, @stacktrace, @errorlog_message, @errorlog_source, @errorlog_id, @ex_where)";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("errorlog_sql", sql),
                new KeyValuePair<string, string>("stacktrace", stacktrace),
                new KeyValuePair<string, string>("errorlog_message", message),
                new KeyValuePair<string, string>("errorlog_source", source),
                new KeyValuePair<string, string>("errorlog_id", errorlog_id),
                new KeyValuePair<string, string>("ex_where", ex_where)
            };

            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[dbConnection.ToString()].ConnectionString);

            using (con)
            {
                con.Open();

                // Insert some data
                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = sqlInsert;
                    try
                    {
                        foreach (var param in paramList)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    catch (Exception ex)
                    {

                        //LogError("executeSQLStatement", sql, parameters.ToString(), ex.Message.ToString(), ex.InnerException.ToString(), ex.Source.ToString());

                    }
                    cmd.ExecuteNonQuery();


                }


            }
        }

        public static DataSet DataSetFromSQL(string sqlQueryString, List<KeyValuePair<string, string>> parameters = null, SynapseHelpers.DBConnections dbConnection = SynapseHelpers.DBConnections.PGSQLConnection)
        {
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[dbConnection.ToString()].ConnectionString);
            DataSet ds = new DataSet();

            using (con)
            {
                NpgsqlCommand cmd = new NpgsqlCommand();
                NpgsqlDataAdapter da = new NpgsqlDataAdapter();
                da.SelectCommand = cmd;
                cmd.CommandText = sqlQueryString;
                try
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                catch { }
                da.SelectCommand.Connection = con;

                try
                {
                    da.Fill(ds);
                }
                catch (Exception ex)
                {

                    string errorId = System.Guid.NewGuid().ToString();
                    string stacktrace = ex.StackTrace.ToString() ?? "";
                    string message = ex.Message.ToString() ?? "";
                    string source = ex.Source.ToString() ?? "";
                    string where = "";
                    try
                    {
                        where = ((Npgsql.PostgresException)ex).Where ?? ""; // "Where"; // ex..ToString() ?? "";
                    }
                    catch { }

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<br />" + errorId);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Message: <br />" + message);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Statement: <br />" + where);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Stack Trace: <br />" + stacktrace);
                    LogSQLError(sqlQueryString, stacktrace, message, source, errorId, where);
                    throw new Exception(sb.ToString());
                }


            }

            return ds;


        }

        public static String ExcecuteNonQueryFromSQL(string sqlQueryString, List<KeyValuePair<string, string>> parameters = null, SynapseHelpers.DBConnections dbConnection = SynapseHelpers.DBConnections.PGSQLConnection)
        {
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[dbConnection.ToString()].ConnectionString);

            string retVal = "";

            using (con)
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQueryString, con);
                try
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value == null ? DBNull.Value : (object)param.Value);
                    }
                }
                catch { }

                con.Open();


                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    string errorId = System.Guid.NewGuid().ToString();
                    string stacktrace = ex.StackTrace.ToString() ?? "";
                    string message = ex.Message.ToString() ?? "";
                    string source = ex.Source.ToString() ?? "";
                    string where = "";
                    try
                    {
                        where = ((Npgsql.PostgresException)ex).Where ?? ""; // "Where"; // ex..ToString() ?? "";
                    }
                    catch (Exception exe)
                    {
                    }


                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<br />" + errorId);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Message: <br />" + message);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Statement: <br />" + where);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Stack Trace: <br />" + stacktrace);
                    LogSQLError(sqlQueryString, stacktrace, message, source, errorId, where);
                    throw new Exception(sb.ToString());
                }



            }

            return retVal;

        }


        public static String StringFromSQL(string sqlQueryString, string valToRead, List<KeyValuePair<string, string>> parameters = null, SynapseHelpers.DBConnections dbConnection = SynapseHelpers.DBConnections.PGSQLConnection)
        {
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[dbConnection.ToString()].ConnectionString);

            string retVal = "";

            using (con)
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQueryString, con);
                try
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                catch { }

                con.Open();


                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                    string errorId = System.Guid.NewGuid().ToString();
                    string stacktrace = ex.StackTrace.ToString() ?? "";
                    string message = ex.Message.ToString() ?? "";
                    string source = ex.Source.ToString() ?? "";
                    string where = ((Npgsql.PostgresException)ex).Where ?? ""; // "Where"; // ex..ToString() ?? "";


                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<br />" + errorId);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Message: <br />" + message);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Statement: <br />" + where);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Stack Trace: <br />" + stacktrace);
                    LogSQLError(sqlQueryString, stacktrace, message, source, errorId, where);
                    throw new Exception(sb.ToString());
                }


                try
                {
                    retVal = (string)cmd.Parameters["_entityid"].Value.ToString();
                }
                catch { }

                int i = 0;

                foreach (var p in cmd.Parameters)
                {
                    var a = p.ToString();
                    i++;
                }

                int b = i;
            }

            return retVal;

        }

        public static string ExecuteScalar(string sqlQueryString, List<KeyValuePair<string, string>> parameters = null, SynapseHelpers.DBConnections dbConnection = SynapseHelpers.DBConnections.PGSQLConnection)
        {
            NpgsqlConnection con = new NpgsqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[dbConnection.ToString()].ConnectionString);

            string retVal = "";

            using (con)
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQueryString, con);
                try
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                catch { }

                con.Open();


                try
                {
                    retVal = Convert.ToString(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {

                    string errorId = System.Guid.NewGuid().ToString();
                    string stacktrace = ex.StackTrace.ToString() ?? "";
                    string message = ex.Message.ToString() ?? "";
                    string source = ex.Source.ToString() ?? "";
                    string where = ((Npgsql.PostgresException)ex).Where ?? ""; // "Where"; // ex..ToString() ?? "";


                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("<br />" + errorId);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Message: <br />" + message);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Statement: <br />" + where);
                    sb.AppendLine("<hr />");
                    sb.AppendLine("Stack Trace: <br />" + stacktrace);
                    LogSQLError(sqlQueryString, stacktrace, message, source, errorId, where);
                    throw new Exception(sb.ToString());
                }
            }

            return retVal;

        }

    }
}