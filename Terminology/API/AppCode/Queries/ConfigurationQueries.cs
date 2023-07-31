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
﻿using AutoMapper;
using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Config;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public class ConfigurationQueries : IConfigurationQueries
    {
        private IReadOnlyRepository<FormularyRuleConfig> _configRepo;
        private IMapper _mapper;

        public ConfigurationQueries(IReadOnlyRepository<FormularyRuleConfig> configRepo, IMapper mapper)
        {
            _configRepo = configRepo;
            _mapper = mapper;
        }
        public FormularyRuleConfigDTO GetConfigurationByName(string name)
        {
            if (name.IsEmpty()) { return null; }

            var configDetail = _configRepo.ItemsAsReadOnly.Where(c => c.Name.ToLower() == name.ToLower() && c.Recordstatus == 1).FirstOrDefault();

            if (configDetail == null) { return null; }

            return _mapper.Map<FormularyRuleConfigDTO>(configDetail);

        }
    }
}
