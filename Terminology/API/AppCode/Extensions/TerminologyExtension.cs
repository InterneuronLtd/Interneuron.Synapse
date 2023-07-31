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


﻿using Interneuron.Terminology.API.AppCode.Queries;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Interneuron.Terminology.API.AppCode.Extensions
{
    public static class TerminologyExtension
    {
        public static Dictionary<string, int> DMDCodeLogicalLevelsMapping = new Dictionary<string, int>() { { "VTM", 1 }, { "VMP", 2 }, { "AMP", 3 } };

        public static Dictionary<int, string> DMDLogicalCodeLevelsMapping = new Dictionary<int, string>() { { 1, "VTM" }, { 2, "VMP" }, { 3, "AMP" } };

        public static string GetDMDLevelCodeByLogicalLevel(this int? logicalLevel)
        {
            if (!logicalLevel.HasValue || !DMDLogicalCodeLevelsMapping.ContainsKey(logicalLevel.Value)) return null;

            return DMDLogicalCodeLevelsMapping[logicalLevel.Value];
        }

        public static string GetDMDParentLevelCodeByLogicalLevel(this int? logicalLevel)
        {
            if (!logicalLevel.HasValue || !DMDLogicalCodeLevelsMapping.ContainsKey(logicalLevel.Value)) return null;

            if (logicalLevel == 1) return "";
            if (logicalLevel == 2) return "VTM";
            else return "VMP";
        }


        public static int? GetDMDLogicalLevelByLevelCode(this string levelCode)
        {
            if (!DMDCodeLogicalLevelsMapping.ContainsKey(levelCode)) return null;

            return DMDCodeLogicalLevelsMapping[levelCode.ToUpper()];
        }


        public static string GetTypeName(this LookupType lookupType)
        {
            switch (lookupType)
            {
                case LookupType.FormularyMedicationType:
                    return "MedicationType";
                case LookupType.FormularyRules:
                    return "Rules";
                case LookupType.RecordStatus:
                    return "RecordStatus";
                case LookupType.FormularyStatus:
                    return "FormularyStatus";
                case LookupType.OrderableStatus:
                    return "OrderableStatus";
                case LookupType.TitrationType:
                    return "TitrationType";
                case LookupType.RoundingFactor:
                    return "RoundingFactor";
                case LookupType.Modifier:
                    return "Modifier";
                //case LookupType.DMDDoseForm:
                //    return "DoseForm";
                //case LookupType.DMDPharamceuticalStrength:
                //    return "PharamceuticalStrength";
                case LookupType.ModifiedRelease:
                    return "ModifiedRelease";
                case LookupType.ProductType:
                    return "ProductType";
                case LookupType.ClassificationCodeType:
                    return "ClassificationCodeType";
                case LookupType.IdentificationCodeType:
                    return "IdentificationCodeType";
                case LookupType.OrderFormType:
                    return "OrderFormType";
                case LookupType.RouteFieldType:
                    return "RouteFieldType";
                case LookupType.DrugClass:
                    return "DrugClass";
            }

            return "";
        }

    }
}
