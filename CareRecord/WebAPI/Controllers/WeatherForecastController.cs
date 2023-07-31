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
﻿//using System;
//using Hl7.Fhir.Model;
//using Interneuron.CareRecord.Service.Interfaces;
//using Interneuron.CareRecord.Model.DomainModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Linq;
//using Interneuron.CareRecord.API.AppCode.Core;
//using System.Collections.Generic;
//using static Hl7.Fhir.Model.Patient;
//using Interneuron.Common.Extensions;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;

//namespace Interneuron.CareRecord.API.Controllers
//{
//    [ApiController]
//    [Route("[controller]")]
//    public class WeatherForecastController : ControllerBase
//    {
//        private IServiceProvider provider = null;

//        private static readonly string[] Summaries = new[]
//        {
//            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//        };

//        private readonly ILogger<WeatherForecastController> _logger;
//        //private readonly ISynapseResourceService _readonlyRepo;

//        //public WeatherForecastController(ILogger<WeatherForecastController> logger, ISynapseResourceService readonlyRepo)
//        public WeatherForecastController(ILogger<WeatherForecastController> logger, IServiceProvider provider)
//        //public WeatherForecastController(ILogger<WeatherForecastController> logger)
//        {
//            _logger = logger;
//            // _readonlyRepo = readonlyRepo;
//            this.provider = provider;
//        }

//        [HttpGet]
//        [Route("")]
//        [Route("[action]/{personId?}")]
//        public FhirResponse Get(string personId)
//        {
//            //ISynapseResourceService<entitystorematerialised_CorePerson> x = this.provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePerson>)) as ISynapseResourceService<entitystorematerialised_CorePerson>;

//            //var peron = x.GetQuerable().FirstOrDefault();
//            //var birth = peron.Dateofbirth;

//            //var patient = new Patient();
//            //var id = new Identifier();
//            //id.Value = "000-12-3456";
//            //patient.Id = "aaaa";
//            //var contact = new Patient.ContactComponent();
//            //contact.Name = new HumanName();
//            //contact.Name.Family = "Parks";
//            //setup other contact details
//            //patient.Contact.Add(contact);
//            //patient.Gender = AdministrativeGender.Male;

//            Patient patient = new Patient();

//            entitystorematerialised_CorePerson person = new entitystorematerialised_CorePerson();

//            ISynapseResourceService<entitystorematerialised_CorePerson> x = this.provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePerson>)) as ISynapseResourceService<entitystorematerialised_CorePerson>;

//            if (!string.IsNullOrEmpty(personId))
//            {
//                IEnumerable<entitystorematerialised_CorePerson> persons = x.GetQuerable().Where(x => x.PersonId == personId);

//                foreach (entitystorematerialised_CorePerson p in persons)
//                {
//                    person = p;
//                }
//            }
//            else
//            {
//                person = x.GetQuerable().FirstOrDefault();
//            }

//            ISynapseResourceService<entitystorematerialised_CorePersonidentifier> pi = this.provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersonidentifier>)) as ISynapseResourceService<entitystorematerialised_CorePersonidentifier>;

//            ISynapseResourceService<entitystorematerialised_CorePersonaddress1> y = this.provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersonaddress1>)) as ISynapseResourceService<entitystorematerialised_CorePersonaddress1>;

//            ISynapseResourceService<entitystorematerialised_CorePersoncontactinfo1> z = this.provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersoncontactinfo1>)) as ISynapseResourceService<entitystorematerialised_CorePersoncontactinfo1>;

//            ISynapseResourceService<entitystorematerialised_CoreNextofkin1> n = this.provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CoreNextofkin1>)) as ISynapseResourceService<entitystorematerialised_CoreNextofkin1>;


//            if (!string.IsNullOrEmpty(person.PersonId))
//            {
//                List<entitystorematerialised_CorePersonidentifier> personIdentifiers = pi.GetQuerable().Where(x => x.PersonId == person.PersonId).ToList();

//                List<entitystorematerialised_CorePersonaddress1> personAddresses = y.GetQuerable().Where(x => x.PersonId == person.PersonId).ToList();

//                List<entitystorematerialised_CorePersoncontactinfo1> personContacts = z.GetQuerable().Where(x => x.PersonId == person.PersonId).ToList();

//                List<entitystorematerialised_CoreNextofkin1> nextOfKins = n.GetQuerable().Where(x => x.PersonId == person.PersonId).ToList();

//                patient.Identifier = new List<Identifier>();

//                foreach (entitystorematerialised_CorePersonidentifier personIdentifier in personIdentifiers)
//                {
//                    Identifier id = new Identifier();
//                    id.Value = personIdentifier.Idnumber;
//                    id.System = personIdentifier.Idtypecode;

//                    patient.Identifier.Add(id);
//                }

//                patient.Name.Add(new HumanName().WithGiven(person.Firstname).AndFamily(person.Familyname));

//                patient.MaritalStatus = new CodeableConcept();

//                patient.MaritalStatus.Text = person.Maritalstatustext;

//                patient.BirthDate = person.Dateofbirth.ToString();

//                var gender = new AdministrativeGender();

//                switch (person.Gendertext.ToLower())
//                {
//                    case "male":
//                        gender = AdministrativeGender.Male;
//                        break;
//                    case "female":
//                        gender = AdministrativeGender.Female;
//                        break;
//                    case "m":
//                        gender = AdministrativeGender.Male;
//                        break;
//                    case "f":
//                        gender = AdministrativeGender.Female;
//                        break;
//                    default:
//                        gender = AdministrativeGender.Unknown;
//                        break;
//                }

//                patient.Gender = gender;

//                patient.Address = new List<Address>();

//                foreach (entitystorematerialised_CorePersonaddress1 personAddress in personAddresses)
//                {
//                    Address address = new Address();
//                    address.Line = address.Line.Append(personAddress.Line1);
//                    address.Line = address.Line.Append(", ");
//                    address.Line = address.Line.Append(personAddress.Line2);
//                    address.Line = address.Line.Append(", ");
//                    address.Line = address.Line.Append(personAddress.Line3);
//                    address.City = personAddress.City;
//                    address.State = personAddress.Countystateprovince;
//                    address.Country = personAddress.Country;
//                    address.PostalCode = personAddress.Postcodezip;
//                    address.Type = Address.AddressType.Both;
//                    patient.Address.Add(address);
//                }

//                patient.Telecom = new List<ContactPoint>();

//                foreach (entitystorematerialised_CorePersoncontactinfo1 personContact in personContacts)
//                {
//                    ContactPoint contact = new ContactPoint();
//                    contact.Value = personContact.Contactdetails;

//                    switch (personContact.Contacttypecode.ToLower())
//                    {
//                        case "email":
//                            contact.System = ContactPoint.ContactPointSystem.Email;
//                            break;
//                        case "home":
//                            contact.System = ContactPoint.ContactPointSystem.Phone;
//                            contact.Use = ContactPoint.ContactPointUse.Home;
//                            break;
//                        case "personal":
//                            contact.System = ContactPoint.ContactPointSystem.Phone;
//                            contact.Use = ContactPoint.ContactPointUse.Mobile;
//                            break;
//                        case "work":
//                            contact.System = ContactPoint.ContactPointSystem.Other;
//                            contact.Use = ContactPoint.ContactPointUse.Work;
//                            break;
//                        default:
//                            contact.System = ContactPoint.ContactPointSystem.Other;
//                            break;
//                    }

//                    patient.Telecom.Add(contact);
//                }

//                patient.Contact = new List<ContactComponent>();

//                foreach (entitystorematerialised_CoreNextofkin1 nextOfKin in nextOfKins)
//                {
//                    ContactComponent contactComponent = new ContactComponent();
//                    contactComponent.Address = new Address();
//                    contactComponent.Address.Line = contactComponent.Address.Line.Append(nextOfKin.Addressstreet);
//                    contactComponent.Address.Line = contactComponent.Address.Line.Append(", ");
//                    contactComponent.Address.Line = contactComponent.Address.Line.Append(nextOfKin.Addressstreet2);
//                    contactComponent.Address.City = nextOfKin.Addresscity;
//                    contactComponent.Address.State = nextOfKin.Addresscountystateprovince;
//                    contactComponent.Address.Country = nextOfKin.Addresscountry;
//                    contactComponent.Address.PostalCode = nextOfKin.Addresspostalcode;
//                    contactComponent.Address.Type = Address.AddressType.Both;

//                    List<CodeableConcept> relationships = new List<CodeableConcept>();
//                    CodeableConcept relationship = new CodeableConcept();

//                    relationship.Text = nextOfKin.Relationship;

//                    relationships.Add(relationship);

//                    contactComponent.Relationship = relationships;

//                    HumanName name = new HumanName();

//                    contactComponent.Name = name.WithGiven(nextOfKin.Givenname).AndFamily(nextOfKin.Familyname);

//                    ContactPoint contact = new ContactPoint();


//                    if (!string.IsNullOrEmpty(nextOfKin.Personalcontactinfo))
//                    {
//                        contact.Value = nextOfKin.Personalcontactinfo;
//                        contact.System = ContactPoint.ContactPointSystem.Phone;
//                        contact.Use = ContactPoint.ContactPointUse.Home;
//                    }

//                    if (string.IsNullOrEmpty(nextOfKin.Personalcontactinfo) && !string.IsNullOrEmpty(nextOfKin.Businesscontactinfo))
//                    {
//                        contact.Value = nextOfKin.Businesscontactinfo;
//                        contact.System = ContactPoint.ContactPointSystem.Phone;
//                        contact.Use = ContactPoint.ContactPointUse.Work;
//                    }

//                    contactComponent.Telecom.Add(contact);

//                    patient.Contact.Add(contactComponent);
//                }

//                var fhirResponse = new FhirResponse(System.Net.HttpStatusCode.OK, patient);
//                return fhirResponse;
//            }
//            else
//            {
//                var fhirResponse = new FhirResponse(System.Net.HttpStatusCode.NoContent);

//                entitystore_CorePerson1 entitystore_person = new entitystore_CorePerson1();

//                ISynapseResourceService<entitystore_CorePerson1> a = this.provider.GetService(typeof(ISynapseResourceService<entitystore_CorePerson1>)) as ISynapseResourceService<entitystore_CorePerson1>;

//                var persons1 = a.GetQuerable().Where(x => x.PersonId == personId).OrderByDescending(x => x.Sequenceid).FirstOrDefault();

//                if (persons1 != null && persons1.PersonId.IsNotEmpty() && persons1.Recordstatus == 2)
//                {
//                    fhirResponse = new FhirResponse(System.Net.HttpStatusCode.Gone);
//                }
//                else {
//                    fhirResponse = new FhirResponse(System.Net.HttpStatusCode.NotFound);
//                }

//                return fhirResponse;
//            }


//        }

//        [HttpGet]
//        [Route("")]
//        [Route("[action]/{encounterId?}")]
//        public FhirResponse GetEncounter(string encounterId)
//        {
//            Encounter encounter = new Encounter();

//            entitystorematerialised_CoreEncounter enc = new entitystorematerialised_CoreEncounter();

//            ISynapseResourceService<entitystorematerialised_CoreEncounter> en = this.provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CoreEncounter>)) as ISynapseResourceService<entitystorematerialised_CoreEncounter>;

//            encounter.Identifier = new List<Identifier>();

//            if (!string.IsNullOrEmpty(encounterId))
//            {
//                entitystorematerialised_CoreEncounter enc1 = en.GetQuerable().Where(x => x.EncounterId == encounterId).FirstOrDefault();

//                Identifier id = new Identifier();
//                id.Value = enc1.Visitnumber;

//                switch (enc1.Episodestatuscode.ToLower())
//                {
//                    case "active":
//                        encounter.Status = Encounter.EncounterStatus.InProgress;
//                        break;
//                    case "cancelled":
//                        encounter.Status = Encounter.EncounterStatus.Cancelled;
//                        break;
//                }

//                encounter.Class.Code = enc1.Patientclasscode;



//            }


//            var fhirResponse = new FhirResponse(System.Net.HttpStatusCode.OK, encounter);
//            return fhirResponse;
//        }

//        //public IEnumerable<WeatherForecast> Get()
//        //{
//        //    //var x = _readonlyRepo.ItemsAsReadOnly.FirstOrDefault();
//        //    //var x = _readonlyRepo.GetQuerable<CorePerson>().FirstOrDefault();


//        //    var resouceService = Startup.AutofacContainer.Resolve<ISynapseResourceService<CorePerson>>();

//        //    var corePerson = resouceService.GetQuerable().FirstOrDefault();

//        //    var patient = new Patient();
//        //    var id = new Identifier();
//        //    id.Value = "000-12-3456";
//        //    patient.Id = "aaaa";
//        //    var contact = new Patient.ContactComponent();
//        //    contact.Name = new HumanName();
//        //    contact.Name.Family = "Parks";
//        //    // setup other contact details
//        //    patient.Contact.Add(contact);
//        //    patient.Gender = AdministrativeGender.Male;


//        //    var rng = new Random();
//        //    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
//        //    {
//        //        Date = DateTime.Now.AddDays(index),
//        //        TemperatureC = rng.Next(-20, 55),
//        //        Summary = Summaries[rng.Next(Summaries.Length)]
//        //    })
//        //    .ToArray();
//        //}

//        //This is just a temporary implementation
        
//    }
//}
