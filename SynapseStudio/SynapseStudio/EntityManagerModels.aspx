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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntityManagerModels.aspx.cs" Inherits="SynapseStudio.EntityManagerModels" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">



        <div>
            <asp:HiddenField ID="hdnEntityID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnNextOrdinalPosition" runat="server" />
            <asp:HiddenField ID="hdnDataType" runat="server" />
        </div>

        <h3>Entity Models</h3>

        <div class="well" style="margin-top: 15px;">
            <div class="row">
                <div class="col-lg-2">
                    <asp:Button ID="btnViewDetails" runat="server" CssClass="btn btn-default btn-block" Text="Details" OnClick="btnViewDetails_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnManageAttributes" runat="server" CssClass="btn btn-default btn-block" Text="Attributes" OnClick="btnManageAttributes_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnManageRelations" runat="server" CssClass="btn btn-default btn-block" Text="Relations" OnClick="btnManageRelations_Click" />
                </div>
                <div class="col-lg-2">
                    <asp:Button ID="btnManageAPI" runat="server" CssClass="btn btn-default btn-block" Text="APIs" OnClick="btnManageAPI_Click" />
                </div>
                <div class="col-lg-2">
                    <asp:Button ID="btnManageModels" runat="server" CssClass="btn btn-default btn-block" Text="Models" OnClick="btnManageModels_Click" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-primary">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-tasks"></i>&nbsp;<asp:Label ID="lblHeading" runat="server" Text="Models"></asp:Label>
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
                                <h3><img src="img/csharp.png" alt="C Sharp" style="height: 45px;" />&nbsp;C# Model</h3>
                                <asp:TextBox TextMode="MultiLine" Rows="20" ID="txtCSharpDataModel" runat="server" CssClass="form-control input-lg"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-12">
                                <h3><img src="img/swift.png" alt="C Sharp" style="height: 45px;" />&nbsp;Swift Model</h3>
                                <asp:TextBox TextMode="MultiLine" Rows="20" ID="txtSwiftDataModel" runat="server" CssClass="form-control input-lg"></asp:TextBox>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>
