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

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ListSelectQuestions.aspx.cs" Inherits="SynapseStudio.ListSelectQuestions" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="Content/themes/base/jquery-ui.css" rel="stylesheet" />
    <script src="Scripts/knockout-2.2.0.js"></script>
    <script src="Scripts/jquery-ui-1.12.1.js"></script>

    <script type="text/javascript">

        function PostData(service, data) {

            //var service = service;
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


        function getAvailableQuestions() {
            //console.log($("#ContentPlaceHolder1_lblDefaultContext").html());
            var url = "ListSelectQuestions.aspx/GetAvailableQuestions";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "','defaultContext':'" + $("#ContentPlaceHolder1_lblDefaultContext").html()  + "'}";
            //console.log(data);
            return PostData(url, data);

        }

        function getSelectedQuestions() {


            var url = "ListSelectQuestions.aspx/GetSelectedQuestions";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "'}";
            return PostData(url, data);

        }


        function addQuestionToList(QuestionId) {
            console.log("Added: " + QuestionId);
            var ordinalposition = $('#lblNextOrdinalPosition').text();
            var url = "ListSelectQuestions.aspx/AddQuestionToList";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "','question_id':'" + QuestionId + "','ordinalposition':'" + ordinalposition + "'}";
            return PostData(url, data)

        }


        function removeQuestionFromList(QuestionId) {

            var ordinalposition = $('#lblNextOrdinalPosition').text();
            var url = "ListSelectQuestions.aspx/RemoveQuestionFromList";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "','question_id':'" + QuestionId + "'}";
            return PostData(url, data);

        }

        function GetNextOrdinalPosition() {
            var url = "ListSelectQuestions.aspx/GetNextOrdinalPosition";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "'}";
            return PostData(url, data);
        }

        function UpdateQuestionOrder(listquestion_id, order) {            
            var url = "ListSelectQuestions.aspx/UpdateOrdinalPosition";
            var id = $('#listID').text();
            var data = "{'listId': '" + id + "','listquestion_id':'" + listquestion_id + "','ordinalposition':" + order + "}";
            return PostData(url, data);
        }


        function SaveQuickQuestions(QuestionId, displayname, defaultcssclass) {
            var url = "ListSelectQuestions.aspx/SaveQuickQuestions";
            var listId = $('#listID').text();
            var data = "{'listId': '" + listId + "','listQuestion_id':'" + QuestionId + "','displayname':'" + displayname + "','defaultcssclassname':'" + defaultcssclass + "'}";
            return PostData(url, data);
        }

        function SelectUnselectQuestion(cb, QuestionID) {

            //console.log(cb.id);


            var li = '#avail' + cb.id;
            if (cb.checked) {
                $.when(
                    addQuestionToList(cb.id)
                ).done(
                    function (

                    ) {

                        getSelectedList();
                    });

                $(li).addClass('itemSelected');

            }
            else {
                $.when(
                    removeQuestionFromList(cb.id)
                ).done(
                    function (

                    ) {
                        getSelectedList();

                    });

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



                });

        }

        function getSelectedList() {

            $.when(
                getSelectedQuestions()
            ).done(
                function (
                    selectedList
                ) {

                    console.log("Selected List: "  + JSON.stringify(selectedList));
                    var html = "<ul id='selectedList'>";
                    $.each(selectedList.d, function (i, selectedItem) {


                        var func = "'" + selectedItem.listquestion_id + "'"; //, '" + selectedItem.Questionname + "');";

                        html += "<li id='" + selectedItem.listquestion_id + "'>";
                        html += "<div class='row'><div class='col-md-1'>";
                        html += i + 1;
                        html += '.&nbsp;</div><div class="col-md-11">';
                        html += selectedItem.questiondisplay;                       
                        html += "</div></div>";
                        html += '</li>';
                    });
                    html += "</ul>";
                    $('#selectedQuestions').html(html);


                    $("#selectedList").sortable({
                        revert: true,
                        stop: function (event, ui) {

                            var sortOrder = $("#selectedList").sortable('toArray');

                            //console.log(sortOrder.length);
                            //sortOrder = JSON.parse(sortOrder)

                            var arrayLength = sortOrder.length;
                            for (var i = 0; i < arrayLength; i++) {

                                UpdateQuestionOrder(sortOrder[i], i + 1);
                                getSelectedList();

                            }

                        }
                    });

                    getNextOrdinalPositionForLabel();


                });

        }

        $(document).ready(function () {


            var listID = getParameterByName('id');
            //console.log(listID);
            $('#listID').text(listID);

            getNextOrdinalPositionForLabel();

            $.when(
                getAvailableQuestions()
            ).done(
                function (
                    availableList
                ) {

                    //console.log(availableList);
                    var html = "<ul id='availableList'>";
                    $.each(availableList.d, function (i, availableItem) {

                        html += '<li id="avail' + availableItem.question_id + '"';
                        if (availableItem.isselected) {
                            html += ' class="itemSelected"';
                        }
                        html += '><div class="row"><div class="col-md-1"><input type="checkbox" id="' + availableItem.question_id + '"';
                        if (availableItem.isselected) {
                            html += ' checked';
                        }
                        html += ' onclick="SelectUnselectQuestion(this);"/></div><div class="col-md-11">' + availableItem.questiondisplay + '</div></div></li>';

                    });
                    html += "</ul";
                    $('#availableQuestions').html(html);

                    getSelectedList();

                });


           

        });

       

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




        


        <div>
            <asp:HiddenField ID="hdnListID" runat="server" />
            <asp:HiddenField ID="hdnUserName" runat="server" />
            <asp:HiddenField ID="hdnNextOrdinalPosition" runat="server" />
            <asp:HiddenField ID="hdnDataType" runat="server" />
            <asp:HiddenField ID="hdnDefaultContext" runat="server" />
        </div>

        <div class="row" style="margin-top: 7px;">
            <div class="col-md-6">
                <h4 style="color: #fff;">List Manager - Select Questions</h4>
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
            <h4>Default Context: <asp:Label ID="lblDefaultContext" runat="server"></asp:Label></h4>
            <h4>Next Ordinal Position: <span id="lblNextOrdinalPosition"></span></h4>
        </div>


        <div class="row">

            <div class="col-md-6">
                <div class="selectContainer">
                    <p style="font-size: 1.8em; font-weight: bold">Available Questions with Context set to : <asp:Label id="lblContextField" runat="server"></asp:Label></p>
                    <div id="availableQuestions">
                    </div>
                </div>
            </div>


            <div class="col-md-6">
                <div class="selectContainer">
                    <p style="font-size: 1.8em; font-weight: bold">Selected Questions</p>
                    <div id="selectedQuestions"></div>
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
