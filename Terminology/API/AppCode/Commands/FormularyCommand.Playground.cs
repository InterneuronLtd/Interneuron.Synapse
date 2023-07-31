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
﻿using Interneuron.Common.Extensions;
using Interneuron.FDBAPI.Client.DataModels;
using Interneuron.Terminology.API.AppCode.DTOs;
using Interneuron.Terminology.Infrastructure.Domain;
using Interneuron.Terminology.Model.DomainModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Commands
{
    public partial class FormularyCommand : IFormularyCommands
    {

        public void ChangeFDBDataSchema()
        {
            var repo = this._provider.GetService(typeof(IRepository<FormularyDetail>)) as IRepository<FormularyDetail>;

            var formularyDetails = repo.Items;

            if (formularyDetails.IsCollectionValid())
            {
                formularyDetails.Each(rec => {

                    if (rec.UnlicensedUse.IsNotEmpty())
                    {
                        var currentModels = JsonConvert.DeserializeObject<List<FDBIdText>>(rec.UnlicensedUse);

                        if (currentModels.IsCollectionValid())
                        {
                            var newModels = currentModels.Select(fdbRec => new FormularyLookupItemDTO { Cd = fdbRec.Id, Desc = fdbRec.Text, Source = TerminologyConstants.FDB_DATA_SRC });

                            var dataAsString = JsonConvert.SerializeObject(newModels);
                            rec.UnlicensedUse = dataAsString;
                        }
                    }

                    //if (rec.LicensedUse.IsNotEmpty())
                    //{
                    //    var currentModels = JsonConvert.DeserializeObject<List<FDBIdText>>(rec.LicensedUse);

                    //    if (currentModels.IsCollectionValid())
                    //    {
                    //        var newModels = currentModels.Select(fdbRec => new FormularyLookupItemDTO { Cd = fdbRec.Id, Desc = fdbRec.Text, Source = TerminologyConstants.FDB_DATA_SRC });

                    //        var dataAsString = JsonConvert.SerializeObject(newModels);
                    //        rec.LicensedUse = dataAsString;
                    //    }
                    //}

                    //if (rec.SafetyMessage.IsNotEmpty())
                    //{
                    //    var currentModels = JsonConvert.DeserializeObject<List<string>>(rec.SafetyMessage);

                    //    if (currentModels.IsCollectionValid())
                    //    {
                    //        var newModels = currentModels.Select(fdbRec => new FormularyLookupItemDTO { Cd = fdbRec, Desc = fdbRec, Source = TerminologyConstants.FDB_DATA_SRC });

                    //        var dataAsString = JsonConvert.SerializeObject(newModels);
                    //        rec.SafetyMessage = dataAsString;
                    //    }
                    //}

                    //if (rec.SideEffect.IsNotEmpty())
                    //{
                    //    var currentModels = JsonConvert.DeserializeObject<List<string>>(rec.SideEffect);

                    //    if (currentModels.IsCollectionValid())
                    //    {
                    //        var newModels = currentModels.Select(fdbRec => new FormularyLookupItemDTO { Cd = fdbRec, Desc = fdbRec, Source = TerminologyConstants.FDB_DATA_SRC });

                    //        var dataAsString = JsonConvert.SerializeObject(newModels);
                    //        rec.SideEffect = dataAsString;
                    //    }
                    //}

                    //if (rec.Caution.IsNotEmpty())
                    //{
                    //    var currentModels = JsonConvert.DeserializeObject<List<string>>(rec.Caution);

                    //    if (currentModels.IsCollectionValid())
                    //    {
                    //        var newModels = currentModels.Select(fdbRec => new FormularyLookupItemDTO { Cd = fdbRec, Desc = fdbRec, Source = TerminologyConstants.FDB_DATA_SRC });

                    //        var dataAsString = JsonConvert.SerializeObject(newModels);
                    //        rec.Caution = dataAsString;
                    //    }
                    //}

                    //if (rec.ContraIndication.IsNotEmpty())
                    //{
                    //    var currentModels = JsonConvert.DeserializeObject<List<FDBAPI.Client.DataModels.FDBIdText>>(rec.ContraIndication);

                    //    if (currentModels.IsCollectionValid())
                    //    {
                    //        var newModels = currentModels.Select(fdbRec => new FormularyLookupItemDTO { Cd = fdbRec.Id, Desc = fdbRec.Text, Source= TerminologyConstants.FDB_DATA_SRC });

                    //        var contraIndicationAsString = JsonConvert.SerializeObject(newModels);
                    //        rec.ContraIndication = contraIndicationAsString;
                    //    }
                    //}

                });
            }

            repo.SaveChanges();
        }
    }
}
