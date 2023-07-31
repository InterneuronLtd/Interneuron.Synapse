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
﻿using Interneuron.Common.Extensions;
using System;
using System.Collections.Generic;

namespace SynapseStudioWeb.AppCode.Constants
{
    public class TerminologyConstants
    {
        public const string ROUTEFIELDTYPE_ADDITONAL_CD = "001";
        public const string ROUTEFIELDTYPE_UNLICENSED_CD = "002";
        public const string ROUTEFIELDTYPE_NORMAL_CD = "003";
        public const string ROUTEFIELDTYPE_DISCRETIONARY_CD = "004";

        public const string MANUAL_DATA_SOURCE = "RNOH";
        public const string DMD_DATA_SOURCE = "DMD";
        public const string DMD_DATA_SOURCE_DISPLAY = "DM+D";
        public const string FDB_DATA_SOURCE = "FDB";

        public const string MANUAL_RECORD_SOURCE = "M";
        public const string IMPORTED_RECORD_SOURCE = "I";


        public const string PRIMARY_IDENTIFICATION_CODE_TYPE = "DMD";
        public const string CUSTOM_IDENTIFICATION_CODE_TYPE = "Custom";

        public const string CLASSIFICATION_CODE_TYPE = "Classification";
        public const string IDENTIFICATION_CODE_TYPE = "Identification";

        public const string DRAFT_STATUS_CD = "001";
        public const string READY_FOR_REVIEW_STATUS_CD = "002";
        public const string ACTIVE_STATUS_CD = "003";
        public const string ARCHIEVED_STATUS_CD = "004";
        public const string INACTIVE_STATUS_CD = "005";
        public const string DELETED_STATUS_CD = "006";


        public const string DEFAULT_DROPDOWN_TEXT = "Please Select";

        public static readonly Dictionary<string, string> ColorForRecordSource = new Dictionary<string, string> { { "RNOH", "#fff3e0" }, { "FDB", "#ffebee" }, { "DMD", "#e0f2f1" } };

        public static Func<string, string> GetColorForRecordSource = (src) => (src.IsNotEmpty() && ColorForRecordSource.ContainsKey(src.ToUpper())) ? ColorForRecordSource[src.ToUpper()] : null;

        public static Func<string, string> GetMergeRuleDesc = (rule) => rule.IsNotEmpty()? Startup.StaticConfiguration[$"MMCMergeRules:{rule}"] : null;

        public static Func<string, string> GetFormularyRules = (rule) => rule.IsNotEmpty() ? Startup.StaticConfiguration[$"Formulary_Rules:{rule}"] : null;
    }
}
