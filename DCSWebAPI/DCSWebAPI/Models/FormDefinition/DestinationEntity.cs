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


﻿using DCSWebAPI.Models.WebAPI.SynapseDynamicAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCSWebAPI.Models.FormDefinition
{
    public class DestinationEntity
    {
        private List<SQLParameter> _SQLParameters;
        private string _SQL;
        private SectionFields _AssociatedField;

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("operationType")]
        public string OperationType { get; set; }

        [JsonProperty("associatedFieldId")]
        public string AssociatedFieldId { get; set; }

        [JsonProperty("associatedFieldType")]
        public string AssociatedFieldType { get; set; }

        [JsonProperty("destinationAttributes")]
        public string[] DestinationAttributes { get; set; }

        [JsonProperty("destinationValues")]
        public string[] DestinationValues { get; set; }

        [JsonProperty("Wherecols")]
        public string[] WhereCols { get; set; }

        [JsonProperty("Wheredata")]
        public string[] WhereData { get; set; }

        [JsonProperty("destinationAttributeDatatypes")]
        public string[] DestinationAttributeDatatypes { get; set; }

        [JsonProperty("OutID")]
        public string OutID { get; set; }

        internal string SQL
        {
            set { _SQL = value; }
            get
            {
                if (string.IsNullOrWhiteSpace(_SQL))
                {
                    if (OperationType == SQLOperationType.INSERT)
                    {
                        _SQL = PrepareInsertStatement();
                    }
                    else if (OperationType == SQLOperationType.UPDATE)
                    {
                        _SQL = PrepareUpdateStatement();
                    }
                }
                return _SQL;
            }
        }

        internal Form ParentForm { get; set; }

        internal List<SQLParameter> SQLParameters
        {
            set { _SQLParameters = value; }
            get
            {
                if (_SQLParameters == null)
                    _SQLParameters = InitilizeSQLParameters();
                return _SQLParameters;
            }
        }

        internal SectionFields AssociatedField
        {
            set { _AssociatedField = value; }
            get
            {
                if (_AssociatedField == null)
                    _AssociatedField = GetAssociatedField(AssociatedFieldId);
                return _AssociatedField;
            }
        }

        private List<SQLParameter> InitilizeSQLParameters()
        {
            var parameterList = new List<SQLParameter>();

            for (int i = 0; i < DestinationValues.Length; i++)
            {
                string value = DestinationValues[i].Split(":").GetValue(1).ToString();
                string valueType = DestinationValues[i].Split(":").GetValue(0).ToString();
                object parameterValue = null;

                switch (valueType)
                {
                    case DestinationValueType.CONSTANT:
                        parameterValue = value;
                        parameterList.Add(new SQLParameter
                        {
                            Name = "@" + DestinationAttributes[i],
                            Value = parameterValue
                        });
                        break;
                    case DestinationValueType.COMPUTABLE:
                        parameterValue = GetComputableValue(value);
                        parameterList.Add(new SQLParameter
                        {
                            Name = "@" + DestinationAttributes[i],
                            Value = parameterValue
                        });
                        if (!string.IsNullOrWhiteSpace(OutID))
                        {
                            var parameter = ParentForm.FormParameters.Where(item => item.ParameterKey.Equals(OutID.Split(":").GetValue(1).ToString())).FirstOrDefault();
                            if (string.IsNullOrEmpty(parameter.ParameterValue))
                                parameter.ParameterValue = parameterValue as string;
                        }
                        break;
                    case DestinationValueType.PARAMETER:
                        parameterValue = ParentForm.FormParameters.Where(item => item.ParameterKey.Equals(value)).FirstOrDefault().ParameterValue;
                        parameterList.Add(new SQLParameter
                        {
                            Name = "@" + DestinationAttributes[i],
                            Value = parameterValue
                        });
                        break;
                    case DestinationValueType.FIELD:
                        if (!AssociatedFieldType.Equals("chkbl"))
                        {
                            SectionFields fld = GetAssociatedField(value);
                            parameterValue = fld.FieldData[fld.FieldData.Count - 1].Text;
                            parameterList.Add(new SQLParameter
                            {
                                Name = "@" + DestinationAttributes[i],
                                Value = parameterValue
                            });
                        }
                        else
                        {
                            int count = 0;
                            for (int j = 0; j < AssociatedField.FieldData.Count; j++)
                            {
                                parameterValue = AssociatedField.FieldData[j].Text;
                                parameterList.Add(new SQLParameter
                                {
                                    Name = "@" + DestinationAttributes[i] + count++,
                                    Value = parameterValue
                                });
                            }
                        }
                        break;
                }
            }

            if (WhereData != null)
            {
                for (int i = 0; i < WhereData.Length; i++)
                {
                    string value = WhereData[i].Split(":").GetValue(1).ToString();
                    string valueType = WhereData[i].Split(":").GetValue(0).ToString();
                    object parameterValue = null;

                    switch (valueType)
                    {
                        case DestinationValueType.CONSTANT:
                            parameterValue = value;
                            break;
                        case DestinationValueType.COMPUTABLE:
                            parameterValue = GetComputableValue(value);
                            break;
                        case DestinationValueType.PARAMETER:
                            parameterValue = ParentForm.FormParameters.Where(item => item.ParameterKey.Equals(value)).FirstOrDefault().ParameterValue;
                            break;
                        case DestinationValueType.FIELD:
                            parameterValue = GetAssociatedField(value).FieldData[0].Text;
                            break;
                    }

                    parameterList.Add(new SQLParameter
                    {
                        Name = "@" + WhereCols[i],
                        Value = parameterValue
                    });
                }
            }

            return parameterList;
        }

        private string PrepareInsertStatement()
        {
            StringBuilder insertQuery = new StringBuilder();

            insertQuery.Append("INSERT INTO " + this.Entity + "(" + String.Join(", ", DestinationAttributes) + ")");
            insertQuery.Append(" VALUES ");
            if (!string.IsNullOrWhiteSpace(AssociatedFieldType) && AssociatedFieldType.Equals("chkbl"))
            {
                int count = 0;
                foreach (var data in AssociatedField.FieldData)
                {
                    insertQuery.Append("(");
                    for (int i = 0; i < DestinationAttributes.Length; i++)
                    {
                        insertQuery.Append("@" + (DestinationValues[i].Split(':').GetValue(1).ToString().Equals(AssociatedFieldId) ? DestinationAttributes[i] + (count) : DestinationAttributes[i]) + ", ");
                    }
                    insertQuery.Append("), ");
                    count++;
                }
            }
            else
            {
                insertQuery.Append("(");
                for (int i = 0; i < DestinationAttributes.Length; i++)
                {
                    insertQuery.Append("@" + DestinationAttributes[i] + ", ");
                }
                insertQuery.Append("), ");
            }

            insertQuery.Replace(", )", ")");

            string qry = insertQuery.ToString().TrimEnd(new char[] { ',', ' ' });

            return qry;
        }

        private string PrepareUpdateStatement()
        {
            StringBuilder updateQuery = new StringBuilder();

            updateQuery.Append("UPDATE " + Entity + " SET " + DestinationAttributes[0] + " = @" + DestinationAttributes[0]);
            for (int i = 1; i < this.DestinationAttributes.Length; i++)
            {
                updateQuery.Append(", " + DestinationAttributes[i] + " = @" + DestinationAttributes[i]);
            }

            updateQuery.Append(" WHERE 1 = 1");

            foreach (string whereCols in WhereCols)
            {
                updateQuery.Append(" AND " + whereCols + " = @" + whereCols);
            }

            return updateQuery.ToString();
        }

        private SectionFields GetAssociatedField(string fieldId)
        {
            bool found = false;
            SectionFields field = null;

            foreach (var section in ParentForm.Sections)
            {
                foreach (var sectionField in section.Fields)
                {
                    if (sectionField.FieldId.Equals(fieldId))
                    {
                        field = sectionField;
                        found = true;
                    }
                    if (found)
                        break;
                }
                if (found)
                    break;
            }

            if (!found)
                throw new Exception("Field Not Found in the JSON supplied");
            return field;
        }

        private object GetComputableValue(string value)
        {
            if (value.Equals("guid"))
            {
                return Guid.NewGuid().ToString();
            }
            else if (value.Equals("currentdatetime"))
            {
                return DateTime.Now;
            }
            else
                return string.Empty;
        }
    }
}