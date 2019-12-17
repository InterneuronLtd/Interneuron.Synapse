// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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

using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Synapse.STS.UI;

namespace Synapse.STS.UI
{
    /// <summary>
    /// This sample controller allows a user to revoke grants given to clients
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    public class GrantsController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clients;
        private readonly IResourceStore _resources;

        public GrantsController(IIdentityServerInteractionService interaction,
            IClientStore clients,
            IResourceStore resources)
        {
            _interaction = interaction;
            _clients = clients;
            _resources = resources;
        }

        /// <summary>
        /// Show list of grants
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("Index", await BuildViewModelAsync());
        }

        /// <summary>
        /// Handle postback to revoke a client
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke(string clientId)
        {
            await _interaction.RevokeUserConsentAsync(clientId);
            return RedirectToAction("Index");
        }

        private async Task<GrantsViewModel> BuildViewModelAsync()
        {
            var grants = await _interaction.GetAllUserConsentsAsync();

            var list = new List<GrantViewModel>();
            foreach(var grant in grants)
            {
                var client = await _clients.FindClientByIdAsync(grant.ClientId);
                if (client != null)
                {
                    var resources = await _resources.FindResourcesByScopeAsync(grant.Scopes);

                    var item = new GrantViewModel()
                    {
                        ClientId = client.ClientId,
                        ClientName = client.ClientName ?? client.ClientId,
                        ClientLogoUrl = client.LogoUri,
                        ClientUrl = client.ClientUri,
                        Created = grant.CreationTime,
                        Expires = grant.Expiration,
                        IdentityGrantNames = resources.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                        ApiGrantNames = resources.ApiResources.Select(x => x.DisplayName ?? x.Name).ToArray()
                    };

                    list.Add(item);
                }
            }

            return new GrantsViewModel
            {
                Grants = list
            };
        }
    }
}