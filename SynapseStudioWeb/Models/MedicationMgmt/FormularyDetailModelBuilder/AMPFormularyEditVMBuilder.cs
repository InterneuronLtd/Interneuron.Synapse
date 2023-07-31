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
using Microsoft.AspNetCore.Http;
using SynapseStudioWeb.AppCode.Constants;
using SynapseStudioWeb.DataService.APIModel;
using SynapseStudioWeb.Helpers;
using SynapseStudioWeb.Models.MedicationMgmt;
using System.Collections.Generic;

namespace SynapseStudioWeb.Models.MedicinalMgmt
{
    public class AMPFormularyEditVMBuilder : IFormularyEditVMBuilder
    {
        private IMapper _mapper;
        private HttpContext _httpContext;
        private FormularyHeaderAPIModel _apiModel;

        public AMPFormularyEditVMBuilder(IMapper mapper, HttpContext httpContext, FormularyHeaderAPIModel apiModel)
        {
            _mapper = mapper;
            _httpContext = httpContext;
            _apiModel = apiModel;
        }

        public FormularyEditModel BuildVM()
        {
            var ampVM = _mapper.Map<FormularyEditModel>(_apiModel);

            //_mapper.Map(_apiModel.Detail, ampVM);

            //ampVM.FormularyClassificationCodes = _mapper.Map<List<FormularyAdditionalCodeModel>>(_apiModel.FormularyAdditionalCodes);

            //ampVM.Ingredients = _mapper.Map<List<FormularyIngredientModel>>(_apiModel.FormularyIngredients);

            //AssignAdditionalAttrs(ampVM);

            ampVM.IsImported = string.Compare(ampVM.RecSource, "I", true) == 0;
            //ampVM.FormularyVersionId = _apiModel.FormularyVersionId;
            ampVM.OriginalStatus = ampVM.Status;
            ampVM.ControlIdentifier = "Edit_Formulary";

            return ampVM;
        }

        //private void AssignAdditionalAttrs(FormularyEditModel ampVM)
        //{
        //    ampVM.FormCd.SafeAssignCodeNameFromSession(SynapseSession.FormsLkpKey, _httpContext);
        //    ampVM.Supplier.SafeAssignCodeNameFromSession(SynapseSession.SupplierLkpKey, _httpContext);
        //    ampVM.UnitDoseFormUnits.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContext);
        //    ampVM.UnitDoseUnitOfMeasure.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContext);
        //    //ampVM.MaximumDoseUnit.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContext);

        //    AssignIngredientsAttrs(ampVM);
        //    //AssignFormNRouteAttrs(ampVM);
        //    AssignRouteAttrs(ampVM.Route);
        //    AssignRouteAttrs(ampVM.UnlicensedRoute);
        //}

        //private void AssignIngredientsAttrs(FormularyEditModel ampVM)
        //{
        //    if (ampVM.Ingredients.IsCollectionValid())
        //    {
        //        ampVM.Ingredients.Each(ing =>
        //        {
        //            ing.Ingredient.SafeAssignCodeNameFromSession(SynapseSession.IngredientsLkpKey, _httpContext);
        //            ing.StrengthValueNumeratorUnit.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContext);
        //            ing.StrengthValueDenominatorUnit.SafeAssignCodeNameFromSession(SynapseSession.UOMsLkpKey, _httpContext);
        //        });
        //    }
        //}

        ////private void AssignFormNRouteAttrs(FormularyEditModel ampVM)
        ////{
        ////    if (ampVM.FormNRoute.IsCollectionValid())
        ////    {
        ////        ampVM.FormNRoute.Each(ing => ing.SafeAssignCodeNameFromSession(SynapseSession.FormNRoutesLkpKey, _httpContext));
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
