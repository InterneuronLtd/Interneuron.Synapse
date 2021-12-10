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


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.CareRecord.API.AppCode.DTO.FluidBalance
{
    public class PersonDTO
    {
		public string person_id { get; set; }
		public string firstname { get; set; }
		public string middlename { get; set; }
		public string familyname { get; set; }
		public string fullname { get; set; }
		public string preferredname { get; set; }
		public string titlecode { get; set; }
		public string titletext { get; set; }
		public string dateofbirth { get; set; }
		public string dateofbirthts { get; set; }
		public string dateofdeath { get; set; }
		public string dateofdeathts { get; set; }
		public string gendercode { get; set; }
		public string gendertext { get; set; }
		public string ethnicitycode { get; set; }
		public string ethnicitytext { get; set; }
		public string maritalstatuscode { get; set; }
		public string maritalstatustext { get; set; }
		public string religioncode { get; set; }
		public string religiontext { get; set; }
		public bool deathindicator { get; set; }
		public string primarylanguagecode { get; set; }
		public string primarylanguagetext { get; set; }
		public string interpreterrequired { get; set; }
	}
}
