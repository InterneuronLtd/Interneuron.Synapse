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
using Microsoft.AspNetCore.Http;
using SynapseStudioWeb.AppCode.Constants;
using SynapseStudioWeb.DataService.APIModel;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models.MedicationMgmt;
using System.Collections.Generic;
using System.Linq;

namespace SynapseStudioWeb.Models.MedicinalMgmt
{
    public class VTMFormularyEditVMBuilder : IFormularyEditVMBuilder
    {
        private IMapper _mapper;
        private HttpContext _httpContext;
        private FormularyHeaderAPIModel _apiModel;

        public VTMFormularyEditVMBuilder(IMapper mapper, HttpContext httpContext, FormularyHeaderAPIModel apiModel)
        {
            _mapper = mapper;
            _httpContext = httpContext;
            _apiModel = apiModel;
        }
        public FormularyEditModel BuildVM()
        {
            var vm = _mapper.Map<FormularyEditModel>(_apiModel);

            //_mapper.Map(_apiModel.Detail, vm);

            //var allAdditionalCodes = _mapper.Map<List<FormularyAdditionalCodeModel>>(_apiModel.FormularyAdditionalCodes);

            //if (allAdditionalCodes.IsCollectionValid())
            //{
            //    vm.FormularyClassificationCodes = allAdditionalCodes.Where(ac => ac.CodeType.IsNotEmpty() && string.Compare(ac.CodeType, "Classification", true) == 0)?.ToList();

            //    vm.FormularyIdentificationCodes = allAdditionalCodes.Where(ac => ac.CodeType.IsNotEmpty() && string.Compare(ac.CodeType, "Identification", true) == 0)?.ToList();
            //}

            //AssignOtherAttrs(vm);

            vm.IsImported = string.Compare(vm.RecSource, "I", true) == 0;
            // vm.FormularyVersionId = _apiModel.FormularyVersionId;
            vm.OriginalStatus = vm.Status;
            vm.ControlIdentifier = "Edit_Formulary";

            return vm;
        }

        //private void AssignRouteAttrs(List<CodeNameSelectorModel> routesList)
        //{
        //    if (routesList.IsCollectionValid())
        //    {
        //        routesList.Each(rt =>
        //        {
        //            rt.SafeAssignCodeNameFromSession(SynapseSession.RoutesLkpKey, _httpContext);
        //            rt.IsReadonly = (rt.Source.IsNotEmpty() && string.Compare(rt.Source, "dmd", true) == 0);
        //            rt.Source = rt.Source.IsNotEmpty() ? rt.Source : "DMD";
        //            rt.SourceColor = TerminologyConstants.ColorForRecordSource.ContainsKey(rt.Source) ? TerminologyConstants.ColorForRecordSource[rt.Source] : rt.SourceColor;
        //        });
        //    }
        //}

        //private void AssignOtherAttrs(FormularyEditModel vm)
        //{
        //    AssignRouteAttrs(vm.Route);
        //    AssignRouteAttrs(vm.UnlicensedRoute);
        //}
    }
}
