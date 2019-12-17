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
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageRoleScope.aspx.cs" Inherits="SynapseStudio.ManageRoleScope" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-wrapper">
        <div>
            <asp:HiddenField ID="hdnEntityID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnNextOrdinalPosition" runat="server" />
            <asp:HiddenField ID="hdnDataType" runat="server" />
        </div>

        <h3>
            <asp:Label ID="lblRoleName" runat="server"></asp:Label></h3>

        <div class="well" style="margin-top: 15px;">
            <div class="row">
                <div class="col-lg-2">

                    <asp:HyperLink ID="lnkExternalLogins" runat="server" CssClass="btn btn-default btn-block" Text="Local Logins" NavigateUrl="~/ManageRole.aspx"></asp:HyperLink>
                </div>
                <div class="col-lg-2">

                    <asp:HyperLink ID="lnkLocalLogins" runat="server" CssClass="btn btn-default btn-block" Text="External Users" NavigateUrl="ManageExternalLogins.aspx"></asp:HyperLink>
                </div>
                <div class="col-lg-3">

                    <b>
                        <asp:HyperLink ID="lnkAddScope" runat="server" CssClass="btn btn-default btn-block" Text="Scopes" NavigateUrl="ManageRoleScope.aspx"></asp:HyperLink></b>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-briefcase"></i>&nbsp;<asp:Label ID="lblHeading" runat="server" Text="Api Scopes"></asp:Label>
                        </h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <h1>
                                    <asp:Label ID="lblSummaryType" runat="server"></asp:Label>
                                </h1>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgIdentityResource" runat="server" class="form-group">
                                    <asp:Label ID="lblResource" runat="server" CssClass="control-label" for="ddlResource" Text="Select an API Scope to give permissions on this role" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="errResource" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlApiScopes" DataValueField="Id" DataTextField="Name" runat="server" CssClass="form-control input-lg">
                                    </asp:DropDownList>
                                    <br />
                                    <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-primary" Text="Add To Role" Width="200" OnClick="btnAdd_Click" />
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblError" Visible="false" runat="server" CssClass="contentAlertDanger"></asp:Label>
                                <asp:Label ID="lblSuccess" Visible="false" runat="server" CssClass="contentAlertSuccess"></asp:Label>
                            </div>
                        </div>

                        <hr />

                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">
                                    <asp:Label ID="lblTestType" runat="server"></asp:Label>
                                    <asp:Label ID="lblNamespaceName" runat="server"></asp:Label>
                                    API Scopes this Role has permissions to (<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgScopesInRole" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnItemCommand="dgScopesInRole_ItemCommand">
                                    <Columns>
                                        <asp:BoundColumn DataField="Name" HeaderText="API Scope">
                                            <HeaderStyle Width="45%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Description" HeaderText="Description">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:Button ID="btnRemoveClaim" CssClass="btn-default" OnClientClick="return confirm('Are you sure you want to remove this scope from role?');" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%# Eval("Id") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
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
