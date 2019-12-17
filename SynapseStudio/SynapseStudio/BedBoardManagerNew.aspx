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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BedBoardManagerNew.aspx.cs" Inherits="SynapseStudio.BedBoardManagerNew" %>

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
            <asp:HiddenField ID="hdnNamespaceID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnLocalNamespaceID" runat="server" />
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-bed"></i>&nbsp;New Bed Board</h3>
                    </div>
                    <div class="panel-body">
                        <h3>General Details</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgBedBoardName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBedBoardName" runat="server" CssClass="control-label" for="txtBedBoardName" Text="Enter a name for the bed board" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBedBoardName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtBedBoardName" runat="server" CssClass="form-control input-lg"></asp:TextBox>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgBedBoardDescription" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBedBoardDescription" runat="server" CssClass="control-label" for="txtBedBoardDescription" Text="Enter a description for the bed board" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBedBoardDescription" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtBedBoardDescription" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                </asp:Panel>
                            </div>
                        </div>

                        <h3>Underlying Baseview</h3>
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
                                <asp:Panel ID="fgPersonIDField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblPersonIDField" runat="server" CssClass="control-label" for="ddlPersonIDField" Text="Select PersonID field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errPersonIDField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlPersonIDField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlPersonIDField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgEncounterIDField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblEncounterIDField" runat="server" CssClass="control-label" for="ddlEncounterIDField" Text="Select EncounterID field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errEncounterIDField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlEncounterIDField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlEncounterIDField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgWardField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblWardField" runat="server" CssClass="control-label" for="ddlWardField" Text="Select Ward Code field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errWardField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlWardField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlWardField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgBedField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBedField" runat="server" CssClass="control-label" for="ddlBedField" Text="Select Bed Code field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBedField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlBedField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlBedField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>


                        <%-- Top Settings --%>
                        <h3>Top Section</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgTopSetting" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblTopSetting" runat="server" CssClass="control-label" for="ddlTopSetting" Text="Select Top Area Setting" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errTopSetting" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlTopSetting" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlTopSetting_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Please select . . .</asp:ListItem>
                                        <asp:ListItem Value="1">Single Section</asp:ListItem>
                                        <asp:ListItem Value="2">Two Sections</asp:ListItem>
                                    </asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgTopField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblTopField" runat="server" CssClass="control-label" for="ddlTopField" Text="Select Top Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errTopField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlTopField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlTopField_SelectedIndexChanged">
                                    </asp:DropDownList>
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


                        <%-- Middle Settings --%>
                        <h3>Middle Section</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgMiddleSetting" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblMiddleSetting" runat="server" CssClass="control-label" for="ddlMiddleSetting" Text="Select Middle Area Setting" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errMiddleSetting" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlMiddleSetting" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlMiddleSetting_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Please select . . .</asp:ListItem>
                                        <asp:ListItem Value="1">Single Section</asp:ListItem>
                                        <asp:ListItem Value="2">Two Sections</asp:ListItem>
                                    </asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgMiddleField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblMiddleField" runat="server" CssClass="control-label" for="ddlMiddleField" Text="Select Middle Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errMiddleField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlMiddleField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlMiddleSetting_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgMiddleLeftField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblMiddleLeftField" runat="server" CssClass="control-label" for="ddlMiddleLeftField" Text="Select Middle Left Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errMiddleLeftField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlMiddleLeftField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlMiddleLeftField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgMiddleRightField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblMiddleRightField" runat="server" CssClass="control-label" for="ddlMiddleRightField" Text="Select Middle Right Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errMiddleRightField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlMiddleRightField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlMiddleRightField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>


                        <%-- Bottom Settings --%>
                        <h3>Bottom Section</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgBottomSetting" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBottomSetting" runat="server" CssClass="control-label" for="ddlBottomSetting" Text="Select Bottom Area Setting" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBottomSetting" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlBottomSetting" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlBottomSetting_SelectedIndexChanged">
                                        <asp:ListItem Value="0">Please select . . .</asp:ListItem>
                                        <asp:ListItem Value="1">Single Section</asp:ListItem>
                                        <asp:ListItem Value="2">Two Sections</asp:ListItem>
                                    </asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgBottomField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBottomField" runat="server" CssClass="control-label" for="ddlBottomField" Text="Select Bottom Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBottomField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlBottomField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlBottomSetting_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgBottomLeftField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBottomLeftField" runat="server" CssClass="control-label" for="ddlBottomLeftField" Text="Select Bottom Left Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBottomLeftField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlBottomLeftField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlBottomLeftField_SelectedIndexChanged"></asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgBottomRightField" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblBottomRightField" runat="server" CssClass="control-label" for="ddlBottomRightField" Text="Select Bottom Right Field from Baseview" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errBottomRightField" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlBottomRightField" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlBottomRightField_SelectedIndexChanged"></asp:DropDownList>
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
