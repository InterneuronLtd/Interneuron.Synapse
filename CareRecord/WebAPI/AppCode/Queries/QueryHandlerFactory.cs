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
using System;

namespace Interneuron.CareRecord.API.AppCode
{
    public class QueryHandlerFactory
    {
        private IServiceProvider _provider;

        // public delegate QueryHandler Factory(IServiceProvider _provider); //not useful for now

        public QueryHandlerFactory(System.IServiceProvider provider)
        {
            this._provider = provider;
        }
        public IQueryHandler GetHandler(string actionName)
        {
            if (actionName.EqualsIgnoreCase("read"))
            {
                return this._provider.GetService(typeof(ReadQueryHandler)) as ReadQueryHandler;
            }

            return null;
        }
    }
}