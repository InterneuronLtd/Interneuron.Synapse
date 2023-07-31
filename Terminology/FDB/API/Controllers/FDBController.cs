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


﻿using FirstDataBank.DrugServer.API;
using FirstDataBank.DrugServer.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using InterneuronFDBAPI.Models;
using InterneuronFDBAPI.Infrastructure;
using Serilog;
using System.Web.Http.Results;
using Serilog.Core;
using System.Web.Script.Serialization;

namespace InterneuronFDBAPI.Controllers
{

    [RoutePrefix("api/fdb")]
    public class FDBController : ApiController
    {

        [FDBAuthorization]
        [HttpPost]
        [Route("GetCautionsByCodes")]
        public IHttpActionResult GetCautionsByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var response = new Dictionary<string, List<string>>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                try
                {
                    var productCautions = GetCautionsForCode(req.ProductType, req.ProductCode);

                    if (productCautions != null && productCautions.Count > 0)
                        response[req.ProductCode] = productCautions.Distinct().ToList();
                }
                catch (FDBApplicationException fdbAppException)
                {
                    //Intentionally suppressing the exception
                }
                catch (Exception ex)
                {
                    //Intentionally suppressing the exception
                }
            }
            return Ok(response);
        }


        [FDBAuthorization]
        [HttpGet]
        [Route("GetCautionsByCode")]
        public IHttpActionResult GetCautionsByCode(string productType, string productCode)
        {
            try
            {
                var productCautions = GetCautionsForCode(productType, productCode);

                return Ok(productCautions.Distinct());
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private List<String> GetCautionsForCode(string productType, string productCode)
        {
            List<String> productCautions = new List<String>();

            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
            filter.DrugClass.Brand = true;
            filter.DrugClass.BrandedGeneric = true;

            if (string.Compare(productType, "VTM", true) == 0)
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Drug, productCode);

                Drug drug = drugSystem.Navigation.GetDrugById(drugBase);

                Product[] products = drug.GetRelatedProducts(filter);

                foreach (Product product in products)
                {
                    DrugBase drugBaseProduct = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, product.SingleId);

                    Product drugProduct = drugSystem.Navigation.GetProductById(drugBaseProduct);

                    Warning[] warnings = drugProduct.GetWarnings();

                    if (warnings != null && warnings.Length > 0)
                    {
                        productCautions.AddRange(warnings.Select(c => c.Text));
                    }
                }
            }
            else
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, productCode);

                Product product = drugSystem.Navigation.GetProductById(drugBase);

                Warning[] warnings = product.GetWarnings();

                if (warnings != null && warnings.Length > 0)
                {
                    productCautions.AddRange(warnings.Select(c => c.Text));
                }
            }

            return productCautions;
        }

        [HttpGet]
        [Route("GetCautionsByName")]
        public IHttpActionResult GetCautionsByName(string productType, string productName)
        {
            List<String> productCautions = new List<String>();

            try
            {
                DrugSystem drugSystem = InitializeDrugSystem();

                // Note - you could use a FilterUK here if required
                FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
                filter.DrugClass.Brand = true;
                filter.DrugClass.BrandedGeneric = true;

                if (string.Compare(productType, "VTM", true) == 0)
                {
                    Drug[] drugs = drugSystem.Navigation.GetDrugsByName(productName, filter);

                    if (drugs != null && drugs.Length > 0)
                    {
                        foreach (Drug drug in drugs)
                        {
                            Product[] products = drug.GetRelatedProducts(filter);

                            if (products != null && products.Length > 0)
                            {
                                foreach (Product product in products)
                                {
                                    Warning[] warnings = product.GetWarnings();

                                    if (warnings != null && warnings.Length > 0)
                                    {
                                        productCautions.AddRange(warnings.Select(c => c.Text));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Product[] products = drugSystem.Navigation.GetProductsByName(productName, filter);

                    if (products != null && products.Length > 0)
                    {
                        foreach (Product product in products)
                        {
                            Warning[] warnings = product.GetWarnings();

                            if (warnings != null && warnings.Length > 0)
                            {
                                productCautions.AddRange(warnings.Select(c => c.Text));
                            }
                        }
                    }
                }

                return Ok(productCautions.Distinct());
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("GetContraIndicationsByCodes")]
        public IHttpActionResult GetContraIndicationsByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var response = new Dictionary<string, List<ContraindicationsModel>>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                try
                {
                    var result = GetContraIndicationsForCode(req.ProductType, req.ProductCode);

                    if (result != null && result.Count > 0)
                        response[req.ProductCode] = result.ToList();
                }
                catch (FDBApplicationException fdbAppException)
                {
                    //Log.Logger.Error(fdbAppException, fdbAppException.Message);
                    //Intentionally suppressing the exception
                }
                catch (Exception ex)
                {
                    //Log.Logger.Error(ex, ex.Message);
                    //Intentionally suppressing the exception
                }
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetContraIndicationsByCode")]
        public IHttpActionResult GetContraIndicationsByCode(string productType, string productCode)
        {
            try
            {
                var contraIndications = GetContraIndicationsForCode(productType, productCode);

                return Ok(contraIndications);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private List<ContraindicationsModel> GetContraIndicationsForCode(string productType, string productCode)
        {
            List<ContraindicationsModel> contraIndications = new List<ContraindicationsModel>();


            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
            filter.DrugClass.Brand = true;
            filter.DrugClass.BrandedGeneric = true;

            if (string.Compare(productType, "VTM", true) == 0)
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Drug, productCode);

                Drug drug = drugSystem.Navigation.GetDrugById(drugBase);

                Product[] products = drug.GetRelatedProducts(filter);

                foreach (Product product in products)
                {
                    DrugBase drugBaseProduct = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, product.SingleId);

                    Product drugProduct = drugSystem.Navigation.GetProductById(drugBaseProduct);

                    ContraPrec[] contraPrecs = product.GetContraindicationsAndPrecautions();

                    if (contraPrecs != null && contraPrecs.Length > 0)
                    {
                        foreach (ContraPrec contraPrec in contraPrecs)
                        {
                            if (contraPrec.ConditionAlertSeverity == ConditionAlertSeverity.Contraindication)
                            {
                                contraIndications.Add(new ContraindicationsModel() { Id = contraPrec.Id.ToString(), Text = contraPrec.Text });
                            }
                        }
                    }
                }
            }
            else
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, productCode);

                Product product = drugSystem.Navigation.GetProductById(drugBase);

                ContraPrec[] contraPrecs = product.GetContraindicationsAndPrecautions();

                if (contraPrecs != null && contraPrecs.Length > 0)
                {
                    foreach (ContraPrec contraPrec in contraPrecs)
                    {
                        if (contraPrec.ConditionAlertSeverity == ConditionAlertSeverity.Contraindication)
                        {
                            contraIndications.Add(new ContraindicationsModel() { Id = contraPrec.Id.ToString(), Text = contraPrec.Text });
                        }
                    }
                }
            }

            if (contraIndications == null) return null;

            var uniqueList = (from dataList in contraIndications
                              select dataList).GroupBy(n => new { n.Id, n.Text })
                                       .Select(g => g.FirstOrDefault())
                                       .ToList();

            return uniqueList;
        }

        [HttpGet]
        [Route("GetContraIndicationsByName")]
        public IHttpActionResult GetContraIndicationsByName(string productType, string productName)
        {
            List<ContraindicationsModel> contraIndications = new List<ContraindicationsModel>();

            try
            {
                DrugSystem drugSystem = InitializeDrugSystem();

                // Note - you could use a FilterUK here if required
                FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
                filter.DrugClass.Brand = true;
                filter.DrugClass.BrandedGeneric = true;

                if (string.Compare(productType, "VTM", true) == 0)
                {
                    Drug[] drugs = drugSystem.Navigation.GetDrugsByName(productName, filter);

                    if (drugs != null && drugs.Length > 0)
                    {
                        foreach (Drug drug in drugs)
                        {
                            Product[] products = drug.GetRelatedProducts(filter);

                            if (products != null && products.Length > 0)
                            {
                                foreach (Product product in products)
                                {
                                    ContraPrec[] contraPrecs = product.GetContraindicationsAndPrecautions();

                                    if (contraPrecs != null && contraPrecs.Length > 0)
                                    {
                                        foreach (ContraPrec contraPrec in contraPrecs)
                                        {
                                            if (contraPrec.ConditionAlertSeverity == ConditionAlertSeverity.Contraindication)
                                            {
                                                contraIndications.Add(new ContraindicationsModel() { Id = contraPrec.Id.ToString(), Text = contraPrec.Text });
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Product[] products = drugSystem.Navigation.GetProductsByName(productName, filter);

                    if (products != null && products.Length > 0)
                    {
                        foreach (Product product in products)
                        {
                            ContraPrec[] contraPrecs = product.GetContraindicationsAndPrecautions();

                            if (contraPrecs != null && contraPrecs.Length > 0)
                            {
                                foreach (ContraPrec contraPrec in contraPrecs)
                                {
                                    if (contraPrec.ConditionAlertSeverity == ConditionAlertSeverity.Contraindication)
                                    {
                                        contraIndications.Add(new ContraindicationsModel() { Id = contraPrec.Id.ToString(), Text = contraPrec.Text });
                                    }
                                }
                            }
                        }
                    }
                }

                var uniqueList = (from dataList in contraIndications
                                  select dataList).GroupBy(n => new { n.Id, n.Text })
                           .Select(g => g.FirstOrDefault())
                           .ToList();

                return Ok(uniqueList);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("GetSideEffectsByCodes")]
        public IHttpActionResult GetSideEffectsByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var response = new Dictionary<string, List<string>>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                try
                {
                    var result = GetSideEffectsForCode(req.ProductType, req.ProductCode);

                    if (result != null && result.Count > 0)
                        response[req.ProductCode] = result.Distinct().ToList();
                }
                catch (FDBApplicationException fdbAppException)
                {
                    //Log.Logger.Error(fdbAppException, fdbAppException.Message);
                    //Intentionally suppressing the exception
                }
                catch (Exception ex)
                {
                    //Log.Logger.Error(ex, ex.Message);
                    //Intentionally suppressing the exception
                }
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetSideEffectsByCode")]
        public IHttpActionResult GetSideEffectsByCode(string productType, string productCode)
        {
            try
            {
                var productSideEffects = GetSideEffectsForCode(productType, productCode);

                return Ok(productSideEffects.Distinct());
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private List<String> GetSideEffectsForCode(string productType, string productCode)
        {
            List<String> productSideEffects = new List<String>();

            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
            filter.DrugClass.Brand = true;
            filter.DrugClass.BrandedGeneric = true;

            if (string.Compare(productType, "VTM", true) == 0)
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Drug, productCode);

                Drug drug = drugSystem.Navigation.GetDrugById(drugBase);

                Product[] products = drug.GetRelatedProducts(filter);

                foreach (Product product in products)
                {
                    DrugBase drugBaseProduct = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, product.SingleId);

                    Product drugProduct = drugSystem.Navigation.GetProductById(drugBaseProduct);

                    SideEffect[] sideEffects = product.GetSideEffects();

                    if (sideEffects != null && sideEffects.Length > 0)
                    {
                        productSideEffects.AddRange(sideEffects.Select(c => c.Text));
                    }
                }
            }
            else
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, productCode);

                Product product = drugSystem.Navigation.GetProductById(drugBase);

                SideEffect[] sideEffects = product.GetSideEffects();

                if (sideEffects != null && sideEffects.Length > 0)
                {
                    productSideEffects.AddRange(sideEffects.Select(c => c.Text));
                }
            }

            return productSideEffects;
        }

        [HttpGet]
        [Route("GetSideEffectsByName")]
        public IHttpActionResult GetSideEffectsByName(string productType, string productName)
        {
            List<String> productSideEffects = new List<String>();

            try
            {
                DrugSystem drugSystem = InitializeDrugSystem();

                // Note - you could use a FilterUK here if required
                FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
                filter.DrugClass.Brand = true;
                filter.DrugClass.BrandedGeneric = true;

                if (string.Compare(productType, "VTM", true) == 0)
                {
                    Drug[] drugs = drugSystem.Navigation.GetDrugsByName(productName, filter);

                    if (drugs != null && drugs.Length > 0)
                    {
                        foreach (Drug drug in drugs)
                        {
                            Product[] products = drug.GetRelatedProducts(filter);

                            if (products != null && products.Length > 0)
                            {
                                foreach (Product product in products)
                                {
                                    SideEffect[] sideEffects = product.GetSideEffects();

                                    if (sideEffects != null && sideEffects.Length > 0)
                                    {
                                        productSideEffects.AddRange(sideEffects.Select(c => c.Text));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Product[] products = drugSystem.Navigation.GetProductsByName(productName, filter);

                    if (products != null && products.Length > 0)
                    {
                        foreach (Product product in products)
                        {
                            SideEffect[] sideEffects = product.GetSideEffects();

                            if (sideEffects != null && sideEffects.Length > 0)
                            {
                                productSideEffects.AddRange(sideEffects.Select(c => c.Text));
                            }
                        }
                    }
                }

                return Ok(productSideEffects.Distinct());
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("GetSafetyMessagesByCodes")]
        public IHttpActionResult GetSafetyMessagesByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var response = new Dictionary<string, List<string>>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                try
                {
                    var result = GetSafetyMessagesForCode(req.ProductType, req.ProductCode);

                    if (result != null && result.Count > 0)
                        response[req.ProductCode] = result.Distinct().ToList();
                }
                catch (FDBApplicationException fdbAppException)
                {
                    //Log.Logger.Error(fdbAppException, fdbAppException.Message);
                    //Intentionally suppressing the exception
                }
                catch (Exception ex)
                {
                    //Log.Logger.Error(ex, ex.Message);
                    //Intentionally suppressing the exception
                }
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetSafetyMessagesByCode")]
        public IHttpActionResult GetSafetyMessagesByCode(string productType, string productCode)
        {
            try
            {
                var productSafetyMessages = GetSafetyMessagesForCode(productType, productCode);

                return Ok(productSafetyMessages.Distinct());
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }



        private List<String> GetSafetyMessagesForCode(string productType, string productCode)
        {
            List<String> productSafetyMessages = new List<String>();

            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
            filter.DrugClass.Brand = true;
            filter.DrugClass.BrandedGeneric = true;

            if (string.Compare(productType, "VTM", true) == 0)
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Drug, productCode);

                Drug drug = drugSystem.Navigation.GetDrugById(drugBase);

                Product[] products = drug.GetRelatedProducts(filter);

                foreach (Product product in products)
                {
                    DrugBase drugBaseProduct = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, product.SingleId);

                    ProductUK productUK = (ProductUK)drugSystem.Navigation.GetProductById(drugBaseProduct);

                    MedicineSafetyMessage[] safetyMessages = productUK.GetMedicineSafetyMessages();

                    if (safetyMessages != null && safetyMessages.Length > 0)
                    {
                        productSafetyMessages.AddRange(safetyMessages.Select(c => c.SafetyMessage));
                    }
                }
            }
            else
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, productCode);

                ProductUK productUK = (ProductUK)drugSystem.Navigation.GetProductById(drugBase);

                MedicineSafetyMessage[] safetyMessages = productUK.GetMedicineSafetyMessages();

                if (safetyMessages != null && safetyMessages.Length > 0)
                {
                    productSafetyMessages.AddRange(safetyMessages.Select(c => c.SafetyMessage));
                }
            }

            return productSafetyMessages;
        }


        [HttpPost]
        [Route("GetHighRiskFlagByCodes")]
        public IHttpActionResult GetHighRiskByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var response = new Dictionary<string, bool?>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                try
                {
                    response[req.ProductCode] = GetHighRiskFlagForCode(req.ProductType, req.ProductCode);
                }
                catch (FDBApplicationException fdbAppException)
                {
                    //Log.Logger.Error(fdbAppException, fdbAppException.Message);
                    //Intentionally suppressing the exception
                }
                catch (Exception ex)
                {
                    //Log.Logger.Error(ex, ex.Message);
                    //Intentionally suppressing the exception
                }
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetHighRiskFlagByCode")]
        public IHttpActionResult GetHighRiskFlagByCode(string productType, string productCode)
        {
            try
            {
                var highRiskFlag = GetHighRiskFlagForCode(productType, productCode);

                return Ok(highRiskFlag);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private bool? GetHighRiskFlagForCode(string productType, string productCode)
        {
            if (string.Compare(productType, "VTM", true) == 0) return null;

            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
            filter.DrugClass.Brand = true;
            filter.DrugClass.BrandedGeneric = true;

            DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, productCode);

            ProductUK productUK = (ProductUK)drugSystem.Navigation.GetProductById(drugBase);

            var highRiskFlag = productUK.HighRisk;

            return highRiskFlag;
        }

        [HttpGet]
        [Route("GetSafetyMessagesByName")]
        public IHttpActionResult GetSafetyMessagesByName(string productType, string productName)
        {
            List<String> productSafetyMessages = new List<String>();

            try
            {
                DrugSystem drugSystem = InitializeDrugSystem();

                // Note - you could use a FilterUK here if required
                FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
                filter.DrugClass.Brand = true;
                filter.DrugClass.BrandedGeneric = true;

                if (string.Compare(productType, "VTM", true) == 0)
                {
                    Drug[] drugs = drugSystem.Navigation.GetDrugsByName(productName, filter);

                    if (drugs != null && drugs.Length > 0)
                    {
                        foreach (Drug drug in drugs)
                        {
                            Product[] products = drug.GetRelatedProducts(filter);

                            if (products != null && products.Length > 0)
                            {
                                foreach (Product product in products)
                                {
                                    ProductUK productUK = (ProductUK)product;

                                    MedicineSafetyMessage[] safetyMessages = productUK.GetMedicineSafetyMessages();

                                    if (safetyMessages != null && safetyMessages.Length > 0)
                                    {
                                        productSafetyMessages.AddRange(safetyMessages.Select(c => c.SafetyMessage));
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Product[] products = drugSystem.Navigation.GetProductsByName(productName, filter);

                    if (products != null && products.Length > 0)
                    {
                        foreach (Product product in products)
                        {
                            ProductUK productUK = (ProductUK)product;

                            MedicineSafetyMessage[] safetyMessages = productUK.GetMedicineSafetyMessages();

                            if (safetyMessages != null && safetyMessages.Length > 0)
                            {
                                productSafetyMessages.AddRange(safetyMessages.Select(c => c.SafetyMessage));
                            }
                        }
                    }
                }

                return Ok(productSafetyMessages.Distinct());
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("GetEndorsementByCodes")]
        public IHttpActionResult GetEndorsementByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var response = new Dictionary<string, bool>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                try
                {
                    var result = GetEndorsementForCode(req.ProductType, req.ProductCode);

                    response[req.ProductCode] = result;
                }
                catch (FDBApplicationException fdbAppException)
                {
                    //Log.Logger.Error(fdbAppException, fdbAppException.Message);
                    //Intentionally suppressing the exception
                }
                catch (Exception ex)
                {
                    //Log.Logger.Error(ex, ex.Message);
                    //Intentionally suppressing the exception
                }
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetEndorsementByCode")]
        public IHttpActionResult GetEndorsementByCode(string productType, string productCode)
        {
            try
            {
                var productEndorsement = GetEndorsementForCode(productType, productCode);

                return Ok(productEndorsement);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private bool GetEndorsementForCode(string productType, string productCode)
        {
            bool productEndorsement = false;

            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
            filter.DrugClass.Brand = true;
            filter.DrugClass.BrandedGeneric = true;

            if (string.Compare(productType, "VTM", true) == 0)
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Drug, productCode);

                Drug drug = drugSystem.Navigation.GetDrugById(drugBase);

                Product[] products = drug.GetRelatedProducts(filter);

                foreach (Product product in products)
                {
                    DrugBase drugBaseProduct = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, product.SingleId);

                    ProductUK productUK = (ProductUK)drugSystem.Navigation.GetProductById(drugBaseProduct);

                    productEndorsement = productUK.ACBS;
                }
            }
            else
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, productCode);

                ProductUK productUK = (ProductUK)drugSystem.Navigation.GetProductById(drugBase);

                productEndorsement = productUK.ACBS;
            }

            return productEndorsement;
        }

        [HttpGet]
        [Route("GetEndorsementByName")]
        public IHttpActionResult GetEndorsementByName(string productType, string productName)
        {
            bool productEndorsement = false;

            try
            {
                DrugSystem drugSystem = InitializeDrugSystem();

                // Note - you could use a FilterUK here if required
                FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
                filter.DrugClass.Brand = true;
                filter.DrugClass.BrandedGeneric = true;

                if (string.Compare(productType, "VTM", true) == 0)
                {
                    Drug[] drugs = drugSystem.Navigation.GetDrugsByName(productName, filter);

                    if (drugs != null && drugs.Length > 0)
                    {
                        foreach (Drug drug in drugs)
                        {
                            Product[] products = drug.GetRelatedProducts(filter);

                            if (products != null && products.Length > 0)
                            {
                                foreach (Product product in products)
                                {
                                    ProductUK productUK = (ProductUK)product;

                                    productEndorsement = productUK.ACBS;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Product[] products = drugSystem.Navigation.GetProductsByName(productName, filter);

                    if (products != null && products.Length > 0)
                    {
                        foreach (Product product in products)
                        {
                            ProductUK productUK = (ProductUK)product;

                            MedicineSafetyMessage[] safetyMessages = productUK.GetMedicineSafetyMessages();

                            productEndorsement = productUK.ACBS;
                        }
                    }
                }

                return Ok(productEndorsement);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("GetLicensedUseByCodes")]
        public IHttpActionResult GetLicensedUseByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var response = new Dictionary<string, List<LicensedUseModel>>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                try
                {
                    var result = GetLicensedUseForCode(req.ProductType, req.ProductCode);

                    if (result != null && result.Count > 0)
                        response[req.ProductCode] = result;
                }
                catch (FDBApplicationException fdbAppException)
                {
                    //Log.Logger.Error(fdbAppException, fdbAppException.Message);
                    //Intentionally suppressing the exception
                }
                catch (Exception ex)
                {
                    //Log.Logger.Error(ex, ex.Message);
                    //Intentionally suppressing the exception
                }
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetLicensedUseByCode")]
        public IHttpActionResult GetLicensedUseByCode(string productType, string productCode)
        {

            try
            {
                List<LicensedUseModel> uniqueList = GetLicensedUseForCode(productType, productCode);

                return Ok(uniqueList);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private List<LicensedUseModel> GetLicensedUseForCode(string productType, string productCode)
        {
            List<LicensedUseModel> licensedUses = new List<LicensedUseModel>();

            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
            filter.DrugClass.Brand = true;
            filter.DrugClass.BrandedGeneric = true;

            if (string.Compare(productType, "VTM", true) == 0)
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Drug, productCode);

                Drug drug = drugSystem.Navigation.GetDrugById(drugBase);

                Product[] products = drug.GetRelatedProducts(filter);

                foreach (Product product in products)
                {
                    DrugBase drugBaseProduct = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, product.SingleId);

                    Product drugProduct = drugSystem.Navigation.GetProductById(drugBaseProduct);

                    Indication[] indications = product.GetIndications(false);

                    if (indications != null && indications.Length > 0)
                    {
                        foreach (Indication indication in indications)
                        {
                            licensedUses.Add(new LicensedUseModel() { Id = indication.Id, Text = indication.Text });
                        }
                    }
                }
            }
            else
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, productCode);

                Product product = drugSystem.Navigation.GetProductById(drugBase);

                Indication[] indications = product.GetIndications(false);

                if (indications != null && indications.Length > 0)
                {
                    foreach (Indication indication in indications)
                    {
                        licensedUses.Add(new LicensedUseModel() { Id = indication.Id, Text = indication.Text });
                    }
                }
            }

            if (licensedUses == null) return null;

            var uniqueList = (from dataList in licensedUses
                              select dataList).GroupBy(n => new { n.Id, n.Text })
                               .Select(g => g.FirstOrDefault())
                               .ToList();
            return uniqueList;
        }

        [HttpGet]
        [Route("GetLicensedUseByName")]
        public IHttpActionResult GetLicensedUseByName(string productType, string productName)
        {
            List<LicensedUseModel> licensedUses = new List<LicensedUseModel>();

            try
            {
                DrugSystem drugSystem = InitializeDrugSystem();

                // Note - you could use a FilterUK here if required
                FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
                filter.DrugClass.Brand = true;
                filter.DrugClass.BrandedGeneric = true;

                if (string.Compare(productType, "VTM", true) == 0)
                {
                    Drug[] drugs = drugSystem.Navigation.GetDrugsByName(productName, filter);

                    if (drugs != null && drugs.Length > 0)
                    {
                        foreach (Drug drug in drugs)
                        {
                            Product[] products = drug.GetRelatedProducts(filter);

                            if (products != null && products.Length > 0)
                            {
                                foreach (Product product in products)
                                {
                                    Indication[] indications = product.GetIndications(false);

                                    if (indications != null && indications.Length > 0)
                                    {
                                        foreach (Indication indication in indications)
                                        {
                                            licensedUses.Add(new LicensedUseModel() { Id = indication.Id, Text = indication.Text });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Product[] products = drugSystem.Navigation.GetProductsByName(productName, filter);

                    if (products != null && products.Length > 0)
                    {
                        foreach (Product product in products)
                        {
                            Indication[] indications = product.GetIndications(false);

                            if (indications != null && indications.Length > 0)
                            {
                                foreach (Indication indication in indications)
                                {
                                    licensedUses.Add(new LicensedUseModel() { Id = indication.Id, Text = indication.Text });
                                }
                            }
                        }
                    }
                }

                var uniqueList = (from dataList in licensedUses
                                  select dataList).GroupBy(n => new { n.Id, n.Text })
                   .Select(g => g.FirstOrDefault())
                   .ToList();

                return Ok(uniqueList);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("GetUnLicensedUseByCodes")]
        public IHttpActionResult GetUnLicensedUseByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var response = new Dictionary<string, List<UnlicensedUseModel>>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                try
                {
                    var result = GetUnLicensedUseForCode(req.ProductType, req.ProductCode);

                    if (result != null && result.Count > 0)
                        response[req.ProductCode] = result;
                }
                catch (FDBApplicationException fdbAppException)
                {
                    //Log.Logger.Error(fdbAppException, fdbAppException.Message);
                    //Intentionally suppressing the exception
                }
                catch (Exception ex)
                {
                    //Log.Logger.Error(ex, ex.Message);
                    //Intentionally suppressing the exception
                }
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetUnLicensedUseByCode")]
        public IHttpActionResult GetUnLicensedUseByCode(string productType, string productCode)
        {
            try
            {
                List<UnlicensedUseModel> uniqueList = GetUnLicensedUseForCode(productType, productCode);

                return Ok(uniqueList);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private List<UnlicensedUseModel> GetUnLicensedUseForCode(string productType, string productCode)
        {
            List<UnlicensedUseModel> unLicensedUses = new List<UnlicensedUseModel>();

            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
            filter.DrugClass.Brand = true;
            filter.DrugClass.BrandedGeneric = true;

            if (string.Compare(productType, "VTM", true) == 0)
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Drug, productCode);

                Drug drug = drugSystem.Navigation.GetDrugById(drugBase);

                Product[] products = drug.GetRelatedProducts(filter);

                foreach (Product product in products)
                {
                    DrugBase drugBaseProduct = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, product.SingleId);

                    Product drugProduct = drugSystem.Navigation.GetProductById(drugBaseProduct);

                    Indication[] indications = product.GetIndications(true).ToList().Where(u => u.Unlicensed == true).ToArray();

                    if (indications != null && indications.Length > 0)
                    {
                        foreach (Indication indication in indications)
                        {
                            unLicensedUses.Add(new UnlicensedUseModel() { Id = indication.Id, Text = indication.Text });
                        }
                    }
                }
            }
            else
            {
                DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, productCode);

                Product product = drugSystem.Navigation.GetProductById(drugBase);

                Indication[] indications = product.GetIndications(true).ToList().Where(u => u.Unlicensed == true).ToArray();

                if (indications != null && indications.Length > 0)
                {
                    foreach (Indication indication in indications)
                    {
                        unLicensedUses.Add(new UnlicensedUseModel() { Id = indication.Id, Text = indication.Text });
                    }
                }
            }

            if (unLicensedUses == null) return null;

            var uniqueList = (from dataList in unLicensedUses
                              select dataList).GroupBy(n => new { n.Id, n.Text })
               .Select(g => g.FirstOrDefault())
               .ToList();
            return uniqueList;
        }

        [HttpGet]
        [Route("GetUnLicensedUseByName")]
        public IHttpActionResult GetUnLicensedUseByName(string productType, string productName)
        {
            List<UnlicensedUseModel> unLicensedUses = new List<UnlicensedUseModel>();

            try
            {
                DrugSystem drugSystem = InitializeDrugSystem();

                // Note - you could use a FilterUK here if required
                FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
                filter.DrugClass.Brand = true;
                filter.DrugClass.BrandedGeneric = true;

                if (string.Compare(productType, "VTM", true) == 0)
                {
                    Drug[] drugs = drugSystem.Navigation.GetDrugsByName(productName, filter);

                    if (drugs != null && drugs.Length > 0)
                    {
                        foreach (Drug drug in drugs)
                        {
                            Product[] products = drug.GetRelatedProducts(filter);

                            if (products != null && products.Length > 0)
                            {
                                foreach (Product product in products)
                                {
                                    Indication[] indications = product.GetIndications(true).ToList().Where(u => u.Unlicensed == true).ToArray();

                                    if (indications != null && indications.Length > 0)
                                    {
                                        foreach (Indication indication in indications)
                                        {
                                            unLicensedUses.Add(new UnlicensedUseModel() { Id = indication.Id, Text = indication.Text });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Product[] products = drugSystem.Navigation.GetProductsByName(productName, filter);

                    if (products != null && products.Length > 0)
                    {
                        foreach (Product product in products)
                        {
                            Indication[] indications = product.GetIndications(true).ToList().Where(u => u.Unlicensed == true).ToArray();

                            if (indications != null && indications.Length > 0)
                            {
                                foreach (Indication indication in indications)
                                {
                                    unLicensedUses.Add(new UnlicensedUseModel() { Id = indication.Id, Text = indication.Text });
                                }
                            }
                        }
                    }
                }

                var uniqueList = (from dataList in unLicensedUses
                                  select dataList).GroupBy(n => new { n.Id, n.Text })
                   .Select(g => g.FirstOrDefault())
                   .ToList();

                return Ok(uniqueList);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("GetAdverseEffectsFlagByCodes")]
        public IHttpActionResult GetAdverseEffectsFlagByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var response = new Dictionary<string, bool>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                try
                {
                    var result = GetAdverseEffectsFlagForCode(req.ProductCode);

                    response[req.ProductCode] = result;
                }
                catch (FDBApplicationException fdbAppException)
                {
                    //Log.Logger.Error(fdbAppException, fdbAppException.Message);
                    //Intentionally suppressing the exception
                }
                catch (Exception ex)
                {
                    //Log.Logger.Error(ex, ex.Message);
                    //Intentionally suppressing the exception
                }
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetAdverseEffectsFlagByCode")]
        public IHttpActionResult GetAdverseEffectsFlagByCode(string productCode)
        {
            //List<String> productCautions = new List<String>();
            //bool adverseEffectFlag = false;

            try
            {
                var adverseEffectFlag = GetAdverseEffectsFlagForCode(productCode);

                return Ok(adverseEffectFlag);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private bool GetAdverseEffectsFlagForCode(string productCode)
        {
            bool adverseEffectFlag;
            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
            filter.DrugClass.Brand = true;
            filter.DrugClass.BrandedGeneric = true;

            DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, productCode);

            ProductUK product = (ProductUK)drugSystem.Navigation.GetProductById(drugBase);

            adverseEffectFlag = product.ReportAdverseEffect;
            return adverseEffectFlag;
        }

        private DrugSystem InitializeDrugSystem()
        {
            try
            {
                DrugSystem drugSystem = DrugSystemFactory.CreateSystem();

                string disclaimer = drugSystem.GetDisclaimer();
                // UK-Specific
                {
                    DrugSystemUK drugSystemUK = (DrugSystemUK)drugSystem;
                    string copyright = drugSystemUK.GetCrownCopyright();
                    string bulletin = drugSystemUK.GetBulletin();
                }

                drugSystem.Environment.Language = Language.English;

                drugSystem.Environment.AttributePopulation.All();
                drugSystem.Environment.DrugTerminology = DrugTerminology.SNoMedCT;
                drugSystem.Environment.AttributePopulation.PrimaryPreferredName = true;
                drugSystem.Environment.AttributePopulation.DrugClass = true;
                drugSystem.Environment.AttributePopulation.DrugType = true;
                drugSystem.Environment.AttributePopulation.DiscontinuedDate = true;

                //UK - Specific
                {
                    AttributePopulationUK attrUK = (AttributePopulationUK)drugSystem.Environment.AttributePopulation;
                    attrUK.Dentist = true;
                }

                drugSystem.Environment.PrimaryPreferredNameFormat.AbbrevName = true;
                drugSystem.Environment.PrimaryPreferredNameFormat.IncludeCompanyName = true;
                drugSystem.Environment.PrimaryPreferredNameFormat.TallManName = true;
                drugSystem.Environment.SecondaryPreferredNameFormat.IncludeCompanyName = true;
                drugSystem.Environment.PrimaryPreferredNameFormat.TallManName = true;
                drugSystem.Environment.PicklistOrdering = PicklistOrdering.Option1;

                return drugSystem;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("GetFDBDetailByCodes")]
        public IHttpActionResult GetFDBDetailByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var adverseEffects = new Dictionary<string, bool>();
            var unLicensedUses = new Dictionary<string, List<UnlicensedUseModel>>();
            var licensedUses = new Dictionary<string, List<LicensedUseModel>>();
            var endorsements = new Dictionary<string, bool>();
            var safetyMessages = new Dictionary<string, List<string>>();
            var sideEffects = new Dictionary<string, List<string>>();
            var contraIndications = new Dictionary<string, List<ContraindicationsModel>>();
            var cautions = new Dictionary<string, List<string>>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                if (TryGetAdverseEffectsFlagForCode(req.ProductCode, out bool adverseEffectsResult))
                {
                    adverseEffects[req.ProductCode] = adverseEffectsResult;
                }

                if (TryGetUnLicensedUseForCode(req.ProductType, req.ProductCode, out List<UnlicensedUseModel> unLicensedUsesResult))
                {
                    unLicensedUses[req.ProductCode] = unLicensedUsesResult;
                }

                if (TryGetLicensedUseForCode(req.ProductType, req.ProductCode, out List<LicensedUseModel> licensedUsesResult))
                {
                    licensedUses[req.ProductCode] = licensedUsesResult;
                }

                if (TryGetEndorsementForCode(req.ProductType, req.ProductCode, out bool endorsementResult))
                {
                    endorsements[req.ProductCode] = endorsementResult;
                }

                if (TryGetSafetyMessagesForCode(req.ProductType, req.ProductCode, out List<string> safetyMessageResult))
                {
                    safetyMessages[req.ProductCode] = safetyMessageResult;
                }

                if (TryGetSideEffectsForCode(req.ProductType, req.ProductCode, out List<string> sideEffectsResult))
                {
                    sideEffects[req.ProductCode] = sideEffectsResult;
                }

                if (TryGetContraIndicationsForCode(req.ProductType, req.ProductCode, out List<ContraindicationsModel> contraIndicationsResult))
                {
                    contraIndications[req.ProductCode] = contraIndicationsResult;
                }

                if (TryGetCautionsForCode(req.ProductType, req.ProductCode, out List<string> cautionsResult))
                {
                    cautions[req.ProductCode] = cautionsResult;
                }

            }

            var response = new FDBAPIResponse
            {
                ContraIndications = contraIndications,
                SideEffects = sideEffects,
                SafetyMessages = safetyMessages,
                Endorsements = endorsements,
                LicensedUses = licensedUses,
                UnLicensedUses = unLicensedUses,
                AdverseEffects = adverseEffects,
                Cautions = cautions
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("GetTherapeuticClassificationGroupsByCodes")]
        public IHttpActionResult GetTherapeuticClassificationGroupsByCodes([FromBody] List<FDBDataRequest> request)
        {
            if (request == null || request.Count == 0) BadRequest();

            var response = new Dictionary<string, (string, string)>();

            foreach (var req in request)
            {
                if (string.IsNullOrEmpty(req.ProductCode) || string.IsNullOrEmpty(req.ProductType)) continue;

                try
                {
                    response[req.ProductCode] = GetTherapeuticClassificationGroupsForCode(req.ProductType, req.ProductCode);
                }
                catch (FDBApplicationException fdbAppException)
                {
                    //Log.Logger.Error(fdbAppException, fdbAppException.Message);
                    //Intentionally suppressing the exception
                }
                catch (Exception ex)
                {
                    //Log.Logger.Error(ex, ex.Message);
                    //Intentionally suppressing the exception
                }
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("GetTherapeuticClassificationGroupsByCode")]
        public IHttpActionResult GetTherapeuticClassificationGroupsByCode(string productType, string productCode)
        {
            try
            {
                var classGroup = GetTherapeuticClassificationGroupsForCode(productType, productCode);

                return Ok(classGroup);
            }
            catch (FDBApplicationException fdbAppException)
            {
                return StatusCode(System.Net.HttpStatusCode.NoContent);
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
        }

        private (string, string) GetTherapeuticClassificationGroupsForCode(string productType, string productCode)
        {
            if (string.Compare(productType, "VTM", true) == 0) return (null, null);

            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
            filter.DrugClass.Brand = true;
            filter.DrugClass.BrandedGeneric = true;

            DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, productCode);

            Product product = (Product)drugSystem.Navigation.GetProductById(drugBase);

            var classMembers = product.GetTherapeuticClassificationsMembership();

            if (classMembers == null || classMembers.Length == 0) return (null, null);

            var classMembersWithCodeNames = classMembers.Select(rec => $"{rec.Text} - [{rec.Reference}]");
            var classMembersWithCodeRefs = classMembers.Select(rec => rec.Reference);

            var classDataWithName = string.Join(" | ", classMembersWithCodeNames);
            var classDataWithRefs = classMembersWithCodeRefs.Last();// string.Join(" | ", classMembersWithCodeRefs);

            return (classDataWithRefs, classDataWithName);
        }


        private bool TryGetAdverseEffectsFlagForCode(string productCode, out bool result)
        {
            result = false;
            try
            {
                result = GetAdverseEffectsFlagForCode(productCode);
                return true;
            }
            catch (Exception ex)
            {
                //Log.Logger.Error(ex, ex.Message);
                //Intentionally suppressing the exception
            }
            return false;
        }

        private bool TryGetUnLicensedUseForCode(string productType, string productCode, out List<UnlicensedUseModel> result)
        {
            result = null;
            try
            {
                result = GetUnLicensedUseForCode(productType, productCode);
                return true;
            }
            catch (Exception ex)
            {
                //Log.Logger.Error(ex, ex.Message);
                //Intentionally suppressing the exception
            }
            return false;
        }

        private bool TryGetLicensedUseForCode(string productType, string productCode, out List<LicensedUseModel> result)
        {
            result = null;
            try
            {
                result = GetLicensedUseForCode(productType, productCode);
                return true;
            }
            catch (Exception ex)
            {
                //Log.Logger.Error(ex, ex.Message);
                //Intentionally suppressing the exception
            }
            return false;
        }

        private bool TryGetEndorsementForCode(string productType, string productCode, out bool result)
        {
            result = false;
            try
            {
                result = GetEndorsementForCode(productType, productCode);
                return true;
            }
            catch (Exception ex)
            {
                //Log.Logger.Error(ex, ex.Message);
                //Intentionally suppressing the exception
            }
            return false;
        }

        private bool TryGetSafetyMessagesForCode(string productType, string productCode, out List<string> result)
        {
            result = null;
            try
            {
                result = GetSafetyMessagesForCode(productType, productCode);
                return true;
            }
            catch (Exception ex)
            {
                //Log.Logger.Error(ex, ex.Message);
                //Intentionally suppressing the exception
            }
            return false;
        }

        private bool TryGetSideEffectsForCode(string productType, string productCode, out List<string> result)
        {
            result = null;
            try
            {
                result = GetSideEffectsForCode(productType, productCode);
                return true;
            }
            catch (Exception ex)
            {
                //Log.Logger.Error(ex, ex.Message);
                //Intentionally suppressing the exception
            }
            return false;
        }

        private bool TryGetContraIndicationsForCode(string productType, string productCode, out List<ContraindicationsModel> result)
        {
            result = null;
            try
            {
                result = GetContraIndicationsForCode(productType, productCode);
                return true;
            }
            catch (Exception ex)
            {
                //Log.Logger.Error(ex, ex.Message);
                //Intentionally suppressing the exception
            }
            return false;
        }

        private bool TryGetCautionsForCode(string productType, string productCode, out List<string> result)
        {
            result = null;
            try
            {
                result = GetCautionsForCode(productType, productCode);
                return true;
            }
            catch (Exception ex)
            {
                //Log.Logger.Error(ex, ex.Message);
                //Intentionally suppressing the exception
            }
            return false;
        }

        private PatientInformation CreatePatientInformation(PatientInfo pinfo)
        {
            PatientInformation p;
            if (pinfo == null || pinfo.gender == -1)
                return null;

            if (pinfo.gender == 1 || pinfo.gender == 2 || pinfo.gender == 3)
                p = new PatientInformation((Gender)pinfo.gender);
            else
                throw new Exception("Invalid Gender");

            if (pinfo.age > 0)
            {
                p.Age = pinfo.age;
            }

            if (pinfo.weight > 0)
            {
                p.Weight = pinfo.weight;
            }

            if (pinfo.bsa > 0)
            {
                p.BodySurfaceArea = pinfo.bsa;
            }

            if (pinfo.allergens != null)
            {
                foreach (var item in pinfo.allergens)
                {
                    p.Allergens.Add(new Allergen(item.uname, AllergenTerminology.SNoMedCT, (AllergenConceptType)item.type, item.code));
                }
            }

            if (pinfo.conditions != null)
            {
                foreach (var item in pinfo.conditions)
                {
                    p.Conditions.Add(new Condition(item.uname, ConditionTerminology.SNoMedCT, item.code));
                }
            }
            p.ConditionListComplete = false;
            return p;

        }

        private List<ContraindicationsPrecautionsModel> GetContraindicationsAndPrecautionsForProducts(object[] products, PatientInfo pinfo = null)
        {
            List<ContraindicationsPrecautionsModel> contraIndicationsPrecautions = new List<ContraindicationsPrecautionsModel>();

            PatientInformation p = null;
            try
            {
                p = CreatePatientInformation(pinfo);
            }
            catch (Exception e)
            {
                throw e;
            }

            foreach (var product in products)
            {
                ContraPrec[] contraPrecs;

                if (p != null)
                {
                    if (product is OrderableMed)
                        contraPrecs = ((OrderableMed)product).GetContraindicationsAndPrecautions(p);
                    else
                        if (product is Product)
                        contraPrecs = ((Product)product).GetContraindicationsAndPrecautions(p);
                    else
                        contraPrecs = null;
                }
                else
                {
                    if (product is OrderableMed)
                        contraPrecs = ((OrderableMed)product).GetContraindicationsAndPrecautions();
                    else
                                           if (product is Product)
                        contraPrecs = ((Product)product).GetContraindicationsAndPrecautions();
                    else
                        contraPrecs = null;
                }
                if (contraPrecs != null && contraPrecs.Length > 0)
                {
                    foreach (ContraPrec contraPrec in contraPrecs)
                    {
                        contraIndicationsPrecautions.Add(new ContraindicationsPrecautionsModel() { Id = contraPrec.Id.ToString(), type = contraPrec.ConditionAlertSeverity, Text = contraPrec.Text });
                    }
                }
            }

            if (contraIndicationsPrecautions == null) return null;

            var uniqueList = (from dataList in contraIndicationsPrecautions
                              select dataList).GroupBy(n => new { n.Id, n.type, n.Text })
                                       .Select(g => g.FirstOrDefault())
                                       .ToList();

            System.Diagnostics.Debug.WriteLine(uniqueList.Count);
            return uniqueList;
        }

        private List<Warning> GetWarningsForProducts(object[] products, PatientInfo pinfo = null)
        {
            List<Warning> productWarnings = new List<Warning>();

            PatientInformation p = null;

            try
            {
                p = CreatePatientInformation(pinfo);
            }
            catch (Exception e)
            {
                throw e;
            }
            DrugSystem drugSystem = InitializeDrugSystem();
            foreach (object product in products)
            {
                Warning[] warnings;

                if (p != null)
                {
                    if (product is OrderableMed)
                        warnings = ((OrderableMed)product).GetWarnings(p);
                    else
                        if (product is Product)
                        warnings = ((Product)product).GetWarnings(p);
                    else
                        warnings = null;
                }
                else
                {
                    if (product is OrderableMed)
                        warnings = ((OrderableMed)product).GetWarnings();
                    else
                                           if (product is Product)
                        warnings = ((Product)product).GetWarnings();
                    else
                        warnings = null;
                }

                if (warnings != null && warnings.Length > 0)
                {
                    productWarnings.AddRange(warnings);
                }




            }

            if (productWarnings == null) return null;

            var uniqueList = (from dataList in productWarnings
                              select dataList).GroupBy(n => new { n.Text })
                                       .Select(g => g.FirstOrDefault())
                                       .ToList();
            System.Diagnostics.Debug.WriteLine(uniqueList.Count);

            return uniqueList;

        }
        private List<MedicineSafetyMessage> GetSafetyMessagesForProducts(object[] products)
        {
            List<MedicineSafetyMessage> productSafetyMessages = new List<MedicineSafetyMessage>();
            DrugSystem drugSystem = InitializeDrugSystem();

            foreach (object product in products)
            {
                MedicineSafetyMessage[] safetyMessages;

                if (product is OrderableMed)
                {
                    safetyMessages = ((OrderableMedUK)product).GetMedicineSafetyMessages();
                }
                else
                if (product is Product)
                {
                    DrugBase drugBaseProduct = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, ((Product)product).SingleId);

                    ProductUK productUK = (ProductUK)drugSystem.Navigation.GetProductById(drugBaseProduct);

                    safetyMessages = productUK.GetMedicineSafetyMessages();
                }
                else
                    safetyMessages = null;

                if (safetyMessages != null && safetyMessages.Length > 0)
                {
                    productSafetyMessages.AddRange(safetyMessages);
                }


            }
            System.Diagnostics.Debug.WriteLine(productSafetyMessages.Count);


            var uniqueList = (from dataList in productSafetyMessages
                              select dataList).GroupBy(n => new { n.SafetyMessage })
                            .Select(g => g.FirstOrDefault())
                            .ToList();

            return uniqueList;
        }

        private List<MandatoryInstruction> GetMandatoryInstructionsForProducts(object[] products)
        {
            List<MandatoryInstruction> productMandatoryInstructions = new List<MandatoryInstruction>();
            foreach (Product product in products)
            {
                MandatoryInstruction[] mi = product.GetMandatoryInstructions();

                if (mi != null && mi.Length > 0)
                {
                    productMandatoryInstructions.AddRange(mi);
                }
            }
            System.Diagnostics.Debug.WriteLine(productMandatoryInstructions.Count);

            var uniqueList = (from dataList in productMandatoryInstructions
                              select dataList).GroupBy(n => new { n.Text, n.Type })
                            .Select(g => g.FirstOrDefault())
                            .ToList();
            return uniqueList;
        }
        private List<SideEffect> GetSideEffectsForProducts(object[] products)
        {
            List<SideEffect> productSideEffects = new List<SideEffect>();

            foreach (object product in products)
            {
                SideEffect[] sideEffects;

                if (product is OrderableMed)
                {
                    try
                    {
                        sideEffects = ((OrderableMed)product).GetSideEffects();
                    }
                    catch
                    {
                        sideEffects = null;
                    }
                }
                else if (product is Product)
                {
                    sideEffects = ((Product)product).GetSideEffects();
                }
                else
                    sideEffects = null;

                if (sideEffects != null && sideEffects.Length > 0)
                {
                    productSideEffects.AddRange(sideEffects);
                }
            }
            var uniqueList = (from dataList in productSideEffects
                              select dataList).GroupBy(n => new { n.Text })
                             .Select(g => g.FirstOrDefault())
                             .ToList();
            System.Diagnostics.Debug.WriteLine(uniqueList.Count);

            return uniqueList;

        }


        private void CreateDrugSystemAndProduct(out DrugBase db, out object[] ps, string name, string productcode, string producttype, Route[] routes, ref List<GenericError> unknownproducts, bool forceProductsForVtm = false)
        {
            //create drug base 
            DrugSystem drugSystem = InitializeDrugSystem();

            // Note - you could use a FilterUK here if required
            Filter filter = new Filter();
            filter.DrugClass.BrandedGeneric = true;
            filter.MarketStatus.Unknown = true;
            filter.Unlicensed = true;
            //filter.ParallelImport = true;

            DrugBase drugBase;
            List<Product> products = new List<Product>();
            ps = null;
            try
            {
                if (string.Compare(producttype, "VTM", true) == 0)
                {
                    drugBase = drugSystem.ObjectFactory.Create_DrugBase(name, DrugTerminology.SNoMedCT, DrugConceptType.Drug, productcode);

                    Drug drug = drugSystem.Navigation.GetDrugById(drugBase);
                    if (forceProductsForVtm)
                    {
                        products = drug.GetRelatedProducts(filter).ToList();
                        if (routes == null || routes.Length == 0)
                            ps = products.ToArray();
                        else
                            ps = products.FindAll(x => x.GetRoutes(true).Where(y => routes.ToList().FindAll(r => r.code == y.Id).Count != 0).ToList().Count != 0).ToArray();
                    }
                    else
                    {
                        OrderableMed[] omeds;

                        //changing the below way of getting orderable meds to FDB provided code using vtmid and routeid parts
                        //var omeds = drug.GetOrderableMeds(omf);

                        //foreach (var o in omeds)
                        //{
                        //    o.UserSpecifiedName = name;
                        //}
                        //if (routes == null || routes.Length == 0)
                        //    ps = omeds;
                        //else
                        //    ps = omeds.ToList().FindAll(x => x.MultipleIdList.Where(y => routes.ToList().FindAll(r => r.code == y.Id).Count != 0).ToList().Count != 0).ToArray();


                        if (routes == null || routes.Length == 0)
                        {
                            OrderableMedFilterUK omf = new OrderableMedFilterUK();
                            omf.DrugClass.BrandedGeneric = true;
                            omf.MarketStatus.Unknown = true;
                            omf.Unlicensed = true;
                            omf.RouteStatus.All();
                            omf.IncludeDiscretionaryRouted = true;
                            omf.OrderableMedCategory = OrderableMedCategory.DrugRoute;
                            //omf.FDBRecognisedOnly = false;
                            omeds = drug.GetOrderableMeds(omf);
                        }
                        else
                        {
                            var tempomeds = new List<OrderableMed>();
                            DrugPartId vtmId = new DrugPartId(DrugPartConceptType.Drug, productcode);
                            foreach (var r in routes)
                            {
                                DrugPartId routeID = new DrugPartId(DrugPartConceptType.Route, r.code);
                                DrugPartId[] oMedComponents = new DrugPartId[] { vtmId, routeID };
                                OrderableMed omed = drugSystem.Navigation.GetOrderableMedById(
                                    drugSystem.ObjectFactory.Create_DrugBase(name, DrugTerminology.SNoMedCT, DrugConceptType.OrderableMed, oMedComponents));
                                if (omed.SingleId.StartsWith("-"))
                                {
                                    unknownproducts.Add(new GenericError("Side effects, Precautions and Contraindication decision support is not available for the " + r.name + " route. Please exercise caution", name, productcode, "unknownroute", r.name));

                                }
                                //else
                                //{
                                //}
                                tempomeds.Add(omed);

                            }
                            omeds = tempomeds.ToArray();
                        }
                        foreach (var o in omeds)
                        {
                            o.UserSpecifiedName = name;
                        }
                        ps = omeds;
                    }

                    if (ps.Length == 0)
                    {
                        unknownproducts.Add(new GenericError("No decision support or warnings are available for this medicine. Please exercise caution", name, productcode, "unknownproduct"));
                    }
                    db = drugBase;
                }
                else
                {
                    drugBase = drugSystem.ObjectFactory.Create_DrugBase(name, DrugTerminology.SNoMedCT, DrugConceptType.Product, productcode);
                    Product product = drugSystem.Navigation.GetProductById(drugBase);
                    products.Add(product);
                    ps = products.ToArray();
                    db = drugBase;
                }
            }
            catch
            {
                unknownproducts.Add(new GenericError("No decision support or warnings are available for this medicine. Please exercise caution", name, productcode, "unknownproduct"));
                ps = null;
                db = null;
            }
        }


        [HttpPost]
        [Route("GetFDBWarnings")]
        public IHttpActionResult GetFDBWarnings([FromBody] FDBDataRequestPatientSpecific request)
        {
            if (request == null) return BadRequest();
            if (request.products.Count == 0 && request.currentproducts.Count == 0)
                return Ok(new List<EPMAWarnings>());

            //temporary fix start-- to be removed after filtering custom meds from epma
            //request.products.RemoveAll(item => item.ProductCode.ToLower() == "custom");
            //request.currentproducts.RemoveAll(item => item.ProductCode.ToLower() == "custom");

            if (request.products.Count == 0 && request.currentproducts.Count == 0)
                return Ok(new List<EPMAWarnings>());


            //temporary fix end

            var contraindicationsandprecautions = new Dictionary<string, List<ContraindicationsPrecautionsModel>>();
            var contraindicationsandprecautionsspecific = new Dictionary<string, List<ContraindicationsPrecautionsModel>>();
            var warnings = new Dictionary<string, List<Warning>>();
            var warningsspecific = new Dictionary<string, List<Warning>>();
            var safteymessages = new Dictionary<string, List<MedicineSafetyMessage>>();
            var mandatoryinstructions = new Dictionary<string, List<MandatoryInstruction>>();
            var sideeffects = new Dictionary<string, List<SideEffect>>();

            List<DrugBase> currentDrugs = new List<DrugBase>();
            List<DrugBase> prospectiveDrugs = new List<DrugBase>();

            PatientInformation p = null;
            try
            {
                p = CreatePatientInformation(request.patientinfo);
            }
            catch (Exception e)
            {
                throw e;
            }

            //p.Conditions = new Condition[] { new Condition("Epilepsy", ConditionTerminology.SNoMedCT, "84757009") };
            //p.ConditionListComplete = false;
            //p.Weight = 60;

            System.Diagnostics.Debug.WriteLine("initialize drug system " + DateTime.Now);

            DrugSystem system = InitializeDrugSystem();

            var g_orderablemeds = new Dictionary<string, List<object>>();
            var g_products = new Dictionary<string, List<object>>();
            var _unknownproducts = new List<GenericError>();
            foreach (var rp in request.products)
            {
                if (string.IsNullOrEmpty(rp.ProductCode) || string.IsNullOrEmpty(rp.ProductType)) return BadRequest("Product code or product type not supplied");

                DrugBase drugBase;
                object[] orderablemeds;
                object[] products;

                if (rp.ProductType.ToLower() == "vtm")
                {
                    System.Diagnostics.Debug.WriteLine("get orderable meds for vtm " + DateTime.Now);

                    // get drugbase and products
                    CreateDrugSystemAndProduct(out drugBase, out orderablemeds, rp.NameIdentifier, rp.ProductCode, rp.ProductType, rp.Routes, ref _unknownproducts);

                    if (drugBase != null && orderablemeds != null)
                    {
                        if (!g_orderablemeds.ContainsKey(rp.ProductCode))
                            g_orderablemeds.Add(rp.ProductCode, new List<object> { });

                        g_orderablemeds[rp.ProductCode].AddRange(orderablemeds.Where(x => g_orderablemeds[rp.ProductCode].Find(ox => ((OrderableMed)ox).SingleId == ((OrderableMed)x).SingleId) == null).ToList());

                        System.Diagnostics.Debug.WriteLine("add vtm omeds for screening " + DateTime.Now);
                        foreach (OrderableMed item in orderablemeds)
                        {
                            prospectiveDrugs.Add(item);
                        }
                    }
                    //System.Diagnostics.Debug.WriteLine("get products for vtm " + DateTime.Now);

                    //// get drugbase and products
                    //CreateDrugSystemAndProduct(out drugBase, out products, rp.NameIdentifier, rp.ProductCode, rp.ProductType, rp.Routes, ref _unknownproducts, true);
                    //if (drugBase != null && products != null)
                    //{
                    //    if (!g_products.ContainsKey(rp.ProductCode))
                    //        g_products.Add(rp.ProductCode, new List<object> { });

                    //    g_products[rp.ProductCode].AddRange(products.Where(x => g_products[rp.ProductCode].Find(ox => ((Product)ox).SingleId == ((Product)x).SingleId) == null).ToList());
                    //}


                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("get products for product " + DateTime.Now);

                    // get drugbase and products
                    CreateDrugSystemAndProduct(out drugBase, out products, rp.NameIdentifier, rp.ProductCode, rp.ProductType, rp.Routes, ref _unknownproducts, true);
                    if (drugBase != null && products != null)
                    {
                        if (!g_products.ContainsKey(rp.ProductCode))
                            g_products.Add(rp.ProductCode, new List<object> { });

                        g_products[rp.ProductCode].AddRange(products.Where(x => g_products[rp.ProductCode].Find(ox => ((Product)ox).SingleId == ((Product)x).SingleId) == null).ToList());

                        System.Diagnostics.Debug.WriteLine("add product for screening " + DateTime.Now);

                        prospectiveDrugs.Add(drugBase);
                    }
                }
            }
            if (request.currentproducts != null)
            {
                foreach (var rp in request.currentproducts)
                {
                    DrugBase drugBase;
                    object[] orderablemeds;
                    object[] products;
                    if (request.products == null || (request.products != null && request.products.Count == 0))
                    {
                        if (rp.ProductType.ToLower() == "vtm")
                        {
                            System.Diagnostics.Debug.WriteLine("get orderable for vtm meds-currentmeds " + DateTime.Now);

                            // get drugbase and products
                            CreateDrugSystemAndProduct(out drugBase, out orderablemeds, rp.NameIdentifier, rp.ProductCode, rp.ProductType, rp.Routes, ref _unknownproducts);
                            if (drugBase != null && orderablemeds != null)
                            {
                                if (!g_orderablemeds.ContainsKey(rp.ProductCode))
                                    g_orderablemeds.Add(rp.ProductCode, new List<object> { });

                                g_orderablemeds[rp.ProductCode].AddRange(orderablemeds.Where(x => g_orderablemeds[rp.ProductCode].Find(ox => ((OrderableMed)ox).SingleId == ((OrderableMed)x).SingleId) == null).ToList());
                            }

                            System.Diagnostics.Debug.WriteLine("add vtm omeds for screening - currentmeds" + DateTime.Now);
                            foreach (OrderableMed item in orderablemeds)
                            {
                                currentDrugs.Add(item);
                            }
                            //System.Diagnostics.Debug.WriteLine("get products for vtm currentmeds " + DateTime.Now);

                            //// get drugbase and products
                            //CreateDrugSystemAndProduct(out drugBase, out products, rp.NameIdentifier, rp.ProductCode, rp.ProductType, rp.Routes, ref _unknownproducts, true);
                            //if (drugBase != null && orderablemeds != null)
                            //{
                            //    if (!g_products.ContainsKey(rp.ProductCode))
                            //        g_products.Add(rp.ProductCode, new List<object> { });

                            //    g_products[rp.ProductCode].AddRange(products.Where(x => g_products[rp.ProductCode].Find(ox => ((Product)ox).SingleId == ((Product)x).SingleId) == null).ToList());
                            //}
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("get product currentmeds  " + DateTime.Now);

                            // get drugbase and products
                            CreateDrugSystemAndProduct(out drugBase, out products, rp.NameIdentifier, rp.ProductCode, rp.ProductType, rp.Routes, ref _unknownproducts, true);

                            if (drugBase != null && products != null)
                            {
                                if (!g_products.ContainsKey(rp.ProductCode))
                                    g_products.Add(rp.ProductCode, new List<object> { });
                                g_products[rp.ProductCode].AddRange(products.Where(x => g_products[rp.ProductCode].Find(ox => ((Product)ox).SingleId == ((Product)x).SingleId) == null).ToList());

                                currentDrugs.Add(drugBase);
                            }
                        }
                    }
                    else
                    {
                        if (string.Compare(rp.ProductType, "vtm", true) == 0)
                        {
                            CreateDrugSystemAndProduct(out drugBase, out orderablemeds, rp.NameIdentifier, rp.ProductCode, rp.ProductType, rp.Routes, ref _unknownproducts);

                            //DrugBase pdb = system.ObjectFactory.Create_DrugBase(rp.NameIdentifier, DrugTerminology.SNoMedCT, DrugConceptType.Drug, rp.ProductCode);
                            if (drugBase != null)
                                foreach (OrderableMed item in orderablemeds)
                                {
                                    currentDrugs.Add(item);
                                }
                        }
                        else
                        {
                            CreateDrugSystemAndProduct(out drugBase, out orderablemeds, rp.NameIdentifier, rp.ProductCode, rp.ProductType, rp.Routes, ref _unknownproducts, true);

                            //DrugBase pdb = system.ObjectFactory.Create_DrugBase(rp.NameIdentifier, DrugTerminology.SNoMedCT, DrugConceptType.Product, rp.ProductCode);
                            if (drugBase != null)
                                currentDrugs.Add(drugBase);
                        }
                    }
                }
            }

            foreach (var item in g_orderablemeds)
            {
                // get contraindications and precautions
                if (!(request.warningtypes != null && request.warningtypes.Count > 0 &&
                        !request.warningtypes.Contains(WarningType.ContraIndication) && !request.warningtypes.Contains(WarningType.Precaution)))
                {
                    //System.Diagnostics.Debug.WriteLine("get contraindications  " + DateTime.Now);

                    //// get contraindications and precautions
                    //contraindicationsandprecautions.Add(item.Key, GetContraindicationsAndPrecautionsForProducts(item.Value.ToArray()));

                    // System.Diagnostics.Debug.WriteLine("get contraindications specific  " + DateTime.Now);

                    //get specific contraindications and precautions
                    // not requried for vtms, hence commenting out //
                    // contraindicationsandprecautionsspecific.Add(item.Key, GetContraindicationsAndPrecautionsForProducts(item.Value.ToArray(), request.patientinfo));

                }

                //get safety messages
                if (!(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.SafetyMessage)))
                {
                    System.Diagnostics.Debug.WriteLine("get safety messages  " + DateTime.Now);

                    safteymessages.Add(item.Key, GetSafetyMessagesForProducts(item.Value.ToArray()));
                }

                //get side effects
                if (!(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.SideEffect)))
                {
                    System.Diagnostics.Debug.WriteLine("get sideeffects  " + DateTime.Now);

                    sideeffects.Add(item.Key, GetSideEffectsForProducts(item.Value.ToArray()));
                }
            }

            foreach (var item in g_products)
            {
                // get contraindications and precautions
                //for products patientcheck will work at the bottom 
                //if (!(request.warningtypes != null && request.warningtypes.Count > 0 &&
                //        !request.warningtypes.Contains(WarningType.ContraIndication) && !request.warningtypes.Contains(WarningType.Precaution))
                //        && !contraindicationsandprecautions.ContainsKey(item.Key))
                //{
                //    System.Diagnostics.Debug.WriteLine("get contraindications  " + DateTime.Now);

                //    // get contraindications and precautions
                //    contraindicationsandprecautions.Add(item.Key, GetContraindicationsAndPrecautionsForProducts(item.Value.ToArray()));

                //    System.Diagnostics.Debug.WriteLine("get contraindications specific  " + DateTime.Now);

                //    //get specific contraindications and precautions
                //    contraindicationsandprecautionsspecific.Add(item.Key, GetContraindicationsAndPrecautionsForProducts(item.Value.ToArray(), request.patientinfo));

                //}

                //get safety messages
                if (!(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.SafetyMessage))
                                            && !safteymessages.ContainsKey(item.Key))

                {
                    System.Diagnostics.Debug.WriteLine("get safety messages  " + DateTime.Now);

                    safteymessages.Add(item.Key, GetSafetyMessagesForProducts(item.Value.ToArray()));
                }

                //get side effects
                if (!(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.SideEffect))
                                                                && !sideeffects.ContainsKey(item.Key))

                {
                    System.Diagnostics.Debug.WriteLine("get sideeffects  " + DateTime.Now);

                    sideeffects.Add(item.Key, GetSideEffectsForProducts(item.Value.ToArray()));
                }

                //get drug warnings
                if (!(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.DrugWarning)))
                {
                    System.Diagnostics.Debug.WriteLine("get warnings  " + DateTime.Now);

                    //get drug warnings
                    warnings.Add(item.Key, GetWarningsForProducts(item.Value.ToArray()));

                    System.Diagnostics.Debug.WriteLine("get warnigns specific  " + DateTime.Now);

                    //get specific warnings
                    warningsspecific.Add(item.Key, GetWarningsForProducts(item.Value.ToArray(), request.patientinfo));
                }

                if (!(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.MandatoryInstruction)))
                {
                    System.Diagnostics.Debug.WriteLine("get mandatory instr  " + DateTime.Now);

                    //get mandatory instructions
                    mandatoryinstructions.Add(item.Key, GetMandatoryInstructionsForProducts(item.Value.ToArray()));

                }
            }


            System.Diagnostics.Debug.WriteLine("create screening " + DateTime.Now);

            Screening screen = system.Screening;

            // Set which modules to screen
            screen.ModulesToScreen.Clear();
            screen.ModulesToScreen.All();
            screen.ModulesToScreen.PharmacologicalEquivalence = false;
            screen.ModulesToScreen.DrugEquivalence = true;
            screen.ModulesToScreen.DuplicateTherapy = true;
            screen.ModulesToScreen.DrugDoubling = true;
            screen.ModulesToScreen.PatientCheck = true;
            screen.ModulesToScreen.DoseRangeCheck = false;

            //Condition[] conditions = new Condition[] { new Condition("UTI", ConditionTerminology.SNoMedCT, "68566005") };
            // Allergen[] allergens = new Allergen[] { new Allergen("warfarinaallergy", AllergenTerminology.SNoMedCT, AllergenConceptType.Substance, "256259004") };

            // PatientInformation pInfo = new PatientInformation(Gender.Female, 11680, 45, 1.56f, conditions, allergens, drugsToCompare.ToArray<DrugBase>(), prospDrugs);
            p.CurrentDrugs = currentDrugs.ToArray<DrugBase>();
            p.ProspectiveDrugs = prospectiveDrugs.ToArray<DrugBase>();

            if (request.products.Count == 0)
            {
                screen.CheckAllDrugs = true; // check all in currentdrugs if prospective is empty
            }
            ScreeningError[] errors;

            System.Diagnostics.Debug.WriteLine("get screening " + DateTime.Now);

            ScreeningResultSet resultSet = null;

            if ((prospectiveDrugs.Count > 0) || (request.products.Count == 0 && currentDrugs.Count > 0))
                resultSet = screen.Screen(p);

            if (resultSet != null && resultSet.HasErrors)
            {
                errors = resultSet.GetErrors();
            }
            else
                errors = new ScreeningError[] { };
            System.Diagnostics.Debug.WriteLine(" screening complete " + DateTime.Now);

            DrugDoubling[] drugDoubling = new DrugDoubling[] { };
            DrugEquivalence[] drugEquivalence = new DrugEquivalence[] { };
            DrugInteraction[] drugInteractions = new DrugInteraction[] { };
            DrugSensitivity[] drugSensitivities = new DrugSensitivity[] { };
            DuplicateTherapy[] duplicateTherapies = new DuplicateTherapy[] { };
            PatientCheck[] pc = new PatientCheck[] { };


            if (resultSet != null && !(request.warningtypes != null && request.warningtypes.Count > 0 &&
                    !request.warningtypes.Contains(WarningType.ContraIndication) && !request.warningtypes.Contains(WarningType.Precaution)))
            {
                pc = resultSet.GetPatientCheckResults();
                var uniqueList = (from dataList in pc
                                  select dataList).GroupBy(n => new { n.Drug.UserSpecifiedName, n.AlertRelevanceType, n.AlertText, n.ConditionAlertSeverity, n.FullAlertMessage })
                                       .Select(g => g.FirstOrDefault())
                                       .ToList();

            }
            if (resultSet != null && !(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.DrugDoubling)))
            {
                System.Diagnostics.Debug.WriteLine("get drug doubling " + DateTime.Now);

                //get drug doubling
                //filter results where primary drug and secondary drug same - this is a result of using orderable meds for VTMs. A VTM might result in more than one orderable med
                //based on the number of descretionary routes selected in the prescription. More than on orderable med concept for a VTM results in duplicate therapy warnings and drug doubling 
                //warnigns on the same prescription.
                drugDoubling = resultSet.GetDrugDoublingResults().Where(x => x.PrimaryDrug.UserSpecifiedName != x.SecondaryDrug.UserSpecifiedName).ToArray();
            }

            if (resultSet != null && !(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.DrugEquivalance)))
            {
                System.Diagnostics.Debug.WriteLine("get drug equivalence " + DateTime.Now);
                //get drug equivalance
                drugEquivalence = resultSet.GetDrugEquivalenceResults();
            }

            if (resultSet != null && !(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.DrugInteraction)))
            {
                System.Diagnostics.Debug.WriteLine("get drug interactions " + DateTime.Now);

                //get drug-drug interactions
                drugInteractions = resultSet.GetDrugInteractionResults();

            }

            if (resultSet != null && !(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.Sensitivity)))
            {
                System.Diagnostics.Debug.WriteLine("get sensitivities " + DateTime.Now);

                //get sensitivities
                drugSensitivities = resultSet.GetDrugSensitivityResults();
            }

            if (resultSet != null && !(request.warningtypes != null && request.warningtypes.Count > 0 && !request.warningtypes.Contains(WarningType.DuplicateTherapy)))
            {
                System.Diagnostics.Debug.WriteLine("get duplicates therapies " + DateTime.Now);

                //get duplicate therapy
                //filter results where primary drug and secondary drug same - this is a result of using orderable meds for VTMs. A VTM might result in more than one orderable med
                //based on the number of descretionary routes selected in the prescription. More than on orderable med concept for a VTM results in duplicate therapy warnings and drug doubling 
                //warnigns on the same prescription.
                duplicateTherapies = resultSet.GetDuplicateTherapyResults().Where(x => x.PrimaryDrug.UserSpecifiedName != x.SecondaryDrug.UserSpecifiedName).ToArray();

            }
            System.Diagnostics.Debug.WriteLine("prepare responses " + DateTime.Now);

            //prepare response
            List<EPMAWarnings> warningsReponse = new List<EPMAWarnings>();
            //add contraindications

            //add specific contraindications

            //foreach (var cd in contraindicationsandprecautionsspecific)
            //{
            //    foreach (var cp in cd.Value)
            //    {
            //        var warning = new EPMAWarnings
            //        {
            //            encounter_id = "",
            //            person_id = "",
            //            fdbmessageid = cp.Id,
            //            allergencode = null,
            //            allergeningredient = null,
            //            allergenmatchtype = null,
            //            drugingredient = null,
            //            epma_warnings_id = Guid.NewGuid().ToString(),
            //            fdbseverity = null,
            //            message = cp.Text,
            //            overridemessage = null,
            //            overriderequired = null,
            //            primarymedicationcode = cd.Key, // request.products.Where(x => x.NameIdentifier == cd.Key).ToArray()[0].ProductCode,
            //            primaryprescriptionid = cd.Key,
            //            secondarymedicationcode = null,
            //            secondaryprescriptionid = null,
            //            severity = null,
            //            warningcategories = null,
            //            warningtype = cp.type.ToString().ToLower(),
            //            ispatientspecific = true,
            //            msgtype = null
            //        };
            //        warningsReponse.Add(warning);
            //    }
            //}

            foreach (var pck in pc)
            {
                var ppname = request.products.FirstOrDefault(x => x.NameIdentifier == pck.Drug.UserSpecifiedName);
                if (ppname == null)
                    ppname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == pck.Drug.UserSpecifiedName);

                var warning = new EPMAWarnings
                {
                    encounter_id = "",
                    person_id = "",
                    fdbmessageid = "",
                    allergencode = null,
                    allergeningredient = null,
                    allergenmatchtype = null,
                    drugingredient = null,
                    epma_warnings_id = Guid.NewGuid().ToString(),
                    fdbseverity = pck.AlertRelevanceType.ToString(),
                    message = pck.AlertText + " - " + pck.FullAlertMessage,
                    overridemessage = null,
                    overriderequired = null,
                    primarymedicationcode = ppname.ProductCode,//pck.Drug.SingleId,
                    primaryprescriptionid = pck.Drug.UserSpecifiedName,
                    secondarymedicationcode = null,
                    secondaryprescriptionid = null,
                    severity = null,
                    warningcategories = null,
                    warningtype = pck.ConditionAlertSeverity.ToString().ToLower(),
                    ispatientspecific = true,
                    msgtype = pck.AlertText
                };
                warning.message = warning.message.Replace(warning.primaryprescriptionid, "<b>" + ppname.TherapyName + "</b>");

                //warning.message = warning.message.Replace(warning.primaryprescriptionid, ppname.TherapyName);

                warningsReponse.Add(warning);
            }

            foreach (var cd in contraindicationsandprecautions)
            {
                foreach (var cp in cd.Value)
                {
                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = cp.Id,
                        allergencode = null,
                        allergeningredient = null,
                        allergenmatchtype = null,
                        drugingredient = null,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = null,
                        message = cp.Text,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = cd.Key, // request.products.Where(x => x.NameIdentifier == cd.Key).ToArray()[0].ProductCode,
                        primaryprescriptionid = cd.Key,
                        secondarymedicationcode = null,
                        secondaryprescriptionid = null,
                        severity = null,
                        warningcategories = null,
                        warningtype = cp.type.ToString().ToLower(),
                        ispatientspecific = false,
                        msgtype = null

                    };

                    var existing = warningsReponse.FindAll(x => x.primarymedicationcode == warning.primarymedicationcode && x.warningtype == warning.warningtype
                     && x.fdbmessageid == warning.fdbmessageid);
                    if (existing.Count == 0)
                        warningsReponse.Add(warning);
                }
            }

            foreach (var wd in warningsspecific)
            {
                foreach (var wr in wd.Value)
                {
                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = wr.Text,
                        allergencode = null,
                        allergeningredient = null,
                        allergenmatchtype = null,
                        drugingredient = null,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = null,
                        message = wr.Text,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = wd.Key, // request.products.Where(x => x.NameIdentifier == wd.Key).ToArray()[0].ProductCode,
                        primaryprescriptionid = wd.Key,
                        secondarymedicationcode = null,
                        secondaryprescriptionid = null,
                        severity = null,
                        warningcategories = new JavaScriptSerializer().Serialize(wr.Categories),
                        warningtype = WarningType.DrugWarning,
                        ispatientspecific = true,
                        msgtype = null
                    };
                    warningsReponse.Add(warning);
                }
            }

            foreach (var wd in warnings)
            {
                foreach (var wr in wd.Value)
                {
                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = wr.Text,
                        allergencode = null,
                        allergeningredient = null,
                        allergenmatchtype = null,
                        drugingredient = null,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = null,
                        message = wr.Text,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = wd.Key, // request.products.Where(x => x.NameIdentifier == wd.Key).ToArray()[0].ProductCode,
                        primaryprescriptionid = wd.Key,
                        secondarymedicationcode = null,
                        secondaryprescriptionid = null,
                        severity = null,
                        warningcategories = new JavaScriptSerializer().Serialize(wr.Categories),
                        warningtype = WarningType.DrugWarning,
                        ispatientspecific = false,
                        msgtype = null
                    };

                    var existing = warningsReponse.FindAll(x => x.primarymedicationcode == warning.primarymedicationcode && x.warningtype == warning.warningtype
                    && new JavaScriptSerializer().Serialize(x.warningcategories) == new JavaScriptSerializer().Serialize(warning.warningcategories)
                    && x.message == warning.message);
                    if (existing.Count == 0)
                        warningsReponse.Add(warning);

                }
            }




            foreach (var sd in safteymessages)
            {
                foreach (var sf in sd.Value)
                {
                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = sf.SafetyMessage,
                        allergencode = null,
                        allergeningredient = null,
                        allergenmatchtype = null,
                        drugingredient = null,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = null,
                        message = sf.SafetyMessage,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = sd.Key, // request.products.Where(x => x.NameIdentifier == sd.Key).ToArray()[0].ProductCode,
                        primaryprescriptionid = sd.Key,
                        secondarymedicationcode = null,
                        secondaryprescriptionid = null,
                        severity = null,
                        warningcategories = null,
                        warningtype = WarningType.SafetyMessage,
                        ispatientspecific = false,
                        msgtype = sf.MessageType
                    };
                    warningsReponse.Add(warning);
                }
            }


            foreach (var md in mandatoryinstructions)
            {
                foreach (var mi in md.Value)
                {
                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = mi.Id.ToString(),
                        allergencode = null,
                        allergeningredient = null,
                        allergenmatchtype = null,
                        drugingredient = null,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = null,
                        message = mi.Text,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = md.Key, // request.products.Where(x => x.NameIdentifier == md.Key).ToArray()[0].ProductCode,
                        primaryprescriptionid = md.Key,
                        secondarymedicationcode = null,
                        secondaryprescriptionid = null,
                        severity = null,
                        warningcategories = null,
                        warningtype = WarningType.MandatoryInstruction,
                        ispatientspecific = false,
                        msgtype = mi.Type
                    };
                    warningsReponse.Add(warning);
                }
            }


            foreach (var sd in sideeffects)
            {
                foreach (var se in sd.Value)
                {
                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = se.Text,
                        allergencode = null,
                        allergeningredient = null,
                        allergenmatchtype = null,
                        drugingredient = null,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = null,
                        message = se.Text,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = sd.Key, // request.products.Where(x => x.NameIdentifier == sd.Key).ToArray()[0].ProductCode,
                        primaryprescriptionid = sd.Key,
                        secondarymedicationcode = null,
                        secondaryprescriptionid = null,
                        severity = null,
                        warningcategories = null,
                        warningtype = WarningType.SideEffect,
                        ispatientspecific = false,
                        msgtype = null
                    };
                    warningsReponse.Add(warning);
                }
            }


            foreach (var di in drugInteractions)
            {
                var isunique = warningsReponse.Where(w => w.warningtype == WarningType.DrugInteraction
                                                  && w.primaryprescriptionid == di.PrimaryDrug.UserSpecifiedName
                                                  && w.secondaryprescriptionid == di.SecondaryDrug.UserSpecifiedName
                                                  && w.alertmsgplaintext == di.FullAlertMessage
                                                  && w.fdbseverity == di.AlertSeverity.ToString());
                if (isunique.Count() == 0)
                {
                    var ppname = request.products.FirstOrDefault(x => x.NameIdentifier == di.PrimaryDrug.UserSpecifiedName);
                    if (ppname == null)
                        ppname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == di.PrimaryDrug.UserSpecifiedName);

                    var spname = request.products.FirstOrDefault(x => x.NameIdentifier == di.SecondaryDrug.UserSpecifiedName);
                    if (spname == null)
                        spname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == di.SecondaryDrug.UserSpecifiedName);

                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = null,
                        allergencode = null,
                        allergeningredient = null,
                        allergenmatchtype = null,
                        drugingredient = null,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = di.AlertSeverity.ToString(),
                        message = di.FullAlertMessage,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = ppname.ProductCode, // di.PrimaryDrug.SingleId,
                        primaryprescriptionid = di.PrimaryDrug.UserSpecifiedName,
                        secondarymedicationcode = spname.ProductCode,// di.SecondaryDrug.SingleId,
                        secondaryprescriptionid = di.SecondaryDrug.UserSpecifiedName,
                        severity = null,
                        warningcategories = null,
                        warningtype = WarningType.DrugInteraction,
                        ispatientspecific = true,
                        msgtype = null,
                        alertmsgplaintext = di.FullAlertMessage
                    };
                    warning.message = warning.message.Replace(warning.primaryprescriptionid, "<b>" + ppname.TherapyName + "</b>").Replace(warning.secondaryprescriptionid, "<b>" + spname.TherapyName + "</b>");
                    warningsReponse.Add(warning);
                }
            }

            foreach (var db in drugDoubling)
            {
                var isunique = warningsReponse.Where(w => w.warningtype == WarningType.DrugDoubling
                                                    && w.primaryprescriptionid == db.PrimaryDrug.UserSpecifiedName
                                                    && w.secondaryprescriptionid == db.SecondaryDrug.UserSpecifiedName
                                                    && w.alertmsgplaintext == db.FullAlertMessage
                                                    && w.msgtype == db.SharedIngredientDescription);
                if (isunique.Count() == 0)
                {
                    var ppname = request.products.FirstOrDefault(x => x.NameIdentifier == db.PrimaryDrug.UserSpecifiedName);
                    if (ppname == null)
                        ppname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == db.PrimaryDrug.UserSpecifiedName);

                    var spname = request.products.FirstOrDefault(x => x.NameIdentifier == db.SecondaryDrug.UserSpecifiedName);
                    if (spname == null)
                        spname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == db.SecondaryDrug.UserSpecifiedName);
                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = null,
                        allergencode = null,
                        allergeningredient = null,
                        allergenmatchtype = null,
                        drugingredient = null,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = null,
                        message = db.FullAlertMessage,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = ppname.ProductCode, // db.PrimaryDrug.SingleId,
                        primaryprescriptionid = db.PrimaryDrug.UserSpecifiedName,
                        secondarymedicationcode = spname.ProductCode, // db.SecondaryDrug.SingleId,
                        secondaryprescriptionid = db.SecondaryDrug.UserSpecifiedName,
                        severity = null,
                        warningcategories = null,
                        warningtype = WarningType.DrugDoubling,
                        ispatientspecific = true,
                        msgtype = db.SharedIngredientDescription,
                        alertmsgplaintext = db.FullAlertMessage
                    };
                    //warning.message = warning.message.Replace(warning.primaryprescriptionid, ppname.TherapyName).Replace(warning.secondaryprescriptionid, spname.TherapyName);
                    warning.message = warning.message.Replace(warning.primaryprescriptionid, "<b>" + ppname.TherapyName + "</b>").Replace(warning.secondaryprescriptionid, "<b>" + spname.TherapyName + "</b>");

                    warningsReponse.Add(warning);
                }
            }

            foreach (var de in drugEquivalence)
            {
                var isunique = warningsReponse.Where(w => w.warningtype == WarningType.DrugEquivalance
                                                   && w.primaryprescriptionid == de.PrimaryDrug.UserSpecifiedName
                                                   && w.secondaryprescriptionid == de.SecondaryDrug.UserSpecifiedName
                                                   && w.alertmsgplaintext == de.FullAlertMessage
                                                   && w.msgtype == de.SharedCoreGenericDescription);
                if (isunique.Count() == 0)
                {
                    var ppname = request.products.FirstOrDefault(x => x.NameIdentifier == de.PrimaryDrug.UserSpecifiedName);
                    if (ppname == null)
                        ppname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == de.PrimaryDrug.UserSpecifiedName);

                    var spname = request.products.FirstOrDefault(x => x.NameIdentifier == de.SecondaryDrug.UserSpecifiedName);
                    if (spname == null)
                        spname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == de.SecondaryDrug.UserSpecifiedName);
                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = null,
                        allergencode = null,
                        allergeningredient = null,
                        allergenmatchtype = null,
                        drugingredient = null,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = null,
                        message = de.FullAlertMessage,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = ppname.ProductCode, // de.PrimaryDrug.SingleId,
                        primaryprescriptionid = de.PrimaryDrug.UserSpecifiedName,
                        secondarymedicationcode = spname.ProductCode, // de.SecondaryDrug.SingleId,
                        secondaryprescriptionid = de.SecondaryDrug.UserSpecifiedName,
                        severity = null,
                        warningcategories = null,
                        warningtype = WarningType.DrugEquivalance,
                        ispatientspecific = true,
                        msgtype = de.SharedCoreGenericDescription,
                        alertmsgplaintext = de.FullAlertMessage
                    };
                    warning.message = warning.message.Replace(warning.primaryprescriptionid, "<b>" + ppname.TherapyName + "</b>").Replace(warning.secondaryprescriptionid, "<b>" + spname.TherapyName + "</b>");

                    //warning.message = warning.message.Replace(warning.primaryprescriptionid, ppname.TherapyName).Replace(warning.secondaryprescriptionid, spname.TherapyName);

                    warningsReponse.Add(warning);
                }
            }

            foreach (var dt in duplicateTherapies)
            {
                var isunique = warningsReponse.Where(w => w.warningtype == WarningType.DuplicateTherapy
                                                  && w.primaryprescriptionid == dt.PrimaryDrug.UserSpecifiedName
                                                  && w.secondaryprescriptionid == dt.SecondaryDrug.UserSpecifiedName
                                                  && w.alertmsgplaintext == dt.FullAlertMessage
                                                  && w.msgtype == dt.CommonTherapeuticClassDescription);
                if (isunique.Count() == 0)
                {
                    var ppname = request.products.FirstOrDefault(x => x.NameIdentifier == dt.PrimaryDrug.UserSpecifiedName);
                    if (ppname == null)
                        ppname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == dt.PrimaryDrug.UserSpecifiedName);

                    var spname = request.products.FirstOrDefault(x => x.NameIdentifier == dt.SecondaryDrug.UserSpecifiedName);
                    if (spname == null)
                        spname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == dt.SecondaryDrug.UserSpecifiedName);

                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = null,
                        allergencode = null,
                        allergeningredient = null,
                        allergenmatchtype = null,
                        drugingredient = null,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = null,
                        message = dt.FullAlertMessage,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = ppname.ProductCode, // dt.PrimaryDrug.SingleId,
                        primaryprescriptionid = dt.PrimaryDrug.UserSpecifiedName,
                        secondarymedicationcode = spname.ProductCode, // dt.SecondaryDrug.SingleId,
                        secondaryprescriptionid = dt.SecondaryDrug.UserSpecifiedName,
                        severity = null,
                        warningcategories = null,
                        warningtype = WarningType.DuplicateTherapy,
                        ispatientspecific = true,
                        msgtype = dt.CommonTherapeuticClassDescription,
                        alertmsgplaintext = dt.FullAlertMessage
                    };

                    warning.message = warning.message.Replace(warning.primaryprescriptionid, "<b>" + ppname.TherapyName + "</b>").Replace(warning.secondaryprescriptionid, "<b>" + spname.TherapyName + "</b>");

                    //warning.message = warning.message.Replace(warning.primaryprescriptionid, ppname.TherapyName).Replace(warning.secondaryprescriptionid, spname.TherapyName);

                    warningsReponse.Add(warning);
                }
            }

            foreach (var ds in drugSensitivities)
            {
                var isunique = warningsReponse.Where(w => w.warningtype == WarningType.Sensitivity
                                                  && w.primaryprescriptionid == ds.Drug.UserSpecifiedName
                                                  && w.allergencode == ds.Allergen.Id
                                                  && w.alertmsgplaintext == ds.FullAlertMessage
                                                  && w.msgtype == ds.Allergen.Concept.ToString());
                if (isunique.Count() == 0)
                {
                    var ppname = request.products.FirstOrDefault(x => x.NameIdentifier == ds.Drug.UserSpecifiedName);
                    if (ppname == null)
                        ppname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == ds.Drug.UserSpecifiedName);



                    var warning = new EPMAWarnings
                    {
                        encounter_id = "",
                        person_id = "",
                        fdbmessageid = null,
                        allergencode = ds.Allergen.Id,
                        allergeningredient = ds.AllergenIngredientDescription,
                        allergenmatchtype = ds.MatchType.ToString(),
                        drugingredient = ds.DrugIngredientDescription,
                        epma_warnings_id = Guid.NewGuid().ToString(),
                        fdbseverity = null,
                        message = ds.FullAlertMessage,
                        overridemessage = null,
                        overriderequired = null,
                        primarymedicationcode = ppname.ProductCode, // ds.Drug.SingleId,
                        primaryprescriptionid = ds.Drug.UserSpecifiedName,
                        secondarymedicationcode = null,
                        secondaryprescriptionid = null,
                        severity = null,
                        warningcategories = null,
                        warningtype = WarningType.Sensitivity,
                        ispatientspecific = true,
                        msgtype = ds.Allergen.Concept.ToString(),
                        alertmsgplaintext = ds.FullAlertMessage
                    };
                    warning.message = warning.message.Replace(warning.primaryprescriptionid, "<b>" + ppname.TherapyName + "</b>");

                    //warning.message = warning.message.Replace(warning.primaryprescriptionid, ppname.TherapyName);

                    warningsReponse.Add(warning);
                }
            }

            foreach (var item in errors)
            {

                var ppname = request.products.FirstOrDefault(x => x.NameIdentifier == item.TriggeredBy.Drug.UserSpecifiedName);
                if (ppname == null)
                    ppname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == item.TriggeredBy.Drug.UserSpecifiedName);



                var warning = new EPMAWarnings
                {
                    encounter_id = "",
                    person_id = "",
                    fdbmessageid = null,
                    allergencode = null,
                    allergeningredient = null,
                    allergenmatchtype = null,
                    drugingredient = null,
                    epma_warnings_id = Guid.NewGuid().ToString(),
                    fdbseverity = item.ErrorSeverity.ToString(),
                    message = item.Description,
                    overridemessage = null,
                    overriderequired = null,
                    primarymedicationcode = ppname.ProductCode, // item.TriggeredBy.Drug.SingleId,
                    primaryprescriptionid = item.TriggeredBy.Drug.UserSpecifiedName,
                    primarymedicationname = ppname.TherapyName,
                    secondarymedicationcode = null,
                    secondaryprescriptionid = null,
                    severity = null,
                    warningcategories = item.ErrorType.ToString(),
                    warningtype = "error",
                    ispatientspecific = false,
                    msgtype = item.DescriptionType.ToString()
                };
                warning.message = warning.message.Replace(warning.primaryprescriptionid, "<b>" + ppname.TherapyName + "</b>");

                //warning.message = warning.message.Replace(warning.primaryprescriptionid, ppname.TherapyName);

                warningsReponse.Add(warning);
            }

            foreach (var item in _unknownproducts)
            {
                var ppname = request.products.FirstOrDefault(x => x.NameIdentifier == item.uniqueidentifier);
                if (ppname == null)
                    ppname = request.currentproducts.FirstOrDefault(x => x.NameIdentifier == item.uniqueidentifier);

                var warning = new EPMAWarnings
                {
                    encounter_id = "",
                    person_id = "",
                    fdbmessageid = null,
                    allergencode = null,
                    allergeningredient = null,
                    allergenmatchtype = null,
                    drugingredient = null,
                    epma_warnings_id = Guid.NewGuid().ToString(),
                    fdbseverity = null,
                    message = item.message,
                    overridemessage = null,
                    overriderequired = null,
                    primarymedicationcode = item.productcode,
                    primaryprescriptionid = item.uniqueidentifier,
                    primarymedicationname = ppname.TherapyName,
                    secondarymedicationcode = null,
                    secondaryprescriptionid = null,
                    severity = null,
                    warningcategories = item.category,
                    warningtype = "error",
                    ispatientspecific = false,
                    msgtype = item.category + (!string.IsNullOrEmpty(item.subcategory) ? (" - " + item.subcategory) : "")
                };
                warning.message = warning.message.Replace(warning.primaryprescriptionid, "<b>" + ppname.TherapyName + "</b>");

                //warning.message = warning.message.Replace(warning.primaryprescriptionid, ppname.TherapyName);

                warningsReponse.Add(warning);
            }


            System.Diagnostics.Debug.WriteLine("done " + DateTime.Now);

            return Ok(warningsReponse);

        }

    }
}
