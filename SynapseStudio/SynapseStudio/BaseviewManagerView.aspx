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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BaseviewManagerView.aspx.cs" Inherits="SynapseStudio.BaseviewManagerView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">

        <div>
            <asp:HiddenField ID="hdnBaseViewID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnNextOrdinalPosition" runat="server" />
            <asp:HiddenField ID="hdnDataType" runat="server" />
        </div>

        <h3>Baseview Details</h3>

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
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgEntities" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" ShowHeader="false">

                                    <Columns>
                                        <asp:BoundColumn DataField="BaseViewdetail" HeaderText="BaseViewdetail">
                                            <HeaderStyle Width="40%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="BaseViewdescription" HeaderText="BaseViewdescription">
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
                        
                    </div>

                </div>

                <br />

                <div class="panel panel-primary">
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
