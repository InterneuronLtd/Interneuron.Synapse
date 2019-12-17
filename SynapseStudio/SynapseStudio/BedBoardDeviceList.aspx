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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BedBoardDeviceList.aspx.cs" Inherits="SynapseStudio.BedBoardDeviceList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>Synapse Studio <small>Bed Board Devices</small></h1>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-desktop"></i>&nbsp;Bed Board Devices (<asp:Label ID="lblBedBoardCount" runat="server" Text="0"></asp:Label>)</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-6">
                                <asp:Button ID="btnNewDevice" runat="server" CssClass="btn btn-primary pull-left" Text="New Bed Board Device" Width="200" OnClick="btnNewDevice_Click"/>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgBedBoardDevices" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundColumn DataField="bedboarddevicename" HeaderText="Device">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="deviceipaddress" HeaderText="IP Address">
                                            <HeaderStyle Width="10%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="bedboardname" HeaderText="Bed Board">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="wardlocationdisplayname" HeaderText="Ward">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="bayroomlocationdisplayname" HeaderText="Bay / Room">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundColumn>                                        
                                        <asp:BoundColumn DataField="bedlocationdisplayname" HeaderText="Bed">
                                            <HeaderStyle Width="15%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="bedboarddevice_id" DataNavigateUrlFormatString="BedBoardDeviceView.aspx?id={0}" Text="View">
                                            <HeaderStyle Width="10%" />
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
