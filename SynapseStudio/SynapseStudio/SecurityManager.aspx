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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurityManager.aspx.cs" Inherits="SynapseStudio.SecurityManager" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Synapse Security"></asp:Label>
                </h1>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-laptop"></i>&nbsp;Client Apps</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCreateNewEntity" runat="server" CssClass="btn btn-primary pull-right" Text="New Client" Width="200" OnClick="btnCreateNewEntity_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">Client Apps (<asp:Label ID="lblClientCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgClients" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">
                                    <Columns>
                                       <asp:HyperLinkColumn DataNavigateUrlField="clientid" HeaderText="Client ID" DataNavigateUrlFormatString="Clients.aspx?&action=view&id={0}" DataTextField="clientid" >
                                        <HeaderStyle Width="50%" />
                                      
                                       </asp:HyperLinkColumn>
                                       
                                        <asp:BoundColumn DataField="enabled" HeaderText="Enabled">
                                            <HeaderStyle Width="25%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="Id" DataNavigateUrlFormatString="ClientGrantTypes.aspx?&action=view&id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-briefcase"></i>&nbsp;Identity Claims</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnManageIdentityClaims" runat="server" CssClass="btn btn-primary pull-right" Text="Manage Claims" Width="200" OnClick="btnManageIdentityClaims_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                    <asp:Label ID="Label2" runat="server"></asp:Label>
                                    Identity Claims (<asp:Label ID="lblIdentityClaimCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgIdentityClaims" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="claimname" HeaderText="Claim">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="resourcename" HeaderText="Resource">
                                            <HeaderStyle Width="45%" />
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="col-lg-3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-server"></i>&nbsp;API Scopes</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnManageAPIs" runat="server" CssClass="btn btn-primary pull-right" Text="Manage APIs" Width="200" OnClick="btnManageAPIs_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">API Scopes (<asp:Label ID="lblAPICount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgAPIs" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="scopename" HeaderText="Scope">
                                            <HeaderStyle Width="25%" />
                                        </asp:BoundColumn>
                                         <asp:BoundColumn DataField="scopedescription" HeaderText="Description">
                                            <HeaderStyle Width="50%" />
                                        </asp:BoundColumn>
                                         <asp:BoundColumn DataField="resourcename" HeaderText="API Resource">
                                            <HeaderStyle Width="25%" />
                                        </asp:BoundColumn>
                                    </Columns>

                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-user"></i>&nbsp;Roles</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAddRole" runat="server" CssClass="btn btn-primary pull-right" Text="Add Role" Width="200" OnClick="btnAddRole_Click" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAddUser" runat="server" CssClass="btn btn-primary pull-right" Text="Add local user" Width="200" OnClick="btnAddUser_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">Roles (<asp:Label ID="lblRoleCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgRoles" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="rolename" HeaderText="Role Name">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="id" DataNavigateUrlFormatString="ManageRole.aspx?&action=view&id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
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
