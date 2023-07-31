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
﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SynapseStudioWeb.Models
{
    public class ClientModel
    {
        public string ErrorMessage { get; set; }

        public bool Enabled = true;
        [DisplayName("* Please enter a client id")]
        [Required(ErrorMessage = "Please enter a client id")]
        public string ClientId { get; set; }

        [DisplayName("Require client secret")]
        public bool ClientSecret { get; set; }
        [DisplayName("Please enter a client name")]
        public string ClientName { get; set; }

        [DisplayName("Please enter a client description")]
        public string Description { get; set; }
        [DisplayName("Require consent")]
        public bool Requireconsent { get; set; }
        [DisplayName("Allow access tokens via browser")]
        public bool accesstokensbrowser { get; set; }
        [DisplayName("Allow offline access")]
        public bool OfflineAccess { get; set; } = true;
        [DisplayName("Identity token lifetime")]
        public string IdentityToken { get; set; } = "1800";
        [DisplayName("Access token lifetime")]
        public string AccessToken { get; set; } = "1800";
        [DisplayName("Authorization code lifetime")]
        public string AuthorizationCode { get; set; } = "1800";
        [DisplayName("Consent lifetime")]
        public string ConsentLifetime { get; set; } = "1800";
        [DisplayName("Absolute refresh token lifetime")]
        public string AbsoluteRefreshToken { get; set; } = "2592000";
        [DisplayName("Sliding refresh token lifetime")]
        public string SlidingRefreshToken { get; set; } = "1296000";
        [DisplayName("Refresh token usage")]
        public bool RefreshTokenUsage { get; set; } = true;
        [DisplayName("Update access token claims on refresh")]
        public bool UpdateAccessToken { get; set; } = true;
        [DisplayName("Refresh token expiration")]
        public bool RefreshTokenExpiration { get; set; } = true;
        [DisplayName("Enable local login")]
        public bool EnableLocalLogin { get; set; } = true;
        [DisplayName("Always send client claims")]
        public bool AlwaysSendClientClaims { get; set; }
        [DisplayName("Client claims prefix")]
        public string ClientClaimsPrefix { get; set; }

        public string posttype { get; set; }
    }
}
