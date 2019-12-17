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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntityManagerNewMeta.aspx.cs" Inherits="SynapseStudio.EntityManagerNewMeta" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Entity Manager"></asp:Label>
                </h1>
            </div>
        </div>

        <div>
            <asp:HiddenField ID="hdnNamespaceID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnLocalNamespaceID" runat="server" />
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-database"></i>&nbsp;New Meta Entity</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <h3 class="panel-title" style="font-weight: bold; font-size: 2em;">New
                                    <asp:Label ID="lblNamespaceName" runat="server"></asp:Label>
                                    entity</h3>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgEntityName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblEntityName" runat="server" CssClass="control-label" for="txtEntityName" Text="* Please enter a name for the new entity" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                             <span style="color: #c5997a; float: right;">(All non-alphanumeric characters willl be stripped out during validation)</span>
                                        </div>
                                    </div>
                                    
                                    <asp:Label ID="errEntityName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtEntityName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>

                                <asp:Panel ID="fgEntityComments" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblEntityComments" runat="server" CssClass="control-label" for="txtEntityName" Text="Enter a description for the new entity" Font-Bold="true"></asp:Label>
                                        </div>                                       
                                    </div>
                                    
                                    <asp:Label ID="errEntityComments" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtEntityComments" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" ></asp:TextBox>
                                </asp:Panel>


                            </div>
                        </div>                                                
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnValidateEntity" runat="server" CssClass="btn btn-info pull-right" Text="Validate" Width="200" OnClick="btnValidateEntity_Click" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnCreateNewEntity" runat="server" CssClass="btn btn-primary pull-right" Text="Create Entity" Width="200" OnClick="btnCreateNewEntity_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblError" runat="server" CssClass="contentAlertDanger"></asp:Label>
                                <asp:Label ID="lblSuccess" runat="server" CssClass="contentAlertSuccess"></asp:Label>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgEntities" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="entityname" HeaderText="Entity">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="entityid" DataNavigateUrlFormatString="EntityManagerView.aspx?&action=view&id={0}" Text="View">
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
