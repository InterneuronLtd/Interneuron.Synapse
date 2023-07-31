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
﻿using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.Model.DomainModels;

namespace Interneuron.Terminology.API.AppCode.Commands.ImportRules
{
    public class NullImportRule : IImportRule
    {
        private DMDDetailResultDTO _dMDDetailResultDTO;
        private FormularyHeader _formularyDAO;

        public NullImportRule(DMDDetailResultDTO dMDDetailResultDTO, FormularyHeader formularyDAO)
        {
            _dMDDetailResultDTO = dMDDetailResultDTO;

            _formularyDAO = formularyDAO;
        }

        public void MutateByRules()
        {
        }
    }
}
