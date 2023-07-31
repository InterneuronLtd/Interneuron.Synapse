//Interneuron Synapse

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


﻿using Interneuron.Terminology.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Linq;

namespace Interneuron.Terminology.API.AppCode.Infrastructure.Logging
{
    public class InterneuronAppUserClaimsSerilogEnricher : ILogEventEnricher
    {
        public const string ClientDetailsPropertyName = "ClientDetails";

        LogEventProperty _cachedProperty;
        private HttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;
        bool _isActualValue = false;
        private string _synapseRolesClaims;
        private string _scope;

        public InterneuronAppUserClaimsSerilogEnricher(IConfiguration configuration) : this(new HttpContextAccessor(), configuration)
        {
        }

        public InterneuronAppUserClaimsSerilogEnricher(HttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            _synapseRolesClaims = _configuration?.GetSection("Logs")?.GetValue<string>("TokenUserRolesClaimType");
            _synapseRolesClaims = _synapseRolesClaims ?? "SynapseRoles";

            _scope = _configuration?.GetSection("Logs")?.GetValue<string>("TokenUserScopesClaimType");
            _scope = _scope ?? "scope";
        }


        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
        {
            if (_cachedProperty == null || !_isActualValue)
                _cachedProperty = CreateProperty(propertyFactory);

            return _cachedProperty;
        }

        private LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
        {
            try
            {
                string userClaimDetails = "";

                var roleClaims = _httpContextAccessor?.HttpContext?.User?.FindAll(_synapseRolesClaims).ToList();
                var scopeClaims = _httpContextAccessor?.HttpContext?.User?.FindAll(_scope).ToList();

                if (roleClaims != null && roleClaims.Count > 0)
                {
                    var roles = roleClaims.Select(r => new { type = r.Type, value = r.Value }).ToList();
                    userClaimDetails += JsonConvert.SerializeObject(roles);
                }

                if (scopeClaims != null && scopeClaims.Count > 0)
                {
                    var scopes = scopeClaims.Select(r => new { type = r.Type, value = r.Value }).ToList();
                    userClaimDetails += JsonConvert.SerializeObject(scopes);
                }

                if (userClaimDetails != null && userClaimDetails != "")
                {
                    _isActualValue = true;
                    return propertyFactory.CreateProperty(ClientDetailsPropertyName, userClaimDetails);
                }

                return propertyFactory.CreateProperty(ClientDetailsPropertyName, null);
            }
            catch { }
            return propertyFactory.CreateProperty(ClientDetailsPropertyName, null);
        }
    }

    public class InterneuronAppUserSerilogEnricher : ILogEventEnricher
    {

        public const string UserNamePropertyName = "UserName";

        private string _claimsUserId = "IPUId";

        LogEventProperty _cachedProperty;
        private HttpContextAccessor _httpContextAccessor;
        private IConfiguration _configuration;
        bool _isActualValue = false;

        public InterneuronAppUserSerilogEnricher(IConfiguration configuration) : this(new HttpContextAccessor(), configuration)
        {

        }

        public InterneuronAppUserSerilogEnricher(HttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _claimsUserId = _configuration?.GetSection("Logs")?.GetValue<string>("TokenUserIdClaimType");
            _claimsUserId = _claimsUserId ?? "IPUId";
        }


        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(GetLogEventProperty(propertyFactory));
        }

        private LogEventProperty GetLogEventProperty(ILogEventPropertyFactory propertyFactory)
        {
            if (_cachedProperty == null || !_isActualValue)
                _cachedProperty = CreateProperty(propertyFactory);

            return _cachedProperty;
        }

        private LogEventProperty CreateProperty(ILogEventPropertyFactory propertyFactory)
        {
            try
            {
                var userId = _httpContextAccessor?.HttpContext?.User?.FindFirst(_claimsUserId);
                if (userId != null)
                {
                    _isActualValue = true;
                    return propertyFactory.CreateProperty(UserNamePropertyName, userId.Value);
                }

                return propertyFactory.CreateProperty(UserNamePropertyName, "UNKNOWN");
            }
            catch { }
            return propertyFactory.CreateProperty(UserNamePropertyName, "UNKNOWN");
        }
    }

    public static class InterneuronSerilogLoggerConfigurationExtensions
    {
        public static LoggerConfiguration WithInterneuronAppUserName(this LoggerEnrichmentConfiguration enrichmentConfiguration, IConfiguration configuration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With(new InterneuronAppUserSerilogEnricher(configuration));
        }

        public static LoggerConfiguration WithInterneuronAppClientDetails(this LoggerEnrichmentConfiguration enrichmentConfiguration, IConfiguration configuration)
        {
            if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
            return enrichmentConfiguration.With(new InterneuronAppUserClaimsSerilogEnricher(configuration));
        }
    }
}
