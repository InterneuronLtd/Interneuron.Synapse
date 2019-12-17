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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntityManagerView.aspx.cs" Inherits="SynapseStudio.EntityManagerView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">

        <div>
            <asp:HiddenField ID="hdnEntityID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnNextOrdinalPosition" runat="server" />
            <asp:HiddenField ID="hdnDataType" runat="server" />
        </div>

        <h3>Entity Details</h3>

        <div class="well" style="margin-top: 15px;">
            <div class="row">
                <div class="col-lg-2">
                    <asp:Button ID="btnViewDetails" runat="server" CssClass="btn btn-default btn-block" Text="Details" OnClick="btnViewDetails_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnManageAttributes" runat="server" CssClass="btn btn-default btn-block" Text="Attributes" OnClick="btnManageAttributes_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnManageRelations" runat="server" CssClass="btn btn-default btn-block" Text="Relations" OnClick="btnManageRelations_Click" />
                </div>
                <div class="col-lg-2">
                    <asp:Button ID="btnManageAPI" runat="server" CssClass="btn btn-default btn-block" Text="APIs" OnClick="btnManageAPI_Click" />
                </div>
                <div class="col-lg-2">
                    <asp:Button ID="btnManageModels" runat="server" CssClass="btn btn-default btn-block" Text="Models" OnClick="btnManageModels_Click" />
                </div>
            </div>
        </div>



        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-cog"></i>&nbsp;<asp:Label ID="Label1" runat="server" Text="Details"></asp:Label>
                        </h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <h1>
                                    <asp:Label ID="lblSummaryType" runat="server"></asp:Label>
                                </h1>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgEntities" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="false">

                                    <Columns>
                                        <asp:BoundColumn DataField="entitydetail" HeaderText="entitydetail">
                                            <HeaderStyle Width="40%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="entitydescription" HeaderText="entitydescription">
                                            <HeaderStyle Width="60%" />
                                        </asp:BoundColumn>
                                    </Columns>



                                </asp:DataGrid>
                            </div>
                        </div>


                        <asp:Button ID="btnShowDeleteEntity" runat="server" CssClass="btn btn-default pull-left" Text="Enable Delete" Width="200" OnClick="btnShowDeleteEntity_Click" />
                        <asp:Button ID="btnDropEntity" runat="server" CssClass="btn btn-danger pull-right" Text="Delete Entity" Width="200" OnClick="btnDropEntity_Click" Visible="false" />

                         <h4><asp:Label ID="lblError" runat="server" ForeColor="PaleVioletRed"></asp:Label></h4>
                        
                        <asp:Panel ID="pnlHasDepencies" runat="server" CssClass="alert alert-danger">
                            <h3>
                                You are unable to delete this entity as it has
                                <asp:Label ID="lblHasDepencies" runat="server"></asp:Label>
                                baseviews that are dependant on it.
                            </h3>
                        </asp:Panel>


                        <div class="row">
                            <div class="col-md-12">
                                <h3>Baseviews that are dependant on this entity (<asp:Label ID="lblDependentBaseviewCount" runat="server"></asp:Label>)</h3>
                                <asp:DataGrid ID="dgBaseViewDependancies" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="false">

                                    <Columns>
                                        <asp:BoundColumn DataField="dependent_view" HeaderText="Baseview">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="baseview_id" DataNavigateUrlFormatString="BaseViewManageSQL.aspx?&action=view&id={0}" Text="View">
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

