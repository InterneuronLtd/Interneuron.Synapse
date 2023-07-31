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
using System.Threading.Tasks;
using Interneuron.FDBAPI.Client;

namespace FDBConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            FDBAPIClient client = new FDBAPIClient("https://localhost:44316/api/fdb");

            //var result = await client.GetContraIndicationsByCode("vtm", "387458008", "");//Generic exception thrown

            //var result = await client.GetCautionsByCode("VMP", "35901911000001104", "");
            //var result = await client.GetCautionsByCode("VTM", "27658006", "");
            //var result = await client.GetCautionsByName("VMP", "Parace*", "");
            //var result = await client.GetCautionsByName("VTM", "para*", "");

            //var result = await client.GetContraIndicationsByCode("VMP", "35901911000001104", "");
            //var result = await client.GetContraIndicationsByCode("VTM", "27658006", "");
            //var result = await client.GetContraIndicationsByName("VMP", "para*", "");
            //var result = await client.GetContraIndicationsByName("VTM", "para*", "");

            //var result = await client.GetSideEffectsByCode("VMP", "35901911000001104", "");
            //var result = await client.GetSideEffectsByCode("VTM", "27658006", "");
            //var result = await client.GetSideEffectsByName("VMP", "para*", "");
            //var result = await client.GetSideEffectsByName("VTM", "para*", "");

            //var result = await client.GetSafetyMessagesByCode1("VMP", "35901911000001104", "");
            //var result = await client.GetSafetyMessagesByCode("VTM", "27658006", "");
            //var result = await client.GetSafetyMessagesByName("VMP", "para*", "");
            //var result = await client.GetSafetyMessagesByName("VTM", "para*", "");

            //var result = await client.GetLicensedUseByCode("VMP", "35901911000001104", "");
            //var result = await client.GetLicensedUseByCode("VTM", "27658006", "");
            //var result = await client.GetLicensedUseByName("VMP", "para*", "");
            //var result = await client.GetLicensedUseByName("VTM", "para*", "");

            //var result = await client.GetUnLicensedUseByCode("VMP", "35901911000001104", "");
            //var result = await client.GetUnLicensedUseByCode("VTM", "27658006", "");
            //var result = await client.GetUnLicensedUseByName("VMP", "para*", "");
            //var result = await client.GetUnLicensedUseByName("VTM", "para*", "");

            var result = await client.GetAdverseEffectsFlagByCode("134537008", "");

            Console.ReadLine();
        }
    }
}
