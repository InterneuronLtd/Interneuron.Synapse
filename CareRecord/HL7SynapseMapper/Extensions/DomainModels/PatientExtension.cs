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
//using Hl7.Fhir.Model;
//using Interneuron.CareRecord.Model.DomainModels;
//using Interneuron.Common.Extensions;
//using System.Collections.Generic;
//using System.Linq;
//using static Hl7.Fhir.Model.Patient;

//namespace Interneuron.CareRecord.HL7SynapseService.Implementations.ModelExtensions
//{
//    public static class PatientExtension
//    {
//        public static Patient GetPatientForPerson(this entitystorematerialised_CorePerson corePerson)
//        {
//            if (corePerson.IsNull()) return null;

//            var patient = new Patient
//            {
//                Id = corePerson.PersonId,
//                BirthDate = corePerson.Dateofbirth.ToString(),
//                MaritalStatus = new CodeableConcept { Text = corePerson.Maritalstatustext },
//            };
//            patient.Name.Add(new HumanName()
//                .WithGiven(corePerson.Firstname)
//                .AndFamily(corePerson.Familyname));

//            switch (corePerson.Gendertext.ToLower())
//            {
//                case "male":
//                    patient.Gender = AdministrativeGender.Male;
//                    break;
//                case "female":
//                    patient.Gender = AdministrativeGender.Female;
//                    break;
//                case "m":
//                    patient.Gender = AdministrativeGender.Male;
//                    break;
//                case "f":
//                    patient.Gender = AdministrativeGender.Female;
//                    break;
//                default:
//                    patient.Gender = AdministrativeGender.Unknown;
//                    break;
//            }

//            return patient;
//        }

//        public static Identifier GetIdentifierForPeron(this entitystorematerialised_CorePersonidentifier personIdentifier)
//        {
//            if (personIdentifier.IsNull()) return null;

//            return new Identifier
//            {
//                Value = personIdentifier.Idnumber,
//                System = personIdentifier.Idtypecode
//            };
//        }

//        public static List<Identifier> GetIdentifiersForPerson(this IEnumerable<entitystorematerialised_CorePersonidentifier> personIdentifiers)
//        {
//            var identifiers = new List<Identifier>();

//            if (!personIdentifiers.IsCollectionValid()) return identifiers;

//            personIdentifiers.Each(pi =>
//            {
//                var identifier = pi.GetIdentifierForPeron();

//                if (identifier != null)
//                    identifiers.Add(identifier);
//            });

//            return identifiers;
//        }

//        public static Address GetAddressForPerson(this entitystorematerialised_CorePersonaddress1 personAddress)
//        {
//            if (personAddress.IsNull()) return null;

//            Address address = new Address();
//            address.Line = address.Line.Append(personAddress.Line1);
//            address.Line = address.Line.Append(", ");
//            address.Line = address.Line.Append(personAddress.Line2);
//            address.Line = address.Line.Append(", ");
//            address.Line = address.Line.Append(personAddress.Line3);
//            address.City = personAddress.City;
//            address.State = personAddress.Countystateprovince;
//            address.Country = personAddress.Country;
//            address.PostalCode = personAddress.Postcodezip;
//            address.Type = Address.AddressType.Both;

//            return address;
//        }

//        public static List<Address> GetAddressesForPerson(this IEnumerable<entitystorematerialised_CorePersonaddress1> personAddresses)
//        {
//            var addresses = new List<Address>();

//            if (!personAddresses.IsCollectionValid()) return addresses;

//            personAddresses.Each(addr =>
//            {
//                var addrPerson = addr.GetAddressForPerson();

//                if (addrPerson != null)
//                    addresses.Add(addrPerson);
//            });

//            return addresses;
//        }

//        public static ContactPoint GetContactInfoForPerson(this entitystorematerialised_CorePersoncontactinfo1 personContact)
//        {
//            if (personContact.IsNull()) return null;

//            ContactPoint contact = new ContactPoint
//            {
//                Value = personContact.Contactdetails
//            };

//            switch (personContact.Contacttypecode.ToLower())
//            {
//                case "email":
//                    contact.System = ContactPoint.ContactPointSystem.Email;
//                    break;
//                case "home":
//                    contact.System = ContactPoint.ContactPointSystem.Phone;
//                    contact.Use = ContactPoint.ContactPointUse.Home;
//                    break;
//                case "personal":
//                    contact.System = ContactPoint.ContactPointSystem.Phone;
//                    contact.Use = ContactPoint.ContactPointUse.Mobile;
//                    break;
//                case "work":
//                    contact.System = ContactPoint.ContactPointSystem.Other;
//                    contact.Use = ContactPoint.ContactPointUse.Work;
//                    break;
//                default:
//                    contact.System = ContactPoint.ContactPointSystem.Other;
//                    break;
//            }

//            return contact;
//        }

//        public static List<ContactPoint> GetContactInfosForPerson(this IEnumerable<entitystorematerialised_CorePersoncontactinfo1> personContacts)
//        {
//            var contactPoints = new List<ContactPoint>();

//            if (!personContacts.IsCollectionValid()) return contactPoints;

//            personContacts.Each(pc =>
//            {
//                var contactPoint = pc.GetContactInfoForPerson();

//                if (contactPoint != null)
//                    contactPoints.Add(contactPoint);
//            });

//            return contactPoints;
//        }

//        public static ContactComponent GetContactInfoForKin(this entitystorematerialised_CoreNextofkin1 nextOfKin)
//        {
//            if (nextOfKin.IsNull()) return null;

//            ContactComponent contactComponent = new ContactComponent
//            {
//                Address = new Address()
//            };

//            string addresssLine = string.Join(nextOfKin.Addressstreet, ", ", nextOfKin.Addressstreet2);

//            contactComponent.Address.Line = contactComponent.Address.Line.Append(addresssLine);
//            contactComponent.Address.City = nextOfKin.Addresscity;
//            contactComponent.Address.State = nextOfKin.Addresscountystateprovince;
//            contactComponent.Address.Country = nextOfKin.Addresscountry;
//            contactComponent.Address.PostalCode = nextOfKin.Addresspostalcode;
//            contactComponent.Address.Type = Address.AddressType.Both;

//            List<CodeableConcept> relationships = new List<CodeableConcept>();

//            CodeableConcept relationship = new CodeableConcept
//            {
//                Text = nextOfKin.Relationship
//            };

//            relationships.Add(relationship);

//            contactComponent.Relationship = relationships;

//            HumanName nextOfKinName = new HumanName();

//            contactComponent.Name = nextOfKinName.WithGiven(nextOfKin.Givenname).AndFamily(nextOfKin.Familyname);

//            ContactPoint contact = new ContactPoint();

//            if (!string.IsNullOrEmpty(nextOfKin.Personalcontactinfo))
//            {
//                contact.Value = nextOfKin.Personalcontactinfo;
//                contact.System = ContactPoint.ContactPointSystem.Phone;
//                contact.Use = ContactPoint.ContactPointUse.Home;
//            }

//            if (string.IsNullOrEmpty(nextOfKin.Personalcontactinfo) && !string.IsNullOrEmpty(nextOfKin.Businesscontactinfo))
//            {
//                contact.Value = nextOfKin.Businesscontactinfo;
//                contact.System = ContactPoint.ContactPointSystem.Phone;
//                contact.Use = ContactPoint.ContactPointUse.Work;
//            }

//            contactComponent.Telecom.Add(contact);

//            return contactComponent;
//        }

//        public static List<ContactComponent> GetContactInfosForKin(this IEnumerable<entitystorematerialised_CoreNextofkin1> nextOfKins)
//        {
//            var kins = new List<ContactComponent>();

//            if (!nextOfKins.IsCollectionValid()) return kins;

//            nextOfKins.Each(nkc =>
//            {
//                var kin = nkc.GetContactInfoForKin();

//                if (kin != null)
//                    kins.Add(kin);
//            });

//            return kins;
//        }
//    }
//}
