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
    public class FormularyBasicInfoModel
    {
        public string rowId { get; set; }
        public DateTime? createddate { get; set; }
        public string createdby { get; set; }
        public string tenant { get; set; }
        public string formularyId { get; set; }
        public int? versionId { get; set; }
        public string formularyVersionId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string productType { get; set; }
        public string parentCode { get; set; }
        public string parentName { get; set; }
        public string parentProductType { get; set; }
        public string recStatusCode { get; set; }
        public DateTime? recStatuschangeDate { get; set; }
        public bool? isDuplicate { get; set; }
        public bool? isLatest { get; set; }
        public string recSource { get; set; }
        public string vtmId { get; set; }
        public string vmpId { get; set; }

        //public List<FormularyAdditionalCodeModel> formularyAdditionalCodes { get; set; }
        //public DetailModel detail { get; set; }
        //public List<FormularyIndicationModel> formularyIndications { get; set; }
        //public List<FormularyIngredientModel> formularyIngredients { get; set; }
        //public List<FormularyRouteDetailModel> formularyRouteDetails { get; set; }

    }
}
