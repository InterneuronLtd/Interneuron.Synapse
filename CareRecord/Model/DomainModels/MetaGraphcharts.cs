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
using System.Collections.Generic;

namespace Interneuron.CareRecord.Model.DomainModels
{
    public partial class baseview_MetaGraphcharts : Interneuron.CareRecord.Infrastructure.Domain.EntityBase
    {
        public string ChartId { get; set; }
        public string Chartname { get; set; }
        public string GraphId { get; set; }
        public string Graphname { get; set; }
        public string Units { get; set; }
        public string Graphtype { get; set; }
        public string[] Parameterkey { get; set; }
        public decimal? Chartgraphorder { get; set; }
        public decimal[] Parameterdomain { get; set; }
        public decimal[] Secondaryparameterdomain { get; set; }
        public decimal[] Ewsbandingrange { get; set; }
        public string[] Parameterlabelsdomain { get; set; }
        public string[] Scale { get; set; }
        public string[] Ordinalparameterdomain { get; set; }
    }
}
