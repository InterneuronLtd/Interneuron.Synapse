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

<%@ Page Title="Client grant types" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientGrantTypes.aspx.cs" Inherits="SynapseStudio.ClientGrantTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">

        <div>
            <asp:HiddenField ID="hdnClientID" runat="server" />
        </div>

        <h3><asp:Label ID="lblH3Client" runat="server"></asp:Label><asp:Label ID="lblH3GrantType" runat="server" Text=" grant types"></asp:Label></h3>

        <div class="well" style="margin-top: 15px;">
            <div class="row">
                <div class="col-lg-3">
                    <asp:Button ID="btnClientClaims" runat="server" CssClass="btn btn-default btn-block" Text="Client claims" OnClick="btnClientClaims_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnClientRedirectUris" runat="server" CssClass="btn btn-default btn-block" Text="Client redirect URI's" OnClick="btnClientRedirectUris_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnClientScopes" runat="server" CssClass="btn btn-default btn-block" Text="Client scopes" OnClick="btnClientScopes_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnClientSecrets" runat="server" CssClass="btn btn-default btn-block" Text="Client secrets" OnClick="btnClientSecrets_Click" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-tasks"></i>&nbsp;
                            <asp:Label ID="lblClient" runat="server"></asp:Label>
                            <asp:Label ID="lblHeading" runat="server" Text="grant types"></asp:Label>
                        </h3>
                    </div>
                    <div class="panel-body">
                        <h3>New client grant type</h3>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlClientGrantType" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblClientGrantType" runat="server" CssClass="control-label" Text="* Please select a client grant type" Font-Bold="true"></asp:Label>
                                            <asp:DropDownList ID="ddlClientGrantType" runat="server" CssClass="form-control input-lg">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvClientGrantType" runat="server" ControlToValidate="ddlClientGrantType" InitialValue="Please select ..." ErrorMessage="Please select grant type" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-primary pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnAddClientGrantType" runat="server" CssClass="btn btn-primary pull-right" Text="Add " Width="200" OnClick="btnAddClientGrantType_Click" ValidationGroup="validate" />
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
                                    <asp:Label ID="lblClientName" runat="server"></asp:Label>
                                    grant types (<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgClientGrantType" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnItemCommand="dgClientGrantType_ItemCommand" OnItemDataBound="dgClientGrantType_ItemDataBound">

                                    <Columns>
                                        <asp:BoundColumn DataField="Id" HeaderText="Id">
                                            <HeaderStyle Width="0%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="GrantType" HeaderText="Grant Type">
                                            <HeaderStyle Width="50%" />
                                        </asp:BoundColumn>
                                        <asp:ButtonColumn ButtonType="LinkButton" CommandName="Remove" Text="Remove">
                                            <HeaderStyle Width="50%" />
                                        </asp:ButtonColumn>
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
