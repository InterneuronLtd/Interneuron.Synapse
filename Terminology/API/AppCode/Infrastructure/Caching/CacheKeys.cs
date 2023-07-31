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
using Interneuron.Terminology.API.AppCode.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Infrastructure.Caching
{
    public class CacheKeys
    {
        public const string DMDAvailRestrictions = "dmdavailrestrictions";

        public const string DMDBasisOfName = "dmdbasisofname";

        public const string DMDControlDrugCategory = "dmdcontroldrugcategory";

        public const string DMDForm = "dmdform";

        public const string DMDLicensingAuthority = "dmdlicensingauthority";

        public const string DMDOntFormRoute = "dmdontformroute";

        public const string DMDPrescribingStatus = "dmdprescribingstatus";

        public const string DMDRoute = "dmdroute";

        public const string DMDSupplier = "dmdsupplier";

        public const string DMDUOM = "dmduom";

        public const string DMDDoseForm = "dmddoseform";

        public const string DMDIngredient = "dmdingredient";

        public const string DMDPharamceuticalStrength = "dmdpharamceuticalstrength";

        public const string LookupCommon = "lookupcommon";

        public const string ATCCode = "atccode";

        public const string DMD_ATC_Code = "dmdatccode";

        public const string BNFCode = "bnfcode";

        public const string DMD_BNF_Code = "dmdbnfcode";

        public const string SNOMED_TRADE_FAMILIES = "snomedtradefamilies";

        public const string SNOMED_MODIFIED_RELEASE = "snomedmodifiedrelease";

        public static List<string> GetAllCacheKeys()
        {
            Type type = typeof(CacheKeys);
            var flags = BindingFlags.Static | BindingFlags.Public;

            try
            {
                var fieldValsAsObj = type.GetFields(flags).Where(f => f.IsLiteral).Select(f => f.GetRawConstantValue());

                if (fieldValsAsObj.IsCollectionValid())
                    return fieldValsAsObj.Select(f => f.ToString()).ToList();// fieldNames.ToList();

            }
            catch { }

            return null;
        }
    }

    public static class CacheExtension
    {
        public static string GetCacheKeyByLookupTypeName(this LookupType lookupType)
        {
            switch (lookupType)
            {
                case LookupType.DMDAvailRestrictions:
                    return CacheKeys.DMDAvailRestrictions;
                case LookupType.DMDBasisOfName:
                    return CacheKeys.DMDBasisOfName;
                case LookupType.DMDControlDrugCategory:
                    return CacheKeys.DMDControlDrugCategory;
                case LookupType.DMDForm:
                    return CacheKeys.DMDForm;
                case LookupType.DMDLicensingAuthority:
                    return CacheKeys.DMDLicensingAuthority;
                case LookupType.DMDOntFormRoute:
                    return CacheKeys.DMDOntFormRoute;
                case LookupType.DMDPrescribingStatus:
                    return CacheKeys.DMDPrescribingStatus;
                case LookupType.DMDRoute:
                    return CacheKeys.DMDRoute;
                case LookupType.DMDSupplier:
                    return CacheKeys.DMDSupplier;
                case LookupType.DMDDoseForm:
                    return CacheKeys.DMDDoseForm;
                case LookupType.DMDUOM:
                    return CacheKeys.DMDUOM;
                case LookupType.DMDIngredient:
                    return CacheKeys.DMDIngredient;
                case LookupType.DMDPharamceuticalStrength:
                    return CacheKeys.DMDPharamceuticalStrength;
                case LookupType.ATCCode:
                    return CacheKeys.ATCCode;
                case LookupType.BNFCode:
                    return CacheKeys.BNFCode;
                case LookupType.FormularyMedicationType:
                case LookupType.FormularyRules:
                case LookupType.RecordStatus:
                case LookupType.FormularyStatus:
                case LookupType.OrderableStatus:
                case LookupType.TitrationType:
                case LookupType.Modifier:
                case LookupType.RoundingFactor:
                //case LookupType.DoseForm:
                //case LookupType.PharamceuticalStrength:
                case LookupType.ModifiedRelease:
                case LookupType.ProductType:
                case LookupType.ClassificationCodeType:
                case LookupType.IdentificationCodeType:
                case LookupType.OrderFormType:
                case LookupType.RouteFieldType:
                case LookupType.DrugClass:
                    return CacheKeys.LookupCommon;
                default:
                    return null;
            }
        }
    }
}
