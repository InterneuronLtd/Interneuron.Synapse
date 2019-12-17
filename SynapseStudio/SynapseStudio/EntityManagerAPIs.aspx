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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntityManagerAPIs.aspx.cs" Inherits="SynapseStudio.EntityManagerAPIs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div id="page-wrapper">

        <div>
            <asp:HiddenField ID="hdnEntityID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnNextOrdinalPosition" runat="server" />
            <asp:HiddenField ID="hdnDataType" runat="server" />
            <asp:HiddenField ID="hdnAPIURL" runat="server" />
            <asp:HiddenField ID="hdnNamespace" runat="server" />
            <asp:HiddenField ID="hdnEntityName" runat="server" />
            <asp:HiddenField ID="hdnKeyAttribute" runat="server" />
        </div>

        <h3>Entity APIs</h3>

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
                        <h3 class="panel-title"><i class="fa fa-cog"></i>&nbsp;APIs</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-lg-12">
                                <h1>
                                    <asp:Label ID="lblSummaryType" runat="server"></asp:Label>
                                </h1>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-success">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-address-card"></i>&nbsp;Get Object By ID</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table">
                                    <tr>
                                        <td style="width: 20%;">Method
                                        </td>
                                        <td style="width: 80%;">GetObject
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">HTTP Verb
                                        </td>
                                        <td style="width: 80%;">
                                            GET
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Method Description
                                        </td>
                                        <td style="width: 80%;">Returns a single JSON object by filtering the Key Attribute of entity with the supplied ID parameter.<br />
                                            Response can be formatted using optional parameters
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">URL
                                        </td>
                                        <td style="width: 80%;">
                                            <asp:HyperLink ID="hlGetObject" runat="server" Target="_blank" ForeColor="AliceBlue"></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Required Parameters</td>
                                        <td style="width: 80%;">
                                            <table class="table">
                                                <tr>
                                                    <td style="width: 20%;"><strong>Parameter</strong></td>
                                                    <td style="width: 40%;"><strong>Example Usuage</strong></td>
                                                    <td style="width: 40%;"><strong>Description</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapsenamespace</td>
                                                    <td style="width: 40%;">?synapsenamespace=<asp:Label ID="lblnamespace1" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Namespace of the entity</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapseentityname</td>
                                                    <td style="width: 40%;">&synapseentityname=<asp:Label ID="lblentity1" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Entity Name</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">id</td>
                                                    <td style="width: 40%;">&id=00000000-0000-0000-0000-000000000000</td>
                                                    <td style="width: 40%;">Key Attribute for the entity (<asp:Label ID="lblkey1" runat="server"></asp:Label>)</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Optional Parameters</td>
                                        <td style="width: 80%;">
                                            <table class="table table-bordered  table-striped">
                                                <tr>
                                                    <td style="width: 20%;"><strong>Parameter</strong></td>
                                                    <td style="width: 40%;"><strong>Example Usuage</strong></td>
                                                    <td style="width: 40%;"><strong>Description</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">returnsystemattributes</td>
                                                    <td style="width: 40%;">&returnsystemattributes=1</td>
                                                    <td style="width: 40%;">Return System Attributes along with response.<br />
                                                        returnsystemattributes = 1 returns all system atributes</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-success">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-list"></i>&nbsp;Get List</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table">
                                    <tr>
                                        <td style="width: 20%;">Method
                                        </td>
                                        <td style="width: 80%;">GetList
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">HTTP Verb
                                        </td>
                                        <td style="width: 80%;">
                                            GET
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Method Description
                                        </td>
                                        <td style="width: 80%;">Returns a JSON list of the entity.<br />
                                            Can be filtered and the response formatted using optional parameters.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">URL
                                        </td>
                                        <td style="width: 80%;">
                                            <asp:HyperLink ID="hlGetList" runat="server" Target="_blank" ForeColor="AliceBlue"></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Required Parameters</td>
                                        <td style="width: 80%;">
                                            <table class="table">
                                                <tr>
                                                    <td style="width: 20%;"><strong>Parameter</strong></td>
                                                    <td style="width: 40%;"><strong>Example Usuage</strong></td>
                                                    <td style="width: 40%;"><strong>Description</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapsenamespace</td>
                                                    <td style="width: 40%;">?synapsenamespace=<asp:Label ID="lblnamespace2" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Namespace of the entity</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapseentityname</td>
                                                    <td style="width: 40%;">&synapseentityname=<asp:Label ID="lblentity2" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Entity Name</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Optional Parameters</td>
                                        <td style="width: 80%;">
                                            <table class="table table-bordered  table-striped">
                                                <tr>
                                                    <td style="width: 20%;"><strong>Parameter</strong></td>
                                                    <td style="width: 40%;"><strong>Example Usuage</strong></td>
                                                    <td style="width: 40%;"><strong>Description</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">returnsystemattributes</td>
                                                    <td style="width: 40%;">&returnsystemattributes=1</td>
                                                    <td style="width: 40%;">Return System Attributes along with response.<br />
                                                        returnsystemattributes = 1 returns all system atributes</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">orderby</td>
                                                    <td style="width: 40%;">&orderby=_sequenceid ASC</td>
                                                    <td style="width: 40%;">Order by clause</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">limit</td>
                                                    <td style="width: 40%;">&limit=100</td>
                                                    <td style="width: 40%;">Limit the returned resultset to the number of rows specified</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">offset</td>
                                                    <td style="width: 40%;">?offset=10</td>
                                                    <td style="width: 40%;">Exclude the first number of rows specified from the result set<br />
                                                        * Only useful when order by specified<br />
                                                        ** Useful for paging</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-success">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-list-ol"></i>&nbsp;Get List by Attribute Value</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table">
                                    <tr>
                                        <td style="width: 20%;">Method
                                        </td>
                                        <td style="width: 80%;">GetListByAttribute
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">HTTP Verb
                                        </td>
                                        <td style="width: 80%;">
                                            GET
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Method Description
                                        </td>
                                        <td style="width: 80%;">Returns a JSON list of the entity using the value of a supplied attribute.<br />
                                            Can be filtered and the response formatted using optional parameters.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">URL
                                        </td>
                                        <td style="width: 80%;">
                                            <asp:HyperLink ID="hlGetListByID" runat="server" Target="_blank" ForeColor="AliceBlue"></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Required Parameters</td>
                                        <td style="width: 80%;">
                                            <table class="table">
                                                <tr>
                                                    <td style="width: 20%;"><strong>Parameter</strong></td>
                                                    <td style="width: 40%;"><strong>Example Usuage</strong></td>
                                                    <td style="width: 40%;"><strong>Description</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapsenamespace</td>
                                                    <td style="width: 40%;">?synapsenamespace=<asp:Label ID="lblnamespace3" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Namespace of the entity</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapseentityname</td>
                                                    <td style="width: 40%;">&synapseentityname=<asp:Label ID="lblentity3" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Entity Name</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapseattributename</td>
                                                    <td style="width: 40%;">&synapseattributename=person_id</td>
                                                    <td style="width: 40%;">Attribute name that you want to filter the list on</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">attributevalue</td>
                                                    <td style="width: 40%;">&attributevalue=123</td>
                                                    <td style="width: 40%;">Value that you want to filter the supplied attribute against</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Optional Parameters</td>
                                        <td style="width: 80%;">
                                            <table class="table table-bordered  table-striped">
                                                <tr>
                                                    <td style="width: 20%;"><strong>Parameter</strong></td>
                                                    <td style="width: 40%;"><strong>Example Usuage</strong></td>
                                                    <td style="width: 40%;"><strong>Description</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">returnsystemattributes</td>
                                                    <td style="width: 40%;">&returnsystemattributes=1</td>
                                                    <td style="width: 40%;">Return System Attributes along with response.<br />
                                                        returnsystemattributes = 1 returns all system atributes</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">orderby</td>
                                                    <td style="width: 40%;">&orderby=_sequenceid ASC</td>
                                                    <td style="width: 40%;">Order by clause</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">limit</td>
                                                    <td style="width: 40%;">&limit=100</td>
                                                    <td style="width: 40%;">Limit the returned resultset to the number of rows specified</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">offset</td>
                                                    <td style="width: 40%;">?offset=10</td>
                                                    <td style="width: 40%;">Exclude the first number of rows specified from the result set<br />
                                                        * Only useful when order by specified<br />
                                                        ** Useful for paging</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-warning">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-plus"></i>&nbsp;Post Object</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table">
                                    <tr>
                                        <td style="width: 20%;">Method
                                        </td>
                                        <td style="width: 80%;">PostObject
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">HTTP Verb
                                        </td>
                                        <td style="width: 80%;">
                                            POST
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Method Description
                                        </td>
                                        <td style="width: 80%;">Post an object to insert a new record or update an existing record. <br />
                                            To update an existing record, ensure that the Key Attribute value (<asp:Label ID="lblkey2" runat="server"></asp:Label>) matches a key attribute value in the entity store. <br />
                                            To insert a new record pass a new value for the Key Attribute value (<asp:Label ID="lblkey3" runat="server"></asp:Label>).
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">URL
                                        </td>
                                        <td style="width: 80%;">
                                            <asp:HyperLink ID="hlPostObject" runat="server" Target="_blank" ForeColor="AliceBlue"></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Required Parameters</td>
                                        <td style="width: 80%;">
                                            <table class="table">
                                                <tr>
                                                    <td style="width: 20%;"><strong>Parameter</strong></td>
                                                    <td style="width: 40%;"><strong>Example Usuage</strong></td>
                                                    <td style="width: 40%;"><strong>Description</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapsenamespace</td>
                                                    <td style="width: 40%;">?synapsenamespace=<asp:Label ID="lblnamespace4" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Namespace of the entity</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapseentityname</td>
                                                    <td style="width: 40%;">&synapseentityname=<asp:Label ID="lblentity4" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Entity Name</td>
                                                </tr>                                                
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Optional Parameters</td>
                                        <td style="width: 80%;">
                                            * There are no optional parameters for this method
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Example Value</td>
                                        <td style="width: 80%;">
                                            <div id="divPostSample" runat="server"></div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-danger">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-trash"></i>&nbsp;Delete Object</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table">
                                    <tr>
                                        <td style="width: 20%;">Method
                                        </td>
                                        <td style="width: 80%;">DeleteObject
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">HTTP Verb
                                        </td>
                                        <td style="width: 80%;">
                                            DELETE
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Method Description
                                        </td>
                                        <td style="width: 80%;">Soft Deletes the record based on the supplied Key Attribute value (<asp:Label ID="lblkey4" runat="server"></asp:Label>)<br />
                                            All previous versions of the record in the entity store will be marked with _recordstatus = 2
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">URL
                                        </td>
                                        <td style="width: 80%;">
                                            <asp:HyperLink ID="hlDeleteObject" runat="server" Target="_blank" ForeColor="AliceBlue"></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Required Parameters</td>
                                        <td style="width: 80%;">
                                            <table class="table">
                                                <tr>
                                                    <td style="width: 20%;"><strong>Parameter</strong></td>
                                                    <td style="width: 40%;"><strong>Example Usuage</strong></td>
                                                    <td style="width: 40%;"><strong>Description</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapsenamespace</td>
                                                    <td style="width: 40%;">?synapsenamespace=<asp:Label ID="lblnamespace5" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Namespace of the entity</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapseentityname</td>
                                                    <td style="width: 40%;">&synapseentityname=<asp:Label ID="lblentity5" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Entity Name</td>
                                                </tr>         
                                                <tr>
                                                    <td style="width: 20%;">id</td>
                                                    <td style="width: 40%;">&id=00000000-0000-0000-0000-000000000000</td>
                                                    <td style="width: 40%;">Key Attribute for the entity (<asp:Label ID="lblkey5" runat="server"></asp:Label>)</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Optional Parameters</td>
                                        <td style="width: 80%;">
                                            * There are no optional parameters for this method
                                        </td>
                                    </tr>                                   
                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>



        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-danger">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-trash"></i>&nbsp;Delete Objects By Attribute</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table">
                                    <tr>
                                        <td style="width: 20%;">Method
                                        </td>
                                        <td style="width: 80%;">DeleteObjectByAttribute
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">HTTP Verb
                                        </td>
                                        <td style="width: 80%;">
                                            DELETE
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Method Description
                                        </td>
                                        <td style="width: 80%;">Soft Deletes the record based on the supplied Attribute and Attribute value <br />
                                            All previous versions of the record in the entity store will be marked with _recordstatus = 2
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">URL
                                        </td>
                                        <td style="width: 80%;">
                                            <asp:HyperLink ID="hlDeleteByAttribute" runat="server" Target="_blank" ForeColor="AliceBlue"></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Required Parameters</td>
                                        <td style="width: 80%;">
                                            <table class="table">
                                                <tr>
                                                    <td style="width: 20%;"><strong>Parameter</strong></td>
                                                    <td style="width: 40%;"><strong>Example Usuage</strong></td>
                                                    <td style="width: 40%;"><strong>Description</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapsenamespace</td>
                                                    <td style="width: 40%;">?synapsenamespace=<asp:Label ID="Label2" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Namespace of the entity</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapseentityname</td>
                                                    <td style="width: 40%;">&synapseentityname=<asp:Label ID="Label3" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Entity Name</td>
                                                </tr>         
                                                <tr>
                                                    <td style="width: 20%;">synapseattributename</td>
                                                    <td style="width: 40%;">&synapseattributename=person_id</td>
                                                    <td style="width: 40%;">Attribute name that you want to filter the list on</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">attributevalue</td>
                                                    <td style="width: 40%;">&attributevalue=123</td>
                                                    <td style="width: 40%;">Value that you want to filter the supplied attribute against</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Optional Parameters</td>
                                        <td style="width: 80%;">
                                            * There are no optional parameters for this method
                                        </td>
                                    </tr>                                   
                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-success">
                    <div class="panel-heading">
                        <h3 class="panel-title"><i class="fa fa-history"></i>&nbsp;Get Object History</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table">
                                    <tr>
                                        <td style="width: 20%;">Method
                                        </td>
                                        <td style="width: 80%;">GetObjectHistory
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">HTTP Verb
                                        </td>
                                        <td style="width: 80%;">
                                            GET
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Method Description
                                        </td>
                                        <td style="width: 80%;">Gets a JSON list of objects based on the supplied Key Attribute value (<asp:Label ID="lblkey6" runat="server"></asp:Label>)
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">URL
                                        </td>
                                        <td style="width: 80%;">
                                            <asp:HyperLink ID="hlGetObjectHistory" runat="server" Target="_blank" ForeColor="AliceBlue"></asp:HyperLink>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Required Parameters</td>
                                        <td style="width: 80%;">
                                            <table class="table">
                                                <tr>
                                                    <td style="width: 20%;"><strong>Parameter</strong></td>
                                                    <td style="width: 40%;"><strong>Example Usuage</strong></td>
                                                    <td style="width: 40%;"><strong>Description</strong></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapsenamespace</td>
                                                    <td style="width: 40%;">?synapsenamespace=<asp:Label ID="lblnamespace6" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Namespace of the entity</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20%;">synapseentityname</td>
                                                    <td style="width: 40%;">&synapseentityname=<asp:Label ID="lblentity6" runat="server"></asp:Label></td>
                                                    <td style="width: 40%;">Entity Name</td>
                                                </tr>         
                                                <tr>
                                                    <td style="width: 20%;">id</td>
                                                    <td style="width: 40%;">&id=00000000-0000-0000-0000-000000000000</td>
                                                    <td style="width: 40%;">Key Attribute for the entity (<asp:Label ID="Label4" runat="server"></asp:Label>)</td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%;">Optional Parameters</td>
                                        <td style="width: 80%;">
                                            * There are no optional parameters for this method
                                        </td>
                                    </tr>                                   
                                </table>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </div>
</asp:Content>


