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

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_DemoPerson : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string PersonId { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Familyname { get; set; }
        public string Fullname { get; set; }
        public string Preferredname { get; set; }
        public string Titlecode { get; set; }
        public string Titletext { get; set; }
        public DateTime? Dateofbirth { get; set; }
        public DateTime? Dateofbirthts { get; set; }
        public DateTime? Dateofdeath { get; set; }
        public DateTime? Dateofdeathts { get; set; }
        public string Gendercode { get; set; }
        public string Gendertext { get; set; }
        public string Ethnicitycode { get; set; }
        public string Ethnicitytext { get; set; }
        public string Maritalstatuscode { get; set; }
        public string Maritalstatustext { get; set; }
        public string Religioncode { get; set; }
        public string Religiontext { get; set; }
        public bool? Deathindicator { get; set; }
        public string Primarylanguagecode { get; set; }
        public string Primarylanguagetext { get; set; }
        public string Interpreterrequired { get; set; }
        public string AnonstringFullnames { get; set; }
        public DateTime? AnondateDob { get; set; }
    }
}
