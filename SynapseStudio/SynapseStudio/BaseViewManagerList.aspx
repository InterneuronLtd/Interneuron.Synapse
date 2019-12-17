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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BaseViewManagerList.aspx.cs" Inherits="SynapseStudio.BaseViewManagerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Baseview Manager"></asp:Label>
                </h1>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-anchor"></i>&nbsp;Baseview Manager</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnManageLocalNamespaces" runat="server" CssClass="btn btn-info pull-right" Text="Manage BaseView Namespaces" Width="250" OnClick="btnManageLocalNamespaces_Click" />
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgSynapseNamespace" runat="server" class="form-group">
                                    <asp:Label ID="lblSynapseNamespace" runat="server" CssClass="control-label" for="ddlSynapseNamespace" Text="Select a namespace from the list below" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="errrSynapseNamespace" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlSynapseNamespace" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlSynapseNamespace_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>


                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCreateNew" runat="server" CssClass="btn btn-primary pull-right" Text="New" Width="200" OnClick="btnCreateNew_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">
                                    <asp:Label ID="lblTestType" runat="server"></asp:Label>
                                    <asp:Label ID="lblNamespaceName" runat="server"></asp:Label>
                                    BASEVIEWS (<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgBaseviews" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="baseviewname" HeaderText="Baseview">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="baseview_id" DataNavigateUrlFormatString="BaseviewManagerView.aspx?&action=view&id={0}" Text="View">
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
