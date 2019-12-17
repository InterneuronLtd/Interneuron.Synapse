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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DevOpsDashboard.aspx.cs" Inherits="SynapseStudio.DevOpsDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>DevOps <small>Dashboard</small></h1>
                <div class="alert alert-dismissable alert-info">
                    <button data-dismiss="alert" class="close" type="button">&times;</button>
                   Note: You must manually refresh this dashboard. The data provided for this dashboard comes from a second connection to the PostgresSQL system database which is only initialised when you use the Refresh button below.                   
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12" style="margin-bottom:10px;">
            <asp:Button ID="btnRefreshDashboard" runat="server" CssClass="btn btn-primary pull-right"  Text="Refresh Dashboard" Width="200" OnClick="btnRefreshDashboard_Click"/>
            <asp:Label ID="lblDashboardRefreshed" runat="server" Text="Dashboard has not been refreshed yet."></asp:Label>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-bar-chart-o"></i> Synapse Database Activity</h3>
                    </div>
                    <div class="panel-body">

                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dataGridDatabaseActivity" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundColumn DataField="pid" HeaderText="PID">
                                            <HeaderStyle Width="5%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="usename" HeaderText="User">
                                            <HeaderStyle Width="5%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="application_name" HeaderText="Application">
                                            <HeaderStyle Width="5%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="client_addr" HeaderText="Client Addr.">
                                            <HeaderStyle Width="5%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="query" HeaderText="Query">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="query_start" HeaderText="Query Start">
                                            <HeaderStyle Width="10%" />
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>            
        </div>
        <div class="row">
            <div class="col-md-12" style="margin-bottom:10px;">
            <asp:Button ID="Button1" runat="server" CssClass="btn btn-primary pull-right"  Text="Refresh Dashboard" Width="200" OnClick="btnRefreshDashboard_Click"/>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-bar-chart-o"></i> PostgreSQL HA Status</h3>
                    </div>
                    <div class="panel-body">

                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgReplStatus" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="True" ShowHeader="True">
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>            
        </div>

    </div>

</asp:Content>

