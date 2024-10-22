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
﻿using System;
using System.Collections.Generic;

namespace Interneuron.Terminology.API.AppCode.DTOs
{
    public partial class FormularyIngredientDTO : TerminologyResource
    {
        public string RowId { get; set; }
        public DateTime? Createddate { get; set; }
        public string Createdby { get; set; }
        public DateTime? Updateddate { get; set; }
        public string Updatedby { get; set; }
        public string FormularyVersionId { get; set; }
        public string IngredientCd { get; set; }
        public string IngredientName { get; set; }
        public string BasisOfPharmaceuticalStrengthCd { get; set; }
        public string BasisOfPharmaceuticalStrengthDesc { get; set; }
        public string StrengthValueNumerator { get; set; }
        public string StrengthValueNumeratorUnitCd { get; set; }
        public string StrengthValueNumeratorUnitDesc { get; set; }
        public string StrengthValueDenominator { get; set; }
        public string StrengthValueDenominatorUnitCd { get; set; }
        public string StrengthValueDenominatorUnitDesc { get; set; }

    }
}
