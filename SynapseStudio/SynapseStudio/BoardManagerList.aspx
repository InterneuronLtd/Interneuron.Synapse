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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BoardManagerList.aspx.cs" Inherits="SynapseStudio.BoardManagerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>Synapse Studio <small>Boards</small></h1>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-bed"></i>&nbsp;Bed Boards (<asp:Label ID="lblBedBoardCount" runat="server" Text="0"></asp:Label>)</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Button ID="btnBedBoardDevices" runat="server" CssClass="btn btn-default pull-left" Text="Manage Devices" Width="200" OnClick="btnBedBoardDevices_Click"/>
                            </div>
                            <div class="col-md-6">
                                <asp:Button ID="btnNewBedBoard" runat="server" CssClass="btn btn-primary pull-right" Text="New Bed Board" Width="200" OnClick="btnNewBedBoard_Click"/>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgBedBoards" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundColumn DataField="bedboardname" HeaderText="Bed Board">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="bedboard_id" DataNavigateUrlFormatString="BedBoardManagerView.aspx?id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>                        
            <div class="col-md-4">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-map"></i> &nbsp;Locator Boards (<asp:Label ID="lblLocatorBoardCount" runat="server" Text="0"></asp:Label>)</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                 <asp:Button ID="ButtbtnLocatorBoardDeviceson1" runat="server" CssClass="btn btn-default pull-left" Text="Manage Devices" Width="200" OnClick="ButtbtnLocatorBoardDeviceson1_Click"/>
                            </div>
                            <div class="col-md-6">
                                <asp:Button ID="btnNewLocator" runat="server" CssClass="btn btn-primary pull-right" Text="New Locator Board" Width="200" OnClick="btnNewLocator_Click"/>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                             <asp:DataGrid ID="dgLocatorBoards" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundColumn DataField="locatorboardname" HeaderText="Locator Board">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="locatorboard_id" DataNavigateUrlFormatString="LocatorBoardManagerView.aspx?id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-street-view"></i>&nbsp;Inpatient Management Apps (<asp:Label ID="lblInpatientManagementCount" runat="server" Text="0"></asp:Label>)</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6" style="vertical-align: top;">
                            </div>
                            <div class="col-md-6">
                                <asp:Button ID="btnNewLInpatientManagement" runat="server" CssClass="btn btn-primary pull-right" Text="New Inpatient Management App" Width="250" OnClick="btnNewLInpatientManagement_Click"/>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                            <%--    <asp:DataGrid ID="dgLocalEntities" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="false">
                                    <Columns>
                                        <asp:BoundColumn DataField="entityname" HeaderText="Entity">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="entityid" DataNavigateUrlFormatString="EntityManagerAttributes.aspx?&action=view&id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
                                    </Columns>
                                </asp:DataGrid>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>


