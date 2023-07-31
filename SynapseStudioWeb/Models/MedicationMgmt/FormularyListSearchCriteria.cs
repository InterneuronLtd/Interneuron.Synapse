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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Models.MedicinalMgmt
{
    public class FormularyListSearchCriteria
    {
        public string SearchTerm { get; set; }
        public bool? HideArchived { get; set; }
        public string RecStatusCds { get; set; }
        public bool? Flags { get; set; }
        public string FormularyCode { get; set; }
        public static Dictionary<string, string> GetPossibleFlags()
        {
            return new Dictionary<string, string>
            {
                {"BlackTriangle", "Has Black Triangle" },
                {"BloodProduct", "Blood Product" },
                {"ClinicalTrialMedication", "Clinical Trial Medication" },
                {"CriticalDrug", "Critical Drug" },
                { "CFCFree","CFC Free" },
                {"CustomControlledDrug" , "Custom Controlled Drug" },

                {"Diluent", "Diluent" },

                {"EMAAdditionalMonitoring", "EMA Additional Monitoring" },
                {"ExpensiveMedication", "Expensive Medication" },

                {"GastroResistant", "Gastro Resistant" },
                { "GlutenFree", "Gluten Free" },

                {"HighAlertMedication", "High Alert Medication" },
                {"IgnoreDuplicateWarnings", "Ignore Duplicate Warnings" },
                {"IsIndicationMandatory", "Indication Mandatory" },

                {"IVtoOral", "IV to Oral" },
                {"ModifiedRelease", "Modified Release" },
                
                {"NotforPRN", "Not for PRN" },
                {"OutpatientMedication", "Outpatient Medication" },

                { "Prescribable", "Prescribable" },
                { "Parallelimport", "Parallel import" },
                {"PreservativeFree", "Preservative Free" },

                {"SugarFree" , "Sugar Free" },
                {"UnlicensedMedication", "Unlicensed Medication" },
                {"WitnessingRequired", "Witnessing Required" }
            };
        }

    }
}
