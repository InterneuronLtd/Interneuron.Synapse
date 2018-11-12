<%--
Interneuron Synapse

Copyright (C) 2018  Interneuron CIC



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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListManagerNew.aspx.cs" Inherits="SynapseStudio.ListManagerNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="List Manager"></asp:Label>
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
                        <h3 class="panel-title"><i class="fa fa-database"></i>&nbsp;New List</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <h3 class="panel-title" style="font-weight: bold; font-size: 2em;">New
                                    <asp:Label ID="lblNamespaceName" runat="server"></asp:Label>
                                    List</h3>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgListName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblListName" runat="server" CssClass="control-label" for="txtListName" Text="* Please enter a name for the new List" Font-Bold="true"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <%-- <span style="color: #c5997a; float: right;">(All non-alphanumeric characters willl be stripped out during validation)</span>--%>
                                        </div>
                                    </div>

                                    <asp:Label ID="errListName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtListName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>

                                <asp:Panel ID="fgListComments" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblListComments" runat="server" CssClass="control-label" for="txtListName" Text="Enter a description for the new List" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errListComments" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtListComments" runat="server" CssClass="form-control input-lg" TextMode="MultiLine"></asp:TextBox>
                                </asp:Panel>

                                <h4>Baseview for List</h4>

                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="fgBaseViewNamespace" runat="server" class="form-group">
                                            <asp:Label ID="lblBaseViewNamespace" runat="server" CssClass="control-label" for="ddlBaseViewNamespace" Text="Select a baseview namespace from the list below" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errrBaseViewNamespace" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlBaseViewNamespace" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlBaseViewNamespace_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="fgBaseView" runat="server" class="form-group">
                                            <asp:Label ID="lblBaseView" runat="server" CssClass="control-label" for="ddlBaseView" Text="Select a baseview from the list below" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errBaseView" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlBaseView" runat="server" CssClass="form-control input-lg">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="fgDefaultContext" runat="server" class="form-group">
                                            <asp:Label ID="lblDefaultContext" runat="server" CssClass="control-label" for="ddlDefaultContext" Text="Select a the entity that defines the default context for this list" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errDefaultContext" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlDefaultContext" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlDefaultContext_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <h3>
                                            <asp:Label ID="lblDefaultContextField" runat="server"></asp:Label>
                                        </h3>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="fgMatchedContextField" runat="server" class="form-group">
                                            <asp:Label ID="lblMatchedContextField" runat="server" CssClass="control-label" for="ddlMatchedContextField" Text="Select the field from the baseview that defines the key for the default context" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errMatchedContextField" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlMatchedContextField" runat="server" CssClass="form-control input-lg">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="fgPatientBannerField" runat="server" class="form-group">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblPatientBannerField" runat="server" CssClass="control-label" for="ddlPatientBannerField" Text="Select Patient Banner Field" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>

                                            <asp:Label ID="errPatientBannerField" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlPatientBannerField" runat="server" CssClass="form-control input-lg"></asp:DropDownList>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="fgRowCSSField" runat="server" class="form-group">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblRowCSSField" runat="server" CssClass="control-label" for="ddlRowCSSField" Text="Select Dynamic Row CSS Field" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>

                                            <asp:Label ID="errRowCSSField" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlRowCSSField" runat="server" CssClass="form-control input-lg"></asp:DropDownList>
                                        </asp:Panel>
                                    </div>
                                </div>


                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="pblTableClass" runat="server" class="form-group">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblTableClass" runat="server" CssClass="control-label" for="txtTableClass" Text="Please enter css class for the table" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                            <asp:Label ID="errTableClass" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtTableClass" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="pnlTableHeaderClass" runat="server" class="form-group">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblTableHeaderClass" runat="server" CssClass="control-label" for="txtTableHeaderClass" Text="Please enter css class for the table header" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                            <asp:Label ID="errTableHeaderClass" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtTableHeaderClass" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="pnlDefaultTableRowCSS" runat="server" class="form-group">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <asp:Label ID="lblDefaultTableRowCSS" runat="server" CssClass="control-label" for="txtDefaultTableRowCSS" Text="Please enter the default css class for the table rows" Font-Bold="true"></asp:Label>
                                                </div>
                                            </div>
                                            <asp:Label ID="errDefaultTableRowCSS" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtDefaultTableRowCSS" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                        </asp:Panel>
                                    </div>
                                </div>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnValidateList" runat="server" CssClass="btn btn-info pull-right" Text="Validate" Width="200" OnClick="btnValidateList_Click" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnCreateNewList" runat="server" CssClass="btn btn-primary pull-right" Text="Create List" Width="200" OnClick="btnCreateNewList_Click" />
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
                                        <asp:BoundColumn DataField="ListName" HeaderText="List">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="Listid" DataNavigateUrlFormatString="ListManagerView.aspx?&action=view&id={0}" Text="View">
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
