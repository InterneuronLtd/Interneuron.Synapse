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


﻿using Interneuron.Common.Extensions;
using System.Collections.Generic;
using Hl7.Fhir.Model;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.CareRecord.HL7SynapseService.Models;

namespace Interneuron.CareRecord.API.AppCode.Extensions
{
    public static class HL7BundleExtension
    {
        public static Bundle Append(this Bundle bundle, IEnumerable<ResourceData> resourcesData)
        {
            if (!resourcesData.IsCollectionValid() && Bundle.BundleType.Searchset == bundle.Type)
            {
                Bundle.EntryComponent bundleEntry = new Bundle.EntryComponent();
                
                OperationOutcome outcome = new OperationOutcome();
                CodeableConcept codeableConcept = new CodeableConcept();
                codeableConcept.Text = "There is no matching record found for the given criteria";
                outcome.AddWarning(string.Format("Not Found"), OperationOutcome.IssueType.NotFound, codeableConcept);
                bundleEntry.Resource = outcome;
                bundleEntry.Resource.Id = "warning";
                bundleEntry.Search = new Bundle.SearchComponent() {
                    Mode = Bundle.SearchEntryMode.Outcome
                };
                bundle.Entry.Add(bundleEntry);
                return bundle;
            }

            foreach (ResourceData resourceData in resourcesData)
            {
                bundle.Append(resourceData);
            }

            return bundle;
        }
        public static Bundle Append(this Bundle bundle, ResourceData resourceData, FhirResponse response = null)
        {
            Bundle.EntryComponent bundleEntry;
            switch (bundle.Type)
            {
                //case Bundle.BundleType.History: bundleEntry = resourceData.ToTransactionEntry(); break;
                case Bundle.BundleType.Searchset: bundleEntry = resourceData.TranslateToSparseEntry(); break;
                case Bundle.BundleType.BatchResponse: bundleEntry = resourceData.TranslateToSparseEntry(response); break;
                case Bundle.BundleType.TransactionResponse: bundleEntry = resourceData.TranslateToSparseEntry(response); break;
                default: bundleEntry = resourceData.TranslateToSparseEntry(); break;
            }
            bundle.Entry.Add(bundleEntry);

            return bundle;
        }

        public static Bundle.EntryComponent TranslateToSparseEntry(this ResourceData resourceData, FhirResponse response = null)
        {
            var bundleEntry = new Bundle.EntryComponent();

            if (response != null)
            {
                bundleEntry.Response = new Bundle.ResponseComponent()
                {
                    Status = string.Format("{0} {1}", (int)response.StatusCode, response.StatusCode),

                    Location = response.Key != null ? response.Key.ToString() : null,

                    Etag = response.Key != null ? ETagExtension.Create(response.Key.VersionId).ToString() : null,

                    LastModified =
                        (resourceData != null && resourceData.Resource != null && resourceData.Resource.Meta != null)
                            ? resourceData.Resource.Meta.LastUpdated
                            : null
                };
            }

            SetBundleEntryResource(resourceData, bundleEntry);

            return bundleEntry;
        }

        private static void SetBundleEntryResource(this ResourceData resourceData, Bundle.EntryComponent bundleEntry)
        {
            if (resourceData.Resource.IsNotNull())
            {
                bundleEntry.Resource = resourceData.Resource;
                //resourceData.Key.ApplyTo(bundleEntry.Resource);
                //bundleEntry.FullUrl = resourceData.fHIRParam.ToUriString();
            }
        }
    }
}
