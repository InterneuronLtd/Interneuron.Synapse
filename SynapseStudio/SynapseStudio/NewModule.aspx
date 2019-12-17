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
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewModule.aspx.cs" Inherits="SynapseStudio.NewModule" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Module"></asp:Label>
                </h1>
            </div>
        </div>

        <div>
            <asp:HiddenField ID="hdnClientID" runat="server" />
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-database"></i>&nbsp;New Module</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <h3 class="panel-title" style="font-weight: bold; font-size: 2em;">New Module</h3>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                              
                                <asp:Panel ID="pnlClientId" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblmodulename" runat="server" CssClass="control-label" Text="* Please enter module name" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtmodulename" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ControlToValidate="txtmodulename" ID="rfvClientId" runat="server" ErrorMessage="Please enter Application" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="Panel1" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblmoduledescription" runat="server" CssClass="control-label" Text="* Please enter module description" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtmoduledescription" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ControlToValidate="txtmoduledescription" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter Application" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                 <asp:Panel ID="Panel2" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lbljsurl" runat="server" CssClass="control-label" Text="* Please enter URL" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtjsurl" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ControlToValidate="txtjsurl" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Please enter Application" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                 <asp:Panel ID="Panel3" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lbldomselector" runat="server" CssClass="control-label" Text="* Please enter DOM selector" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtdomselector" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ControlToValidate="txtdomselector" ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter Application" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                               
                                 
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" />
                                <asp:Button ID="btnAddNewModule" runat="server" CssClass="btn btn-primary pull-right" Text="Add new Module" Width="200"  ValidationGroup="validate" OnClick="btnAddNewModule_Click"  />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblError" runat="server" CssClass="contentAlertDanger"></asp:Label>
                                <asp:Label ID="lblSuccess" runat="server" CssClass="contentAlertSuccess"></asp:Label>
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>



    </div>
</asp:Content>
