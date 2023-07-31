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
using Interneuron.CareRecord.Infrastructure.Search;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.Common.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Interneuron.CareRecord.HL7SynapseHandler.Service.Implementations.Queries.SearchTermsBuilder
{
    public class ObservationResultSearchTermsBuilder : SearchTermsBuilder
    {
        private string _defaultHospitalRefNo;
        private IServiceProvider _provider;

        public ObservationResultSearchTermsBuilder(IServiceProvider _provider)
        {
            this._provider = _provider;
            this._defaultHospitalRefNo = GetDefaultHospitalRefNo();
        }

        public override Dictionary<string, Func<string, List<SynapseSearchTerm>>> GetSearchTerms()
        {
            return new Dictionary<string, Func<string, List<SynapseSearchTerm>>>()
            {
                    { "Patient.Identifier", PatientIdSearchCriteria },
                    { "Subject.Patient.Identifier", PatientIdSearchCriteria },
                    { "_id", ResultIdSearchCriteria },
                    { "_lastUpdated", (paramVal)=>
                        {
                            return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"resultData.{nameof(entitystorematerialised_CoreResult.Createddate)}", DateTimeSearchExpressionProvider.GreaterThanEqualToOperator, paramVal.Substring(2,paramVal.Length-2), new DateTimeSearchExpressionProvider()) };
                        }
                    },
                    { "Patient.DateOfBirth", PatientDOBCriteria
                    },
                    {"FamilyName", PatientSearchCriteria }
            };

        }

        private string GetDefaultHospitalRefNo()
        {
            IConfiguration configuration = this._provider.GetService(typeof(IConfiguration)) as IConfiguration;

            IConfigurationSection careRecordConfig = configuration.GetSection("CareRecordConfig");

            return careRecordConfig.GetValue<string>("HospitalNumberReference");
        }

        private List<SynapseSearchTerm> PatientIdSearchCriteria(string paramVal)
        {
            var searchTerms = new List<SynapseSearchTerm>();

            if (paramVal.Contains("|"))
            {
                var paramVals = paramVal.Split("|");

                searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idtypecode)}", DefaultSearchExpressionProvider.EqualsOperator, paramVals[0], new DefaultSearchExpressionProvider()));

                searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idnumber)}", DefaultSearchExpressionProvider.EqualsOperator, paramVals[1], new DefaultSearchExpressionProvider()));

                return searchTerms;
            }
            searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idtypecode)}", DefaultSearchExpressionProvider.EqualsOperator, this._defaultHospitalRefNo, new DefaultSearchExpressionProvider()));

            searchTerms.Add(new SynapseSearchTerm($"personIdData.{nameof(entitystorematerialised_CorePersonidentifier.Idnumber)}", DefaultSearchExpressionProvider.EqualsOperator, paramVal, new DefaultSearchExpressionProvider()));

            return searchTerms;
        }

        private List<SynapseSearchTerm> PatientSearchCriteria(string paramVal)
        {
            var searchTerms = new List<SynapseSearchTerm>
            {
                new SynapseSearchTerm($"personData.{nameof(entitystorematerialised_CorePerson.Familyname)}", StringSearchExpressionProvider.EqualsOperator, paramVal, new StringSearchExpressionProvider())
            };

            return searchTerms;
        }

        private List<SynapseSearchTerm> PatientDOBCriteria(string paramVal)
        {
            return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"personData.{nameof(entitystorematerialised_CorePerson.Dateofbirth)}", DateTimeSearchExpressionProvider.EqualsOperator, paramVal, new DateTimeSearchExpressionProvider())
            };
        }

        private List<SynapseSearchTerm> ResultIdSearchCriteria(string paramVal)
        {
            return new List<SynapseSearchTerm>() { new SynapseSearchTerm($"resultData.{nameof(entitystorematerialised_CoreResult.ResultId)}", StringSearchExpressionProvider.EqualsOperator, paramVal, new StringSearchExpressionProvider())
            };
        }
    }
}
