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
﻿using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.Commands.EditMergeHandlers;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary.Requests;
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Commands
{
    public partial class FormularyCommand
    {
        public CreateEditFormularyDTO UpdateFormulary(CreateEditFormularyRequest request)
        {
            var response = new CreateEditFormularyDTO
            {
                Status = new StatusDTO { StatusCode = TerminologyConstants.STATUS_SUCCESS, StatusMessage = "", ErrorMessages = new List<string>() },
                Data = new List<FormularyDTO>()
            };

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var uniqueIds = request.RequestsData.Select(req => req.FormularyVersionId).Distinct().ToList();

            //Considering non-deleted records only, for this comparision
            var existingFormulariesFromDB = formularyRepo.GetFormularyListForIds(uniqueIds, true).ToList();

            var hasValidRecords = ValidateAllRecords(existingFormulariesFromDB, request.RequestsData, response);

            if (!hasValidRecords)
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                return response;
            }

            var uniqueCodes = existingFormulariesFromDB.Select(req => req.Code).Distinct().ToList();

            //This is to get all versions of records
            var existingFormulariesByCodes = formularyRepo.GetLatestFormulariesByCodes(uniqueCodes.ToArray(), true).ToList();

            var toBeSavedFormularies = new List<FormularyHeader>();

            var newRecordsPersisted = new List<FormularyHeader>();

            uniqueIds.Each(recId =>
            {
                var updatedRecord = UpdateRecord(recId, request, existingFormulariesFromDB, formularyRepo, existingFormulariesByCodes);

                newRecordsPersisted.Add(updatedRecord);
            });

            formularyRepo.SaveChanges();

            if (newRecordsPersisted.IsCollectionValid())
            {
                newRecordsPersisted.Each(rec =>
                {
                    RePopulateDTOPostSave(rec, response);
                });
            }

            return response;
        }

        private FormularyHeader UpdateRecord(string recId, CreateEditFormularyRequest request, List<FormularyHeader> existingFormulariesFromDB, IFormularyRepository<FormularyHeader> formularyRepo, List<FormularyHeader> existingFormulariesByCodes)
        {
            var recFromRequest = request.RequestsData.Single(rec => rec.FormularyVersionId == recId);

            var existingRecFromDb = existingFormulariesFromDB.Single(rec => rec.FormularyVersionId == recId);

            var newRecordToUpdate = MergeRecordFromDbWithRequestData(existingRecFromDb, recFromRequest);

            //Commented below code to avoid record going into deleted status
            //if (recFromRequest.RecStatusCode == TerminologyConstants.RECORDSTATUS_APPROVED)
            //{
            //    //Check if there are any other latest records with same code apart from the current record
            //    var duplicateFormulariesWithSameCodeFromDb = existingFormulariesByCodes.Where(dup => dup.Code == recFromRequest.Code && dup.FormularyVersionId != recId && dup.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED && dup.IsLatest == true);

            //    //Mark those other records as deleted
            //    if (duplicateFormulariesWithSameCodeFromDb.IsCollectionValid())
            //    {
            //        DeleteRecordWithSimilarCodeForEdit(duplicateFormulariesWithSameCodeFromDb, formularyRepo);
            //    }
            //}

            formularyRepo.Add(newRecordToUpdate);

            existingRecFromDb.IsLatest = false;

            formularyRepo.Update(existingRecFromDb);

            return newRecordToUpdate;
        }

        private FormularyHeader MergeRecordFromDbWithRequestData(FormularyHeader recToUpdate, FormularyDTO recFromRequest)
        {
            if (string.Compare(recToUpdate.ProductType, "vtm", true) == 0)
                return new VTMEditMergeHandler(_mapper).Merge(recToUpdate, recFromRequest);
            if (string.Compare(recToUpdate.ProductType, "vmp", true) == 0)
                return new VMPEditMergeHandler(_mapper).Merge(recToUpdate, recFromRequest);
            if (string.Compare(recToUpdate.ProductType, "amp", true) == 0)
                return new AMPEditMergeHandler(_mapper).Merge(recToUpdate, recFromRequest);

            return null;
        }

        

        private bool DeleteRecordWithSimilarCodeForEdit(IEnumerable<FormularyHeader> duplicateFormulariesWithSameCodeFromDb, IFormularyRepository<FormularyHeader> formularyRepo)
        {
            //Update the old records with IsLatest = false
            //Add new record with Delete status

            if (!duplicateFormulariesWithSameCodeFromDb.IsCollectionValid()) return false;

            duplicateFormulariesWithSameCodeFromDb.Each(dup =>
            {
                if (dup != null)
                {
                    var rootEntityIdentifier = Guid.NewGuid().ToString();

                    var dupAsNew = dup.CloneFormulary(_mapper, rootEntityIdentifier);

                    dupAsNew.RecStatusCode = TerminologyConstants.RECORDSTATUS_DELETED;
                    dupAsNew.RecStatuschangeDate = DateTime.UtcNow;
                    dupAsNew.FormularyVersionId = rootEntityIdentifier;
                    dupAsNew.IsLatest = true;
                    dupAsNew.VersionId = dup.VersionId + 1;

                    formularyRepo.Add(dupAsNew);

                    dup.IsLatest = false;
                    formularyRepo.Update(dup);
                }
            });

            return true;
        }

        

        private bool ValidateAllRecords(List<FormularyHeader> existingFormulariesFromDB, List<FormularyDTO> requestDTOs, CreateEditFormularyDTO response)
        {
            if (!existingFormulariesFromDB.IsCollectionValid())
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                response.Status.ErrorMessages.Add(NO_MATCHING_RECORDS_MSG);
                return false;
            }

            var hasAnyInvalidRecs = false;

            var uniqueCodes = existingFormulariesFromDB.Select(rec => rec.Code).ToList();

            var uniqueFormularyIds = existingFormulariesFromDB.Select(rec => rec.FormularyId).ToList();

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            //This will get the other records with same code that are in non-deleted or non-archived status
            var otherFormulariesForCodesFromDb = formularyRepo.ItemsAsReadOnly.Where(rec => rec.IsLatest == true && uniqueCodes.Contains(rec.Code)
            && !uniqueFormularyIds.Contains(rec.FormularyId) && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED).ToList();

            requestDTOs.Each(recDTO =>
            {
                //Is this record exists and is a latest record in the system - It might have updated already in the system - Do not update then
                if (!existingFormulariesFromDB.Any(rec => rec.FormularyVersionId == recDTO.FormularyVersionId && rec.IsLatest.GetValueOrDefault() == true))
                {
                    hasAnyInvalidRecs = true;
                    response.Status.ErrorMessages.Add("This record does not exist or is not latest in the system: Id: {0}".ToFormat(recDTO.FormularyVersionId));
                }

                //If any other record with same code exists and with same record status (other than Deleted or Archived), then it cannot be updated
                if (otherFormulariesForCodesFromDb.IsCollectionValid() && otherFormulariesForCodesFromDb.Any(rec => rec.RecStatusCode == recDTO.RecStatusCode))
                {
                    hasAnyInvalidRecs = true;
                    response.Status.ErrorMessages.Add("The record with the same code and same status already exists the system. Please archive the delete the other record and re-try. Id: {0}".ToFormat(recDTO.FormularyVersionId));
                }
            });

            if (hasAnyInvalidRecs) return false;
            return true;
        }
    }
}
