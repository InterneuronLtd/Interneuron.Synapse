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
﻿using System.Collections.Generic;
using Interneuron.FDBAPI.Client.DataModels;

namespace Interneuron.FDBAPI.Client
{
    public class FDBAPIResponse
    {
        public Dictionary<string, List<FDBIdText>> ContraIndications { get; set; }
        public Dictionary<string, List<string>> SideEffects { get; set; }
        public Dictionary<string, List<string>> SafetyMessages { get; set; }
        public Dictionary<string, bool> Endorsements { get; set; }
        public Dictionary<string, List<FDBIdText>> LicensedUses { get; set; }
        public Dictionary<string, List<FDBIdText>> UnLicensedUses { get; set; }
        public Dictionary<string, bool> AdverseEffects { get; set; }
        public Dictionary<string, List<string>> Cautions { get; set; }

    }
}
