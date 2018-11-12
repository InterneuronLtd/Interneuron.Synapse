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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListManagerView.aspx.cs" Inherits="SynapseStudio.ListManagerView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">

        <div>
            <asp:HiddenField ID="hdnListID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnNextOrdinalPosition" runat="server" />
            <asp:HiddenField ID="hdnDataType" runat="server" />
            <asp:HiddenField ID="hdnBaseView" runat="server" />
        </div>

        <h3>
            <asp:Label ID="lblSummaryType" runat="server"></asp:Label>
            - List Details</h3>

        <div class="well" style="margin-top: 15px;">
            <div class="row">                
                <div class="col-lg-3">
                    <asp:Button ID="btnViewDetails" runat="server" CssClass="btn btn-default btn-block" Text="Details" OnClick="btnViewDetails_Click" />
                </div>
                <%--<div class="col-lg-3">
                    <asp:Button ID="btnManageAttributes" runat="server" CssClass="btn btn-default btn-block" Text="Attributes" OnClick="btnManageAttributes_Click" />
                </div>--%>
                <div class="col-lg-3">
                    <asp:Button ID="btnSelectAttributes" runat="server" CssClass="btn btn-default btn-block" Text="Select Columns" OnClick="btnSelectAttributes_Click" />
                </div>
                <div class="col-lg-2">
                    <asp:Button ID="btnSelectQuestions" runat="server" CssClass="btn btn-default btn-block" Text="Select Questions" OnClick="btnSelectQuestions_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnManageAPI" runat="server" CssClass="btn btn-default btn-block" Text="APIs" OnClick="btnManageAPI_Click" />
                </div>                                
            </div>
        </div>


        <div class="well" style="margin-top: 15px;">
            <div class="row">
                <div class="col-lg-12">
                    <asp:HyperLink ID="hlPreview" runat="server" CssClass="btn btn-default btn-block" Text="Preview List"></asp:HyperLink>
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
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgEntities" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="false">

                                    <Columns>
                                        <asp:BoundColumn DataField="listdetail" HeaderText="Detail">
                                            <HeaderStyle Width="40%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="listdescription" HeaderText="BaseViewdescription">
                                            <HeaderStyle Width="60%" />
                                        </asp:BoundColumn>
                                    </Columns>



                                </asp:DataGrid>
                            </div>
                        </div>
                        <div>
                            <asp:Button ID="btnShowDeleteBaseView" runat="server" CssClass="btn btn-default pull-left" Text="Enable Delete" Width="200" OnClick="btnShowDeleteBaseView_Click" />
                            <asp:Button ID="btnDropBaseView" runat="server" CssClass="btn btn-danger pull-right" Text="Delete BaseView" Width="200" OnClick="btnDropBaseView_Click" Visible="false" />
                        </div>

                        <div class="">

                            <h2>Edit</h2>

                             <asp:Panel ID="fgListName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblListName" runat="server" CssClass="control-label" for="txtListName" Text="* Please enter a name for the List" Font-Bold="true"></asp:Label>
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
                                            <asp:Label ID="lblListComments" runat="server" CssClass="control-label" for="txtListName" Text="Enter a description for the  List" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errListComments" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtListComments" runat="server" CssClass="form-control input-lg" TextMode="MultiLine"></asp:TextBox>
                                </asp:Panel>



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

                            <div class="row">
                                <div class="col-md-12">                                    
                                    <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                    <asp:Button ID="btnCreateNewList" runat="server" CssClass="btn btn-primary pull-right" Text="Save List" Width="200" onclick="btnCreateNewList_Click" />
                                </div>
                            </div>                            
                        </div>


                    </div>

                </div>

                <br />

                <div class="panel panel-primary hidden">
                    <div class="panel-heading">
                        &nbsp;
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <div class="row">
                                    <div class="col-lg-2">
                                        <asp:Button ID="btnRecreateBaseview" runat="server" CssClass="btn btn-info pull-left" Text="Recreate Baseview" OnClick="btnRecreateBaseview_Click" />
                                    </div>
                                    <div class="col-lg-10">
                                        <asp:Label ID="lblRecreateBaseView" runat="server" ForeColor="GreenYellow" Font-Bold="true"></asp:Label>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Literal ID="ltrlError" runat="server"></asp:Literal>
                                        <asp:Label ID="lblError" runat="server" CssClass="contentAlertDanger"></asp:Label>
                                        <asp:Label ID="lblSuccess" runat="server" CssClass="contentAlertSuccess"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
