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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BedBoardDeviceView.aspx.cs" Inherits="SynapseStudio.BedBoardDeviceView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:label id="lblSummaryType" runat="server" text="Bedboard Device"></asp:label>
                </h1>
            </div>
        </div>

        <div>
            <asp:hiddenfield id="hdnDeviceID" runat="server" />
            <asp:hiddenfield id="hdnUserName" runat="server" />
            <asp:hiddenfield id="hdnLocalNamespaceID" runat="server" />
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-desktop"></i>&nbsp;Bedboard Device</h3>
                    </div>
                    <div class="panel-body">

                        <div class="well" style="margin-top: 15px;">
                            <div class="row">
                                <div class="col-lg-12">
                                   <asp:HyperLink ID="hlPreview" runat="server" CssClass="btn btn-default btn-block" Text="Preview with current IP Address"></asp:HyperLink>
                                </div>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgDeviceName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblDeviceName" runat="server" CssClass="control-label" for="txtDeviceName" Text="* Please enter a name for the device" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                          
                                        </div>
                                    </div>

                                    <asp:Label ID="errDeviceName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtDeviceName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:panel>

                                <asp:panel id="fgIPAddress" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblIPAddress" runat="server" CssClass="control-label" for="txtIPAddress" Text="Enter the static IP Address for the device" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errIPAddress" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtIPAddress" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:panel>


                                <h4>Bed Board</h4>

                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:panel id="fgBedBoard" runat="server" class="form-group">
                                            <asp:Label ID="lblBedBoard" runat="server" CssClass="control-label" for="ddlBedBoard" Text="Select Bed Board" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errBedBoard" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlBedBoard" runat="server" CssClass="form-control input-lg">
                                            </asp:DropDownList>
                                        </asp:panel>
                                    </div>
                                </div>

                                <h4>Location Details</h4>

                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:panel id="fgWard" runat="server" class="form-group">
                                            <asp:Label ID="lblWard" runat="server" CssClass="control-label" for="ddllWard" Text="Select ward" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errlWard" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddllWard" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddllWard_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </asp:panel>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:panel id="fgBayRoom" runat="server" class="form-group">
                                            <asp:Label ID="lblBayRoom" runat="server" CssClass="control-label" for="ddlBayRoom" Text="Select bay / room" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errBayRoom" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlBayRoom" runat="server" CssClass="form-control input-lg" OnSelectedIndexChanged="ddlBayRoom_SelectedIndexChanged" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </asp:panel>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:panel id="fgBed" runat="server" class="form-group">
                                            <asp:Label ID="lblBed" runat="server" CssClass="control-label" for="ddlBed" Text="Select bed" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errBed" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlBed" runat="server" CssClass="form-control input-lg" AutoPostBack="false">
                                            </asp:DropDownList>
                                        </asp:panel>
                                    </div>
                                </div>



                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:button id="btnSave" runat="server" cssclass="btn btn-info pull-right" text="Save" width="200" onclick="btnSave_Click" />
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger pull-right" Text="Delete" Width="200" OnClick="btnDelete_Click"/>
                            </div>
                            <div class="col-md-4">
                                <asp:button id="btnCancel" runat="server" cssclass="btn btn-default pull-left" text="Cancel" width="200" onclick="btnCancel_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:label id="lblError" runat="server" cssclass="contentAlertDanger"></asp:label>
                                <asp:label id="lblSuccess" runat="server" cssclass="contentAlertSuccess"></asp:label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>



    </div>

</asp:Content>
