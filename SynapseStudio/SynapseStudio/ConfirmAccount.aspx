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

<%@ Page Title="" Language="C#" MasterPageFile="~/Login.Master" AutoEventWireup="true" CodeBehind="ConfirmAccount.aspx.cs" Inherits="SynapseStudio.ConfirmAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-wrapper">

        <div class="row" style="margin-top: 40px;">
            <div class="col-md-4"></div>

            <div class="col-md-4 login-container">

                <div class="login-form">
                    <div>


                        <asp:Panel ID="pnlSuccess" runat="server">
                            <div class="alert alert-success">
                                <h3>Thank you</h3>
                                You account has been confirmed.
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnlFailed" runat="server">
                            <div class="alert alert-danger">
                                <h3>Sorry</h3>
                                Invalid Confirmation Code, please ckeck and try again
                            </div>
                        </asp:Panel>

                        <asp:Panel ID="pnlAlreadyConfirmed" runat="server">
                            <div class="alert alert-warning">
                                <h3>Already Confirmed</h3>
                                Your account has already been confirmed
                            </div>
                        </asp:Panel>




                        <div class="form-links">                          
                        </div>
                    </div>
                </div>

            </div>

            <div class="col-md-4">
            </div>


        </div>

        <div class="row" style="margin-top: 20px;">
            <div class="col-md-4"></div>

            <div class="col-md-4">
                <a class="btn btn-default" href="Login.aspx">Click here to login</a>
            </div>

            <div class="col-md-4"></div>
        </div>







    </div>
</asp:Content>

