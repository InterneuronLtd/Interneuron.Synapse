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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddUsers.aspx.cs" Inherits="SynapseStudio.AddUsers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div>
            <asp:HiddenField ID="hdnClientID" runat="server" />
        </div>
        <h3>
            <asp:Label ID="lblH3AddUsers" runat="server" Text="Add local user"></asp:Label>
        </h3>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-tasks"></i>&nbsp;
                            <asp:Label ID="lblHeading" runat="server" Text="Add local user"></asp:Label>
                        </h3>
                    </div>
                    <div class="panel-body">
                        <h3>Local user registration</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlFirstName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblFirstName" runat="server" CssClass="control-label" for="txtFirstName" Text="First name" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlLastName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblLastName" runat="server" CssClass="control-label" for="txtLastName" Text="Last name" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlEmail" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblEmail" runat="server" CssClass="control-label" for="txtEmail" Text="Email" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlUserName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblUserName" runat="server" CssClass="control-label" for="txtUserName" Text="* Username (This Username will be used for login)" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ControlToValidate="txtUserName" ID="rfvUserName" runat="server" ErrorMessage="Please enter username" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlPassword" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblPassword" runat="server" CssClass="control-label" for="txtPassword" Text="* Password" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ControlToValidate="txtPassword" ID="rfvPassword" runat="server" ErrorMessage="Please enter password" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnAddUser" runat="server" CssClass="btn btn-primary pull-right" Text="Add user" Width="200"  ValidationGroup="validate" OnClick="btnAddUser_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Label ID="lblError" runat="server" CssClass="contentAlertDanger"></asp:Label>
                                <asp:Label ID="lblSuccess" runat="server" CssClass="contentAlertSuccess"></asp:Label>
                            </div>
                        </div>
                        <hr />
                        <br />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
