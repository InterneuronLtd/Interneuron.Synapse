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

namespace SynapseStudioWeb.DataService.APIModel
{
    public class DMDSearchResultsAPIModel
    {
        public string SearchText { get; set; }
        public List<DMDSearchResultAPIModel> Data { get; set; }
    }

    public class DmdVmpIngredient
    {
        public long? Isid { get; set; }
        public int? BasisStrntcd { get; set; }
        public long? BsSubid { get; set; }
        public decimal? StrntNmrtrVal { get; set; }
        public long? StrntNmrtrUomcd { get; set; }
        public int? StrntDnmtrVal { get; set; }
        public long? StrntDnmtrUomcd { get; set; }
    }

    public class DMDSearchResultsWithHierarchy
    {
        public string SearchText { get; set; }
        public List<DMDSearchResultWithTree> Data { get; set; }
    }

    public class DMDSearchResultWithTree : DMDSearchResultAPIModel
    {
        public List<DMDSearchResultWithTree> Parents { get; set; }
        public List<DMDSearchResultWithTree> Children { get; set; }
    }
    public class DMDSearchResultAPIModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string ParentCode { get; set; }
        public List<DMDLookup> Routes { get; set; }
        public List<DMDLookup> Forms { get; set; }
        public long? SupplierCode { get; set; }
        public string Supplier { get; set; }
        public DMDLookup PrescribingStatus { get; set; }
        public DMDLookup ControlDrugCategory { get; set; }
        public int? LogicalLevel { get; set; }
        public string Level { get; set; }
        public List<DmdVmpIngredient> VMPIngredients { get; set; }
    }

    public class DMDLookup
    {
        public string Cd { get; set; }
        public DateTime? Cddt { get; set; }
        public long? Cdprev { get; set; }
        public string Desc { get; set; }
        public short? Invalid { get; set; }
        public short? Recordstatus { get; set; }
    }
}
