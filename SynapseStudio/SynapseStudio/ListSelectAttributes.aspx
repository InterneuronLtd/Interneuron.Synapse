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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListSelectAttributes.aspx.cs" Inherits="SynapseStudio.ListSelectAttributes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/themes/base/jquery-ui.css" rel="stylesheet" />
    <script src="Scripts/knockout-2.2.0.js"></script>
    <script src="Scripts/jquery-ui-1.12.1.js"></script>

    <script type="text/javascript">

        function PostData(service, data) {

            var service = service;
            var uri = service;

            return jQuery.ajax({
                data: data,
                dataType: "json",
                contentType: "application/json; charset=utf-8",
                url: encodeURI(uri),
                type: 'POST',
                success: function (result) {
                    // Do something with the result
                }
            });

        }


        function getAvailableAttributes() {
            var url = "ListSelectAttributes.aspx/GetAvailableAttributes";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "'}";

            return PostData(url, data)

        }

        function getSelectedAttributes() {


            var url = "ListSelectAttributes.aspx/GetSelectedAttributes";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "'}";
            return PostData(url, data)

        }


        function addAttributeToList(attributename) {
            console.log("Added: " + attributename);
            var ordinalposition = $('#lblNextOrdinalPosition').text();
            var url = "ListSelectAttributes.aspx/AddAttributeToList";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "','attributename':'" + attributename + "','ordinalposition':'" + ordinalposition + "'}";
            return PostData(url, data)

        }


        function removeAttributeFromList(attributename) {

            var ordinalposition = $('#lblNextOrdinalPosition').text();
            var url = "ListSelectAttributes.aspx/RemoveAttributeFromList";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "','attributename':'" + attributename + "'}";
            return PostData(url, data)

        }

        function GetNextOrdinalPosition() {
            var url = "ListSelectAttributes.aspx/GetNextOrdinalPosition";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "'}";
            return PostData(url, data)
        }

        function UpdateAttributeOrder(attributeId, order) {
            var url = "ListSelectAttributes.aspx/UpdateOrdinalPosition";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "','listattribute_id':'" + attributeId + "','ordinalposition':" + order + "}";
            return PostData(url, data)
        }


        function SaveQuickAttributes(attributeId, displayname, defaultcssclass) {
            var url = "ListSelectAttributes.aspx/SaveQuickAttributes";
            var listId = $('#listID').text();
            var data = "{'listId': '" + listId + "','listattribute_id':'" + attributeId + "','displayname':'" + displayname + "','defaultcssclassname':'" + defaultcssclass + "'}";
            return PostData(url, data)
        }

        function SelectUnselectAttribute(cb, attributeID) {

            console.log(cb.id);

            var li = '#avail' + cb.id;
            if (cb.checked) {
                $.when(
                    addAttributeToList(cb.id)
                ).done(
                    function (

                    ) {

                        getSelectedList();
                    })

                $(li).addClass('itemSelected');

            }
            else {
                $.when(
                    removeAttributeFromList(cb.id)
                ).done(
                    function (

                    ) {
                        getSelectedList();

                    })

                $(li).removeClass('itemSelected');
            }






        }

        function getNextOrdinalPositionForLabel() {

            $.when(
                GetNextOrdinalPosition()
            ).done(
                function (
                    nextOrdinalPosition
                ) {

                    var nop = nextOrdinalPosition.d + "";

                    $('#lblNextOrdinalPosition').text(nop);



                })

        }

        function getSelectedList() {

            $.when(
                getSelectedAttributes()
            ).done(
                function (
                    selectedList
                ) {


                    var html = "<ul id='selectedList'>";
                    $.each(selectedList.d, function (i, selectedItem) {


                        var func = "'" + selectedItem.listattribute_id + "'"; //, '" + selectedItem.attributename + "');";

                        html += "<li id='" + selectedItem.listattribute_id + "'>";
                        html += "<div class='row'><div class='col-md-9'>"
                        html += i + 1;
                        html += '.&nbsp;';
                        html += selectedItem.attributename;
                        html += "</div><div class='col-md-3'>"
                        //html += "<a href='#' onclick='showDetailModal(\"" + selectedItem.listattribute_id + "\",\"" + selectedItem.attributename + "\")' class='pull-right'><small>Edit</small></a>";
                        html += "<a href='#' onclick='showDetailModal(" + JSON.stringify(selectedItem) + ")' class='pull-right'><small>Edit</small></a>";
                        html += "</div></div>"
                        html += '</li>';
                    });
                    html += "</ul>"
                    $('#selectedAttributes').html(html);


                    $("#selectedList").sortable({
                        revert: true,
                        stop: function (event, ui) {

                            var sortOrder = $("#selectedList").sortable('toArray');

                            console.log(sortOrder.length);
                            //sortOrder = JSON.parse(sortOrder)

                            var arrayLength = sortOrder.length;
                            for (var i = 0; i < arrayLength; i++) {

                                UpdateAttributeOrder(sortOrder[i], i + 1);
                                getSelectedList();

                            }

                        }
                    });

                    getNextOrdinalPositionForLabel();


                })

        }

        $(document).ready(function () {


            var listID = getParameterByName('id');
            //console.log(listID);
            $('#listID').text(listID);

            getNextOrdinalPositionForLabel();

            $.when(
                getAvailableAttributes()
            ).done(
                function (
                    availableList
                ) {


                    var html = "<ul id='availableList'>";
                    $.each(availableList.d, function (i, availableItem) {

                        html += '<li id="avail' + availableItem.attributename + '"'
                        if (availableItem.isselected) {
                            html += ' class="itemSelected"';
                        }
                        html += '><input type="checkbox" id="' + availableItem.attributename + '"';
                        if (availableItem.isselected) {
                            html += ' checked';
                        }
                        html += ' onclick="SelectUnselectAttribute(this);"/>&nbsp;&nbsp;' + availableItem.attributename + '</li>';

                    });
                    html += "</ul";
                    $('#availableAttributes').html(html);

                    getSelectedList();

                })


            $('#btnQuickSave').click(function () {
                //alert('Saved');

                var attributeId = $('#attributeId').text();
                var displayname = $('#ContentPlaceHolder1_txtDisplayName').val();
                var defaultcssclass = $('#ContentPlaceHolder1_txtDefaultCSSClass').val();


                console.log(attributeId + "," + displayname + "," + defaultcssclass);

                SaveQuickAttributes(attributeId, displayname, defaultcssclass)

                $('#detail-modal').modal('hide');//modal_1 is the id 1

                getSelectedList();
            });

        });

        function showDetailModal(item) {
            //e.preventDefault();
            console.log(item);


            // Clear out current values from form
            $('#attributeName').text("");
            $('#attributeId').text("");
            $('#ContentPlaceHolder1_txtDisplayName').val("");
            $('#ContentPlaceHolder1_txtDefaultCSSClass').val("");

            $('#attributeName').text(item.attributename);
            $('#attributeId').text(item.listattribute_id);
            $('#ContentPlaceHolder1_txtDisplayName').val(item.displayname);
            $('#ContentPlaceHolder1_txtDefaultCSSClass').val(item.defaultcssclassname);


            $("#detail-modal").modal("show");
        }

    </script>

    <style type="text/css">
        .selectContainer {
            border: 1px dashed white;
            padding: 20px;
            margin: 20px;
        }



        #availableList {
            list-style-type: none;
            margin: 0;
            padding: 0;
            width: 99%;
        }

            #availableList li {
                margin: 5px;
                padding-left: 10px;
                padding-top: 5px;
                padding-bottom: 5px;
                font-size: 1.2em;
                width: 99%;
                border: 1px dashed gray;
                background-color: #333333;
            }


        .itemSelected {
            border: 1px dashed green !important;
            color: #ccff99 !important;
        }

        #selectedList {
            list-style-type: none;
            margin: 0;
            padding: 0;
            width: 99%;
        }

            #selectedList li {
                margin: 5px;
                padding-left: 10px;
                padding-top: 5px;
                padding-bottom: 5px;
                padding-right: 5px;
                font-size: 1.2em;
                width: 99%;
                border: 1px dashed gray;
                background-color: #333333;
                cursor: move; /* fallback if grab cursor is unsupported */
                cursor: grab;
                cursor: -moz-grab;
            }

                #selectedList li:active {
                    cursor: grabbing;
                    cursor: -moz-grabbing;
                    cursor: -webkit-grabbing;
                }

                #selectedList li span {
                    position: absolute;
                    margin-left: -1.3em;
                }
    </style>


</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div id="page-wrapper">




        <div class="modal" tabindex="-1" role="dialog" id="detail-modal">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span style="color: #b16060" aria-hidden="true">&times;</span>
                        </button>
                        <h3 class="modal-title">Attribute Properties</h3>
                    </div>
                    <div class="modal-body">
                        <span id="attributeId" class="hidden"></span>
                        <span class="lead" style="color: rgb(42, 159, 214);"><span class="text-uppercase" id="attributeName"></span></span>
                        <br />
                        <br />
                        <asp:Panel ID="fgDisplayName" runat="server" class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Label ID="lblDisplayName" runat="server" CssClass="control-label" for="txtDisplayName" Text="Column Heading Display Name" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <asp:Label ID="errDisplayName" runat="server"></asp:Label>
                            <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" MaxLength="255"></asp:TextBox>
                        </asp:Panel>

                        <asp:Panel ID="pnlDefaultCSSClass" runat="server" class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:Label ID="lblDefaultCSSClass" runat="server" CssClass="control-label" for="txtDefaultCSSClass" Text="Default CSS Class" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <asp:Label ID="errDefaultCSSClass" runat="server"></asp:Label>
                            <asp:TextBox ID="txtDefaultCSSClass" runat="server" CssClass="form-control" MaxLength="255"></asp:TextBox>
                        </asp:Panel>

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-primary" id="btnQuickSave">Save changes</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>


        <div>
            <asp:HiddenField ID="hdnListID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnNextOrdinalPosition" runat="server" />
            <asp:HiddenField ID="hdnDataType" runat="server" />
        </div>

        <div class="row" style="margin-top: 7px;">
            <div class="col-md-6">
                <h4 style="color: #fff;">List Manager - Select Attributes</h4>
            </div>
            <div class="col-md-6">
                <h4 style="color: lightgray;" class="pull-right">
                    <asp:Label ID="lblSummaryType" runat="server"></asp:Label></h4>
            </div>
        </div>



        <div class="well" style="margin-top: 15px;">
            <div class="row">                
                <div class="col-lg-3">
                    <asp:Button ID="btnViewDetails" runat="server" CssClass="btn btn-default btn-block" Text="Details" OnClick="btnViewDetails_Click" />
                </div>
                <%--<div class="col-lg-3">
                    <asp:Button ID="btnManageAttributes" runat="server" CssClass="btn btn-default btn-block" Text="Attributes" OnClick="btnManageAttributes_Click" />
                </div>--%>
                <div class="col-lg-3">
                    <asp:Button ID="btnSelectAttributes" runat="server" CssClass="btn btn-default btn-block" Text="Select Columns" OnClick="btnSelectAttributes_Click" />
                </div>
                <div class="col-lg-2">
                    <asp:Button ID="btnSelectQuestions" runat="server" CssClass="btn btn-default btn-block" Text="Select Questions" OnClick="btnSelectQuestions_Click" />
                </div>
                <div class="col-lg-3">
                    <asp:Button ID="btnManageAPI" runat="server" CssClass="btn btn-default btn-block" Text="APIs" OnClick="btnManageAPI_Click" />
                </div>                                
            </div>
        </div>


        <div class="hidden">
            <h4>ListID: <span id="listID"></span></h4>
            <h4>Next Ordinal Position: <span id="lblNextOrdinalPosition"></span></h4>
        </div>


        <div class="row">

            <div class="col-md-6">
                <div class="selectContainer">
                    <p style="font-size: 1.8em; font-weight: bold">Available Attributes from Baseview</p>
                    <div id="availableAttributes">
                    </div>
                </div>
            </div>


            <div class="col-md-6">
                <div class="selectContainer">
                    <p style="font-size: 1.8em; font-weight: bold">Selected Attributes</p>
                    <div id="selectedAttributes"></div>
                </div>
            </div>


            <div class="well" style="margin-top: 15px;">
                <div class="row">
                    <div class="col-lg-12">
                        <asp:HyperLink ID="hlPreview" runat="server" CssClass="btn btn-default btn-block" Text="Preview List"></asp:HyperLink>
                    </div>
                </div>
            </div>

        </div>


    </div>

</asp:Content>
