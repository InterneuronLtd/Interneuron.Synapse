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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IdentityClaimsManager.aspx.cs" Inherits="SynapseStudio.IdentityClaimsManager" %>

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

        <h3>Identity Claims</h3>

        <div class="well" style="margin-top: 15px;">
            <div class="row">
                <div class="col-lg-2">

                    <asp:HyperLink ID="lnkIdentityClaims" runat="server" CssClass="btn btn-default btn-block" Text="Identity Claims" NavigateUrl="IdentityClaimsManager.aspx"></asp:HyperLink>
                </div>
                <div class="col-lg-3">

                    <asp:HyperLink ID="lnkIdentityResources" runat="server" CssClass="btn btn-default btn-block" Text="Identity Resources" NavigateUrl="IdentityResourceManager.aspx"></asp:HyperLink>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-briefcase"></i>&nbsp;<asp:Label ID="lblHeading" runat="server" Text="Claims"></asp:Label>
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
                        <h3>New Claim</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgClaimName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblClaimName" runat="server" CssClass="control-label" for="txtClaimName" Text="* Please enter a name for the new claim" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <span style="color: #c5997a; float: right;">(All non-alphanumeric characters willl be stripped out during validation)</span>
                                        </div>
                                    </div>
                                    <asp:Label ID="errClaimName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtClaimName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgIdentityResource" runat="server" class="form-group">
                                    <asp:Label ID="lblResource" runat="server" CssClass="control-label" for="ddlResource" Text="Select an Identity Resource" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="errResource" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlResource" DataTextField="name" DataValueField="resourceid" runat="server" CssClass="form-control input-lg">
                                    </asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnValidateAndCreate" runat="server" CssClass="btn btn-primary pull-right" Text="Validate and Create" Width="200" OnClick="btnValidateAndCreate_Click" />


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
                                    Claims (<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgIdentityClaims" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnItemCommand="dgIdentityClaims_ItemCommand">
                                    <Columns>
                                        <asp:BoundColumn DataField="claimname" HeaderText="Claim">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="resourcename" HeaderText="Resource">
                                            <HeaderStyle Width="45%" />
                                        </asp:BoundColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:Button ID="btnRemoveClaim" CssClass="btn-link" OnClientClick="return confirm('Are you sure you want to delete this claim?');" runat="server" Text="Delete" CommandName="RemoveClaim" CommandArgument='<%# Eval("claimid") %>' />
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
