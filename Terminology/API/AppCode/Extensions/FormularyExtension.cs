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


﻿using AutoMapper;
using Interneuron.Common.Extensions;
using Interneuron.FDBAPI.Client.DataModels;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.Model.DomainModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Extensions
{
    public static class FormularyExtension
    {
        public static FormularyHeader CloneFormulary(this FormularyHeader existingRecord, IMapper mapper, string rootEntityIdentifier = null)
        {
            var newFormularyHeader = mapper.Map<FormularyHeader>(existingRecord);

            newFormularyHeader.RowId = Guid.NewGuid().ToString();

            if (!rootEntityIdentifier.IsEmpty())
                newFormularyHeader.FormularyVersionId = rootEntityIdentifier;

            if (existingRecord.FormularyDetail.IsCollectionValid())
            {
                newFormularyHeader.FormularyDetail = mapper.Map<ICollection<FormularyDetail>>(existingRecord.FormularyDetail);
                newFormularyHeader.FormularyDetail.First().FormularyVersionId = newFormularyHeader.FormularyVersionId;
                newFormularyHeader.FormularyDetail.First().RowId = Guid.NewGuid().ToString();
            }

            if (existingRecord.FormularyAdditionalCode.IsCollectionValid())
            {
                newFormularyHeader.FormularyAdditionalCode = mapper.Map<ICollection<FormularyAdditionalCode>>(existingRecord.FormularyAdditionalCode);
                newFormularyHeader.FormularyAdditionalCode.Each(ac =>
                {
                    ac.FormularyVersionId = newFormularyHeader.FormularyVersionId;
                    ac.RowId = Guid.NewGuid().ToString();
                });
            }

            //if (existingRecord.FormularyIndication.IsCollectionValid())
            //{
            //    newFormularyHeader.FormularyIndication = mapper.Map<ICollection<FormularyIndication>>(existingRecord.FormularyIndication);
            //    newFormularyHeader.FormularyIndication.Each(rec =>
            //    {
            //        rec.FormularyVersionId = newFormularyHeader.FormularyVersionId;
            //        rec.RowId = Guid.NewGuid().ToString();
            //    });
            //}

            if (existingRecord.FormularyIngredient.IsCollectionValid())
            {
                newFormularyHeader.FormularyIngredient = mapper.Map<ICollection<FormularyIngredient>>(existingRecord.FormularyIngredient);
                newFormularyHeader.FormularyIngredient.Each(rec =>
                {
                    rec.FormularyVersionId = newFormularyHeader.FormularyVersionId;
                    rec.RowId = Guid.NewGuid().ToString();
                });
            }

            if (existingRecord.FormularyRouteDetail.IsCollectionValid())
            {
                newFormularyHeader.FormularyRouteDetail = mapper.Map<ICollection<FormularyRouteDetail>>(existingRecord.FormularyRouteDetail);
                newFormularyHeader.FormularyRouteDetail.Each(rec =>
                {
                    rec.FormularyVersionId = newFormularyHeader.FormularyVersionId;
                    rec.RowId = Guid.NewGuid().ToString();
                });
            }

            if (existingRecord.FormularyExcipient.IsCollectionValid())
            {
                newFormularyHeader.FormularyExcipient = mapper.Map<ICollection<FormularyExcipient>>(existingRecord.FormularyExcipient);
                newFormularyHeader.FormularyExcipient.Each(rec =>
                {
                    rec.FormularyVersionId = newFormularyHeader.FormularyVersionId;
                    rec.RowId = Guid.NewGuid().ToString();
                });
            }

            //if (existingRecord.FormularyOntologyForm.IsCollectionValid())
            //{
            //    newFormularyHeader.FormularyOntologyForm = mapper.Map<ICollection<FormularyOntologyForm>>(existingRecord.FormularyOntologyForm);
            //    newFormularyHeader.FormularyOntologyForm.Each(rec =>
            //    {
            //        rec.FormularyVersionId = newFormularyHeader.FormularyVersionId;
            //        rec.RowId = Guid.NewGuid().ToString();
            //    });
            //}

            return newFormularyHeader;
        }

        public static string SafeGetStringifiedCodeDescListForCode(this string code, Dictionary<string, List<string>> listData, string source = null)
        {
            if (code.IsEmpty() || !listData.IsCollectionValid()) return null;

            if (!listData.ContainsKey(code)) return null;

            var data = listData[code];

            if (data == null) return null;

            var list = data.Select(rec =>new FormularyLookupItemDTO { Cd = rec, Desc = rec, Source = source }).ToList();

            if (!list.IsCollectionValid()) return null;

            return JsonConvert.SerializeObject(list);
        }

        public static string SafeGetStringifiedCodeDescListForCode(this string code, Dictionary<string, List<FDBIdText>> listData, string source = null)
        {
            if (code.IsEmpty() || !listData.IsCollectionValid()) return null;

            if (!listData.ContainsKey(code)) return null;

            var data = listData[code];

            if (data == null) return null;

            var list = data.Select(rec => new FormularyLookupItemDTO { Cd = rec.Id, Desc = rec.Text, Source = source }).ToList();

            if (!list.IsCollectionValid()) return null;

            return JsonConvert.SerializeObject(list);
        }

    }
}
