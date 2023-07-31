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


﻿using SynapseDynamicAPI.Models;
using System;
using System.Collections.Generic;

namespace SynapseDynamicAPI.Services
{
    internal static class AuthenticatorServices
    {
        private static Object repositoryAccessLock = new object();

        private static Dictionary<string, string> smartCardTokenRepository = new Dictionary<string, string>();

        public static void SaveSmartCardToken(SmartCardUserModel user) 
        {
            lock (repositoryAccessLock)
            {
                smartCardTokenRepository[user.UserId] = user.SmartCardToken;
            }
        }

        public static void RemoveSmartCardToken(string userId)
        {
            lock (repositoryAccessLock)
            {
                if (smartCardTokenRepository.ContainsKey(userId))
                {
                    smartCardTokenRepository.Remove(userId);
                }
            }
        }

        public static string GetSmartCardToken(string userId)
        {
            string smartCardToken = null;

            lock (repositoryAccessLock)
            {
                if (smartCardTokenRepository.ContainsKey(userId))
                {
                    smartCardToken = smartCardTokenRepository[userId];
                }
            }

            return smartCardToken;
        }
    }
}
