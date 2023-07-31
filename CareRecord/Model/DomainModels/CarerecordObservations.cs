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


using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_CarerecordObservations : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string ObservationeventId { get; set; }
        public string PersonId { get; set; }
        public DateTime? Datestarted { get; set; }
        public DateTime? Datefinished { get; set; }
        public string Username { get; set; }
        public int? Userid { get; set; }
        public string EncounterId { get; set; }
        public string Scale { get; set; }
        public string Temperature { get; set; }
        public string Consciousness { get; set; }
        public string Systolicbloodpressure { get; set; }
        public string Diastolicbloodpressure { get; set; }
        public string Pulse { get; set; }
        public string Respirations { get; set; }
        public string Oxygensaturations { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Painscoreatrest { get; set; }
        public string Painscorewithmovement { get; set; }
        public string Isonoxygen { get; set; }
        public string Bowelsopen { get; set; }
        public string Device { get; set; }
        public string Inspireoxygenpercentage { get; set; }
        public string Inspireoxygenlitrepermin { get; set; }
        public string Earlywarningscore { get; set; }
        public string Glucose { get; set; }
        public string Respiratorydistress { get; set; }
        public string Escalationofcare { get; set; }
        public string Monitoring { get; set; }
        public string Ispatientsick { get; set; }
        public string Concernsaboutpatient { get; set; }
        public string Couldbeinfection { get; set; }
        public string Reasonfornotescalating { get; set; }
        public string Escalatedtowhom { get; set; }
        public string Monitoringcomments { get; set; }
        public string Dfnconcern { get; set; }
    }
}
