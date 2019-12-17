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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="IdentityResourceManager.aspx.cs" Inherits="SynapseStudio.IdentityResourceManager" %>

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

        <h3>Identity Resources</h3>

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
                        <h3>New Resource</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgClaimName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblClaimName" runat="server" CssClass="control-label" for="txtResourceName" Text="* Please enter a name for the new Resource" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <span style="color: #c5997a; float: right;">(All non-alphanumeric characters willl be stripped out during validation)</span>
                                        </div>
                                    </div>
                                    <asp:Label ID="errClaimName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtResourceName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="Panel1" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="Label1" runat="server" CssClass="control-label" for="txtDisplayName" Text="* Please enter a Display Name" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <span style="color: #c5997a; float: right;">(All non-alphanumeric characters willl be stripped out during validation)</span>
                                        </div>
                                    </div>
                                    <asp:Label ID="Label2" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="Panel3" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="Label5" runat="server" CssClass="control-label" for="txtDescription" Text="* Please enter a Description" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <span style="color: #c5997a; float: right;">(All non-alphanumeric characters willl be stripped out during validation)</span>
                                        </div>
                                    </div>
                                    <asp:Label ID="Label6" runat="server"></asp:Label>
                                    <asp:TextBox Rows="4" Columns="10" ID="txtDescription" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="Panel2" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="Label3" runat="server" CssClass="control-label" for="chkEnabled" Text="* Enabled?" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <span style="color: #c5997a; float: right;">(All non-alphanumeric characters willl be stripped out during validation)</span>
                                        </div>
                                    </div>
                                    <asp:Label ID="Label4" runat="server"></asp:Label>
                                    <asp:CheckBox ID="chkEnabled" runat="server" Checked="true" CssClass="form-control input-lg" MaxLength="255"></asp:CheckBox>
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
                                    Resources (<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgIdentityResources" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnItemCommand="dgIdentityResource_ItemCommand">
                                    <Columns>
                                        <asp:BoundColumn DataField="Name" HeaderText="Resource Name"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="DisplayName" HeaderText="Display Name"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Description" HeaderText="Description"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="Enabled" HeaderText="Is Enabled?"></asp:BoundColumn>
                                        <asp:TemplateColumn>
                                            <ItemTemplate>
                                                <asp:Button ID="btnRemoveResource" CssClass="btn-link" OnClientClick="return confirm('Are you sure you want to toggle status?');" runat="server" Text="Toggle Status" CommandName="ToggleStatus" CommandArgument='<%# Eval("Id") +":"+  Eval("Enabled") %>' />
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
