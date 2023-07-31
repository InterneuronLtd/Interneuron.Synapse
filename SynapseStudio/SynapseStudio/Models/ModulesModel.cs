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


namespace SynapseStudio.Models
{
    public class ModulesModel
    {
        public string module_id { get; set; }
        public string modulename { get; set; }
        public string moduledescription { get; set; }
        public string jsurl { get; set; }
        public int displayorder { get; set; }
        public string domselector { get; set; }
    }

    public class PatientListModel
    {
        public string list_id { get; set; }
        public string listname { get; set; }
        public string listdescription { get; set; }
    }
}