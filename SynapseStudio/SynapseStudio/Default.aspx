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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SynapseStudio.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>Synapse Studio <small>Dashboard</small></h1>
                <div class="alert alert-dismissable alert-warning">
                    <button data-dismiss="alert" class="close" type="button">&times;</button>
                    Welcome to the admin dashboard!                    
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-bar-chart-o"></i>Core Entities (<asp:Label ID="lblCoreEntityCount" runat="server" Text="0"></asp:Label>)</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6" style="vertical-align: top;">
                            </div>
                            <div class="col-md-6">
                                <asp:Button ID="btnNewCoreEntity" runat="server" CssClass="btn btn-primary pull-right" Text="New Core Entity" Width="200" OnClick="btnNewCoreEntity_Click"/>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgCoreEnties" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="false">
                                    <Columns>
                                        <asp:BoundColumn DataField="entityname" HeaderText="Entity">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="entityid" DataNavigateUrlFormatString="EntityManagerAttributes.aspx?&action=view&id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>            
            <div class="col-md-3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-bar-chart-o"></i> Extended Entities (<asp:Label ID="lblExtendedCount" runat="server" Text="0"></asp:Label>)</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6" style="vertical-align: top;">
                            </div>
                            <div class="col-md-6">
                                <asp:Button ID="btnNewExtendedEntity" runat="server" CssClass="btn btn-primary pull-right" Text="New Extended Entity" Width="200" OnClick="btnNewExtendedEntity_Click"/>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgExtendedEntities" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="false">
                                    <Columns>
                                        <asp:BoundColumn DataField="entityname" HeaderText="Entity">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="entityid" DataNavigateUrlFormatString="EntityManagerAttributes.aspx?&action=view&id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-bar-chart-o"></i> Meta Entities (<asp:Label ID="lblMetaEntityCount" runat="server" Text="0"></asp:Label>)</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6" style="vertical-align: top;">
                            </div>
                            <div class="col-md-6">
                                <asp:Button ID="btnNewMetaEntity" runat="server" CssClass="btn btn-primary pull-right" Text="New Meta Entity" Width="200" OnClick="btnNewMetaEntity_Click"/>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgMetaEntities" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="false">
                                    <Columns>
                                        <asp:BoundColumn DataField="entityname" HeaderText="Entity">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="entityid" DataNavigateUrlFormatString="EntityManagerAttributes.aspx?&action=view&id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-bar-chart-o"></i> Local Entities (<asp:Label ID="lblLocalCount" runat="server" Text="0"></asp:Label>)</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6" style="vertical-align: top;">
                            </div>
                            <div class="col-md-6">
                                <asp:Button ID="btnNewLocalEntity" runat="server" CssClass="btn btn-primary pull-right" Text="New Local Entity" Width="200" onclick="btnNewLocalEntity_Click"/>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgLocalEntities" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="false">
                                    <Columns>
                                        <asp:BoundColumn DataField="entityname" HeaderText="Entity">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="entityid" DataNavigateUrlFormatString="EntityManagerAttributes.aspx?&action=view&id={0}" Text="View">
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

