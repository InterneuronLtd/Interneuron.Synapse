<%--
Interneuron Synapse

Copyright (C) 2019  Interneuron CIC



This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  

See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
--%>

<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="RegistrationThankYou.aspx.cs" Inherits="SynapseStudio.RegistrationThankYou" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-wrapper">



        <div class="row" style="margin-top: 40px;">
            <div class="col-md-4"></div>

            <div class="col-md-4 login-container">

                <div class="login-form">
                    <div>

                        <div style="text-align: center;">
                            <h2 style="color: #2a9fd6; font-weight: bolder;">Thank you for Registering</h2>
                        </div>


                        <div style="text-align: center;">
                            <h2>Please check your email for a link to confirm your email address</h2>
                        </div>

                        <div style="text-align: center;">
                            <h2>Your account will also need to be authorised before you can sign in. An email has been sent to your <asp:Label ID="lblAccountAdministrator" runat="server" Text="Clinician"></asp:Label> asking them to authorise your account</h2>
                        </div>

                        <br />

                        <div style="text-align: center;">
                            <a href="logout.aspx" style="font-size:2em;">Return to login page</a>
                        </div>

                    </div>

                    <div class="form-links">
                        <br />
                    </div>
                </div>
            </div>

            <div class="col-md-4"></div>


        </div>





    </div>

</asp:Content>
