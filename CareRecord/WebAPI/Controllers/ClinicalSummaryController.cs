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


﻿using Interneuron.CareRecord.API.AppCode.DTO;
using Interneuron.CareRecord.Model.DomainModels.Manual;
using Interneuron.CareRecord.Model.DomainModels;
using Interneuron.Common.Extensions;
using InterneuronAutonomic.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Interneuron.CareRecord.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ClinicalSummaryController : ControllerBase
    {
        DynamicAPIClient _dynamicAPIClient;
        private IConfiguration _configuration { get; }

        //List<string[]> labResultsConfigs = new List<string[]>();
        public ClinicalSummaryController(DynamicAPIClient dynamicAPIClient, IConfiguration Configuration)
        {
            _dynamicAPIClient = dynamicAPIClient;

            _configuration = Configuration;

            //var section = _configuration.GetSection("LabResults:Configs");
            
            //if(section.GetChildren().ToListOrDefault().Count > 0)
            //{
            //    for(int i = 0; i < section.GetChildren().ToListOrDefault().Count; i++)
            //    {
            //        var labresults = _configuration.GetSection("LabResults:Configs" + ":" + i.ToString())?.GetChildren()?.ToArray()?.Select(x => x.Value).ToArray();

            //        labResultsConfigs.Add(labresults);
            //    }
            //}
            
        }

        [HttpGet]
        [Route("GetDiagnoses/{personId}")]
        public async Task<string> GetDiagnoses(string personId)
        {
            string result = string.Empty;
           
            result = await new TaskFactory<string>().StartNew(() =>
            {
                return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_getdiagnoses", CreateDiagnosesFilter(personId));
            });

            return result;
        }

        [HttpGet]
        [Route("GetProcedures/{personId}")]
        public async Task<string> GetProcedures(string personId)
        {
            string result = string.Empty;

            result = await new TaskFactory<string>().StartNew(() =>
            {
                return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_getprocedures", CreateDiagnosesFilter(personId));
            });

            return result;
        }

        [HttpPost]
        [Route("PostDiagnosis/{personId}")]
        public async Task<string> PostDiagnosis(string personId, [FromBody] Diagnosis diagnosis)
        {
            if (diagnosis.IsNull())
                return null;

            string encounterId = "";

            string encounterDetails = CheckIfPatientIsAdmitted(personId);

            if(encounterDetails != "[]" || encounterDetails.IsNotEmpty())
            {
                var enc = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(encounterDetails);

                foreach (var item in enc)
                {
                    if (item.ContainsKey("encounter_id"))
                    {
                        encounterId = item["encounter_id"];
                    }
                }
            }
            else
            {
                string encounter = GetLatestEncounter(personId);

                if (encounter != "[]" || encounter.IsNotEmpty())
                {
                    var encter = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(encounter);

                    foreach (var item in encter)
                    {
                        if (item.ContainsKey("encounter_id"))
                        {
                            encounterId = item["encounter_id"];
                        }
                    }
                }
            }

            Diagnosis diag = new Diagnosis();

            diag.clinicalstatus = diagnosis.clinicalstatus;
            //diag.clinicalsummary_id = clinicalSummaryId;
            diag.diagnosiscode = diagnosis.diagnosiscode;
            diag.diagnosistext = diagnosis.diagnosistext;
            diag.diagnosis_id = diagnosis.diagnosis_id;
            diag.encounter_id = encounterId;
            diag.enddate = diagnosis.enddate;
            diag.onsetdate = diagnosis.onsetdate;
            diag.person_id = personId;
            diag.reportedby = diagnosis.reportedby;
            diag.resolveddate = diagnosis.resolveddate;
            diag.statuscode = diagnosis.statuscode;
            diag.statustext = diagnosis.statustext;
            diag.verificationstatus = diagnosis.verificationstatus;
            diag.isdateapproximate = diagnosis.isdateapproximate;
            diag.dateeffectiveperiod = diagnosis.dateeffectiveperiod;
            diag.effectivedatestring = diagnosis.effectivedatestring;

        string postObjectResult = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.PostObject("core", "diagnosis", diag);
            });

            return postObjectResult;
        }

        [HttpPost]
        [Route("PostProcedure/{personId}")]
        public async Task<string> PostProcedure(string personId, [FromBody] Procedure procedure)
        {
            if (procedure.IsNull())
                return null;

            string encounterId = "";

            string encounterDetails = CheckIfPatientIsAdmitted(personId);

            if (encounterDetails != "[]" || encounterDetails.IsNotEmpty())
            {
                var enc = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(encounterDetails);

                foreach (var item in enc)
                {
                    if (item.ContainsKey("encounter_id"))
                    {
                        encounterId = item["encounter_id"];
                    }
                }
            }
            else
            {
                string encounter = GetLatestEncounter(personId);

                if (encounter != "[]" || encounter.IsNotEmpty())
                {
                    var encter = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(encounter);

                    foreach (var item in encter)
                    {
                        if (item.ContainsKey("encounter_id"))
                        {
                            encounterId = item["encounter_id"];
                        }
                    }
                }
            }

            Procedure proc = new Procedure();

            proc.anaesthesiacode = procedure.anaesthesiacode;
            proc.anaesthesiaminutes = procedure.anaesthesiaminutes;
            //proc.clinicalsummary_id = clinicalSummaryId;
            proc.code = procedure.code;
            proc.duration = procedure.duration;
            proc.encounter_id = encounterId;
            proc.isprimary = procedure.isprimary;
            proc.name = procedure.name;
            proc.operation_id = procedure.operation_id;
            proc.performedby = procedure.performedby;
            proc.person_id = personId;
            proc.proceduredate = procedure.proceduredate;
            proc.proceduremodifiercode = procedure.proceduremodifiercode;
            proc.proceduremodifiertext = procedure.proceduremodifiertext;
            proc.procedure_id = procedure.procedure_id;
            proc.setid = procedure.setid;
            proc.status = procedure.status;
            proc.isdateapproximate = procedure.isdateapproximate;
            proc.dateeffectiveperiod = procedure.dateeffectiveperiod;
            proc.effectivedatestring = procedure.effectivedatestring;

            string postObjectResult = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.PostObject("core", "procedure", proc);
            });

            return postObjectResult;
        }

        private List<object> CreateDiagnosesFilter(string personId)
        {
            Filter filter = new Filter();
            filter.filterClause = "person_id = @person_id";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter personIdParameter = new FilterParameter();
            personIdParameter.paramName = "person_id";
            personIdParameter.paramValue = personId;

            //FilterParameter encounterIdParameter = new FilterParameter();
            //encounterIdParameter.paramName = "status_code";
            //encounterIdParameter.paramValue = activeOnly;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(personIdParameter);
            //filterParameters.filterparams.Add(encounterIdParameter);

            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT * ";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY 1 desc";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }

        //private List<object> CreateClinicalSummaryListFilter(string personId)
        //{
        //    Filter filter = new Filter();
        //    filter.filterClause = "person_id = @person_id";

        //    Filters filters = new Filters();
        //    filters.filters.Add(filter);

        //    FilterParameter personIdParameter = new FilterParameter();
        //    personIdParameter.paramName = "person_id";
        //    personIdParameter.paramValue = personId;

           

        //    FilterParameters filterParameters = new FilterParameters();
        //    filterParameters.filterparams.Add(personIdParameter);

        //    SelectStatement selectStatement = new SelectStatement();
        //    selectStatement.selectstatement = "SELECT * ";

        //    OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
        //    orderByGroupByStatement.ordergroupbystatement = "ORDER BY 1 desc";

        //    List<object> body = new List<object>();
        //    body.Add(filters);
        //    body.Add(filterParameters);
        //    body.Add(selectStatement);
        //    body.Add(orderByGroupByStatement);

        //    return body;
        //}

        //[HttpPost]
        //[Route("GetClinicalSummaryList/{personId}")]
        //public async Task<string> GetClinicalSummaryList(string personId)
        //{
        //    var result = await new TaskFactory<string>().StartNew(() =>
        //    {
                
        //        return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_clinicalsummarylist", CreateClinicalSummaryListFilter(personId));
        //    });

        //    return result;
        //}


        //[HttpPost]
        //[Route("GetActiveDiagnoses/{personId}/{encounterId}")]
        //public async Task<string> GetActiveDiagnoses(string personId, string encounterId)
        //{
        //    var result = await new TaskFactory<string>().StartNew(() =>
        //    {

        //        return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_activediagnoses", CreateActiveDiagnosesFilter(personId, encounterId));
        //    });

        //    return result;
        //}

        //private List<object> CreateActiveDiagnosesFilter(string personId, string encounterId)
        //{
        //    Filter filter = new Filter();
        //    filter.filterClause = "person_id = @person_id and encounter_id = @encounter_id";

        //    Filters filters = new Filters();
        //    filters.filters.Add(filter);

        //    FilterParameter personIdParameter = new FilterParameter();
        //    personIdParameter.paramName = "person_id";
        //    personIdParameter.paramValue = personId;

        //    FilterParameter encounterIdParameter = new FilterParameter();
        //    encounterIdParameter.paramName = "encounter_id";
        //    encounterIdParameter.paramValue = encounterId;

        //    FilterParameters filterParameters = new FilterParameters();
        //    filterParameters.filterparams.Add(personIdParameter);
        //    filterParameters.filterparams.Add(encounterIdParameter);
            
        //    SelectStatement selectStatement = new SelectStatement();
        //    selectStatement.selectstatement = "SELECT * ";

        //    OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
        //    orderByGroupByStatement.ordergroupbystatement = "ORDER BY 1 desc";

        //    List<object> body = new List<object>();
        //    body.Add(filters);
        //    body.Add(filterParameters);
        //    body.Add(selectStatement);
        //    body.Add(orderByGroupByStatement);

        //    return body;
        //}

        //[HttpPost]
        //[Route("GetCompletedProcedures/{personId}/{encounterId}")]
        //public async Task<string> GetCompletedProcedures(string personId, string encounterId, string clinicalSummaryId)
        //{
        //    var result = await new TaskFactory<string>().StartNew(() =>
        //    {

        //        return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_completedprocedures", CreateActiveDiagnosesFilter(personId, encounterId));
        //    });

        //    return result;
        //}

        [HttpPost]
        [Route("GetLabResults/{personId}")]
        public async Task<List<string>> GetLabResults(string personId, [FromBody] List<string[]> labResultsConfigs)
        {
            List<string> results = new List<string>();

            foreach(var labresults in labResultsConfigs)
            {
                var result = await new TaskFactory<string>().StartNew(() =>
                {

                    return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_labresults", CreateLabResultsFilter(personId, labresults));
                });

                Dictionary<string, int> newOrderIndexedMap = Enumerable.Range(0, labresults.Length - 1).ToDictionary(r => labresults[r].ToLower().Replace("_", " "), r => r);

                var res = JsonConvert.DeserializeObject<List<LabResults>>(result);

                var orderedResult = res.OrderBy(r =>
                {
                    int index;
                    bool foundIndex = newOrderIndexedMap.TryGetValue(r.observationidentifiertext.ToLower(), out index);
                    return foundIndex ? index : Int32.MaxValue;
                });

                result = JsonConvert.SerializeObject(orderedResult);

                results.Add(result);
            }
            
            return results;
        }

        private List<object> CreateLabResultsFilter(string personId, string[] labResults)
        {
            Filter filter = new Filter();

            string conditions = "lower(observationidentifiertext) in (";

            string inCondition = "";

            for (int srIndex = 0; srIndex < labResults.Length; srIndex++)
            {
                inCondition = $"{inCondition},'{labResults[srIndex].ToLower().Replace("_"," ")}'";
            }

            inCondition = inCondition.TrimStart(',');

            conditions = $"{conditions} {inCondition}) and person_id = @person_id";

            filter.filterClause = conditions;

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter parameter = new FilterParameter();
            parameter.paramName = "person_id";
            parameter.paramValue = personId;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(parameter);

            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT distinct on (observationidentifiertext) observationidentifiertext, *";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY observationidentifiertext, observationdatetime DESC";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }

        [HttpGet]
        [Route("GetSepsisChartList/{personId}")]
        public async Task<List<string>> GetSepsisChartList(string personId)
        {
            List<string> results = new List<string>();

            var obs = await new TaskFactory<string>().StartNew(() =>
            {

                return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_latestheartrateandtemperature", CreateObservationFilter(personId));
            });

            results.Add(obs);


            var crp = await new TaskFactory<string>().StartNew(() =>
            {

                return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_latestcrpresult", CreateCRPFilter(personId));
            });

            results.Add(crp);


            var mb = await new TaskFactory<string>().StartNew(() =>
            {

                return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_latestmicrobiologyreport", CreateMBFilter(personId));
            });

            results.Add(mb);

            var prescriptions = await new TaskFactory<string>().StartNew(() =>
            {

                return _dynamicAPIClient.GetBaseViewListByPost("epma_prescriptiondetail", CreateMedicationFilter(personId));
            });

            results.Add(prescriptions);

            return results;
        }

        //private List<object> CreateObservationFilter(string personId, string encounterId)
        private List<object> CreateObservationFilter(string personId)
        {
            Filter filter = new Filter();
            filter.filterClause = "person_id = @person_id";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter personIdParameter = new FilterParameter();
            personIdParameter.paramName = "person_id";
            personIdParameter.paramValue = personId;

            //FilterParameter encounterIdParameter = new FilterParameter();
            //encounterIdParameter.paramName = "encounter_id";
            //encounterIdParameter.paramValue = encounterId;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(personIdParameter);
            //filterParameters.filterparams.Add(encounterIdParameter);

            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT distinct on (name) name, timerecorded, *";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY name, timerecorded desc";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }

        private List<object> CreateCRPFilter(string personId)
        {
            Filter filter = new Filter();
            filter.filterClause = "person_id = @person_id";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter personIdParameter = new FilterParameter();
            personIdParameter.paramName = "person_id";
            personIdParameter.paramValue = personId;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(personIdParameter);


            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT *";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY observationdatetime desc LIMIT 1";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }

        private List<object> CreateMBFilter(string personId)
        {
            Filter filter = new Filter();
            filter.filterClause = "person_id = @person_id";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter personIdParameter = new FilterParameter();
            personIdParameter.paramName = "person_id";
            personIdParameter.paramValue = personId;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(personIdParameter);


            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT * ";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY 2 DESC";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }

        //private List<object> CreateMedicationFilter(string personId, string encounterId)
        private List<object> CreateMedicationFilter(string personId)
        {
            string encounterId = "";

            string encounterDetails = CheckIfPatientIsAdmitted(personId);

            if (encounterDetails != "[]" || encounterDetails.IsNotEmpty())
            {
                var enc = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(encounterDetails);

                foreach (var item in enc)
                {
                    if (item.ContainsKey("encounter_id"))
                    {
                        encounterId = item["encounter_id"];
                    }
                }
            }

            Filter filter = new Filter();
            filter.filterClause = "person_id = @person_id and encounter_id = @encounter_id " 
                                + "and prescriptionstatus_id in ('fd8833de-213b-4570-8cc7-67babfa31393', '63e946cd-b4a4-4f60-9c18-a384c49486ea', 'fe406230-be68-4ad6-a979-ef15c42365cf') "
                                + "and prescriptioncontext_id = '18075621-59fd-4daa-a891-6bb492db087e' "
                                + "and (__medications::text ilike '%paracetamol%' or __medications::text ilike '%\"isantimicrobial\":true%')";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter personIdParameter = new FilterParameter();
            personIdParameter.paramName = "person_id";
            personIdParameter.paramValue = personId;

            FilterParameter encounterIdParameter = new FilterParameter();
            encounterIdParameter.paramName = "encounter_id";
            encounterIdParameter.paramValue = encounterId;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(personIdParameter);
            filterParameters.filterparams.Add(encounterIdParameter);

            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT * ";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY lastmodifiedon desc";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }

        //[HttpPost]
        //[Route("GetTaskLists/{personId}/{encounterId}/{clinicalSummaryId}")]
        //public async Task<string> GetTaskLists(string personId, string encounterId, string clinicalSummaryId)
        //{
        //    var result = await new TaskFactory<string>().StartNew(() =>
        //    {

        //        return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_tasklist", CreateGenericClinicalSummaryFilter(personId, encounterId, clinicalSummaryId));
        //    });

        //    return result;
        //}

        [HttpPost]
        [Route("PostClinicalSummaryNotes/{personId}")]
        public async Task<string> PostClinicalSummaryNotes(string personId, [FromBody] ClinicalSummaryNotes clinicalSummaryNotes)
        {
            if (clinicalSummaryNotes.IsNull())
                return null;

            ClinicalSummaryNotes csn = new ClinicalSummaryNotes();

            csn.clinicalsummarynotes_id = clinicalSummaryNotes.clinicalsummarynotes_id;
            //csn.clinicalsummary_id = clinicalSummaryId;
            //csn.encounter_id = encounterId;
            csn.notes = clinicalSummaryNotes.notes;
            csn.person_id = personId;
            
            string postObjectResult = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.PostObject("core", "clinicalsummarynotes", csn);
            });

            return postObjectResult;
        }

        [HttpPost]
        [Route("PostInvestigation/{personId}")]
        public async Task<string> PostInvestigation(string personId, [FromBody] Investigation investigation)
        {
            if (investigation.IsNull())
                return null;

            Investigation investigationResults = new Investigation();

            investigationResults.clinicalinvestigationnotes = investigation.clinicalinvestigationnotes;
            //investigationResults.clinicalsummary_id = clinicalSummaryId;
            //investigationResults.encounter_id = encounterId;
            investigationResults.investigation_id = investigation.investigation_id;
            investigationResults.person_id = personId;
            
            string postObjectResult = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.PostObject("core", "investigation", investigationResults);
            });

            return postObjectResult;
        }

        [HttpPost]
        [Route("PostDischargePlan/{personId}")]
        public async Task<string> PostDischargePlan(string personId, [FromBody] Dischargeplan dischargeplan)
        {
            if (dischargeplan.IsNull())
                return null;

            Dischargeplan dp = new Dischargeplan();

            //dp.clinicalsummary_id = clinicalSummaryId;
            dp.dischargeplannotes = dischargeplan.dischargeplannotes;
            dp.dischargeplan_id = dischargeplan.dischargeplan_id;
            //dp.encounter_id = encounterId;
            dp.person_id = personId;

            string postObjectResult = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.PostObject("core", "dischargeplan", dp);
            });

            return postObjectResult;
        }

        [HttpPost]
        [Route("PostTask/{personId}")]
        public async Task<string> PostTask(string personId, [FromBody] Model.DomainModels.Manual.Task tsk)
        {
            if (tsk.IsNull())
                return null;

            string encounterId = "";

            string encounterDetails = CheckIfPatientIsAdmitted(personId);

            if (encounterDetails != "[]" || encounterDetails.IsNotEmpty())
            {
                var enc = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(encounterDetails);

                foreach (var item in enc)
                {
                    if (item.ContainsKey("encounter_id"))
                    {
                        encounterId = item["encounter_id"];
                    }
                }
            }
            else
            {
                string encounter = GetLatestEncounter(personId);

                if (encounter != "[]" || encounter.IsNotEmpty())
                {
                    var encter = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(encounter);

                    foreach (var item in encter)
                    {
                        if (item.ContainsKey("encounter_id"))
                        {
                            encounterId = item["encounter_id"];
                        }
                    }
                }
            }

            Model.DomainModels.Manual.Task t = new Model.DomainModels.Manual.Task();

            t.allocateddatetime = tsk.allocateddatetime;
            t.allocatedto = tsk.allocatedto;
            //t.clinicalsummary_id = clinicalSummaryId;
            t.correlationid = tsk.correlationid;
            t.correlationtype = tsk.correlationtype;
            t.duedate = tsk.duedate;
            t.encounter_id = encounterId;
            t.notes = tsk.notes;
            t.owner = tsk.owner;
            t.ownerassigneddatetime = tsk.ownerassigneddatetime;
            t.person_id = personId;
            t.priority = tsk.priority;
            t.status = tsk.status;
            t.taskcreatedby = tsk.taskcreatedby;
            t.taskcreateddatetime = tsk.taskcreateddatetime;
            t.taskdetails = tsk.taskdetails;
            t.taskname = tsk.taskname;
            t.tasktype = tsk.tasktype;
            t.task_id = tsk.task_id;
            
            string postObjectResult = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.PostObject("core", "task", t);
            });

            return postObjectResult;
        }

        //[HttpDelete]
        //[Route("DeleteDiagnosis/{diagnosisId}")]
        //public async Task<string> DeleteDiagnosis(string diagnosisId)
        //{
        //    if (diagnosisId.IsNull())
        //        return null;

        //    string deleteObject = await new TaskFactory().StartNew(() =>
        //    {
        //        return _dynamicAPIClient.DeleteObject("core", "diagnosis", diagnosisId);
        //    });

        //    return deleteObject;
        //}

        //[HttpDelete]
        //[Route("DeleteProcedure/{procedureId}")]
        //public async Task<string> DeleteProcedure(string procedureId)
        //{
        //    if (procedureId.IsNull())
        //        return null;

        //    string deleteObject = await new TaskFactory().StartNew(() =>
        //    {
        //        return _dynamicAPIClient.DeleteObject("core", "procedure", procedureId);
        //    });

        //    return deleteObject;
        //}

        //[HttpDelete]
        //[Route("DeleteTask/{taskId}")]
        //public async Task<string> DeleteTask(string taskId)
        //{
        //    if (taskId.IsNull())
        //        return null;

        //    string deleteObject = await new TaskFactory().StartNew(() =>
        //    {
        //        return _dynamicAPIClient.DeleteObject("core", "task", taskId);
        //    });

        //    return deleteObject;
        //}

        [HttpGet]
        [Route("GetDiagnosisHistory/{diagnosisId}")]
        public async Task<string> GetDiagnosisHistory(string diagnosisId)
        {
            if (diagnosisId.IsNull())
                return null;

            string historyObject = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.GetObjectHistory("core", "diagnosis", diagnosisId);
            });

            return historyObject;
        }

        [HttpGet]
        [Route("GetProcedureHistory/{procedureId}")]
        public async Task<string> GetProcedureHistory(string procedureId)
        {
            if (procedureId.IsNull())
                return null;

            string historyObject = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.GetObjectHistory("core", "procedure", procedureId);
            });

            return historyObject;
        }

        [HttpGet]
        [Route("GetTaskHistory/{taskId}")]
        public async Task<string> GetTaskHistory(string taskId)
        {
            if (taskId.IsNull())
                return null;

            string historyObject = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.GetObjectHistory("core", "task", taskId);
            });

            return historyObject;
        }

        [HttpGet]
        [Route("GetClinicalSummaryNotesHistory/{clinicalSummaryNotesId}")]
        public async Task<string> GetClinicalSummaryNotesHistory(string clinicalSummaryNotesId)
        {
            if (clinicalSummaryNotesId.IsNull())
                return null;

            string historyObject = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.GetObjectHistory("core", "clinicalsummarynotes", clinicalSummaryNotesId);
            });

            return historyObject;
        }

        [HttpGet]
        [Route("GetInvestigationHistory/{investigationId}")]
        public async Task<string> GetInvestigationHistory(string investigationId)
        {
            if (investigationId.IsNull())
                return null;

            string historyObject = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.GetObjectHistory("core", "investigation", investigationId);
            });

            return historyObject;
        }

        [HttpGet]
        [Route("GetDischargePlanHistory/{dischargeplanId}")]
        public async Task<string> GetDischargePlanHistory(string dischargeplanId)
        {
            if (dischargeplanId.IsNull())
                return null;

            string historyObject = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.GetObjectHistory("core", "dischargeplan", dischargeplanId);
            });

            return historyObject;
        }

        [HttpGet]
        [Route("GetClinicalSummaryNote/{personId}")]
        public async Task<string> GetClinicalSummaryNote(string personId)
        {
            var result = await new TaskFactory<string>().StartNew(() =>
            {

                return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_clinicalsummarynotes", CreateGenericClinicalSummaryFilter(personId));
            });

            return result;
        }

        [HttpGet]
        [Route("GetDischargePlan/{personId}")]
        public async Task<string> GetDischargePlan(string personId)
        {
            var result = await new TaskFactory<string>().StartNew(() =>
            {

                return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_dischargeplan", CreateGenericClinicalSummaryFilter(personId));
            });

            return result;
        }

        [HttpGet]
        [Route("GetInvestigation/{personId}")]
        public async Task<string> GetInvestigation(string personId)
        {
            var result = await new TaskFactory<string>().StartNew(() =>
            {

                return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_investigation", CreateGenericClinicalSummaryFilter(personId));
            });

            return result;
        }

        [HttpGet]
        [Route("GetTasks/{personId}")]
        public async Task<string> GetTasks(string personId)
        {
            var result = await new TaskFactory<string>().StartNew(() =>
            {

                return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_gettasks", CreateTaskFilter(personId));
            });

            return result;
        }

        [HttpGet]
        [Route("GetClinicalSummaryStatuses/{type}")]
        public async Task<string> GetClinicalSummaryStatuses(string type)
        {
            var result = await new TaskFactory<string>().StartNew(() =>
            {

                return _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_clinicalsummarystatuses", CreateClinicalSummaryStatusFilter(type));
            });

            return result;
        }

        private List<object> CreateClinicalSummaryStatusFilter(string type)
        {
            Filter filter = new Filter();
            filter.filterClause = "lower(type) = lower(@type)";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter typeParameter = new FilterParameter();
            typeParameter.paramName = "type";
            typeParameter.paramValue = type;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(typeParameter);


            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT *";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY displayorder";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }

        //[HttpPost]
        //[Route("PostClinicalSummary/{personId}/{encounterId}/{clinicalSummaryId}")]
        //public async Task<string> PostClinicalSummary(string personId, string encounterId, string clinicalSummaryId, [FromBody] ClinicalSummary clinicalSummary)
        //{
        //    if (clinicalSummary.IsNull())
        //        return null;

        //    ClinicalSummary cs = new ClinicalSummary();

        //    cs.clinicalsummary_id = clinicalSummaryId;
        //    cs.status = clinicalSummary.status;
        //    cs.encounter_id = encounterId;
        //    cs.person_id = personId;

        //    string postObjectResult = await new TaskFactory().StartNew(() =>
        //    {
        //        return _dynamicAPIClient.PostObject("core", "clinicalsummary", cs);
        //    });

        //    return postObjectResult;
        //}

        //private List<object> CreateGenericClinicalSummaryFilter(string personId, string encounterId, string clinicalSummaryId)
        private List<object> CreateGenericClinicalSummaryFilter(string personId)
        {
            Filter filter = new Filter();
            //filter.filterClause = "person_id = @person_id and encounter_id = @encounter_id and clinicalsummary_id = @clinicalsummary_id";
            filter.filterClause = "person_id = @person_id";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter personIdParameter = new FilterParameter();
            personIdParameter.paramName = "person_id";
            personIdParameter.paramValue = personId;

            //FilterParameter encounterIdParameter = new FilterParameter();
            //encounterIdParameter.paramName = "encounter_id";
            //encounterIdParameter.paramValue = encounterId;

            //FilterParameter clinicalSummaryIdParameter = new FilterParameter();
            //clinicalSummaryIdParameter.paramName = "clinicalsummary_id";
            //clinicalSummaryIdParameter.paramValue = clinicalSummaryId;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(personIdParameter);
            //filterParameters.filterparams.Add(encounterIdParameter);
            //filterParameters.filterparams.Add(clinicalSummaryIdParameter);

            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT * ";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY 1 desc";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }

        //[HttpGet]
        //[Route("GetClinicalSummaryWithCreate/{personId}/{encounterId}")]
        //public async Task<string> GetClinicalSummaryWithCreate(string personId, string encounterId)
        //{
        //    string currClinicalSummary = await GetClinicalSummary(personId, encounterId); //to be async

        //    if (currClinicalSummary == null || string.Compare(currClinicalSummary, "[]") == 0)
        //    {
        //        //Can be in parallel -- to be refactored
        //        string clinicalSummaryId = Guid.NewGuid().ToString();

        //        currClinicalSummary = await CreateNewClinicalSummary(personId, encounterId, clinicalSummaryId);
        //    }

        //    return currClinicalSummary;
        //}

        //[HttpGet]
        //[Route("GetClinicalSummary/{personId}/{encounterId}")]
        //public async Task<string> GetClinicalSummary(string personId, string encounterId)
        //{

        //    var result = await new TaskFactory<string>().StartNew(() =>
        //    {
        //        return _dynamicAPIClient.GetListByPost("core", "clinicalsummary", CreateActiveDiagnosesFilter(personId, encounterId));
        //    });

        //    return result;
        //}

        //private async Task<string> CreateNewClinicalSummary(string personId, string encounterId, string clinicalSummaryId)
        //{
        //    ClinicalSummary cs = new ClinicalSummary();

        //    cs.clinicalsummary_id = clinicalSummaryId;
        //    cs.encounter_id = encounterId;
        //    cs.person_id = personId;
        //    cs.status = "IN PROGRESS";


        //    string postObjectResult = await new TaskFactory().StartNew(() =>
        //    {
        //        return _dynamicAPIClient.PostObject("core", "clinicalsummary", cs);
        //    });


        //    //var csList = JsonConvert.DeserializeObject<List<ClinicalSummary>>(postObjectResult);

        //    //var csObject = csList.FirstOrDefault();

        //    //var csId = csObject.clinicalsummary_id;

        //    return postObjectResult;
        //}

        private string CheckIfPatientIsAdmitted(string personId)
        {
            var result =  _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_currentinpatients", CreateEncounterFilter(personId));
            
            return result;
        }

        private List<object> CreateEncounterFilter(string personId)
        {
            Filter filter = new Filter();
            filter.filterClause = "person_id = @person_id";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter personIdParameter = new FilterParameter();
            personIdParameter.paramName = "person_id";
            personIdParameter.paramValue = personId;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(personIdParameter);


            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT *";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY admitdatetime desc LIMIT 1";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }

        private string GetLatestEncounter(string personId)
        {
            var result = _dynamicAPIClient.GetBaseViewListByPost("clinicalsummary_getlatestencounter", CreateEncounterFilter(personId));

            return result;
        }

        [HttpGet]
        [Route("GetProviders")]
        public async Task<string> GetProviders()
        {
            string providerObject = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.GetList("core", "provider");
            });

            return providerObject;
        }

        private List<object> CreateTaskFilter(string personId)
        {
            Filter filter = new Filter();
            //filter.filterClause = "person_id = @person_id and encounter_id = @encounter_id and clinicalsummary_id = @clinicalsummary_id";
            filter.filterClause = "person_id = @person_id";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter personIdParameter = new FilterParameter();
            personIdParameter.paramName = "person_id";
            personIdParameter.paramValue = personId;

            //FilterParameter encounterIdParameter = new FilterParameter();
            //encounterIdParameter.paramName = "encounter_id";
            //encounterIdParameter.paramValue = encounterId;

            //FilterParameter clinicalSummaryIdParameter = new FilterParameter();
            //clinicalSummaryIdParameter.paramName = "clinicalsummary_id";
            //clinicalSummaryIdParameter.paramValue = clinicalSummaryId;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(personIdParameter);
            //filterParameters.filterparams.Add(encounterIdParameter);
            //filterParameters.filterparams.Add(clinicalSummaryIdParameter);

            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT * ";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY person_id";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }

    }
}