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


﻿using SynapseStudioWeb.Models.MedicationMgmt;

namespace SynapseStudioWeb.Controllers.MedicationManagement.BulkEditMergeHandlers
{
    public class VMPBulkEditMergeHandler : IBulkEditMergeHandler
    {
        public void Merge(BulkFormularyEditModel editedData, BulkFormularyEditModel orginalObj, FormularyEditModel modelFromDB)
        {
            editedData.FormularyIdentificationCodes = modelFromDB.FormularyIdentificationCodes;

            editedData.Prescribable = editedData.NullablePrescribable ?? modelFromDB.Prescribable;

            editedData.WitnessingRequired = editedData.NullableWitnessingRequired ?? modelFromDB.WitnessingRequired;

            editedData.Status = editedData.Status ?? modelFromDB.Status;

            editedData.RecStatuschangeMsg = editedData.RecStatuschangeMsg ?? modelFromDB.RecStatuschangeMsg;
        }
    }
}
