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

namespace Interneuron.Terminology.Infrastructure.Domain
{
    public interface IAuditable
	{
		//DateTime? CreatedDate { get; set; }
        //DateTime? UpdatedDate { get; set; }
        //string CreatedMachine { get; set; }
        DateTime? Createdtimestamp { get; set; }
         DateTime? Createddate { get; set; }
        string Createdby { get; set; }
        DateTime? Updatedtimestamp { get; set; }
        DateTime? Updateddate { get; set; }
        string Updatedby { get; set; }
        string Timezonename { get; set; }
        int? Timezoneoffset { get; set; }
        string Tenant { get; set; }
    }

}
