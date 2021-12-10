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


﻿using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.Extensions
{
    public static class OperationOutcomeExtension
    {
        public static OperationOutcome Init(this OperationOutcome outcome)
        {
            if (outcome.Issue == null)
            {
                outcome.Issue = new List<OperationOutcome.IssueComponent>();
            }
            return outcome;
        }
        public static OperationOutcome AddError(this OperationOutcome outcome, string message)
        {
            return outcome.AddIssue(OperationOutcome.IssueSeverity.Error, message);
        }

        public static OperationOutcome AddWarning(this OperationOutcome outcome, string message, OperationOutcome.IssueType issueType=OperationOutcome.IssueType.NotFound, CodeableConcept codeableConcept=null)
        {
            return outcome.AddIssue(OperationOutcome.IssueSeverity.Warning, message, issueType, codeableConcept);
        }

        private static OperationOutcome AddIssue(this OperationOutcome outcome, OperationOutcome.IssueSeverity severity, string message)
        {
            if (outcome.Issue == null) outcome.Init();

            var item = new OperationOutcome.IssueComponent();
            item.Severity = severity;
            item.Diagnostics = message;
            outcome.Issue.Add(item);
            return outcome;
        }

        private static OperationOutcome AddIssue(this OperationOutcome outcome, OperationOutcome.IssueSeverity severity, string message, OperationOutcome.IssueType issueType=OperationOutcome.IssueType.Unknown)
        {
            if (outcome.Issue == null) outcome.Init();

            var item = new OperationOutcome.IssueComponent();
            item.Severity = severity;
            item.Diagnostics = message;
            item.Code = issueType;
            outcome.Issue.Add(item);
            return outcome;
        }

        private static OperationOutcome AddIssue(this OperationOutcome outcome, OperationOutcome.IssueSeverity severity, string message, OperationOutcome.IssueType issueType = OperationOutcome.IssueType.Unknown, CodeableConcept codeableConcept=null)
        {
            if (outcome.Issue == null) outcome.Init();

            var item = new OperationOutcome.IssueComponent();
            item.Severity = severity;
            item.Diagnostics = message;
            item.Code = issueType;
            item.Details = codeableConcept;
            outcome.Issue.Add(item);
            return outcome;
        }
    }
}
