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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListQuestionNew.aspx.cs" Inherits="SynapseStudio.ListQuestionNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-12">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="List Question Manager"></asp:Label>
                </h1>
            </div>
        </div>

        <div>
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnQuestionID" runat="server" />
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-question"></i>&nbsp;New Question</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">

                                <h4>Details</h4>

                                <asp:Panel ID="fgQuickName" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblQuickName" runat="server" CssClass="control-label" for="txtQuickName" Text="Enter a name for the question" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errQuickName" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtQuickName" runat="server" CssClass="form-control input-lg"></asp:TextBox>
                                </asp:Panel>

                                <%--  <asp:Panel ID="fgQuestionDescription" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblQuestionDescription" runat="server" CssClass="control-label" for="txtQuestionDescription" Text="Enter a description for the question" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errQuestionDescription" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtQuestionDescription" runat="server" CssClass="form-control input-lg" TextMode="MultiLine"></asp:TextBox>
                                </asp:Panel>--%>


                                <h4>Context</h4>


                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="fgDefaultContext" runat="server" class="form-group">
                                            <asp:Label ID="lblDefaultContext" runat="server" CssClass="control-label" for="ddlDefaultContext" Text="Select a the entity that defines the default context for this list" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errDefaultContext" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlDefaultContext" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlDefaultContext_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <h4 style="color: #00ff21">Context Field:
                                            <asp:Label ID="lblDefaultContextField" runat="server"></asp:Label>
                                        </h4>
                                    </div>
                                </div>




                                <h4>Question Type</h4>


                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Panel ID="fgQuestionType" runat="server" class="form-group">
                                            <asp:Label ID="lblQuestionType" runat="server" CssClass="control-label" for="ddlQuestionType" Text="Select a question type from the list below" Font-Bold="true"></asp:Label>
                                            <asp:Label ID="errQuestionType" runat="server"></asp:Label>
                                            <asp:DropDownList ID="ddlQuestionType" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlQuestionType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </asp:Panel>
                                    </div>
                                </div>


                                <asp:Panel ID="fgLabelText" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblLabelText" runat="server" CssClass="control-label" for="txtLabelText" Text="* Enter the text for the label" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errLabelText" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtLabelText" runat="server" CssClass="form-control input-lg"></asp:TextBox>
                                </asp:Panel>

                                <asp:Panel ID="fgCustomHTML" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblCustomHTML" runat="server" CssClass="control-label" for="txtCustomHTML" Text="* enter the custom HTML that you wish to display" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errCustomHTML" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtCustomHTML" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                </asp:Panel>

                                <asp:Panel ID="fgCustomHTMLAlt" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblCustomHTMLAlt" runat="server" CssClass="control-label" for="txtCustomHTMLAlt" Text="* enter the custom HTML that you wish to display for unselected option" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errCustomHTMLAlt" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtCustomHTMLAlt" runat="server" CssClass="form-control input-lg" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                </asp:Panel>



                                <asp:Panel ID="fgDefaultValueText" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblDefaultValueText" runat="server" CssClass="control-label" for="txtDefaultValueText" Text="Enter any default value you would like to set" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errDefaultValueText" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtDefaultValueText" runat="server" CssClass="form-control input-lg"></asp:TextBox>
                                </asp:Panel>

                                <asp:Panel ID="fgDefaultValueDate" runat="server" class="form-group">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblDefaultValueDate" runat="server" CssClass="control-label" for="txtDefaultValueDate" Text="Enter any default value you would like to set" Font-Bold="true"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="errDefaultValueDate" runat="server"></asp:Label>
                                    <asp:TextBox ID="txtDefaultValueDate" runat="server" CssClass="form-control input-lg"></asp:TextBox>
                                </asp:Panel>


                                <asp:Panel ID="pnlOptions" runat="server">


                                    <h4>Options</h4>


                                    <div class="row">
                                        <div class="col-md-12">
                                            <div id="fgOptionType" runat="server" class="form-group">
                                                <asp:Label ID="lblOptionType" runat="server" CssClass="control-label" for="ddlOptionType" Text="* Select the option type (Internal or SQL Query)" Font-Bold="true"></asp:Label>
                                                <asp:Label ID="errOptionType" runat="server"></asp:Label>
                                                <asp:DropDownList ID="ddlOptionType" runat="server" CssClass="form-control input-lg" AutoPostBack="true" OnSelectedIndexChanged="ddlOptionType_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                    </div>


                                    <div id="fgOptionCollection" runat="server" class="form-group">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label ID="lblOptionCollection" runat="server" CssClass="control-label" for="ddlOptionGroup" Text="* Select Internal Option Collection" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>

                                        <asp:Label ID="errOptionCollection" runat="server"></asp:Label>
                                        <asp:DropDownList ID="ddlOptionCollection" runat="server" CssClass="form-control input-lg"></asp:DropDownList>
                                    </div>


                                    <div id="fgOptionSQLStatement" runat="server" class="form-group">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:Label ID="lblOptionSQLStatement" runat="server" CssClass="control-label" for="txtOptionSQLStatement" Text="Enter the SQL Statement to retrieve your options" Font-Bold="true"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="alert alert-info">
                                            <h4>SQL Requirements</h4>
                                            SQL Statemust must return fields called displayfield, valuefield and flagfield                                         
                                        </div>
                                        <asp:Label ID="errOptionSQLStatement" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtOptionSQLStatement" runat="server" CssClass="form-control input-lg" TextMode="MultiLine"></asp:TextBox>
                                    </div>

                                </asp:Panel>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnValidateList" runat="server" CssClass="btn btn-info pull-right" Text="Validate" Width="200" OnClick="btnValidateList_Click" />
                                <asp:Button ID="btnCancel" runat="server" CssClass="btn btn-info pull-left" Text="Cancel" Width="200" OnClick="btnCancel_Click" />
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary pull-right" Text="Create Question" Width="200" OnClick="btnSave_Click" />
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

</asp:Content>
