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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using NHapi.Model.V21.Datatype;

namespace SynapseStudioWeb.Helpers
{
    public class SynapseSession
    {
        private const string userID = "userID";
        private const string fullName = "fullName";
        private const string entityId = "entityId";
        private const string listId = "listId";
        private const string baseviewId = "baseviewId";
        private const string isPharamacist = "isPharamacist";
        public static string UserID
        {
            get { return userID; }
        }
        public static string FullName
        {
            get { return fullName; }
        }
        public static string EntityId
        {
            get { return entityId; }
        }
        public static string ListId
        {
            get { return listId; }
        }
        public static string BaseViewId
        {
            get { return baseviewId; }
        }

        public static string IsPharamacist
        {
            get { return isPharamacist; }
        }

        public const string UserClaims = "UserClaims";

        public static string FormsLkpKey { get; } = "FormsLkpKey";

        public static string RoutesLkpKey { get; } = "RoutesLkpKey";

        public static string FormNRoutesLkpKey { get; } = "FormNRoutesLkpKey";

        public static string IngredientsLkpKey { get; } = "IngredientsLkpKey";

        public static string UOMsLkpKey { get; } = "UOMsLkpKey";

        public static string SupplierLkpKey { get; } = "SupplierLkpKey";

        public static string BasisOfPharmaStrengthLkpKey { get; } = "BasisOfPharmaStrengthLkpKey";


        public static string IsFormularyLookupExists { get; } = "IsFormularyLookupExists";
        public static string FormularyRecStatusLkpKey { get; } = "FormularyRecStatusLkpKey";
        public static string FormularySearchResults { get; } = "FormularySearchResults";

        public static string FormularyStatusLkpKey { get; } = "FormularyStatusLkpKey";

        public static string FormularyMedicationTypeLkpKey { get; } = "FormularyMedicationTypeLkpKey";
        public static string FormularyGetDrugClassLookup { get; } = "FormularyGetDrugClassLookup";

        public static string BasisOfPreferredName { get; } = "BasisOfPreferredName";

        public static string LicensingAuthority { get; } = "GetLicensingAuthority";

        public static string DoseForms { get; } = "GetDoseForms";

        public static string RoundingFactor { get; } = "GetRoundingFactor";

        public static string ControlledDrugCategories { get; } = "GetControlledDrugCategories";

        public static string MarkedModifiers { get; } = "MarkedModifiers";

        public static string ModifiedReleases { get; } = "ModifiedReleases";

        public static string OrderableStatuses { get; } = "OrderableStatuses";

        public static string PrescribingStatuses { get; } = "PrescribingStatuses";

        public static string RestrictionsOnAvailability { get; } = "RestrictionsOnAvailability";

        public static string TitrationType { get; } = "TitrationType";

        public static string FormularyStatuses { get; } = "FormularyStatuses";

        public static string ProductTypes { get; } = "ProductTypes";

        public static string ClassificationCodeTypes { get; } = "ClassificationCodeTypes";

        public static string IdentificationCodeTypes { get; } = "IdentificationCodeTypes";

        public static string OrderFormTypes { get; } = "OrderFormTypes";

        public static string BasisOfPharmaStrengths { get; } = "BasisOfPharmaStrengths";
    }
}
