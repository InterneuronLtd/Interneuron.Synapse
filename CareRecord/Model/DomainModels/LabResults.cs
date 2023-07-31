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
    public class LabResults
    {
        public string observationidentifiertext { get; set; } 
        public string result_id { get; set; }
        public string person_id { get; set; }
        public string order_id { get; set; }
        public string observationdate { get; set; }
        public string observationtime { get; set; }
        public string observationdatetime { get; set; }
        public string observationidentifiercode { get; set; }
        public string observationvalue { get; set; }
        public string referencerangelow { get; set; }
        public string referencerangehigh { get; set; }
        public string unitstext { get; set; }
        public string abnormalflag { get; set; }
        public string rangelevel { get; set; }
    }
}
