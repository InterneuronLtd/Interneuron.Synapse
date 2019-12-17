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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocatorBoardManagerNew.aspx.cs" Inherits="SynapseStudio.LocatorBoardManagerNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:label id="lblSummaryType" runat="server" text="Board Manager"></asp:label>
                </h1>
            </div>
        </div>

        <div>
            <asp:hiddenfield id="hdnNamespaceID" runat="server" />
            <asp:hiddenfield id="hdnUserName" runat="server" />
            <asp:hiddenfield id="hdnLocalNamespaceID" runat="server" />
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-map"></i>&nbsp;New Locator Board</h3>
                    </div>
                    <div class="panel-body">
                        <h3>General Details</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgLocatorBoardName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblLocatorBoardName" runat="server" CssClass="control-label" for="txtLocatorBoardName" Text="Enter a name for the bed board" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errLocatorBoardName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtLocatorBoardName" runat="server" CssClass="form-control input-lg"></asp:TextBox>
                                </asp:panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgLocatorBoardDescription" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblLocatorBoardDescription" runat="server" CssClass="control-label" for="txtLocatorBoardDescription" Text="Enter a description for the bed board" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errLocatorBoardDescription" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtLocatorBoardDescription" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                </asp:panel>
                            </div>
                        </div>


                        <h3>Underlying List</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgList" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblList" runat="server" CssClass="control-label" for="ddlList" Text="Select Underlying List" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errList" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlList" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlList_SelectedIndexChanged"></asp:DropDownList>
                                </asp:panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgListLocationField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblListLocationField" runat="server" CssClass="control-label" for="ddlListLocationField" Text="Select Location field from the underlying list" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errListLocationField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlListLocationField" runat="server" CssClass="form-control input-lg"></asp:DropDownList>
                                </asp:panel>
                            </div>
                        </div>


                        <h3>Underlying Baseview for Ward Information</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgBaseViewNamespace" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBaseViewNamespace" runat="server" CssClass="control-label" for="ddlBaseViewNamespace" Text="Select Baseview Namespace" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBaseViewNamespace" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlBaseViewNamespace" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlBaseViewNamespace_SelectedIndexChanged"></asp:DropDownList>
                                </asp:panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgBaseView" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBaseView" runat="server" CssClass="control-label" for="ddlBaseView" Text="Select Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBaseView" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlBaseView" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlBaseView_SelectedIndexChanged"></asp:DropDownList>
                                </asp:panel>
                            </div>
                        </div>                        

                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgLocationIDField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblLocationIDField" runat="server" CssClass="control-label" for="ddlLocationIDField" Text="Select Location ID field from Baseview" Font-Bold="true"></asp:Label>                                            
                                        </div>
                                    </div>

                                    <asp:Label ID="errLocationIDField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlLocationIDField" runat="server" CssClass="form-control input-lg" ></asp:DropDownList>
                                </asp:panel>
                            </div>
                        </div>

                                               

                        <%-- Top Settings --%>
                        <h3>Location Display Fields</h3>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgHeading" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblHeading" runat="server" CssClass="control-label" for="ddlHeading" Text="Select Heading Field" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errHeading" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlHeading" runat="server" CssClass="form-control input-lg"></asp:DropDownList>
                                </asp:panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgTopLeftField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblTopLeftField" runat="server" CssClass="control-label" for="ddlTopLeftField" Text="Select Top Left Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errTopLeftField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlTopLeftField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlTopLeftField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:panel id="fgTopRightField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblTopRightField" runat="server" CssClass="control-label" for="ddlTopRightField" Text="Select Top Right Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errTopRightField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlTopRightField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlTopRightField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:panel>
                            </div>
                        </div>



                        <div class="row">
                            <div class="col-md-12">
                                <asp:button id="btnCancel" runat="server" cssclass="btn btn-info pull-left" text="Cancel" width="200" onclick="btnCancel_Click" />
                                <asp:button id="btnSave" runat="server" cssclass="btn btn-primary pull-right" text="Save" width="200" onclick="btnSave_Click" />
                            </div>
                        </div>
                        <br />

                        <div class="row">
                            <div class="col-md-12">
                                <asp:literal runat="server" id="ltrlError" />
                                <asp:label id="lblSuccess" runat="server" cssclass="contentAlertSuccess"></asp:label>
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>



    </div>

</asp:Content>
