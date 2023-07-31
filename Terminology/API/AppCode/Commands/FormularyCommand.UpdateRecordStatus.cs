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
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary;
using Interneuron.Terminology.API.AppCode.DTOs.Formulary.Requests;
using Interneuron.Terminology.API.AppCode.Extensions;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Commands
{
    public partial class FormularyCommand : IFormularyCommands
    {
        const string NO_MATCHING_RECORDS_MSG = "No matching records found in the system.\n";
        const string NO_MATCHING_RECORD_MSG = "No matching record for {0} found in the system.\n";
        const string ALREADY_UPDATED_RECORD_MSG = "This record {0} has already been updated in the system.";


        public UpdateFormularyRecordStatusDTO UpdateFormularyRecordStatus(UpdateFormularyRecordStatusRequest request)
        {
            var response = new UpdateFormularyRecordStatusDTO
            {
                Status = new StatusDTO { StatusCode = TerminologyConstants.STATUS_SUCCESS, StatusMessage = "", ErrorMessages = new List<string>() },
                Data = new List<FormularyDTO>()
            };

            if (request == null || !request.RequestData.IsCollectionValid())
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                response.Status.ErrorMessages.Add(INVALID_INPUT_MSG);

                return response;
            }

            var recordsToUpdate = request.RequestData;

            var requests = new List<UpdateFormularyRecordStatusRequestData>();

            recordsToUpdate.Each(r =>
            {
                if (r.FormularyVersionId.IsNotEmpty() && r.RecordStatusCode.IsNotEmpty())
                {
                    r.FormularyVersionId = r.FormularyVersionId.Trim();
                    r.RecordStatusCode = r.RecordStatusCode.Trim();
                    requests.Add(r);
                }
            });

            if (!recordsToUpdate.IsCollectionValid())
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                response.Status.ErrorMessages.Add(INVALID_INPUT_MSG);

                return response;
            }

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var uniqueFormularyVersionIds = recordsToUpdate.Select(r => r.FormularyVersionId).Distinct().ToList();

            var formulariesFromDbForIds = formularyRepo.GetFormularyListForIds(uniqueFormularyVersionIds, true).ToList();

            var hasValidRecs = ValidateRecords(formulariesFromDbForIds, recordsToUpdate, response);

            if (!hasValidRecs)
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                return response;
            }

            uniqueFormularyVersionIds.Each(recId =>
            {
                var recordToUpdate = recordsToUpdate.SingleOrDefault(rec => rec.FormularyVersionId == recId);
                UpdateRecordStatus(recordToUpdate, response, formulariesFromDbForIds, formularyRepo);

            });

            return response;
        }

        public UpdateFormularyRecordStatusDTO BulkUpdateFormularyRecordStatus(UpdateFormularyRecordStatusRequest request)
        {
            var response = new UpdateFormularyRecordStatusDTO
            {
                Status = new StatusDTO { StatusCode = TerminologyConstants.STATUS_SUCCESS, StatusMessage = "", ErrorMessages = new List<string>() },
                Data = new List<FormularyDTO>()
            };

            if (request == null || !request.RequestData.IsCollectionValid())
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                response.Status.ErrorMessages.Add(INVALID_INPUT_MSG);

                return response;
            }

            var recordsToUpdate = request.RequestData;

            var requests = new List<UpdateFormularyRecordStatusRequestData>();

            recordsToUpdate.Each(r =>
            {
                if (r.FormularyVersionId.IsNotEmpty() && r.RecordStatusCode.IsNotEmpty())
                {
                    r.FormularyVersionId = r.FormularyVersionId.Trim();
                    r.RecordStatusCode = r.RecordStatusCode.Trim();
                    requests.Add(r);
                }
            });

            if (!recordsToUpdate.IsCollectionValid())
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                response.Status.ErrorMessages.Add(INVALID_INPUT_MSG);

                return response;
            }

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            var uniqueFormularyVersionIds = recordsToUpdate.Select(r => r.FormularyVersionId).Distinct().ToList();

            var formulariesFromDbForIds = formularyRepo.GetFormularyListForIds(uniqueFormularyVersionIds, true).ToList();

            var hasValidRecs = ValidateRecords(formulariesFromDbForIds, recordsToUpdate, response);

            if (!hasValidRecs)
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                return response;
            }

            return response;
        }


        private bool ValidateRecords(List<FormularyHeader> formulariesFromDbForIds, List<UpdateFormularyRecordStatusRequestData> recordsToUpdate, UpdateFormularyRecordStatusDTO response)
        {
            if (!formulariesFromDbForIds.IsCollectionValid())
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                response.Status.ErrorMessages.Add(NO_MATCHING_RECORDS_MSG);
                return false;
            }

            //if (formulariesFromDbForIds.Any(rec => string.Compare(rec.ProductType, "amp", true) != 0))
            //{
            //    response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
            //    response.Status.ErrorMessages.Add("Only records at the AMP level can be edited.");
            //    return false;
            //}

            var hasInvalidRecs = false;

            var uniqueCodes = formulariesFromDbForIds.Select(rec => rec.Code).ToList();

            var uniqueFormularyIds = formulariesFromDbForIds.Select(rec => rec.FormularyId).ToList();

            var formularyRepo = this._provider.GetService(typeof(IFormularyRepository<FormularyHeader>)) as IFormularyRepository<FormularyHeader>;

            //This will get the other records with same code that are in non-deleted or non-archived status
            var otherFormulariesForCodesFromDb = formularyRepo.ItemsAsReadOnly.Where(rec => rec.IsLatest == true && uniqueCodes.Contains(rec.Code)
            && !uniqueFormularyIds.Contains(rec.FormularyId) && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED && rec.RecStatusCode != TerminologyConstants.RECORDSTATUS_DELETED).ToList();

            recordsToUpdate.Each(recToUpdate =>
            {
                var code = formularyRepo.ItemsAsReadOnly.Where(rec => rec.IsLatest == true && rec.FormularyVersionId == recToUpdate.FormularyVersionId).Select(rec => rec.Code).FirstOrDefault();
                var name = formularyRepo.ItemsAsReadOnly.Where(rec => rec.IsLatest == true && rec.FormularyVersionId == recToUpdate.FormularyVersionId).Select(rec => rec.Name).FirstOrDefault();

                //Is this record exist and is a latest record in the system - It might have updated already in the system - Do not update then
                if (!formulariesFromDbForIds.Any(rec => rec.FormularyVersionId == recToUpdate.FormularyVersionId && rec.IsLatest.GetValueOrDefault() == true))
                {
                    hasInvalidRecs = true;
                    response.Status.ErrorMessages.Add("This record does not exist or is not latest in the system: Id: {0}".ToFormat("(" + code + ")" + " " + name));
                }

                //If any other record with same code exists and with same record status (other than Deleted or Archived), then it cannot be updated
                if (otherFormulariesForCodesFromDb.IsCollectionValid() && otherFormulariesForCodesFromDb.Any(rec => rec.RecStatusCode == recToUpdate.RecordStatusCode && rec.Code == code))
                {
                    hasInvalidRecs = true;
                    response.Status.ErrorMessages.Add("The record with the same code and same status already exists in the system. Please archive the other record and re-try. Code: {0}".ToFormat("(" + code + ")" + " " + name));
                }

                if(formulariesFromDbForIds.Any(rec => rec.RecStatusCode == recToUpdate.RecordStatusCode && rec.FormularyVersionId == recToUpdate.FormularyVersionId && rec.IsLatest == true))
                {
                    hasInvalidRecs = true;
                    response.Status.ErrorMessages.Add("The record you are trying to update already has the same status. Please use different status and re-try. Code: {0}".ToFormat("(" + code + ")" + " " + name));
                }
            });


            if (hasInvalidRecs) return false;
            return true;
        }

        private void UpdateRecordStatus(UpdateFormularyRecordStatusRequestData recordToUpdate, UpdateFormularyRecordStatusDTO response, List<FormularyHeader> formulariesFromDb, IFormularyRepository<FormularyHeader> formularyRepo)
        {
            var isUpdatable = false;

            var matchingRecordInDb = formulariesFromDb.Where(rec => rec.FormularyVersionId == recordToUpdate.FormularyVersionId).FirstOrDefault();

            if (matchingRecordInDb == null)
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_BAD_REQUEST;
                response.Status.ErrorMessages.Add(NO_MATCHING_RECORD_MSG.ToFormat(recordToUpdate.FormularyVersionId));
                return;
            }

            //No need to specially handle for approved as there cannot  be records or codes in the same status
            //if (recordToUpdate.RecordStatusCode == TerminologyConstants.RECORDSTATUS_APPROVED)
            //{
            //    isUpdatable = HandleApprovedStatus(matchingRecordInDb, recordToUpdate, formularyRepo);
            //}
            //else
            //{
            //    isUpdatable = UpdateStatusForRecord(matchingRecordInDb, recordToUpdate, formularyRepo);
            //}

            isUpdatable = UpdateStatusForRecord(matchingRecordInDb, recordToUpdate, formularyRepo);

            if (!isUpdatable)
            {
                response.Status.StatusCode = TerminologyConstants.STATUS_FAIL;
                response.Status.ErrorMessages.Add(NO_MATCHING_RECORD_MSG.ToFormat(recordToUpdate.FormularyVersionId));
            }

            formularyRepo.SaveChanges();
        }

        //No need to specially handle for approved as there cannot  be records or codes in the same status
        //private bool HandleApprovedStatus(FormularyHeader matchingRecordInDb, UpdateFormularyRecordStatusRequestData recordToUpdate, IFormularyRepository<FormularyHeader> formularyRepo)
        //{
        //    var allFormulariesWithSameCodeFromDb = formularyRepo.GetLatestFormulariesByCodes(new string[] { matchingRecordInDb.Code }, true).ToList();//gets only the latest and also non-deleted

        //    if (!allFormulariesWithSameCodeFromDb.IsCollectionValid()) return false;

        //    //Any record other than the status change record and non-archived and non-deleted
        //    var duplicateFormulariesWithSameCodeFromDb = allFormulariesWithSameCodeFromDb.Where(dup => dup.FormularyVersionId != matchingRecordInDb.FormularyVersionId && dup.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED);

        //    if (duplicateFormulariesWithSameCodeFromDb.IsCollectionValid())
        //    {
        //        //For all these records - mark the status as deleted and update to db
        //        DeleteRecordWithSimilarCode(duplicateFormulariesWithSameCodeFromDb, formularyRepo);
        //    }

        //    return UpdateStatusForRecord(matchingRecordInDb, recordToUpdate, formularyRepo, true);


        //    ////Check whether this record is duplicate of any other record
        //    ////Then, delete the original record, else just update this record
        //    //if (matchingRecordInDb.IsDuplicate.HasValue && matchingRecordInDb.IsDuplicate.Value)
        //    //{
        //    //    var allFormulariesWithSameCodeFromDb = formularyRepo.GetLatestFormulariesByCodes(new string[] { matchingRecordInDb.Code }, true).ToList();//gets only the latest and also non-deleted

        //    //    if (!allFormulariesWithSameCodeFromDb.IsCollectionValid()) return false;



        //    //    //Any record other than the status change record and non-archived and non-deleted
        //    //    var duplicateFormulariesWithSameCodeFromDb = allFormulariesWithSameCodeFromDb.Where(dup => dup.FormularyVersionId != matchingRecordInDb.FormularyVersionId && dup.RecStatusCode != TerminologyConstants.RECORDSTATUS_ARCHIVED);

        //    //    if (duplicateFormulariesWithSameCodeFromDb.IsCollectionValid())
        //    //    {
        //    //        //For all these records - mark the status as deleted and update to db
        //    //        DeleteRecordWithSimilarCode(duplicateFormulariesWithSameCodeFromDb, formularyRepo);
        //    //    }

        //    //    return UpdateStatusForRecord(matchingRecordInDb, recordToUpdate, formularyRepo);
        //    //}

        //    //return UpdateStatusForRecord(matchingRecordInDb, recordToUpdate, formularyRepo);
        //}


        private bool UpdateStatusForRecord(FormularyHeader updatableFormulary, UpdateFormularyRecordStatusRequestData recordToUpdate, IFormularyRepository<FormularyHeader> formularyRepo, bool removeDuplicateFlag = false)
        {
            //Update the existing record with IsLatest = false and 
            //Create new record with new status

            if (updatableFormulary != null)
            {
                //Get Updatable record
                var existingRecord = formularyRepo.GetFormularyListForIds(new List<string> { updatableFormulary.FormularyVersionId }).FirstOrDefault();

                if (existingRecord == null) return false;

                var rootEntityIdentifier = Guid.NewGuid().ToString();

                var newFormularyHeader = existingRecord.CloneFormulary(_mapper, rootEntityIdentifier);

                newFormularyHeader.RecStatusCode = recordToUpdate.RecordStatusCode;
                newFormularyHeader.RecStatuschangeMsg = recordToUpdate.RecordStatusCodeChangeMsg;
                newFormularyHeader.IsLatest = true;
                newFormularyHeader.RecStatuschangeDate = DateTime.UtcNow;

                newFormularyHeader.FormularyVersionId = rootEntityIdentifier;
                newFormularyHeader.VersionId = existingRecord.VersionId + 1;

                if (removeDuplicateFlag)
                {
                    newFormularyHeader.IsDuplicate = false;
                    newFormularyHeader.DuplicateOfFormularyId = null;
                }

                formularyRepo.Add(newFormularyHeader);

                //Update existing
                existingRecord.IsLatest = false;
                formularyRepo.Update(existingRecord);

                //updatableFormulary.RecStatusCode = recordToUpdate.RecordStatusCode;
                //updatableFormulary.RecStatuschangeMsg = recordToUpdate.RecordStatusCodeChangeMsg;

                //updatableFormulary.RecStatuschangeDate = DateTime.UtcNow;

                //updatableFormulary.FormularyVersionId = Guid.NewGuid().ToString();
                //updatableFormulary.VersionId = updatableFormulary.VersionId+1;

                //var detail = updatableFormulary.FormularyDetail.First();
                //detail.FormularyVersionId = updatableFormulary.FormularyVersionId;
                //detail.RowId = null;

                //if (updatableFormulary.FormularyAdditionalCode.IsCollectionValid())
                //{
                //    updatableFormulary.FormularyAdditionalCode.Each(ac =>
                //    {
                //        ac.RowId = null;
                //        ac.FormularyVersionId = updatableFormulary.FormularyVersionId;
                //    });
                //}

                //if (updatableFormulary.FormularyIndication.IsCollectionValid())
                //{
                //    updatableFormulary.FormularyIndication.Each(ac =>
                //    {
                //        ac.RowId = null;
                //        ac.FormularyVersionId = updatableFormulary.FormularyVersionId;
                //    });
                //}

                //if (updatableFormulary.FormularyIngredient.IsCollectionValid())
                //{
                //    updatableFormulary.FormularyIngredient.Each(ac =>
                //    {
                //        ac.RowId = null;
                //        ac.FormularyVersionId = updatableFormulary.FormularyVersionId;
                //    });
                //}

                //if (updatableFormulary.FormularyRouteDetail.IsCollectionValid())
                //{
                //    updatableFormulary.FormularyRouteDetail.Each(ac =>
                //    {
                //        ac.RowId = null;
                //        ac.FormularyVersionId = updatableFormulary.FormularyVersionId;
                //    });
                //}

                //formularyRepo.Add(updatableFormulary);
            }

            return true;
        }

        private bool DeleteRecordWithSimilarCode(IEnumerable<FormularyHeader> duplicateFormulariesWithSameCodeFromDb, IFormularyRepository<FormularyHeader> formularyRepo)
        {
            //Update the old records with IsLatest = false
            //Add new record with Delete status

            //var uniqueFormularyVersions = duplicateFormulariesWithSameCodeFromDb.Select(dup => dup.FormularyVersionId).ToList();

            //var updatableFormularies = formularyRepo.GetFormularyListForIds(uniqueFormularyVersions).ToList();

            //if (updatableFormularies.IsCollectionValid())
            //{
            //    updatableFormularies.Each(recToUpdate =>
            //    {
            //        recToUpdate.IsLatest = false;
            //        formularyRepo.Update(recToUpdate);
            //    });
            //}

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

                    //var detail = dupAsNew.FormularyDetail.First();

                    //detail.FormularyVersionId = dupAsNew.FormularyVersionId;

                    //if (dupAsNew.FormularyAdditionalCode.IsCollectionValid())
                    //{
                    //    dupAsNew.FormularyAdditionalCode.Each(ac =>
                    //    {
                    //        ac.FormularyVersionId = dupAsNew.FormularyVersionId;
                    //    });
                    //}

                    //if (dupAsNew.FormularyIndication.IsCollectionValid())
                    //{
                    //    dupAsNew.FormularyIndication.Each(ac =>
                    //    {
                    //        ac.FormularyVersionId = dupAsNew.FormularyVersionId;
                    //    });
                    //}

                    //if (dupAsNew.FormularyIngredient.IsCollectionValid())
                    //{
                    //    dupAsNew.FormularyIngredient.Each(ac =>
                    //    {
                    //        ac.FormularyVersionId = dupAsNew.FormularyVersionId;
                    //    });
                    //}

                    //if (dupAsNew.FormularyRouteDetail.IsCollectionValid())
                    //{
                    //    dupAsNew.FormularyRouteDetail.Each(ac =>
                    //    {
                    //        ac.FormularyVersionId = dupAsNew.FormularyVersionId;
                    //    });
                    //}

                    //formularyRepo.Add(dupAsNew);
                }
            });

            return true;
        }
    }
}
