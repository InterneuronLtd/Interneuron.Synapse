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
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace Interneuron.Terminology.API.AppCode.Validators
{
    /// <summary>
    /// This validator will check if any record (having same code) exists in the system by comparing against the 
    /// fields passed as parameter
    /// </summary>
    public class SimilarFormularyValidator : ITerminologyValidator
    {
        private FormularyHeader _formularyHeader;
        private List<FormularyHeader> _existingFormulariesFromDB;
        private Dictionary<string, HashSet<string>> _comparableFields;

        public SimilarFormularyValidator(FormularyHeader formularyHeader, List<FormularyHeader> existingFormulariesFromDB, Dictionary<string, HashSet<string>> comparableFields)
        {
            _formularyHeader = formularyHeader;
            _existingFormulariesFromDB = existingFormulariesFromDB;
            _comparableFields = comparableFields;
        }
        //FormularyHeader formularyHeader, List<FormularyHeader> existingFormulariesFromDB, Dictionary<string, HashSet<string>> comparableFields
        public TerminologyValidationResult Validate()
        {
            var validationResult = new TerminologyValidationResult();

            //header is the root entity here and composites all other entities
            if (_formularyHeader == null || !_existingFormulariesFromDB.IsCollectionValid() || !_comparableFields.IsCollectionValid())
            {
                validationResult.IsValid = false;
                return validationResult;
            }

            var dbMatchingExisting = _existingFormulariesFromDB.Where(e => e.Code == _formularyHeader.Code).ToList();

            if (!dbMatchingExisting.IsCollectionValid())
            {
                validationResult.IsValid = false;
                return validationResult;
            }

            bool isHeaderSame = false, isDetailSame = false, isIndicationSame = false, isIngredientSame = false, isRouteDetailSame = false, isAdditionalCodeSame = false,
            isOntologyFormSame = false;

            foreach (var matchingRecInDb in dbMatchingExisting)
            {
                if (isHeaderSame && isDetailSame && isIndicationSame && isIngredientSame && isRouteDetailSame && isAdditionalCodeSame && isOntologyFormSame)
                {
                    validationResult.IsValid = true;
                    return validationResult;
                }

                isHeaderSame = CheckIsHeaderSame(_formularyHeader, matchingRecInDb, _comparableFields);

                isDetailSame = CheckIsDetailSame(_formularyHeader, matchingRecInDb, _comparableFields);

                isAdditionalCodeSame = CheckIsAdditionalCodesSame(_formularyHeader, matchingRecInDb, _comparableFields);

                isIndicationSame = CheckIsIndicationsSame(_formularyHeader, matchingRecInDb, _comparableFields);

                isOntologyFormSame = CheckIsOntologyFormSame(_formularyHeader, matchingRecInDb, _comparableFields);

                isIngredientSame = CheckIsIngredientsSame(_formularyHeader, matchingRecInDb, _comparableFields);

                isRouteDetailSame = CheckIsRouteDetailsSame(_formularyHeader, matchingRecInDb, _comparableFields);
                
                if (isHeaderSame && isDetailSame && isAdditionalCodeSame && isIndicationSame && isIngredientSame && isRouteDetailSame && isOntologyFormSame)
                {
                    validationResult.IsValid = true;
                    return validationResult;
                }
            }

            validationResult.IsValid = false;
            return validationResult;
        }

        private bool CheckIsOntologyFormSame(FormularyHeader formularyHeader, FormularyHeader matchingRecInDb, Dictionary<string, HashSet<string>> comparableFields)
        {
            var ontologyFields = comparableFields.ContainsKey("formulary_ontologyform") ? comparableFields["formulary_ontologyform"] : null;

            if (!ontologyFields.IsCollectionValid()) return true;

            if ((!matchingRecInDb.FormularyOntologyForm.IsCollectionValid() && formularyHeader.FormularyOntologyForm.IsCollectionValid()) || (matchingRecInDb.FormularyOntologyForm.IsCollectionValid() && !formularyHeader.FormularyOntologyForm.IsCollectionValid()))
            {
                return true;
            }

            var inputTypeHasAllProps = formularyHeader.FormularyOntologyForm.HasAllProperties<FormularyOntologyForm>(ontologyFields);

            var dbTypeHasAllProps = matchingRecInDb.FormularyOntologyForm.HasAllProperties<FormularyOntologyForm>(ontologyFields);

            if (!inputTypeHasAllProps || !dbTypeHasAllProps) return true;

            var matchingCounter = new List<bool>();

            var ontFieldsArr = ontologyFields.ToArray();

            var selectFields = $"new {{{string.Join(",", ontFieldsArr)}}}";

            var inputFieldsData = formularyHeader.FormularyOntologyForm.AsQueryable().Select(selectFields).ToDynamicList();

            if (!inputFieldsData.IsCollectionValid()) return true;

            inputFieldsData.Each(inp =>
            {
                var whereClause = "";
                var fieldDataList = new List<string>();

                for (var fieldNmIndex = 0; fieldNmIndex < ontFieldsArr.Length; fieldNmIndex++)
                {
                    whereClause = $"{whereClause} && {ontFieldsArr[fieldNmIndex]} == @{fieldNmIndex}";

                    fieldDataList.Add(inp[ontFieldsArr[fieldNmIndex]]);// $"{fieldDataList},{(inp[ingredientsFieldsArr[fieldNmIndex]])}";
                }
                whereClause = whereClause.TrimStart(' ', '&', '&');

                var dbDataList = matchingRecInDb.FormularyOntologyForm.AsQueryable().Where(whereClause, fieldDataList.ToArray()).ToDynamicList();

                if (dbDataList.IsCollectionValid())
                {
                    matchingCounter.Add(true);
                }

            });

            var isOntoSame = formularyHeader.FormularyIngredient.Count == matchingCounter.Count;

            return isOntoSame;
        }

        private bool CheckIsDetailSame(FormularyHeader formularyHeader, FormularyHeader matchingRecInDb, Dictionary<string, HashSet<string>> comparableFields)
        {
            var detailFields = comparableFields.ContainsKey("formulary_detail") ? comparableFields["formulary_detail"] : null;

            if (!detailFields.IsCollectionValid()) return true;

            if ((matchingRecInDb.FormularyDetail.IsCollectionValid() && !formularyHeader.FormularyDetail.IsCollectionValid()) || (!matchingRecInDb.FormularyDetail.IsCollectionValid() && formularyHeader.FormularyDetail.IsCollectionValid()))
            {
                return true;
            }

            if (matchingRecInDb.FormularyDetail.IsCollectionValid() && formularyHeader.FormularyDetail.IsCollectionValid())
            {
                var isDetailSame = formularyHeader.FormularyDetail.First().HasSamePropertyValues<FormularyDetail>(matchingRecInDb.FormularyDetail.First(), detailFields);

                return isDetailSame;
            }

            return false;
        }

        private bool CheckIsHeaderSame(FormularyHeader formularyHeader, FormularyHeader matchingRecInDb, Dictionary<string, HashSet<string>> comparableFields)
        {
            var headerFields = comparableFields.ContainsKey("formulary_header") ? comparableFields["formulary_header"] : null;

            //Will compare only if there are any fields to be compared
            if (!headerFields.IsCollectionValid()) return true;

            var isHeaderSame = formularyHeader.HasSamePropertyValues<FormularyHeader>(matchingRecInDb, headerFields);

            return isHeaderSame;
        }

        private bool CheckIsAdditionalCodesSame(FormularyHeader formularyHeader, FormularyHeader matchingRecInDb, Dictionary<string, HashSet<string>> comparableFields)
        {
            var addCodeFields = comparableFields.ContainsKey("formulary_additionalcodes") ? comparableFields["formulary_additionalcodes"] : null;

            if (!addCodeFields.IsCollectionValid()) return true;

            if ((!matchingRecInDb.FormularyAdditionalCode.IsCollectionValid() && formularyHeader.FormularyAdditionalCode.IsCollectionValid()) || (matchingRecInDb.FormularyAdditionalCode.IsCollectionValid() && !formularyHeader.FormularyAdditionalCode.IsCollectionValid()))
            {
                return true;
            }

            //var matchingCounter = new List<bool>();

            //foreach (var input in formularyHeader.FormularyAdditionalCode)
            //{
            //    foreach (var dbData in matchingRecInDb.FormularyAdditionalCode)
            //    {
            //        if (input.HasSamePropertyValues<FormularyAdditionalCode>(dbData, addCodeFields))
            //        {
            //            matchingCounter.Add(true);
            //            break;
            //        }
            //    }
            //}

            //var isAdditionalCodeSame = formularyHeader.FormularyAdditionalCode.Count == matchingCounter.Count;

            //var inputDataAsString = "";
            //var dbDataAsString = "";

            //addCodeFields.Each(fldName =>
            //{
            //    var inputResults = formularyHeader.FormularyAdditionalCode.GetPropertyResult(fldName);
            //    if (inputResults.IsCollectionValid())
            //    {
            //        inputResults.Each((res) => inputDataAsString = $"{inputDataAsString}{res}");
            //    }

            //    var dbResults = matchingRecInDb.FormularyAdditionalCode.GetPropertyResult(fldName);
            //    if (dbResults.IsCollectionValid())
            //    {
            //        dbResults.Each((res) => dbDataAsString = $"{dbDataAsString}{res}");
            //    }
            //});

            //var isAdditionalCodeSame = (inputDataAsString == dbDataAsString);

            //return isAdditionalCodeSame;

            var inputTypeHasAllProps = formularyHeader.FormularyAdditionalCode.HasAllProperties<FormularyAdditionalCode>(addCodeFields);

            var dbTypeHasAllProps = matchingRecInDb.FormularyAdditionalCode.HasAllProperties<FormularyAdditionalCode>(addCodeFields);

            if (!inputTypeHasAllProps || !dbTypeHasAllProps) return true;

            var matchingCounter = new List<bool>();

            var fieldsArr = addCodeFields.ToArray();

            var selectFields = $"new {{{string.Join(",", fieldsArr)}}}";

            var inputFieldsData = formularyHeader.FormularyAdditionalCode.AsQueryable().Select(selectFields).ToDynamicList();

            if (!inputFieldsData.IsCollectionValid()) return true;

            inputFieldsData.Each(inp =>
            {
                var whereClause = "";
                var fieldDataList = new List<string>();

                for (var fieldNmIndex = 0; fieldNmIndex < fieldsArr.Length; fieldNmIndex++)
                {
                    whereClause = $"{whereClause} && {fieldsArr[fieldNmIndex]} == @{fieldNmIndex}";

                    fieldDataList.Add(inp[fieldsArr[fieldNmIndex]]);
                }
                whereClause = whereClause.TrimStart(' ', '&', '&');

                var dbDataList = matchingRecInDb.FormularyAdditionalCode.AsQueryable().Where(whereClause, fieldDataList.ToArray()).ToDynamicList();

                if (dbDataList.IsCollectionValid())
                {
                    matchingCounter.Add(true);
                }

            });

            var isAdditionalCodeSame = formularyHeader.FormularyAdditionalCode.Count == matchingCounter.Count;

            return isAdditionalCodeSame;
        }

        private bool CheckIsIndicationsSame(FormularyHeader formularyHeader, FormularyHeader matchingRecInDb, Dictionary<string, HashSet<string>> comparableFields)
        {
            var indicationFields = comparableFields.ContainsKey("formulary_indication") ? comparableFields["formulary_indication"] : null;

            if (!indicationFields.IsCollectionValid()) return true;

            if ((!matchingRecInDb.FormularyIndication.IsCollectionValid() && formularyHeader.FormularyIndication.IsCollectionValid()) || (matchingRecInDb.FormularyIndication.IsCollectionValid() && !formularyHeader.FormularyIndication.IsCollectionValid()))
            {
                return true;
            }

            //var matchingCounter = new List<bool>();

            //foreach (var input in formularyHeader.FormularyIndication)
            //{
            //    foreach (var dbData in matchingRecInDb.FormularyIndication)
            //    {
            //        if (input.HasSamePropertyValues<FormularyIndication>(dbData, indicationFields))
            //        {
            //            matchingCounter.Add(true);
            //            break;
            //        }
            //    }
            //}

            //var isIndicationSame = formularyHeader.FormularyIndication.Count == matchingCounter.Count;

            //var inputDataAsString = "";
            //var dbDataAsString = "";
            //indicationFields.Each(fldName =>
            //{
            //    var inputResults = formularyHeader.FormularyIndication.GetPropertyResult(fldName);
            //    if (inputResults.IsCollectionValid())
            //    {
            //        inputResults.Each((res) => inputDataAsString = $"{inputDataAsString}{res}");
            //    }

            //    var dbResults = matchingRecInDb.FormularyIndication.GetPropertyResult(fldName);
            //    if (dbResults.IsCollectionValid())
            //    {
            //        dbResults.Each((res) => dbDataAsString = $"{dbDataAsString}{res}");
            //    }
            //});

            //var isIndicationSame = (inputDataAsString == dbDataAsString);

            //return isIndicationSame;

            var inputTypeHasAllProps = formularyHeader.FormularyIndication.HasAllProperties<FormularyIndication>(indicationFields);

            var dbTypeHasAllProps = matchingRecInDb.FormularyIndication.HasAllProperties<FormularyIndication>(indicationFields);

            if (!inputTypeHasAllProps || !dbTypeHasAllProps) return true;

            var matchingCounter = new List<bool>();

            var fieldsArr = indicationFields.ToArray();

            var selectFields = $"new {{{string.Join(",", fieldsArr)}}}";

            var inputFieldsData = formularyHeader.FormularyIndication.AsQueryable().Select(selectFields).ToDynamicList();

            if (!inputFieldsData.IsCollectionValid()) return true;

            inputFieldsData.Each(inp =>
            {
                var whereClause = "";
                var fieldDataList = new List<string>();

                for (var fieldNmIndex = 0; fieldNmIndex < fieldsArr.Length; fieldNmIndex++)
                {
                    whereClause = $"{whereClause} && {fieldsArr[fieldNmIndex]} == @{fieldNmIndex}";

                    fieldDataList.Add(inp[fieldsArr[fieldNmIndex]]);
                }
                whereClause = whereClause.TrimStart(' ', '&', '&');

                var dbDataList = matchingRecInDb.FormularyIndication.AsQueryable().Where(whereClause, fieldDataList.ToArray()).ToDynamicList();

                if (dbDataList.IsCollectionValid())
                {
                    matchingCounter.Add(true);
                }

            });

            var isIndicationSame = formularyHeader.FormularyIndication.Count == matchingCounter.Count;

            return isIndicationSame;

        }

        private bool CheckIsIngredientsSame(FormularyHeader formularyHeader, FormularyHeader matchingRecInDb, Dictionary<string, HashSet<string>> comparableFields)
        {
            var ingredientsFields = comparableFields.ContainsKey("formulary_ingredient") ? comparableFields["formulary_ingredient"] : null;

            if (!ingredientsFields.IsCollectionValid()) return true;

            if ((!matchingRecInDb.FormularyIngredient.IsCollectionValid() && formularyHeader.FormularyIngredient.IsCollectionValid()) || (matchingRecInDb.FormularyIngredient.IsCollectionValid() && !formularyHeader.FormularyIngredient.IsCollectionValid()))
            {
                return true;
            }

            //var matchingCounter = new List<bool>();

            //foreach (var input in formularyHeader.FormularyIngredient)
            //{
            //    foreach (var dbData in matchingRecInDb.FormularyIngredient)
            //    {
            //        if (input.HasSamePropertyValues<FormularyIngredient>(dbData, ingredientsFields))
            //        {
            //            matchingCounter.Add(true);
            //            break;
            //        }
            //    }
            //}

            //var isIngredientSame = formularyHeader.FormularyIngredient.Count == matchingCounter.Count;

            var inputTypeHasAllProps = formularyHeader.FormularyIngredient.HasAllProperties<FormularyIngredient>(ingredientsFields);

            var dbTypeHasAllProps = matchingRecInDb.FormularyIngredient.HasAllProperties<FormularyIngredient>(ingredientsFields);

            if (!inputTypeHasAllProps || !dbTypeHasAllProps) return true;

            var matchingCounter = new List<bool>();

            var ingredientsFieldsArr = ingredientsFields.ToArray();

            var selectFields = $"new {{{string.Join(",", ingredientsFieldsArr)}}}";

            var inputFieldsData = formularyHeader.FormularyIngredient.AsQueryable().Select(selectFields).ToDynamicList();

            if (!inputFieldsData.IsCollectionValid()) return true;

            inputFieldsData.Each(inp =>
            {
                var whereClause = "";
                var fieldDataList = new List<string>();

                for (var fieldNmIndex = 0; fieldNmIndex < ingredientsFieldsArr.Length; fieldNmIndex++)
                {
                    whereClause = $"{whereClause} && {ingredientsFieldsArr[fieldNmIndex]} == @{fieldNmIndex}";

                    fieldDataList.Add(inp[ingredientsFieldsArr[fieldNmIndex]]);// $"{fieldDataList},{(inp[ingredientsFieldsArr[fieldNmIndex]])}";
                }
                whereClause = whereClause.TrimStart(' ', '&', '&');

                var dbDataList = matchingRecInDb.FormularyIngredient.AsQueryable().Where(whereClause, fieldDataList.ToArray()).ToDynamicList();

                if (dbDataList.IsCollectionValid())
                {
                    matchingCounter.Add(true);
                }

            });

            var isIngredientSame = formularyHeader.FormularyIngredient.Count == matchingCounter.Count;

            return isIngredientSame;

            //ingredientsFields.Each(fldName =>
            //{
            //    var inputResults = formularyHeader.FormularyIngredient.GetPropertyResult(fldName);
            //    if (inputResults.IsCollectionValid())
            //    {
            //        inputResults.Each((res) => inputDataAsString = $"{inputDataAsString}{res}");
            //    }

            //    var dbResults = matchingRecInDb.FormularyIngredient.GetPropertyResult(fldName);
            //    if (dbResults.IsCollectionValid())
            //    {
            //        dbResults.Each((res) => dbDataAsString = $"{dbDataAsString}{res}");
            //    }
            //});

            //var isIngredientSame = (inputDataAsString == dbDataAsString);

            //return isIngredientSame;

        }

        private bool CheckIsRouteDetailsSame(FormularyHeader formularyHeader, FormularyHeader matchingRecInDb, Dictionary<string, HashSet<string>> comparableFields)
        {
            var routeDetailsFields = comparableFields.ContainsKey("formulary_routedetail") ? comparableFields["formulary_routedetail"] : null;

            if (!routeDetailsFields.IsCollectionValid()) return true;

            if ((!matchingRecInDb.FormularyRouteDetail.IsCollectionValid() && formularyHeader.FormularyRouteDetail.IsCollectionValid()) || (matchingRecInDb.FormularyRouteDetail.IsCollectionValid() && !formularyHeader.FormularyRouteDetail.IsCollectionValid()))
            {
                return true;
            }

            //var matchingCounter = new List<bool>();

            //foreach (var input in formularyHeader.FormularyRouteDetail)
            //{
            //    foreach (var dbData in matchingRecInDb.FormularyRouteDetail)
            //    {
            //        if (input.HasSamePropertyValues<FormularyRouteDetail>(dbData, routeDetailsFields))
            //        {
            //            matchingCounter.Add(true);
            //            break;
            //        }
            //    }
            //}

            //var isRouteDetailsSame = formularyHeader.FormularyRouteDetail.Count == matchingCounter.Count;

            //var inputDataAsString = "";
            //var dbDataAsString = "";

            //routeDetailsFields.Each(fldName =>
            //{
            //    var inputResults = formularyHeader.FormularyRouteDetail.GetPropertyResult(fldName);
            //    if (inputResults.IsCollectionValid())
            //    {
            //        inputResults.Each((res) => inputDataAsString = $"{inputDataAsString}{res}");
            //    }

            //    var dbResults = matchingRecInDb.FormularyRouteDetail.GetPropertyResult(fldName);
            //    if (dbResults.IsCollectionValid())
            //    {
            //        dbResults.Each((res) => dbDataAsString = $"{dbDataAsString}{res}");
            //    }
            //});

            //var isRouteDetailsSame = (inputDataAsString == dbDataAsString);

            //return isRouteDetailsSame;

            var inputTypeHasAllProps = formularyHeader.FormularyRouteDetail.HasAllProperties<FormularyRouteDetail>(routeDetailsFields);

            var dbTypeHasAllProps = matchingRecInDb.FormularyRouteDetail.HasAllProperties<FormularyRouteDetail>(routeDetailsFields);

            if (!inputTypeHasAllProps || !dbTypeHasAllProps) return true;

            var matchingCounter = new List<bool>();

            var fieldsArr = routeDetailsFields.ToArray();

            var selectFields = $"new {{{string.Join(",", fieldsArr)}}}";

            var inputFieldsData = formularyHeader.FormularyRouteDetail.AsQueryable().Select(selectFields).ToDynamicList();

            if (!inputFieldsData.IsCollectionValid()) return true;

            inputFieldsData.Each(inp =>
            {
                var whereClause = "";
                var fieldDataList = new List<string>();

                for (var fieldNmIndex = 0; fieldNmIndex < fieldsArr.Length; fieldNmIndex++)
                {
                    whereClause = $"{whereClause} && {fieldsArr[fieldNmIndex]} == @{fieldNmIndex}";

                    fieldDataList.Add(inp[fieldsArr[fieldNmIndex]]);
                }
                whereClause = whereClause.TrimStart(' ', '&', '&');

                var dbDataList = matchingRecInDb.FormularyRouteDetail.AsQueryable().Where(whereClause, fieldDataList.ToArray()).ToDynamicList();

                if (dbDataList.IsCollectionValid())
                {
                    matchingCounter.Add(true);
                }

            });

            var isRouteDetailsSame = formularyHeader.FormularyRouteDetail.Count == matchingCounter.Count;

            return isRouteDetailsSame;
        }

    }
}
