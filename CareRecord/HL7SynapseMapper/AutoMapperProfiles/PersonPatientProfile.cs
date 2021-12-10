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


using AutoMapper;
using Hl7.Fhir.Model;
using Interneuron.CareRecord.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interneuron.Common.Extensions;
using static Hl7.Fhir.Model.Patient;

namespace Interneuron.CareRecord.HL7SynapseService.AutoMapMapperProfiles
{
    public class PersonPatientProfile : Profile
    {
        public PersonPatientProfile()
        {
            CreateMap<entitystorematerialised_CorePerson, Patient>()
                .ForMember(dest => dest.Meta, opt => opt.MapFrom(src => GetMeta(src)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.Dateofbirth.ToString()))
                .ForMember(dest => dest.MaritalStatus, opt => opt.MapFrom(src => new CodeableConcept { Text = src.Maritalstatustext }))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => new List<HumanName>
                {
                    new HumanName().WithGiven(src.Firstname).AndFamily(src.Familyname)
                }))
                .ForMember(dest => dest.Gender, opt =>
                {
                    opt.PreCondition(src => !src.Gendertext.IsEmpty());
                    opt.MapFrom(GenderMapper);
                });
            CreateMap<entitystorematerialised_CorePersonidentifier, Patient>()
                .ForMember(dest => dest.Identifier, opt => opt.MapFrom<PersonIdentifierResolver>());

            CreateMap<entitystorematerialised_CorePersonaddress1, Patient>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom<PersonAddressResolver>());

            CreateMap<entitystorematerialised_CorePersoncontactinfo1, Patient>()
                .ForMember(dest => dest.Telecom, opt => opt.MapFrom<PersonTelecomResolver>());

            CreateMap<entitystorematerialised_CoreNextofkin1, Patient>()
                .ForMember(dest => dest.Contact, opt => opt.MapFrom<PersonKinContactResolver>());

            //CreateMap<entitystorematerialised_CorePersoncontactinfo1, ContactPoint>()
            //    .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Contactdetails))
            //    .ForMember(dest => dest, opt => opt.MapFrom(ContactSystemMapper));
        }

        private Meta GetMeta(entitystorematerialised_CorePerson src)
        {
            if (src.IsNull() || !src.Createdtimestamp.HasValue) return null;

            return new Meta
            {
                VersionId = $"{src.Sequenceid}",
                LastUpdated = src.Createdtimestamp
            };
        }
        private AdministrativeGender GenderMapper(entitystorematerialised_CorePerson corePerson, Patient patient)
        {
            switch (corePerson.Gendertext.ToLower())
            {
                case "male":
                    return AdministrativeGender.Male;
                case "female":
                    return AdministrativeGender.Female;
                case "m":
                    return AdministrativeGender.Male;
                case "f":
                    return AdministrativeGender.Female;
                default:
                    return AdministrativeGender.Unknown;
            }
        }
    }

    internal class PersonKinContactResolver : IValueResolver<entitystorematerialised_CoreNextofkin1, Patient, List<ContactComponent>>
    {
        public List<ContactComponent> Resolve(entitystorematerialised_CoreNextofkin1 nextOfKins, Patient destination, List<ContactComponent> destMember, ResolutionContext context)
        {
            var kins = new List<ContactComponent>();

            if (destination.Contact.IsCollectionValid())
            {
                destination.Contact.Each(contact => kins.Add(contact));
            }

            if (nextOfKins.IsNull()) return kins;

            var kin = GetContactInfoForKin(nextOfKins);

            if (kin != null)
                kins.Add(kin);

            return kins;
        }

        private ContactComponent GetContactInfoForKin(entitystorematerialised_CoreNextofkin1 nextOfKin)
        {
            if (nextOfKin.IsNull()) return null;

            ContactComponent contactComponent = new ContactComponent
            {
                Address = new Address()
            };

            string addresssLine = string.Join(nextOfKin.Addressstreet, ", ", nextOfKin.Addressstreet2);

            contactComponent.Address.Line = contactComponent.Address.Line.Append(addresssLine);
            contactComponent.Address.City = nextOfKin.Addresscity;
            contactComponent.Address.State = nextOfKin.Addresscountystateprovince;
            contactComponent.Address.Country = nextOfKin.Addresscountry;
            contactComponent.Address.PostalCode = nextOfKin.Addresspostalcode;
            contactComponent.Address.Type = Address.AddressType.Both;

            List<CodeableConcept> relationships = new List<CodeableConcept>();

            CodeableConcept relationship = new CodeableConcept
            {
                Text = nextOfKin.Relationship
            };

            relationships.Add(relationship);

            contactComponent.Relationship = relationships;

            HumanName nextOfKinName = new HumanName();

            contactComponent.Name = nextOfKinName.WithGiven(nextOfKin.Givenname).AndFamily(nextOfKin.Familyname);

            ContactPoint contact = new ContactPoint();

            if (!string.IsNullOrEmpty(nextOfKin.Personalcontactinfo))
            {
                contact.Value = nextOfKin.Personalcontactinfo;
                contact.System = ContactPoint.ContactPointSystem.Phone;
                contact.Use = ContactPoint.ContactPointUse.Home;
            }

            if (string.IsNullOrEmpty(nextOfKin.Personalcontactinfo) && !string.IsNullOrEmpty(nextOfKin.Businesscontactinfo))
            {
                contact.Value = nextOfKin.Businesscontactinfo;
                contact.System = ContactPoint.ContactPointSystem.Phone;
                contact.Use = ContactPoint.ContactPointUse.Work;
            }

            contactComponent.Telecom.Add(contact);

            return contactComponent;
        }
    }

    internal class PersonTelecomResolver : IValueResolver<entitystorematerialised_CorePersoncontactinfo1, Patient, List<ContactPoint>>
    {
        public List<ContactPoint> Resolve(entitystorematerialised_CorePersoncontactinfo1 personContact, Patient destination, List<ContactPoint> destMember, ResolutionContext context)
        {
            var contactPoints = new List<ContactPoint>();

            if (destination.Telecom.IsCollectionValid())
            {
                destination.Telecom.Each(tel => contactPoints.Add(tel));
            }

            if (personContact.IsNull()) return contactPoints;

            var contactPoint = ContactSystemMapper(personContact);

            if (contactPoint != null)
                contactPoints.Add(contactPoint);

            return contactPoints;
        }

        private ContactPoint ContactSystemMapper(entitystorematerialised_CorePersoncontactinfo1 personContact)
        {
            if (personContact == null) return null;

            var contact = new ContactPoint();

            switch (personContact.Contacttypecode.ToLower())
            {
                case "email":
                    contact.System = ContactPoint.ContactPointSystem.Email;
                    break;
                case "home":
                    contact.System = ContactPoint.ContactPointSystem.Phone;
                    contact.Use = ContactPoint.ContactPointUse.Home;
                    break;
                case "personal":
                    contact.System = ContactPoint.ContactPointSystem.Phone;
                    contact.Use = ContactPoint.ContactPointUse.Mobile;
                    break;
                case "work":
                    contact.System = ContactPoint.ContactPointSystem.Other;
                    contact.Use = ContactPoint.ContactPointUse.Work;
                    break;
                default:
                    contact.System = ContactPoint.ContactPointSystem.Other;
                    break;
            }

            return contact;
        }

    }

    internal class PersonAddressResolver : IValueResolver<entitystorematerialised_CorePersonaddress1, Patient, List<Address>>
    {
        public List<Address> Resolve(entitystorematerialised_CorePersonaddress1 personAddresses, Patient destination, List<Address> destMember, ResolutionContext context)
        {
            var addresses = new List<Address>();

            if (destination.Address.IsCollectionValid())
            {
                destination.Address.Each(addr => addresses.Add(addr));
            }

            if (personAddresses.IsNull()) return addresses;

            var addrPerson = GetAddressForPerson(personAddresses);

            if (addrPerson != null)
                addresses.Add(addrPerson);

            return addresses;
        }

        private Address GetAddressForPerson(entitystorematerialised_CorePersonaddress1 personAddress)
        {
            if (personAddress.IsNull()) return null;

            Address address = new Address();
            address.Line = address.Line.Append(personAddress.Line1);
            address.Line = address.Line.Append(", ");
            address.Line = address.Line.Append(personAddress.Line2);
            address.Line = address.Line.Append(", ");
            address.Line = address.Line.Append(personAddress.Line3);
            address.City = personAddress.City;
            address.State = personAddress.Countystateprovince;
            address.Country = personAddress.Country;
            address.PostalCode = personAddress.Postcodezip;
            address.Type = Address.AddressType.Both;

            return address;
        }
    }

    internal class PersonIdentifierResolver : IValueResolver<entitystorematerialised_CorePersonidentifier, Patient, List<Identifier>>
    {
        public List<Identifier> Resolve(entitystorematerialised_CorePersonidentifier personIdentifier, Patient destination, List<Identifier> destMember, ResolutionContext context)
        {
            var identifiers = new List<Identifier>();

            if (destination.Identifier.IsCollectionValid())
            {
                destination.Identifier.Each(iden => identifiers.Add(iden));
            }

            if (personIdentifier.IsNull()) return identifiers;

            var identifier = GetIdentifierForPeron(personIdentifier);

            if (identifier != null)
                identifiers.Add(identifier);

            return identifiers;
        }

        private Identifier GetIdentifierForPeron(entitystorematerialised_CorePersonidentifier personIdentifier)
        {
            if (personIdentifier.IsNull()) return null;

            return new Identifier
            {
                Value = personIdentifier.Idnumber,
                System = personIdentifier.Idtypecode
            };
        }
    }
}
