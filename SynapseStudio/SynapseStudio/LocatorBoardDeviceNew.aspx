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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocatorBoardDeviceNew.aspx.cs" Inherits="SynapseStudio.LocatorBoardDeviceNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="New Locator Board Device"></asp:Label>
                </h1>
            </div>
        </div>

        <div>
            <asp:HiddenField ID="hdnDeviceID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnLocalNamespaceID" runat="server" />
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-desktop"></i>&nbsp;New Locatorboard Device</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">                             
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgDeviceName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblDeviceName" runat="server" CssClass="control-label" for="txtDeviceName" Text="* Please enter a name for the device" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                          
                                        </div>
                                    </div>

                                    <asp:Label ID="errDeviceName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtDeviceName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>

                                <asp:Panel ID="fgIPAddress" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblIPAddress" runat="server" CssClass="control-label" for="txtIPAddress" Text="Enter the static IP Address for the device" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errIPAddress" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtIPAddress" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>


                                <h4>Locator Board</h4>

                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="fgLocatorBoard" runat="server" class="form-group">
                                            <asp:Label ID="lblLocatorBoard" runat="server" CssClass="control-label" for="ddlLocatorBoard" Text="Select Locator Board" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errLocatorBoard" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlLocatorBoard" runat="server" CssClass="form-control input-lg">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <h4>Location Details</h4>

                                <asp:Panel ID="fgLocationCode" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblLocationCode" runat="server" CssClass="control-label" for="txtLocationCode" Text="* Please enter the location code" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                          
                                        </div>
                                    </div>

                                    <asp:Label ID="Label2" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtLocationCode" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>
                         


                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-info pull-right" Text="Save" Width="200" OnClick="btnSave_Click" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-default pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click"/>                                
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblError" runat="server" CssClass="contentAlertDanger"></asp:Label>
                                <asp:Label ID="lblSuccess" runat="server" CssClass="contentAlertSuccess"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



    </div>

</asp:Content>

