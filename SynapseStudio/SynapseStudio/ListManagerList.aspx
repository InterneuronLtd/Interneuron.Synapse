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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListManagerList.aspx.cs" Inherits="SynapseStudio.ListManagerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="List Manager"></asp:Label>
                </h1>
            </div>
        </div>



        <div class="row">
            <div class="col-lg-4">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-list"></i>&nbsp;Lists</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Panel ID="fgtesttypeid" runat="server" class="form-group">
                                    <asp:Label ID="lbltesttypeid" runat="server" CssClass="control-label" for="ddltesttypeid" Text="Select a namespace from the list below" Font-Bold="true"></asp:Label>
                                    <asp:Label ID="errtesttypeid" runat="server"></asp:Label>
                                    <asp:DropDownList ID="ddlSynapseNamespace" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlSynapseNamespace_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnCreateNewList" runat="server" CssClass="btn btn-primary pull-right" Text="New" OnClick="btnCreateNewList_Click" Width="200" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">
                                    <asp:Label ID="lblTestType" runat="server"></asp:Label>
                                    <asp:Label ID="lblNamespaceName" runat="server"></asp:Label>
                                    LISTS (<asp:Label ID="lblResultCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgEntities" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="listname" HeaderText="List">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="list_id" DataNavigateUrlFormatString="ListSelectAttributes.aspx?&action=view&id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
                                    </Columns>



                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="col-lg-4">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-question"></i>&nbsp;Questions</h3>
                    </div>
                    <div class="panel-body">                       
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAddNewQuestion" runat="server" CssClass="btn btn-primary pull-right" Text="New Question" OnClick="btnAddNewQuestion_Click" Width="200" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                    <asp:Label ID="Label2" runat="server"></asp:Label>
                                    QUESTIONS (<asp:Label ID="lblQuestionCount" runat="server"></asp:Label>)</span>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgQuestions" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="defaultcontextfieldname" HeaderText="Context">
                                            <HeaderStyle Width="30%" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="questionquickname" HeaderText="Question">
                                            <HeaderStyle Width="45%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="question_id" DataNavigateUrlFormatString="ListQuestionView.aspx?&action=view&id={0}" Text="View">
                                            <HeaderStyle Width="25%" />
                                        </asp:HyperLinkColumn>
                                    </Columns>



                                </asp:DataGrid>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="col-lg-4">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-ellipsis-v"></i>&nbsp;Question Option Collections</h3>
                    </div>
                    <div class="panel-body">                       
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnAddNewOptionCollection" runat="server" CssClass="btn btn-primary pull-right" Text="New Collection" OnClick="btnAddNewOptionCollection_Click" Width="200" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <span style="font-size: 1.8em;">
                                    COLLECTIONS (<asp:Label ID="lblQuestionCollectionCount" runat="server"></asp:Label>)</span>
                            </div>                          
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-12">
                                <asp:DataGrid ID="dgCollections" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">

                                    <Columns>
                                        <asp:BoundColumn DataField="questionoptioncollectionname" HeaderText="Collection">
                                            <HeaderStyle Width="75%" />
                                        </asp:BoundColumn>
                                        <asp:HyperLinkColumn DataNavigateUrlField="questionoptioncollection_id" DataNavigateUrlFormatString="ListOptionCollectionView.aspx?&action=view&id={0}" Text="View">
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