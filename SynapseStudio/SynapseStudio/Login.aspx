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

<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SynapseStudio.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-wrapper">


        <div class="row" style="margin-top:40px;">
            <div class="col-md-4"></div>

            <div class="col-md-4">

                <div class="login-form" style="background-color: transparent !important;">
                    <div style="text-align: center;">
                        <%--<img src="img/SynapseStudio.png" style="max-width: 99%;" />--%>                        
                        <img src="img/Studio_White.png" style="width: 99%;"/>
                    </div>
                </div>
            </div>

            <div class="col-md-4"></div>
        </div>


        <div class="row" style="margin-top: 15px;">
            <div class="col-md-4"></div>

            <div class="col-md-4 login-container">

                <div class="login-form">
                    <div>

                        <div style="text-align: center;">
                            <h2 style="color: #2a9fd6; font-weight: bolder;">Login</h2>
                        </div>

                        <asp:Panel ID="fgtxtEmail" runat="server" CssClass="form-group">
                            <label class="control-label" for="txtEmail">Email</label>
                            <asp:TextBox ID="txtEmail" runat="server" placeholder="Email" CssClass="form-control input-lg"></asp:TextBox>
                        </asp:Panel>

                        <asp:Panel ID="fgtxtPassword" runat="server" CssClass="form-group">
                            <label class="control-label" for="txtPassword">Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" placeholder="Password" CssClass="form-control input-lg" TextMode="Password"></asp:TextBox>
                        </asp:Panel>

                        <asp:Button ID="btnLogin" runat="server" CssClass="btn btn-lg btn-primary btn-block" Text="Sign In" OnClick="btnLogin_Click" />


                        <asp:Label ID="lblError" runat="server" CssClass="contentAlertDanger"></asp:Label>

                        <asp:Button ID="btnResendValidationEmail" runat="server" Text="Resend Validation Email" CssClass="btn btn-block btn-success" OnClick="btnResendValidationEmail_Click" />

                        <br />

                        <div id="divThankYou" runat="server" class="alert alert-success">
                            <h3>Thank you</h3>
                            Validation Email Sent Again
                        </div>

                        <div id="divError" runat="server" class="alert alert-danger">
                            <h3>Error</h3>
                            Sorry, something went wrong sending the email.
                            <br />
                            <br />
                            Details:
                            <br />
                            <br />
                            <asp:Label ID="lblEmailError" runat="server"></asp:Label>
                        </div>

                        <div class="form-links">
                            <br />
                        </div>
                    </div>
                </div>

            </div>

            <div class="col-md-4">
            </div>

            <asp:HiddenField ID="hdnEmail" runat="server" />

        </div>


        <div class="row" style="margin-top: 15px;">
            <div class="col-md-4"></div>

            <div class="col-md-4">
                <div class="login-form">
                    <div>
                        <%--<a class="btn btn-default" href="RegisterAs.aspx">Register</a>--%> <a class="btn btn-default pull-right" href="ResetPassword.aspx">Reset password</a>
                    </div>
                </div>
            </div>

            <div class="col-md-4"></div>
        </div>


       

        <div class="row" style="margin-top: 30px;">
            <div class="col-md-4"></div>

            <div class="col-md-4">

                <div class="login-form">
                    <div>
                        Powered by <a href="http://www.interneuron.org" target="_blank">Interneuron CIC</a>
                    </div>
                </div>
            </div>

            <div class="col-md-4"></div>
        </div>

        <div class="hidden">
            <asp:Label ID="lblRedirect" runat="server"></asp:Label>
        </div>


    </div>
</asp:Content>
