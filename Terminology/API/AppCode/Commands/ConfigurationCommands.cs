//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

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


﻿using AutoMapper;
using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Config;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Commands
{
    public class ConfigurationCommands : IConfigurationCommands
    {
        private IRepository<FormularyRuleConfig> _configRepo;
        private IMapper _mapper;

        public ConfigurationCommands(IRepository<FormularyRuleConfig> configRepo, IMapper mapper)
        {
            _configRepo = configRepo;
            _mapper = mapper;
        }
        public async Task<FormularyRuleConfigDTO> SaveConfiguration(FormularyRuleConfigRequest request)
        {
            //check if exists
            var pk = request.RowId != null ? request.RowId.Trim() : null;

            var prevConfigs = _configRepo.Items.Where(c => (pk == null && c.Name.Trim() == request.Name.Trim()) || (request.RowId != null && c.RowId == request.RowId)).ToList();

            if (pk != null && prevConfigs.IsCollectionValid())
            {
                prevConfigs.Each(c =>
                {
                    c.Recordstatus = 2;//In-active

                    _configRepo.Update(c);
                });
            }

            var configModel = new FormularyRuleConfig();

            request.RowId = null;

            configModel = _mapper.Map(request, configModel);

            _configRepo.Add(configModel);

            var savedStsCd = await _configRepo.SaveChangesAsync();

            if (savedStsCd == -1)
            {
                return null;
            }

            var newDto = _mapper.Map<FormularyRuleConfigDTO>(configModel);
            return newDto;
        }
    }
}
