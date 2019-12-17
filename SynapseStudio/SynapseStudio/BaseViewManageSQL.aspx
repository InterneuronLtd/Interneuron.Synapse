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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BaseViewManageSQL.aspx.cs" Inherits="SynapseStudio.BaseViewManageSQL" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">

        <div>
            <asp:HiddenField ID="hdnBaseViewID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnNextOrdinalPosition" runat="server" />
            <asp:HiddenField ID="hdnDataType" runat="server" />
            <asp:HiddenField ID="hdnNamespaceID" runat="server" />
        </div>

        <h3>Baseview SQL Statement</h3>

        <div class="well" style="margin-top: 15px;">
            <div class="row">
                <div class="col-lg-3">
                    <asp:Button ID="btnViewDetails" runat="server" CssClass="btn btn-default btn-block" Text="Details" OnClick="btnViewDetails_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnSQL" runat="server" CssClass="btn btn-default btn-block" Text="SQL" OnClick="btnSQL_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnManageAttributes" runat="server" CssClass="btn btn-default btn-block" Text="Attributes" OnClick="btnManageAttributes_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnManageAPI" runat="server" CssClass="btn btn-default btn-block" Text="APIs" OnClick="btnManageAPI_Click" />
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
                        <asp:Panel ID="fgSQL" runat="server" class="form-group">
                            <div class="row">
                                <div class="col-lg-12">
                                    <h3>Namespace:
                                        <asp:Label ID="lblNamespaceName" runat="server"></asp:Label>
                                    </h3>
                                    <h3>BaseView:
                                        <asp:Label ID="lblBaseViewName" runat="server"></asp:Label>
                                    </h3>
                                    <h3>
                                        <asp:Label ID="lblBaseViewComments" runat="server"></asp:Label>
                                    </h3>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Label ID="errSQL" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtSQL" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="25"></asp:TextBox>
                                </div>
                            </div>

                            <br />
                            <br />

                            <div class="well">
                                <h3>Re-Create BaseView</h3>
                                <p>If you would like to re-create the baseview please edit your SQL Statement above and when you are ready click on the 'Re-validate BaseView' button below.</p>
                                <br />
                                <p>Please note that the system does not currently support any consistency checking to make sure that all previously defined columns are available in the new SQL statement. This could affect any front-end clients that are using the APIs used by this BaseView.</p>
                                <br />
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnValidateEntity" runat="server" CssClass="btn btn-info pull-right" Text="Re-Validate" Width="200" OnClick="btnValidateEntity_Click" />
                                        <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                        <asp:Button ID="btnCreateNewEntity" runat="server" CssClass="btn btn-primary pull-right" Text="Recreate Baseview" Width="200" OnClick="btnCreateNewEntity_Click" />
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="lblError" runat="server" CssClass="contentAlertDanger"></asp:Label>
                                        <asp:Label ID="lblSuccess" runat="server" CssClass="contentAlertSuccess"></asp:Label>
                                    </div>
                                </div>
                            </div>


                        </asp:Panel>
                    </div>



                </div>
            </div>
        </div>
    </div>


</asp:Content>


