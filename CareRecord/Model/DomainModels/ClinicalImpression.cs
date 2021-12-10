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


using System;
using System.Collections.Generic;
using System.Text;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public class ClinicalImpression
    {
        List<Diagnosis> diagnosis;
        List<Procedure> procedures;
		ClinicalSummaryNotes clinicalSummaryNotes;
		Investigation investigation;
		Dischargeplan dischargeplan;
		List<Task> tasks;
		ClinicalImpression clinicalSummary;
	}

	public class Diagnosis
	{
		public string diagnosis_id { get; set; }
		public string person_id { get; set; }
		public string encounter_id { get; set; }
		public string operation_id { get; set; }
		public string diagnosiscode { get; set; }
		public string diagnosistext { get; set; }
		public string statuscode { get; set; }
		public string statustext { get; set; }
		public DateTime? onsetdate { get; set; }
		public DateTime? enddate { get; set; }
		public string clinicalstatus { get; set; }
		public string verificationstatus { get; set; }
		public DateTime? resolveddate { get; set; }
		public string reportedby { get; set; }
		public string clinicalsummary_id { get; set; }
		public string isdateapproximate { get; set; }
		public string dateeffectiveperiod { get; set; }
		public string effectivedatestring { get; set; }
	}

	public class Procedure
	{
		public string procedure_id { get; set; }
		public DateTime? proceduredate { get; set; }
		public int? duration { get; set; }
		public string person_id { get; set; }
		public string encounter_id { get; set; }
		public bool? isprimary { get; set; }
		public string operation_id { get; set; }
		public string name { get; set; }
		public string anaesthesiacode { get; set; }
		public string proceduremodifiercode { get; set; }
		public string proceduremodifiertext { get; set; }
		public int? setid { get; set; }
		public float? anaesthesiaminutes { get; set; }
		public string status { get; set; }
		public string code { get; set; }
		public string performedby { get; set; }
		public string clinicalsummary_id { get; set; }
		public string isdateapproximate { get; set; }
		public string dateeffectiveperiod { get; set; }
		public string effectivedatestring { get; set; }
	}

	public class ClinicalSummaryNotes
	{
		public string clinicalsummarynotes_id { get; set; }
		public string notes { get; set; }
		public string person_id { get; set; }
		//public string encounter_id { get; set; }
		//public string clinicalsummary_id { get; set; }
	}

	public class Investigation
	{
		public string investigation_id { get; set; }
		public string clinicalinvestigationnotes { get; set; }
		public string person_id { get; set; }
		//public string encounter_id { get; set; }
		//public string clinicalsummary_id { get; set; }
	}

	public class Dischargeplan
	{
		public string dischargeplan_id { get; set; }
		public string dischargeplannotes { get; set; }
		public string person_id { get; set; }
		//public string encounter_id { get; set; }
		//public string clinicalsummary_id { get; set; }
	}

	public class Task
	{
		public string task_id { get; set; }
		public string correlationid { get; set; }
		public string correlationtype { get; set; }
		public string person_id { get; set; }
		public string tasktype { get; set; }
		public string taskdetails { get; set; }
		public string taskcreatedby { get; set; }
		public DateTime? taskcreateddatetime { get; set; }
		public string taskname { get; set; }
		public string allocatedto { get; set; }
		public string notes { get; set; }
		public string priority { get; set; }
		public string status { get; set; }
		public string owner { get; set; }
		public string encounter_id { get; set; }
		public DateTime? duedate { get; set; }
		public DateTime? allocateddatetime { get; set; }
		public DateTime? ownerassigneddatetime { get; set; }
		//public string clinicalsummary_id { get; set; }
	}

	//public class ClinicalSummary
	//{
	//	public string clinicalsummary_id { get; set; }
	//	public string person_id { get; set; }
	//	public string encounter_id { get; set; }
	//	public string status { get; set; }
	//}

	public class ClinicalSummaryStatuses
	{
		public string clinicalsummarystatuses_id { get; set; }
		public string code { get; set; }
		public string description { get; set; }
		public string type { get; set; }
		public int? displayorder { get; set; }
	}
}
