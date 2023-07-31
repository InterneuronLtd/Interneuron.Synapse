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
﻿

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNetCore.Identity;

namespace SynapseStudio
{
    public partial class AddUsers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.lblError.Text = string.Empty;
                this.lblSuccess.Text = string.Empty;
                this.lblError.Visible = false;
                this.lblSuccess.Visible = false;
            }
        }

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            AddUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearAttributesForm();
        }

        private void AddUser()
        {
            try
            {
                string userId = Guid.NewGuid().ToString();

                string hashedPassword = string.Empty;

                PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
                IdentityUser user = new IdentityUser(this.txtUsername.Text);
                hashedPassword = passwordHasher.HashPassword(user, this.txtPassword.Text);

                //using (SHA256 shA256 = SHA256.Create())
                //{
                //    byte[] bytes = Encoding.UTF8.GetBytes(this.txtPassword.Text);
                //    hashedPassword = Convert.ToBase64String(((HashAlgorithm)shA256).ComputeHash(bytes));
                //}

                string sql = "INSERT INTO \"AspNetUsers\"(\"Id\", \"UserName\", \"NormalizedUserName\", \"Email\", \"NormalizedEmail\", \"EmailConfirmed\", \"PasswordHash\", \"SecurityStamp\", \"ConcurrencyStamp\", \"PhoneNumberConfirmed\",                    \"TwoFactorEnabled\", \"LockoutEnabled\", \"AccessFailedCount\") "
                               + " VALUES(@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, false, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, false, false, false, 0)";

                var paramList = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("Id", userId),
                    new KeyValuePair<string, string>("Username", this.txtUsername.Text),
                    new KeyValuePair<string, string>("NormalizedUserName", this.txtUsername.Text.ToUpper()),
                    new KeyValuePair<string, string>("Email", this.txtEmail.Text),
                    new KeyValuePair<string, string>("NormalizedEmail", this.txtEmail.Text.ToUpper()),
                    new KeyValuePair<string, string>("PasswordHash", hashedPassword),
                    new KeyValuePair<string, string>("SecurityStamp", Guid.NewGuid().ToString()),
                    new KeyValuePair<string, string>("ConcurrencyStamp", Guid.NewGuid().ToString())
                };

                DataServices.ExcecuteNonQueryFromSQL(sql, paramList, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);

                string query = "INSERT INTO \"AspNetUserClaims\" (\"UserId\", \"ClaimType\", \"ClaimValue\") " +
                               "VALUES(@UserId, @ClaimType, @ClaimValue), " +
                               "(@UserId, @ClaimType1, @ClaimValue1), " +
                               "(@UserId, @ClaimType2, @ClaimValue2), " +
                               "(@UserId, @ClaimType3, @ClaimValue3), " +
                               "(@UserId, @ClaimType4, @ClaimValue4);";

                var parameters = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("UserId", userId),
                    new KeyValuePair<string, string>("ClaimType", "name"),
                    new KeyValuePair<string, string>("ClaimValue", this.txtFirstName.Text + ' ' + this.txtLastName.Text),
                    new KeyValuePair<string, string>("ClaimType1", "given_name"),
                    new KeyValuePair<string, string>("ClaimValue1", this.txtFirstName.Text),
                    new KeyValuePair<string, string>("ClaimType2", "family_name"),
                    new KeyValuePair<string, string>("ClaimValue2", this.txtLastName.Text),
                    new KeyValuePair<string, string>("ClaimType3", "email"),
                    new KeyValuePair<string, string>("ClaimValue3", this.txtEmail.Text),
                    new KeyValuePair<string, string>("ClaimType4", "email_verified"),
                    new KeyValuePair<string, string>("ClaimValue4", "true")
                };

                DataServices.ExcecuteNonQueryFromSQL(query, parameters, dbConnection: SynapseHelpers.DBConnections.PGSQLConnectionSIS);

                ClearAttributesForm();
                this.lblSuccess.Text = "New user added";
                this.lblSuccess.Visible = true;

            }
            catch (Exception ex)
            {
                this.lblError.Text = ex.ToString();
                this.lblError.Visible = true;
                return;
            }
        }

        private void ClearAttributesForm()
        {
            this.lblError.Text = string.Empty;
            this.lblSuccess.Text = string.Empty;
            this.lblError.Visible = false;
            this.lblSuccess.Visible = false;
            this.txtFirstName.Text = string.Empty;
            this.txtLastName.Text = string.Empty;
            this.txtEmail.Text = string.Empty;
            this.txtUsername.Text = string.Empty;
            this.txtPassword.Text = string.Empty;
        }
    }
}