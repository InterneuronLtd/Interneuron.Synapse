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
﻿

using System;


namespace SynapseDynamicAPI.Models
{
    public class InpatientTransferMessageModel
    {
        public string WardCode { get; set; }
        public string BayCode { get; set; }
        public string BedCode { get; set; }
        public string ExpectedDischargeDate { get; set; }
        public string VisitNumber { get; set; }
        public string ConsultingDoctorName { get; set; }
        public string ConsultingDoctorGMCCode { get; set; }
        public string ConsultingDoctorPASId { get; set; }
        public string SpecialtyCode { get; set; }
        public string Person_Id { get; set; }
        public string Encounter_id { get; set; }
        public string PatinetClassCode { get; set; }
        public string MRN { get; set; }
        public string EMPI { get; set; }
        public DateTime? BedTransferDateTime { get; set; }
    }
}
