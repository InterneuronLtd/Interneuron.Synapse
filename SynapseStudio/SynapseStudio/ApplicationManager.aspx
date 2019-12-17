
<%--Interneuron Synapse

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
along with this program.If not, see<http://www.gnu.org/licenses/>.--%>


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationManager.aspx.cs" Inherits="SynapseStudio.ApplicationManager" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Application Manager"></asp:Label>
                </h1>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-6">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-laptop"></i>&nbsp;Applications</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCreateApplication" runat="server" CssClass="btn btn-primary pull-right" Text="New Application" Width="200" OnClick="btnCreateApplication_Click"  />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">Applications(<asp:Label ID="lblApplicationCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgApplication" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundColumn DataField="applicationname" HeaderText="Applications">
                                            <HeaderStyle Width="70%" />
                                        </asp:BoundColumn>
                                       
                                        <asp:HyperLinkColumn DataNavigateUrlField="application_id" DataNavigateUrlFormatString="ApplicationModuleMapping.aspx?&app=view&id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-6">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-briefcase"></i>&nbsp;Modules</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnManageIdentityClaims" runat="server" CssClass="btn btn-primary pull-right" Text="New Module" Width="200" OnClick="btnManageIdentityClaims_Click"  />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                    <asp:Label ID="Label2" runat="server"></asp:Label>
                                    Modules (<asp:Label ID="lblModuleCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgModule" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="modulename" HeaderText="Module">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="moduledescription" HeaderText="Description">
                                            <HeaderStyle Width="45%" />
                                        </asp:BoundColumn>
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
