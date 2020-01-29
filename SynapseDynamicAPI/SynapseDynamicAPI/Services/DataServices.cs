//Interneuron Synapse

//Copyright(C) 2019  Interneuron CIC

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
             
            return JsonConvert.SerializeObject(rows);
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
             
            return JsonConvert.SerializeObject(row);
        }

        public static void executeSQLStatement(string sql, List<KeyValuePair<string, object>> parameters = null, string databaseName = "connectionString_SynapseDataStore")
        {

            NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable(databaseName));

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
                    cmd.ExecuteNonQuery();
                     


                }


            }


        }

        public static DataSet DataSetFromSQL(string sqlQueryString, List<KeyValuePair<string, object>> parameters = null, string databaseName = "connectionString_SynapseDataStore")
        {
            NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable(databaseName));
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
                da.Fill(ds);
                 
            }

            return ds;


        }

        public static String ExcecuteNonQueryFromSQL(string sqlQueryString, List<KeyValuePair<string, object>> parameters = null, string databaseName = "connectionString_SynapseDataStore")
        {
            NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable(databaseName));

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
                cmd.ExecuteNonQuery();
                 

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

        public static string ExcecuteNonQuerySQL(string query, string returnColumn = null, List<KeyValuePair<string, object>> parameters = null, string databaseName = "connectionString_SynapseDataStore")
        {
            string retVal = string.Empty;

            if (!string.IsNullOrEmpty(returnColumn))
            {
                query = query + " RETURNING " + returnColumn;
            }

            using (NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable(databaseName)))
            {

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, con))
                {

                    if (parameters != null && parameters.Count > 0)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    con.Open();

                    using (NpgsqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            retVal = "{ \"Status\": \"Success\", \"Message\": \"\", \"Value\": \"" + Convert.ToString(dr[0]) + "\"}";
                        }
                        dr.Close();
                    }

                    con.Close();
                    
                }
            }
            return retVal;
        }
    }
}
