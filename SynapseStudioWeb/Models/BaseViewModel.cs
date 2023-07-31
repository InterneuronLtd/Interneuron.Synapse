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
    public class BaseViewModel
    {
        public string Baseview_id { get; set; }
        [Required(ErrorMessage = "Please enter a new name for the baseview")]
        [RegularExpression("^[a-z0-9 -]+$", ErrorMessage = "Entity name can be only non-alphanumeric and lower case")]
        [Remote(action: "VerifyBaseview", controller: "Baseview")]
        public string BaseviewName { get; set; }
        public string BaseviewDesc { get; set; }
        [Required(ErrorMessage = "Please enter a the SQL Statement for the new baseview")]
        public string BaseviewSQL { get; set; }
        public string BaseviewNamespaceName { get; set; }
        public List<BaseViewDetailDto> BaseViewDetailDto { get; set; }
    }
    public class BaseViewListDto
    {
        public string Baseview_id { get; set; }
        public string BaseviewName { get; set; }
    }

    public class BaseViewDetailDto
    {

        public string BaseViewdetail { get; set; }
        public string BaseViewDescription { get; set; }
    }

    public class BaseViewAttributeDto
    {

        public string AttributeName { get; set; }
        public string DataType { get; set; }
    }

    public class BaseViewSQLModel
    {

        public string SQL { get; set; }
        public string Summary { get; set; }
        public string NamespaceId { get; set; }
        public string NextOrdinalPosition { get; set; }
        public string ViewName { get; set; }
        public string NamespaceName { get; set; }
        public string BaseViewComments { get; set; }
    }

    public class BaseviewAPI
    {
        public string GetList { get; set; }        
        public string GetListByAttribute { get; set; }
        public string PostObject { get; set; }
         
    }

    public class BaseviewNamespaceModel
    {
        public string BaseviewNamespaceId { get; set; }
        [Required(ErrorMessage = "Please enter a name for the new BaseView namespace")]
        [RegularExpression("^[a-z0-9 -]+$", ErrorMessage = "Namespace can be only non-alphanumeric and lower case")]
        [Remote(action: "VerifyBaseviewNamespace", controller: "Baseview")]
        public string BaseviewNamespace { get; set; }
        public string BaseviewNamespaceDescription { get; set; }
        public List<BaseviewNamespaceDto> BaseviewNamespaceDto { get; set; }
    }
    public class BaseviewNamespaceDto
    {
        public string BaseviewNamespaceId { get; set; }
        public string BaseviewNamespace { get; set; }
        
    }
}
