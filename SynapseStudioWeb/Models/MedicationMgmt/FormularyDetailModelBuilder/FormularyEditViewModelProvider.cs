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
using Microsoft.AspNetCore.Http;
using SynapseStudioWeb.DataService.APIModel;
using SynapseStudioWeb.Models.MedicationMgmt;
using System;

namespace SynapseStudioWeb.Models.MedicinalMgmt
{
    public class FormularyEditViewModelProvider
    {
        private IMapper _mapper;
        private HttpContext _context;
        public Func<FormularyHeaderAPIModel> ApiModelProvider;

        public FormularyEditViewModelProvider(IMapper mapper, HttpContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public FormularyEditModel GetFormularyEditVM()
        {
            if (ApiModelProvider == null) return null;

            var apiModel = ApiModelProvider();

            if (apiModel == null) return null;

            if (string.Compare(apiModel.ProductType, "vmp", true) == 0)
                return new VMPFormularyEditVMBuilder(_mapper, _context, apiModel).BuildVM();
            if (string.Compare(apiModel.ProductType, "amp", true) == 0)
                return new AMPFormularyEditVMBuilder(_mapper, _context, apiModel).BuildVM();
            if (string.Compare(apiModel.ProductType, "vtm", true) == 0)
                return new VTMFormularyEditVMBuilder(_mapper, _context, apiModel).BuildVM();

            return null;
        }
    }
}
