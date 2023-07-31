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


﻿using InterneuronFDBAPI.Models;
using System.Collections.Generic;

namespace InterneuronFDBAPI.Controllers
{
    internal class FDBAPIResponse
    {
        public Dictionary<string, List<ContraindicationsModel>> ContraIndications { get; set; }
        public Dictionary<string, List<string>> SideEffects { get; set; }
        public Dictionary<string, List<string>> SafetyMessages { get; set; }
        public Dictionary<string, bool> Endorsements { get; set; }
        public Dictionary<string, List<LicensedUseModel>> LicensedUses { get; set; }
        public Dictionary<string, List<UnlicensedUseModel>> UnLicensedUses { get; set; }
        public Dictionary<string, bool> AdverseEffects { get; set; }
        public Dictionary<string, List<string>> Cautions { get; set; }
    }
}