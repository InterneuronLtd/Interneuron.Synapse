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

namespace SynapseStudioWeb.Models.MedicinalMgmt
{
    public class VMPFormularyEditVMBuilder : IFormularyEditVMBuilder
    {
        private IMapper _mapper;
        private HttpContext _httpContext;
        private FormularyHeaderAPIModel _apiModel;

        public VMPFormularyEditVMBuilder(IMapper mapper, HttpContext httpContext, FormularyHeaderAPIModel apiModel)
        {
            _mapper = mapper;
            _httpContext = httpContext;
            _apiModel = apiModel;
        }

        public FormularyEditModel BuildVM()
        {
            var vmpVM = _mapper.Map<FormularyEditModel>(_apiModel);

            //_mapper.Map(_apiModel.Detail, vmpVM);

            ////vmpVM.FormularyAdditionalCodes = _mapper.Map<List<FormularyAdditionalCodeModel>>(_apiModel.FormularyAdditionalCodes);

            //vmpVM.Ingredients = _mapper.Map<List<FormularyIngredientModel>>(_apiModel.FormularyIngredients);

            //AssignAdditionalAttrs(vmpVM);

            vmpVM.IsImported = string.Compare(vmpVM.RecSource, "I", true) == 0;
            //vmpVM.FormularyVersionId = _apiModel.FormularyVersionId;
            vmpVM.OriginalStatus = vmpVM.Status;
            vmpVM.ControlIdentifier = "Edit_Formulary";

            return vmpVM;
        }

        //private void AssignAdditionalAttrs(FormularyEditModel vmpVM)
        //{
        //    vmpVM.FormCd.SafeAssignCodeNameFromSession(SynapseSession.FormsLkpKey, _httpContext);
        //    //vmpVM.Supplier.SafeAssignCodeNameFromSession(SynapseSession.SupplierLkpKey, _httpContext);
        //    vmpVM.UnitDoseFormUnits.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContext);
        //    vmpVM.UnitDoseUnitOfMeasure.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContext);
        //    //vmpVM.MaximumDoseUnit.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContext);

        //    AssignIngredientsAttrs(vmpVM);
        //    //AssignFormNRouteAttrs(vmpVM);
        //    AssignRouteAttrs(vmpVM.Route);
        //    AssignRouteAttrs(vmpVM.UnlicensedRoute);
        //}

        //private void AssignIngredientsAttrs(FormularyEditModel vmpVM)
        //{
        //    if (vmpVM.Ingredients.IsCollectionValid())
        //    {
        //        vmpVM.Ingredients.Each(ing =>
        //        {
        //            ing.Ingredient.SafeAssignCodeNameFromSession(SynapseSession.IngredientsLkpKey, _httpContext);
        //            ing.StrengthValueNumeratorUnit.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContext);
        //            ing.StrengthValueDenominatorUnit.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContext);
        //        });
        //    }
        //}

        ////private void AssignFormNRouteAttrs(VMPFormularyEditModel vmpVM)
        ////{
        ////    if (vmpVM.FormNRoute.IsCollectionValid())
        ////    {
        ////        vmpVM.FormNRoute.Each(ing => ing.SafeAssignCodeNameFromSession(SynapseSession.FormNRoutesLkpKey, _httpContext));
        ////    }
        ////}

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
    }
}
