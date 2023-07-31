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


﻿namespace SynapseStudioWeb.Models.MedicinalMgmt
{
    public class FormularyIngredientModel
    {
        public string FormularyVersionId { get; set; }
        public CodeNameSelectorModel Ingredient { get; set; }
        public string BasisOfPharmaceuticalStrength { get; set; }
        public string StrengthValNumerator { get; set; }
        public CodeNameSelectorModel StrengthValueNumeratorUnit { get; set; }
        public string StrengthValDenominator { get; set; }
        public CodeNameSelectorModel StrengthValueDenominatorUnit { get; set; }

    }
}



