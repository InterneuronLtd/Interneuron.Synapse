//Interneuron Synapse

//Copyright(C) 2018  Interneuron CIC

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SynapseDynamicAPI.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;



namespace SynapseDynamicAPI.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes)]
    public class AuthController : Controller
    {
        private const string AuthSchemes =
        CookieAuthenticationDefaults.AuthenticationScheme + "," +
        JwtBearerDefaults.AuthenticationScheme;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [HttpPost]
        [Route("Auth/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok("Successfully logged out.");
        }

        //[HttpPost]
        //[Route("Auth/Logout")]
        //public async Task<IActionResult> Login(LoginView model, string returnUrl = null)
        //{
        //    ViewData["ReturnUrl"] = returnUrl;
        //    if (ModelState.IsValid)
        //    {

        //        //var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
        //        //if (result.Succeeded)
        //        //{
        //        //    return Ok(returnUrl);
        //        //}

        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}



        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/ValidateADUser")]
        public bool ValidateUser([FromBody] ADModel admodel)
        {

            return true;

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/GenerateToken")]
        public async Task<IActionResult> GenerateToken([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(loginModel.Email);

                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);
                    if (result.Succeeded)
                    {

                        Claim[] claims = new[]
                        {
                          new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                          new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                        string token_key = _config["Tokens:Key"];
                        string token_issuer = _config["Tokens:Issuer"];
                        double token_timeout = Double.Parse(_config["Tokens:Timeout"]);

                        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(token_key));
                        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        JwtSecurityToken token = new JwtSecurityToken(token_issuer,
                          token_issuer,
                          claims,
                          expires: DateTime.Now.AddMinutes(token_timeout),
                          signingCredentials: creds);

                        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                    }
                }
            }

            return BadRequest("Could not generate token. Are your credentials correct?");
        }

        protected string SynapseAPIUser()
        {
            ClaimsPrincipal principal = HttpContext.User;
            ////Get Claims
            //if (principal?.Claims != null)
            //{
            //    foreach (var claim in principal.Claims)
            //    {
            //        log.Debug($"CLAIM TYPE: {claim.Type}; CLAIM VALUE: {claim.Value}");
            //    }

            //}
            return principal?.Claims?.SingleOrDefault(p => p.Type == "username")?.Value;
        }
    }

    public class ADModel {
        public string domainName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
