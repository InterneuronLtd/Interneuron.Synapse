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


﻿using Hl7.Fhir.Rest;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Extensions
{
    public static class HttpRequestExtensions
    {
        public static List<Tuple<string, string>> TupledParameters(this HttpRequest request)
        {
            var list = new List<Tuple<string, string>>();

            IQueryCollection queryCollection = request.Query;
            foreach (var query in queryCollection)
            {
                list.Add(new Tuple<string, string>(query.Key, query.Value));
            }
            return list;
        }

        public static SearchParams GetSearchParams(this HttpRequest request)
        {
            var parameters = request.TupledParameters().Where(tp => tp.Item1 != "_format");
            var searchCommand = SearchParams.FromUriParamList(parameters);
            return searchCommand;
        }
    }
}
