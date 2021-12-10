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

namespace Interneuron.CareRecord.Repository.QueryBuilders
{
    public class QueryBuilderFactory
    {
        private IServiceProvider _provider;

        public QueryBuilderFactory(IServiceProvider provider)
        {
            this._provider = provider;
        }

        public QueryBuilder GetQueryBuilder(string searchEntityIdentifier)
        {
            switch (searchEntityIdentifier.ToLower())
            {
                case "search_encounter":
                    return this._provider.GetService(typeof(EncounterSearchQueryBuilder)) as EncounterSearchQueryBuilder;
                case "search_observation":
                    return this._provider.GetService(typeof(ObservationResultSearchQueryBuilder)) as ObservationResultSearchQueryBuilder;
                case "search_patient":
                    return this._provider.GetService(typeof(PatientSearchQueryBuilder)) as PatientSearchQueryBuilder;
            }
            return null;
        }
    }
}
