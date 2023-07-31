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
using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.Extensions
{
    public static class TypeExtensions
    {
        public static Type ConvertToListOrType(Type type, dynamic entity)
        {
            Type dataType = default(Type);

            if (entity is System.Collections.IEnumerable)
            { 
                dataType = typeof(List<>).MakeGenericType(type);
            }
            else
            {
                dataType = type;
            }

            return dataType;
        }
    }
}
