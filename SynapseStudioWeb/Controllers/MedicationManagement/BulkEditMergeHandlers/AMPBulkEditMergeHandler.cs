//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

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
using SynapseStudioWeb.Models.MedicationMgmt;
using SynapseStudioWeb.Models.MedicinalMgmt;
using System.Collections.Generic;

namespace SynapseStudioWeb.Controllers.MedicationManagement.BulkEditMergeHandlers
{
    public class AMPBulkEditMergeHandler : IBulkEditMergeHandler
    {
        public void Merge(BulkFormularyEditModel editedData, BulkFormularyEditModel orginalObj, FormularyEditModel modelFromDB)
        {
            editedData.FormularyIdentificationCodes = modelFromDB.FormularyIdentificationCodes;

            editedData.FormularyClassificationCodes = modelFromDB.FormularyClassificationCodes;

            if (!editedData.UnlicensedRoute.IsCollectionValid())
            {
                editedData.UnlicensedRoute = modelFromDB.UnlicensedRoute;
            }
            if (!editedData.UnlicensedUse.IsCollectionValid())
            {
                editedData.UnlicensedUse = modelFromDB.UnlicensedUse;
            }
            if (!editedData.LocalLicensedRoute.IsCollectionValid())
            {
                editedData.LocalLicensedRoute = modelFromDB.LocalLicensedRoute;
            }
            if (!editedData.LocalUnlicensedRoute.IsCollectionValid())
            {
                editedData.LocalUnlicensedRoute = modelFromDB.LocalUnlicensedRoute;
            }
            if (!editedData.LocalLicensedUse.IsCollectionValid())
            {
                editedData.LocalLicensedUse = modelFromDB.LocalLicensedUse;
            }
            if (!editedData.LocalUnlicensedUse.IsCollectionValid())
            {
                editedData.LocalUnlicensedUse = modelFromDB.LocalUnlicensedUse;
            }
            if (editedData.RoundingFactorCd.IsEmpty())
            {
                editedData.RoundingFactorCd = modelFromDB.RoundingFactorCd;
            }
            if (!editedData.CustomWarnings.IsCollectionValid())
            {
                editedData.CustomWarnings = modelFromDB.CustomWarnings;
            }
            if (!editedData.Reminders.IsCollectionValid())
            {
                editedData.Reminders = modelFromDB.Reminders;
            }
            if (!editedData.Endorsements.IsCollectionValid())
            {
                editedData.Endorsements = modelFromDB.Endorsements;
            }
            if (editedData.MedusaPreparationInstructionsEditable.IsEmpty())
            {
                if (modelFromDB.MedusaPreparationInstructions.IsCollectionValid())
                {
                    editedData.MedusaPreparationInstructionsEditable = modelFromDB.MedusaPreparationInstructions[0].Id;
                }
            }
            if (!editedData.SideEffects.IsCollectionValid())
            {
                editedData.SideEffects = modelFromDB.SideEffects;
            }
            if (!editedData.Cautions.IsCollectionValid())
            {
                editedData.Cautions = modelFromDB.Cautions;
            }
            editedData.ClinicalTrialMedication = editedData.NullableClinicalTrialMedication ?? modelFromDB.ClinicalTrialMedication;

            editedData.CriticalDrug = editedData.NullableCriticalDrug ?? modelFromDB.CriticalDrug;

            editedData.IVToOral = editedData.NullableIVToOral ?? modelFromDB.IVToOral;

            editedData.ExpensiveMedication = editedData.NullableExpensiveMedication ?? modelFromDB.ExpensiveMedication;

            editedData.HighAlertMedication = editedData.NullableHighAlertMedication ?? modelFromDB.HighAlertMedication;

            editedData.NotForPrn = editedData.NullableNotForPrn ?? modelFromDB.NotForPrn;

            editedData.Prescribable = editedData.NullablePrescribable ?? modelFromDB.Prescribable;

            editedData.OutpatientMedication = editedData.NullableOutpatientMedication ?? modelFromDB.OutpatientMedication;

            editedData.TitrationTypesEditableId = editedData.TitrationTypesEditableId ?? modelFromDB.TitrationTypesEditableId;

            if (!editedData.TitrationTypes.IsCollectionValid())
            {
                editedData.TitrationTypes = modelFromDB.TitrationTypes;
            }
            if (orginalObj.RnohFormularyStatuscd == editedData.RnohFormularyStatuscd)
            {
                editedData.RnohFormularyStatuscd = modelFromDB.RnohFormularyStatuscd;
            }

            editedData.IgnoreDuplicateWarnings = editedData.NullableIgnoreDuplicateWarnings ?? modelFromDB.IgnoreDuplicateWarnings;

            editedData.WitnessingRequired = editedData.NullableWitnessingRequired ?? modelFromDB.WitnessingRequired;

            editedData.IsBloodProduct = editedData.NullableIsBloodProduct ?? modelFromDB.IsBloodProduct;

            editedData.IsDiluent = editedData.NullableIsDiluent ?? modelFromDB.IsDiluent;

            editedData.IsCustomControlledDrug = editedData.NullableIsCustomControlledDrug ?? modelFromDB.IsCustomControlledDrug;

            editedData.IsPrescriptionPrintingRequired = editedData.NullableIsPrescriptionPrintingRequired ?? modelFromDB.IsPrescriptionPrintingRequired;

            if (!editedData.Diluents.IsCollectionValid())
            {
                editedData.Diluents = modelFromDB.Diluents;
            }

            editedData.IsIndicationMandatory = editedData.NullableIsIndicationMandatory ?? modelFromDB.IsIndicationMandatory;

            editedData.IsGastroResistant = editedData.NullableIsGastroResistant ?? modelFromDB.IsGastroResistant;

            editedData.IsModifiedRelease = editedData.NullableIsModifiedRelease ?? modelFromDB.IsModifiedRelease;

            editedData.Status = editedData.Status ?? modelFromDB.Status;

            editedData.RecStatuschangeMsg = editedData.RecStatuschangeMsg ?? modelFromDB.RecStatuschangeMsg;

            if (editedData.DelLocalLicensedIndication)
            {
                editedData.LocalLicensedUse = new List<CodeNameSelectorModel>();
            }

            if (editedData.DelLocalUnlicensedIndication)
            {
                editedData.LocalUnlicensedUse = new List<CodeNameSelectorModel>();
            }

            if (editedData.DelLocalLicensedRoute)
            {
                editedData.LocalLicensedRoute = new List<CodeNameSelectorModel>();
            }

            if(editedData.DelLocalUnlicensedRoute)
            {
                editedData.LocalUnlicensedRoute = new List<CodeNameSelectorModel>();
            }

            if (editedData.DelMedusaPrepIns)
            {
                editedData.MedusaPreparationInstructionsEditable = string.Empty;
            }

            if (editedData.DelCompatibleDiluents)
            {
                editedData.Diluents = new List<CodeNameSelectorModel>();
            }

            if (editedData.DelCustomWarning)
            {
                editedData.CustomWarnings = new List<FormularyCustomWarningModel>();
            }

            if (editedData.DelReminder)
            {
                editedData.Reminders = new List<FormularyReminderModel>();
            }

            if (editedData.DelEndorsement)
            {
                editedData.Endorsements = new List<string>();
            }

            if (editedData.DelRoundingFactor)
            {
                editedData.RoundingFactorCd = null;
            }

            //if (editedData.DelFormularyStatus)
            //{
            //    editedData.RnohFormularyStatuscd = null;
            //}

            if (editedData.DelTitrationType)
            {
                editedData.TitrationTypesEditableId = null;
            }
        }
    }
}
