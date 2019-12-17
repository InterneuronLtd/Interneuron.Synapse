//Interneuron Synapse

//Copyright(C) 2019  Interneuron CIC



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

namespace SynapseStudio.Models
{
    public class ApplicationModuleMappingModel
    {
        public string applicationmodulemapping_id { get; set; }
        public string module_id { get; set; }
        public string modulename { get; set; }
        public string moduledescription { get; set; }
        public string jsurl { get; set; }
        public int? displayorder { get; set; }
        public string domselector { get; set; }
        public string displayname { get; set; }
        public bool isselected { get; set; }
}

    public class Mappedmodule
    {
        public string applicationmodulemapping_id { get; set; }
        public string application_id { get; set; }
        public string module_id { get; set; }
       
        public int? displayorder { get; set; }
        public string displayname { get; set; }

        public bool? isdefaultmodule { get; set; }
    }

    public class MappedList
    {
        public string applicationlist_id { get; set; }
        public string application_id { get; set; }
        public string listid { get; set; }

        public int? displayorder { get; set; }
        public string listname { get; set; }

      
    }
}