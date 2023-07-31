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


﻿using System.Collections.Generic;

namespace Interneuron.Terminology.API.AppCode.DTOs
{
    public class DMDDetailResultDTO : TerminologyResource
    {
        public string Code { get; set; }

        public string PrevCode { get; set; }

        public string Name { get; set; }

        public List<DmdLookupRouteDTO> Routes { get; set; }

        public DmdLookupFormDTO Form { get; set; }

        public string SupplierCode { get; set; }

        public string Supplier { get; set; }

        public DmdLookupPrescribingstatusDTO PrescribingStatus { get; set; }

        public DmdLookupControldrugcatDTO ControlDrugCategory { get; set; }

        public DmdLookupAvailrestrictDTO AvailableRestriction { get; set; }

        public DmdLookupBasisofnameDTO BasisOfName { get; set; }

        public DmdLookupDrugformindDTO DoseForm { get; set; }

        public DmdLookupLicauthDTO LicensingAuthority { get; set; }

        public List<DmdLookupOntformrouteDTO> OntologyFormRoutes { get; set; }

        public DmdLookupUomDTO UnitDoseFormSizeUOM { get; set; }

        public DmdLookupUomDTO UnitDoseUOM { get; set; }

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

        public List<DmdVmpIngredientDTO> VMPIngredients { get; set; }

    }
}
