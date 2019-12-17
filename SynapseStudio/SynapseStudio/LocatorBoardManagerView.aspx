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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LocatorBoardManagerView.aspx.cs" Inherits="SynapseStudio.LocatorBoardManagerView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Board Manager"></asp:Label>
                </h1>
            </div>
        </div>

        <div>
            <asp:HiddenField ID="hdnBoardID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnLocalNamespaceID" runat="server" />
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <%-- <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-bed"></i>&nbsp;Options</h3>
                    </div>--%>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-3">
                                <asp:HyperLink ID="hlPreview" runat="server" CssClass="btn btn-block btn-default pull-left" Text="Preview" Target="_blank"></asp:HyperLink>
                                <%--<asp:Button ID="btnPreview" runat="server" CssClass="btn btn-block btn-default pull-left" Text="Preview" OnClick="btnPreview_Click" />--%>
                            </div>
                            <div class="col-md-1">
                                <%--<asp:Button ID="btnRandom" runat="server" CssClass="btn btn-block btn-default pull-left" Text="New Random" OnClick="btnRandom_Click" />--%>
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-block btn-danger" Text="Delete Board" OnClick="btnDelete_Click" />
                            </div>
                            <div class="col-md-4">
                                <%--<asp:Button ID="btnClone" runat="server" CssClass="btn btn-block btn-primary" Text="Clone this board" OnClick="btnClone_Click" />--%>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-bed"></i>&nbsp;Bed Board</h3>
                    </div>
                    <div class="panel-body">

                        <h3>General Details</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgLocatorBoardName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblLocatorBoardName" runat="server" CssClass="control-label" for="txtLocatorBoardName" Text="Enter a name for the bed board" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errLocatorBoardName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtLocatorBoardName" runat="server" CssClass="form-control input-lg"></asp:TextBox>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgLocatorBoardDescription" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblLocatorBoardDescription" runat="server" CssClass="control-label" for="txtLocatorBoardDescription" Text="Enter a description for the bed board" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errLocatorBoardDescription" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtLocatorBoardDescription" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                </asp:Panel>
                            </div>
                        </div>


                        <h3>Underlying List</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgList" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblList" runat="server" CssClass="control-label" for="ddlList" Text="Select Underlying List" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errList" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlList" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlList_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgListLocationField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblListLocationField" runat="server" CssClass="control-label" for="ddlListLocationField" Text="Select Location field from the underlying list" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errListLocationField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlListLocationField" runat="server" CssClass="form-control input-lg"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>


                        <h3>Underlying Baseview for Ward Information</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgBaseViewNamespace" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBaseViewNamespace" runat="server" CssClass="control-label" for="ddlBaseViewNamespace" Text="Select Baseview Namespace" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBaseViewNamespace" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlBaseViewNamespace" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlBaseViewNamespace_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgBaseView" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBaseView" runat="server" CssClass="control-label" for="ddlBaseView" Text="Select Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBaseView" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlBaseView" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlBaseView_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgLocationIDField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblLocationIDField" runat="server" CssClass="control-label" for="ddlLocationIDField" Text="Select Location ID field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errLocationIDField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlLocationIDField" runat="server" CssClass="form-control input-lg"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>



                        <%-- Top Settings --%>
                        <h3>Location Display Fields</h3>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgHeading" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblHeading" runat="server" CssClass="control-label" for="ddlHeading" Text="Select Heading Field" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errHeading" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlHeading" runat="server" CssClass="form-control input-lg"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgTopLeftField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblTopLeftField" runat="server" CssClass="control-label" for="ddlTopLeftField" Text="Select Top Left Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errTopLeftField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlTopLeftField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlTopLeftField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgTopRightField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblTopRightField" runat="server" CssClass="control-label" for="ddlTopRightField" Text="Select Top Right Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errTopRightField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlTopRightField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlTopRightField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary pull-right" Text="Save" Width="200" OnClick="btnSave_Click" />
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Literal runat="server" ID="ltrlError" />
                                <asp:Label ID="lblSuccess" runat="server" CssClass="contentAlertSuccess"></asp:Label>
                            </div>
                        </div>
                        <br />
                    </div>
                </div>
            </div>
        </div>



    </div>

</asp:Content>
