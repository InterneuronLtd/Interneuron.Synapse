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


﻿using Interneuron.CareRecord.Infrastructure.Search;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Interneuron.CareRecord.Repository.Infrastructure;
using Interneuron.Common.Extensions;

namespace Interneuron.CareRecord.Repository.QueryBuilders
{
    public class PatientSearchQueryBuilder : QueryBuilder
    {
        public override List<dynamic> Execute(List<SynapseSearchTerm> synapseSearchTerms)
        {
            var baseQuery = (from person in this.dbContext.entitystorematerialised_CorePerson
                             join personId in this.dbContext.entitystorematerialised_CorePersonidentifier on
                             person.PersonId equals personId.PersonId
                             join personAddress in this.dbContext.entitystorematerialised_CorePersonaddress1 on
                             person.PersonId equals personAddress.PersonId into pa
                             from address in pa.DefaultIfEmpty()
                             join personContact in this.dbContext.entitystorematerialised_CorePersoncontactinfo1 on
                             person.PersonId equals personContact.PersonId into pc
                             from contact in pc.DefaultIfEmpty()
                             join personNextOfKin in this.dbContext.entitystorematerialised_CoreNextofkin1 on
                             person.PersonId equals personNextOfKin.PersonId into pn
                             from nextOfKin in pn.DefaultIfEmpty()
                             select new
                             {
                                 personData = person,
                                 personIdData = personId,
                                 personAddressData = address,
                                 personContactData = contact,
                                 personNextOfKinData = nextOfKin
                             });

            var searchOp = new GenericSearchOpProcessor();

            var withSearchClause = searchOp.Apply(baseQuery, synapseSearchTerms);

#if DEBUG 
            var stringQuery = withSearchClause.ToSql();
#endif
            var matPersons = withSearchClause
                .OrderByDescending((entity) => entity.personData.Createdtimestamp)
                .Select(entity => new
                {
                    person = entity.personData,
                    patientIdentifer = entity.personIdData,
                    personAddress = entity.personAddressData,
                    personContact = entity.personContactData,
                    personNextOfKin = entity.personNextOfKinData
                })
                .ToList();

            if (matPersons.IsCollectionValid())
            {
                var persons = matPersons.Select(m => m.person).ToList();
                var patientIdentifiers = matPersons.Select(m => m.patientIdentifer).ToList();
                var personAddresses = matPersons.Select(m => m.personAddress).ToList();
                var personContacts = matPersons.Select(m => m.personContact).ToList();
                var personNextOfKins = matPersons.Select(m => m.personNextOfKin).ToList();

                return new List<dynamic> { persons, patientIdentifiers, personAddresses, personContacts, personNextOfKins };
            }

            return null;
        }
    }
}
