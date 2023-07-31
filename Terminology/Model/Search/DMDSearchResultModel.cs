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
﻿using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System.Collections.Generic;

namespace Interneuron.Terminology.Model.Search
{
    public class DMDSearchResultModel : EntityBase
    {
        public string Code { get; set; }

        public string Name { get; set; }

        //public long? RouteCode { get; set; }

        //public string Route { get; set; }

        public List<DmdLookupRoute> Routes { get; set; }

        //public long? FormCode { get; set; }

        //public string Form { get; set; }

        public DmdLookupForm Form { get; set; }

        public string SupplierCode { get; set; }

        public string Supplier { get; set; }

        //public long? PrescribingStatusCode { get; set; }

        //public string PrescribingStatus { get; set; }

        //public long? ControlDrugCategoryCode { get; set; }

        //public string ControlDrugCategory { get; set; }

        public DmdLookupPrescribingstatus PrescribingStatus { get; set; }

        public DmdLookupControldrugcat ControlDrugCategory { get; set; }

        public int? LogicalLevel { get; set; }

        public string Level { get; set; }

        public string ParentCode { get; set; }

        public List<DmdVmpIngredient> VMPIngredients { get; set; }

    }
}
