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

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseDynamicAPI.Services
{
    public class SynapseEntityHelperServices
    {
        public static string GetEntityAttributes(string synapsenamespace, string synapseentityname, string returnsystemattributes)
        {
            string sql = "SELECT entitysettings.getentityattributestring(@p_synapsenamespacename, @p_entityname, CAST(@p_returnsystemattributes AS INTEGER));";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_synapsenamespacename", synapsenamespace),
                     new KeyValuePair<string, object>("p_entityname", synapseentityname),
                     new KeyValuePair<string, object>("p_returnsystemattributes", returnsystemattributes)
                };

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                return dt.Rows[0][0].ToString();
            }
            catch(Exception ex)
            {
                return " * ";
            }

        }

        public static string GetBaseViewAttributes(string baseviewname)
        {
            string sql = "SELECT entitysettings.getentityattributestring(@p_baseviewname);";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_baseviewname", baseviewname)
                };

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                return " * ";
            }

        }


        public static DataTable GetEntityRelations(string synapsenamespace, string synapseentityname)
        {
            string sql = "SELECT entitysettings.getentityrelations(@p_synapsenamespacename, @p_entityname);";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_synapsenamespacename", synapsenamespace),
                     new KeyValuePair<string, object>("p_entityname", synapseentityname)                     
                };

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                return dt;
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException("Error:" + ex.InnerException.ToString());
            }

        }

        public static string GetEntityKeyAttribute(string synapsenamespace, string synapseentityname)
        {
            string sql = "SELECT entitysettings.getentitykeyattribute(@p_synapsenamespacename, @p_entityname);";
            var paramList = new List<KeyValuePair<string, object>>() {
                     new KeyValuePair<string, object>("p_synapsenamespacename", synapsenamespace),
                     new KeyValuePair<string, object>("p_entityname", synapseentityname)                     
                };

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
                DataTable dt = ds.Tables[0];
                return dt.Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                throw new System.InvalidOperationException("Error:" + ex.InnerException.ToString());
            }

        }
    }
}
