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


﻿using Interneuron.Common.Extensions;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace SynapseDynamicAPI.Infrastructure.Filters
{
    //Not being used - to change the endpoint name - only for reference
    public class ActionNameOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            context.ApiDescription.RelativePath = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ApiDescription.ActionDescriptor).ActionName;

            //var x1 = (context.ApiDescription.ActionDescriptor as ActionDescriptor);

            foreach (var parameter in context.ApiDescription.ParameterDescriptions)
            {
                if (parameter.RouteInfo != null && !operation.Parameters.Single(x => x.Name.Equals(parameter.Name)).Required)
                {
                    parameter.RouteInfo.DefaultValue = null;
                }

            }

        }
    }

    public class ActionNameOperationFilter1 : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Paths.Each(p =>
            {

                if (p.Value != null)
                {
                    if (p.Value.Delete is Swashbuckle.AspNetCore.Swagger.Operation op)
                    {
                        //p.key = $"/{op.OperationId}";
                    }
                }
            });
            if (context != null && context.ApiDescriptions != null)
            {
                context.ApiDescriptions.Each(apiDesc =>
                {
                    apiDesc.RelativePath = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)apiDesc.ActionDescriptor).ActionName;
                });
            }
        }
    }

    // this inheritance is required, as IsOptional has only getter
    //public class OptionalHttpParameterDescriptor : ReflectedHttpParameterDescriptor
    //{
    //    public OptionalHttpParameterDescriptor(ReflectedHttpParameterDescriptor parameterDescriptor)
    //        : base(parameterDescriptor.ActionDescriptor, parameterDescriptor.ParameterInfo)
    //    {
    //    }
    //    public override bool IsOptional => true;
    //}
}
