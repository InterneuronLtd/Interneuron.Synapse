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


﻿using Interneuron.Common.Extensions;
using System.Collections.Generic;

namespace SynapseStudioWeb.DataService.APIModel
{

    public partial class TerminologyAPIResponse<T>
    {
        public StatusCode StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
        public T Data { get; set; }
    }

    public enum StatusCode
    {
        Success = 1,
        Fail = 2,
    }

    public partial class TerminiologyAPIStatusModel
    {
        public short StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<string> ErrorMessages { get; set; } = new List<string>();
    }

    public static class TerminiologyStatusModelExtension
    {
        public static bool IsSuccess(this TerminiologyAPIStatusModel statusModel)
        {
            return statusModel != null && statusModel.StatusCode == 1;
        }

        public static bool HasErrors(this TerminiologyAPIStatusModel statusModel)
        {
            return statusModel != null && statusModel.ErrorMessages.IsCollectionValid();
        }

        public static List<string> GetErrors(this TerminiologyAPIStatusModel statusModel)
        {
            return statusModel != null && statusModel.ErrorMessages.IsCollectionValid() ? statusModel.ErrorMessages : null;
        }

        public static string GetFlattenedErrors(this TerminiologyAPIStatusModel statusModel)
        {
            return statusModel != null && statusModel.ErrorMessages.IsCollectionValid() ? string.Join('\n', statusModel.ErrorMessages) : null;
        }
    }


}
