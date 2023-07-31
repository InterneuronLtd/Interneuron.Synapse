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


using Hl7.Fhir.Model;
using Interneuron.Common.Extensions;
using System;

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.Extensions
{
    public static class Hl7Extensions
    {
        public static Type GetHL7ModelType(this string type)
        {
            if (type.IsNotEmpty() && type.EqualsIgnoreCase("patient"))
            {
                return typeof(Patient);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("encounter"))
            {
                return typeof(Encounter);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("observation"))
            {
                return typeof(Observation);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("result"))
            {
                return typeof(DiagnosticReport);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("procedure"))
            {
                return typeof(Procedure);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("report"))
            {
                return typeof(DiagnosticReport);
            }


            return null;
        }
    }
}
