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
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Newapplication.aspx.cs" Inherits="SynapseStudio.Newapplication"  Async="true"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Applications"></asp:Label>
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
                        <h3 class="panel-title"><i class="fa fa-database"></i>&nbsp;New Application</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <h3 class="panel-title" style="font-weight: bold; font-size: 2em;">New Application</h3>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                              
                                <asp:Panel ID="pnlClientId" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblApplication" runat="server" CssClass="control-label" Text="* Please enter Application Name" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtApplication" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ControlToValidate="txtApplication" ID="rfvClientId" runat="server" ErrorMessage="Please enter Application" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                               
                               
                                 
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" />
                                <asp:Button ID="btnAddNewApplication" runat="server" CssClass="btn btn-primary pull-right" Text="Add new Application" Width="200"  ValidationGroup="validate" OnClick="btnAddNewApplication_Click" />
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

