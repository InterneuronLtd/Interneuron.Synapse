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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntityManagerRelations.aspx.cs" Inherits="SynapseStudio.EntityManagerRelations" %>

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

        <h3>Entity Relations</h3>

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
                        <h3 class="panel-title"><i class="fa fa-tasks"></i>&nbsp;<asp:Label ID="lblHeading" runat="server" Text="Relations"></asp:Label>
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
                        <h3>New Relation</h3>
                        <div class="row">
                            <div class="col-md-12">

                                <asp:Panel ID="fgSynapseNamespace" runat="server" class="form-group">
                                    <asp:Label ID="lblSynapseNamespace" runat="server" CssClass="control-label" for="ddlSynapseNamespace" Text="Select a namespace from the list below" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="errSynapseNamespace" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlSynapseNamespace" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlSynapseNamespace_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </asp:Panel>

                                <asp:Panel ID="fgEntity" runat="server" class="form-group">
                                    <asp:Label ID="lblEntity" runat="server" CssClass="control-label" for="ddlEntity" Text="Select an entity from the list below" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="errEntity" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlEntity" runat="server" CssClass="form-control input-lg">
                                    </asp:DropDownList>
                                </asp:Panel>

                                <asp:Panel ID="pnlLocalAttribute" runat="server" class="form-group">
                                    <asp:Label ID="lblLocalAttribute" runat="server" CssClass="control-label" for="ddlLocalAttribute" Text="Optional - Select a the local attribute that you want to create the relationship for" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="errLocalAttribute" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlLocalAttribute" runat="server" CssClass="form-control input-lg">
                                    </asp:DropDownList>
                                </asp:Panel>

                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnValidateAttribute" runat="server" CssClass="btn btn-primary pull-right" Text="Validate and Create" Width="200" OnClick="btnValidateAttribute_Click" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnCreateNewAttribute" runat="server" CssClass="btn btn-primary pull-right" Text="Add " Width="200" OnClick="btnCreateNewAttribute_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblError" runat="server" CssClass="contentAlertDanger"></asp:Label>
                                <asp:Label ID="lblSuccess" runat="server" CssClass="contentAlertSuccess"></asp:Label>
                            </div>
                        </div>

                        <hr />

                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">
                                    <asp:Label ID="lblTestType" runat="server"></asp:Label>
                                    <asp:Label ID="lblNamespaceName" runat="server"></asp:Label>
                                    Relations (<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgEntities" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="parentsynapsenamespacename" HeaderText="Parent Namespace">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="parententityname" HeaderText="Parent Entity">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="parentattributename" HeaderText="Parent Attribute">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="attributeid" DataNavigateUrlFormatString="RelationView.aspx?&action=view&id={0}" Text="View">
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

