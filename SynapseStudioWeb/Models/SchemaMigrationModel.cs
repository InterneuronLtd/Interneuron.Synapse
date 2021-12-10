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


﻿using SynapseStudioWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SynapseStudioWeb.Models
{     
    public class TreeViewNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
    }
    [XmlRoot(ElementName = "Entity")]

    public class Entity
    {
        [XmlElement(ElementName = "Namespace")]
        public string Namespace { get; set; }
        [XmlElement(ElementName = "EntityManager")]
        public string EntityManager { get; set; }
        [XmlElement(ElementName = "EntityAttributes")]
        public string EntityAttributes { get; set; }
        [XmlElement(ElementName = "EntityRelation")]
        public string EntityRelation { get; set; }

        [XmlElement(ElementName = "SystemNamespace")]
        public string SystemNamespace { get; set; }

        [XmlElement(ElementName = "ViewDefinition")]
        public string ViewDefinition { get; set; }
        [XmlElement(ElementName = "MaterializerViewDefinition")]
        public string MaterializerViewDefinition { get; set; }
        [XmlAttribute(AttributeName = "id")]

        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        public SynapseHelpers.MigrationAction action { get; set; }

        public List<Action> attributeActions { get; set; }

    }

    [XmlRoot(ElementName = "Entities")]
    public class Entities
    {
        [XmlElement(ElementName = "Entity")]
        public List<Entity> Entity { get; set; }

        public Entities()
        {
            this.Entity = new List<Entity>();
        }

        internal DataSet GetCurrentSchema()
        {
            DataSet idsFromName = SynapseHelpers.GetEntityIDsByName(string.Join("','", Entity.Select(x => x.Name.Trim()).ToList<string>()));

            var allEntitiyIds = idsFromName.Tables[0].AsEnumerable().Select(x => x["entityid"].ToString()).ToList<string>();
            string entityids = string.Join("','", allEntitiyIds);
            DataSet ds = new DataSet();
            DataTable dt = SynapseHelpers.GetEntityManager(entityids).Tables[0];
            if (dt != null)
                ds.Tables.Add(dt.Copy());
            dt = SynapseHelpers.GetEntityAttributes(entityids).Tables[0];
            if (dt != null)
                ds.Tables.Add(dt.Copy());
            dt = SynapseHelpers.GetEntityRelation(entityids).Tables[0];
            if (dt != null)
                ds.Tables.Add(dt.Copy());
            dt = SynapseHelpers.GetEntityNamespace(entityids).Tables[0];
            if (dt != null)
                ds.Tables.Add(dt.Copy());
            return ds;
        }
    }

    [XmlRoot(ElementName = "Baseview")]

    public class Baseview
    {
        [XmlElement(ElementName = "Namespace")]
        public string Namespace { get; set; }

        [XmlElement(ElementName = "BaseviewManager")]
        public string BaseviewManager { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        public SynapseHelpers.MigrationAction action { get; set; }

    }

    [XmlRoot(ElementName = "Baseviews")]
    public class Baseviews
    {
        [XmlElement(ElementName = "Baseview")]
        public List<Baseview> Baseview { get; set; }

        public Baseviews()
        {
            this.Baseview = new List<Baseview>();
        }

        internal DataSet GetCurrentSchema()
        {
            var allBaseviewIds = Baseview.Select(x => x.Id.Trim()).ToList<string>();
            string baseviewIDs = string.Join(",", allBaseviewIds);
            DataSet ds = new DataSet();
            DataTable dt = SynapseHelpers.GetBaseviewManager(baseviewIDs).Tables[0];
            if (dt != null)
                ds.Tables.Add(dt.Copy());

            dt = SynapseHelpers.GetBaseviewNamespace(baseviewIDs).Tables[0];
            if (dt != null)
                ds.Tables.Add(dt.Copy());
            return ds;
        }
    }

    [XmlRoot(ElementName = "SList")]

    public class SList
    {
        [XmlElement(ElementName = "Namespace")]
        public string Namespace { get; set; }

        [XmlElement(ElementName = "ListManager")]
        public string ListManager { get; set; }

        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "ListAttribute")]
        public string ListAttribute { get; set; }

        [XmlElement(ElementName = "ListAttributeStyleRule")]
        public string ListAttributeStyleRule { get; set; }

        [XmlElement(ElementName = "ListBaseviewFilter")]
        public string ListBaseviewFilter { get; set; }

        [XmlElement(ElementName = "ListBaseviewParameter")]
        public string ListBaseviewParameter { get; set; }

        [XmlElement(ElementName = "ListQuestion")]
        public string ListQuestion { get; set; }

        [XmlElement(ElementName = "listQuestionResponse")]
        public string listQuestionResponse { get; set; }

        public SynapseHelpers.MigrationAction action { get; set; }

        [XmlElement(ElementName = "baseview")]
        public Baseview baseview { get; set; }
        [XmlElement(ElementName = "entity")]
        public Entity entity { get; set; }



    }

    [XmlRoot(ElementName = "SLists")]
    public class SLists
    {
        [XmlElement(ElementName = "SList")]
        public List<SList> SList { get; set; }

        public SLists()
        {
            this.SList = new List<SList>();
        }

        internal DataSet GetCurrentSchema()
        {
            var allListIds = SList.Select(x => x.Id.Trim()).ToList<string>();
            string listIds = string.Join(",", allListIds);
            DataSet ds = new DataSet();
            DataTable dt = SynapseHelpers.GetListManager(listIds).Tables[0];
            if (dt != null)
                ds.Tables.Add(dt.Copy());

            dt = SynapseHelpers.GetListNamespace(listIds).Tables[0];
            if (dt != null)
                ds.Tables.Add(dt.Copy());
            return ds;
        }
    }

    [XmlRoot(ElementName = "Export")]
    public class Export
    {
        [XmlElement(ElementName = "Entities")]
        public Entities Entities { get; set; }

        [XmlElement(ElementName = "Baseviews")]
        public Baseviews Baseviews { get; set; }

        [XmlElement(ElementName = "SLists")]
        public SLists SLists { get; set; }


        [XmlAttribute(AttributeName = "format")]
        public SynapseHelpers.DataSetSerializerType format { get; set; }

        public Export()
        {
            this.Entities = new Entities();
            this.Baseviews = new Baseviews();
            this.SLists = new SLists();
        }

        public string SerializeToXML(Export e)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Export));
            var xmlString = string.Empty;

            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    serializer.Serialize(writer, e);
                    xmlString = sw.ToString();
                }
            }
            return xmlString;

        }
        public string SerializeToXML()
        {
            return this.SerializeToXML(this);
        }

    }

    public class Action
    {

        public string componentKeyValue { get; set; }
        public string sourceComponentId { get; set; }
        public string destinationComponentId { get; set; }

        public string dbTableName { get; set; }
        public string contextColumnName { get; set; }
        public string sourceContextId { get; set; }
        public string destinationContextId { get; set; }
        public string componentKeyColumnName { get; set; }
        public string componentIdColumnName { get; set; }

        public SynapseHelpers.MigrationAction migrationAction { get; set; }
        public bool userConsent { get; set; }

        public Action()
        { }

        public Action(string componentKeyColumnName, string componentIdColumnName, string componentKeyValue, string sourceComponentId,
                        string destinationComponentId, string tableName, SynapseHelpers.MigrationAction migrationAction,
                    bool userConsent, string contextIdColumnName, string sourceContextId, string destinationContextId)
        {
            this.componentKeyValue = componentKeyValue;
            this.sourceComponentId = sourceComponentId;
            this.destinationComponentId = destinationComponentId;
            this.migrationAction = migrationAction;
            this.userConsent = userConsent;
            this.dbTableName = tableName;
            this.contextColumnName = contextIdColumnName;
            this.sourceContextId = sourceContextId;
            this.destinationContextId = destinationContextId;
            this.componentKeyColumnName = componentKeyColumnName;
            this.componentIdColumnName = componentIdColumnName;
        }
    }

    public class ImportStatusMessage
    {
        public string message { get; set; }
        public string type { get; set; }
        public bool seen { get; set; }

        public DateTime timestamp { get; set; }
        public int serialno { get; set; }



        public ImportStatusMessage(DateTime timestamp, string msg, string type, int serialno, bool seen = false)
        {
            this.message = msg;
            this.timestamp = timestamp;
            this.seen = seen;
            this.type = type;
            this.serialno = serialno;
        }

    }

}
