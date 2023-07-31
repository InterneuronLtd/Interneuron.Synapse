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

using Interneuron.Common.Extensions;
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
        /// <summary>
        /// This will open the connection to the database and starts the transaction
        /// Use this connection and the transaction objects to perform the db operation in the same transaction
        /// </summary>
        /// <returns></returns>
        public static Tuple<NpgsqlConnection, NpgsqlTransaction> BeginDbTransaction()
        {
            string databaseName = "connectionString_SynapseDataStore";

            NpgsqlConnection conn = new NpgsqlConnection(Environment.GetEnvironmentVariable(databaseName));
            conn.Open();

            NpgsqlTransaction tran = conn.BeginTransaction();

            return new Tuple<NpgsqlConnection, NpgsqlTransaction>(conn, tran);
        }

        /// <summary>
        /// This function will commit and dispose the connection and transaction objects
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public static void CommitDbTransaction(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            transaction.Commit();
            connection.Close();

            transaction.Dispose();
            connection.Dispose();
        }

        /// <summary>
        /// This function will commit and dispose the connection and transaction objects
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        public static void RollbackDbTransaction(NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            transaction.Rollback();
            connection.Close();

            transaction.Dispose();
            connection.Dispose();
        }

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

        public static void executeSQLStatement(string sql, List<KeyValuePair<string, object>> parameters = null, string databaseName = "connectionString_SynapseDataStore", NpgsqlConnection existingCon = null)
        {
            NpgsqlConnection con = existingCon ?? new NpgsqlConnection(Environment.GetEnvironmentVariable(databaseName));

            //using (con)
            //{
            if (existingCon == null)
                con.Open();

            // Insert some data
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = con;
                cmd.CommandText = sql;

                if (parameters.IsCollectionValid())
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }
                cmd.ExecuteNonQuery();
            }
            //}

            if (existingCon == null)
            {
                con.Close();
                con.Dispose();
            }
        }

        public static DataSet DataSetFromSQL(string sqlQueryString, List<KeyValuePair<string, object>> parameters = null, string databaseName = "connectionString_SynapseDataStore", NpgsqlConnection existingCon = null)
        {
            NpgsqlConnection con = existingCon ?? new NpgsqlConnection(Environment.GetEnvironmentVariable(databaseName));
            DataSet ds = new DataSet();

            //using (con)
            //{

            if (existingCon == null)
                con.Open();

            NpgsqlCommand cmd = new NpgsqlCommand();

            NpgsqlDataAdapter da = new NpgsqlDataAdapter();

            da.SelectCommand = cmd;
            cmd.CommandText = sqlQueryString;

            if (parameters.IsCollectionValid())
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value);
                }
            }


            da.SelectCommand.Connection = con;
            da.Fill(ds);

            if (existingCon == null)
            {
                con.Close();
                if (cmd != null) cmd.Dispose();
                con.Dispose();
            }

            //}

            return ds;
        }

        public static String ExcecuteNonQueryFromSQL(string sqlQueryString, List<KeyValuePair<string, object>> parameters = null, string databaseName = "connectionString_SynapseDataStore")
        {
            NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable(databaseName));

            string retVal = "";

            using (con)
            {
                NpgsqlCommand cmd = new NpgsqlCommand(sqlQueryString, con);

                if (parameters.IsCollectionValid())
                {
                    foreach (var param in parameters)
                    {
                        cmd.Parameters.AddWithValue(param.Key, param.Value);
                    }
                }

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

        public static NpgsqlConnection GetPGLock(string lockId)
        {
            var getAdvisoryLockCmd = $"SELECT pg_try_advisory_lock(hashtext('{lockId}'))";

            NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable("connectionString_SynapseDataStore"));

            //NpgsqlConnection con = new NpgsqlConnection("Server=interneuron-ind-db-test.cjdliyabgdwt.ap-south-1.rds.amazonaws.com;User Id=Inddbtestadmin;Password=N3ur0n!inddbtest;Database=DEV_synapse;Port=5432;");
            //NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5433;User Id=postgres;Password=password;");

            try
            {
                con.Open();

                var commandQuery = new NpgsqlCommand(getAdvisoryLockCmd, con);
                var acquireResult = commandQuery.ExecuteScalar();
                if (acquireResult != null && bool.TryParse(acquireResult.ToString(), out var islockAcquired) && islockAcquired)
                {
                    return con;
                }
            }
            catch (Exception e)
            {
                if (con.State == ConnectionState.Open)
                {
                    ReleasePGLockAndCloseConnection(lockId, con);
                }
                LogSQLError(getAdvisoryLockCmd, e.StackTrace, e.Message, e.Source, System.Guid.NewGuid().ToString(), "getlock:" + lockId);
            }

            if (con.State == ConnectionState.Open)
            {
                con.Close();
                con.Dispose();

            }
            return null;
        }

        public static NpgsqlConnection GetPGLockWithWait(string lockId)
        {
            var getAdvisoryLockCmd = $"SELECT pg_advisory_lock(hashtext('{lockId}'))";

            NpgsqlConnection con = new NpgsqlConnection(Environment.GetEnvironmentVariable("connectionString_SynapseDataStore"));
            //NpgsqlConnection con = new NpgsqlConnection("Server=interneuron-ind-db-test.cjdliyabgdwt.ap-south-1.rds.amazonaws.com;User Id=Inddbtestadmin;Password=N3ur0n!inddbtest;Database=DEV_synapse;Port=5432;");
            // NpgsqlConnection con = new NpgsqlConnection("Server=localhost;Port=5433;User Id=postgres;Password=password;");

            try
            {
                con.Open();
                var commandQuery = new NpgsqlCommand(getAdvisoryLockCmd, con);
                commandQuery.ExecuteNonQuery();
                return con;

            }
            catch (Exception e)
            {
                if (con.State == ConnectionState.Open)
                {
                    ReleasePGLockAndCloseConnection(lockId, con);
                }
                LogSQLError(getAdvisoryLockCmd, e.StackTrace, e.Message, e.Source, System.Guid.NewGuid().ToString(), "getlock:" + lockId);
            }

            return null;
        }


        public static bool ReleasePGLockAndCloseConnection(string lockId, NpgsqlConnection con)
        {
            var releaseAdvisoryLockCmd = $"SELECT pg_advisory_unlock(hashtext('{lockId}'))";

            try
            {
                var commandQuery = new NpgsqlCommand(releaseAdvisoryLockCmd, con);
                var releaseResult = commandQuery.ExecuteScalar();
                con.Close();
                con.Dispose();
                if (releaseResult != null && bool.TryParse(releaseResult.ToString(), out var islockReleased) && islockReleased)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    con.Dispose();
                }
                LogSQLError(releaseAdvisoryLockCmd, e.StackTrace, e.Message, e.Source, System.Guid.NewGuid().ToString(), "releaselock:" + lockId);
            }

            return false;

        }

    }
}
