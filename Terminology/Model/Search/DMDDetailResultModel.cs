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
    public class DMDDetailResultModel : EntityBase
    {
        public string Code { get; set; }
        public string PrevCode { get; set; }

        public string Name { get; set; }

        public List<DmdLookupRoute> Routes { get; set; }

        public DmdLookupForm Form { get; set; }

        public string SupplierCode { get; set; }

        public string Supplier { get; set; }

        public DmdLookupPrescribingstatus PrescribingStatus { get; set; }

        public DmdLookupControldrugcat ControlDrugCategory { get; set; }

        public DmdLookupAvailrestrict AvailableRestriction { get; set; }

        public DmdLookupBasisofname BasisOfName { get; set; }
        
        public DmdLookupDrugformind DoseForm { get; set; }

        public DmdLookupLicauth LicensingAuthority { get; set; }

        public List<DmdLookupOntformroute> OntologyFormRoutes { get; set; }
        
        public DmdLookupUom UnitDoseFormSizeUOM { get; set; }
        
        public DmdLookupUom UnitDoseUOM { get; set; }

        public decimal? Udfs { get; set; }

        public string SugF { get; set; }
        public string GluF { get; set; }
        public string PresF { get; set; }
        public string CfcF { get; set; }
        public string Ema { get; set; }
        public string ParallelImport { get; set; }
        public int? LogicalLevel { get; set; }

        public string Level { get; set; }

        public string ParentCode { get; set; }

        public List<DmdVmpIngredient> VMPIngredients { get; set; }

    }
}
