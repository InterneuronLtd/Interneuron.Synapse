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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocatorBoardDeviceList.aspx.cs" Inherits="SynapseStudio.LocatorBoardDeviceList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>Synapse Studio <small>Locator Board Devices</small></h1>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-map"></i>&nbsp;Locator Board Devices (<asp:Label ID="lblLocatorBoardCount" runat="server" Text="0"></asp:Label>)</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Button ID="btnNewDevice" runat="server" CssClass="btn btn-primary pull-left" Text="New Locator Board Device" Width="200" OnClick="btnNewDevice_Click"/>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgLocatorBoardDevices" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundColumn DataField="locatorboarddevicename" HeaderText="Device">
                                            <HeaderStyle Width="20%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="deviceipaddress" HeaderText="IP Address">
                                            <HeaderStyle Width="20%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="locatorboardname" HeaderText="Locator Board">
                                            <HeaderStyle Width="20%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="locationid" HeaderText="Location Code">
                                            <HeaderStyle Width="20%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="locatorboarddevice_id" DataNavigateUrlFormatString="LocatorBoardDeviceView.aspx?id={0}" Text="View">
                                            <HeaderStyle Width="20%" />
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
