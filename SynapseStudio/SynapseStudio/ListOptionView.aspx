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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListOptionView.aspx.cs" Inherits="SynapseStudio.ListOptionView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Question Option"></asp:Label>
                </h1>
            </div>
        </div>

        <div>
            <asp:HiddenField ID="hdnOptionCollectionID" runat="server" />
            <asp:HiddenField ID="hdnOptionID" runat="server" />
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-question"></i>&nbsp;List Option</h3>
                        <h4>List Option Collection:
                            <asp:Label ID="lblListOptionCollection" runat="server"></asp:Label></h4>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgOptionValueText" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblOptionValueText" runat="server" CssClass="control-label" for="txtOptionValueText" Text="* Please enter the value that you want to assocaite with the option" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errOptionValueText" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtOptionValueText" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>


                                <asp:Panel ID="fgOptionDisplayText" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblOptionDisplayText" runat="server" CssClass="control-label" for="txtOptionValueText" Text="* Please enter the value that you want to display for the option" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errOptionDisplayText" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtOptionDisplayText" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>


                                <asp:Panel ID="fgOptionFlag" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblOptionFlag" runat="server" CssClass="control-label" for="txtOptionFlag" Text="Enter the HTML snippet for the flag if selected" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errOptionFlag" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtOptionFlag" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="6"></asp:TextBox>
                                </asp:Panel>

                                <asp:Panel ID="fgOptionFlagAlt" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblOptionFlagAlt" runat="server" CssClass="control-label" for="txtOptionFlagAlt" Text="Enter the HTML snippet for the flag if not selected" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errOptionFlagAlt" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtOptionFlagAlt" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="6"></asp:TextBox>
                                </asp:Panel>


                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                            </div>
                            <div class="col-md-4" style="text-align: center;">
                                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-danger" Text="Delete" Width="200" OnClick="btnDelete_Click" />
                            </div>
                            <div class="col-md-4">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary pull-right" Text="Save" Width="200" OnClick="btnSave_Click" />
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

</asp:Content>

