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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace InterneuronFDBAPI.Controllers
{
    [RoutePrefix("api/fdbplayground")]
    public class FDBPlaygroundController : ApiController
    {
        [HttpGet]
        [Route("allroutes")]
        public void AllRoutes()
        {

            var drugSys = DrugSystemFactory.CreateSystem();
            drugSys.Environment.DrugTerminology = DrugTerminology.SNoMedCT;
            drugSys.Environment.PrimaryPreferredNameFormat.All();
            drugSys.Environment.SecondaryPreferredNameFormat.All();

            drugSys.Environment.AttributePopulation.All();
            drugSys.Environment.MethodPopulation.GetSCTDrugLinks = true;


            var allRoutes = drugSys.Navigation.GetAllRoutes();

        }
        [HttpGet]
        [Route("testsnomed")]
        public void TestSnomed(string prodId = null, string prodName = null)
        {
            var drugSys = DrugSystemFactory.CreateSystem();
            drugSys.Environment.DrugTerminology = DrugTerminology.SNoMedCT;
            drugSys.Environment.PrimaryPreferredNameFormat.All();
            drugSys.Environment.SecondaryPreferredNameFormat.All();

            drugSys.Environment.AttributePopulation.All();
            drugSys.Environment.MethodPopulation.GetSCTDrugLinks = true;

            if (!string.IsNullOrEmpty(prodId))
            {
                var drugBase = drugSys.ObjectFactory.Create_DrugBase(string.Empty, DrugTerminology.SNoMedCT, DrugConceptType.Product, prodId);
                var prod = (ProductUK)drugSys.Navigation.GetProductById(drugBase);
                var classifications = prod.GetTherapeuticClassifications();
                var highRisk = prod.HighRisk;

                var classificationMemberships = prod.GetTherapeuticClassificationsMembership();

                if (classifications != null)
                {
                    foreach (var item in classifications)
                    {
                        var text = $"{item.Text}-{item.Reference}";
                    }
                }

                if (classificationMemberships != null)
                {
                    foreach (var item in classificationMemberships)
                    {
                        var text = $"{item.Text}-{item.Reference}";
                    }
                }

                var patient = new PatientInformation(Gender.Unknown, 17600);
                //134517009
                var doseSuggestions = prod.GetDoseSuggestions(patient, false);

                foreach (var dose in doseSuggestions)
                {
                    var text = dose.GetText();
                    var upperDoseBound = dose.GetUpperBounds();
                    var lowerDoseBound = dose.GetLowerBounds();
                }


                var suggestedSupply = prod.GetSuggestedSupply();
                foreach (var item in suggestedSupply)
                {
                    var x = item;
                }

            }

            if (!string.IsNullOrEmpty(prodName))
            {
                var prods = drugSys.Navigation.GetProductsByName(prodName, new Filter());
                if (prods != null)
                {
                    foreach (var prod in prods)
                    {
                        var scrDrugLinks = prod.GetSCTDrugLinks();
                    }
                }
            }
        }

        [HttpGet]
        [Route("test")]
        public void Test(string prodId = null, string prodName = null)
        {
            var drugSys = DrugSystemFactory.CreateSystem();
            drugSys.Environment.DrugTerminology = DrugTerminology.MDDF;
            drugSys.Environment.PrimaryPreferredNameFormat.All();
            drugSys.Environment.SecondaryPreferredNameFormat.All();

            drugSys.Environment.AttributePopulation.All();
            drugSys.Environment.MethodPopulation.GetSCTDrugLinks = true;

            if (!string.IsNullOrEmpty(prodId))
            {
                var drugBase = drugSys.ObjectFactory.Create_DrugBase(string.Empty, DrugTerminology.MDDF, DrugConceptType.Product, prodId);
                var prod = (ProductUK)drugSys.Navigation.GetProductById(drugBase);

                var scrDrugLinks = prod.GetSCTDrugLinks();

                var patient = new PatientInformation(Gender.Unknown, 02);

                var doseSuggestions = prod.GetDoseSuggestions(patient, true);

                var suggestedSupply = prod.GetSuggestedSupply();
                foreach (var item in suggestedSupply)
                {
                    var x = item;
                }

            }

            if (!string.IsNullOrEmpty(prodName))
            {

                var prods = drugSys.Navigation.GetProductsByName(prodName, new Filter());
                if (prods != null)
                {
                    foreach (var prod in prods)
                    {
                        var scrDrugLinks = prod.GetSCTDrugLinks();
                    }
                }
            }
        }
    }


    //[HttpGet]
    //[Route("test123")]
    //public void x(string code)
    //{
    //    //DrugSystem system = DrugSystemFactory.CreateSystem();
    //    DrugSystem drugSystem = InitializeDrugSystem();

    //    // Note - you could use a FilterUK here if required
    //    FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
    //    filter.DrugClass.Brand = true;
    //    filter.DrugClass.BrandedGeneric = true;
    //    filter.DrugClass.Generic = true;

    //    var ver = drugSystem.GetVersion();

    //    DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Product, code);

    //    DrugBase oDrugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.OrderableMed, code);

    //    var orderMed = drugSystem.Navigation.GetOrderableMedById(oDrugBase);

    //    Product product = drugSystem.Navigation.GetProductById(drugBase);


    //    PatientInformation pInfo = new PatientInformation(Gender.Unknown, 9000);//, 45, 1.56f, conditions, allergens, curDrugs, prospDrugs);

    //    Dose[] suggestions = product.GetDoseSuggestions(pInfo, true);

    //    var oSuggstns = orderMed.GetDoseSuggestions(pInfo);
    //}

    //[HttpGet]
    //[Route("test1234")]
    //public void x1(string code)
    //{
    //    //DrugSystem system = DrugSystemFactory.CreateSystem();
    //    DrugSystem drugSystem = InitializeDrugSystem();

    //    // Note - you could use a FilterUK here if required
    //    FirstDataBank.DrugServer.API.Filter filter = new FirstDataBank.DrugServer.API.Filter();
    //    filter.DrugClass.Brand = true;
    //    filter.DrugClass.BrandedGeneric = true;

    //    DrugBase drugBase = drugSystem.ObjectFactory.Create_DrugBase("", DrugTerminology.SNoMedCT, DrugConceptType.Drug, code);

    //    Product product = drugSystem.Navigation.GetProductById(drugBase);


    //    PatientInformation pInfo = new PatientInformation(Gender.Female, 11680);//, 45, 1.56f, conditions, allergens, curDrugs, prospDrugs);

    //    Dose[] suggestions = product.GetDoseSuggestions(pInfo, true);
    //}
}