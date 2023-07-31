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
using System.Text;

namespace Interneuron.Terminology.Infrastructure
{
    public class APIRequestContext
    {
        public TerminologyAPIUser TerminologyAPIUser { get; set; }
        public string ClientId { get; set; }
        public string AuthToken { get; set; }

        public static Func<APIRequestContext> APIRequestContextProvider;

        public static APIRequestContext CurrentContext
        {
            get
            {
                if (APIRequestContextProvider == null)
                    throw new Exception("Context provider for the API is not setup");

                return APIRequestContextProvider();
            }
        }
    }
}
        