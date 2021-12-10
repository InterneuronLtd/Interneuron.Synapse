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


﻿using Interneuron.Common.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SynapseStudioWeb.Models.MedicinalMgmt
{
    [Serializable]
    public class FormularyAdditionalCodeModel
    {
        public string FormularyVersionId { get; set; }
        public string AdditionalCode { get; set; }
        public string AdditionalCodeSystem { get; set; }
        public string AdditionalCodeDesc { get; set; }
        public string Attr1 { get; set; }
        public string MetaJson { get; set; }
        public string Source { get; set; }
        public string CodeType { get; set; }

        public string AdditionalProps
        {
            get
            {
                var addnlProps = new StringBuilder();

                if (AdditionalCodeDesc.IsNotEmpty())
                    addnlProps.Append($"Description: {AdditionalCodeDesc}<br/>");
                if (Source.IsNotEmpty())
                    addnlProps.Append($"Source: {Source}<br/>");

                if (MetaJson.IsNotEmpty())
                {
                    try
                    {
                        var metaDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(MetaJson);
                        metaDict.Each((kv) =>
                        {
                            addnlProps.Append($"{kv.Key}: {kv.Value}<br/>");
                        });

                    }
                    catch { }
                }

                return addnlProps.ToString();
            }
        }
    }
}



