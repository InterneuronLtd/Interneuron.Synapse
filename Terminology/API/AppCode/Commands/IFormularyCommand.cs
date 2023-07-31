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
﻿using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary.Requests;
using Interneuron.Terminology.Infrastructure.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Commands
{
    public interface IFormularyCommands
    {
        Task<ImportFormularyResultsDTO> ImportByCodes(List<string> dmdCodes, string defaultFormularyStatusCode = TerminologyConstants.FORMULARYSTATUS_FORMULARY, string defaultRecordStatusCode = TerminologyConstants.RECORDSTATUS_DRAFT);

        UpdateFormularyRecordStatusDTO UpdateFormularyRecordStatus(UpdateFormularyRecordStatusRequest request);

        UpdateFormularyRecordStatusDTO BulkUpdateFormularyRecordStatus(UpdateFormularyRecordStatusRequest request);

        Task<CreateEditFormularyDTO> CreateFormulary(CreateEditFormularyRequest request);

        CreateEditFormularyDTO UpdateFormulary(CreateEditFormularyRequest request);

        Task<ImportFormularyResultsDTO> FileImport(CreateEditFormularyRequest request);

        void ChangeFDBDataSchema();//TBR

        Task<StatusDTO> ImportAllDMDCodes();

        Task InvokePostImportProcess();

        Task InvokePostImportProcessForCodes(List<string> codes);
    }
}
