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
﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Models
{
    public class APIModel
    {
        public string GetList { get; set; }
        public string GetObject { get; set; }
        public string GetListByAttribute { get; set; }
        public string PostObject { get; set; }
        public string DeleteObject { get; set; }
        public string DeleteObjectByAttribute { get; set; }
        public string GetObjectHistory { get; set; }
        public string KeyAttribute { get; set; }
        public string EntityName { get; set; }
        public string Namespance { get; set; }
        public string SamplePostJson { get; set; }

    }
}
    
