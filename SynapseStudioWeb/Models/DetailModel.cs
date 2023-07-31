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
    public class DetailModel
    {
        public List<DetailDto> DetailDto { get; set; }
        public List<BaseViewDto> BaseViewDto { get; set; }
        


    }
    public class DetailDto
    {
        
        public string EntityDetail { get; set; }
        public string EntityDescription { get; set; }
    }
    public class BaseViewDto
    {
        public string baseview_id { get; set; }
        public string dependent_view { get; set; }
         
    }
}
