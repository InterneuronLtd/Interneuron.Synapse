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
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.Model.DomainModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Queries
{
    public abstract class RuleBoundBaseFormularyBuilder
    {
        protected FormularyDTO _formularyDTO = new FormularyDTO();
        protected FormularyHeader _formularyDAO;
        protected IServiceProvider _provider;
        protected IMapper _mapper;
        protected IConfiguration _configuration;

        public FormularyDTO FormularyDTO { get { return _formularyDTO; } }

        public RuleBoundBaseFormularyBuilder(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
            _mapper = this._provider.GetService(typeof(IMapper)) as IMapper;
            _configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

        }

        public virtual async Task CreateBase(FormularyHeader formularyDAO)
        {
            _formularyDAO = formularyDAO;

            _formularyDTO = this._mapper.Map<FormularyDTO>(formularyDAO);
        }

        public virtual void CreateDetails()
        {
            var formularyDetailObj = _formularyDAO.FormularyDetail.FirstOrDefault();

            if (formularyDetailObj.IsNotNull())
            {
                _formularyDTO.Detail = this._mapper.Map<FormularyDetailDTO>(formularyDetailObj);
            }
        }

        public virtual void CreateAdditionalCodes()
        {
            if (_formularyDAO.FormularyAdditionalCode.IsCollectionValid())
            {
                _formularyDTO.FormularyAdditionalCodes = this._mapper.Map<List<FormularyAdditionalCodeDTO>>(_formularyDAO.FormularyAdditionalCode);
            }
        }
        public virtual void CreateIngredients()
        {
            if (_formularyDAO.FormularyIngredient.IsCollectionValid())
            {
                _formularyDTO.FormularyIngredients = new List<FormularyIngredientDTO>();

                _formularyDAO.FormularyIngredient.Each(fi =>
                {
                    _formularyDTO.FormularyIngredients.Add(_mapper.Map<FormularyIngredientDTO>(fi));
                });
            }
        }

        public virtual void CreateExcipients()
        {
            if (_formularyDAO.FormularyExcipient.IsCollectionValid())
            {
                _formularyDTO.FormularyExcipients = new List<FormularyExcipientDTO>();

                _formularyDAO.FormularyExcipient.Each(fi =>
                {
                    _formularyDTO.FormularyExcipients.Add(_mapper.Map<FormularyExcipientDTO>(fi));
                });
            }
        }

        public virtual void CreateRouteDetails()
        {
            if (_formularyDAO.FormularyRouteDetail.IsCollectionValid())
            {
                _formularyDTO.FormularyRouteDetails = new List<FormularyRouteDetailDTO>();

                _formularyDAO.FormularyRouteDetail.Each(route =>
                {
                    _formularyDTO.FormularyRouteDetails.Add(_mapper.Map<FormularyRouteDetailDTO>(route));
                });
            }
        }

        public virtual void CreateLocalRouteDetails()
        {
            if (_formularyDAO.FormularyLocalRouteDetail.IsCollectionValid())
            {
                _formularyDTO.FormularyLocalRouteDetails = new List<FormularyLocalRouteDetailDTO>();

                _formularyDAO.FormularyLocalRouteDetail.Each(route =>
                {
                    _formularyDTO.FormularyLocalRouteDetails.Add(_mapper.Map<FormularyLocalRouteDetailDTO>(route));
                });
            }
        }

        //public virtual void CreateOntologyForms()
        //{
        //    if (_formularyDAO.FormularyOntologyForm.IsCollectionValid())
        //    {
        //        _formularyDTO.FormularyOntologyForms = new List<FormularyOntologyFormDTO>();

        //        _formularyDAO.FormularyOntologyForm.Each(rec =>
        //        {
        //            _formularyDTO.FormularyOntologyForms.Add(_mapper.Map<FormularyOntologyFormDTO>(rec));
        //        });
        //    }
        //}

        //public virtual void CreateIndications() 
        //{
        //    if (_formularyDAO.FormularyIndication.IsCollectionValid())
        //    {
        //        _formularyDTO.FormularyIndications = new List<FormularyIndicationDTO>();

        //        _formularyDAO.FormularyIndication.Each(rec => 
        //            {
        //                _formularyDTO.FormularyIndications.Add(_mapper.Map<FormularyIndicationDTO>(rec));
        //            }
        //        );
        //    }
        //}

        public virtual async Task HydrateLookupDescriptions()
        {
            var formularyQueries = this._provider.GetService(typeof(IFormularyQueries)) as IFormularyQueries;

            var uomsLkp = await formularyQueries.GetLookup<DmdLookupUomDTO, string, string>(LookupType.DMDUOM, (rec) => rec.Cd, rec => rec.Desc);
            var doseFormsLkp = await formularyQueries.GetLookup<DmdLookupDrugformindDTO, string, string>(LookupType.DMDDoseForm, rec => rec.Cd.ToString(), rec => rec.Desc);
            var formsLkp = await formularyQueries.GetLookup<DmdLookupFormDTO, string, string>(LookupType.DMDForm, rec => rec.Cd, rec => rec.Desc);
            var strengthsLkp = await formularyQueries.GetLookup<DmdLookupBasisofstrengthDTO, string, string>(LookupType.DMDPharamceuticalStrength, rec => rec.Cd.ToString(), rec => rec.Desc);
            var ingredientsLkp = await formularyQueries.GetLookup<DmdLookupIngredientDTO, string, string>(LookupType.DMDIngredient, rec => rec.Isid.ToString(), rec => rec.Nm);
            var routesLkp = await formularyQueries.GetLookup<DmdLookupRouteDTO, string, string>(LookupType.DMDRoute, rec => rec.Cd, rec => rec.Desc);
            //var routesLkp = await formularyQueries.GetLookup<DmdLookupRouteDTO, string, string>(LookupType.TitrationType, rec => rec.Cd, rec => rec.Desc);

            var titrationTypesLkp = new Dictionary<string, object>();

            var data = await formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.TitrationType);
            var lookupData = data.Where(d => d.Type == LookupType.TitrationType.GetTypeName()).ToList();
            if (lookupData.IsCollectionValid())
                lookupData.Each(d => titrationTypesLkp[d.Cd] = d);


            HydrateLookupsInDetail(uomsLkp, doseFormsLkp, formsLkp, titrationTypesLkp, _formularyDTO);

            HydrateLookupsInIngredients(uomsLkp, strengthsLkp, ingredientsLkp, _formularyDTO);
            await HydrateLookupsInRoutes(formularyQueries, routesLkp, _formularyDTO);
            await HydrateLookupsInLocalRoutes(formularyQueries, routesLkp, _formularyDTO);
        }

        private async Task HydrateLookupsInRoutes(IFormularyQueries formularyQueries, Dictionary<string, string> routesLkp, FormularyDTO formularyDTO)
        {
            if (formularyDTO.FormularyRouteDetails.IsCollectionValid())
            {
                var routeTypes = await formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.RouteFieldType);
                var routeTypesLkp = new Dictionary<string, string>();

                if (routesLkp.IsCollectionValid() && routeTypes.IsCollectionValid())
                {
                    routeTypesLkp = routeTypes.Where(d => d.Type == LookupType.RouteFieldType.GetTypeName())?
                                        .Select(rec => new
                                        {
                                            cd = rec.Cd,
                                            desc = rec.Desc
                                        })
                                        .ToDictionary(k => k.cd, d => d.desc);

                    formularyDTO.FormularyRouteDetails.Each(route =>
                    {
                        if (route.RouteCd.IsNotEmpty() && routesLkp.ContainsKey(route.RouteCd) && routeTypesLkp.ContainsKey(route.RouteFieldTypeCd))
                        {
                            route.RouteDesc = routesLkp[route.RouteCd];
                            route.RouteFieldTypeDesc = routeTypesLkp[route.RouteFieldTypeCd];
                        }
                    });
                }
            }
        }

        private async Task HydrateLookupsInLocalRoutes(IFormularyQueries formularyQueries, Dictionary<string, string> routesLkp, FormularyDTO formularyDTO)
        {
            if (formularyDTO.FormularyLocalRouteDetails.IsCollectionValid())
            {
                var routeTypes = await formularyQueries.GetLookup<FormularyLookupItemDTO>(LookupType.RouteFieldType);
                var routeTypesLkp = new Dictionary<string, string>();

                if (routesLkp.IsCollectionValid() && routeTypes.IsCollectionValid())
                {
                    routeTypesLkp = routeTypes.Where(d => d.Type == LookupType.RouteFieldType.GetTypeName())?
                                        .Select(rec => new
                                        {
                                            cd = rec.Cd,
                                            desc = rec.Desc
                                        })
                                        .ToDictionary(k => k.cd, d => d.desc);

                    formularyDTO.FormularyLocalRouteDetails.Each(route =>
                    {
                        if (route.RouteCd.IsNotEmpty() && routesLkp.ContainsKey(route.RouteCd) && routeTypesLkp.ContainsKey(route.RouteFieldTypeCd))
                        {
                            route.RouteDesc = routesLkp[route.RouteCd];
                            route.RouteFieldTypeDesc = routeTypesLkp[route.RouteFieldTypeCd];
                        }
                    });
                }
            }
        }

        private void HydrateLookupsInIngredients(Dictionary<string, string> uomsLkp, Dictionary<string, string> strengthsLkp, Dictionary<string, string> ingredientsLkp, FormularyDTO formularyDTO)
        {
            if (formularyDTO.FormularyIngredients.IsCollectionValid())
            {
                formularyDTO.FormularyIngredients.Each(ing =>
                {
                    if (ing.BasisOfPharmaceuticalStrengthCd.IsNotEmpty() && strengthsLkp.ContainsKey(ing.BasisOfPharmaceuticalStrengthCd))
                    {
                        ing.BasisOfPharmaceuticalStrengthDesc = strengthsLkp[ing.BasisOfPharmaceuticalStrengthCd];
                    }

                    if (ing.IngredientCd.IsNotEmpty() && ing.IngredientName.IsEmpty() && ingredientsLkp.ContainsKey(ing.IngredientCd))
                    {
                        ing.IngredientName = ingredientsLkp[ing.IngredientCd];
                    }

                    if (ing.StrengthValueDenominatorUnitCd.IsNotEmpty() && uomsLkp.ContainsKey(ing.StrengthValueDenominatorUnitCd))
                    {
                        ing.StrengthValueDenominatorUnitDesc = uomsLkp[ing.StrengthValueDenominatorUnitCd];
                    }
                    if (ing.StrengthValueNumeratorUnitCd.IsNotEmpty() && uomsLkp.ContainsKey(ing.StrengthValueNumeratorUnitCd))
                    {
                        ing.StrengthValueNumeratorUnitDesc = uomsLkp[ing.StrengthValueNumeratorUnitCd];
                    }
                });
            }
        }

        private void HydrateLookupsInDetail(Dictionary<string, string> uomsLkp, Dictionary<string, string> doseFormsLkp, Dictionary<string, string> formsLkp,
            Dictionary<string, object> titrationTypesLkp, FormularyDTO formularyDTO)
        {
            if (formularyDTO.Detail != null)
            {
                if (formularyDTO.Detail.DoseFormCd.IsNotEmpty() && doseFormsLkp.ContainsKey(formularyDTO.Detail.DoseFormCd))
                {
                    formularyDTO.Detail.DoseFormDesc = doseFormsLkp[formularyDTO.Detail.DoseFormCd];
                }

                if (formularyDTO.Detail.UnitDoseFormUnits.IsNotEmpty() && uomsLkp.ContainsKey(formularyDTO.Detail.UnitDoseFormUnits))
                {
                    formularyDTO.Detail.UnitDoseFormUnitsDesc = uomsLkp[formularyDTO.Detail.UnitDoseFormUnits];
                }

                if (formularyDTO.Detail.UnitDoseUnitOfMeasureCd.IsNotEmpty() && uomsLkp.ContainsKey(formularyDTO.Detail.UnitDoseUnitOfMeasureCd))
                {
                    formularyDTO.Detail.UnitDoseUnitOfMeasureDesc = uomsLkp[formularyDTO.Detail.UnitDoseUnitOfMeasureCd];
                }

                if (formularyDTO.Detail.FormCd.IsNotEmpty() && formsLkp.ContainsKey(formularyDTO.Detail.FormCd))
                {
                    formularyDTO.Detail.FormDesc = formsLkp[formularyDTO.Detail.FormCd];
                }

                formularyDTO.Detail.ChildFormulations?.Each(rec =>
                {
                    if (rec != null && rec.Cd.IsNotEmpty())
                        rec.Desc = formsLkp[rec.Cd];
                });

                formularyDTO.Detail.TitrationTypes?.Each(tit =>
                {
                    if (tit.Cd != null && titrationTypesLkp.ContainsKey(tit.Cd))
                    {
                        tit.Desc = ((FormularyLookupItemDTO)titrationTypesLkp[tit.Cd]).Desc;
                        tit.AdditionalProperties = ((FormularyLookupItemDTO)titrationTypesLkp[tit.Cd]).AdditionalProperties;
                    }
                });

            }
        }
    }
}
