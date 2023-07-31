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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Models
{
    public class DatabaseActivityDto
    {
        public int pid { get; set; }
        public string usename { get; set; }
        public string application_name { get; set; }
        public System.Net.IPAddress client_addr { get; set; }
        public string query { get; set; }
        public DateTime query_start { get; set; }
    }

    public class DatabaseReplStatusDto
    {
        public string pid { get; set; }
        public string usename { get; set; }
        public string client_addr { get; set; }
        public string client_port { get; set; }
        public string backend_start { get; set; }
        public string sent_location { get; set; }
        public string flush_location { get; set; }
        public string replay_location { get; set; }
        
    }

}

