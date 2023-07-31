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
﻿using Newtonsoft.Json;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace SynapseStudioWeb.Helpers
{
    public class SynapseHelpers
    {
        public enum DBConnections
        {
            PGSQLConnection,
            PGSQLConnectionSIS,
            PGSQLConnectionPostgresDB,
            PGSQLConnectionMirthDB
        }

        //Entity Helpers
        public static string GetNamespaceNameFromID(string id)
        {
            string sql = "SELECT synapseNamespacename as retStr FROM entitysettings.synapseNamespace WHERE synapseNamespaceid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }
        public static string GetLocalNamespaceNameFromID(string id)
        {
            string sql = "SELECT localnamespacename as retStr FROM entitysettings.localnamespace WHERE localnamespaceid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }
        public static string GetAPIURL()
        {
            string sql = "SELECT apiurl AS retStr FROM  systemsettings.systemsetup;";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static string GetEntityNameFromID(string id)
        {
            string sql = "SELECT entityname as retStr FROM entitysettings.entitymanager WHERE entityid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static string GetNamepsaceFromEntityID(string id)
        {
            string sql = "SELECT synapseNamespacename as retStr FROM entitysettings.entitymanager WHERE entityid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }


        public static string GetKeyColumnForEntity(string id)
        {
            string sql = "SELECT keycolumn as retStr FROM entitysettings.entitymanager WHERE entityid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static string GetCurrentEntityVersionFromID(string id)
        {
            string sql = "SELECT entityversionid AS retStr FROM entitysettings.entityversion WHERE entityid = @id ORDER BY _sequenceid desc limit 1;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static string GetNextOrdinalPositionFromID(string id)
        {
            string sql = "SELECT max(ordinal_position)+ 1 AS retStr FROM entitysettings.entityattribute WHERE entityid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static DataTable GetEntityDSFromID(string id)
        {
            string sql = "SELECT * FROM entitysettings.entitymanager WHERE entityid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            return dt;

        }

        public static DataTable GetEntityKeyAttributeFromID(string id)
        {
            string sql = "SELECT attributeid, attributename FROM entitysettings.entityattribute WHERE iskeycolumn = 1 AND entityid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            return dt;

        }

        public static string GetEntityIDFromAttributeID(string id)
        {
            string sql = "SELECT entityid AS retStr FROM entitysettings.entityattribute WHERE attributeid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static string GetAttributeNameFromAttributeID(string id)
        {
            string sql = "SELECT attributename AS retStr FROM entitysettings.entityattribute WHERE attributeid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static DataTable GetEntitySampleJSON(string id)
        {
            string sql = "SELECT entitysettings.getjsonmodel(@id);";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            return dt;

        }

        public static string GetEntityNameAndNamespaceFromID(string id)
        {
            //string sql = "SELECT synapsenamespacename || '_' || case when length(coalesce(localnamespacename,'')) = 0 then '' else localnamespacename || '_' end || entityname as retStr FROM entitysettings.entitymanager WHERE entityid = @id;";
            string sql = "SELECT synapsenamespacename || '_' || entityname as retStr FROM entitysettings.entitymanager WHERE entityid = @id;";
            
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static string GetDataTypeFromID(string id)
        {
            string sql = "SELECT datatype as retStr FROM entitysettings.systemdatatype WHERE datatypeid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static string GetDataTypeDisplayFromID(string id)
        {
            string sql = "SELECT datatypedisplay as retStr FROM entitysettings.systemdatatype WHERE datatypeid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        //Baseview Helpers
        public static string GetBaseviewNamespaceNameFromID(string id)
        {
            string sql = "SELECT baseviewnamespace as retStr FROM listsettings.baseviewnamespace WHERE baseviewnamespaceid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }



        public static string GetBaseViewNameAndNamespaceFromID(string id)
        {
            string sql = "SELECT baseviewnamespace || '_'  || baseviewname as retStr FROM listsettings.baseviewmanager WHERE baseview_id = @id";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static DataTable GetBaseviewDTByID(string id)
        {
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseview_id = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            return dt;

        }


        public static string GetBaseviewNameFromID(string id)
        {
            string sql = "SELECT baseviewname as retStr FROM listsettings.baseviewmanager WHERE baseview_id = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }


        public static string GetBaseviewNameSpaceIDFromBaseViewID(string id)
        {
            string sql = "SELECT baseviewnamespaceid as retStr FROM listsettings.baseviewmanager WHERE baseview_id = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static string GetBaseviewNameSpaceNameFromBaseViewID(string id)
        {
            string sql = "SELECT baseviewnamespace as retStr FROM listsettings.baseviewmanager WHERE baseview_id = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static string GetBaseviewCommentsFromBaseViewID(string id)
        {
            string sql = "SELECT baseviewdescription as retStr FROM listsettings.baseviewmanager WHERE baseview_id = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }



        public static DataTable GetEntityBaseviewsDT(string id)
        {
            string sql = "SELECT * FROM entitysettings.v_entitydependentbaseviews WHERE entityid = @id ORDER BY source_schema, source_table;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            return dt;

        }


        //List Helpers
        public static string GetListNameFromID(string id)
        {
            string sql = "SELECT listname as retStr FROM  listsettings.listmanager WHERE list_id = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static string GetListNamespaceNameFromID(string id)
        {
            string sql = "SELECT listnamespace as retStr FROM listsettings.listnamespace WHERE listnamespaceid = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }


        public static DataTable GetListDT(string id)
        {
            string sql = "SELECT * FROM  listsettings.listmanager WHERE list_id = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            return dt;

        }

        public static string GetListNextOrdinalPositionFromID(string id)
        {
            string sql = "SELECT coalesce(max(ordinalposition),0) + 1 AS retStr FROM listsettings.listattribute WHERE list_id = @id;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("id", id)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }


        public static string GetEBoardURL()
        {
            string sql = "SELECT eboardurl AS retStr FROM eboards.setting LIMIT 1;";
            var paramList = new List<KeyValuePair<string, string>>()
            {
            };

            DataSet ds = DataServices.DataSetFromSQL(sql, paramList);
            DataTable dt = ds.Tables[0];

            string retStr = "";

            try
            {
                retStr = dt.Rows[0]["retStr"].ToString();
            }
            catch { }

            return retStr;

        }

        public static object DecodeJWTToken(string tokenString)
        {
            JwtSecurityTokenHandler j = new JwtSecurityTokenHandler();
            JwtSecurityToken jwttoken = new JwtSecurityToken();

            try
            {
                jwttoken = j.ReadJwtToken(tokenString);
            }
            catch
            {

                return null;
            }
            return jwttoken;
        }
        #region SchemaMigration
        public enum DataSetSerializerType { Json, Binary, Xml };
        public enum MigrationAction { New, Update, Delete, Skip };

        public static List<ImportStatusMessage> statusMessages = new List<ImportStatusMessage>();


        public static dynamic SerializeDataSet(object dataset, DataSetSerializerType t)
        {
            if (t.Equals(DataSetSerializerType.Json))
            {
                return JsonConvert.SerializeObject(dataset, Newtonsoft.Json.Formatting.Indented);
            }
            else if (t.Equals(DataSetSerializerType.Binary))
            {
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, dataset);
                return Convert.ToBase64String(ms.ToArray());
            }
            else if (t.Equals(DataSetSerializerType.Xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(DataSet));
                MemoryStream ms = new MemoryStream();
                serializer.Serialize(ms, dataset);
                return System.Text.ASCIIEncoding.ASCII.GetString(ms.ToArray());
            }
            else return null;
        }
        public static dynamic DeserializeDataSet(object serializedString, DataSetSerializerType t)
        {
            if (t.Equals(DataSetSerializerType.Json))
            {
                return JsonConvert.DeserializeObject(serializedString.ToString(), typeof(DataSet));
            }
            else if (t.Equals(DataSetSerializerType.Binary))
            {
                MemoryStream ms = new MemoryStream(Convert.FromBase64String(serializedString.ToString()));
                BinaryFormatter bf = new BinaryFormatter();
                return bf.Deserialize(ms);
            }
            else if (t.Equals(DataSetSerializerType.Xml))
            {
                StringReader sr = new StringReader(serializedString.ToString());
                DataSet ds = new DataSet();
                ds.ReadXml(sr);
                return ds;
            }
            else return null;
        }
        public static Export DeserialiseExport(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Export));
            using (TextReader reader = new StringReader(xml))
            {
                return (Export)serializer.Deserialize(reader);
            }
        }
        public static DataSet GetEntityAttributes(string entityids)
        {
            string sql = "SELECT * FROM entitysettings.entityattribute WHERE entityid in ('" + entityids + "') ORDER BY ordinal_position;";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", entityids)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "entitysettings.entityattribute";
            return ds;
        }
        public static DataSet GetEntityManager(string entityids)
        {
            string sql = "SELECT * FROM entitysettings.entitymanager WHERE entityid in ('" + entityids + "')";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", entityids)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "entitysettings.entitymanager";
            return ds;

        }
        public static DataSet GetEntityIDsByName(string entitynames)
        {
            string sql = "SELECT entityid FROM entitysettings.entitymanager WHERE entityname in ( '" + entitynames + "')";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entitynames", entitynames)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "entitysettings.entityids";
            return ds;

        }
        public static DataSet GetEntityRelation(string entityids)
        {
            string sql = "SELECT * FROM entitysettings.entityrelation WHERE entityid in ('" + entityids + "')";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", entityids)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "entitysettings.entityrelation";
            return ds;
        }
        public static DataSet GetEntityNamespace(string entityids)
        {
            string sql = "SELECT * FROM entitysettings.localnamespace WHERE localnamespaceid in " +
                            "(select localnamespaceid from entitysettings.entitymanager where entityid in ('" + entityids + "'))";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", entityids)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "entitysettings.entitynamespace";
            return ds;
        }
        public static DataSet GetEntitySystemNamespace(string entityids)
        {
            string sql = "SELECT * FROM entitysettings.synapsenamespace WHERE synapsenamespaceid in " +
                            "(select synapsenamespaceid from entitysettings.entitymanager where entityid in ('" + entityids + "'))";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("entityid", entityids)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "entitysettings.synapsenamespace";
            return ds;
        }
        public static DataSet GetBaseviewManager(string baseviewids)
        {
            string sql = "SELECT * FROM listsettings.baseviewmanager WHERE baseview_id in ( '" + baseviewids + "');";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseviewids", baseviewids)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "listsettings.baseviewmanager";
            return ds;

        }
        public static DataSet GetBaseviewNamespace(string baseviewids)
        {
            string sql = "SELECT * FROM listsettings.baseviewnamespace WHERE baseviewnamespaceid in " +
                            "(select baseviewnamespaceid from listsettings.baseviewmanager where baseview_id in ( '" + baseviewids + "'));";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("baseviewids", baseviewids)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "listsettings.baseviewnamespace";
            return ds;
        }

        public static DataSet GetListManager(string listids)
        {
            string sql = "SELECT * FROM  listsettings.listmanager WHERE list_id in ( '" + listids + "');";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listids",listids)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "listsettings.listmanager";

            return ds;

        }

        public static DataSet GetListAttribute(string listids)
        {
            string sql = "SELECT * FROM  listsettings.listattribute WHERE list_id in  ( '" + listids + "');";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listids", listids)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "listsettings.listattribute";

            return ds;

        }
        public static DataSet GetListAttributeStyleRule(string listids)
        {
            string sql = "SELECT * FROM  listsettings.listattributestylerule WHERE list_id in ( '" + listids + "');";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listids", listids)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "listsettings.listattributestylerule";

            return ds;

        }

        public static DataSet GetListBaseviewfilter(string listids)
        {
            string sql = "SELECT * FROM  listsettings.listbaseviewfilter WHERE list_id in ( '" + listids + "');";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listids", listids)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "listsettings.listbaseviewfilter";

            return ds;

        }
        public static DataSet GetListQuestion(string listids)
        {
            string sql = "SELECT * FROM  listsettings.listquestion WHERE list_id in ( '" + listids + "');";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listids", listids)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "listsettings.listquestion";

            return ds;

        }

        public static DataSet GetListQuestionResponse(string listids)
        {
            string sql = "SELECT * FROM  listsettings.listquestionresponse WHERE list_id in ( '" + listids + "');";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listids", listids)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "listsettings.listquestionresponse";
            ds.Tables[0].TableName = "listsettings.listquestionresponse";

            return ds;
        }

        public static DataSet GetListBaseviewParameter(string listids)
        {
            string sql = "SELECT * FROM  listsettings.listbaseviewparameter WHERE listbaseviewfilter_id in" +
                            " ( select listbaseviewfilter_id from listsettings.listbaseviewfilter WHERE list_id in ( '" + listids + "'));";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listids", listids)
            };

            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "listsettings.listbaseviewparameter";

            return ds;

        }

        public static DataSet GetListNamespace(string listids)
        {
            string sql = "SELECT * FROM listsettings.listnamespace WHERE listnamespaceid in " +
                            "(select listnamespaceid from listsettings.listmanager where list_id in ( '" + listids + "'));";
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("listids", listids)
            };
            DataSet ds = DataServices.DataSetFromSQL(sql);
            ds.Tables[0].TableName = "listsettings.listnamespace";
            return ds;
        }

        public static void CompareSchema(Export export, DataSetSerializerType st)
        {
            #region Entities
            DataSet dtDestinationEntitiesSchema = export.Entities.GetCurrentSchema();
            foreach (Entity entity in export.Entities.Entity)
            {
                //Check if this entity exists on this server
                DataTable entitymanager = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityManager, st)).Tables[0];
                string entityId = entity.Id;
                string entityName = entity.Name;
                string destEntityId = CheckIfEntityExists(entityName, entitymanager.Rows[0]["synapsenamespacename"].ToString());

                if (string.IsNullOrEmpty(destEntityId))  //Entity Doesn't exist, create new. 
                {
                    entity.action = MigrationAction.New;
                }
                else //Entity already exists compare components
                {
                    entity.action = MigrationAction.Update;
                    DataTable entityattribute = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityAttributes, st)).Tables[0];

                    DataTable entityrelation = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityRelation, st)).Tables[0];
                    //DataTable entitynamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.Namespace, st)).Tables[0];

                    DataSet dtSourceSchema = new DataSet();
                    dtSourceSchema.Tables.AddRange(new DataTable[] { entityattribute.Copy(), entitymanager.Copy(), entityrelation.Copy() });
                    foreach (DataTable dt in dtSourceSchema.Tables)
                    {
                        string keyColumn = dt.TableName == "entitysettings.entitymanager" ? "entityname" :
                                             dt.TableName == "entitysettings.entityattribute" ? "attributename"
                                                : dt.TableName == "entitysettings.entityrelation" ? "keycolumn" : string.Empty;

                        string idColumn = dt.TableName == "entitysettings.entitymanager" ? "entityid" :
                                             dt.TableName == "entitysettings.entityattribute" ? "attributeid"
                                                : dt.TableName == "entitysettings.entityrelation" ? "entityrelation_id" : string.Empty;

                        string contextIdColumn = dt.TableName == "entitysettings.entitymanager" ? "entityid" :
                                             dt.TableName == "entitysettings.entityattribute" ? "entityid"
                                                : dt.TableName == "entitysettings.entityrelation" ? "entityid" : string.Empty;

                        //filter sourceschema tables for this entity
                        DataTable dtDestinationSchemaFiltered = new DataTable()
                            ;

                        EnumerableRowCollection<DataRow> rows = dtDestinationEntitiesSchema.Tables[dt.TableName].AsEnumerable().Where(c => c.Field<string>(contextIdColumn) == destEntityId);

                        if (rows.Count() != 0)
                            dtDestinationSchemaFiltered = rows.CopyToDataTable();

                        if (dt.TableName == "entitysettings.entityrelation")
                        {
                            if (dt.Rows.Count != 0)
                                dt.Columns.Add("keycolumn", typeof(string), "parententityname+'/'+parentattributename");
                            if (rows.Count() != 0)
                                dtDestinationSchemaFiltered.Columns.Add("keycolumn", typeof(string), "parententityname+'/'+parentattributename");
                            else dtDestinationSchemaFiltered.Columns.Add("keycolumn", typeof(string));
                        }


                        entity.attributeActions.AddRange(CompareDataTables(keyColumn, idColumn, contextIdColumn, entityId, null, dt, dtDestinationSchemaFiltered));

                    }
                }
            }
            #endregion

            #region Baseviews
            // DataSet dtDestinationBaseviewSchema = export.Baseviews.GetCurrentSchema();

            foreach (Baseview baseview in export.Baseviews.Baseview)
            {

                DataTable baseviewmanager = ((DataSet)SynapseHelpers.DeserializeDataSet(baseview.BaseviewManager, export.format)).Tables[0];
                DataTable baseviewnamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(baseview.Namespace, export.format)).Tables[0];


                string baseviewid = baseview.Id;
                string baseviewname = baseview.Name;
                string baseviewnamespacename = baseviewnamespace.Rows[0]["baseviewnamespace"].ToString();

                string destBaseviewId = CheckIfBaseviewExists(baseviewname, baseviewnamespacename);
                if (string.IsNullOrEmpty(destBaseviewId))  //baseview Doesn't exist, create new. 
                {
                    baseview.action = MigrationAction.New;
                }
                else //baseview already exists compare components
                {
                    baseview.action = MigrationAction.Update;
                }
            }
            #endregion


            #region Baseviews
            //  DataSet dtDestinationListsSchema = export.SLists.GetCurrentSchema();

            foreach (SList list in export.SLists.SList)
            {

                DataTable listmanager = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListManager, export.format)).Tables[0];
                DataTable listnamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(list.Namespace, export.format)).Tables[0];

                DataTable listattribute = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListAttribute, export.format)).Tables[0];
                DataTable listattributestyle = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListAttributeStyleRule, export.format)).Tables[0];
                DataTable listbaseviewfilter = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListBaseviewFilter, export.format)).Tables[0];
                DataTable listbaseviewparameter = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListBaseviewParameter, export.format)).Tables[0];
                DataTable listquestion = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListQuestion, export.format)).Tables[0];
                DataTable listquestionresponse = ((DataSet)SynapseHelpers.DeserializeDataSet(list.listQuestionResponse, export.format)).Tables[0];



                string listid = list.Id;
                string listname = list.Name;
                string listnamespacename = listnamespace.Rows[0]["listnamespace"].ToString();

                string destListId = CheckIfListExists(listname, listnamespacename);
                if (string.IsNullOrEmpty(destListId))  //List Doesn't exist, create new. 
                {
                    list.action = MigrationAction.New;
                }
                else //List already exists compare components
                {
                    list.action = MigrationAction.Update;
                }
            }
            #endregion
        }


        /// <summary>
        /// this will be the tooltip
        /// </summary>
        /// <param name="keyColumn">column name that identfies the data in the table (e.g. attributename in entityattribute table)</param>
        /// <param name="idColumn">id for the key column (e.g. attributeId in entityattribute table)</param>
        public static List<Models.Action> CompareDataTables(string keyColumn, string idColumn, string contextIdColumn, string contextId, List<string> compareColumnList, DataTable dtSource, DataTable dtDestination)
        {
            List<Models.Action> ma = new List<Models.Action>();

            var notinDest = dtSource.AsEnumerable().Select(a => a.Field<string>(keyColumn))
                    .Except(dtDestination.AsEnumerable().Select(b => b.Field<string>(keyColumn))).ToList<string>();

            var notinSrc = dtDestination.AsEnumerable().Select(a => a.Field<string>(keyColumn))
                  .Except(dtSource.AsEnumerable().Select(b => b.Field<string>(keyColumn))).ToList<string>();

            var inBoth = dtSource.AsEnumerable().Select(a => a.Field<string>(keyColumn))
                 .Intersect(dtDestination.AsEnumerable().Select(b => b.Field<string>(keyColumn))).ToList<string>();


            DataTable dtNewColumns = dtSource.Clone();
            foreach (string item in notinDest)
            {
                dtNewColumns.ImportRow(dtSource.AsEnumerable().Where(x => x.Field<string>(keyColumn) == item).ToList<DataRow>()[0]);
            }

            //= (from row in dtSrouce.AsEnumerable()
            //                    join keycolumn in temp
            //                    on row.Field<string>("keycolumn") equals id
            //                    select row).CopyToDataTable();
            foreach (string newCol in notinDest) // new/create action object will have source ids for keyId and context
            {
                DataRow row = dtSource.AsEnumerable().Where(x => x.Field<string>(keyColumn) == newCol).ToList<DataRow>()[0];
                DataRow destrow = dtSource.AsEnumerable().First();
                ma.Add(new Models.Action(keyColumn, idColumn, row[keyColumn].ToString(), row[idColumn].ToString(), null, dtSource.TableName,
                                                    MigrationAction.New, true, contextIdColumn, row[contextIdColumn].ToString(), null));
            }

            foreach (string delCol in notinSrc) // delete action object will have destination ids for keyId and context
            {
                DataRow row = dtDestination.AsEnumerable().Where(x => x.Field<string>(keyColumn) == delCol).ToList<DataRow>()[0];
                ma.Add(new Models.Action(keyColumn, idColumn, row[keyColumn].ToString(), null, row[idColumn].ToString(), dtSource.TableName,
                                                    MigrationAction.Delete, true, contextIdColumn, contextId, row[contextIdColumn].ToString()));

                //ma.Add(new Models.Action(row[idColumn].ToString(), row[keyColumn].ToString(), dtDestination.TableName, MigrationAction.Delete, true, row[contextIdColumn].ToString(), contextIdColumn));
            }

            foreach (string updCol in inBoth) // update action object will have both source and destination ids for keyId and contextid
            {
                DataRow row = dtDestination.AsEnumerable().Where(x => x.Field<string>(keyColumn) == updCol).ToList<DataRow>()[0];
                DataRow srcRow = dtSource.AsEnumerable().Where(x => x.Field<string>(keyColumn) == updCol).ToList<DataRow>()[0];
                ma.Add(new Models.Action(keyColumn, idColumn, row[keyColumn].ToString(), row[idColumn].ToString(), row[idColumn].ToString(), dtSource.TableName,
                                                    MigrationAction.Update, true, contextIdColumn, srcRow[contextIdColumn].ToString(), row[contextIdColumn].ToString()));


                // ma.Add(new Models.Action(row[idColumn].ToString(), row[keyColumn].ToString(), dtDestination.TableName, MigrationAction.Update, true, row[contextIdColumn].ToString(), contextIdColumn));
            }

            return ma;
        }
        public static bool ComparDataTableRowValues(DataRow drSource, DataRow drDestination)
        {
            if (drSource.ItemArray.Count() != drDestination.ItemArray.Count()) return false;
            else
                foreach (DataColumn dc in drSource.ItemArray)
                {
                    if (drDestination[dc.ColumnName].ToString() != drSource[dc.ColumnName].ToString()) return false;
                }
            return true;
        }

        //        public static string ApplySchemaChanges(Entity entity, SynapseHelpers.DataSetSerializerType format)
        //        {
        //            string returnMessage = string.Empty;
        //            DataTable entityattribute = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityAttributes, format)).Tables[0];
        //            DataTable entitymanager = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityManager, format)).Tables[0];
        //            DataTable entityrelation = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityRelation, format)).Tables[0];
        //            DataTable entitynamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.Namespace, format)).Tables[0];
        //            DataTable synapsenamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.Namespace, format)).Tables[0];


        //            //check the migration action for this entity ( new / update / delete / skip } 
        //            if (entity.action == MigrationAction.New)
        //            {
        //                //create entity from model
        //                CreateEntity(entity, format);

        //            }
        //            else if (entity.action == MigrationAction.Update)
        //            {

        //                //foreach non system attribute check if user action is true and perform selected migration action 
        //                string localEntityId = CheckIfEntityExists(entity.Name, entitymanager.Rows[0]["synapsenamespacename"].ToString());

        //                //get all update actions for this entity
        //                List<Models.Action> updateActions = entity.attributeActions.Where(x => x.migrationAction == MigrationAction.Update && x.sourceContextId == entity.Id).ToList();
        //                List<Models.Action> deleteActions = entity.attributeActions.Where(x => x.migrationAction == MigrationAction.Delete && x.sourceContextId == entity.Id).ToList();
        //                List<Models.Action> createActions = entity.attributeActions.Where(x => x.migrationAction == MigrationAction.New && x.sourceContextId == entity.Id).ToList();

        //                foreach (Models.Action updateAction in updateActions)
        //                {
        //                    //no update actions are allowed to entities in the studio
        //                    //throw new NotImplementedException();
        //                }

        //                //perform delete operations
        //                foreach (Models.Action deleteAction in deleteActions)
        //                {
        //                    //skippping delete functionality now, below implementation is functional if uncommented. 

        //                    /*
        //                    if (deleteAction.dbTableName.Equals("entitysettings.entityattribute"))
        //                    {
        //                        DropEntityAttribute(deleteAction.destinationComponentId, deleteAction.destinationContextId);
        //                    }
        //                    else if (deleteAction.dbTableName.Equals("entitysettings.entityrelation"))
        //                    {
        //                        DropEntityRelation(deleteAction.destinationComponentId);
        //                    }

        //*/

        //                    //old implementaiton - obsolete. 
        //                    //        string sql = string.Format("delete from {0} where {1} = {2} and {3} = {4} and {5} = {6}", deleteAction.dbTableName,
        //                    //                                                                                    deleteAction.componentIdColumnName,
        //                    //                                                                                    "@destinationcomponentid",
        //                    //                                                                                     deleteAction.componentKeyColumnName,
        //                    //                                                                                      "@componentkeyvalue",
        //                    //                                                                                       deleteAction.contextColumnName,
        //                    //                                                                                        "@destinationcontextid");

        //                    //        var paramList = new List<KeyValuePair<string, string>>() {

        //                    //        new KeyValuePair<string, string>("destinationcomponentid",deleteAction.destinationComponentId),
        //                    //                                new KeyValuePair<string, string>("componentkeyvalue",deleteAction.componentKeyValue),
        //                    //                          new KeyValuePair<string, string>("destinationcontextid",deleteAction.destinationContextId),
        //                    //};

        //                    //        //todo error handling
        //                    //        DataServices.executeSQLStatement(sql, paramList);
        //                }

        //                //perform create operations

        //                //create new attributes from source
        //                foreach (Models.Action createAction in createActions)
        //                {
        //                    DataTable localsystemnamespace = GetEntitySystemNamespace(localEntityId).Tables[0];
        //                    if (createAction.dbTableName.Equals("entitysettings.entityattribute"))
        //                    {
        //                        DataTable dt = entityattribute.AsEnumerable().Where(x => x["attributeid"].ToString() == createAction.sourceComponentId).CopyToDataTable();

        //                        if (dt.Rows.Count != 0)
        //                        {
        //                            DataRow dr = dt.Rows[0];
        //                            if (dr["issystemattribute"].ToString() != "1")
        //                            {
        //                                //if not null success

        //                                CreateNewAttribute(localEntityId, dr["entityname"].ToString(), localsystemnamespace.Rows[0]["synapsenamespaceid"].ToString(), localsystemnamespace.Rows[0]["synapsenamespacename"].ToString(), "createdby_placeholder", dr["attributename"].ToString(), dr["attributedescription"].ToString(), dr["datatype"].ToString(), dr["datatypeid"].ToString(),
        //                                    dr["ordinal_position"].ToString(), dr["attributedefault"].ToString(), dr["maximumlength"].ToString(), dr["commondisplayname"].ToString(), dr["isnullsetting"].ToString());
        //                            }
        //                        }
        //                    }
        //                    else if (createAction.dbTableName.Equals("entitysettings.entityrelation"))
        //                    {
        //                        DataTable dt = entityrelation.AsEnumerable().Where(x => x["entityrelation_id"].ToString() == createAction.sourceComponentId).CopyToDataTable();
        //                        DataRow dr = dt.Rows[0];
        //                        DataSet dslocalparententity = GetEntityIDsByName(dr["parententityname"].ToString());
        //                        if (dslocalparententity.Tables.Count != 0 && dslocalparententity.Tables[0].Rows.Count != 0)
        //                        {


        //                            DataTable dtLocal = GetEntityManager(localEntityId).Tables[0];

        //                            string localattributeid = string.Empty;
        //                            string localattributename = string.Empty;
        //                            if (!string.IsNullOrWhiteSpace(dr["localentityattributename"].ToString()) && dr["localentityattributename"].ToString() != "No Local Attribute")
        //                            {
        //                                DataRow drLocalAttribute = GetEntityAttributes(localEntityId).Tables[0].Select("attributename = '" + dr["localentityattributename"].ToString() + "'")[0];
        //                                localattributename = drLocalAttribute["attributename"].ToString();
        //                                localattributeid = drLocalAttribute["attributeid"].ToString();
        //                            }
        //                            string localparentEntityId = dslocalparententity.Tables[0].Rows[0][0].ToString();
        //                            DataTable dtParent = GetEntityManager(localparentEntityId).Tables[0];

        //                            DataRow drParentAttribute;

        //                            if (!string.IsNullOrWhiteSpace(localattributename))

        //                                drParentAttribute = GetEntityAttributes(dtParent.Rows[0]["entityid"].ToString()).Tables[0].Select("attributename = '" + dr["parentattributename"].ToString().Replace("_" + localattributename, string.Empty) + "'")[0];
        //                            else
        //                                drParentAttribute = GetEntityAttributes(dtParent.Rows[0]["entityid"].ToString()).Tables[0].Select("attributename = '" + dr["parentattributename"].ToString() + "'")[0];


        //                            CreateRelation(localEntityId, dr["entityname"].ToString(), localsystemnamespace.Rows[0]["synapsenamespaceid"].ToString(),
        //                                            localsystemnamespace.Rows[0]["synapsenamespacename"].ToString(), dr["parentattributename"].ToString(), drParentAttribute["attributeid"].ToString(),
        //                                                dtParent.Rows[0]["entityid"].ToString(), dtParent.Rows[0]["entityname"].ToString(), dtParent.Rows[0]["synapsenamespaceid"].ToString(),
        //                                                 dtParent.Rows[0]["synapsenamespacename"].ToString(), "createdby_placeholder", localattributeid,
        //                                                   localattributename, dr["entityrelation_id"].ToString());
        //                        }
        //                    }
        //                }
        //                // UpdateBaseviewsForEntity(localEntityId);

        //                //TODO update list settings after entity change ? 
        //            }


        //            return "success";
        //        }
        //        public static string ApplySchemaChanges(Baseview b)
        //        {

        //            return null;
        //        }

        //        public static string ApplySchemaChanges(SList l)
        //        {

        //            return null;
        //        }

        public static void AddImportMsg(string msg, string type = "info")
        {
            statusMessages.Add(new ImportStatusMessage(DateTime.Now, msg, type, statusMessages.Count, false));
        }
        public static void ApplySchemaChanges(Export export)
        {
            //Iterate through source export
            //1.entity
            //2.baseview
            //3.Lists
            //4.bedboard
            //for each item, do action in tables. 

            statusMessages.Clear();
            //1. Entities
            AddImportMsg("Started Processing Entities...");
            foreach (Entity entity in export.Entities.Entity)
            {
                DataTable entityattribute = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityAttributes, export.format)).Tables[0];
                DataTable entitymanager = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityManager, export.format)).Tables[0];
                DataTable entityrelation = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityRelation, export.format)).Tables[0];
                DataTable entitynamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.Namespace, export.format)).Tables[0];
                DataTable synapsenamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.SystemNamespace, export.format)).Tables[0];


                //check the migration action for this entity ( new / update / delete / skip } 
                if (entity.action == MigrationAction.New)
                {
                    AddImportMsg("Creating New Entity: " + entity.Name);
                    //create entity from model
                    string newentityid = string.Empty;
                    try
                    {
                        newentityid = CreateEntity(entity, export.format);
                    }
                    catch (Exception ex) { }

                    try
                    {
                        Guid.Parse(newentityid);
                        AddImportMsg("Entity Added Successfully:" + entitymanager.Rows[0]["synapsenamespacename"].ToString() + (entitynamespace.Rows.Count > 0 ? "." + entitynamespace.Rows[0]["localnamespacename"].ToString() : "") + "." + entity.Name, "success");
                    }
                    catch (Exception e)
                    {
                        AddImportMsg("Error: Entity was not created:" + entitymanager.Rows[0]["synapsenamespacename"].ToString() + (entitynamespace.Rows.Count > 0 ? "." + entitynamespace.Rows[0]["localnamespacename"].ToString() : "") + "." + entity.Name, "error");
                    }

                }
                else if (entity.action == MigrationAction.Update)
                {
                    //foreach non system attribute check if user action is true and perform selected migration action 
                    string localEntityId = CheckIfEntityExists(entity.Name, entitymanager.Rows[0]["synapsenamespacename"].ToString());

                    //get all update actions for this entity
                    List<Models.Action> updateActions = entity.attributeActions.Where(x => x.migrationAction == MigrationAction.Update && x.sourceContextId == entity.Id).ToList();
                    List<Models.Action> deleteActions = entity.attributeActions.Where(x => x.migrationAction == MigrationAction.Delete && x.sourceContextId == entity.Id).ToList();
                    List<Models.Action> createActions = entity.attributeActions.Where(x => x.migrationAction == MigrationAction.New && x.sourceContextId == entity.Id).ToList();

                    if (createActions.Where(x => x.sourceContextId == entity.Id).Count() != 0)
                        AddImportMsg(string.Format("Updating entity: {0}", entity.Name));

                    #region update and delete -unimplemented
                    foreach (Models.Action updateAction in updateActions)
                    {
                        //no update actions are allowed to entities in the studio
                        //throw new NotImplementedException();
                    }

                    //perform delete operations
                    foreach (Models.Action deleteAction in deleteActions)
                    {
                        //skippping delete functionality now, below implementation is functional if uncommented. 

                        /*
                        if (deleteAction.dbTableName.Equals("entitysettings.entityattribute"))
                        {
                            DropEntityAttribute(deleteAction.destinationComponentId, deleteAction.destinationContextId);
                        }
                        else if (deleteAction.dbTableName.Equals("entitysettings.entityrelation"))
                        {
                            DropEntityRelation(deleteAction.destinationComponentId);
                        }

    */

                        //old implementaiton - obsolete. 
                        //        string sql = string.Format("delete from {0} where {1} = {2} and {3} = {4} and {5} = {6}", deleteAction.dbTableName,
                        //                                                                                    deleteAction.componentIdColumnName,
                        //                                                                                    "@destinationcomponentid",
                        //                                                                                     deleteAction.componentKeyColumnName,
                        //                                                                                      "@componentkeyvalue",
                        //                                                                                       deleteAction.contextColumnName,
                        //                                                                                        "@destinationcontextid");

                        //        var paramList = new List<KeyValuePair<string, string>>() {

                        //        new KeyValuePair<string, string>("destinationcomponentid",deleteAction.destinationComponentId),
                        //                                new KeyValuePair<string, string>("componentkeyvalue",deleteAction.componentKeyValue),
                        //                          new KeyValuePair<string, string>("destinationcontextid",deleteAction.destinationContextId),
                        //};

                        //        //todo error handling
                        //        DataServices.executeSQLStatement(sql, paramList);
                    }

                    #endregion
                    //perform create operations

                    //create new attributes from source
                    foreach (Models.Action createAction in createActions)
                    {


                        DataTable localsystemnamespace = GetEntitySystemNamespace(localEntityId).Tables[0];
                        if (createAction.dbTableName.Equals("entitysettings.entityattribute"))
                        {

                            DataTable dt = entityattribute.AsEnumerable().Where(x => x["attributeid"].ToString() == createAction.sourceComponentId).CopyToDataTable();

                            if (dt.Rows.Count != 0)
                            {
                                DataRow dr = dt.Rows[0];
                                if (dr["issystemattribute"].ToString() != "1")
                                {
                                    AddImportMsg(string.Format("Creating new attribute :{0}", dr["attributename"].ToString()));
                                    //if not null success

                                    string retval = CreateNewAttribute(localEntityId, dr["entityname"].ToString(), localsystemnamespace.Rows[0]["synapsenamespaceid"].ToString(), localsystemnamespace.Rows[0]["synapsenamespacename"].ToString(), "createdby_placeholder", dr["attributename"].ToString(), dr["attributedescription"].ToString(), dr["datatype"].ToString(), dr["datatypeid"].ToString(),
                                           dr["ordinal_position"].ToString(), dr["attributedefault"].ToString(), dr["maximumlength"].ToString(), dr["commondisplayname"].ToString(), dr["isnullsetting"].ToString());

                                    if (!string.IsNullOrEmpty(retval))
                                    {
                                        AddImportMsg(string.Format("Error: There was an Error creating new attribute {0}. Error: {1}", dr["attributename"].ToString(), retval), "error");
                                    }
                                    else
                                        AddImportMsg(string.Format("Attribute created successfully:{0}", dr["attributename"].ToString()), "success");
                                }
                            }
                        }
                        else if (createAction.dbTableName.Equals("entitysettings.entityrelation"))
                        {
                            DataTable dt = entityrelation.AsEnumerable().Where(x => x["entityrelation_id"].ToString() == createAction.sourceComponentId).CopyToDataTable();
                            DataRow dr = dt.Rows[0];
                            DataSet dslocalparententity = GetEntityIDsByName(dr["parententityname"].ToString());
                            AddImportMsg(string.Format("Creating new relation to parent entity :{0} and attribute {1}", dr["parententityname"].ToString(), dr["parentattributename"].ToString()));
                            if (dslocalparententity.Tables.Count != 0 && dslocalparententity.Tables[0].Rows.Count != 0)
                            {
                                DataTable dtLocal = GetEntityManager(localEntityId).Tables[0];

                                string localattributeid = string.Empty;
                                string localattributename = string.Empty;
                                if (!string.IsNullOrWhiteSpace(dr["localentityattributename"].ToString()) && dr["localentityattributename"].ToString() != "No Local Attribute")
                                {
                                    DataRow drLocalAttribute = GetEntityAttributes(localEntityId).Tables[0].Select("attributename = '" + dr["localentityattributename"].ToString() + "'")[0];
                                    localattributename = drLocalAttribute["attributename"].ToString();
                                    localattributeid = drLocalAttribute["attributeid"].ToString();
                                }
                                string localparentEntityId = dslocalparententity.Tables[0].Rows[0][0].ToString();
                                DataTable dtParent = GetEntityManager(localparentEntityId).Tables[0];

                                DataRow drParentAttribute;

                                if (!string.IsNullOrWhiteSpace(localattributename))

                                    drParentAttribute = GetEntityAttributes(dtParent.Rows[0]["entityid"].ToString()).Tables[0].Select("attributename = '" + dr["parentattributename"].ToString().Replace("_" + localattributename, string.Empty) + "'")[0];
                                else
                                    drParentAttribute = GetEntityAttributes(dtParent.Rows[0]["entityid"].ToString()).Tables[0].Select("attributename = '" + dr["parentattributename"].ToString() + "'")[0];


                                string retval = CreateRelation(localEntityId, dr["entityname"].ToString(), localsystemnamespace.Rows[0]["synapsenamespaceid"].ToString(),
                                                   localsystemnamespace.Rows[0]["synapsenamespacename"].ToString(), dr["parentattributename"].ToString(), drParentAttribute["attributeid"].ToString(),
                                                       dtParent.Rows[0]["entityid"].ToString(), dtParent.Rows[0]["entityname"].ToString(), dtParent.Rows[0]["synapsenamespaceid"].ToString(),
                                                        dtParent.Rows[0]["synapsenamespacename"].ToString(), "createdby_placeholder", localattributeid,
                                                          localattributename, dr["entityrelation_id"].ToString());
                                if (!string.IsNullOrEmpty(retval))
                                {
                                    AddImportMsg(string.Format("Error: There was an error creating relation to parent entity {0}. Attribute: {1} Error: {2}", dr["parententityname"].ToString(), dr["attributename"].ToString(), retval), "error");
                                }
                                else
                                {

                                    AddImportMsg("Relation created successfully", "success");
                                }
                            }
                            else
                            {
                                AddImportMsg(string.Format("Parent entity {0} doesn't exist to create relation", dr["parententityname"].ToString()), "error");
                            }
                        }
                    }
                    if (createActions.Count != 0)
                        UpdateBaseviewsForEntity(localEntityId);

                    //TODO update list settings after entity change ? 
                }
            }
            AddImportMsg("Completed Processing Entities");

            //2. Baseviews
            AddImportMsg("Started Processing Baseviews...");
            foreach (Baseview baseview in export.Baseviews.Baseview)
            {
                DataTable baseviewmanager = ((DataSet)SynapseHelpers.DeserializeDataSet(baseview.BaseviewManager, export.format)).Tables[0];
                DataTable baseviewnamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(baseview.Namespace, export.format)).Tables[0];

                string src_baseviewnamespace = baseviewnamespace.Rows[0]["baseviewnamespace"].ToString();
                string src_baseviewnamespaceid = baseviewnamespace.Rows[0]["baseviewnamespaceid"].ToString();
                string src_baseviewnamespacedescription = baseviewnamespace.Rows[0]["baseviewnamespacedescription"].ToString();
                string dest_baseviewnamespaceid = CheckIfBaseviewNamespaceExists(src_baseviewnamespace);


                //check if baseview-namespace exists      
                if (string.IsNullOrEmpty(dest_baseviewnamespaceid))
                {
                    AddImportMsg(string.Format("Destination baseviewnamespace : {0} doesn't exist.", src_baseviewnamespace), "warning");
                    AddImportMsg(string.Format("Creating baseviewnamespace : {0}", src_baseviewnamespace));
                    //create namespace
                    //TODO error handling. 
                    dest_baseviewnamespaceid = CreateBaseviewNameSpace(src_baseviewnamespaceid, src_baseviewnamespace, src_baseviewnamespacedescription, "createdby_placeholder");
                    try { Guid.Parse(dest_baseviewnamespaceid); }
                    catch
                    {
                        AddImportMsg(string.Format("Error: There was an error creating destination baseviewnamespace:{0}. Error: {1}", src_baseviewnamespace, dest_baseviewnamespaceid), "error");
                        AddImportMsg(string.Format("Error: Baseview {0} {1} skipped", baseview.Name, baseview.action.ToString()), "error");
                        continue;
                    }
                }

                if (baseview.action == MigrationAction.New)
                {
                    AddImportMsg(string.Format("Creating Baseview : {0}", baseview.Name));
                    //create Baseview
                    string baseviewId = CreateNewBaseView(dest_baseviewnamespaceid, src_baseviewnamespace, baseview.Name, baseviewmanager.Rows[0]["baseviewdescription"].ToString(),
                                                            baseviewmanager.Rows[0]["baseviewsqlstatement"].ToString(), "Createdby_placeholder", false);
                    try
                    {
                        Guid.Parse(baseviewId);

                        AddImportMsg(string.Format("Baseview : {0} created successfully", baseview.Name));
                    }
                    catch
                    {
                        AddImportMsg(string.Format("Error: There was an error creating new baseview:{0}. Error: {1}", baseview.Name, baseviewId), "error");
                    }
                }
                else if (baseview.action == MigrationAction.Update)
                {
                    AddImportMsg(string.Format("Updating Baseview : {0}", baseview.Name));

                    string destBaseviewId = CheckIfBaseviewExists(baseview.Name, src_baseviewnamespace);
                    //delete and recreate baseview. 
                    string baseviewId = CreateNewBaseView(dest_baseviewnamespaceid, src_baseviewnamespace, baseview.Name, baseviewmanager.Rows[0]["baseviewdescription"].ToString(),
                                                                               baseviewmanager.Rows[0]["baseviewsqlstatement"].ToString(), "Createdby_placeholder", true, destBaseviewId);

                    try { Guid.Parse(baseviewId); AddImportMsg(string.Format("Baseview : {0} updated successfully", baseview.Name), "success"); }
                    catch
                    {
                        AddImportMsg(string.Format("Error: There was an error updating baseview:{0}. Error: {1}", baseview.Name, baseviewId), "error");
                    }
                }
            }
            AddImportMsg("Completed Processing Baseviews");

            AddImportMsg("Started Processing Lists...");
            //3. Lists
            foreach (SList list in export.SLists.SList)
            {
                if (list.action != MigrationAction.Skip)
                {
                    AddImportMsg(string.Format("Processing List:{0} ", list.Name));
                    if (list.entity.EntityManager is null)
                    {
                        AddImportMsg(string.Format("Error: This list, {0},  does not have a relationship with any entity in the export, skipping create/update ", list.Name));
                        continue; // wont create a list without an entity
                    }
                    if (list.baseview.BaseviewManager is null)
                    {
                        AddImportMsg(string.Format("Error: This list, {0},  does not have a relationship with any baseview in the export, skipping create/update ", list.Name));
                        continue;// wont create a list without a baseview
                    }
                    DataTable listmanager = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListManager, export.format)).Tables[0];
                    DataTable listnamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(list.Namespace, export.format)).Tables[0];

                    DataTable listattribute = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListAttribute, export.format)).Tables[0];
                    DataTable listattributestyleRule = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListAttributeStyleRule, export.format)).Tables[0];
                    DataTable listbaseviewfilter = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListBaseviewFilter, export.format)).Tables[0];
                    DataTable listbaseviewparameter = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListBaseviewParameter, export.format)).Tables[0];
                    DataTable listquestion = ((DataSet)SynapseHelpers.DeserializeDataSet(list.ListQuestion, export.format)).Tables[0];
                    DataTable listquestionresponse = ((DataSet)SynapseHelpers.DeserializeDataSet(list.listQuestionResponse, export.format)).Tables[0];

                    DataTable listbaseviewmanager = ((DataSet)SynapseHelpers.DeserializeDataSet(list.baseview.BaseviewManager, export.format)).Tables[0];
                    DataTable listbaseviewnamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(list.baseview.Namespace, export.format)).Tables[0];
                    DataTable listentitymanager = ((DataSet)SynapseHelpers.DeserializeDataSet(list.entity.EntityManager, export.format)).Tables[0];

                    #region create dependencies
                    string src_listname = listmanager.Rows[0]["listname"].ToString();
                    string src_Listnamespace = listnamespace.Rows[0]["listnamespace"].ToString();
                    string src_Listnamespaceid = listnamespace.Rows[0]["listnamespaceid"].ToString();
                    string src_Listnamespacedescription = listnamespace.Rows[0]["listnamespacedescription"].ToString();
                    string dest_Listnamespaceid = CheckIfListNamespaceExists(src_Listnamespace);

                    //check if list-namespace exists  , create new if doesnt exist    
                    if (string.IsNullOrEmpty(dest_Listnamespaceid))
                    {
                        AddImportMsg(string.Format("Warning: List namespace {0},  does not exist", src_Listnamespace), "warning");
                        AddImportMsg(string.Format("Creating List namespace {0}", src_Listnamespace));
                        //create namespace
                        //TODO error handling. 
                        dest_Listnamespaceid = CreateListNameSpace(src_Listnamespaceid, src_Listnamespace, src_Listnamespacedescription, "createdby_placeholder");
                        try
                        {
                            Guid.Parse(dest_Listnamespaceid);
                            AddImportMsg(string.Format("List namespace {0} created successfully", src_Listnamespace), "success");
                        }
                        catch
                        {
                            AddImportMsg(string.Format("Error: There was an error creating List namespace {0}, list {1} creation will be skipped", src_Listnamespace, list.Name), "error");
                            continue;
                        }
                    }

                    //check if baseview exists 
                    string dest_baseviewid = CheckIfBaseviewExists(listbaseviewmanager.Rows[0]["baseviewname"].ToString(), listbaseviewmanager.Rows[0]["baseviewnamespace"].ToString());

                    if (string.IsNullOrEmpty(dest_baseviewid)) //create baseview
                    {
                        AddImportMsg(string.Format("Warning: List baseview {0},  does not exist", listbaseviewmanager.Rows[0]["baseviewname"].ToString()), "warning");
                        AddImportMsg(string.Format("Creating List baseview {0}", listbaseviewmanager.Rows[0]["baseviewname"].ToString()));

                        string src_baseviewnamespace = listbaseviewnamespace.Rows[0]["baseviewnamespace"].ToString();
                        string src_baseviewnamespaceid = listbaseviewnamespace.Rows[0]["baseviewnamespaceid"].ToString();
                        string src_baseviewnamespacedescription = listbaseviewnamespace.Rows[0]["baseviewnamespacedescription"].ToString();
                        string dest_baseviewnamespaceid = CheckIfBaseviewNamespaceExists(src_baseviewnamespace);

                        //check if baseview-namespace exists      
                        if (string.IsNullOrEmpty(dest_baseviewnamespaceid))
                        {
                            //create namespace
                            //TODO error handling. 
                            AddImportMsg(string.Format("Warning: List baseview namespace {0},  does not exist", src_baseviewnamespace), "warning");
                            AddImportMsg(string.Format("Creating List namespace {0}", src_baseviewnamespace));


                            dest_baseviewnamespaceid = CreateBaseviewNameSpace(src_baseviewnamespaceid, src_baseviewnamespace, src_baseviewnamespacedescription, "createdby_placeholder");

                            try
                            {
                                Guid.Parse(dest_baseviewnamespaceid);
                                AddImportMsg(string.Format("List baseview namespace {0} created successfully", src_baseviewnamespace), "success");
                            }
                            catch
                            {
                                AddImportMsg(string.Format("Error: There was an error creating List baseview namespace {0}, list {1} and baseview {2}. List creation will be skipped", src_baseviewnamespace, list.Name,
                                                                                                                                                                                    listbaseviewmanager.Rows[0]["baseviewname"].ToString()), "error");
                                continue;
                            }


                        }
                        dest_baseviewid = CreateNewBaseView(dest_baseviewnamespaceid, src_baseviewnamespace, listbaseviewmanager.Rows[0]["baseviewname"].ToString(), listbaseviewmanager.Rows[0]["baseviewdescription"].ToString(),
                                                              listbaseviewmanager.Rows[0]["baseviewsqlstatement"].ToString(), "Createdby_placeholder", false);

                        try
                        {
                            Guid.Parse(dest_baseviewid);
                            AddImportMsg(string.Format("List baseview {0} created successfully", listbaseviewmanager.Rows[0]["baseviewname"].ToString()), "success");
                        }
                        catch
                        {
                            AddImportMsg(string.Format("Error: There was an error creating List baseview {0} for list {1}. List creation will be skipped", listbaseviewmanager.Rows[0]["baseviewname"].ToString(), list.Name), "error");
                            continue;
                        }


                    }

                    //check if entity exists 
                    string entityName = listentitymanager.Rows[0]["entityname"].ToString();
                    string destEntityId = CheckIfEntityExists(entityName, listentitymanager.Rows[0]["synapsenamespacename"].ToString());

                    if (string.IsNullOrEmpty(destEntityId))  //Entity Doesn't exist, create new. 
                    {
                        AddImportMsg(string.Format("Warning: List entity {0},  does not exist", entityName), "warning");
                        AddImportMsg(string.Format("Creating List entity {0}", entityName));

                        destEntityId = CreateEntity(list.entity, export.format);

                        try
                        {
                            Guid.Parse(destEntityId);
                            AddImportMsg("List Entity Created Successfully:" + entityName, "success");
                        }
                        catch
                        {
                            AddImportMsg(string.Format("Error: There was an error creating List Entity {0}, list  {1} creation will be skipped", entityName, list.Name), "error");
                        }


                    }

                    #endregion
                    if (list.action == MigrationAction.New || list.action == MigrationAction.Update)
                    {
                        //create list
                        string src_ListId = listmanager.Rows[0]["list_id"].ToString();
                        string destListId = string.Empty;
                        if (list.action == MigrationAction.Update)
                        {
                            AddImportMsg(string.Format("Updating list {0}", list.Name));

                            destListId = CheckIfListExists(src_listname, src_Listnamespace);
                            AddImportMsg(string.Format("List already exists, deleting list: {0}", list.Name));
                            string returnvalue = DeleteList(destListId);
                            if (!string.IsNullOrEmpty(returnvalue))
                            {
                                AddImportMsg(string.Format("Error: There was an error deleting list. List update will be skipped"), "error");
                                continue;
                            }
                            AddImportMsg(string.Format("List deleted successfully"), "success");
                        }
                        if (string.IsNullOrEmpty(destListId))
                            destListId = src_ListId;


                        //TODO error handling
                        AddImportMsg(string.Format("Creating list: {0}", list.Name));
                        //create list manager entry
                        string retval = createListManagerEntry(destListId, listmanager.Rows[0]["listname"].ToString(), listmanager.Rows[0]["listdescription"].ToString(), string.Empty,
                                                     dest_baseviewid, dest_Listnamespaceid, src_Listnamespace, destEntityId, listmanager.Rows[0]["defaultcontextfield"].ToString(),
                                                      listmanager.Rows[0]["matchedcontextfield"].ToString(), listmanager.Rows[0]["patientbannerfield"].ToString(), listmanager.Rows[0]["rowcssfield"].ToString(),
                                                       listmanager.Rows[0]["tablecssstyle"].ToString(), listmanager.Rows[0]["tableheadercssstyle"].ToString(), listmanager.Rows[0]["defaultrowcssstyle"].ToString());

                        try { Guid.Parse(retval); }
                        catch (Exception ex)
                        {
                            AddImportMsg(string.Format("Error: There was an creating listmanager entry {0}. List creation will be skipped", retval), "error");
                            continue;
                        }

                        //create list attribure entry
                        foreach (DataRow row in listattribute.Rows)
                        {
                            AddImportMsg(string.Format("Creating list attribute {0}", row["attributename"].ToString()));

                            string sql = string.Format("select baseviewattribute_id from listsettings.baseviewattribute where baseview_id = '{0}' and attributename = '{1}'",
                                                            dest_baseviewid, row["attributename"].ToString());

                            string baseViewAttributeId = DataServices.ExecuteScalar(sql);

                            retval = createListAttributeEntry(row["listattribute_id"].ToString(), destListId, row["attributename"].ToString(), baseViewAttributeId, row["ordinalposition"].ToString(),
                                                         row["datatype"].ToString(), row["displayname"].ToString(), row["defaultcssclassname"].ToString(), row["isselected"].ToString());

                            try { Guid.Parse(retval); }
                            catch (Exception ex)
                            {
                                AddImportMsg(string.Format("Error creating list attribute {0}. Error: {1}", row["attributename"].ToString(), ex.Message), "error");
                            }
                        }
                        //listattributestyle
                        foreach (DataRow row in listattributestyleRule.Rows)
                        {
                            retval = createListAttributeStyleRule(row["listattributestylerule_id"].ToString(), row["listattribute_id"].ToString(), destListId, row["evaluationcomparator"].ToString(),
                                                                 row["expressionstatement"].ToString(), row["evaluationexpression"].ToString(), row["evaluationorder"].ToString(), row["styletoapply"].ToString());

                            try { Guid.Parse(retval); }
                            catch (Exception ex)
                            {
                                AddImportMsg(string.Format("Error creating list listattributestyleRule {0}. Error: {1}", row["styletoapply"].ToString(), ex.Message), "error");
                            }
                        }

                        //listbaseviewfilter
                        foreach (DataRow row in listbaseviewfilter.Rows)
                        {
                            retval = createListBaseViewFilter(row["listbaseviewfilter_id"].ToString(), destListId, row["filtertype"].ToString(), row["evaulationattribute"].ToString(),
                                                         row["evaluationcomparator"].ToString(), row["evaluationcompareattribute"].ToString(), row["evaluationexpression"].ToString(), row["sqlexpressionstatement"].ToString(),
                                                         row["filterexpressionstatement"].ToString(), row["evaluationorder"].ToString(), row["evaluationparameter"].ToString());


                            try { Guid.Parse(retval); }
                            catch (Exception ex)
                            {
                                AddImportMsg(string.Format("Error creating list listbaseviewfilter id {0}. Error: {1}", row["listbaseviewfilter_id"].ToString(), ex.Message), "error");
                            }

                        }

                        //listbaseviewparameter
                        foreach (DataRow row in listbaseviewparameter.Rows)
                        {
                            retval = createListBaseViewParameter(row["listbaseviewparameter"].ToString(), row["listbaseviewfilter_id"].ToString(), row["parametername"].ToString(), row["defaultvalue"].ToString());
                            try { Guid.Parse(retval); }
                            catch (Exception ex)
                            {
                                AddImportMsg(string.Format("Error creating list listbaseviewparameter  {0}. Error: {1}", row["listbaseviewparameter"].ToString(), ex.Message), "error");
                            }
                        }

                        //listquestion

                        foreach (DataRow row in listquestion.Rows)
                        {
                            createListQuestion(row["listquestion_id"].ToString(), destListId, row["question_id"].ToString(), row["ordinalposition"].ToString(), row["isselected"].ToString());
                        }
                        //listquestionresponse
                        foreach (DataRow row in listquestionresponse.Rows)
                        {
                            retval = createListQuestionResponse(row["listquestionresponse_id"].ToString(), row["question_id"].ToString(), destListId, row["listquestion_id"].ToString(),
                                                    row["defaultcontext"].ToString(), row["defaultcontextfieldname"].ToString(), row["contextvalue"].ToString(), row["responsetext"].ToString(),
                                row["responsedatetime"].ToString(), row["responsetextlong"].ToString(), row["optionorder"].ToString());

                            try { Guid.Parse(retval); }
                            catch (Exception ex)
                            {
                                AddImportMsg(string.Format("Error creating list listquestionresponse id {0}. Error: {1}", row["listquestionresponse_id"].ToString(), ex.Message), "error");
                            }
                        }
                        AddImportMsg(string.Format("Completed Processing List {0}", list.Name), "success");
                    }
                }
            }
            AddImportMsg("Completed Processing Lists");

            AddImportMsg("-------------------------Completed Processing Export-----------------------------");
        }
        private static int DropEntityRelation(string relationid)
        {
            string sql = @"delete from entitysettings.entityrelation where entityrelation_id = @p_relationid";

            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_relationid", relationid),
        };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            }
            catch { return -1; }

            return 0;

        }
        private static int DropEntityAttribute(string attributeid, string entityid)
        {
            DataTable dtBVs = SynapseHelpers.GetEntityBaseviewsDT(entityid);

            string sql = @"SELECT entitysettings.dropattributefromentity(
                                @p_attributeid,
	                            @p_entityid
                            )";



            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_attributeid", attributeid),
                new KeyValuePair<string, string>("p_entityid", entityid)
            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 0;

        }

        private static string DeleteList(string listid)
        {
            //delete question response

            string sql = @"delete from listsettings.listquestionresponse where list_id = @list_id";

            var paramList = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("list_id", listid) };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                AddImportMsg(string.Format("Error: There was an error deleting from listquestionresponse : {0}", ex.Message), "error");
                return ex.Message;
            }

            sql = @"delete from listsettings.listquestion where list_id = @list_id";

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                AddImportMsg(string.Format("Error: There was an error deleting from listquestion : {0}", ex.Message), "error");
                return ex.Message;
            }


            sql = @"delete from listsettings.listbaseviewparameter where listbaseviewfilter_id in 
                        (select listbaseviewfilter_id from listsettings.listbaseviewfilter where list_id = @list_id)";

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                AddImportMsg(string.Format("Error: There was an error deleting from listbaseviewparameter : {0}", ex.Message), "error");
                return ex.Message;
            }

            sql = @"delete from listsettings.listbaseviewfilter where list_id = @list_id";

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                AddImportMsg(string.Format("Error: There was an error deleting from listbaseviewfilter : {0}", ex.Message), "error");
                return ex.Message;
            }

            sql = @"delete from listsettings.listattributestylerule where list_id = @list_id";

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                AddImportMsg(string.Format("Error: There was an error deleting from listattributestylerule : {0}", ex.Message), "error");
                return ex.Message;
            }

            sql = @"delete from listsettings.listattribute where list_id = @list_id";

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                AddImportMsg(string.Format("Error: There was an error deleting from listattribute : {0}", ex.Message), "error");
                return ex.Message;
            }

            sql = @"delete from listsettings.listmanager where list_id = @list_id";

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                AddImportMsg(string.Format("Error: There was an error deleting from listmanager : {0}", ex.Message), "error");
                return ex.Message;
            }
            return string.Empty;

        }

        private static string createListQuestionResponse(string listquestionresponse_id, string question_id, string list_id, string listquestion_id,
                                                            string defaultcontext, string defaultcontextfieldname, string contextvalue, string responsetext, string responsedatetime,
                                                            string responsetextlong, string optionorder)
        {
            string sql = @"INSERT INTO listsettings.listquestionresponse(
           listquestionresponse_id, question_id, list_id, listquestion_id, 
            defaultcontext, defaultcontextfieldname, contextvalue, responsetext, 
            responsedatetime, responsetextlong, optionorder)
    VALUES ( @listquestionresponse_id, @question_id, @list_id, @listquestion_id, 
            @defaultcontext, @defaultcontextfieldname, @contextvalue, @responsetext, 
            @responsedatetime, @responsetextlong, @optionorder);";

            var paramList = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("listquestionresponse_id", listquestionresponse_id),
                    new KeyValuePair<string, string>("question_id", question_id),
                   new KeyValuePair<string, string>("list_id", list_id),
                    new KeyValuePair<string, string>("listquestion_id", listquestion_id),
                     new KeyValuePair<string, string>("defaultcontext", defaultcontext),

                new KeyValuePair<string, string>("defaultcontextfieldname", defaultcontextfieldname),
                   new KeyValuePair<string, string>("contextvalue", contextvalue),
                    new KeyValuePair<string, string>("responsetext", responsetext),
                     new KeyValuePair<string, string>("responsedatetime", responsedatetime),
                     new KeyValuePair<string, string>("responsetextlong", responsetextlong),
                     new KeyValuePair<string, string>("optionorder", optionorder)
                                 };
            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                return null;
            }
            return listquestion_id;

        }
        public static int GetNextListOrdinalPosition(string listId)
        {

            string sql = @"SELECT
                           max(la.ordinalposition) as MaxOrdinalPosition
                            FROM listsettings.listattribute la
                            WHERE la.list_id = @list_id
                            AND COALESCE(isselected, false) = true";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId)
            };

            DataSet ds = new DataSet();

            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return 1;
            }

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
            {
                return 1;
            }

            int ret = 0;
            try
            {
                ret = System.Convert.ToInt16(dt.Rows[0]["MaxOrdinalPosition"].ToString());
            }
            catch { }

            ret += 1;

            return ret;

        }
        private static string createListQuestion(string listquestion_id, string list_id, string question_id, string ordinalposition, string isselected)
        {

            string sql = @"INSERT INTO listsettings.listquestion(
            listquestion_id, list_id, question_id, ordinalposition, isselected)
    VALUES ( @listquestion_id, @list_id, @question_id, case when (@ordinalposition='null' or @ordinalposition='') then null else cast(@ordinalposition as int) end, cast(@isselected as bool));";

            try { int.Parse(ordinalposition); }
            catch
            { ordinalposition = string.Empty; }

            var paramList = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("listquestion_id", listquestion_id),
                    new KeyValuePair<string, string>("list_id", list_id),
                   new KeyValuePair<string, string>("question_id", question_id),
                    new KeyValuePair<string, string>("ordinalposition", ordinalposition),
                     new KeyValuePair<string, string>("isselected", isselected)
                                 };
            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return listquestion_id;

        }

        private static string createListBaseViewParameter(string listbaseviewparameter, string listbaseviewfilter_id, string parametername, string defaultvalue)
        {
            string sql = @"INSERT INTO listsettings.listbaseviewparameter(
            listbaseviewparameter, listbaseviewfilter_id,parametername, defaultvalue)
    VALUES(  @listbaseviewparameter, @listbaseviewfilter_id, @parametername, @defaultvalue);";

            var paramList = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("listbaseviewparameter", listbaseviewparameter),

                   new KeyValuePair<string, string>("listbaseviewfilter_id", listbaseviewfilter_id),
                    new KeyValuePair<string, string>("parametername", parametername),
                     new KeyValuePair<string, string>("defaultvalue", defaultvalue)
                                 };
            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return listbaseviewfilter_id;

        }

        private static string createListBaseViewFilter(string listbaseviewfilter_id, string list_id, string filtertype, string evaulationattribute, string evaluationcomparator,
                                                string evaluationcompareattribute, string evaluationexpression, string sqlexpressionstatement, string filterexpressionstatement,
                                                string evaluationorder, string evaluationparameter)
        {
            string sql = @"INSERT INTO listsettings.listbaseviewfilter(
            listbaseviewfilter_id, list_id,filtertype, evaulationattribute, evaluationcomparator, 
            evaluationcompareattribute, evaluationexpression, sqlexpressionstatement,filterexpressionstatement, evaluationorder, evaluationparameter)
    VALUES(  @listattributestylerule_id, @listattribute_id, @filtertype, @evaluationcomparator, 
            @evaluationcompareattribute, @evaluationexpression, @sqlexpressionstatement, @filterexpressionstatement, @evaluationorder, @evaluationparameter);";

            var paramList = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("listattributestylerule_id", listbaseviewfilter_id),

                   new KeyValuePair<string, string>("list_id", list_id),
                    new KeyValuePair<string, string>("filtertype", filtertype),
                     new KeyValuePair<string, string>("evaulationattribute", evaulationattribute),
                new KeyValuePair<string, string>("evaluationcomparator", evaluationcomparator),
                   new KeyValuePair<string, string>("evaluationcompareattribute", evaluationcompareattribute),
                      new KeyValuePair<string, string>("evaluationexpression", evaluationexpression),
                         new KeyValuePair<string, string>("sqlexpressionstatement", sqlexpressionstatement),
                                    new KeyValuePair<string, string>("filterexpressionstatement", filterexpressionstatement),

                new KeyValuePair<string, string>("evaluationorder", evaluationorder),
                   new KeyValuePair<string, string>("evaluationparameter", evaluationparameter),

            };
            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return listbaseviewfilter_id;

        }



        private static string createListAttributeStyleRule(string listattributestylerule_id, string listattribute_id, string list_id, string evaluationcomparator, string expressionstatement,
                                                          string evaluationexpression, string evaluationorder, string styletoapply)
        {
            string sql = @"INSERT INTO listsettings.listattributestylerule(
            listattributestylerule_id, listattribute_id, list_id, evaluationcomparator, 
            expressionstatement, evaluationexpression, evaluationorder, styletoapply)
    VALUES(  @listattributestylerule_id, @listattribute_id, @list_id, @evaluationcomparator, 
            @expressionstatement, @evaluationexpression, @evaluationorder, @styletoapply);";

            var paramList = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("listattributestylerule_id", listattributestylerule_id),
                new KeyValuePair<string, string>("listattribute_id",listattribute_id),
                   new KeyValuePair<string, string>("list_id", list_id),
                new KeyValuePair<string, string>("evaluationcomparator", evaluationcomparator),
                   new KeyValuePair<string, string>("expressionstatement", expressionstatement),
                      new KeyValuePair<string, string>("evaluationexpression", evaluationexpression),

                new KeyValuePair<string, string>("evaluationorder", evaluationorder),
                   new KeyValuePair<string, string>("styletoapply", styletoapply),

            };
            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return listattributestylerule_id;

        }

        private static string createListAttributeEntry(string listattributeid, string listid, string attributename, string baseviewattribute_id, string ordinalposition, string datatype,
                                                        string displayname, string defaultcssclassname, string isselected)
        {
            string sql = @"INSERT INTO listsettings.listattribute(
                              listattribute_id, list_id, baseviewattribute_id, attributename,
            datatype, displayname, ordinalposition, defaultcssclassname,
            isselected)
    VALUES(@listattribute_id,@list_id, @baseviewattribute_id, @attributename,
            @datatype, @displayname, case when (@ordinalposition='null' or @ordinalposition='') then null else CAST(@ordinalposition AS INT) end, @defaultcssclassname,
           cast(@isselected as bool) );";

            try { int.Parse(ordinalposition); }
            catch
            { ordinalposition = string.Empty; }

            var paramList = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("listattribute_id", listattributeid),
                new KeyValuePair<string, string>("list_id",listid),
                   new KeyValuePair<string, string>("baseviewattribute_id", baseviewattribute_id),
                new KeyValuePair<string, string>("attributename", attributename),
                   new KeyValuePair<string, string>("datatype", datatype),
                      new KeyValuePair<string, string>("displayname", displayname),

                new KeyValuePair<string, string>("ordinalposition",ordinalposition.Trim() ),
                   new KeyValuePair<string, string>("defaultcssclassname", defaultcssclassname),
                      new KeyValuePair<string, string>("isselected", isselected),
            };
            try
            {
                DataServices.executeSQLStatement(sql, paramList);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return listid;
        }

        private static string createListManagerEntry(string listId, string listname, string listdescription, string listcontextkey, string baseview_id,
                                            string listnamespaceid, string listnamespace, string defaultcontext, string defaultcontextfield, string matchedcontextfield,
                                             string patientbannerfield, string rowcssfield, string tablecssstyle, string tableheadercssstyle, string defaultrowcssstyle)
        {
            string sql = "INSERT INTO listsettings.listmanager(list_id, listname, listdescription, listcontextkey, baseview_id, listnamespaceid, listnamespace, defaultcontext, defaultcontextfield, matchedcontextfield, tablecssstyle, tableheadercssstyle, defaultrowcssstyle, patientbannerfield, rowcssfield) VALUES (@list_id, @listname, @listdescription, @listcontextkey, @baseview_id, @listnamespaceid, @listnamespace, @defaultcontext, @defaultcontextfield, @matchedcontextfield, @tablecssstyle, @tableheadercssstyle, @defaultrowcssstyle, @patientbannerfield, @rowcssfield);";

            string newId = System.Guid.NewGuid().ToString();
            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("list_id", listId),
                new KeyValuePair<string, string>("listname", listname),
                new KeyValuePair<string, string>("listdescription", listdescription),
                new KeyValuePair<string, string>("listcontextkey", ""),
                new KeyValuePair<string, string>("baseview_id", baseview_id),
                new KeyValuePair<string, string>("listnamespaceid", listnamespaceid),
                new KeyValuePair<string, string>("listnamespace", listnamespace),
                new KeyValuePair<string, string>("defaultcontext", defaultcontext),
                new KeyValuePair<string, string>("defaultcontextfield", defaultcontextfield),
                new KeyValuePair<string, string>("matchedcontextfield", matchedcontextfield),
                new KeyValuePair<string, string>("patientbannerfield", patientbannerfield),
                new KeyValuePair<string, string>("rowcssfield",rowcssfield),
                new KeyValuePair<string, string>("tablecssstyle", tablecssstyle),
                new KeyValuePair<string, string>("tableheadercssstyle", tableheadercssstyle),
                new KeyValuePair<string, string>("defaultrowcssstyle", defaultrowcssstyle)

            };

            try
            {
                DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


            // string returnListId = DataServices.ExecuteScalar(string.Format("select list_id from listsettings.listmanager where listname = '{0}' and listnamespaceid = '{1}'", listname, listnamespaceid));

            return listId;
        }

        private static string CreateEntity(Entity entity, DataSetSerializerType format)
        {

            DataTable entityattribute = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityAttributes, format)).Tables[0];
            DataTable entitymanager = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityManager, format)).Tables[0];
            DataTable entityrelation = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.EntityRelation, format)).Tables[0];
            DataTable entitynamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.Namespace, format)).Tables[0];
            DataTable synapsenamespace = ((DataSet)SynapseHelpers.DeserializeDataSet(entity.SystemNamespace, format)).Tables[0];

            //check if system namespace exists
            string src_synapsenamespacename = synapsenamespace.Rows[0]["synapsenamespacename"].ToString();
            string src_synapsenamespaceid = synapsenamespace.Rows[0]["synapsenamespaceid"].ToString();
            string src_synapsenamespacedescription = synapsenamespace.Rows[0]["synapsenamespacedescription"].ToString();
            string dest_synapsenamespaceid = CheckIfSynapseNamespaceExists(src_synapsenamespacename);

            if (string.IsNullOrEmpty(dest_synapsenamespaceid))
            {
                //create namespace
                //TODO error handling. 
                dest_synapsenamespaceid = CreateSystemNameSpace(src_synapsenamespaceid, src_synapsenamespacename, src_synapsenamespacedescription, "createdby_placeholder");

                try { Guid.Parse(dest_synapsenamespaceid); }
                catch
                {
                    AddImportMsg(string.Format("Error creating synapse namespace {0}. Entity creating skipped.  : {1}", src_synapsenamespacename, dest_synapsenamespaceid), "error");
                    return string.Empty;
                }
            }

            string dest_localnamespaceid = string.Empty;
            string src_localnamespacename = string.Empty;
            //check if local namespace exists
            if (entitynamespace.Rows.Count != 0) // could be a system core entity, skip local namespace
            {
                src_localnamespacename = entitynamespace.Rows[0]["localnamespacename"].ToString();
                string src_localnamespaceid = entitynamespace.Rows[0]["localnamespaceid"].ToString();
                string src_localnamespacedescription = entitynamespace.Rows[0]["localnamespacedescription"].ToString();
                dest_localnamespaceid = CheckIfNamespaceExists(src_localnamespacename);

                if (string.IsNullOrEmpty(dest_localnamespaceid))
                {
                    //create namespace
                    //TODO error handling. 
                    dest_localnamespaceid = CreateLocalNameSpace(src_localnamespaceid, src_localnamespacename, src_localnamespacedescription, "createdby_placeholder");

                    try { Guid.Parse(dest_localnamespaceid); }
                    catch
                    {
                        AddImportMsg(string.Format("Error creating local namespace {0}. Entity creation skipped : {1}", src_localnamespacename, dest_localnamespaceid), "error");
                        return string.Empty;
                    }
                }
            }
            //create entity from model
            string message = string.Empty;
            string newEntityId = CreateNewEntity(dest_synapsenamespaceid, src_synapsenamespacename, entity.Name, entitymanager.Rows[0]["entitydescription"].ToString(),
                                "createdby_placeholder", dest_localnamespaceid, src_localnamespacename, out message);

            if (newEntityId != null)
            {
                AddImportMsg("Entity manager created successfully : " + entity.Name);
                //add attributes
                DataTable localsystemnamespace = GetEntitySystemNamespace(newEntityId).Tables[0];

                AddImportMsg("Adding attributes : " + entity.Name);

                foreach (DataRow dr in entityattribute.Rows)
                {
                    if (dr["issystemattribute"].ToString() != "1" && dr["iskeycolumn"].ToString() != "1")
                    {
                        AddImportMsg(string.Format("Adding attribute  {0}.", dr["attributename"].ToString()));
                        //if not null success
                        string retval = CreateNewAttribute(newEntityId, dr["entityname"].ToString(), localsystemnamespace.Rows[0]["synapsenamespaceid"].ToString(), localsystemnamespace.Rows[0]["synapsenamespacename"].ToString(), "createdby_placeholder", dr["attributename"].ToString(), dr["attributedescription"].ToString(), dr["datatype"].ToString(), dr["datatypeid"].ToString(),
                               dr["ordinal_position"].ToString(), dr["attributedefault"].ToString(), dr["maximumlength"].ToString(), dr["commondisplayname"].ToString(), dr["isnullsetting"].ToString());

                        if (!string.IsNullOrEmpty(retval))
                        {
                            AddImportMsg(string.Format("Error creating attribute  {0}. Entity creation skipped. Error: {1}", dr["attributename"].ToString(), retval), "error");
                            return string.Empty;
                        }
                    }
                }

                foreach (DataRow dr in entityrelation.Rows)
                {
                    DataSet dslocalparententity = GetEntityIDsByName(dr["parententityname"].ToString());
                    AddImportMsg(string.Format("Creating new relation to parent entity :{0} and attribute {1}", dr["parententityname"].ToString(), dr["parentattributename"].ToString()));
                    if (dslocalparententity.Tables.Count != 0 && dslocalparententity.Tables[0].Rows.Count != 0)
                    {
                        //DataTable dtLocal = GetEntityManager(newEntityId).Tables[0];
                        //DataRow drLocalAttribute = GetEntityAttributes(newEntityId).Tables[0].Select("attributename = '" + dr["localentityattributename"].ToString() + "'")[0];

                        //CreateRelation(newEntityId, dr["entityname"].ToString(), localsystemnamespace.Rows[0]["synapsenamespaceid"].ToString(),
                        //                localsystemnamespace.Rows[0]["synapsenamespacename"].ToString(), drParentAttribute["attributename"].ToString(), drParentAttribute["attributeid"].ToString(),
                        //                    dtParent.Rows[0]["entityid"].ToString(), dtParent.Rows[0]["entityname"].ToString(), dtParent.Rows[0]["synapsenamespaceid"].ToString(),
                        //                     dtParent.Rows[0]["synapsenamespacename"].ToString(), "createdby_placeholder", drLocalAttribute["attributeid"].ToString(),
                        //                        drLocalAttribute["attributename"].ToString(), SynapseHelpers.GetNextOrdinalPositionFromID(newEntityId));


                        string localattributeid = string.Empty;
                        string localattributename = string.Empty;
                        if (!string.IsNullOrWhiteSpace(dr["localentityattributename"].ToString()) && dr["localentityattributename"].ToString() != "No Local Attribute")
                        {
                            DataRow drLocalAttribute = GetEntityAttributes(newEntityId).Tables[0].Select("attributename = '" + dr["localentityattributename"].ToString() + "'")[0];
                            localattributename = drLocalAttribute["attributename"].ToString();
                            localattributeid = drLocalAttribute["attributeid"].ToString();
                        }

                        string localparentEntityId = GetEntityIDsByName(dr["parententityname"].ToString()).Tables[0].Rows[0][0].ToString();
                        DataTable dtParent = GetEntityManager(localparentEntityId).Tables[0];
                        DataRow drParentAttribute;

                        if (!string.IsNullOrWhiteSpace(localattributename))

                            drParentAttribute = GetEntityAttributes(dtParent.Rows[0]["entityid"].ToString()).Tables[0].Select("attributename = '" + dr["parentattributename"].ToString().Replace("_" + localattributename, string.Empty) + "'")[0];
                        else
                            drParentAttribute = GetEntityAttributes(dtParent.Rows[0]["entityid"].ToString()).Tables[0].Select("attributename = '" + dr["parentattributename"].ToString() + "'")[0];


                        string retval = CreateRelation(newEntityId, dr["entityname"].ToString(), localsystemnamespace.Rows[0]["synapsenamespaceid"].ToString(),
                                           localsystemnamespace.Rows[0]["synapsenamespacename"].ToString(), dr["parentattributename"].ToString(), drParentAttribute["attributeid"].ToString(),
                                               dtParent.Rows[0]["entityid"].ToString(), dtParent.Rows[0]["entityname"].ToString(), dtParent.Rows[0]["synapsenamespaceid"].ToString(),
                                                dtParent.Rows[0]["synapsenamespacename"].ToString(), "createdby_placeholder", localattributeid,
                                                  localattributename, dr["entityrelation_id"].ToString());
                        if (!string.IsNullOrEmpty(retval))
                        {
                            AddImportMsg(string.Format("Error: There was an error creating relation to parent entity {0}. Attribute: {1} Error: {2}", dr["parententityname"].ToString(), dr["attributename"].ToString(), retval), "error");
                        }
                        else
                        {

                            // AddImportMsg("Relation created successfully", "success");
                        }
                    }

                    else
                    {
                        AddImportMsg(string.Format("Parent entity {0} doesn't exist to create relation", dr["parententityname"].ToString()), "error");
                    }
                }
                try
                {
                    UpdateBaseviewsForEntity(newEntityId);
                }
                catch (Exception ex) { AddImportMsg("Error: there was an error updating baseviews for entity: " + entity.Name + "Exception: " + ex.Message, "error"); }
            }
            else
            {
                AddImportMsg("There was a problem creating " + entity.Name + " : " + message, "error");
            }

            return newEntityId;
        }

        //private static void CreateRelation(string entityId, string entityName, string synapsenamespaceid, string synapsenamespacename, string attributename, string attributeid,
        //                                    string parententityid, string parententityname, string parentsynapsenamespaceid, string parentsynapsenamespacename,
        //                                      string createdby, string localentityattributeid, string localentityattributename, string ordinalPosition)
        //{
        //    string sql = @"SELECT entitysettings.addrelationtoentity(
        //                     @p_entityid, 
        //                     @p_entityname, 
        //                     @p_synapsenamespaceid, 
        //                     @p_synapsenamespacename, 	 
        //                        @p_parententityid,
        //                        @p_parententityname,
        //                        @p_parentsynapsenamespaceid, 
        //                     @p_parentsynapsenamespacename, 
        //                        @p_attributeid,
        //                     @p_attributename,                            
        //                     CAST(@p_ordinal_position AS integer),
        //                     @p_entityversionid,
        //                        @p_username,
        //                        @p_localentityattributeid,
        //                        @p_localentityattributename
        //                    )";

        //    //DataTable dt = SynapseHelpers.GetEntityDSFromID(this.hdnEntityID.Value);
        //    //DataTable dtParent = SynapseHelpers.GetEntityDSFromID(this.ddlEntity.SelectedValue);
        //    //DataTable dtKey = SynapseHelpers.GetEntityKeyAttributeFromID(this.ddlEntity.SelectedValue);

        //    //string attributename = dtKey.Rows[0]["attributename"].ToString();
        //    //if (this.ddlLocalAttribute.SelectedIndex > 0)
        //    //{
        //    //    attributename += "_" + this.ddlLocalAttribute.SelectedItem.Text;
        //    //}


        //    var paramList = new List<KeyValuePair<string, string>>()
        //    {
        //        new KeyValuePair<string, string>("p_entityid", entityId),
        //        new KeyValuePair<string, string>("p_entityname",entityName),
        //        new KeyValuePair<string, string>("p_synapsenamespaceid", synapsenamespaceid),
        //        new KeyValuePair<string, string>("p_synapsenamespacename", synapsenamespacename),
        //        new KeyValuePair<string, string>("p_parententityid", parententityid),
        //        new KeyValuePair<string, string>("p_parententityname", parententityname),
        //        new KeyValuePair<string, string>("p_parentsynapsenamespaceid", parentsynapsenamespaceid),
        //        new KeyValuePair<string, string>("p_parentsynapsenamespacename", parentsynapsenamespacename),
        //        new KeyValuePair<string, string>("p_attributeid", attributeid),
        //        new KeyValuePair<string, string>("p_attributename", attributename),
        //        new KeyValuePair<string, string>("p_ordinal_position", ordinalPosition),
        //        new KeyValuePair<string, string>("p_entityversionid", SynapseHelpers.GetCurrentEntityVersionFromID(entityId)),
        //        new KeyValuePair<string, string>("p_username", createdby),
        //        new KeyValuePair<string, string>("p_localentityattributeid", localentityattributeid),
        //        new KeyValuePair<string, string>("p_localentityattributename", localentityattributename)
        //    };

        //    DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

        //}


        private static string CreateRelation(string entityId, string entityName, string synapsenamespaceid, string synapsenamespacename, string attributename, string attributeid,
                                            string parententityid, string parententityname, string parentsynapsenamespaceid, string parentsynapsenamespacename,
                                              string createdby, string localentityattributeid, string localentityattributename, string relationid = null)
        {

            relationid = relationid ?? new Guid().ToString();
            //--Insert entry into entity relation
            string sql = @"INSERT INTO entitysettings.entityrelation(_createdsource, _createdby, entityrelation_id, synapsenamespaceid, synapsenamespacename, 
                                                                            entityid, entityname, parentsynapsenamespaceid, parentsynapsenamespacename, parententityid, 
                                                                                parententityname, parentattributeid, parentattributename, localentityattributeid, localentityattributename)

                            VALUES(
                            'Migration Tool',
                            @p_username,
                            @s_entityrelation_id,
                            @p_synapsenamespaceid,
                            @p_synapsenamespacename,
                            @p_entityid,
                            @p_entityname,
                            @p_parentsynapsenamespaceid,
                            @p_parentsynapsenamespacename,
                            @p_parententityid,
                            @p_parententityname,
                            @p_attributeid,
                            @p_attributename,
                            @p_localentityattributeid,
                            @p_localentityattributename
                            )";
            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_entityid", entityId),
                new KeyValuePair<string, string>("p_entityname", entityName),
                new KeyValuePair<string, string>("p_synapsenamespaceid", synapsenamespaceid),
                new KeyValuePair<string, string>("p_synapsenamespacename",synapsenamespacename),
                new KeyValuePair<string, string>("p_parententityid", parententityid),
                new KeyValuePair<string, string>("p_parententityname",parententityname),
                new KeyValuePair<string, string>("p_parentsynapsenamespaceid", parentsynapsenamespaceid),
                new KeyValuePair<string, string>("p_parentsynapsenamespacename", parentsynapsenamespacename),
                new KeyValuePair<string, string>("p_attributeid",attributeid),
                new KeyValuePair<string, string>("p_attributename", attributename),
                new KeyValuePair<string, string>("p_username", createdby),
                new KeyValuePair<string, string>("p_localentityattributeid", localentityattributeid),
                new KeyValuePair<string, string>("p_localentityattributename", localentityattributename),
                 new KeyValuePair<string, string>("s_entityrelation_id", relationid)
            };

            string retval = DataServices.ExcecuteNonQueryFromSQL(sql, paramList);

            if (string.IsNullOrEmpty(retval))
            {
                sql = "update entitysettings.entityattribute set entityrelation_id = @p_entityrelation_id where attributename = @p_attributename and entityid = @p_entityid";
                paramList = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("p_entityrelation_id", relationid),
                                                                          new KeyValuePair<string, string>("p_attributename", attributename),
                                                                              new KeyValuePair<string, string>("p_entityid", entityId)};

                retval = DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
                if (!string.IsNullOrEmpty(retval))
                {
                    AddImportMsg(string.Format("ERROR: There was an error updating relationid in entity attribute table for attribute {2} and parent entity {0}.{1}", parentsynapsenamespacename, parententityname, attributename), "error");
                }

            }
            else
            {
                AddImportMsg(string.Format("ERROR: There was a problem creating relation to parent entity {0}.{1} for attribute : {2}", parentsynapsenamespacename, parententityname, attributename), "error");
            }
            return retval;
        }
        private static string CreateNewBaseView(string baseviewNamespaceId, string baseviewNamespace, string baseviewName, string baseviewDescription, string baseviewSQLStatement, string createdBy, bool recreate, string baseviewId = null)
        {
            if (recreate)
            {
                AddImportMsg(string.Format("Dropping existing baseview: {0}", baseviewName));
                string sqlDrop = @"SELECT listsettings.dropbaseview(
	                    @p_baseview_id, 
	                    @p_baseviewname
                    )";

                var paramListDrop = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_baseview_id", baseviewId),
                new KeyValuePair<string, string>("p_baseviewname",baseviewNamespace+"_"+ baseviewName)
            };

                string retval = DataServices.ExcecuteNonQueryFromSQL(sqlDrop, paramListDrop);
                if (!string.IsNullOrEmpty(retval))
                {
                    AddImportMsg(string.Format("Error: There was an error dropping existing baseview {0}, baseview update skipped", baseviewName), "error");
                    return retval;
                }
            }

            string tmpViewName = Guid.NewGuid().ToString("N");
            //Check if we can create a temporary view using the supplied SQL Statement
            string sqlCreate = "CREATE VIEW baseviewcore." + baseviewNamespace + "_" + baseviewName + " AS " + baseviewSQLStatement + ";";
            var paramListCreate = new List<KeyValuePair<string, string>>()
            {
            };

            try
            {
                DataServices.executeSQLStatement(sqlCreate, paramListCreate);
            }
            catch (Exception ex)
            {
                AddImportMsg(string.Format("Error: There was an error creating Baseview {0} in baseviewcore namespace", "baseviewcore." + baseviewNamespace + "_" + baseviewName), "error");

                return ex.Message;
            }



            string sql = !recreate ?
                            "SELECT listsettings.createbaseview(@p_baseviewnamespaceid, @p_baseviewnamespace, @p_baseviewname, @p_baseviewdescription, @p_baseviewsqlstatement, @p_username);"
                            :
                            "SELECT listsettings.recreatebaseview(@p_baseviewnamespaceid, @p_baseviewnamespace, @p_baseviewname, @p_baseviewdescription, @p_baseviewsqlstatement, @p_username, @baseviewid);";


            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_baseviewnamespaceid", baseviewNamespaceId),
                new KeyValuePair<string, string>("p_baseviewnamespace", baseviewNamespace),
                new KeyValuePair<string, string>("p_baseviewname",baseviewName),
                new KeyValuePair<string, string>("p_baseviewdescription", baseviewDescription),
                new KeyValuePair<string, string>("p_baseviewsqlstatement", baseviewSQLStatement),
                new KeyValuePair<string, string>("p_username", createdBy)
                            };

            if (recreate)
                paramList.Add(new KeyValuePair<string, string>("baseviewid", baseviewId));

            DataSet ds = new DataSet();
            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                AddImportMsg(string.Format("Error: There was an error creating Baseview {0}", baseviewName), "error");

                return ex.Message;
            }

            DataTable dt = ds.Tables[0];

            string newGuid = "";

            try
            {
                newGuid = dt.Rows[0][0].ToString();
            }
            catch { return string.Empty; }

            return newGuid;


        }
        private static void UpdateBaseviewsForEntity(string entityId)
        {
            DataTable dtBVs = SynapseHelpers.GetEntityBaseviewsDT(entityId);

            foreach (DataRow row in dtBVs.Rows)
            {
                string baseview_id = row["baseview_id"].ToString();
                string sqlRecreate = "SELECT listsettings.recreatebaseview(@baseview_id);";
                var paramListRecreate = new List<KeyValuePair<string, string>>() {
                   new KeyValuePair<string, string>("baseview_id",baseview_id)
                };

                try
                {
                    DataServices.ExcecuteNonQueryFromSQL(sqlRecreate, paramListRecreate);
                }
                catch (Exception ex)
                {
                    var a = ex.ToString();
                }
            }

        }
        private static string CreateNewAttribute(string entityId, string entityName, string synapseNameSpaceID, string synapseNameSpaceName, string createdBy, string attributeName, string attributeDescripton,
                                string dataType, string dataTypeId, string ordinalPosition, string attributeDefault, string maxLength, string commonDisplayName, string isNullSetting)
        {
            DataTable dtBVs = SynapseHelpers.GetEntityBaseviewsDT(entityId);

            string sql = @"SELECT entitysettings.addattributetoentity(
	                            @p_entityid, 
	                            @p_entityname, 
	                            @p_synapsenamespaceid, 
	                            @p_synapsenamespacename, 
	                            @p_username, 
	                            @p_attributename, 
	                            @p_attributedescription, 
	                            @p_datatype, 
	                            @p_datatypeid, 
	                            CAST(@p_ordinal_position AS integer),
	                            @p_attributedefault, 
	                            @p_maximumlength, 
	                            @p_commondisplayname, 
	                            @p_isnullsetting, 
	                            @p_entityversionid
                            )";

            var paramList = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("p_entityid", entityId),
                new KeyValuePair<string, string>("p_entityname", entityName),
                new KeyValuePair<string, string>("p_synapsenamespaceid", synapseNameSpaceID),
                new KeyValuePair<string, string>("p_synapsenamespacename", synapseNameSpaceName),
                new KeyValuePair<string, string>("p_username", createdBy),
                new KeyValuePair<string, string>("p_attributename", attributeName),
                new KeyValuePair<string, string>("p_attributedescription", attributeDescripton),
                new KeyValuePair<string, string>("p_datatype", dataType),
                new KeyValuePair<string, string>("p_datatypeid", dataTypeId),
                new KeyValuePair<string, string>("p_ordinal_position", ordinalPosition),
                new KeyValuePair<string, string>("p_attributedefault", attributeDefault),
                new KeyValuePair<string, string>("p_maximumlength", maxLength),
                new KeyValuePair<string, string>("p_commondisplayname", commonDisplayName),
                new KeyValuePair<string, string>("p_isnullsetting", isNullSetting),
                new KeyValuePair<string, string>("p_entityversionid", SynapseHelpers.GetCurrentEntityVersionFromID(entityId))
            };

            try
            {
                return DataServices.ExcecuteNonQueryFromSQL(sql, paramList);
            }
            catch (Exception ex) { return ex.Message; }

            //foreach (DataRow row in dtBVs.Rows)
            //{
            //    string baseview_id = row["baseview_id"].ToString();
            //    string sqlRecreate = "SELECT listsettings.recreatebaseview(@baseview_id);";
            //    var paramListRecreate = new List<KeyValuePair<string, string>>() {
            //       new KeyValuePair<string, string>("baseview_id",baseview_id)
            //    };

            //    try
            //    {
            //        DataServices.ExcecuteNonQueryFromSQL(sqlRecreate, paramListRecreate);
            //    }
            //    catch (Exception ex)
            //    {
            //        var a = ex.ToString();
            //    }

            //}
        }
        public static string CreateSystemNameSpace(string id, string name, string description, string createdBy)
        {
            string sql = "INSERT INTO entitysettings.synapsenamespace(_createdsource, synapsenamespaceid, synapsenamespacename, synapsenamespacedescription, _createdby) VALUES ('Migration Tool',@synapsenamespaceid, @synapsenamespacename,  @synapsenamespacedescription, @p_username);";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("synapsenamespaceid", id),
                new KeyValuePair<string, string>("synapsenamespacename", name),
                new KeyValuePair<string, string>("synapsenamespacedescription",description),
                new KeyValuePair<string, string>("p_username", createdBy)

            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
                return id;
            }
            catch (Exception ex) { return ex.Message; }
        }
        public static string CreateLocalNameSpace(string id, string name, string description, string createdBy)
        {
            string sql = "INSERT INTO entitysettings.localnamespace(_createdsource, localnamespaceid, localnamespacename, localnamespacedescription, _createdby) VALUES ('Synapse Studio',@localnamespaceid, @localnamespacename, @localnamespacedescription, @p_username);";

            var paramList = new List<KeyValuePair<string, string>>() {
                  new KeyValuePair<string, string>("localnamespaceid", id),
                new KeyValuePair<string, string>("localnamespacename", name),
                new KeyValuePair<string, string>("localnamespacedescription", description),
                new KeyValuePair<string, string>("p_username", createdBy)

            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
                return id;
            }
            catch (Exception ex) { return ex.Message; }

        }
        public static string CheckIfNamespaceExists(string name)
        {
            string sql = "select localnamespaceid from entitySettings.localnamespace where localnamespacename = @p_namespacename";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_namespacename", name)
                };

            string returnEntityId = DataServices.ExecuteScalar(sql, paramList);

            return returnEntityId;
        }
        public static string CheckIfSynapseNamespaceExists(string name)
        {
            string sql = "select synapsenamespaceid from entitySettings.synapsenamespace where synapsenamespacename = @p_namespacename";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_namespacename", name)
                };

            string returnEntityId = DataServices.ExecuteScalar(sql, paramList);

            return returnEntityId;
        }
        public static string CheckIfEntityExists(string name, string synapsenamespacename)
        {
            string sql = "select entityid from entitySettings.entitymanager where entityname = @p_entityname and synapsenamespacename = @p_synapsenamespacename";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_entityname", name),
                    new KeyValuePair<string, string>("p_synapsenamespacename", synapsenamespacename)
                };

            string returnid = DataServices.ExecuteScalar(sql, paramList);

            return (returnid);
        }
        public static string CheckIfBaseviewExists(string name, string namespacename)
        {
            string sql = "select baseview_id from listSettings.baseviewmanager where baseviewname = @p_name and baseviewnamespace = @p_namespace ";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_name", name),
                 new KeyValuePair<string, string>("p_namespace", namespacename)
                };

            string returnid = DataServices.ExecuteScalar(sql, paramList);

            return (returnid);
        }
        public static string CheckIfListExists(string name, string namespacename)
        {
            string sql = "select list_id from listSettings.listmanager where listname = @p_name and listnamespace = @p_namespace ";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_name", name),
                 new KeyValuePair<string, string>("p_namespace", namespacename)
                };

            string returnid = DataServices.ExecuteScalar(sql, paramList);

            return (returnid);
        }
        public static string CheckIfBaseviewNamespaceExists(string name)
        {
            string sql = "select baseviewnamespaceid from listSettings.baseviewnamespace where baseviewnamespace = @p_namespacename";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_namespacename", name)
                };

            string returnEntityId = DataServices.ExecuteScalar(sql, paramList);

            return returnEntityId;
        }
        public static string CheckIfListNamespaceExists(string name)
        {
            string sql = "select listnamespaceid from listSettings.listnamespace where listnamespace = @p_namespacename";

            var paramList = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("p_namespacename", name)
                };

            string returnEntityId = DataServices.ExecuteScalar(sql, paramList);

            return returnEntityId;
        }
        public static string CreateBaseviewNameSpace(string id, string name, string description, string createdBy)
        {
            string sql = "INSERT INTO listsettings.baseviewnamespace(_createdsource, baseviewnamespace, baseviewnamespacedescription, _createdby,baseviewnamespaceid) VALUES ('Migration Tool', @baseviewnamespace, @baseviewnamespacedescription, @p_username,@baseviewnamespaceid);";

            var paramList = new List<KeyValuePair<string, string>>() {
                 new KeyValuePair<string, string>("baseviewnamespaceid", id),
                new KeyValuePair<string, string>("baseviewnamespace", name),
                new KeyValuePair<string, string>("baseviewnamespacedescription", description),
                new KeyValuePair<string, string>("p_username", createdBy)
            };

            try
            {
                DataServices.executeSQLStatement(sql, paramList);
                return id;
            }
            catch (Exception ex) { return ex.Message; }

        }
        public static string CreateListNameSpace(string id, string name, string description, string createdBy)
        {
            string sql = "INSERT INTO listsettings.listnamespace(_createdsource, listnamespace, listnamespacedescription, _createdby,listnamespaceid) VALUES ('Migration Tool', @listnamespace, @listnamespacedescription, @p_username,@listnamespaceid);";

            var paramList = new List<KeyValuePair<string, string>>() {
                 new KeyValuePair<string, string>("listnamespaceid", id),
                new KeyValuePair<string, string>("listnamespace", name),
                new KeyValuePair<string, string>("listnamespacedescription", description),
                new KeyValuePair<string, string>("p_username", createdBy)
            };
            try
            {
                DataServices.executeSQLStatement(sql, paramList);
                return id;
            }
            catch (Exception ex) { return ex.Message; }

        }
        private static string CreateNewEntity(string synapseNameSpaceId, string synapseNameSpaceName, string entityName, string entityDescription, string createdBy, string localNameSpaceId, string localNameSpaceName, out string message)
        {
            string sql = "SELECT entitysettings.createentityfrommodel(@p_synapsenamespaceid, @p_synapsenamespacename, @p_entityname, @p_entitydescription, @p_username, @p_localnamespaceid, @p_localnamespacename)";

            var paramList = new List<KeyValuePair<string, string>>() {
                  new KeyValuePair<string, string>("p_synapsenamespaceid", synapseNameSpaceId),
                  new KeyValuePair<string, string>("p_synapsenamespacename", synapseNameSpaceName),
                  new KeyValuePair<string, string>("p_entityname", entityName),
                  new KeyValuePair<string, string>("p_entitydescription", entityDescription),
                  new KeyValuePair<string, string>("p_username", createdBy),
                  new KeyValuePair<string, string>("p_localnamespaceid", localNameSpaceId),
                  new KeyValuePair<string, string>("p_localnamespacename", localNameSpaceName),
              };

            DataSet ds = new DataSet();

            try
            {
                ds = DataServices.DataSetFromSQL(sql, paramList);
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return null;
            }

            DataTable dt = ds.Tables[0];

            string newGuid = "";

            try
            {
                newGuid = dt.Rows[0][0].ToString();
            }
            catch (Exception ex1) { message = ex1.Message; return null; }

            message = string.Empty;
            return newGuid;
        }
        #endregion

    }
}
