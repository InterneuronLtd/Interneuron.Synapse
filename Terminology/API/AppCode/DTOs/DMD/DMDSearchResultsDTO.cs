//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

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


﻿using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.DTOs
{
    public class DMDSearchResultsDTO : TerminologyResource
    {
        public string SearchText { get; set; }

        public List<DMDSearchResultDTO> Data { get; set; }
    }

    public class DMDSearchResultDTO : TerminologyResource
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string ParentCode { get; set; }

        //public long? RouteCode { get; set; }

        //public string Route { get; set; }
        public List<DmdLookupRouteDTO> Routes { get; set; }

        //public long? FormCode { get; set; }

        //public string Form { get; set; }

        public DmdLookupFormDTO Form { get; set; }


        public long? SupplierCode { get; set; }

        public string Supplier { get; set; }

        //public long? PrescribingStatusCode { get; set; }

        //public string PrescribingStatus { get; set; }

        //public long? ControlDrugCategoryCode { get; set; }

        //public string ControlDrugCategory { get; set; }

        public DmdLookupPrescribingstatusDTO PrescribingStatus { get; set; }

        public DmdLookupControldrugcatDTO ControlDrugCategory { get; set; }

        //public DmdLookupLicauthDTO CurrentLicensingAuthority { get; set; }
        //public DmdLookupBasisofnameDTO BasisOfName { get; set; }
        //public DmdLookupBasisofstrength BasisOfStrength { get; set; }
        public int? LogicalLevel { get; set; }

        public string Level { get; set; }

        public List<DmdVmpIngredientDTO> VMPIngredients { get; set; }

    }

    public class DMDSearchResultsWithHierarchyDTO : TerminologyResource
    {
        public string SearchText { get; set; }

        public List<DMDSearchResultWithTreeDTO> Data { get; set; }
    }

    public class DMDSearchResultWithTreeDTO : DMDSearchResultDTO
    {
        public List<DMDSearchResultWithTreeDTO> Parents { get; set; }

        public List<DMDSearchResultWithTreeDTO> Children { get; set; }
    }
}
