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
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Extensions
{
    public class FormularyIngredientDTOComparer : IEqualityComparer<FormularyIngredientDTO>
    {
        public bool Equals([AllowNull] FormularyIngredientDTO x, [AllowNull] FormularyIngredientDTO y)
        {
            if (x == null && y == null) return true;
            if (x == null && y != null || x != null && y == null) return false;

            var bpStrEqual = ((x.BasisOfPharmaceuticalStrengthCd.IsEmpty() && y.BasisOfPharmaceuticalStrengthCd.IsEmpty()) || ((x.BasisOfPharmaceuticalStrengthCd.IsNotEmpty() || y.BasisOfPharmaceuticalStrengthCd.IsNotEmpty()) && x.BasisOfPharmaceuticalStrengthCd == y.BasisOfPharmaceuticalStrengthCd));

            var ingCdEquals = ((x.IngredientCd.IsEmpty() && y.IngredientCd.IsEmpty()) || ((x.IngredientCd.IsNotEmpty() || y.IngredientCd.IsNotEmpty()) && x.IngredientCd == y.IngredientCd));

            var denoValEqual = ((x.StrengthValueDenominator.IsEmpty() && y.StrengthValueDenominator.IsEmpty()) || ((x.StrengthValueDenominator.IsNotEmpty() || y.StrengthValueDenominator.IsNotEmpty()) && x.StrengthValueDenominator == y.StrengthValueDenominator));

            var denoValUnitEqual = ((x.StrengthValueDenominatorUnitCd.IsEmpty() && y.StrengthValueDenominatorUnitCd.IsEmpty()) || ((x.StrengthValueDenominatorUnitCd.IsNotEmpty() || y.StrengthValueDenominatorUnitCd.IsNotEmpty()) && x.StrengthValueDenominatorUnitCd == y.StrengthValueDenominatorUnitCd));

            var numValEqual = ((x.StrengthValueNumerator.IsEmpty() && y.StrengthValueNumerator.IsEmpty()) || ((x.StrengthValueNumerator.IsNotEmpty() || y.StrengthValueNumerator.IsNotEmpty()) && x.StrengthValueNumerator == y.StrengthValueNumerator));

            var numValUnitEqual = ((x.StrengthValueNumeratorUnitCd.IsEmpty() && y.StrengthValueNumeratorUnitCd.IsEmpty()) || ((x.StrengthValueNumeratorUnitCd.IsNotEmpty() || y.StrengthValueNumeratorUnitCd.IsNotEmpty()) && x.StrengthValueNumeratorUnitCd == y.StrengthValueNumeratorUnitCd));

            return bpStrEqual && ingCdEquals && denoValEqual && denoValUnitEqual && numValEqual && numValUnitEqual;
        }

        public int GetHashCode([DisallowNull] FormularyIngredientDTO obj)
        {
            var hc = 10;
            hc = hc * 2 + (obj.BasisOfPharmaceuticalStrengthCd ?? "").GetHashCode();
            hc = hc * 2 + (obj.IngredientCd ?? "").GetHashCode();
            hc = hc * 2 + (obj.StrengthValueDenominator ?? "").GetHashCode();
            hc = hc * 2 + (obj.StrengthValueDenominatorUnitCd ?? "").GetHashCode();
            hc = hc * 2 + (obj.StrengthValueNumerator ?? "").GetHashCode();
            hc = hc * 2 + (obj.StrengthValueNumeratorUnitCd ?? "").GetHashCode();

            return hc;
        }
    }
}
