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


﻿using System;
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_BvassessmentAllassessmentwithredandamber : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string AssessmentId { get; set; }
        public string Sectionname { get; set; }
        public string Test { get; set; }
        public string Selection { get; set; }
        public string Section1Recenttraumasurgeryinvasiveprocedure { get; set; }
        public string Section1Indwellinglinesbrokenskin { get; set; }
        public string Section1Impairedimmunity { get; set; }
        public string Section2Impairedimmunity { get; set; }
        public string Section2Respiratory { get; set; }
        public string Section2Urine { get; set; }
        public string Section2Skinjointwound { get; set; }
        public string Section2Indwellingdevice { get; set; }
        public string Section2Brain { get; set; }
        public string Section2Surgical { get; set; }
        public string Section2Other { get; set; }
        public string Section3Doesnwakewhenrousedwonstayawake { get; set; }
        public string Section3Looksveryunwelltohealthcareprofessional { get; set; }
        public string Section3Weakhighpitchedorcontinuouscry { get; set; }
        public string Section3Severetachycardia { get; set; }
        public string Section3Severetachypnoea { get; set; }
        public string Section3Bradycardia { get; set; }
        public string Section3Nonblanchingrashmottledashencyanotic { get; set; }
        public string Section3Temperature { get; set; }
        public string Section3Ifunder3monthstemperature { get; set; }
        public string Section4Notrespondingnormallynosmile { get; set; }
        public string Section4Reducedactivityverysleepy { get; set; }
        public string Section4Moderatetachypnoea { get; set; }
        public string Section4Moderatetachycardia { get; set; }
        public string Section4Spo2lessthen92perorincreasedo2requirement { get; set; }
        public string Section4Nasalflaring { get; set; }
        public string Section4Capillaryrefilltime3seconds { get; set; }
        public string Section4Reducedurineoutput { get; set; }
        public string Section4Legpainorcoldextremities { get; set; }
        public string Section4Immunocompromised { get; set; }
    }
}
