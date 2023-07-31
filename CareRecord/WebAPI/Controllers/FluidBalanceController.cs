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
﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interneuron.CareRecord.HL7SynapseHandler.Service.Models;
using InterneuronAutonomic.API;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Interneuron.CareRecord.API.AppCode.DTO;
using Interneuron.CareRecord.API.AppCode.DTO.FluidBalance;
using Interneuron.CareRecord.API.AppCode.Core;
using Interneuron.Common.Extensions;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Configuration;

namespace Interneuron.CareRecord.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class FluidBalanceController : ControllerBase
    {
        private const string FLUIDBALANCESESSIONKEY = "core|fluidbalancesession";

        private const string WEIGHTKEY = "value";

        private const string DISPLAYORDERKEY = "displayorder";

        int maxIntakeRoutes, maxOutputRoutes;
        private IConfiguration _configuration { get; }

        DynamicAPIClient _dynamicAPIClient;
        public FluidBalanceController(DynamicAPIClient dynamicAPIClient, IConfiguration Configuration)
        {
            _dynamicAPIClient = dynamicAPIClient;

            _configuration = Configuration;

            maxIntakeRoutes = this._configuration.GetValue<int>("FluidBalanceSettings:maxIntakeRoutes");

            maxOutputRoutes = this._configuration.GetValue<int>("FluidBalanceSettings:maxOutputRoutes");
        }

        [HttpGet]
        [Route("GetHelloWorld")]
        public string GetHelloWorld()
        {
            return "Hello World";
        }

        [HttpPost]
        [Route("AddUpdateRemoveSingleVolumeObservation/{personId?}/{encounterId?}")]
        public async Task<string> AddUpdateRemoveSingleVolumeObservation(string personId, string encounterId, [FromBody] FluidBalanceIntakeOutputDTO fbIO)
        {
            if (fbIO.IsNull())
                return null;

            PersonDTO persons = await GetPersonDetails(personId);

            int age = GetPersonAge(persons);

            string weight = await GetWeightObservations(personId, encounterId);

            var lstWt = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(weight);

            string wt = "";

            foreach (var item in lstWt)
            {
                if (item.ContainsKey(WEIGHTKEY))
                {
                    wt = item[WEIGHTKEY];
                }
            }

            float expectedVolume = await GetExpectedOutputVolume(age, float.Parse(wt));

            FluidBalanceIntakeOutputDTO fluidBalanceIntakeOutput = new FluidBalanceIntakeOutputDTO();

            if (fbIO.isamended || fbIO.isremoved)
            {
                fluidBalanceIntakeOutput.fluidbalanceintakeoutput_id = fbIO.fluidbalanceintakeoutput_id;
                fluidBalanceIntakeOutput.modifiedby = fbIO.addedby;
            }
            else
            {
                fluidBalanceIntakeOutput.fluidbalanceintakeoutput_id = Guid.NewGuid().ToString();
                fluidBalanceIntakeOutput.addedby = fbIO.addedby;
                fluidBalanceIntakeOutput.modifiedby = fbIO.addedby;
            }

            fluidBalanceIntakeOutput.personweight = float.Parse(wt);
            fluidBalanceIntakeOutput.fluidbalancesession_id = fbIO.fluidbalancesession_id;
            fluidBalanceIntakeOutput.fluidbalancesessionroute_id = fbIO.fluidbalancesessionroute_id;
            fluidBalanceIntakeOutput.route_id = fbIO.route_id;
            fluidBalanceIntakeOutput.routetype_id = fbIO.routetype_id;
            fluidBalanceIntakeOutput.fluidbalanceiotype_id = fbIO.fluidbalanceiotype_id;
            fluidBalanceIntakeOutput.volume = fbIO.volume;
            if (DateTime.TryParse(fbIO.datetime, out DateTime result))
                fluidBalanceIntakeOutput.datetime = result.ToString("yyyy-MM-ddTHH:mm:ss");
            fluidBalanceIntakeOutput.isamended = fbIO.isamended;
            fluidBalanceIntakeOutput.isremoved = fbIO.isremoved;
            fluidBalanceIntakeOutput.units = "ml";
            fluidBalanceIntakeOutput.person_id = personId;
            fluidBalanceIntakeOutput.expectedvolume = fbIO.expectedvolume;
            fluidBalanceIntakeOutput.isintake = fbIO.isintake;
            fluidBalanceIntakeOutput.otherroutetype = fbIO.otherroutetype;

            if (fbIO.isamended)
                fluidBalanceIntakeOutput.reasonforamend = fbIO.reasonforamend;
            else
                fluidBalanceIntakeOutput.reasonforamend = "";

            if (fbIO.isremoved)
                fluidBalanceIntakeOutput.reasonforremoval = fbIO.reasonforremoval;
            else
                fluidBalanceIntakeOutput.reasonforremoval = "";

            if (!fbIO.isintake)
                fluidBalanceIntakeOutput.fluidcapturedevice_id = fbIO.fluidcapturedevice_id;
            else
                fluidBalanceIntakeOutput.fluidcapturedevice_id = "";

#if debug
                var x = JsonConvert.SerializeObject(fluidBalanceIntakeOutput);
#endif

            string postObjectResult = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.PostObject("core", "fluidbalanceintakeoutput", fluidBalanceIntakeOutput);
            });

            return postObjectResult;
        }

        [HttpPost]
        [Route("AddSessionRoute")]
        public async Task<string> AddSessionRoute([FromBody] FluidBalanceSessionRouteDTO fluidBalanceSessionRoute)
        {
            if (fluidBalanceSessionRoute.IsNull())
            {
                return null;
            }

            int displayOrder = await GetDisplayOrderForRoutes(fluidBalanceSessionRoute.fluidbalancesession_id, fluidBalanceSessionRoute.isintake);

            string errorMessage = string.Empty;

            string fbSessionRouteAdd = string.Empty;

            if (fluidBalanceSessionRoute.isintake && !CheckIfBetweenIntakeDisplayOrder(displayOrder) && displayOrder >= maxIntakeRoutes)
            {
                errorMessage = "Can't add more intake routes";
            }

            if (!fluidBalanceSessionRoute.isintake && !CheckIfBetweenOutputDisplayOrder(displayOrder) && displayOrder >= (maxIntakeRoutes + maxOutputRoutes))
            {
                errorMessage = "Can't add more output routes";
            }

            if (displayOrder != 0)
            {
                FluidBalanceSessionRouteDTO fbsr = new FluidBalanceSessionRouteDTO();
                fbsr.addedby = fluidBalanceSessionRoute.addedby;
                if (DateTime.TryParse(fluidBalanceSessionRoute.dateadded, out DateTime result))
                    fbsr.dateadded = result.ToString("yyyy-MM-ddTHH:mm:ss");
                fbsr.displayorder = displayOrder;
                fbsr.fluidbalancesessionroute_id = Guid.NewGuid().ToString();
                fbsr.fluidbalancesession_id = fluidBalanceSessionRoute.fluidbalancesession_id;
                fbsr.hasbeenamended = false;
                fbsr.isintake = fluidBalanceSessionRoute.isintake;
                fbsr.modifiedby = fluidBalanceSessionRoute.addedby;
                fbsr.routename = fluidBalanceSessionRoute.routename;
                fbsr.route_id = fluidBalanceSessionRoute.route_id;

                FluidBalanceSessionRouteSessionsDTO fbsrs = new FluidBalanceSessionRouteSessionsDTO();
                fbsrs.fluidbalancesessionroutesessions_id = Guid.NewGuid().ToString();
                fbsrs.fluidbalancesession_id = fluidBalanceSessionRoute.fluidbalancesession_id;
                fbsrs.fluidbalancesessionroute_id = fbsr.fluidbalancesessionroute_id;

                List<Dictionary<string, object>> objs = new List<Dictionary<string, object>>();

                var fbSessionRoute = new Dictionary<string, object>
                {
                    { "core|fluidbalancesessionroute", fbsr }
                };

                var fbSessionRouteSessions = new Dictionary<string, object>
                {
                    { "core|fluidbalancesessionroutesessions", fbsrs }
                };

                objs.Add(fbSessionRoute);
                objs.Add(fbSessionRouteSessions);

                fbSessionRouteAdd = await new TaskFactory().StartNew<string>(() => _dynamicAPIClient.PostObjectsInTransaction(objs));
            }

            if (errorMessage.IsEmpty())
            {
                return fbSessionRouteAdd;
            }
            else
            {
                return errorMessage;
            }
        }

        [HttpGet]
        [Route("GetSession/{personId?}/{encounterId?}/{startDate?}")]
        public async Task<string> GetSession(string personId, string encounterId, string startDate)
        {
            if (DateTime.TryParse(startDate, out DateTime startDt))
                startDate = startDt.ToString("yyyy-MM-ddTHH:mm:ss");

            var result = await new TaskFactory<string>().StartNew(() =>
            {
                return _dynamicAPIClient.GetListByPost("core", "fluidbalancesession", CreateSessionFilter(personId, encounterId, startDate));
            });

            return result;
        }

        [HttpGet]
        [Route("GetSessionWithCreate/{personId?}/{encounterId?}/{startDate?}/{userId?}/{stopDate?}")]
        public async Task<string> GetSessionWithCreate(string personId, string encounterId, string startDate, string userId, string stopDate)
        {
            //Get Current Session with routes if present
            //else create new session with default routes
            if (startDate.IsEmpty())
            {
                startDate = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            }

            if (DateTime.TryParse(startDate, out DateTime startDt))
                startDate = startDt.ToString("yyyy-MM-ddTHH:mm:ss");

            string currSession = await GetSession(personId, encounterId, startDate); //to be async

            if (currSession == null || string.Compare(currSession, "[]") == 0)
            {
                //Can be in parallel -- to be refactored
                var prevOngoingInfusions = await GetCIRoutesFromPreviousSession(personId, encounterId, startDate);

                currSession = await CreateNewSessionWithDefaultRoutes(personId, encounterId, userId, startDate, stopDate, prevOngoingInfusions);
            }

            return currSession;
        }

        [HttpPost]
        [Route("SetMonitoringStatus")]
        public async Task<string> SetMonitoringStatus([FromBody] FluidBalancePersonStatusDTO fluidBalancePersonStatus)
        {
            if (fluidBalancePersonStatus.IsNull())
            {
                return null;
            }

            FluidBalancePersonStatusDTO fluidBalancePersonStatusDTO = new FluidBalancePersonStatusDTO();

            if (fluidBalancePersonStatus.fluidbalancepersonstatus_id.IsEmpty())
            {
                fluidBalancePersonStatusDTO.fluidbalancepersonstatus_id = Guid.NewGuid().ToString();
                fluidBalancePersonStatusDTO.addedby = fluidBalancePersonStatus.addedby;
                fluidBalancePersonStatusDTO.modifiedby = fluidBalancePersonStatus.addedby;
            }
            else
            {
                fluidBalancePersonStatusDTO.fluidbalancepersonstatus_id = fluidBalancePersonStatus.fluidbalancepersonstatus_id;
                fluidBalancePersonStatusDTO.modifiedby = fluidBalancePersonStatus.addedby;
            }

            fluidBalancePersonStatusDTO.person_id = fluidBalancePersonStatus.person_id;
            fluidBalancePersonStatusDTO.encounter_id = fluidBalancePersonStatus.encounter_id;
            fluidBalancePersonStatusDTO.isactive = fluidBalancePersonStatus.isactive;

            string result = await new TaskFactory().StartNew(() =>
            {
                return _dynamicAPIClient.PostObject("core", "fluidbalancepersonstatus", fluidBalancePersonStatusDTO);
            });

            return result;
        }

        [HttpGet]
        [Route("GetUrineOutputHistoryForSession/{personId?}/{fluidBalanceSessionId?}/{fluidBalanceSessionRouteId?}/{routeId?}")]
        public async Task<List<UrineOutputHistoryDTO>> GetUrineOutputHistoryForSession(string personId, string fluidBalanceSessionId, string fluidBalanceSessionRouteId, string routeId)
        {
            List<UrineOutputHistoryDTO> urineOutputHistories = new List<UrineOutputHistoryDTO>();

            string result = await new TaskFactory<string>().StartNew(() =>
            {
                return _dynamicAPIClient.GetBaseViewListByPost("fluidbalance_urineoutputhistory", CreateUrineOutputHistoryFilter(fluidBalanceSessionId, fluidBalanceSessionRouteId, routeId));
            });

            //PersonDTO person = await GetPersonDetails(personId);

            string session = await GetSessionBySessionId(fluidBalanceSessionId);

            FluidBalanceSessionDTO fb = JsonConvert.DeserializeObject<FluidBalanceSessionDTO>(session);

            int age = Convert.ToInt32(fb.InitialAge);

            float weight = float.Parse(Convert.ToString(fb.InitialWeight));

            DateTime sessionStartTime = DateTime.Now;

            if (DateTime.TryParse(Convert.ToString(fb.Startdate), out DateTime startDtTm))
            {
                sessionStartTime = startDtTm;
            }

            List <dynamic> urineHistories = JsonConvert.DeserializeObject<List<dynamic>>(result);

            int urineHistoryCounter = 0;

            DateTime previousUrineOutputDtTm = DateTime.Now;

            DateTime currentUrineOutputDtTm = DateTime.Now;

            //float prevActualVol = 0;

            //float currActualVol = 0;

            foreach (var urineHistory in urineHistories)
            {
                previousUrineOutputDtTm = currentUrineOutputDtTm;
                currentUrineOutputDtTm = Convert.ToDateTime(urineHistory["datetime"]);

                //prevActualVol = currActualVol;
                //currActualVol = float.Parse(Convert.ToString(urineHistory["volume"]));

                UrineOutputHistoryDTO urineOutputHistory = new UrineOutputHistoryDTO();
                urineOutputHistory.fluidbalanceescalation_id = urineHistory["fluidbalanceescalation_id"];

                float averageUrineOutput = 0;

                if (urineHistoryCounter == 0)
                {
                    TimeSpan ts = currentUrineOutputDtTm - sessionStartTime;

                    double hrs = ts.TotalHours;

                    float actualVolume = float.Parse(Convert.ToString(urineHistory["volume"]));

                    if(hrs != 0)
                    {
                        averageUrineOutput = (float)Math.Round((actualVolume / hrs), 2);
                    }
                    else
                    {
                        averageUrineOutput = 0;
                    }
                    
                }
                else
                {
                    TimeSpan ts = currentUrineOutputDtTm - previousUrineOutputDtTm;

                    double hrs = ts.TotalHours;

                    float actualVolume = float.Parse(Convert.ToString(urineHistory["volume"]));

                    if (hrs != 0)
                    {
                        averageUrineOutput = (float)Math.Round((actualVolume / hrs), 2);
                    }
                    else
                    {
                        averageUrineOutput = 0;
                    }
                }

                urineOutputHistory.volume = float.Parse(Convert.ToString(urineHistory["volume"]));
                urineOutputHistory.averagevolume = averageUrineOutput;
                urineOutputHistory.age = age;
                urineOutputHistory.personweight = weight;
                urineOutputHistory.sbar = urineHistory["sbar"];
                urineOutputHistory.units = urineHistory["units"];
                urineOutputHistory.expectedvolume = float.Parse(Convert.ToString(urineHistory["expectedvolume"]));
                urineOutputHistory.datetime = Convert.ToDateTime(urineHistory["datetime"]).ToString("yyyy-MM-ddTHH:mm:ss");
                urineOutputHistory.modifiedby = urineHistory["modified"];
                urineOutputHistory.device = urineHistory["device"]; //Added to get device used for collecting urine

                urineOutputHistories.Add(urineOutputHistory);

                urineHistoryCounter++;
            }

            return urineOutputHistories;
        }

        [HttpGet]
        [Route("CalculateExpectedUrineOutput/{personId?}/{encounterId?}")]
        public async Task<float> CalculateExpectedUrineOutput(string personId, string encounterId)
        {
            PersonDTO person = await GetPersonDetails(personId);

            int age = GetPersonAge(person);

            string weight = await GetWeightObservations(personId, encounterId);

            var lstWt = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(weight);

            string wt = "";

            int count = 0;

            foreach (var item in lstWt)
            {
                if (item.ContainsKey(WEIGHTKEY) && count == 0)
                {
                    wt = item[WEIGHTKEY];
                }

                count++;
            }

            float expectedVolume = await GetExpectedOutputVolume(age, float.Parse(wt));

            return expectedVolume;
        }

        [HttpPost]
        [Route("AddFlushToContinuousInfusion")]
        public async Task<string> AddFlushToContinuousInfusion([FromBody] List<dynamic> flushObjects)
        {
            if (flushObjects.Count < 0)
                return null;

            List<dynamic> orderedList = new List<dynamic>(3);

            foreach (var ls in flushObjects)
            {
                if (ls["core|continuousinfusion"] != null)
                {
                    orderedList.Insert(0, ls["core|continuousinfusion"]);
                }
            }

            foreach (var ls in flushObjects)
            {
                if (ls["core|continuousinfusionevent"] != null)
                {
                    orderedList.Insert(1, ls["core|continuousinfusionevent"]);
                }
            }

            foreach (var ls in flushObjects)
            {
                if (ls["core|fluidbalanceintakeoutput"] != null)
                {
                    orderedList.Insert(2, ls["core|fluidbalanceintakeoutput"]);
                }
            }

            string flushEventId = Guid.NewGuid().ToString();

            ContinuousInfusionDTO continuousInfusion = new ContinuousInfusionDTO();

            ContinuousInfusionEventDTO continuousInfusionEvent = new ContinuousInfusionEventDTO();

            FluidBalanceIntakeOutputDTO fluidBalanceIntakeOutput = new FluidBalanceIntakeOutputDTO();

            for (int i=0; i < orderedList.Count; i++)
            {
                if (i==0)
                {
                    continuousInfusion.eventcorrelationid = flushEventId;
                    continuousInfusion.addedby = orderedList[i].addedby;
                    continuousInfusion.completioncomments = orderedList[i].completioncomments;
                    continuousInfusion.continuousinfusion_id = Guid.NewGuid().ToString();
                    if (DateTime.TryParse(Convert.ToString(orderedList[i].finishdatetime), out DateTime finishDt))
                        continuousInfusion.finishdatetime = finishDt.ToString("yyyy-MM-ddTHH:mm:ss");
                    continuousInfusion.flowrate = float.Parse(Convert.ToString(orderedList[i].flowrate));
                    continuousInfusion.flowrateunit = orderedList[i].flowrateunit;
                    continuousInfusion.fluidbalancesessionroute_id = orderedList[i].fluidbalancesessionroute_id;
                    continuousInfusion.fluidbalancesession_id = orderedList[i].fluidbalancesession_id;
                    continuousInfusion.islineremovedoncompletion = orderedList[i].islineremovedoncompletion;
                    continuousInfusion.ispaused = orderedList[i].ispaused;
                    continuousInfusion.modifiedby = orderedList[i].modifiedby;
                    continuousInfusion.notes = orderedList[i].notes;
                    continuousInfusion.pumpnumber = orderedList[i].pumpnumber;
                    continuousInfusion.reasonforpause = orderedList[i].reasonforpause;
                    continuousInfusion.routetype_id = orderedList[i].routetype_id;
                    continuousInfusion.route_id = orderedList[i].route_id;
                    if (DateTime.TryParse(Convert.ToString(orderedList[i].startdatetime), out DateTime startDt))
                        continuousInfusion.startdatetime = startDt.ToString("yyyy-MM-ddTHH:mm:ss");
                    continuousInfusion.totaladministeredvolume = float.Parse(Convert.ToString(orderedList[i].totaladministeredvolume));
                    continuousInfusion.totalremainingvolume = float.Parse(Convert.ToString(orderedList[i].totalremainingvolume));
                    continuousInfusion.totalvolume = float.Parse(Convert.ToString(orderedList[i].totalvolume));
                }

                if (i==1)
                {
                    continuousInfusionEvent.continuousinfusionevent_id = flushEventId;
                    continuousInfusionEvent.eventtype = orderedList[i].eventtype;
                    continuousInfusionEvent.datetime = continuousInfusion.startdatetime;
                    continuousInfusionEvent.continuousinfusion_id = continuousInfusion.continuousinfusion_id;
                    continuousInfusionEvent.addedby = orderedList[i].addedby;
                    continuousInfusionEvent.eventcorrelationid = orderedList[i].eventcorrelationid;
                    continuousInfusionEvent.deletecorrelationid = orderedList[i].deletecorrelationid;
                    continuousInfusionEvent.modifiedby = orderedList[i].modifiedby;
                }

                if (i==2)
                {
                    fluidBalanceIntakeOutput.continuousinfusionevent_id = flushEventId;
                    fluidBalanceIntakeOutput.addedby = orderedList[i].addedby;
                    fluidBalanceIntakeOutput.continuousinfusionvalidation_id = orderedList[i].continuousinfusionvalidation_id;
                    fluidBalanceIntakeOutput.continuousinfusion_id = orderedList[i].continuousinfusion_id;
                    if (DateTime.TryParse(Convert.ToString(orderedList[i].datetime), out DateTime dt))
                        fluidBalanceIntakeOutput.datetime = dt.ToString("yyyy-MM-ddTHH:mm:ss");
                    fluidBalanceIntakeOutput.expectedvolume = float.Parse(Convert.ToString(orderedList[i].expectedvolume));
                    fluidBalanceIntakeOutput.fluidbalanceintakeoutput_id = Guid.NewGuid().ToString();
                    fluidBalanceIntakeOutput.fluidbalanceiotype_id = orderedList[i].fluidbalanceiotype_id;
                    fluidBalanceIntakeOutput.fluidbalancesessionroute_id = orderedList[i].fluidbalancesessionroute_id;
                    fluidBalanceIntakeOutput.fluidbalancesession_id = orderedList[i].fluidbalancesession_id;
                    fluidBalanceIntakeOutput.fluidcapturedevice_id = orderedList[i].fluidcapturedevice_id;
                    fluidBalanceIntakeOutput.isamended = orderedList[i].isamended;
                    fluidBalanceIntakeOutput.isintake = orderedList[i].isintake;
                    fluidBalanceIntakeOutput.isremoved = orderedList[i].isremoved;
                    fluidBalanceIntakeOutput.modifiedby = orderedList[i].modifiedby;
                    fluidBalanceIntakeOutput.personweight = float.Parse(Convert.ToString(orderedList[i].personweight));
                    fluidBalanceIntakeOutput.person_id = orderedList[i].person_id;
                    fluidBalanceIntakeOutput.reasonforamend = orderedList[i].reasonforamend;
                    fluidBalanceIntakeOutput.reasonforremoval = orderedList[i].reasonforremoval;
                    fluidBalanceIntakeOutput.routetype_id = orderedList[i].routetype_id;
                    fluidBalanceIntakeOutput.route_id = orderedList[i].route_id;
                    fluidBalanceIntakeOutput.units = orderedList[i].units;
                    fluidBalanceIntakeOutput.volume = float.Parse(Convert.ToString(orderedList[i].volume));
                }
            }

            List<Dictionary<string, object>> objs = new List<Dictionary<string, object>>();

            var ci = new Dictionary<string, object>
            {
                { "core|continuousinfusion", continuousInfusion }
            };

            var cie = new Dictionary<string, object>
            {
                { "core|continuousinfusionevent", continuousInfusionEvent }
            };

            var fbio = new Dictionary<string, object>
            {
                { "core|fluidbalanceintakeoutput", fluidBalanceIntakeOutput }
            };

            objs.Add(ci);
            objs.Add(cie);
            objs.Add(fbio);

            string flushTrans = await new TaskFactory().StartNew(() => _dynamicAPIClient.PostObjectsInTransaction(objs));
            
            return flushTrans;
        }

        [HttpPost]
        [Route("AddBolusToContinuousInfusion")]
        public async Task<string> AddBolusToContinuousInfusion([FromBody] List<dynamic> bolusObjects)
        {
            if (bolusObjects.Count < 0)
                return null;

            List<dynamic> orderedList = new List<dynamic>(3);

            foreach (var ls in bolusObjects)
            {
                if (ls["core|continuousinfusion"] != null)
                {
                    orderedList.Insert(0, ls["core|continuousinfusion"]);
                }
            }

            foreach (var ls in bolusObjects)
            {
                if (ls["core|continuousinfusionevent"] != null)
                {
                    orderedList.Insert(1, ls["core|continuousinfusionevent"]);
                }
            }

            foreach (var ls in bolusObjects)
            {
                if (ls["core|fluidbalanceintakeoutput"] != null)
                {
                    orderedList.Insert(2, ls["core|fluidbalanceintakeoutput"]);
                }
            }

            string bolusEventId = Guid.NewGuid().ToString();

            ContinuousInfusionDTO continuousInfusion = new ContinuousInfusionDTO();

            ContinuousInfusionEventDTO continuousInfusionEvent = new ContinuousInfusionEventDTO();

            FluidBalanceIntakeOutputDTO fluidBalanceIntakeOutput = new FluidBalanceIntakeOutputDTO();

            for (int i = 0; i < orderedList.Count; i++)
            {
                if (i == 0)
                {
                    continuousInfusion.eventcorrelationid = bolusEventId;
                    continuousInfusion.addedby = orderedList[i].addedby;
                    continuousInfusion.completioncomments = orderedList[i].completioncomments;
                    continuousInfusion.continuousinfusion_id = Guid.NewGuid().ToString();
                    if (DateTime.TryParse(Convert.ToString(orderedList[i].finishdatetime), out DateTime finishDt))
                        continuousInfusion.finishdatetime = finishDt.ToString("yyyy-MM-ddTHH:mm:ss");
                    continuousInfusion.flowrate = float.Parse(Convert.ToString(orderedList[i].flowrate));
                    continuousInfusion.flowrateunit = orderedList[i].flowrateunit;
                    continuousInfusion.fluidbalancesessionroute_id = orderedList[i].fluidbalancesessionroute_id;
                    continuousInfusion.fluidbalancesession_id = orderedList[i].fluidbalancesession_id;
                    continuousInfusion.islineremovedoncompletion = orderedList[i].islineremovedoncompletion;
                    continuousInfusion.ispaused = orderedList[i].ispaused;
                    continuousInfusion.modifiedby = orderedList[i].modifiedby;
                    continuousInfusion.notes = orderedList[i].notes;
                    continuousInfusion.pumpnumber = orderedList[i].pumpnumber;
                    continuousInfusion.reasonforpause = orderedList[i].reasonforpause;
                    continuousInfusion.routetype_id = orderedList[i].routetype_id;
                    continuousInfusion.route_id = orderedList[i].route_id;
                    if (DateTime.TryParse(Convert.ToString(orderedList[i].startdatetime), out DateTime startDt))
                        continuousInfusion.startdatetime = startDt.ToString("yyyy-MM-ddTHH:mm:ss");
                    continuousInfusion.totaladministeredvolume = float.Parse(Convert.ToString(orderedList[i].totaladministeredvolume));
                    continuousInfusion.totalremainingvolume = float.Parse(Convert.ToString(orderedList[i].totalremainingvolume));
                    continuousInfusion.totalvolume = float.Parse(Convert.ToString(orderedList[i].totalvolume));
                }

                if (i == 1)
                {
                    continuousInfusionEvent.continuousinfusionevent_id = bolusEventId;
                    continuousInfusionEvent.eventtype = orderedList[i].eventtype;
                    continuousInfusionEvent.datetime = continuousInfusion.startdatetime;
                    continuousInfusionEvent.continuousinfusion_id = continuousInfusion.continuousinfusion_id;
                    continuousInfusionEvent.addedby = orderedList[i].addedby;
                    continuousInfusionEvent.eventcorrelationid = orderedList[i].eventcorrelationid;
                    continuousInfusionEvent.deletecorrelationid = orderedList[i].deletecorrelationid;
                    continuousInfusionEvent.modifiedby = orderedList[i].modifiedby;
                }

                if (i == 2)
                {
                    fluidBalanceIntakeOutput.continuousinfusionevent_id = bolusEventId;
                    fluidBalanceIntakeOutput.addedby = orderedList[i].addedby;
                    fluidBalanceIntakeOutput.continuousinfusionvalidation_id = orderedList[i].continuousinfusionvalidation_id;
                    fluidBalanceIntakeOutput.continuousinfusion_id = orderedList[i].continuousinfusion_id;
                    if (DateTime.TryParse(Convert.ToString(orderedList[i].datetime), out DateTime dt))
                        fluidBalanceIntakeOutput.datetime = dt.ToString("yyyy-MM-ddTHH:mm:ss");
                    fluidBalanceIntakeOutput.expectedvolume = float.Parse(Convert.ToString(orderedList[i].expectedvolume));
                    fluidBalanceIntakeOutput.fluidbalanceintakeoutput_id = Guid.NewGuid().ToString();
                    fluidBalanceIntakeOutput.fluidbalanceiotype_id = orderedList[i].fluidbalanceiotype_id;
                    fluidBalanceIntakeOutput.fluidbalancesessionroute_id = orderedList[i].fluidbalancesessionroute_id;
                    fluidBalanceIntakeOutput.fluidbalancesession_id = orderedList[i].fluidbalancesession_id;
                    fluidBalanceIntakeOutput.fluidcapturedevice_id = orderedList[i].fluidcapturedevice_id;
                    fluidBalanceIntakeOutput.isamended = orderedList[i].isamended;
                    fluidBalanceIntakeOutput.isintake = orderedList[i].isintake;
                    fluidBalanceIntakeOutput.isremoved = orderedList[i].isremoved;
                    fluidBalanceIntakeOutput.modifiedby = orderedList[i].modifiedby;
                    fluidBalanceIntakeOutput.personweight = float.Parse(Convert.ToString(orderedList[i].personweight));
                    fluidBalanceIntakeOutput.person_id = orderedList[i].person_id;
                    fluidBalanceIntakeOutput.reasonforamend = orderedList[i].reasonforamend;
                    fluidBalanceIntakeOutput.reasonforremoval = orderedList[i].reasonforremoval;
                    fluidBalanceIntakeOutput.routetype_id = orderedList[i].routetype_id;
                    fluidBalanceIntakeOutput.route_id = orderedList[i].route_id;
                    fluidBalanceIntakeOutput.units = orderedList[i].units;
                    fluidBalanceIntakeOutput.volume = float.Parse(Convert.ToString(orderedList[i].volume));
                }
            }

            List<Dictionary<string, object>> objs = new List<Dictionary<string, object>>();

            var ci = new Dictionary<string, object>
            {
                { "core|continuousinfusion", continuousInfusion }
            };

            var cie = new Dictionary<string, object>
            {
                { "core|continuousinfusionevent", continuousInfusionEvent }
            };

            var fbio = new Dictionary<string, object>
            {
                { "core|fluidbalanceintakeoutput", fluidBalanceIntakeOutput }
            };

            objs.Add(ci);
            objs.Add(cie);
            objs.Add(fbio);

            string bolusTrans = await new TaskFactory().StartNew(() => _dynamicAPIClient.PostObjectsInTransaction(objs));

            return bolusTrans;
        }
        
        private async Task<List<ContinuousInfusionDTO>> GetCIRoutesFromPreviousSession(string personId, string encounterId, string startDate)
        {
            DateTime startDt = Convert.ToDateTime(startDate).AddHours(-24);

            string previousSession = await GetSession(personId, encounterId, startDt.ToString("yyyy-MM-ddTHH:mm:ss"));

            List<FluidBalanceSessionDTO> prevSessionObjArray = JsonConvert.DeserializeObject<List<FluidBalanceSessionDTO>>(previousSession);

            if (!prevSessionObjArray.IsCollectionValid())
                return null;

            FluidBalanceSessionDTO prevSessionObj = prevSessionObjArray[0];

            string prevFluidBalanceSessionId = prevSessionObj.FluidbalancesessionId;

            if (!prevFluidBalanceSessionId.IsNotEmpty())
                return null;

            string sessionRoutes = GetRoutesBySessionId(prevFluidBalanceSessionId);

            if (!sessionRoutes.IsNotEmpty())
                return null;

            dynamic prevSessionRoutes = JsonConvert.DeserializeObject(sessionRoutes);

            if (prevSessionRoutes == null)
                return null;

            var sessionRouteFilter = CreateCIFilter(prevSessionRoutes);

            string ciRoutes = _dynamicAPIClient.GetListByPost("core", "continuousinfusion", sessionRouteFilter);

            if (ciRoutes == null || string.Compare(ciRoutes, "[]") == 0)
                return null;

            List<ContinuousInfusionDTO> ci = JsonConvert.DeserializeObject<List<ContinuousInfusionDTO>>(ciRoutes);

            if (!ci.IsCollectionValid())
                return null;

            var ongoingInfusions = ci.Where(x => x.totalremainingvolume > 0).ToList();

            if (ongoingInfusions.IsCollectionValid())
            {
                ongoingInfusions = ongoingInfusions.Distinct(oi => oi.fluidbalancesessionroute_id).ToList();
            }

            return ongoingInfusions;
        }
        
        private List<object> CreateSessionFilter(string personId, string encounterId, string startDate)
        {
            Filter filter = new Filter();
            filter.filterClause = "person_id = @person_id and encounter_id = @encounter_id and startdate::date = @startdate::date";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter personIdParameter = new FilterParameter();
            personIdParameter.paramName = "person_id";
            personIdParameter.paramValue = personId;

            FilterParameter encounterIdParameter = new FilterParameter();
            encounterIdParameter.paramName = "encounter_id";
            encounterIdParameter.paramValue = encounterId;

            FilterParameter startDateParameter = new FilterParameter();
            startDateParameter.paramName = "startdate";
            startDateParameter.paramValue = startDate;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(personIdParameter);
            filterParameters.filterparams.Add(encounterIdParameter);
            filterParameters.filterparams.Add(startDateParameter);

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
        
        private async Task<string> CreateNewSessionWithDefaultRoutes(string personId, string encounterId, string userId, string startDate, string stopDate, List<ContinuousInfusionDTO> prevOngoingInfusions)
        {
            PersonDTO personDTO = await GetPersonDetails(personId);

            int age = GetPersonAge(personDTO);

            string weight = await GetWeightObservations(personId, encounterId);

            var lstWt = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(weight);

            string wt = "";

            foreach (var item in lstWt)
            {
                if (item.ContainsKey(WEIGHTKEY))
                {
                    wt = item[WEIGHTKEY];
                }
            }

            FluidBalanceSessionDTO fb = new FluidBalanceSessionDTO();
            fb.FluidbalancesessionId = Guid.NewGuid().ToString();
            if (DateTime.TryParse(startDate, out DateTime startDt))
                fb.Startdate = startDt.ToString("yyyy-MM-ddTHH:mm:ss");

            if (DateTime.TryParse(stopDate, out DateTime stopDt))
                fb.Stopdate = stopDt.ToString("yyyy-MM-ddTHH:mm:ss");

            fb.PersonId = personId;
            fb.EncounterId = encounterId;
            fb.Addedby = userId;
            fb.InitialExpectedUrineOutput = await CalculateExpectedUrineOutput(personId, encounterId);
            fb.InitialWeight = float.Parse(wt);
            fb.InitialAge = float.Parse(Convert.ToString(age));

            FluidBalanceSessionRouteDTO fbsrIntake = new FluidBalanceSessionRouteDTO();
            fbsrIntake.fluidbalancesessionroute_id = Guid.NewGuid().ToString();
            fbsrIntake.fluidbalancesession_id = fb.FluidbalancesessionId;
            fbsrIntake.route_id = "cb93583a-8885-11ea-bc55-0242ac130003";
            if (DateTime.TryParse(startDate, out DateTime intakeDateAdded))
                fbsrIntake.dateadded = intakeDateAdded.ToString("yyyy-MM-ddTHH:mm:ss");
            fbsrIntake.displayorder = 1;
            fbsrIntake.addedby = userId;
            fbsrIntake.modifiedby = userId;
            fbsrIntake.routename = "Oral";
            fbsrIntake.isintake = true;


            FluidBalanceSessionRouteSessionsDTO fbsrsIntake = new FluidBalanceSessionRouteSessionsDTO
            {
                fluidbalancesessionroutesessions_id = Guid.NewGuid().ToString(),
                fluidbalancesession_id = fb.FluidbalancesessionId,
                fluidbalancesessionroute_id = fbsrIntake.fluidbalancesessionroute_id
            };

            FluidBalanceSessionRouteDTO fbsrOutput = new FluidBalanceSessionRouteDTO();
            fbsrOutput.fluidbalancesessionroute_id = Guid.NewGuid().ToString();
            fbsrOutput.fluidbalancesession_id = fb.FluidbalancesessionId;
            fbsrOutput.route_id = "e387930e-3fe4-4c54-bdce-ebb3b763f3df";
            if (DateTime.TryParse(startDate, out DateTime outputDateAdded))
                fbsrOutput.dateadded = outputDateAdded.ToString("yyyy-MM-ddTHH:mm:ss");
            fbsrOutput.displayorder = maxIntakeRoutes + 1;
            fbsrOutput.addedby = userId;
            fbsrOutput.modifiedby = userId;
            fbsrOutput.routename = "Urine";
            fbsrOutput.isintake = false;

            FluidBalanceSessionRouteSessionsDTO fbsrsOutput = new FluidBalanceSessionRouteSessionsDTO
            {
                fluidbalancesessionroutesessions_id = Guid.NewGuid().ToString(),
                fluidbalancesession_id = fb.FluidbalancesessionId,
                fluidbalancesessionroute_id = fbsrOutput.fluidbalancesessionroute_id
            };

            List<Dictionary<string, object>> objs = new List<Dictionary<string, object>>();

            var fbs = new Dictionary<string, object>
            {
                { "core|fluidbalancesession", fb }
            };

            var flblsrIntake = new Dictionary<string, object>
            {
                { "core|fluidbalancesessionroute", fbsrIntake }
            };

            var flblsrsIntake = new Dictionary<string, object>
            {
                { "core|fluidbalancesessionroutesessions", fbsrsIntake }
            };

            var flblsrOutput = new Dictionary<string, object>
            {
                { "core|fluidbalancesessionroute", fbsrOutput }
            };

            var flblsrsOutput = new Dictionary<string, object>
            {
                { "core|fluidbalancesessionroutesessions", fbsrsOutput }
            };

            if (prevOngoingInfusions.IsCollectionValid())
            {
                prevOngoingInfusions.ForEach(inf =>
                {
                    FluidBalanceSessionRouteSessionsDTO prevSessionRouteSessions = new FluidBalanceSessionRouteSessionsDTO
                    {
                        fluidbalancesessionroutesessions_id = Guid.NewGuid().ToString(),
                        fluidbalancesession_id = fb.FluidbalancesessionId,
                        fluidbalancesessionroute_id = inf.fluidbalancesessionroute_id
                    };

                    var prevSRSessions = new Dictionary<string, object>
                    {
                        { "core|fluidbalancesessionroutesessions", prevSessionRouteSessions }
                    };

                    objs.Add(prevSRSessions);
                });

            }

            objs.Add(fbs);
            objs.Add(flblsrIntake);
            objs.Add(flblsrsIntake);
            objs.Add(flblsrOutput);
            objs.Add(flblsrsOutput);

#if debug
                var x = JsonConvert.SerializeObject(objs);
#endif

            string fbTrans = await new TaskFactory().StartNew<string>(() => _dynamicAPIClient.PostObjectsInTransaction(objs));

            var fbtr = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(fbTrans);

            var allEntities = fbtr.Select(a => a.FirstOrDefault());

            var sessionData = allEntities.Where(dd => dd.Key.EqualsIgnoreCase(FLUIDBALANCESESSIONKEY)).FirstOrDefault();

            return sessionData.Value.ToString();
        }
        
        private string GetRoutesBySessionId(string fluidBalanceSessionId)
        {
            string result = _dynamicAPIClient.GetBaseViewListByPost("fluidbalance_fluidbalancesessionroutes", CreateRouteFilter(fluidBalanceSessionId));

            return result;
        }
        
        private List<object> CreateRouteFilter(string fluidBalanceSessionId)
        {
            Filter filter = new Filter();
            filter.filterClause = "fluidbalancesession_id = @fluidbalancesession_id";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter fluidBalanceSessionIdParameter = new FilterParameter();
            fluidBalanceSessionIdParameter.paramName = "fluidbalancesession_id";
            fluidBalanceSessionIdParameter.paramValue = fluidBalanceSessionId;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(fluidBalanceSessionIdParameter);

            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT * ";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY fluidbalancesession_id";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }
        
        private List<object> CreateCIFilter(dynamic fluidBalanceSessionRoutes)
        {
            Filter filter = new Filter();

            string conditions = "fluidbalancesessionroute_id in (";

            string inCondition = "";

            for (int srIndex = 0; srIndex < fluidBalanceSessionRoutes.Count; srIndex++)
            {
                inCondition = $"{inCondition}, '{fluidBalanceSessionRoutes[srIndex].fluidbalancesessionroute_id}'";
            }

            inCondition = inCondition.TrimStart(',');

            conditions = $"{conditions} {inCondition}) and '1' = @one";

            filter.filterClause = conditions;

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter parameter = new FilterParameter();
            parameter.paramName = "one";
            parameter.paramValue = "1";

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(parameter);

            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT * ";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY 1 DESC";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }
        
        private async Task<string> GetWeightObservations(string personId, string encounterId)
        {
            string result = await new TaskFactory<string>().StartNew(() =>
            {
                return _dynamicAPIClient.GetBaseViewListByPost("fluidbalance_getweightobservations", CreateWeightFilter(personId, encounterId));
            });

            return result;
        }
        
        private List<object> CreateWeightFilter(string personId, string encounterId)
        {
            Filter filter = new Filter();
            filter.filterClause = "person_id = @person_id and encounter_id = @encounter_id";

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
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY observationeventdatetime desc";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }
        
        private async Task<PersonDTO> GetPersonDetails(string personId)
        {
            string result = await new TaskFactory<string>().StartNew(() =>
            {
                return _dynamicAPIClient.GetObject("core", "person", personId);
            });

            PersonDTO personDetails = JsonConvert.DeserializeObject<PersonDTO>(result);

            return personDetails;
        }
        
        private int GetPersonAge(PersonDTO personDetails)
        {
            string dateOfBirth = string.Empty;

            int age = 0;

            dateOfBirth = personDetails.dateofbirth ?? DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");

            age = DateTime.Now.Year - Convert.ToDateTime(dateOfBirth).Year;

            if (DateTime.Now.DayOfYear < Convert.ToDateTime(dateOfBirth).DayOfYear)
                age = age - 1;

            return age;
        }
        
        private async Task<float> GetExpectedOutputVolume(int age, float weight)
        {
            string result = await new TaskFactory<string>().StartNew(() =>
            {
                return _dynamicAPIClient.GetList("meta", "expectedurineoutput");
            });

            List<ExpectedUrineOutputDTO> expectedUrineOutputs = JsonConvert.DeserializeObject<List<ExpectedUrineOutputDTO>>(result);

            if (age > 100 || age < 0)
                return 0;

            ExpectedUrineOutputDTO expectedUrineOutput = expectedUrineOutputs.Find(x => age >= x.agefrom && age <= x.ageto);

            if (expectedUrineOutput == null)
                return 0;

            return expectedUrineOutput.volume * weight;
        }
        
        private async Task<int> GetDisplayOrderForRoutes(string fluidBalanceSessionId, bool isIntake)
        {
            int displayOrder = 0;

            bool isError = false;

            string result = await new TaskFactory<string>().StartNew(() =>
            {
                return _dynamicAPIClient.GetBaseViewListByPost("fluidbalance_getdisplayorderforfluidbalanceroute", CreateDisplayRouteFilter(fluidBalanceSessionId, isIntake));
            });

            var displayOrders = JsonConvert.DeserializeObject<List<Dictionary<string, dynamic>>>(result);

            for(int i = 0; i < displayOrders.Count; i++)
            {
                if (displayOrders[i].ContainsKey(DISPLAYORDERKEY))
                {
                    if (isIntake && (Convert.ToInt32(displayOrders[displayOrders.Count - 1][DISPLAYORDERKEY]) < maxIntakeRoutes || displayOrders.Count == maxIntakeRoutes))
                    {
                        displayOrder = Convert.ToInt32(displayOrders[i][DISPLAYORDERKEY]);
                    }
                    else if (isIntake && Convert.ToInt32(displayOrders[i][DISPLAYORDERKEY]) != i + 1)
                    {
                        displayOrder = i;
                        break;
                    }

                    if (!isIntake && (Convert.ToInt32(displayOrders[displayOrders.Count - 1][DISPLAYORDERKEY]) < (maxIntakeRoutes + maxOutputRoutes) || displayOrders.Count == maxOutputRoutes))
                    {
                        displayOrder = Convert.ToInt32(displayOrders[i][DISPLAYORDERKEY]);
                    }
                    else if (!isIntake && Convert.ToInt32(displayOrders[i][DISPLAYORDERKEY]) != maxOutputRoutes + i + 1)
                    {
                        displayOrder = maxOutputRoutes + i;
                        break;
                    }
                }
            }

            if (isIntake && !CheckIfBetweenIntakeDisplayOrder(displayOrder) && displayOrder >= maxIntakeRoutes)
            {
                isError = true;
                displayOrder = 0;
            }

            if (!isIntake && !CheckIfBetweenOutputDisplayOrder(displayOrder) && displayOrder >= (maxIntakeRoutes + maxOutputRoutes))
            {
                isError = true;
                displayOrder = 0;
            }

            if (!isError)
            {
                displayOrder = displayOrder + 1;
            }

            return displayOrder;
        }
        
        private List<object> CreateDisplayRouteFilter(string fluidBalanceSessionId, bool isIntake)
        {
            Filter filter = new Filter();
            filter.filterClause = "fluidbalancesession_id = @fluidbalancesession_id and isintake = @isintake::boolean";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter fluidBalanceIdParameter = new FilterParameter();
            fluidBalanceIdParameter.paramName = "fluidbalancesession_id";
            fluidBalanceIdParameter.paramValue = fluidBalanceSessionId;

            FilterParameter isIntakeParameter = new FilterParameter();
            isIntakeParameter.paramName = "isintake";
            isIntakeParameter.paramValue = Convert.ToString(isIntake);

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(fluidBalanceIdParameter);
            filterParameters.filterparams.Add(isIntakeParameter);

            SelectStatement selectStatement = new SelectStatement();
            selectStatement.selectstatement = "SELECT * ";

            OrderByGroupByStatement orderByGroupByStatement = new OrderByGroupByStatement();
            orderByGroupByStatement.ordergroupbystatement = "ORDER BY 3 ASC";

            List<object> body = new List<object>();
            body.Add(filters);
            body.Add(filterParameters);
            body.Add(selectStatement);
            body.Add(orderByGroupByStatement);

            return body;
        }
       
        private bool CheckIfBetweenIntakeDisplayOrder(int value)
        {
            return value > 1 && value < maxIntakeRoutes;
        }
        
        private bool CheckIfBetweenOutputDisplayOrder(int value)
        {
            return value > (maxIntakeRoutes + 1) && value < (maxIntakeRoutes + maxOutputRoutes);
        }
        
        private List<object> CreateUrineOutputHistoryFilter(string fluidBalanceSessionId, string fluidBalanceSessionRouteId, string routeId)
        {
            Filter filter = new Filter();
            filter.filterClause = "fluidbalancesession_id = @fluidbalancesession_id and fluidbalancesessionroute_id = @fluidbalancesessionroute_id and route_id = @route_id";

            Filters filters = new Filters();
            filters.filters.Add(filter);

            FilterParameter fluidBalanceSessionIdParameter = new FilterParameter();
            fluidBalanceSessionIdParameter.paramName = "fluidbalancesession_id";
            fluidBalanceSessionIdParameter.paramValue = fluidBalanceSessionId;

            FilterParameter fluidBalanceSessionRouteIdParameter = new FilterParameter();
            fluidBalanceSessionRouteIdParameter.paramName = "fluidbalancesessionroute_id";
            fluidBalanceSessionRouteIdParameter.paramValue = fluidBalanceSessionRouteId;

            FilterParameter routeIdParameter = new FilterParameter();
            routeIdParameter.paramName = "route_id";
            routeIdParameter.paramValue = routeId;

            FilterParameters filterParameters = new FilterParameters();
            filterParameters.filterparams.Add(fluidBalanceSessionIdParameter);
            filterParameters.filterparams.Add(fluidBalanceSessionRouteIdParameter);
            filterParameters.filterparams.Add(routeIdParameter);

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

        private async Task<string> GetSessionBySessionId(string fluidBalanceSessionId)
        {
            var result = await new TaskFactory<string>().StartNew(() =>
            {
                return _dynamicAPIClient.GetObject("core", "fluidbalancesession", fluidBalanceSessionId);
            });

            return result;
        }
    }

}