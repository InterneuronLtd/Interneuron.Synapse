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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocalNamespaceManager.aspx.cs" Inherits="SynapseStudio.LocalNamespaceManager" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-9">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Local Namespaces"></asp:Label>
                </h1>
            </div>
            <div class="col-lg-3">
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-info pull-right" Text="Back" Width="200" OnClick="btnBack_Click"/>
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
                        <h3 class="panel-title"><i class="fa fa-database"></i>&nbsp;Local Namespaces</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-9">
                                <h3 class="panel-title" style="font-weight: bold; font-size: 2em;">New
                                    <asp:Label ID="lblNamespaceName" runat="server"></asp:Label>
                                    Namespace</h3>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">


                                <asp:Panel ID="fgLocalNamespaceName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblLocalNamespaceName" runat="server" CssClass="control-label" for="txtLocalNamespaceName" Text="* Please enter a name for the new local namespace" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <span style="color: #c5997a; float: right;">(All non-alphanumeric characters willl be stripped out during validation)</span>
                                        </div>
                                    </div>

                                    <asp:Label ID="errLocalNamespaceName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtLocalNamespaceName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>

                                <asp:Panel ID="fgLocalNamespaceDescription" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblLocalNamespaceDescription" runat="server" CssClass="control-label" for="txtLocalNamespaceDescription" Text="Enter a description for the new local namespace" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errLocalNamespaceDescription" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtLocalNamespaceDescription" runat="server" CssClass="form-control input-lg" TextMode="MultiLine"></asp:TextBox>
                                </asp:Panel>


                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnValidateEntity" runat="server" CssClass="btn btn-info pull-right" Text="Validate" Width="200" OnClick="btnValidateEntity_Click" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnCreateNewNamespace" runat="server" CssClass="btn btn-primary pull-right" Text="Create Namespace" Width="200" OnClick="btnCreateNewNamespace_Click" />
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
                                        <asp:BoundColumn DataField="localnamespacename" HeaderText="Local Namespace">
                                            <HeaderStyle Width="100%" />
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
