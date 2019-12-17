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

<%@ Page Title="Client secrets" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientSecrets.aspx.cs" Inherits="SynapseStudio.ClientSecrets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">

        <div>
            <asp:HiddenField ID="hdnClientID" runat="server" />
        </div>

        <h3><asp:Label ID="lblH3Client" runat="server"></asp:Label><asp:Label ID="lblH3Secrets" runat="server" Text=" secrets"></asp:Label></h3>

        <div class="well" style="margin-top: 15px;">
            <div class="row">
                <div class="col-lg-3">
                    <asp:Button ID="btnClientClaims" runat="server" CssClass="btn btn-default btn-block" Text="Client claims" OnClick="btnClientClaims_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnClientGrantTypes" runat="server" CssClass="btn btn-default btn-block" Text="Client grant types" OnClick="btnClientGrantTypes_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnClientRedirectUris" runat="server" CssClass="btn btn-default btn-block" Text="Client redirect URI's" OnClick="btnClientRedirectUris_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnClientScopes" runat="server" CssClass="btn btn-default btn-block" Text="Client scopes" OnClick="btnClientScopes_Click" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-tasks"></i>&nbsp;
                            <asp:Label ID="lblClient" runat="server"></asp:Label>
                            <asp:Label ID="lblHeading" runat="server" Text="secrets"></asp:Label>
                        </h3>
                    </div>
                    <div class="panel-body">
                        <h3>New client secret</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlType" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblValue" runat="server" CssClass="control-label" for="txtValue" Text="* Please enter value for client secret" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtValue" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ControlToValidate="txtValue" ID="rfvValue" runat="server" ErrorMessage="Please enter value" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlValue" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblType" runat="server" CssClass="control-label" for="txtType" Text="* Please enter type for client secret" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtType" runat="server" CssClass="form-control input-lg" Text="SharedSecret" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ControlToValidate="txtType" ID="rfvType" runat="server" ErrorMessage="Please enter type" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnAddClientSecret" runat="server" CssClass="btn btn-primary pull-right" Text="Add " Width="200" OnClick="btnAddClientSecret_Click" ValidationGroup="validate" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblError" runat="server" CssClass="contentAlertDanger"></asp:Label>
                                <asp:Label ID="lblSuccess" runat="server" CssClass="contentAlertSuccess"></asp:Label>
                            </div>
                        </div>

                        <hr />

                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">
                                    <asp:Label ID="lblClientName" runat="server"></asp:Label>
                                    secrets (<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgClientSecret" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnItemCommand="dgClientSecret_ItemCommand" OnItemDataBound="dgClientSecret_ItemDataBound">
                                    <Columns>
                                        <asp:BoundColumn DataField="Id" HeaderText="Id">
                                            <HeaderStyle Width="0%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Value" HeaderText="Value">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Type" HeaderText="Type">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn ButtonType="LinkButton" CommandName="Remove" Text="Remove">
                                            <HeaderStyle Width="30%" />
                                        </asp:ButtonColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
