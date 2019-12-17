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

<%@ Page Title="Client redirect URI's" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientRedirectUris.aspx.cs" Inherits="SynapseStudio.ClientRedirectUris" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">

        <div>
            <asp:HiddenField ID="hdnClientID" runat="server" />
        </div>

        <h3><asp:Label ID="lblH3Client" runat="server"></asp:Label><asp:Label ID="lblH3RedirectUri" runat="server" Text=" redirect URI's"></asp:Label></h3>

        <div class="well" style="margin-top: 15px;">
            <div class="row">
                <div class="col-lg-3">
                    <asp:Button ID="btnClientClaims" runat="server" CssClass="btn btn-default btn-block" Text="Client claims" OnClick="btnClientClaims_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnClientGrantTypes" runat="server" CssClass="btn btn-default btn-block" Text="Client grant types" OnClick="btnClientGrantTypes_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnClientScopes" runat="server" CssClass="btn btn-default btn-block" Text="Client scopes" OnClick="btnClientScopes_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnClientSecrets" runat="server" CssClass="btn btn-default btn-block" Text="Client secrets" OnClick="btnClientSecrets_Click" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-tasks"></i>&nbsp;
                            <asp:Label ID="lblClient" runat="server"></asp:Label>
                            <asp:Label ID="lblHeading" runat="server" Text="redirect URI's"></asp:Label>
                        </h3>
                    </div>
                    <div class="panel-body">
                        <h3>New client redirect URI</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlRedirectUri" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblRedirectUri" runat="server" CssClass="control-label" for="txtRedirectUri" Text="* Please enter a redirect URI for client" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtRedirectUri" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvRedirectUri" runat="server" ControlToValidate="txtRedirectUri" ErrorMessage="Please enter redirect URI" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlURIDropdown" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblURIType" runat="server" CssClass="control-label" Text="* Please select a URI type" Font-Bold="true"></asp:Label>
                                            <asp:DropDownList ID="ddlURIType" runat="server" CssClass="form-control input-lg">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvURIType" runat="server" ControlToValidate="ddlURIType" ErrorMessage="Please select an URI type" InitialValue="Please select ..." ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnAddClientRedirectURI" runat="server" CssClass="btn btn-primary pull-right" Text="Add " Width="200" OnClick="btnAddClientRedirectURI_Click" ValidationGroup="validate" />
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
                                    redirect URI's (<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgClientRedirectURI" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnItemCommand="dgClientRedirectURI_ItemCommand" OnItemDataBound="dgClientRedirectURI_ItemDataBound">
                                    <Columns>
                                        <asp:BoundColumn DataField="Id" HeaderText="Id">
                                            <HeaderStyle Width="0%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="URI" HeaderText="URI">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="URIType" HeaderText="URI type">
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
