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
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Commands.ImportRules
{
    public class VMPImportRule : IImportRule
    {
        private DMDDetailResultDTO _dMDDetailResultDTO;
        private FormularyHeader _formularyDAO;

        public VMPImportRule(DMDDetailResultDTO dMDDetailResultDTO, FormularyHeader formularyDAO)
        {
            _dMDDetailResultDTO = dMDDetailResultDTO;

            _formularyDAO = formularyDAO;
        }

        public void MutateByRules()
        {
            IsPrescribable();
            IsWitnessingRequired();
            IsModifiedReleaseOrGastroResistant();
        }

        private void IsModifiedReleaseOrGastroResistant()
        {
            if (_formularyDAO == null || !_formularyDAO.FormularyDetail.IsCollectionValid()) return;

            var detailDAO = _formularyDAO.FormularyDetail.First();
            if (_formularyDAO.Name.Contains("modified-release", StringComparison.OrdinalIgnoreCase))
            {
                detailDAO.IsModifiedRelease = true;
            }
            if (_formularyDAO.Name.Contains("gastro-resistant", StringComparison.OrdinalIgnoreCase))
            {
                detailDAO.IsGastroResistant = true;
            }
        }

        private void IsWitnessingRequired()
        {
            if (_formularyDAO == null || !_formularyDAO.FormularyDetail.IsCollectionValid() || _dMDDetailResultDTO == null) return;

            var detailDAO = _formularyDAO.FormularyDetail.First();

            //Rule defined by RNOH: https://interneuron.atlassian.net/browse/SYN-169
            //Similar to Rule 6: Controlled drugs
            detailDAO.WitnessingRequired = (_dMDDetailResultDTO.ControlDrugCategory == null || (_dMDDetailResultDTO.ControlDrugCategory != null && (!_dMDDetailResultDTO.ControlDrugCategory.Cd.HasValue || _dMDDetailResultDTO.ControlDrugCategory.Cd != 0))) ? "1" : "0";
        }

        private void IsPrescribable()
        {
            if (_formularyDAO == null || !_formularyDAO.FormularyDetail.IsCollectionValid()) return;

            var detail = _formularyDAO.FormularyDetail.First();

            //Rule:
            //Not prescribable as a VMP but AMP prescribable
            //Never Valid To Prescribe As A VMP
            //Not Recommended To Prescribe As A VMP

            if (detail.PrescribingStatusCd == "3" || detail.PrescribingStatusCd == "4" || detail.PrescribingStatusCd == "5")
            {
                detail.Prescribable = false;
                detail.PrescribableSource = TerminologyConstants.DMD_DATA_SRC;
            }
        }
    }
}
