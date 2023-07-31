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
using System;
using Hl7.Fhir.Model;

namespace Interneuron.CareRecord.API.AppCode.Extensions
{
    public static class Hl7ModelExtensions
    {
        public static Type GetHl7ModelType(this string type)
        {
            if (type.IsNotEmpty() && type.EqualsIgnoreCase("patient"))
            {
                return typeof(Patient);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("encounter"))
            {
                return typeof(Encounter);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("observation"))
            {
                return typeof(Observation);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("procedure"))
            {
                return typeof(Procedure);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("report"))
            {
                return typeof(DiagnosticReport);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("result"))
            {
                return typeof(DiagnosticReport);
            }

            if (type.IsNotEmpty() && type.EqualsIgnoreCase("diagnosticreport"))
            {
                return typeof(DiagnosticReport);
            }


            return null;
        }

        //public static IEnumerable<Type> GetDomainModelTypesForHl7Type(this Type hl7ResourceType)
        //{

        //    if (hl7ResourceType == typeof(Patient))
        //    {
        //        yield return typeof(entitystorematerialised_CorePersonidentifier);
        //        yield return typeof(entitystorematerialised_CorePersonaddress1);
        //        yield return typeof(entitystorematerialised_CorePersoncontactinfo1);
        //        yield return typeof(entitystorematerialised_CoreNextofkin1);

        //    }

        //    if (hl7ResourceType == typeof(Encounter))
        //    {
        //        yield return typeof(entitystorematerialised_CorePersonidentifier);
        //    }
        //}

        //public static IEnumerable<EntityBase> GetDomainModelServiceForHl7Type(this Type hl7ResourceType, IServiceProvider provider)
        //{

        //    if (hl7ResourceType == typeof(Patient))
        //    {
        //        yield return provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersonidentifier>)) as ISynapseResourceService<entitystorematerialised_CorePersonidentifier> as EntityBase;
        //        yield return provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersonaddress1>)) as ISynapseResourceService<entitystorematerialised_CorePersonaddress1> as EntityBase;
        //        yield return provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersoncontactinfo1>)) as ISynapseResourceService<entitystorematerialised_CorePersoncontactinfo1> as EntityBase;
        //        yield return provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CoreNextofkin1>)) as ISynapseResourceService<entitystorematerialised_CoreNextofkin1> as EntityBase;
        //    }

        //    if (hl7ResourceType == typeof(Encounter))
        //    {
        //        yield return provider.GetService(typeof(ISynapseResourceService<entitystorematerialised_CorePersonidentifier>)) as ISynapseResourceService<entitystorematerialised_CorePersonidentifier> as EntityBase;
        //    }
        //}

        
    }
}
