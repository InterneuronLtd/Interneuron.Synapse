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


﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Models
{
    public class EntityModel
    {
        public string EntityId { get; set; }
        public string EntityName { get; set; }
        public List<CoreEntityModel> CoreEntityModel { get; set; }
        public List<MetaEntityModel> MetaEntityModel { get; set; }
        public List<ExtendedEntityModel> ExtendedEntityModel { get; set; }
        public List<LocalEntityModel> LocalEntityModel { get; set; }
    }
    public class CoreEntityModel
    {
        public string EntityId { get; set; }
        [Required(ErrorMessage ="Please enter entity name")]
        [RegularExpression("^[a-z0-9 -]+$", ErrorMessage = "Entity name can be only non-alphanumeric and lower case")]
        [Remote(action: "VerifyEntity", controller: "EntityManager")]
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
    }
    public class MetaEntityModel
    {
        public string EntityId { get; set; }        
        [Required(ErrorMessage = "Please enter entity name")]
        [RegularExpression("^[a-z0-9 -]+$", ErrorMessage = "Entity name can be only non-alphanumeric and lower case")]
        [Remote(action: "VerifyEntity", controller: "EntityManager")]
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
    }
    public class ExtendedEntityModel
    {
        public string EntityId { get; set; }
        [Required(ErrorMessage = "Please select the entity")]
        [RegularExpression("^[a-z0-9 -]+$", ErrorMessage = "Entity name can be only non-alphanumeric and lower case")]
        [Remote(action: "VerifyExtendedEntity", controller: "EntityManager", ErrorMessage = "Entity already exists")]
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
    }
    public class LocalEntityModel
    {
        public string EntityId { get; set; }
        [Required(ErrorMessage = "Please enter entity name")]
        [RegularExpression("^[a-z0-9 -]+$", ErrorMessage = "Entity name can be only non-alphanumeric and lower case")]
        [Remote(action: "VerifyLocalEntity", controller: "EntityManager")]
        public string EntityName { get; set; }
        public string EntityDescription { get; set; }
        [Remote(action: "SetLocalNamespaceId", controller: "EntityManager")]
        public string LocalNamespaceId { get; set; }
    }
}
