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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SynapseStudioWeb.AppCode.Constants;
using SynapseStudioWeb.DataService;
using SynapseStudioWeb.DataService.APIModel;
using SynapseStudioWeb.Models.MedicinalMgmt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Controllers
{
    public partial class FormularyController : Controller
    {
        [HttpGet]
        [Route("Formulary/GetRoutes")]
        public async Task<JsonResult> GetRoutes()
        {
            var routesInSession = await _formularyLookupService.GetRoutesLookup();
            if (!routesInSession.IsCollectionValid())
            {
                string token = HttpContext.Session.GetString("access_token");
                var routes = await TerminologyAPIService.GetRoutes(token);
                return Json(routes);
            }
            var routesAsLkp = routesInSession.Select(rec => new RouteLookup { Cd = rec.Key, Desc = rec.Value });
            return Json(routesAsLkp);
        }

        //[HttpGet]
        //[Route("Formulary/GetFormAndRoutes")]
        //public async Task<JsonResult> GetFormAndRoutes()
        //{
        //    string token = HttpContext.Session.GetString("access_token");

        //    var formNRoutes = await TerminologyAPIService.GetFormAndRoutes(token);

        //    return Json(formNRoutes);
        //}

        [HttpGet]
        [Route("Formulary/SearchIngredients")]
        public async Task<JsonResult> SearchIngredients(string q)
        {
            var ingredients = await _formularyLookupService.GetIngredientsLookup();

            if (!ingredients.IsCollectionValid())
            {
                return Json(null);
            }

            var matchingIngredients = ingredients.Where((kv) => kv.Value.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            return Json(matchingIngredients);
        }

        [HttpGet]
        [Route("Formulary/SearchLatestIngredients")]
        public async Task<JsonResult> SearchLatestIngredients(string q)
        {
            var ingredients = await _formularyLookupService.GetLatestIngredientsLookup();

            if (!ingredients.IsCollectionValid())
            {
                return Json(null);
            }

            var matchingIngredients = ingredients.Where((kv) => kv.Value.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            return Json(matchingIngredients);
        }

        [HttpGet]
        [Route("Formulary/SearchAMPByName")]
        public async Task<IActionResult> SearchAMPByName(string q)
        {
            const string AMP_TYPE = "AMP";

            string token = HttpContext.Session.GetString("access_token");

            var response = await TerminologyAPIService.GetLatestFormulariesHeaderOnlyByNameOrCode(token, q, AMP_TYPE, false);

            if (response.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                _toastNotification.AddErrorToastMessage(string.Join('\n', response.ErrorMessages));

                return Json(null);
            }

            var formularies = response.Data;

            if (!formularies.IsCollectionValid()) return Json(null);

            var matchingStartsWith = formularies
               .Where(rec => rec.Name.StartsWith(q, StringComparison.OrdinalIgnoreCase))
               .Select(rec =>
               {
                   return new CodeNameSelectorModel { Id = rec.FormularyVersionId, Name = $"({rec.Code})-{rec.Name}", Source = TerminologyConstants.DMD_DATA_SOURCE, IsReadonly = false };
               }).ToList();

            var matchingContains = formularies
               .Where(rec => rec.Name.Contains(q, StringComparison.OrdinalIgnoreCase))
               .Select(rec =>
               {
                   return new CodeNameSelectorModel { Id = rec.FormularyVersionId, Name = $"({rec.Code})-{rec.Name}", Source = TerminologyConstants.DMD_DATA_SOURCE, IsReadonly = false };
               }).ToList();

            if (!matchingStartsWith.IsCollectionValid()) return Json(matchingContains);
            if (!matchingContains.IsCollectionValid()) return Json(matchingStartsWith);

            matchingStartsWith.AddRange(matchingContains);
            matchingStartsWith = matchingStartsWith.Distinct(rec => rec.Id).ToList();

            return Json(matchingStartsWith);
        }

        [HttpGet]
        [Route("Formulary/SearchDiluentsByName")]
        public async Task<IActionResult> SearchDiluentByName(string q)
        {
            string token = HttpContext.Session.GetString("access_token");

            var response = await TerminologyAPIService.GetFormulariesAsDiluents(token);

            if (response.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                _toastNotification.AddErrorToastMessage(string.Join('\n', response.ErrorMessages));

                return Json(null);
            }

            var formularies = response.Data;

            if (!formularies.IsCollectionValid()) return Json(null);

            var matchingStartsWith = formularies
                .Where(rec => rec.Name.StartsWith(q, StringComparison.OrdinalIgnoreCase))
                .Select(rec =>
            {
                return new CodeNameSelectorModel { Id = rec.Code, Name = $"({rec.Code})-{rec.Name}", Source = TerminologyConstants.MANUAL_DATA_SOURCE, IsReadonly = false };
            }).ToList();

            var matchingContains = formularies
               .Where(rec => rec.Name.Contains(q, StringComparison.OrdinalIgnoreCase))
               .Select(rec =>
               {
                   return new CodeNameSelectorModel { Id = rec.Code, Name = $"({rec.Code})-{rec.Name}", Source = TerminologyConstants.MANUAL_DATA_SOURCE, IsReadonly = false };
               }).ToList();

            if (!matchingStartsWith.IsCollectionValid()) return Json(matchingContains);
            if (!matchingContains.IsCollectionValid()) return Json(matchingStartsWith);

            matchingStartsWith.AddRange(matchingContains);
            matchingStartsWith = matchingStartsWith.Distinct(rec => rec.Id).ToList();

            return Json(matchingStartsWith);
        }

        [HttpGet]
        [Route("Formulary/SearchFormularyByName")]
        public async Task<IActionResult> SearchFormularyByName(string q)
        {
            string token = HttpContext.Session.GetString("access_token");

            var filterCriteria = new FormularyFilterCriteriaAPIModel()
            {
                searchTerm = q,
                hideArchived = false,
                showOnlyDuplicate = false
            };

            var response = await TerminologyAPIService.SearchFormulariesAsList(filterCriteria, token);

            if (response.StatusCode != DataService.APIModel.StatusCode.Success)
            {
                _toastNotification.AddErrorToastMessage(string.Join('\n', response.ErrorMessages));

                return Json(null);
            }

            var formularies = response.Data;

            if (!formularies.IsCollectionValid()) return Json(null);

            return Json(formularies);
        }

        [HttpGet]
        [Route("Formulary/GetUOMs")]
        public async Task<JsonResult> GetUOMs()
        {
            string token = HttpContext.Session.GetString("access_token");

            var uoms = await TerminologyAPIService.GetUOMs(token);

            if (!uoms.IsCollectionValid())
            {
                return Json(null);
            }

            var uomsTxt = uoms.Select(u => new { Cd = u.Cd.ToString(), Desc = u.Desc });

            return Json(uomsTxt);
        }

        [HttpGet]
        [Route("Formulary/SearchUOMs")]
        public async Task<JsonResult> SearchUOMs(string q)
        {
            var uoms = await _formularyLookupService.GetUOMsLookup();

            if (!uoms.IsCollectionValid())
            {
                return Json(null);
            }

            var matchingStartsWith = uoms.Where((kv) => kv.Value.StartsWith(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            var matchingContains = uoms.Where((kv) => kv.Value.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            if (!matchingStartsWith.IsCollectionValid()) return Json(matchingContains);
            if (!matchingContains.IsCollectionValid()) return Json(matchingStartsWith);

            matchingStartsWith.AddRange(matchingContains);
            matchingStartsWith = matchingStartsWith.Distinct(rec => rec.id).ToList();

            return Json(matchingStartsWith);
        }

        [HttpGet]
        [Route("Formulary/SearchLatestUOMs")]
        public async Task<JsonResult> SearchLatestUOMs(string q)
        {
            var uoms = await _formularyLookupService.GetLatestUOMsLookup();

            if (!uoms.IsCollectionValid())
            {
                return Json(null);
            }

            var matchingStartsWith = uoms.Where((kv) => kv.Value.StartsWith(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            var matchingContains = uoms.Where((kv) => kv.Value.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            if (!matchingStartsWith.IsCollectionValid()) return Json(matchingContains);
            if (!matchingContains.IsCollectionValid()) return Json(matchingStartsWith);

            matchingStartsWith.AddRange(matchingContains);
            matchingStartsWith = matchingStartsWith.Distinct(rec => rec.id).ToList();

            return Json(matchingStartsWith);
        }

        [HttpGet]
        [Route("Formulary/SearchSuppliers")]
        public async Task<JsonResult> SearchSuppliers(string q)
        {
            var uoms = await _formularyLookupService.GetSuppliersLookup();

            if (!uoms.IsCollectionValid())
            {
                return Json(null);
            }

            var matchingStartsWith = uoms.Where((kv) => kv.Value.StartsWith(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            var matchingContains = uoms.Where((kv) => kv.Value.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            if (!matchingStartsWith.IsCollectionValid()) return Json(matchingContains);
            if (!matchingContains.IsCollectionValid()) return Json(matchingStartsWith);

            matchingStartsWith.AddRange(matchingContains);
            matchingStartsWith = matchingStartsWith.Distinct(rec => rec.id).ToList();

            return Json(matchingStartsWith);
        }

        [HttpGet]
        [Route("Formulary/SearchLatestSuppliers")]
        public async Task<JsonResult> SearchLatestSuppliers(string q)
        {
            var uoms = await _formularyLookupService.GetLatestSuppliersLookup();

            if (!uoms.IsCollectionValid())
            {
                return Json(null);
            }

            var matchingStartsWith = uoms.Where((kv) => kv.Value.StartsWith(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            var matchingContains = uoms.Where((kv) => kv.Value.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            if (!matchingStartsWith.IsCollectionValid()) return Json(matchingContains);
            if (!matchingContains.IsCollectionValid()) return Json(matchingStartsWith);

            matchingStartsWith.AddRange(matchingContains);
            matchingStartsWith = matchingStartsWith.Distinct(rec => rec.id).ToList();

            return Json(matchingStartsWith);
        }

        [HttpGet]
        [Route("Formulary/SearchFormulations")]
        public async Task<JsonResult> SearchFormulations(string q)
        {
            var forms = await _formularyLookupService.GetFormCodesLookup();

            if (!forms.IsCollectionValid()) return Json(null);

            var matchingFormsStartsWith = forms.Where((kv) => kv.Value.StartsWith(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();
            var matchingFormsContains = forms.Where((kv) => kv.Value.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            if (!matchingFormsStartsWith.IsCollectionValid()) return Json(matchingFormsContains);
            if (!matchingFormsContains.IsCollectionValid()) return Json(matchingFormsStartsWith);

            matchingFormsStartsWith.AddRange(matchingFormsContains);
            matchingFormsStartsWith = matchingFormsStartsWith.Distinct(rec => rec.id).ToList();
            return Json(matchingFormsStartsWith);
        }

        [HttpGet]
        [Route("Formulary/SearchLatestFormulations")]
        public async Task<JsonResult> SearchLatestFormulations(string q)
        {
            var forms = await _formularyLookupService.GetLatestFormCodesLookup();

            if (!forms.IsCollectionValid()) return Json(null);

            var matchingFormsStartsWith = forms.Where((kv) => kv.Value.StartsWith(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();
            var matchingFormsContains = forms.Where((kv) => kv.Value.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            if (!matchingFormsStartsWith.IsCollectionValid()) return Json(matchingFormsContains);
            if (!matchingFormsContains.IsCollectionValid()) return Json(matchingFormsStartsWith);

            matchingFormsStartsWith.AddRange(matchingFormsContains);
            matchingFormsStartsWith = matchingFormsStartsWith.Distinct(rec => rec.id).ToList();
            return Json(matchingFormsStartsWith);
        }

        [HttpGet]
        [Route("Formulary/SearchControlledDrugCategories")]
        public async Task<JsonResult> SearchControlledDrugCategories(string q)
        {
            var catgs = await _formularyLookupService.GetControlledDrugCategories();

            if (!catgs.IsCollectionValid())
            {
                return Json(null);
            }

            var matchingData = catgs.Where((kv) => kv.Value.Contains(q, StringComparison.OrdinalIgnoreCase)).Select(kv => new { id = kv.Key, name = kv.Value }).ToList();

            return Json(matchingData);
        }

        [HttpGet]
        [Route("Formulary/GetSuppliers")]
        public async Task<JsonResult> GetSuppliers()
        {
            string token = HttpContext.Session.GetString("access_token");

            var suppliers = await TerminologyAPIService.GetSuppliers(token);

            if (!suppliers.IsCollectionValid())
            {
                return Json(null);
            }
            var textSup = suppliers.Select(s => new { Cd = s.Cd.ToString(), Desc = s.Desc });
            ;
            return Json(textSup);
        }

        [HttpGet]
        [Route("Formulary/GetFormCodes")]
        public async Task<JsonResult> GetFormCodes()
        {
            string token = HttpContext.Session.GetString("access_token");

            var formCodes = await TerminologyAPIService.GetFormCodes(token);

            if (!formCodes.IsCollectionValid())
            {
                return Json(null);
            }
            var formCodesTxt = formCodes.Select(rec => new CodeNameSelectorModel { Id = rec.Cd, Name = rec.Desc, IsReadonly = false, Source = TerminologyConstants.MANUAL_DATA_SOURCE, SourceColor = TerminologyConstants.GetColorForRecordSource(TerminologyConstants.MANUAL_DATA_SOURCE) });

            return Json(formCodesTxt);
        }

        [HttpGet]
        [Route("Formulary/SearchRoutes")]
        public async Task<JsonResult> SearchRoutes(string q)
        {
            if (q.IsEmpty()) return Json(null);

            var routesInSession = await _formularyLookupService.GetRoutesLookup();

            if (!routesInSession.IsCollectionValid())
            {
                string token = HttpContext.Session.GetString("access_token");
                var routes = await TerminologyAPIService.GetRoutes(token);

                if (!routes.IsCollectionValid())
                    return Json(null);

                var matchingRoutes = routes.Where(rec => rec.Desc.ToLower().StartsWith(q.ToLower())).ToList();

                if (!matchingRoutes.IsCollectionValid())
                    return Json(null);

                var routesAsCodeName = matchingRoutes.Select(rec => new CodeNameSelectorModel { Id = rec.Cd, Name = rec.Desc, IsReadonly = false, Source = TerminologyConstants.MANUAL_DATA_SOURCE, SourceColor = TerminologyConstants.GetColorForRecordSource(TerminologyConstants.MANUAL_DATA_SOURCE) });

                return Json(routesAsCodeName);
            }

            var matchingRoutesInSession = routesInSession.Where(rec => rec.Value.ToLower().StartsWith(q.ToLower())).ToList();

            if (!matchingRoutesInSession.IsCollectionValid())
                return Json(null);

            var routesAsLkp = matchingRoutesInSession.Select(rec => new CodeNameSelectorModel { Id = rec.Key, Name = rec.Value, IsReadonly = false, Source = TerminologyConstants.MANUAL_DATA_SOURCE, SourceColor = TerminologyConstants.GetColorForRecordSource(TerminologyConstants.MANUAL_DATA_SOURCE) });

            return Json(routesAsLkp);
        }

        [HttpGet]
        [Route("Formulary/SearchLatestRoutes")]
        public async Task<JsonResult> SearchLatestRoutes(string q)
        {
            if (q.IsEmpty()) return Json(null);

            var routesInSession = await _formularyLookupService.GetLatestRoutesLookup();

            if (!routesInSession.IsCollectionValid())
            {
                string token = HttpContext.Session.GetString("access_token");
                var routes = await TerminologyAPIService.GetRoutes(token);

                if (!routes.IsCollectionValid())
                    return Json(null);

                var matchingRoutes = routes.Where(rec => rec.Desc.ToLower().StartsWith(q.ToLower())).ToList();

                if (!matchingRoutes.IsCollectionValid())
                    return Json(null);

                var routesAsCodeName = matchingRoutes.Select(rec => new CodeNameSelectorModel { Id = rec.Cd, Name = rec.Desc, IsReadonly = false, Source = TerminologyConstants.MANUAL_DATA_SOURCE, SourceColor = TerminologyConstants.GetColorForRecordSource(TerminologyConstants.MANUAL_DATA_SOURCE) });

                return Json(routesAsCodeName);
            }

            var matchingRoutesInSession = routesInSession.Where(rec => rec.Value.ToLower().StartsWith(q.ToLower())).ToList();

            if (!matchingRoutesInSession.IsCollectionValid())
                return Json(null);

            var routesAsLkp = matchingRoutesInSession.Select(rec => new CodeNameSelectorModel { Id = rec.Key, Name = rec.Value, IsReadonly = false, Source = TerminologyConstants.MANUAL_DATA_SOURCE, SourceColor = TerminologyConstants.GetColorForRecordSource(TerminologyConstants.MANUAL_DATA_SOURCE) });

            return Json(routesAsLkp);
        }

        [HttpGet]
        [Route("Formulary/SearchIndication")]
        public async Task<JsonResult> SearchIndication(string q)
        {
            const string semanticTag = "disorder";

            var token = HttpContext.Session.GetString("access_token");

            var searchResult = await TerminologyAPIService.SearchSNOMEDData(q, semanticTag, token);

            if (searchResult == null || !searchResult.Data.IsCollectionValid()) return Json(new List<string>());

            var resultsAsCodeText = searchResult.Data.Select(rec => new CodeNameSelectorModel { Id = rec.Code, Name = rec.Term, IsReadonly = false, Source = TerminologyConstants.MANUAL_DATA_SOURCE, SourceColor = TerminologyConstants.GetColorForRecordSource(TerminologyConstants.MANUAL_DATA_SOURCE) });

            return Json(resultsAsCodeText);
        }

        [HttpGet]
        [Route("Formulary/SearchTradeFamily")]
        public async Task<JsonResult> SearchTradeFamily(string q)
        {
            const string semanticTag = "product";

            var token = HttpContext.Session.GetString("access_token");

            var searchResult = await TerminologyAPIService.SearchSNOMEDData(q, semanticTag, token);

            if (searchResult == null || !searchResult.Data.IsCollectionValid()) return Json(new List<string>());

            var resultsAsCodeText = searchResult.Data.Select(rec => new CodeNameSelectorModel { Id = rec.Code, Name = rec.Term, IsReadonly = false, Source = TerminologyConstants.MANUAL_DATA_SOURCE, SourceColor = TerminologyConstants.GetColorForRecordSource(TerminologyConstants.MANUAL_DATA_SOURCE) });

            return Json(resultsAsCodeText);
        }

        [HttpGet]
        [Route("Formulary/SearchTitrationType")]
        public async Task<JsonResult> SearchTitrationType(string q)
        {
            if (q.IsEmpty()) return Json(null);

            var titrInSession = await _formularyLookupService.GetTitrationType();

            if (!titrInSession.IsCollectionValid())
            {
                return Json(null);
            }

            var matchingTitrInSession = titrInSession.Where(rec => rec.Value.ToLower().StartsWith(q.ToLower())).ToList();

            if (!matchingTitrInSession.IsCollectionValid())
                return Json(null);

            var titrsAsLkp = matchingTitrInSession.Select(rec => new CodeNameSelectorModel { Id = rec.Key, Name = rec.Value, IsReadonly = false, Source = TerminologyConstants.MANUAL_DATA_SOURCE, SourceColor = TerminologyConstants.GetColorForRecordSource(TerminologyConstants.MANUAL_DATA_SOURCE) });

            return Json(titrsAsLkp);
        }

        [HttpGet]
        [Route("Formulary/GetBasisOfPharmaStrength")]
        public async Task<JsonResult> GetBasisOfPharmaStrength()
        {
            string token = HttpContext.Session.GetString("access_token");

            var basisOfPharmaStrength = await TerminologyAPIService.GetBasisOfPharmaStrength(token);

            basisOfPharmaStrength.Insert(0, new BasisOfPharmaStrengthAPIModel { Cd = "", Desc = "Please Select", Recordstatus = 1 });

            return Json(basisOfPharmaStrength);
        }

        [HttpGet]
        [Route("Formulary/GetAdditionalCodeSystems")]
        public async Task<JsonResult> GetAdditionalCodeSystems()
        {
            var token = HttpContext.Session.GetString("access_token");

            var additionalCodeSystems = await TerminologyAPIService.GetClassificationCodeTypesLookup(token);

            additionalCodeSystems.Insert(0, new FormularyLookupAPIModel { Cd = "", Desc = "Please Select", Recordstatus = 1 });

            return Json(additionalCodeSystems);
        }

        [HttpGet]
        [Route("Formulary/SearchIndicationWithDisorderAndFinding")]
        public async Task<JsonResult> SearchIndicationWithDisorderAndFinding(string q)
        {
            const string disorderSemanticTag = "disorder";

            var token = HttpContext.Session.GetString("access_token");

            var disorderSearchResult = await TerminologyAPIService.SearchSNOMEDData(q, disorderSemanticTag, token);

            IEnumerable<CodeNameSelectorModel> disorderResultsAsCodeText = Enumerable.Empty<CodeNameSelectorModel>();

            if (disorderSearchResult == null || !disorderSearchResult.Data.IsCollectionValid())
            {
                disorderResultsAsCodeText = Enumerable.Empty<CodeNameSelectorModel>();
            }

            if (disorderSearchResult != null && disorderSearchResult.Data.IsCollectionValid())
            {
                disorderResultsAsCodeText = disorderSearchResult.Data.Select(rec => new CodeNameSelectorModel { Id = rec.Code, Name = rec.Term, IsReadonly = false, Source = TerminologyConstants.MANUAL_DATA_SOURCE, SourceColor = TerminologyConstants.GetColorForRecordSource(TerminologyConstants.MANUAL_DATA_SOURCE) });
            }
            

            const string findingSemanticTag = "finding";

            var findingSearchResult = await TerminologyAPIService.SearchSNOMEDData(q, findingSemanticTag, token);

            IEnumerable<CodeNameSelectorModel> findingResultsAsCodeText = Enumerable.Empty<CodeNameSelectorModel>();

            if (findingSearchResult == null || !findingSearchResult.Data.IsCollectionValid())
            {
                findingResultsAsCodeText = Enumerable.Empty<CodeNameSelectorModel>();
            }


            if (findingSearchResult != null && findingSearchResult.Data.IsCollectionValid())
            {
                findingResultsAsCodeText = findingSearchResult.Data.Select(rec => new CodeNameSelectorModel { Id = rec.Code, Name = rec.Term, IsReadonly = false, Source = TerminologyConstants.MANUAL_DATA_SOURCE, SourceColor = TerminologyConstants.GetColorForRecordSource(TerminologyConstants.MANUAL_DATA_SOURCE) });
            }

            IEnumerable<CodeNameSelectorModel> combinedResultsAsCodeText = Enumerable.Empty<CodeNameSelectorModel>();

            combinedResultsAsCodeText = disorderResultsAsCodeText.Concat(findingResultsAsCodeText);

            return Json(combinedResultsAsCodeText);
        }

    }
}