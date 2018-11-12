//Interneuron Synapse

//Copyright(C) 2018  Interneuron CIC

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

using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynapseDynamicAPI.Services
{
    public class DataServices
    {

        public static string ConvertDataTabletoJSONString(DataTable dt)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            var ret = "";
            try
            {
                ret = JsonConvert.SerializeObject(rows);
            }
            catch(Exception er)
            {
                throw new Exception("Error returning data table: " + er.ToString());
            }
            return ret;
        }

        public static string ConvertDataTabletoJSONObject(DataTable dt)
        {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            row = new Dictionary<string, object>();
            foreach (DataRow dr in dt.Rows)
            {                
                foreach (DataColumn col in dt.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }                
            }
            var ret = "";
            try
            {
                ret = JsonConvert.SerializeObject(row);
            }
            catch (Exception er)
            {
                throw new Exception("Error returning data table: " + er.ToString());
            }
            return ret;            
        }

        public static void executeSQLStatement(string sql, List<KeyValuePair<string, object>> parameters = null)
        {

            NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable("connectionString_SynapseDataStore"));

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
                        LogSQLError(sql, stacktrace, message, source, errorId, where);
                        throw new Exception(sb.ToString());
                        throw new System.InvalidOperationException("Error:" + ex.InnerException.ToString());
                    }


                }


            }


        }

        public static DataSet DataSetFromSQL(string sqlQueryString, List<KeyValuePair<string, object>> parameters = null)
        {
            NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable("connectionString_SynapseDataStore"));
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

            return ds;


        }

        public static String ExcecuteNonQueryFromSQL(string sqlQueryString, List<KeyValuePair<string, object>> parameters = null)
        {
            NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable("connectionString_SynapseDataStore"));

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
                    LogSQLError(sqlQueryString, stacktrace, message, source, errorId, where);
                    throw new Exception(sb.ToString());
                }

            }

            return retVal;

        }

        private static void LogSQLError(string sql, string stacktrace, string message, string source, string errorlog_id, string ex_where)
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

            NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable("connectionString_SynapseDataStore"));

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

    }


}
