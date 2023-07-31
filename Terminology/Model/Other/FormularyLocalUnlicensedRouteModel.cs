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
using System;
using System.Collections.Generic;
using System.Text;

namespace Interneuron.Terminology.Model.Other
{
    public class FormularyLocalUnlicensedRouteModel : EntityBase
    {
        public string FormularyVersionId { get; set; }
        public string LocalUnlicensedRoute { get; set; }

        public string LocalUnlicensedRouteMD5 { get; set; }
    }
}
