
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
<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ApplicationModuleMapping.aspx.cs" Inherits="SynapseStudio.ApplicationModuleMapping" %>
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
            var url = "ApplicationModuleMapping.aspx/LoadModuleList";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "'}";

            return PostData(url, data);

        }

        function getSelectedAttributes() {


            var url = "ApplicationModuleMapping.aspx/GetMappedModules";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "'}";
            return PostData(url, data)

        }


        function addAttributeToList(attributename) {
            debugger;
            console.log("Added: " + attributename);
            var ordinalposition = $('#lblNextOrdinalPosition').text();
            var url = "ApplicationModuleMapping.aspx/MapModuletoApplication";
            var id = $('#listID').text();
            var data = "{'listId': '" + attributename.name + "','attributename':'" + attributename.id + "','ordinalposition':'" + 1 + "'}";
            return PostData(url, data)

        }


        function removeAttributeFromList(attributename) {

            var ordinalposition = $('#lblNextOrdinalPosition').text();
            var url = "ApplicationModuleMapping.aspx/UnMapModulefromApplication";
            var id = $('#listID').text();
            var data = "{'listId': '" + attributename.name + "','attributename':'" + attributename.id + "'}";
            return PostData(url, data)

        }

       


        function SaveQuickAttributes(attributeId, displayname, isdefaultmodule) {
            var url = "ApplicationModuleMapping.aspx/SaveDisplayName";
            var listId = $('#listID').text();
            var data = "{'listId': '" + attributeId + "','displayname':'" + displayname + "','isdefaultmodule':'" + isdefaultmodule + "'}";
            return PostData(url, data)
        }

        function SelectUnselectAttribute(cb, attributeID) {

            console.log(cb.name);
           
            var li = '#avail' + cb.id;
            if (cb.checked) {
                $.when(
                    addAttributeToList(cb)
                ).done(
                    function (

                    ) {

                        loadModulelist();
                    })

                $(li).addClass('itemSelected');

            }
            else {
                $.when(
                    removeAttributeFromList(cb)
                ).done(
                    function (

                    ) {
                        loadModulelist();

                    })

                $(li).removeClass('itemSelected');
            }






        }

      

        function getSelectedListModule() {

            $.when(
                getSelectedAttributes()
            ).done(
                function (
                    selectedList
                ) {


                    var html = "<ul id='selectedList'>";
                    $.each(selectedList.d, function (i, selectedItem) {


                        var func = "'" + selectedItem.module_id + "'"; //, '" + selectedItem.attributename + "');";

                        html += "<li id='" + selectedItem.applicationmodulemapping_id + "'>";
                        html += "<div class='row'><div class='col-md-9'>"
                        html += i + 1;
                        html += '.&nbsp;';
                        if (!selectedItem.isdefaultmodule) {
                            html += selectedItem.displayname;
                        }
                        else {
                            html += selectedItem.displayname+'   <b>(Default)</b>';
                        }
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

        function loadModulelist() {

            $.when(
                getAvailableAttributes()
            ).done(
                function (
                    availableList
                ) {

                   
                    var html = "<ul id='availableList'>";
                    var selectedhtml = "<ul id='selectedList'>";
                    $.each(availableList.d, function (i, availableItem) {

                        html += '<li id="avail' + availableItem.module_id + '"'
                        if (availableItem.isselected) {
                            html += ' class="itemSelected"';
                        }
                        html += '><input type="checkbox" name= "' + availableItem.applicationmodulemapping_id + '"    id="' + availableItem.module_id + '"';
                        if (availableItem.isselected) {
                            html += ' checked';
                        }
                        html += ' onclick="SelectUnselectAttribute(this);"/>&nbsp;&nbsp;' + availableItem.modulename + '</li>';

                    });
                    html += "</ul";
                    $('#availableAttributes').html(html);
                    selectedhtml += "</ul>"
                    $('#selectedAttributes').html(selectedhtml);
                    getSelectedListModule();

                })
        }
        $(document).ready(function () {


            loadModulelist();


            $('#btnQuickSave').click(function () {
                //alert('Saved');
                debugger;
                var attributeId = $('#attributeId').text();
                var displayname = $('#ContentPlaceHolder1_txtDisplayName').val();
                var isdefaultmodule = $('#ContentPlaceHolder1_isdefaultmodule:checked').val();
                if (typeof isdefaultmodule === "undefined") {
                    isdefaultmodule = false;
                }
                else {
                    isdefaultmodule = true;
                }

                console.log(attributeId + "," + displayname + "," + isdefaultmodule);

                SaveQuickAttributes(attributeId, displayname, isdefaultmodule)

                $('#detail-modal').modal('hide');//modal_1 is the id 1

                loadModulelist();
            });

        });

        function showDetailModal(item) {
            //e.preventDefault();
            console.log(item.applicationmodulemapping_id);

            debugger;
            // Clear out current values from form
            $('#attributeName').text("");
            $('#attributeId').text("");
            $('#ContentPlaceHolder1_txtDisplayName').val("");
            var isdefaultmodule = item.isdefaultmodule;
            if (isdefaultmodule === null) {
                isdefaultmodule = false;
                $('#ContentPlaceHolder1_isdefaultmodule').prop('checked', false).removeAttr('checked');
            }
           

            $('#attributeName').text(item.attributename);
            $('#attributeId').text(item.applicationmodulemapping_id);
            $('#ContentPlaceHolder1_txtDisplayName').val(item.displayname);
            if (isdefaultmodule === false) {

                $('#ContentPlaceHolder1_isdefaultmodule').prop('checked', false).removeAttr('checked');
            }
            else {
                $('#ContentPlaceHolder1_isdefaultmodule').prop('checked', false).removeAttr('checked');
            }
          
       


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
                                    <asp:Label ID="lblDisplayName" runat="server" CssClass="control-label" for="txtDisplayName" Text="Module Display Name" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <asp:Label ID="errDisplayName" runat="server"></asp:Label>
                           
                            <asp:TextBox ID="txtDisplayName" runat="server" CssClass="form-control" MaxLength="255"></asp:TextBox>
                            <br />
                             
                            <asp:CheckBox ID="isdefaultmodule" CssClass="form-control-" runat="server" />
                             <asp:Label ID="lblisdefault"  runat="server">Default Module</asp:Label>
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
                <h4 style="color: #fff;"> <asp:Label ID="lblapplicationname" runat="server"></asp:Label> Module Mapping</h4>
            </div>
            <div class="col-md-6">
                <h4 style="color: lightgray;" class="pull-right">
                    <asp:Label ID="lblSummaryType" runat="server"></asp:Label></h4>
            </div>
        </div>



      


        <div class="hidden">
            <h4>ListID: <span id="listID"></span></h4>
            <h4>Next Ordinal Position: <span id="lblNextOrdinalPosition"></span></h4>
        </div>


        <div class="row">

            <div class="col-md-6">
                <div class="selectContainer">
                    <p style="font-size: 1.8em; font-weight: bold">Available Modules for  Application</p>
                    <div id="availableAttributes">
                         
                    </div>
                </div>
            </div>
         


            <div class="col-md-6">
                <div class="selectContainer">
                    <p style="font-size: 1.8em; font-weight: bold">Selected Modules</p>
                    <div id="selectedAttributes"></div>
                </div>
            </div>


           

        </div>


    </div>

</asp:Content>
