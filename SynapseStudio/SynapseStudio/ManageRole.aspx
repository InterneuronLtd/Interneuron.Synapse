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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageRole.aspx.cs" Inherits="SynapseStudio.ManageRole" %>


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

                    <asp:HyperLink ID="lnkAddScope" runat="server" CssClass="btn btn-default btn-block" Text="Scopes" NavigateUrl="ManageRoleScope.aspx"></asp:HyperLink>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-briefcase"></i>&nbsp;<asp:Label ID="lblHeading" runat="server" Text="Local Logins"></asp:Label>
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
                        <%--<h3>Add Local Users:</h3>--%>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgClaimName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblLocalUserId" runat="server" CssClass="control-label" for="txtLocalUserId" Text="* Please enter a User-ID to search a local login" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                        </div>
                                    </div>
                                    <asp:TextBox ID="txtLocalUserId" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox><br />
                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-primary" Text="Search Local Login" Width="200" OnClick="Search" />

                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="Panel1" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">


                                            <asp:GridView ID="gvLocalLogins" runat="server" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" OnRowCommand="gvLocalLogins_RowCommand">
                                                <Columns>
                                                    <asp:BoundField DataField="username" HeaderText="User Name" ItemStyle-Width="150" />
                                                    <asp:BoundField DataField="Id" HeaderText="User Id" ItemStyle-Width="150" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Button ID="btnAdd" runat="server" CssClass="btn btn-default" Text="Add" CommandArgument='<%# Eval("Id") %>' CommandName="Add" />
                                                        </ItemTemplate>

                                                    </asp:TemplateField>
                                                </Columns>

                                            </asp:GridView>
                                        </div>
                                        <div class="col-md-6">
                                        </div>
                                    </div>


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
                                    Users in Role (<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgUsersInRole" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnItemCommand="dgUsersInRole_ItemCommand">
                                    <Columns>
                                        <asp:BoundColumn DataField="username" HeaderText="User Name">
                                            <HeaderStyle Width="45%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="Id" HeaderText="Id">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:Button ID="btnRemoveClaim" CssClass="btn-default" OnClientClick="return confirm('Are you sure you want to remove this user from role?');" runat="server" Text="Remove" CommandName="Remove" CommandArgument='<%# Eval("Id") %>' />
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
