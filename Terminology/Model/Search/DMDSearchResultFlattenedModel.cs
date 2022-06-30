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


﻿using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Generic;

namespace Interneuron.Terminology.Model.Search
{
    public class DMDSearchResultFlattenedModel : EntityBase
    {
        public string Code { get; set; }

        public string PrevCode { get; set; }

        public string Name { get; set; }

        public string RouteCode { get; set; }

        public string Route { get; set; }

        //public List<DmdLookupRoute> Routes { get; set; }

        public string FormCode { get; set; }

        public string Form { get; set; }

        //public List<DmdLookupForm> Forms { get; set; }

        public string SupplierCode { get; set; }

        public string Supplier { get; set; }

        public long? PrescribingStatusCode { get; set; }

        public string PrescribingStatus { get; set; }

        public long? ControlDrugCategoryCode { get; set; }

        public string ControlDrugCategory { get; set; }

        //public DmdLookupPrescribingstatus PrescribingStatus { get; set; }

        //public DmdLookupControldrugcat ControlDrugCategory { get; set; }

        public int? LogicalLevel { get; set; }

        public string Level { get; set; }

        public string ParentCode { get; set; }

        public string IngredientSubstanceId { get; set; }
        public long? BasisPharmaceuticalStrengthCd { get; set; }
        public string BasisStrengthSubstanceId { get; set; }
        public string StrengthValueNmtrUnitCd { get; set; }
        public decimal? StrengthValNmtr { get; set; }
        public string StrengthValueDnmtrUnitCd { get; set; }
        public decimal? StrengthValDnmtr { get; set; }
        public long? Basiscd { get; set; }
        public string SugF { get; set; }
        public string GluF { get; set; }
        public string PresF { get; set; }
        public string CfcF { get; set; }
        public long? DfIndcd { get; set; }
        public decimal? Udfs { get; set; }
        public string UdfsUomcd { get; set; }
        public string UnitDoseUomcd { get; set; }
        public long? LicAuthcd { get; set; }
        public string Ema { get; set; }
        public string ParallelImport { get; set; }
        public long? AvailRestrictcd { get; set; }
        public long? Ontcd { get; set; }

        //public List<DmdVmpIngredient> VMPIngredients { get; set; }

    }
}
