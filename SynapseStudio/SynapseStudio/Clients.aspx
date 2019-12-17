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

<%@ Page Title="Clients" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clients.aspx.cs" Inherits="SynapseStudio.Clients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Clients"></asp:Label>
                </h1>
            </div>
        </div>

        <div>
            <asp:HiddenField ID="hdnClientID" runat="server" />
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-database"></i>&nbsp; client Manager</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                            <h3 class="panel-title" style="font-weight: bold; font-size: 2em;"> <asp:Label ID="lblformname" runat="server" CssClass="control-label" Text="New client" Font-Bold="true"></asp:Label></h3>
                            </div>
                             
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlEnabled" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkEnabled" runat="server" CssClass="checkbox-inline" Checked="true" Text="Enabled"></asp:CheckBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlClientId" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblClientId" runat="server" CssClass="control-label" Text="* Please enter a client id" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtClientId" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                            <asp:RequiredFieldValidator ControlToValidate="txtClientId" ID="rfvClientId" runat="server" ErrorMessage="Please enter client id" ValidationGroup="validate" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlReqClntSecret" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkReqClntSecret" runat="server" CssClass="checkbox-inline" Text="Require client secret"></asp:CheckBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlClientName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblClientName" runat="server" CssClass="control-label"  Text="Please enter a client name" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtClientName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                 <asp:Panel ID="pnlDesc" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblDesc" runat="server" CssClass="control-label"  Text="Please enter a client description" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlReqCon" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkReqCon" runat="server" CssClass="checkbox-inline" Text="Require consent"></asp:CheckBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlAllAccTkn" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkAllAccTkn" runat="server" CssClass="checkbox-inline" Text="Allow access tokens via browser"></asp:CheckBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlAllOffAcc" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkAllOffAcc" runat="server" CssClass="checkbox-inline" Checked="true" Text="Allow offline access"></asp:CheckBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlIdTknLt" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblIdTknLt" runat="server" CssClass="control-label"  Text="Identity token lifetime" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtIdTknLt" runat="server" CssClass="form-control input-lg" MaxLength="255" Text="1800"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revIdTknLt" runat="server" ControlToValidate="txtIdTknLt" ErrorMessage="Please enter numbers only" ForeColor="Red" ValidationExpression="^\d+$" ValidationGroup="validate"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlAccTknLt" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblAccTknLt" runat="server" CssClass="control-label"  Text="Access token lifetime" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtAccTknLt" runat="server" CssClass="form-control input-lg" MaxLength="255" Text="1800"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revAccTknLt" runat="server" ControlToValidate="txtAccTknLt" ErrorMessage="Please enter numbers only" ForeColor="Red" ValidationExpression="^\d+$" ValidationGroup="validate"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlAuthCodeLt" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblAuthCodeLt" runat="server" CssClass="control-label"  Text="Authorization code lifetime" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtAuthCodeLt" runat="server" CssClass="form-control input-lg" MaxLength="255" Text="1800"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revAuthCodeLt" runat="server" ControlToValidate="txtAuthCodeLt" ErrorMessage="Please enter numbers only" ForeColor="Red" ValidationExpression="^\d+$" ValidationGroup="validate"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlConLt" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblConLt" runat="server" CssClass="control-label"  Text="Consent lifetime" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtConLt" runat="server" CssClass="form-control input-lg" MaxLength="255" Text="1800"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revConLt" runat="server" ControlToValidate="txtConLt" ErrorMessage="Please enter numbers only" ForeColor="Red" ValidationExpression="^\d+$" ValidationGroup="validate"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlAbReTknLt" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblAbReTknLt" runat="server" CssClass="control-label"  Text="Absolute refresh token lifetime" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtAbReTknLt" runat="server" CssClass="form-control input-lg" MaxLength="255" Text="2592000"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revAbReTknLt" runat="server" ControlToValidate="txtAbReTknLt" ErrorMessage="Please enter numbers only" ForeColor="Red" ValidationExpression="^\d+$" ValidationGroup="validate"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlSlReTknLt" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblSlReTknLt" runat="server" CssClass="control-label"  Text="Sliding refresh token lifetime" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtSlReTknLt" runat="server" CssClass="form-control input-lg" MaxLength="255" Text="1296000"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="revSlReTknLt" runat="server" ControlToValidate="txtSlReTknLt" ErrorMessage="Please enter numbers only" ForeColor="Red" ValidationExpression="^\d+$" ValidationGroup="validate"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlReTknUs" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkReTknUs" runat="server" CssClass="checkbox-inline" Checked="true" Text="Refresh token usage"></asp:CheckBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlUpdAccTknClRe" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkUpdAccTknClRe" runat="server" CssClass="checkbox-inline" Checked="true" Text="Update access token claims on refresh"></asp:CheckBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlReTknExp" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkReTknExp" runat="server" CssClass="checkbox-inline" Checked="true" Text="Refresh token expiration"></asp:CheckBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlEnLocLog" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkEnLocLog" runat="server" CssClass="checkbox-inline" Checked="true" Text="Enable local login"></asp:CheckBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlAlSenClntCl" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkAlSenClntCl" runat="server" CssClass="checkbox-inline" Text="Always send client claims"></asp:CheckBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="pnlClntClPre" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblClntClPre" runat="server" CssClass="control-label"  Text="Client claims prefix" Font-Bold="true"></asp:Label>
                                            <asp:TextBox ID="txtClntClPre" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnAddNewClient" runat="server" CssClass="btn btn-primary pull-right" Text="Add new client" Width="200" OnClick="btnAddNewClient_Click" ValidationGroup="validate" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Label ID="lblError" runat="server" CssClass="contentAlertDanger"></asp:Label>
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
