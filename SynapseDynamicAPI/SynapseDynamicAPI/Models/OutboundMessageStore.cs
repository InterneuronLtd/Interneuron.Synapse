//Interneuron Synapse

//Copyright(C) 2019  Interneuron CIC

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
using System.Linq;
using System.Threading.Tasks;

namespace SynapseDynamicAPI.Models
{
    public class OutboundMessageStore
    {
        public string OutboundMessageStore_Id { get; set; }
        public string Message_Id { get; set; }
        public string Message { get; set; }
        public string DestinationIP { get; set; }
        public string DestinationPort { get; set; }
        public int SendStatus { get; set; }
        public string ResponseType { get; set; }
        public string ResponseText { get; set; }
        public string Person_Id { get; set; }
        public string Encounter_Id { get; set; }
        public string HospitalNumber { get; set; }
        public string EMPINumber { get; set; }
    }
}