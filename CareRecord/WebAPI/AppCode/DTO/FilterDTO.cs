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

namespace Interneuron.CareRecord.API.AppCode.DTO
{
    public class Filter
    {
        public string filterClause { get; set; }
    }

    public class Filters
    {
        public List<Filter> filters { get; set; }

        public Filters()
        {
            filters = new List<Filter>();
        }
    }

    public class FilterParameter
    {
        public string paramName { get; set; }
        public string paramValue { get; set; }
    }

    public class FilterParameters
    {
        public List<FilterParameter> filterparams { get; set; }

        public FilterParameters()
        {
            filterparams = new List<FilterParameter>();
        }
    }

    public class SelectStatement
    {
        public string selectstatement { get; set; }
    }

    public class OrderByGroupByStatement
    {
        public string ordergroupbystatement { get; set; }
    }
}
