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


﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SynapseDynamicAPI.Models;
using SynapseDynamicAPI.Services;
using System;
using System.Collections.Generic;
using System.Data;

namespace SynapseDynamicAPI.Controllers
{
    [Route("Interop/")]
    [Authorize]
    public class InteropController : Controller
    {
        private IConfiguration _configuration { get; }
        public InteropController(IConfiguration Configration)
        {
            _configuration = Configration;
        }
        [HttpGet]
        [Route("")]
        [Route("[action]/{encounterId?}")]
        public string GenerateInpatientTransferMessage(string encounterId)
        {
            string messageControlId = null;
            string mrnpid = _configuration["SynapseCore:Settings:MRN_ID_TYPE"];
            string empipid = _configuration["SynapseCore:Settings:EMPI_ID_TYPE"];
            string sql = "select ebenc.wardcode, beds.baycode, beds.bedcode, to_char(ebenc.edd, 'YYYYMMDDHH24MISS') as edd, enc.visitnumber, " +
                         "enc.consultingdoctortext, exenc.consultingdoctorgmccode, exenc.consultingdoctorpasid, exenc.specialtycode, " +
                         "enc.person_id, enc.encounter_id, enc.patientclasscode, mrnpid.idnumber as mrn, nhspid.idnumber as empi, " +
                         "ebenc.bedtransferdatetime " +
                         "from entitystorematerialised.local_eboards_encounter ebenc " +
                         "inner join entitystorematerialised.core_encounter enc on enc.encounter_id = ebenc.encounter_id " +
                         "left outer join entitystorematerialised.extended_encounter exenc on enc.encounter_id = exenc.encounter_id " +
                         "left outer join entitystorematerialised.meta_wardbaybed beds on beds.wardbaybed_id = ebenc.bedcode " +
                         "left outer join entitystorematerialised.core_personidentifier mrnpid on enc.person_id = mrnpid.person_id and mrnpid.idtypecode = '" + mrnpid + "' " +
                         "left outer join entitystorematerialised.core_personidentifier nhspid on enc.person_id = nhspid.person_id and nhspid.idtypecode = '" + empipid + "' " +
                         "where enc.encounter_id = @encounter_id";

            var selectParamList = new List<KeyValuePair<string, object>>()
            {
                new KeyValuePair<string, object>("encounter_id", encounterId)
            };

            DataSet ds = new DataSet();
            ds = DataServices.DataSetFromSQL(sql, selectParamList);
            DataTable dt = ds.Tables[0];

            if (dt.Rows.Count > 0)
            {
                InpatientTransferMessageModel messageData = new InpatientTransferMessageModel()
                {
                    BayCode = Convert.ToString(dt.Rows[0]["baycode"]),
                    BedCode = Convert.ToString(dt.Rows[0]["bedcode"]),
                    ConsultingDoctorGMCCode = Convert.ToString(dt.Rows[0]["consultingdoctorgmccode"]),
                    ConsultingDoctorName = Convert.ToString(dt.Rows[0]["consultingdoctortext"]),
                    ConsultingDoctorPASId = Convert.ToString(dt.Rows[0]["consultingdoctorpasid"]),
                    EMPI = Convert.ToString(dt.Rows[0]["empi"]),
                    Encounter_id = Convert.ToString(dt.Rows[0]["encounter_id"]),
                    ExpectedDischargeDate = Convert.ToString(dt.Rows[0]["edd"]),
                    MRN = Convert.ToString(dt.Rows[0]["mrn"]),
                    PatinetClassCode = Convert.ToString(dt.Rows[0]["patientclasscode"]),
                    Person_Id = Convert.ToString(dt.Rows[0]["person_id"]),
                    SpecialtyCode = Convert.ToString(dt.Rows[0]["specialtycode"]),
                    VisitNumber = Convert.ToString(dt.Rows[0]["visitnumber"]),
                    WardCode = Convert.ToString(dt.Rows[0]["wardcode"]),
                    BedTransferDateTime = (dt.Rows[0]["bedtransferdatetime"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(dt.Rows[0]["bedtransferdatetime"]))
                };

                var hl7Message = HL7MessageServices.GenerateInpatientTransferMessage(messageData, out messageControlId);

                OutboundMessageStore store = new OutboundMessageStore()
                {
                    EMPINumber = Convert.ToString(dt.Rows[0]["empi"]),
                    Encounter_Id = Convert.ToString(dt.Rows[0]["encounter_id"]),
                    HospitalNumber = Convert.ToString(dt.Rows[0]["mrn"]),
                    Message = hl7Message,
                    Message_Id = Guid.NewGuid().ToString(),
                    OutboundMessageStore_Id = Guid.NewGuid().ToString(),
                    Person_Id = Convert.ToString(dt.Rows[0]["person_id"]),
                    SendStatus = 0
                };

                InsertOutboundMessage(store);
            }
             

            return messageControlId;
        }

        private void InsertOutboundMessage(OutboundMessageStore store)
        {
            string sql = "select outbounddestination_id, destinationip, destinationport from interop.outbounddestination";

            DataSet ds = DataServices.DataSetFromSQL(sql, new List<KeyValuePair<string, object>>());

            string insertSQL = "INSERT INTO interop.outboundmessagestore( " +
                               "_createdsource, _createdby, outboundmessagestore_id, message_id, message, destinationip, destinationport, " +
                               "sendstatus, person_id, encounter_id, hospitalnumber, empinumber) " +
                               "VALUES(@createdsource, @createdby, @outboundmessagestore_id, @message_id, @message, @destinationip, @destinationport, " +
                               "@sendstatus, @person_id, @encounter_id, @hospitalnumber, @empinumber) ";

            var insertParamList = new List<KeyValuePair<string, object>>()
                {
                    new KeyValuePair<string, object>("createdsource", "SynapseDynamicAPI"),
                    new KeyValuePair<string, object>("createdby", "SynapseDynamicAPI"),
                    new KeyValuePair<string, object>("outboundmessagestore_id", store.OutboundMessageStore_Id),
                    new KeyValuePair<string, object>("message_id", store.Message_Id),
                    new KeyValuePair<string, object>("message", store.Message),
                    new KeyValuePair<string, object>("destinationip", Convert.ToString(ds.Tables[0].Rows[0]["destinationip"])),
                    new KeyValuePair<string, object>("destinationport", Convert.ToString(ds.Tables[0].Rows[0]["destinationport"])),
                    new KeyValuePair<string, object>("sendstatus", store.SendStatus),
                    new KeyValuePair<string, object>("person_id", store.Person_Id),
                    new KeyValuePair<string, object>("encounter_id", store.Encounter_Id),
                    new KeyValuePair<string, object>("hospitalnumber", store.HospitalNumber),
                    new KeyValuePair<string, object>("empinumber", store.EMPINumber)
                };

            DataServices.ExcecuteNonQueryFromSQL(insertSQL, insertParamList);
        }
    }
}
