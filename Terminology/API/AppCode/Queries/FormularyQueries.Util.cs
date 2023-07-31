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
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public partial class FormularyQueries : IFormularyQueries
    {
        public CheckIfProductExistsDTO CheckIfProductExists(List<FormularyIngredientDTO> ingredients, string unitDoseFormSize, string formulationName = null, string supplierName = null, string productType = "amp", bool isExactMatch = false)
        {
            var dto = new CheckIfProductExistsDTO();

            if (!ingredients.IsCollectionValid()) return dto;

            if (string.Compare(productType, "amp", true) == 0)
            {
                if (formulationName.IsEmpty() || supplierName.IsEmpty()) return dto;

                var data = CheckIfAMPExists(ingredients, unitDoseFormSize, formulationName, supplierName, isExactMatch);
                dto.DoesExist = data.Item1;
                dto.ExistingFormularyVersionId = data.Item2;
            }
            else if (string.Compare(productType, "vmp", true) == 0)
            {
                if (formulationName.IsEmpty()) return dto;

                var data = CheckIfVMPExists(ingredients, unitDoseFormSize, formulationName, isExactMatch);
                dto.DoesExist = data.Item1;
                dto.ExistingFormularyVersionId = data.Item2;
            }
            else if (string.Compare(productType, "vtm", true) == 0)
            {
                var data = CheckIfVTMExists(ingredients, isExactMatch);
                dto.DoesExist = data.Item1;
                dto.ExistingFormularyVersionId = data.Item2;
            }
            return dto;
        }

        public DeriveProductNamesDTO DeriveProductNames(List<FormularyIngredientDTO> ingredients, string unitDoseFormSize, string formulationName = null, string supplierName = null, string productType = "amp")
        {
            var prodNames = new Dictionary<string, string>();
            var dto = new DeriveProductNamesDTO();

            if (!ingredients.IsCollectionValid()) return dto;

            if (string.Compare(productType, "amp", true) == 0)
            {
                var name = GetAMPProductName(ingredients, unitDoseFormSize, formulationName, supplierName);

                if (name.IsNotEmpty()) prodNames["amp"] = name;
            }
            else if (string.Compare(productType, "vmp", true) == 0)
            {
                var name = GetVMPProductName(ingredients, unitDoseFormSize, formulationName);

                if (name.IsNotEmpty()) prodNames["vmp"] = name;
            }
            else if (string.Compare(productType, "vtm", true) == 0)
            {
                var name = GetVTMProductName(ingredients);

                if (name.IsNotEmpty()) prodNames["vtm"] = name;
            }
            else if (string.Compare(productType, "all", true) == 0)
            {
                var vtmName = GetVTMProductName(ingredients);
                var vmpName = GetVMPProductName(ingredients, unitDoseFormSize, formulationName);
                var ampName = GetAMPProductName(ingredients, unitDoseFormSize, formulationName, supplierName);

                if (vtmName.IsNotEmpty()) prodNames["vtm"] = vtmName;
                if (ampName.IsNotEmpty()) prodNames["amp"] = ampName;
                if (vmpName.IsNotEmpty()) prodNames["vmp"] = vmpName;
            }
            dto.ProductNameByType = prodNames;
            return dto;
        }

        private string GetAMPProductName(List<FormularyIngredientDTO> ingredients, string unitDoseFormSize, string formulationName, string supplierName)
        {
            if (formulationName.IsEmpty() || supplierName.IsEmpty()) return null;

            var doseComps = GeDoseCompositions(ingredients, unitDoseFormSize);

            if (!doseComps.IsCollectionValid()) return null;

            return $"{string.Join(" / ", doseComps)} {formulationName} ({supplierName})";
        }
        private string GetVMPProductName(List<FormularyIngredientDTO> ingredients, string unitDoseFormSize, string formulationName)
        {
            if (formulationName.IsEmpty()) return null;

            var doseComps = GeDoseCompositions(ingredients, unitDoseFormSize);

            if (!doseComps.IsCollectionValid()) return null;

            return $"{string.Join(" / ", doseComps)} {formulationName}";
        }

        private string GetVTMProductName(List<FormularyIngredientDTO> ingredients)
        {
            var drugNames = ingredients.Select(rec => rec.IngredientName).ToList();

            if (!drugNames.IsCollectionValid()) return null;

            return $"{string.Join(" + ", drugNames)}";
        }

        private (bool, string) CheckIfVTMExists(List<FormularyIngredientDTO> ingredients, bool isExactMatch = false)
        {
            var drugNames = ingredients.Select(rec => rec.IngredientName).ToList();

            if (!drugNames.IsCollectionValid()) return (false, null);

            var initDrugNameToCheck = $"{drugNames[0]}";

            //Just to narrow down the records - Actual comparision happens in the below iteration
            var existingProducts = this.GetLatestFormulariesBriefInfoByNameOrCode(initDrugNameToCheck, "VTM", isExactMatch);

            if (!existingProducts.IsCollectionValid()) return (false, null);

            var hasMatching = false;
            var prodId = string.Empty;

            foreach (var existingProd in existingProducts)
            {
                var existingProdName = existingProd.Name;

                if (existingProdName.Contains("+"))
                {
                    string[] strlst = existingProdName.Split("+");

                    if(strlst.Length == drugNames.Count())
                    {
                        List<string> sortedIngs = new List<string>();

                        foreach (string ing in strlst)
                        {
                            sortedIngs.Add(ing.Trim());
                        }

                        sortedIngs.Sort();
                        drugNames.Sort();

                        if(sortedIngs.SequenceEqual(drugNames, StringComparer.OrdinalIgnoreCase))
                        {
                            hasMatching = sortedIngs.SequenceEqual(drugNames, StringComparer.OrdinalIgnoreCase);
                            prodId = existingProd.FormularyVersionId;
                            break;
                        }

                    }
                }
                else
                {
                    foreach (var drugName in drugNames)
                    {
                        hasMatching = existingProdName.EqualsIgnoreCase(drugName);
                    }

                    if (hasMatching)
                    {
                        prodId = existingProd.FormularyVersionId;
                        break;
                    }
                }
                
            }

            return (hasMatching, prodId);
        }

        private (bool, string) CheckIfAMPExists(List<FormularyIngredientDTO> ingredients, string unitDoseFormSize, string formulationName, string supplierName, bool isExactMatch = false)
        {
            //Note: This may not be the most accurate way for verifiying 
            var doseComps = GeDoseCompositions(ingredients, unitDoseFormSize);

            if (!doseComps.IsCollectionValid()) return (false, null);

            var initDrugNameToCheck = $"{doseComps[0]} {formulationName}";

            var existingProducts = this.GetLatestFormulariesBriefInfoByNameOrCode(initDrugNameToCheck, "AMP", isExactMatch);

            if (!existingProducts.IsCollectionValid()) return (false, null);

            var hasMatching = false;
            var prodId = string.Empty;

            foreach (var existingProd in existingProducts)
            {
                var existingProdName = existingProd.Name;

                foreach (var doseComp in doseComps)
                {
                    hasMatching = existingProdName.IndexOf(doseComp, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                if (hasMatching)
                    hasMatching = existingProdName.IndexOf(formulationName, StringComparison.OrdinalIgnoreCase) >= 0;

                if (hasMatching)
                    hasMatching = existingProdName.IndexOf(supplierName, StringComparison.OrdinalIgnoreCase) >= 0;

                if (hasMatching)
                {
                    prodId = existingProd.FormularyVersionId;
                    break;
                }
            }

            return (hasMatching, prodId);
        }

        private (bool, string) CheckIfVMPExists(List<FormularyIngredientDTO> ingredients, string unitDoseFormSize, string formulationName, bool isExactMatch = false)
        {
            //Note: This may not be the most accurate way for verifiying 
            var doseComps = GeDoseCompositions(ingredients, unitDoseFormSize);

            if (!doseComps.IsCollectionValid()) return (false, null);

            var initDrugNameToCheck = $"{doseComps[0]} {formulationName}";

            var existingProducts = this.GetLatestFormulariesBriefInfoByNameOrCode(initDrugNameToCheck, "VMP", isExactMatch);

            if (!existingProducts.IsCollectionValid()) return (false, null);


            var hasMatching = false;
            var prodId = string.Empty;

            foreach (var existingProd in existingProducts)
            {
                var existingProdName = existingProd.Name;

                if (existingProdName.Contains(" / "))
                {
                    string[] strlst = existingProdName.Split(" / ");

                    if (strlst.Length == ingredients.Count())
                    {
                        List<string> sortedIngs = new List<string>();

                        List<string> sortNewIngs = new List<string>();

                        foreach (var ing in strlst)
                        {
                            sortedIngs.Add(ing.Trim());
                        }

                        foreach (var ing in ingredients)
                        {
                            sortNewIngs.Add(ing.IngredientName.Trim());
                        }

                        sortedIngs.Sort();
                        sortNewIngs.Sort();

                        for(int i = 0; i < sortedIngs.Count(); i++)
                        {
                            if (sortedIngs[i].IndexOf(sortNewIngs[i], StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                foreach (var doseComp in doseComps)
                                {
                                    hasMatching = existingProdName.IndexOf(doseComp, StringComparison.OrdinalIgnoreCase) >= 0;
                                }

                                if (hasMatching)
                                    hasMatching = existingProdName.IndexOf(formulationName, StringComparison.OrdinalIgnoreCase) >= 0;

                                if (hasMatching)
                                {
                                    prodId = existingProd.FormularyVersionId;
                                    break;
                                }
                            }

                            break;
                        }

                    }
                }
                else
                {
                    foreach (var doseComp in doseComps)
                    {
                        hasMatching = existingProdName.IndexOf(doseComp, StringComparison.OrdinalIgnoreCase) >= 0;
                    }

                    if (hasMatching)
                        hasMatching = existingProdName.IndexOf(formulationName, StringComparison.OrdinalIgnoreCase) >= 0;

                    if (hasMatching)
                    {
                        prodId = existingProd.FormularyVersionId;
                        break;
                    }
                }
            }

            return (hasMatching, prodId);
        }

        private List<string> GeDoseCompositions(List<FormularyIngredientDTO> ingredients, string unitDoseFormSize)
        {
            return ingredients.Select(rec =>
            {
                var numDoseSize = "";
                var denoDoseSize = "";
                var numDoseSizeUnit = "";
                var denoDoseSizeUnit = "";
                var desc = $"{rec.IngredientName}";

                if (rec.StrengthValueNumerator.IsNotEmpty())
                {
                    numDoseSizeUnit = rec.StrengthValueNumeratorUnitDesc;

                    if (rec.StrengthValueNumerator.IndexOf(".") >= 0)
                    {
                        decimal.TryParse(rec.StrengthValueNumerator, out decimal num);
                        decimal.TryParse(unitDoseFormSize, out decimal unitSize);
                        if (num > 0 && unitSize > 0)
                            numDoseSize = (num * unitSize).ToString();

                        if (numDoseSize.Contains(".0000"))
                            numDoseSize = numDoseSize.Replace(".0000", "");
                        else if (numDoseSize.Contains("."))
                            numDoseSize = numDoseSize.TrimEnd(new char[] { '0' });
                       
                        if(numDoseSize.EndsWith("."))
                            numDoseSize = numDoseSize.Replace(".", "");
                    }
                    else
                    {
                        int.TryParse(rec.StrengthValueNumerator, out int num);
                        decimal.TryParse(unitDoseFormSize, out decimal unitSize);
                        if (num > 0 && unitSize > 0)
                            numDoseSize = (num * unitSize).ToString();

                        if (numDoseSize.Contains(".0000"))
                            numDoseSize = numDoseSize.Replace(".0000", "");
                        else if (numDoseSize.Contains("."))
                            numDoseSize = numDoseSize.TrimEnd(new char[] { '0' });

                        if (numDoseSize.EndsWith("."))
                            numDoseSize = numDoseSize.Replace(".", "");
                    }

                    if (rec.StrengthValueDenominator.IsNotEmpty())
                    {
                        denoDoseSizeUnit = rec.StrengthValueDenominatorUnitDesc;

                        if (rec.StrengthValueDenominator.IndexOf(".") >= 0)
                        {
                            decimal.TryParse(rec.StrengthValueDenominator, out decimal num);
                            decimal.TryParse(unitDoseFormSize, out decimal unitSize);
                            if (num > 0 && unitSize > 0)
                                denoDoseSize = (num * unitSize).ToString();

                            if (denoDoseSize.Contains(".0000"))
                                denoDoseSize = denoDoseSize.Replace(".0000", "");
                            else if (denoDoseSize.Contains("."))
                                denoDoseSize = denoDoseSize.TrimEnd(new char[] { '0' });

                            if (denoDoseSize.EndsWith("."))
                                denoDoseSize = denoDoseSize.Replace(".", "");
                        }
                        else
                        {
                            int.TryParse(rec.StrengthValueDenominator, out int num);
                            decimal.TryParse(unitDoseFormSize, out decimal unitSize);
                            if (num > 0 && unitSize > 0)
                                denoDoseSize = (num * unitSize).ToString();

                            if (denoDoseSize.Contains(".0000"))
                                denoDoseSize = denoDoseSize.Replace(".0000", "");
                            else if (denoDoseSize.Contains("."))
                                denoDoseSize = denoDoseSize.TrimEnd(new char[] { '0' });

                            if (denoDoseSize.EndsWith("."))
                                denoDoseSize = denoDoseSize.Replace(".", "");
                        }
                    }

                    if (numDoseSize.IsNotEmpty())
                    {
                        if (denoDoseSize.IsNotEmpty())
                        {
                            desc = $"{desc} {numDoseSize}{numDoseSizeUnit}/{denoDoseSize}{denoDoseSizeUnit}";
                        }
                        else
                        {
                            desc = $"{desc} {numDoseSize}{numDoseSizeUnit}";
                        }
                    }
                }

                return desc;
            }).ToList();
        }
    }
}
