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

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SchemaImport.aspx.cs" Inherits="SynapseStudio.SchemaImport" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="page-wrapper">
        <div class="row">
            <div class="col-lg-5">
                <h1>
                    <asp:Label ID="lblSummaryType" runat="server" Text="List Manager"> Import Schema</asp:Label>
                </h1>
            </div>
        </div>

        <div class="row">
            <asp:Panel ID="pnlSelectFile" runat="server">
                <div class="col-lg-5">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa fa-list"></i>&nbsp;Import Objects On This Server</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-12">
                                <div class="row">
                                    <asp:Label ID="lblMsgSelectObjects" runat="server" CssClass="control-label" for="tv" Text="Please select a synapse schema export file" Font-Bold="true"></asp:Label>
                                    <div class="col-md-12"></div>
                                    <div class="col-md-12"></div>

                                    <asp:FileUpload CssClass="btn btn-block pull-left" ID="fImport" runat="server" />
                                    <br />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnPreProcessExport" runat="server" CssClass="btn btn-primary pull-right" Text="Process Export" Width="200" OnClick="btnPreProcessExport_Click" />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <br />
                                            <asp:Label ID="lblError" runat="server" CssClass="text-danger"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlSelectObjects" Visible="false" runat="server">
                <div class="col-lg-7">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <h3 class="panel-title"><i class="fa fa-list"></i>&nbsp;Select Objects To Import</h3>
                        </div>
                        <div class="panel-body">
                            <div class="col-md-12">
                                <div class="row">

                                    <asp:Label ID="Label1" runat="server" CssClass="control-label" for="tv" Text="Please select objects to import Or,  " Font-Bold="true">  </asp:Label>

                                    <asp:LinkButton ID="btnOtherFile" runat="server" CssClass="warning" Text="Select a different file" Width="200" OnClick="btnOtherFile_Click" />
                                    <asp:Button ID="btnImportTop" runat="server" CssClass="btn btn-primary pull-right" Text="Import Selected Objects" Width="200" OnClick="btnImport_Click" />


                                    <div class="col-md-12"></div>
                                    <div class="col-md-12"></div>
                                    <asp:TreeView ID="tv" runat="server" OnTreeNodeCheckChanged="tv_TreeNodeCheckChanged"></asp:TreeView>
                                    <br />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Button ID="btnImport" runat="server" CssClass="btn btn-primary pull-right" Text="Import Selected Objects" Width="200" OnClick="btnImport_Click" />

                                    <div class="row">
                                        <div class="col-md-12">
                                            <br />
                                            <asp:Label ID="lblErrorSelectObjects" runat="server" CssClass="text-danger"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>

        </div>
        <div class="row">

            <asp:Panel ID="pnlStatus" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server" />
                <asp:Timer runat="server" ID="UpdateTimer" Interval="2000" OnTick="UpdateTimer_Tick" />
                <asp:UpdatePanel runat="server" ID="TimedPanel" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="UpdateTimer" EventName="Tick" />
                        <asp:PostBackTrigger ControlID="btnOk" />
                    </Triggers>
                    <ContentTemplate>

                        <div class="col-lg-7">
                            <div class="panel panel-primary">
                                <div class="panel-heading">
                                    <h3 class="panel-title"><i class="fa fa-list"></i>&nbsp;Import Status</h3>
                                </div>
                                <div class="panel-body">
                                    <div class="col-md-12">

                                        <asp:Panel ID="pnlmsg" runat="server" ScrollBars="Auto">
                                            <div style="background-color: white" runat="server" id="divmsg"></div>

                                        </asp:Panel>

                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:Button ID="btnOk" CssClass="btn btn-primary pull-right" runat="server" Text="Done" OnClick="btnOk_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>






                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>

    </div>
</asp:Content>
