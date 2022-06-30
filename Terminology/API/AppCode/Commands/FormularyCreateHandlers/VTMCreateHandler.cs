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


﻿using AutoMapper;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.Model.DomainModels;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Commands.FormularyCreateHandlers
{
    public class VTMCreateHandler: BaseCreateHandler
    {
        public VTMCreateHandler(IMapper mapper, FormularyDTO request) : base(mapper, request)
        {
        }

        public override FormularyHeader CreateFormulary()
        {
            var formularyHeader = CreateHeader();
            formularyHeader.ProductType = "VTM";
            formularyHeader.ParentProductType = null;
            PopulateFormularyDetail(formularyHeader);
            return formularyHeader;
        }
    }
}
