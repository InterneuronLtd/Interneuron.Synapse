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


﻿using System.Collections.Generic;

namespace InterneuronFDBAPI.Controllers
{
    public class FDBDataRequest
    {
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
    }

    public class FDBEPMADataRequest
    {
        public string ProductType { get; set; }
        public string ProductCode { get; set; }
        public string NameIdentifier { get; set; }
        public string TherapyName { get; set; }
        public Route[] Routes { get; set; }

    }

    public class Route
    {
        public string name;
        public string code;
    }

    public class PatientInfo
    {
        public int gender { get; set; }
        public int age { get; set; }
        public float weight { get; set; }
        public float bsa { get; set; }
        public Allergens[] allergens { get; set; }

        public Conditions[] conditions { get; set; }

        public PatientInfo()
        {

        }

        public PatientInfo(int gender, int age, float weight, float bsa)
        {
            this.gender = gender;
            this.age = age;
            this.weight = weight;
            this.bsa = bsa;
        }
    }

    public class Allergens
    {
        public string uname { get; set; }
        public int type { get; set; }
        public string code { get; set; }

    }

    public class Conditions
    {
        public string uname { get; set; }
        public int type { get; set; }
        public string code { get; set; }

    }

    public class FDBDataRequestPatientSpecific
    {
        public List<FDBEPMADataRequest> products;
        public List<FDBEPMADataRequest> currentproducts;
        public PatientInfo patientinfo;
        public List<string> warningtypes;
    }

    public struct WarningType
    {
        public static string DrugWarning = "drugwarnings";
        public static string ContraIndication = "contraindication";
        public static string Precaution = "precautions";
        public static string SafetyMessage = "safetymessage";
        public static string MandatoryInstruction = "mandatoryinstruction";
        public static string SideEffect = "sideeffect";
        public static string DrugInteraction = "druginteraction";
        public static string DrugDoubling = "drugdoubling";
        public static string DrugEquivalance = "drugequivalance";
        public static string DuplicateTherapy = "duplicatetherapy";
        public static string Sensitivity = "sensitivity";
    }

    public class GenericError
    {
        public string category;
        public string productcode;
        public string message;
        public string subcategory;
        public string uniqueidentifier;
        public GenericError(string message, string uniqueidentifier, string productcode = null, string category = null, string subcategory = null)
        {
            this.uniqueidentifier = uniqueidentifier;
            this.message = message;
            this.category = category;
            this.subcategory = subcategory;
            this.productcode = productcode;
        }
    }
}
