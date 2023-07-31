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


﻿using Interneuron.Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SynapseStudioWeb.AppCode.Constants;
using SynapseStudioWeb.AppCode.Middleware;
using SynapseStudioWeb.Models.MedicationMgmt;
using SynapseStudioWeb.Models.MedicinalMgmt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Helpers
{
    public static class Extensions
    {
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), row[property.Name].ToString());
                    property.SetValue(item, day, null);
                }
                else
                {
                    if (row[property.Name] == DBNull.Value)
                        property.SetValue(item, null, null);
                    else
                        property.SetValue(item, row[property.Name], null);
                }
            }
            return item;
        }

        public static void SetObject(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        public static CodeNameSelectorModel ConvertToCodeNameModel<T>(this T data, Func<T, string> GetId, Func<T, string> GetName = null)
        {
            if (data == null) return null;

            var id = GetId(data);
            var name = GetName != null ? GetName(data) : id;

            if (id == null) return null;

            var model = new CodeNameSelectorModel
            {
                Id = id,
                Name = name
            };
            model.SourceColor = (model.Source.IsNotEmpty() && TerminologyConstants.ColorForRecordSource.ContainsKey(model.Source.ToUpper())) ? TerminologyConstants.ColorForRecordSource[model.Source.ToUpper()] : string.Empty;

            return model;
        }
        public static CodeNameSelectorModel ConvertToCodeNameModel<T>(this T data, Func<T, string> GetId, Func<T, string> GetName = null, Func<T, string> GetSource = null, Func<T, bool> GetReadOnly = null)
        {
            if (data == null) return null;

            var id = GetId(data);
            var name = GetName != null ? GetName(data) : id;

            if (id == null) return null;

            var srcOfData = GetSource?.Invoke(data);

            var model = new CodeNameSelectorModel
            {
                Id = id,
                Name = name,
                Source = srcOfData,
                IsReadonly = GetReadOnly != null ? GetReadOnly(data) : (srcOfData.IsEmpty()) ? false : (string.Compare(srcOfData, TerminologyConstants.MANUAL_DATA_SOURCE, true) != 0)
            };

            model.SourceColor = (model.Source.IsNotEmpty() && TerminologyConstants.ColorForRecordSource.ContainsKey(model.Source.ToUpper())) ? TerminologyConstants.ColorForRecordSource[model.Source.ToUpper()] : string.Empty;
            return model;
        }


        public static List<CodeNameSelectorModel> ConvertToCodeNameModel<T>(this List<T> data, Func<T, string> GetId, Func<T, string> GetName = null, Func<T, string> GetDataSource = null, Func<T, bool> GetReadOnly = null)
        {
            var list = new List<CodeNameSelectorModel>();

            if (data == null || data.Count == 0) return list;

            foreach (var item in data)
            {
                var id = GetId(item);
                var name = GetName != null ? GetName(item) : id;
                var dataSource = GetDataSource?.Invoke(item);

                if (id.IsEmpty()) continue;

                list.Add(new CodeNameSelectorModel
                {
                    Id = id,
                    Name = name,
                    Source = dataSource,
                    SourceColor = dataSource.IsNotEmpty() ? (TerminologyConstants.ColorForRecordSource.ContainsKey(dataSource.ToUpper()) ? TerminologyConstants.ColorForRecordSource[dataSource.ToUpper()] : null) : null,
                    IsReadonly = GetReadOnly != null ? GetReadOnly(item) : (dataSource.IsEmpty()) ? false : (string.Compare(dataSource, TerminologyConstants.MANUAL_DATA_SOURCE, true) != 0)
                });
            }
            return list;
        }



        public static void SafeAssignCodeNameFromSession(this CodeNameSelectorModel codeNameModel, string sessionKey, HttpContext httpContext)
        {
            if (codeNameModel == null || codeNameModel.Id == null || httpContext == null || sessionKey.IsEmpty()) return;
            var lkp = httpContext.Session.GetObject<Dictionary<string, string>>(sessionKey);
            codeNameModel.Name = (lkp.IsCollectionValid() && lkp.ContainsKey(codeNameModel.Id)) ? lkp[codeNameModel.Id] : codeNameModel.Name;
        }

        public static string SerializeListToJsonArray(this IList list)
        {
            if (list == null || list.Count == 0) return "[]";

            return JsonConvert.SerializeObject(list);
        }

        public static string SerializeCodeNameListToJsonArray(this IList<CodeNameSelectorModel> codeNameList)
        {
            var list = new List<CodeNameSelectorModel>();

            if (codeNameList == null || codeNameList.Count == 0) return JsonConvert.SerializeObject(list);

            return JsonConvert.SerializeObject(codeNameList);
        }

        public static string SerializeCodeNameItemToJsonArray(this CodeNameSelectorModel codeName)
        {
            var list = new List<CodeNameSelectorModel>();

            if (codeName == null) return JsonConvert.SerializeObject(list);

            list.Add(codeName);
            return JsonConvert.SerializeObject(list);
        }

        public static SelectList ToSelectList(this Dictionary<string, string> keyValuePairs, string defaultText, string selected = "", bool ignorePresentStatus = false)
        {
            var list = new List<SelectListItem>();

            if (defaultText.IsNotEmpty())
            {
                if(selected.IsEmpty())
                {
                    list.Insert(0, new SelectListItem() { Value = "", Text = defaultText.Trim(), Selected = true });
                }
                else
                {
                    list.Insert(0, new SelectListItem() { Value = "", Text = defaultText.Trim() });
                }
            }

            if (!keyValuePairs.IsCollectionValid()) return null;

            keyValuePairs.Keys.Each(k =>
            {
                if (ignorePresentStatus)
                {
                    if (selected.IsEmpty() || (selected != k))
                    {
                        list.Add(new SelectListItem()
                        {
                            Text = keyValuePairs[k],
                            Value = k,
                            Selected = (selected.IsNotEmpty() && selected.Trim() == k)
                        });
                    }
                }
                else
                {
                    list.Add(new SelectListItem()
                    {
                        Text = keyValuePairs[k],
                        Value = k,
                        Selected = (selected.IsNotEmpty() && selected.Trim() == k)
                    });
                }
            });

            return new SelectList(list, "Value", "Text", selected);
        }

        public static string SerializeAdditionalCodeToJsonArray(this IList<FormularyAdditionalCodeModel> codeNameList)
        {
            var list = new List<FormularyAdditionalCodeModel>();

            if (codeNameList == null || codeNameList.Count == 0) return JsonConvert.SerializeObject(list);

            return JsonConvert.SerializeObject(codeNameList);
        }

        public static string SerializeIngredientsToJsonArray(this IList<FormularyIngredientModel> codeNameList)
        {
            var list = new List<FormularyIngredientModel>();

            if (codeNameList == null || codeNameList.Count == 0) return JsonConvert.SerializeObject(list);

            return JsonConvert.SerializeObject(codeNameList);
        }

        public static string SerializeExcipientsToJsonArray(this IList<FormularyExcipientModel> codeNameList)
        {
            var list = new List<FormularyExcipientModel>();

            if (codeNameList == null || codeNameList.Count == 0) return JsonConvert.SerializeObject(list);

            return JsonConvert.SerializeObject(codeNameList);
        }

        public static IApplicationBuilder UseMaximumRequestTimeout(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<MaximumRequestTimeoutMiddleware>();

            return builder;
        }

        public static string MakeUrl(string input)
        {
            Regex re = new Regex(@"(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?");
            MatchCollection mc = re.Matches(input);

            foreach (Match m in mc)
            {
                input = input.Replace(m.Value, "<a href=" + m.Value + ">" + m.Value + "</a>");
            }

            return input;
        }

        public static Nullable<T> ToNullable<T>(this string s) where T : struct
        {
            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;
        }
    }
}
