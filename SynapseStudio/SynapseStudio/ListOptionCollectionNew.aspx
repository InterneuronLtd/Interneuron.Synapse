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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListOptionCollectionNew.aspx.cs" Inherits="SynapseStudio.ListOptionCollectionNew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="Question Option Collections"></asp:Label>
                </h1>
            </div>
        </div>

        <div>
            <asp:HiddenField ID="hdnOptionCollectionID" runat="server" />
            
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-database"></i>&nbsp;New List Option Collection</h3>
                    </div>
                    <div class="panel-body">                       
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgCollectionName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblCollectionName" runat="server" CssClass="control-label" for="txtCollectionName" Text="* Please enter a name for the option collection" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errCollectionName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtCollectionName" runat="server" CssClass="form-control input-lg" MaxLength="255"></asp:TextBox>
                                </asp:Panel>

                                <asp:Panel ID="fgCollectionDescription" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblCollectionDescription" runat="server" CssClass="control-label" for="txtCollectionDescription" Text="Enter a description" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errCollectionDescription" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtCollectionDescription" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="6"></asp:TextBox>
                                </asp:Panel>


                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">                               
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary pull-right" Text="Save" Width="200" OnClick="btnSave_Click" />
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
                                <asp:DataGrid ID="dgOptions" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="optiondisplaytext" HeaderText="Display">
                                            <HeaderStyle Width="25%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="optiondisplaytext" HeaderText="Value">
                                            <HeaderStyle Width="25%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="optionflag" HeaderText="Flag">
                                            <HeaderStyle Width="25%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="questionoption_id" DataNavigateUrlFormatString="ListOptionView.aspx?&action=view&id={0}" Text="View">
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
