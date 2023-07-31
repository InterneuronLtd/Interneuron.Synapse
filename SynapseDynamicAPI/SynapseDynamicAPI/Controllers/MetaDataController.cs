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


﻿

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SynapseDynamicAPI.Models;
using SynapseDynamicAPI.Models.Meta;
using SynapseDynamicAPI.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SynapseDynamicAPI.Controllers
{
    [Authorize]
    [Route("MetaData/")]
    public class MetaDataController : ControllerBase
    {
        private BaseViewController _baseViewController;

        public MetaDataController(BaseViewController baseViewController)
        {
            _baseViewController = baseViewController;
        }

        [HttpGet]
        [Route("[action]")]
        public string GetUserPersona()
        {
            var userId = User.Claims.FirstOrDefault(cl => cl.Type.Equals("IPUId")).Value;

            List<object> baseviewPostBody = new List<object>();

            GetByPostBody getByPostBody = new GetByPostBody();

            string selectstatement = "SELECT *";
            string ordergroupbystatement = string.Empty;

            GetByPostBodyFilter filters = new GetByPostBodyFilter() {
                FilterClause = "useridentifier IN (@userid)"
            };

            GetByPostBodyFilterParam filterparams = new GetByPostBodyFilterParam() {
                ParamName = "userid",
                ParamValue = userId
            };

            baseviewPostBody.Add(new { filters = new GetByPostBodyFilter[] { filters } });
            baseviewPostBody.Add(new { filterparams = new GetByPostBodyFilterParam[] { filterparams } });
            baseviewPostBody.Add(new { selectstatement = selectstatement });
            baseviewPostBody.Add(new { ordergroupbystatement = ordergroupbystatement });

            //string personaJson = _baseViewController.GetBaseViewListByPost("meta_persona", null, null, null, null, JsonConvert.SerializeObject(baseviewPostBody));

            string personaJson = _baseViewController.GetBaseViewList("meta_persona", null, null, null, null);

            List<MetaPersonaBaseview> personaData = JsonConvert.DeserializeObject<List<MetaPersonaBaseview>>(personaJson);

            var groupedData = from data in personaData
                              group data by new
                              {
                                  personaId = data.persona_id,
                                  personaDispName = data.personadispname,
                                  personaName = data.personaname,
                                  personaDispOrder = data.personadisporder,

                              } into personas
                              select new Persona
                              {
                                  PersonaId = personas.Key.personaId,
                                  PersonaName = personas.Key.personaName,
                                  DisplayName = personas.Key.personaDispName,
                                  DisplayOrder = personas.Key.personaDispOrder,
                                  PersonaContexts = new List<PersonaContext>()
                              };

            var personaList = groupedData.ToList();

            foreach (var data in personaData)
            {
                Persona persona = personaList.First(per => per.PersonaId.Equals(data.persona_id));
                //Added condition
                bool personaContextExists = persona.PersonaContexts.Any(item => item.PersonaId == data.persona_id && item.DisplayName == data.displayname && item.ContextName == data.contextname);
                if (!personaContextExists)
                {
                    persona.PersonaContexts.Add(new PersonaContext
                    {
                        PersonaContextId = data.personacontext_id,
                        PersonaId = data.persona_id,
                        ContextName = data.contextname,
                        DisplayName = data.displayname,
                        DisplayOrder = data.displayorder
                    });
                }
            }

            return JsonConvert.SerializeObject(personaList);
        }
    }
}
