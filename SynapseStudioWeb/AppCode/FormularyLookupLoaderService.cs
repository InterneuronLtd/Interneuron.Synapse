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
using Microsoft.AspNetCore.Http;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.AppCode
{

    public class FormularyLookupLoaderService
    {
        private HttpContext _httpContext;

        public FormularyLookupLoaderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public async Task LoadLookUps()
        {
            //await Task.WhenAll(GetDrugClassLookup (),GetRoutesLookup(), GetFormAndRoutesLookup(), GetIngredientsLookup(), GetUOMsLookup(), GetSuppliersLookup(), GetFormCodesLookup(), GetBasisOfPharmaStrengthLookup(), GetRecordStatusLookup(), GetMedicationTypeLookup(), GetBasisOfPreferredName(), GetLicensingAuthority(), GetDoseForms(), GetRoundingFactor(), GetControlledDrugCategories(), GetMarkedModifiers(), GetModifiedReleases(), GetOrderableStatuses(), GetPrescribingStatuses(), GetRestrictionsOnAvailability(), GetTitrationType(), GetFormularyStatuses(), GetProductTypes(), GetClassificationCodeTypes(), GetOrderFormTypes());

            await Task.WhenAll(GetRoutesLookup(), GetIngredientsLookup(), GetUOMsLookup(), GetSuppliersLookup(), GetFormCodesLookup(), GetBasisOfPharmaStrengthLookup(), GetRecordStatusLookup(),  GetBasisOfPreferredName(), GetLicensingAuthority(), GetDoseForms(), GetRoundingFactor(), GetControlledDrugCategories(),  GetPrescribingStatuses(), GetRestrictionsOnAvailability(), GetTitrationType(), GetFormularyStatuses(), GetProductTypes(), GetClassificationCodeTypes());
        }

       

        public async Task<Dictionary<string, string>> GetRoutesLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.RoutesLkpKey);

            if (!dataDictionary.IsCollectionValid())
            {
                var dataLkp = await DataService.TerminologyAPIService.GetRoutes(token);

                if (dataLkp.IsCollectionValid())
                {
                    dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.RoutesLkpKey, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetLatestRoutesLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = new Dictionary<string, string>();

            var dataLkp = await DataService.TerminologyAPIService.GetRoutes(token);

            if (dataLkp.IsCollectionValid())
            {
                dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1 && (rec.IsLatest == true || rec.IsLatest.HasValue == false)).ToDictionary(k => k.Cd.ToString(), v => v.Desc);
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        //public async Task<Dictionary<string, string>> GetFormAndRoutesLookup(Action<Dictionary<string, string>> onResultFetch = null)
        //{
        //    string token = _httpContext.Session.GetString("access_token");

        //    var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.FormNRoutesLkpKey);

        //    if (!dataDictionary.IsCollectionValid())
        //    {
        //        var dataLkp = await DataService.TerminologyAPIService.GetFormAndRoutes(token);

        //        if (dataLkp.IsCollectionValid())
        //        {
        //            dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

        //            _httpContext.Session.SetObject(SynapseSession.FormNRoutesLkpKey, dataDictionary);
        //        }
        //    }

        //    dataDictionary = dataDictionary ?? new Dictionary<string, string>();

        //    onResultFetch?.Invoke(dataDictionary);
        //    return dataDictionary;
        //}

        public async Task<Dictionary<string, string>> GetIngredientsLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.IngredientsLkpKey);

            if (!dataDictionary.IsCollectionValid())
            {
                var dataLkp = await TerminologyAPIService.GetIngredients(token);

                if (dataLkp.IsCollectionValid())
                {
                    dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1 && !rec.Invalid.HasValue).ToDictionary(k => k.Isid.ToString(), v => v.Nm);

                    _httpContext.Session.SetObject(SynapseSession.IngredientsLkpKey, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetLatestIngredientsLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = new Dictionary<string, string>();

            var dataLkp = await TerminologyAPIService.GetIngredients(token);

            if (dataLkp.IsCollectionValid())
            {
                dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1 && !rec.Invalid.HasValue && (rec.IsLatest == true || rec.IsLatest.HasValue == false)).ToDictionary(k => k.Isid.ToString(), v => v.Nm);

            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetUOMsLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.UOMsLkpKey);

            if (!dataDictionary.IsCollectionValid())
            {
                var dataLkp = await TerminologyAPIService.GetUOMs(token);

                if (dataLkp.IsCollectionValid())
                {
                    dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.UOMsLkpKey, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetLatestUOMsLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = new Dictionary<string, string>();

            var dataLkp = await TerminologyAPIService.GetUOMs(token);

            if (dataLkp.IsCollectionValid())
            {
                dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1 && (rec.IsLatest == true || rec.IsLatest.HasValue == false)).ToDictionary(k => k.Cd.ToString(), v => v.Desc);
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetSuppliersLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.SupplierLkpKey);

            if (!dataDictionary.IsCollectionValid())
            {
                var dataLkp = await TerminologyAPIService.GetSuppliers(token);

                if (dataLkp.IsCollectionValid())
                {
                    dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.SupplierLkpKey, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetLatestSuppliersLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = new Dictionary<string, string>();

            var dataLkp = await TerminologyAPIService.GetSuppliers(token);

            if (dataLkp.IsCollectionValid())
            {
                dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1 && (rec.IsLatest == true || rec.IsLatest.HasValue == false)).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetFormCodesLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.FormsLkpKey);

            if (!dataDictionary.IsCollectionValid())
            {
                var dataLkp = await DataService.TerminologyAPIService.GetFormCodes(token);

                if (dataLkp.IsCollectionValid())
                {
                    dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.FormsLkpKey, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }


        public async Task<Dictionary<string, string>> GetLatestFormCodesLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = new Dictionary<string, string>();

            var dataLkp = await DataService.TerminologyAPIService.GetFormCodes(token);

            if (dataLkp.IsCollectionValid())
            {
                dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1 && (rec.IsLatest == true || rec.IsLatest.HasValue == false)).ToDictionary(k => k.Cd.ToString(), v => v.Desc);
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetBasisOfPharmaStrengthLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.BasisOfPharmaStrengthLkpKey);

            if (!dataDictionary.IsCollectionValid())
            {
                var dataLkp = await TerminologyAPIService.GetBasisOfPharmaStrength(token);

                if (dataLkp.IsCollectionValid())
                {
                    dataDictionary = dataLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.BasisOfPharmaStrengthLkpKey, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetRecordStatusLookup(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var recordStatusDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.FormularyRecStatusLkpKey);

            if (!recordStatusDictionary.IsCollectionValid())
            {
                var recordStatusLkp = await DataService.TerminologyAPIService.GetRecordStatusLookup(token);

                if (recordStatusLkp.IsCollectionValid())
                {
                    recordStatusDictionary = recordStatusLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.FormularyRecStatusLkpKey, recordStatusDictionary);
                }
            }

            recordStatusDictionary = recordStatusDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(recordStatusDictionary);
            return recordStatusDictionary;
        }

        //public async Task<Dictionary<string, string>> GetMedicationTypeLookup(Action<Dictionary<string, string>> onResultFetch = null)
        //{
        //    string token = _httpContext.Session.GetString("access_token");

        //    var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.FormularyMedicationTypeLkpKey);

        //    if (!dataDictionary.IsCollectionValid())
        //    {
        //        var recordStatusLkp = await TerminologyAPIService.GetMedicationTypes(token);

        //        if (recordStatusLkp.IsCollectionValid())
        //        {
        //            dataDictionary = recordStatusLkp.Where(rec => rec.recordstatus == 1).ToDictionary(k => k.cd.ToString(), v => v.desc);

        //            _httpContext.Session.SetObject(SynapseSession.FormularyMedicationTypeLkpKey, dataDictionary);
        //        }
        //    }

        //    dataDictionary = dataDictionary ?? new Dictionary<string, string>();

        //    onResultFetch?.Invoke(dataDictionary);
        //    return dataDictionary;
        //}

        public async Task<Dictionary<string, string>> GetBasisOfPreferredName(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.BasisOfPreferredName);

            if (!dataDictionary.IsCollectionValid())
            {
                var basisOfPreferredNameLkp = await TerminologyAPIService.GetBasisOfPreferredName(token);

                if (basisOfPreferredNameLkp.IsCollectionValid())
                {
                    dataDictionary = basisOfPreferredNameLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.BasisOfPreferredName, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetLicensingAuthority(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.LicensingAuthority);

            if (!dataDictionary.IsCollectionValid())
            {
                var licensingAuthorityLkp = await TerminologyAPIService.GetLicensingAuthority(token);

                if (licensingAuthorityLkp.IsCollectionValid())
                {
                    dataDictionary = licensingAuthorityLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.LicensingAuthority, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        //public async Task<Dictionary<string, string>> GetDrugClassLookup(Action<Dictionary<string, string>> onResultFetch = null)
        //{
        //    string token = _httpContext.Session.GetString("access_token");

        //    var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.FormularyGetDrugClassLookup);

        //    if (!dataDictionary.IsCollectionValid())
        //    {
        //        var recordStatusLkp = await TerminologyAPIService.GetDrugClassLookup(token);

        //        if (recordStatusLkp.IsCollectionValid())
        //        {
        //            dataDictionary = recordStatusLkp.Where(rec => rec.recordstatus == 1).ToDictionary(k => k.cd.ToString(), v => v.desc);

        //            _httpContext.Session.SetObject(SynapseSession.FormularyGetDrugClassLookup, dataDictionary);
        //        }
        //    }

        //    dataDictionary = dataDictionary ?? new Dictionary<string, string>();

        //    onResultFetch?.Invoke(dataDictionary);
        //    return dataDictionary;
        //}

        public async Task<Dictionary<string, string>> GetDoseForms(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.DoseForms);

            if (!dataDictionary.IsCollectionValid())
            {
                var doseFormsLkp = await TerminologyAPIService.GetDoseForms(token);

                if (doseFormsLkp.IsCollectionValid())
                {
                    dataDictionary = doseFormsLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.DoseForms, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetRoundingFactor(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.RoundingFactor);

            if (!dataDictionary.IsCollectionValid())
            {
                var roundingFactorLkp = await TerminologyAPIService.GetRoundingFactor(token);

                if (roundingFactorLkp.IsCollectionValid())
                {
                    dataDictionary = roundingFactorLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.RoundingFactor, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetControlledDrugCategories(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.ControlledDrugCategories);

            if (!dataDictionary.IsCollectionValid())
            {
                var controlledDrugCategoriesLkp = await TerminologyAPIService.GetControlledDrugCategories(token);

                if (controlledDrugCategoriesLkp.IsCollectionValid())
                {
                    dataDictionary = controlledDrugCategoriesLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.ControlledDrugCategories, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        //public async Task<Dictionary<string, string>> GetMarkedModifiers(Action<Dictionary<string, string>> onResultFetch = null)
        //{
        //    string token = _httpContext.Session.GetString("access_token");

        //    var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.MarkedModifiers);

        //    if (!dataDictionary.IsCollectionValid())
        //    {
        //        var markedModifiersLkp = await TerminologyAPIService.GetMarkedModifiers(token);

        //        if (markedModifiersLkp.IsCollectionValid())
        //        {
        //            dataDictionary = markedModifiersLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

        //            _httpContext.Session.SetObject(SynapseSession.MarkedModifiers, dataDictionary);
        //        }
        //    }

        //    dataDictionary = dataDictionary ?? new Dictionary<string, string>();

        //    onResultFetch?.Invoke(dataDictionary);
        //    return dataDictionary;
        //}

        //public async Task<Dictionary<string, string>> GetModifiedReleases(Action<Dictionary<string, string>> onResultFetch = null)
        //{
        //    string token = _httpContext.Session.GetString("access_token");

        //    var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.ModifiedReleases);

        //    if (!dataDictionary.IsCollectionValid())
        //    {
        //        var modifiedReleasesLkp = await TerminologyAPIService.GetModifiedReleases(token);

        //        if (modifiedReleasesLkp.IsCollectionValid())
        //        {
        //            dataDictionary = modifiedReleasesLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

        //            _httpContext.Session.SetObject(SynapseSession.ModifiedReleases, dataDictionary);
        //        }
        //    }

        //    dataDictionary = dataDictionary ?? new Dictionary<string, string>();

        //    onResultFetch?.Invoke(dataDictionary);
        //    return dataDictionary;
        //}

        //public async Task<Dictionary<string, string>> GetOrderableStatuses(Action<Dictionary<string, string>> onResultFetch = null)
        //{
        //    string token = _httpContext.Session.GetString("access_token");

        //    var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.OrderableStatuses);

        //    if (!dataDictionary.IsCollectionValid())
        //    {
        //        var orderableStatusesLkp = await TerminologyAPIService.GetOrderableStatuses(token);

        //        if (orderableStatusesLkp.IsCollectionValid())
        //        {
        //            dataDictionary = orderableStatusesLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

        //            _httpContext.Session.SetObject(SynapseSession.OrderableStatuses, dataDictionary);
        //        }
        //    }

        //    dataDictionary = dataDictionary ?? new Dictionary<string, string>();

        //    onResultFetch?.Invoke(dataDictionary);
        //    return dataDictionary;
        //}

        public async Task<Dictionary<string, string>> GetPrescribingStatuses(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.PrescribingStatuses);

            if (!dataDictionary.IsCollectionValid())
            {
                var prescribingStatusesLkp = await TerminologyAPIService.GetPrescribingStatuses(token);

                if (prescribingStatusesLkp.IsCollectionValid())
                {
                    dataDictionary = prescribingStatusesLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.PrescribingStatuses, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetRestrictionsOnAvailability(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.RestrictionsOnAvailability);

            if (!dataDictionary.IsCollectionValid())
            {
                var restrictionsOnAvailabilityLkp = await TerminologyAPIService.GetRestrictionsOnAvailability(token);

                if (restrictionsOnAvailabilityLkp.IsCollectionValid())
                {
                    dataDictionary = restrictionsOnAvailabilityLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.RestrictionsOnAvailability, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetTitrationType(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.TitrationType);

            if (!dataDictionary.IsCollectionValid())
            {
                var titrationTypeLkp = await TerminologyAPIService.GetTitrationType(token);

                if (titrationTypeLkp.IsCollectionValid())
                {
                    dataDictionary = titrationTypeLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.TitrationType, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetFormularyStatuses(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.FormularyStatuses);

            if (!dataDictionary.IsCollectionValid())
            {
                var formularyStatusesLkp = await TerminologyAPIService.GetFormularyStatuses(token);

                if (formularyStatusesLkp.IsCollectionValid())
                {
                    dataDictionary = formularyStatusesLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.FormularyStatuses, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetProductTypes(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.ProductTypes);

            if (!dataDictionary.IsCollectionValid())
            {
                var formularyStatusesLkp = await TerminologyAPIService.GetProductTypes(token);

                if (formularyStatusesLkp.IsCollectionValid())
                {
                    dataDictionary = formularyStatusesLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.ProductTypes, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetClassificationCodeTypes(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.ClassificationCodeTypes);

            if (!dataDictionary.IsCollectionValid())
            {
                var codeSystemsLkp = await TerminologyAPIService.GetClassificationCodeTypesLookup(token);

                if (codeSystemsLkp.IsCollectionValid())
                {
                    dataDictionary = codeSystemsLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.ClassificationCodeTypes, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }

        public async Task<Dictionary<string, string>> GetIdentificationCodeTypes(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.IdentificationCodeTypes);

            if (!dataDictionary.IsCollectionValid())
            {
                var codeSystemsLkp = await TerminologyAPIService.GetIdentificationCodeTypesLookup(token);

                if (codeSystemsLkp.IsCollectionValid())
                {
                    dataDictionary = codeSystemsLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.IdentificationCodeTypes, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }
        

        //public async Task<Dictionary<string, string>> GetOrderFormTypes(Action<Dictionary<string, string>> onResultFetch = null)
        //{
        //    string token = _httpContext.Session.GetString("access_token");

        //    var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.OrderFormTypes);

        //    if (!dataDictionary.IsCollectionValid())
        //    {
        //        var orderFormTypesLkp = await TerminologyAPIService.GetOrderFormTypes(token);

        //        if (orderFormTypesLkp.IsCollectionValid())
        //        {
        //            dataDictionary = orderFormTypesLkp.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

        //            _httpContext.Session.SetObject(SynapseSession.OrderFormTypes, dataDictionary);
        //        }
        //    }

        //    dataDictionary = dataDictionary ?? new Dictionary<string, string>();

        //    onResultFetch?.Invoke(dataDictionary);
        //    return dataDictionary;
        //}

        public async Task<Dictionary<string, string>> GetBasisOfPharmaStrengths(Action<Dictionary<string, string>> onResultFetch = null)
        {
            string token = _httpContext.Session.GetString("access_token");

            var dataDictionary = _httpContext.Session.GetObject<Dictionary<string, string>>(SynapseSession.BasisOfPharmaStrengths);

            if (!dataDictionary.IsCollectionValid())
            {
                var basisOfPharmaStrengths = await TerminologyAPIService.GetBasisOfPharmaStrength(token);

                if (basisOfPharmaStrengths.IsCollectionValid())
                {
                    dataDictionary = basisOfPharmaStrengths.Where(rec => rec.Recordstatus == 1).ToDictionary(k => k.Cd.ToString(), v => v.Desc);

                    _httpContext.Session.SetObject(SynapseSession.BasisOfPharmaStrengths, dataDictionary);
                }
            }

            dataDictionary = dataDictionary ?? new Dictionary<string, string>();

            onResultFetch?.Invoke(dataDictionary);
            return dataDictionary;
        }
    }
}
