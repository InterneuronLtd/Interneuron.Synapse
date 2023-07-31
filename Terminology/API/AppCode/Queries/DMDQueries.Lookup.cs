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
﻿using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public partial class DMDQueries
    {
        private async Task<ConcurrentDictionary<string, List<DmdLookupRouteDTO>>> GetRoutesLookupForCodes(IEnumerable<string> dmdCodes)
        {
            if (!dmdCodes.IsCollectionValid()) return null;

            var uniqueCodes = dmdCodes.Distinct();

            var routesMapped = new ConcurrentDictionary<string, List<DmdLookupRouteDTO>>();

            var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdVmpDrugroute>)) as IReadOnlyRepository<DmdVmpDrugroute>;

            var items = repo.ItemsAsReadOnly.Where(r => r.Vpid != null && uniqueCodes.Contains(r.Vpid.ToString()) && r.Recordstatus == 1);

            if (!items.IsCollectionValid()) return null;

            var routes = await this.GetLookup<DmdLookupRouteDTO>(LookupType.DMDRoute);

            var lookup = routes.Distinct(r => r.Cd).ToDictionary(k => k.Cd, v => v);

            items.AsParallel().Each(item =>
            {
                if (lookup.ContainsKey(item.Routecd.ToString()))
                {
                    if (!routesMapped.ContainsKey(item.Routecd.ToString()))
                    {
                        routesMapped[item.Routecd.ToString()] = new List<DmdLookupRouteDTO>() { lookup[item.Routecd.ToString()] };
                    }
                    else
                    {
                        routesMapped[item.Routecd.ToString()].Add(lookup[item.Routecd.ToString()]);
                    }
                }
            });

            return routesMapped;
        }

        private async Task<ConcurrentDictionary<string, List<DmdLookupFormDTO>>> GetFormsLookupForCodes(IEnumerable<string> dmdCodes)
        {
            if (!dmdCodes.IsCollectionValid()) return null;

            var uniqueCodes = dmdCodes.Distinct();

            var routesMapped = new ConcurrentDictionary<string, List<DmdLookupFormDTO>>();

            var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdVmpDrugform>)) as IReadOnlyRepository<DmdVmpDrugform>;

            var items = repo.ItemsAsReadOnly.Where(r => r.Vpid != null && uniqueCodes.Contains(r.Vpid.ToString()) && r.Recordstatus == 1);

            if (!items.IsCollectionValid()) return null;

            var forms = await this.GetLookup<DmdLookupFormDTO>(LookupType.DMDForm);

            var lookup = forms.Distinct(r => r.Cd).ToDictionary(k => k.Cd, v => v);

            items.AsParallel().Each(item =>
            {
                if (lookup.ContainsKey(item.Formcd))
                {
                    if (!routesMapped.ContainsKey(item.Formcd.ToString()))
                    {
                        routesMapped[item.Formcd.ToString()] = new List<DmdLookupFormDTO>() { lookup[item.Formcd] };
                    }
                    else
                    {
                        routesMapped[item.Formcd.ToString()].Add(lookup[item.Formcd]);
                    }
                }
            });

            return routesMapped;
        }

        private async Task<ConcurrentDictionary<string, List<DmdLookupPrescribingstatusDTO>>> GetPrescribingStatusLookupForCodes(IEnumerable<string> dmdCodes)
        {
            if (!dmdCodes.IsCollectionValid()) return null;

            var uniqueCodes = dmdCodes.Distinct();

            var routesMapped = new ConcurrentDictionary<string, List<DmdLookupPrescribingstatusDTO>>();

            var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdVmp>)) as IReadOnlyRepository<DmdVmp>;

            var items = repo.ItemsAsReadOnly.Where(r => r.Vpid != null && uniqueCodes.Contains(r.Vpid.ToString()) && r.Recordstatus == 1);

            if (!items.IsCollectionValid()) return null;

            var presStatuses = await this.GetLookup<DmdLookupPrescribingstatusDTO>(LookupType.DMDPrescribingStatus);

            var lookup = presStatuses.Distinct(r => r.Cd).ToDictionary(k => k.Cd, v => v);

            items.AsParallel().Each(item =>
            {
                if (lookup.ContainsKey(item.PresStatcd))
                {
                    if (!routesMapped.ContainsKey(item.PresStatcd.ToString()))
                    {
                        routesMapped[item.PresStatcd.ToString()] = new List<DmdLookupPrescribingstatusDTO>() { lookup[item.PresStatcd] };
                    }
                    else
                    {
                        routesMapped[item.PresStatcd.ToString()].Add(lookup[item.PresStatcd]);
                    }
                }
            });

            return routesMapped;
        }

        private async Task<ConcurrentDictionary<string, List<DmdLookupControldrugcatDTO>>> GetDrugCatLookupForCodes(IEnumerable<string> dmdCodes)
        {
            if (!dmdCodes.IsCollectionValid()) return null;

            var uniqueCodes = dmdCodes.Distinct();

            var routesMapped = new ConcurrentDictionary<string, List<DmdLookupControldrugcatDTO>>();

            var repo = this._provider.GetService(typeof(IReadOnlyRepository<DmdVmpControldrug>)) as IReadOnlyRepository<DmdVmpControldrug>;

            var items = repo.ItemsAsReadOnly.Where(r => r.Vpid != null && uniqueCodes.Contains(r.Vpid.ToString()) && r.Recordstatus == 1);

            if (!items.IsCollectionValid()) return null;

            var categories = await this.GetLookup<DmdLookupControldrugcatDTO>(LookupType.DMDControlDrugCategory);

            var lookup = categories.Distinct(r => r.Cd).ToDictionary(k => k.Cd, v => v);

            items.AsParallel().Each(item =>
            {
                if (lookup.ContainsKey(item.Catcd))
                {
                    if (!routesMapped.ContainsKey(item.Catcd.ToString()))
                    {
                        routesMapped[item.Catcd.ToString()] = new List<DmdLookupControldrugcatDTO>() { lookup[item.Catcd] };
                    }
                    else
                    {
                        routesMapped[item.Catcd.ToString()].Add(lookup[item.Catcd]);
                    }
                }
            });

            return routesMapped;
        }
    }
}
