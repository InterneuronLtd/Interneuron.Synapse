class formularyListTree {

    constructTreeView(nodeClick) {

        let glyph_opts = {
            preset: "awesome5",
            map: {
            }
        };

        $("#treetable").fancytree({
            extensions: ["glyph", "table", 'contextMenu'],
            checkbox: true,
            icon: function (event, data) {
                switch (data.node.data.Level) {
                    case "VTM":
                        return "Medicationtypenotdefined.svg";
                    case "VMP":
                        return "Capsule.svg";
                    case "AMP":
                        return "ActualMedicinalProduct.svg";
                }
            },
            imagePath: "../img/",
            autoScroll: true,
            iconTooltip: function (event, data) {
                switch (data.node.data.Level) {
                    case "VTM":
                        return "VTM";
                    case "VMP":
                        return "VMP";
                    case "AMP":
                        return "AMP";
                }
            },
            tooltip: function (event, data) {
                switch (data.node.data.Level) {
                    case "VTM":
                        return "VTM";
                    case "VMP":
                        return "VMP";
                    case "AMP":
                        return "AMP";
                }
            },
            glyph: glyph_opts,
            source: [],
            table: {
                checkboxColumnIdx: 0,
                nodeColumnIdx: 1
            },
            activate: function (event, data) {
            },
            expand: function (event, data) {
                formularyListNodeExpandHandler(event, data);
            },
            collapse: function (event, data) {
                let allNodeCollapsed = true;
                var tree = data.tree;
                tree.visit((treenode) => {
                    allNodeCollapsed = allNodeCollapsed && !treenode.isExpanded();
                });
                if (allNodeCollapsed)
                    $('#lnkCollapseAll').removeClass('fa-caret-down').addClass('fa-caret-right');
            },
            lazyLoad: (event, data) => {
                this.loadChildrenFormularies(data);
            },
            loadChildren: (e, data) => {
                let selectedDatas = {};
                let currentLevelSelected = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL);
                let selectedDataString = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_IDs);
                let currentRecordStatusSelected = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_STATUS);

                if (selectedDataString)
                    selectedDatas = JSON.parse(selectedDataString);

                var tree = data.tree;//$.ui.fancytree.getTree("#treetable");
                tree.visit(function (treenode) {
                    //treenode.unselectable = (currentLevelSelected && currentLevelSelected != treenode.data.Level);
                    if (treenode.statusNodeType != 'nodata')
                        treenode.unselectable = (currentLevelSelected && currentRecordStatusSelected && currentLevelSelected != treenode.data.Level && currentRecordStatusSelected != treenode.data.recordstatus.description);

                    if (selectedDatas && Object.keys(selectedDatas).length > 0)
                        treenode.setSelected(selectedDatas[treenode.data.formularyVersionId] ? true : false);

                    treenode.render();
                });
            },
            select: (event, data) => {
                // Get a list of all selected nodes, and convert to a key array:
                //let selNodeData = $.map(data.tree.getSelectedNodes(), function (node) {
                //    return node.key | node.title | node.data.formularyVersionId;
                //});

                //$("#inspectortext").text(selKeys.join(", "));

                addOrRemoveSelectedNodes(event, data);

            },
            click: function (event, data) {
                sessionStorage.setItem("BULK_SEL_TARGET_SEL_LEVEL", "");
                //if (nodeClick) nodeClick(event, data);
            },
            renderColumns: function (event, data) {
                var node = data.node,
                    $tdList = $(node.tr).find(">td");
                //$tdList.eq(0).text(node.getIndexHier());
                //if (node.data.recordstatus.isDuplicate === true) {
                //    $tdList.eq(1).addClass("bg-warning text-white");
                //    node.setExpanded(true);
                //}
                if (node.data.recordstatus.description != null) {
                    //$tdList.eq(2).text(node.data.recordstatus.description);
                    $tdList.eq(2).html("<a href='#' class='record-status-link' name='node_record_status' id='lnkNodeRecordStatus'>" + node.data.recordstatus.description + "</a>");
                }
            }
        });
    }

    loadChildrenFormularies(data) {
        let dfd = new $.Deferred();
        data.result = dfd.promise();

        if (!data || !data.node || !data.node.key) {
            dfd.reject([]);
            return;
        }

        let hideArchived = $('#chkSearchArchive').is(':checked');

        let searchCriteria = {
            formularyCode: data.node.key,
            hideArchived: hideArchived
        };

        ajaxPostJson(`Formulary/LoadChildrenFormularies`, searchCriteria,
            (res) => {
                dfd.resolve(res);
            },
            (err) => {
                console.error(err);
                dfd.reject([]);
            })
    }

    loadData(url, args, beforeLoad, afterLoad) {
        if (beforeLoad) {
            beforeLoad();
        }
        ajaxPostJson(url, args,
            (data) => {
                if (afterLoad) {
                    afterLoad(true, data);
                }
                var tree = $.ui.fancytree.getTree("#treetable");
                //tree.reload(data);
                tree.setOption("source", data.results);

                //let isDuplicate = false;

                //if (data && data.length > 0) {
                //    for (var i = 0; i < data.length; i++) {
                //        if (data[i].data.recordstatus.isDuplicate) {
                //            isDuplicate = true;
                //            break;
                //        }
                //    }
                //}

                //if (isDuplicate) {
                //    $.ui.fancytree.getTree("#treetable").expandAll();
                //}
            },
            (err) => {
                afterLoad(false, err);
            });
    }
}

$.contextMenu({
    selector: "#treetable span.fancytree-title",
    build: function ($trigger, e) {
        // this callback is executed every time the menu is to be shown
        // its results are destroyed every time the menu is hidden
        // e is the original contextmenu event, containing e.pageX and e.pageY (amongst other data)
        let node = $.ui.fancytree.getNode($trigger);
        let currentLevelSelected = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL);

        let menuObj = {};
        menuObj.callback = (key, options) => {
            contextMenuCallbackHandler(key, options, node, currentLevelSelected);
        };

        menuObj.items = {};

        menuObj = new FormularyListContextMenuItemsBuilder(menuObj, node).buildMenu();

        return menuObj;
    }
});

var treeList = new formularyListTree();

$(document).ready(function () {

    toastr.options.preventDuplicates = true;

    sessionStorage.setItem("BULK_SEL_TARGET_SEL_LEVEL", "");
    //sessionStorage.setItem("BULK_SEL_NODE_ID", "");

    showScrollToTop();

    initPage();

    $('#pnlFormulariesPagination').pagination({
        cssStyle: 'light-theme',
        hrefTextPrefix: '#',
        onPageClick: (pageNumberClicked, e) => {
            let initLoad = $('#pnlFormulariesPagination').data('int-load');//no need to call the pagination since page is just loaded
            if (!initLoad || (initLoad && pageNumberClicked != 1)) {
                $('#pnlUpdateProgress').show();

                $('#pnlFormulariesPagination').data('int-load', false);
                ajaxGetJson('Formulary/GetFormulariesByPageNumber', { pageNumber: pageNumberClicked },
                    (data) => {
                        $('#pnlUpdateProgress').hide();
                        var tree = $.ui.fancytree.getTree("#treetable");
                        //tree.reload(data);
                        tree.setOption("source", data);
                    },
                    (err) => {
                        $('#pnlUpdateProgress').hide();
                        toastr.error('Error getting the results for this page.');
                        console.error(err);
                    });
            }
        }
    });

    $('#pnlFilterContainer').keypress((event) => {
        if (event.keyCode == 13) {
            $('#btnApplyFilter').click();
        }
    });

    $('#filter').click(function () {
        $('#pnlFilterContainer').toggle();
        $('#pnlFilterResults-row').toggle();
        $('#filter i').toggleClass('text-info');
    });

    $('.formulary-list #iBulkEdit, .formulary-list #iBulkEditFloat ').click(function () {
        let selectedRecordsAsString = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_IDs);
        if (!selectedRecordsAsString) return;

        let selectedRecords = JSON.parse(selectedRecordsAsString);
        if (!selectedRecords || Object.keys(selectedRecords).length == 0) return;

        let $container = $('#pnlBulkEditSelectorContainer');
        $container.show();
        $container.html('');
        $('#pnlUpdateProgress').show();
        ajaxLoad('/Formulary/GetBulkEditSelectorPartial', null, $container, () => { $('#pnlUpdateProgress').hide(); }, () => { $('#pnlUpdateProgress').hide(); });
    });

    $('.formulary-list #lnkCollapseAll').click(function () {
        let tree = $.ui.fancytree.getTree("#treetable");
        tree.visit(function (node) {
            node.setExpanded(false);
        });
    });

    $('.formulary-list #lnkDeSelectAll').click(function (e) {
        let tree = $.ui.fancytree.getTree("#treetable");
        tree.visit(function (node) {
            node.setSelected(false);
        });
    });

    $("#btnstatuschange").click(function () {
        if ($("#statusChangeReason").val().trim() === "" && $("#ddlstatus").children("option:selected").val() === "004") {
            $("#lblstatuschangeerror").show();
            return;
        }

        if ($("#ddlstatus").children("option:selected").val() === "006") {
            $.confirm('Are you sure?', (action) => {
                if (action === 'yes') {
                    let url = "/Formulary/UpdateFormularyStatus";
                    let args = {
                        formularyVersionId: $("#statuschangeformularyVersionId").val(),
                        reason: $("#statusChangeReason").val(),
                        status: $("#ddlstatus").children("option:selected").val()
                    };
                    $('#pnlUpdateProgress').show();
                    $('#btnstatuschange').hide();
                    $('#btnStatusChangeDisabled').show();
                    ajaxPost(url, args,
                        (data) => {
                            if (data) {
                                $('#pnlUpdateProgress').hide();
                                $('#btnstatuschange').show();
                                $('#btnStatusChangeDisabled').hide();
                                $('#changestatus').modal('hide');
                                callSearchFormularies();
                            } else {
                                $('#pnlUpdateProgress').hide();
                                $('#btnstatuschange').show();
                                $('#btnStatusChangeDisabled').hide();
                            }
                        },
                        (err) => {
                            console.error(err);
                            $('#pnlUpdateProgress').hide();
                            $('#btnstatuschange').show();
                            $('#btnStatusChangeDisabled').hide();
                        }
                    );
                }
                else {
                    return;
                }
            });
        }
        else {
            let url = "/Formulary/UpdateFormularyStatus";
            let args = {
                formularyVersionId: $("#statuschangeformularyVersionId").val(),
                reason: $("#statusChangeReason").val(),
                status: $("#ddlstatus").children("option:selected").val()
            };
            $('#pnlUpdateProgress').show();
            $('#btnstatuschange').hide();
            $('#btnStatusChangeDisabled').show();
            ajaxPost(url, args,
                (data) => {
                    if (data) {
                        $('#pnlUpdateProgress').hide();
                        $('#btnstatuschange').show();
                        $('#btnStatusChangeDisabled').hide();
                        $('#changestatus').modal('hide');
                        callSearchFormularies();
                    } else {
                        $('#pnlUpdateProgress').hide();
                        $('#btnstatuschange').show();
                        $('#btnStatusChangeDisabled').hide();
                    }
                },
                (err) => {
                    console.error(err);
                    $('#pnlUpdateProgress').hide();
                    $('#btnstatuschange').show();
                    $('#btnStatusChangeDisabled').hide();
                }
            );
        }
    });

    $("#btnsavedelete").click(function () {
        if ($("#formularydeleteReason").val().trim() === "") {

            $("#lbldeleteerror").show();
            return;
        }

        let url = "/Formulary/UpdateFormularyStatus";
        let args = {
            formularyVersionId: $("#deleteformularyVersionId").val(),
            reason: $("#formularydeleteReason").val(),
            status: "006"
        };

        ajaxPost(url, args,
            (data) => {
                $('#deleteFormulary').modal('hide');
                callSearchFormularies();
            },
            (err) => {
                console.error(err);
            });
    });

    $('#ddlstatus').on('change', function (e) {
        var optionSelected = $("option:selected", this);
        var valueSelected = this.value;
        $("#statusChangeReason").val("");
        if (valueSelected === "004") {
            $("#showstatusChangeReason").show();
        }
        else {
            $("#showstatusChangeReason").hide();
        }
    });

    $("#treetable").on("click", "i[name=result_node_edit]", function (e) {
        var node = $.ui.fancytree.getNode(e),
            $input = $(e.target);
        editFormularyNode(node);
        e.stopPropagation();
    });

    $("#treetable").on("click", "a[name=node_record_status]", function (e) {
        var node = $.ui.fancytree.getNode(e),
            $input = $(e.target);

        //let statusesToAssign = getAssignableStatuses(node.data.recordstatus.description);
        $('#pnlUpdateProgress').show();
        ajaxGetJson('Formulary/GetNextLevelStatuses', { status: node.data.recordstatus.code },
            (data) => {
                $('#pnlUpdateProgress').hide();
                if (!data) {
                    console.error(err);
                    node.removeClass('fancytree-active');
                    node.setActive(false);
                    toastr.error('Error getting record status details.');
                    return;
                }

                $("#statuschangeformularyVersionId").val(node.data.formularyVersionId);
                $('#ddlstatus').html('');
                $("#showstatusChangeReason").hide();

                $.each(data, (k, v) => {
                    let selected = '';
                    if (v.selected)
                        selected = 'selected';

                    $('#ddlstatus').append(`<option value="${v.value}" ${selected}>${v.text}</option>`);
                });

                $('#changestatus').modal('show');
            },
            (err) => {
                $('#pnlUpdateProgress').hide();
                console.error(err);
                node.removeClass('fancytree-active');
                node.setActive(false);
                toastr.error('Error getting record status details.');
            });

        //if (!statusesToAssign || statusesToAssign.length == 0) {
        //    node.removeClass('fancytree-active');
        //    node.setActive(false);
        //    return;
        //}

        //$("#statuschangeformularyVersionId").val(node.data.formularyVersionId);
        //$('#ddlstatus').html('');
        //$("#showstatusChangeReason").hide();

        //$.each(statusesToAssign, (k, v) => {
        //    $('#ddlstatus').append(`<option value="${v.val}">${v.text}</option>`);
        //});

        //$('#changestatus').modal('show');

        e.stopPropagation();  // prevent fancytree activate for this row
    });

    $("#chkSearchArchive").change(function () {
        var checkBox = document.getElementById("chkSearchArchive");
        if (checkBox.checked == true) {
            $('#lstBoxFilterStatuses option:selected').each(function () {
                $(this).prop('selected', false);
            })
            $('#lstBoxFilterStatuses').multiselect('refresh');
        }
        //callSearchFormularies();
    });

    $('#lstBoxFilterStatuses').on('change', function () {
        //let searchText = $('#searchtext').val();
        let multiSelectSearch = $(this).find("option:selected");//$('#lstBoxFilterStatuses').val().toString();
        //let chkSearchArchive = $('#chkSearchArchive').is(':checked');
        var arrSelected = [];
        $("#chkSearchArchive").prop("checked", false);
        multiSelectSearch.each(function () {
            arrSelected.push($(this).val());
        });

        if (arrSelected.toString().indexOf('Duplicate') > -1 && (arrSelected.toString().indexOf('|') > -1)) {
            $('#lstBoxFilterStatuses option:selected').each(function () {
                $(this).prop('selected', false);
            })
            $('#lstBoxFilterStatuses').multiselect('refresh');
            return false;
        }

        //callSearchFormularies();
    });

    $('#btnApplyFilter').click(function (e) {

        $('#pnlFormulariesPagination').data('int-load', true);

        let canProceed = displayFilterResults();

        if (!canProceed) return;

        $('#pnlFilterContainer').toggle();
        $('#pnlFilterResults-row').toggle();
        $('#filter i').toggleClass('text-info')
        callSearchFormularies();
    });

    $("#addMed").on('click', function () {
        $("#btnImport").show();
        $("#btnImport").attr("disabled", true);
        $("#btnImportDisabled").hide();
        $('#modal-spinner').hide();
        $('#chkImportAutoSelect').prop("checked", false);
        $('#medModal').modal('show');
    });

    $("#customMed").on('click', function () {
        let $container = $('#pnlCreateCustomMedContainer');
        $container.show();
        $container.html('');
        $('#pnlUpdateProgress').show();
        ajaxLoad('/Formulary/GetCustomMedContainerPartial', null, $container, () => { $('#pnlUpdateProgress').hide(); }, () => { $('#pnlUpdateProgress').hide(); });

        //$('#customMedModal').modal('show');
        //$("#dvLoading").show();
        //$("#btnSaveCustomMedDisabled").hide();
        //ajaxLoad('/Formulary/LoadVMPForm',
        //    null,
        //    null,
        //    (data) => {
        //        $("#customMedComponent").html(data);
        //        $("#dvLoading").hide();
        //        $("#ArchiveReasonshow").hide();
        //        $("#lblArchivestatuserror").hide();
        //    }
        //);
    });

    //$("#btnSaveCustomMed").click(function () {

    //    $("#customMedForm").find('input:text').each(function () {
    //        this.value = $(this).val().trim();
    //    });

    //    let formInput = $("#customMedForm").serializeObject();

    //    processAutoCompletes(formInput, '#Create_New_Custom_Medication');
    //    processAdditionalCodes(formInput, '#Create_New_Custom_Medication');
    //    processIngredients(formInput, '#Create_New_Custom_Medication');
    //    processExcipients(formInput, '#Create_New_Custom_Medication');

    //    let validationSmryPanel = $('#Create_New_Custom_Medication div#pnlMedicationValidationSmry');

    //    //This is required - otherwise the hidden fields will be ignored from validation (but required for Autocomplete)
    //    $("#customMedForm").data("validator").settings.ignore = "";
    //    //$.validator.setDefaults({ ignore: '' });

    //    let validator = $("#customMedForm").validate({
    //        //ignore: '',
    //        errorContainer: validationSmryPanel,
    //        errorLabelContainer: $("ul", validationSmryPanel),
    //        wrapper: 'li'
    //    });

    //    let isFormValid = validator.form();
    //    //console.log(isFormValid);

    //    if (!isFormValid) return;

    //    ajaxPost('Formulary/AddCustomMedication', formInput,
    //        (data) => {

    //            if (data != null) {
    //                $('#customMedComponent').html(data);
    //            }
    //            else {
    //                $('#customMedModal').modal('hide');
    //                $('#customMedComponent').html('');
    //            }
    //        }
    //    );
    //});

    $("#btnUpdateCustomMed").click(function () {
        let formInput = $('#editMedForm').serializeObject();

        let isBulkEdit = (formInput['IsBulkEdit'] && (formInput['IsBulkEdit'] == true || formInput['IsBulkEdit'] == 'True'))

        if (isBulkEdit) {
            if ($("#Edit_Formulary_ddlRecordStatus").children("option:selected").val() == "") {
                UpdateFormulary();
            }
            else {
                if ($("#RecStatuschangeMsg").val().trim() === "" && $("#Edit_Formulary_ddlRecordStatus").children("option:selected").val() === "004") {
                    $("#lblstatuschgerr").show();
                    return;
                }

                let request = {};

                let selectedRecords = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_IDs);

                if (selectedRecords) {
                    let recordsJson;

                    recordsJson = JSON.parse(selectedRecords);

                    jsonObj = [];

                    Object.keys(recordsJson).forEach(function (rec) {
                        item = {}
                        item["FormularyVersionId"] = rec;
                        item["RecordStatusCode"] = $("#Edit_Formulary_ddlRecordStatus").children("option:selected").val();
                        item["RecordStatusCodeChangeMsg"] = $("#RecStatuschangeMsg").val()

                        jsonObj.push(item);
                    });

                    request.RequestData = jsonObj;
                }

                if (request != {} && isBulkEdit) {
                    let url = 'Formulary/BulkUpdateFormularyStatus';

                    ajaxPost(url, request,
                        (data) => {
                            if (data != null && data.length > 0 && data[0] == "success") {
                                UpdateFormulary();
                            }
                            else {
                                if (data != null && data.length > 0) {
                                    toastr.error(data);
                                }
                            }
                        },
                        (err) => {
                            console.log(err);
                        }
                    );
                }
            }
        }
        else {
            UpdateFormulary();
        }

    });

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    $("#customMedModal").on("hide.bs.modal", function () {
        $('#customMedModal .add-additional-code').off();
        $('#customMedModal .add-ingredient').off();
        $("#customMedComponent").html();

    });

    $('#history').click(function () {
        $('#historyModal').modal('show');
    });

    $('#historyModal').on('show.bs.modal', function (e) {
        $('#pnlUpdateProgress').show();
        getHistoryOfFormularies('Formulary/GetHistoryOfFormularies', null, null, null);
    });

    $("#historyModal").on("hide.bs.modal", function () {
        $(".fancySearchRow").remove();
        $("#tblHistory tbody tr").remove();
        $("#tblHistory tfoot tr").remove();
    });

    //$('#historyModal').on('shown.bs.modal', function (e) {
    //    applyPagingToHistoryTable();
    //});
   
    //$("#historySearch").on("keyup", function () {
    //    var value = $(this).val();

    //    if (value != "") {
    //        var totalRecords = 0;

    //        $("#tblHistory tbody tr").each(function (index) {
    //            $row = $(this);

    //            var id = $row.find("td:nth-child(3)").text().toLowerCase();

    //            if (id.indexOf(value.toLowerCase()) !== 0) {
    //                $row.hide();
    //            }
    //            else {
    //                $row.show();
    //                totalRecords = totalRecords + 1;
    //            }
    //        });

            
    //        $('#historyPagination').pagination('updateItems', totalRecords);
    //    }
        
    //});
});

function initPage() {

    $('#lstBoxFilterStatuses').multiselect({
        includeSelectAllOption: false,
        nonSelectedText: 'Multi-select filter'
    });

    $('#pnlFormulariesPagination').data('int-load', true);

    //No need to select 'Formulary'by default anymore
    //$('#lstBoxFilterStatuses').multiselect('select', 'Form|001', true);

    //displayFilterResults();

    loadResults();

    setTimeout(() => {
        $('.formulary-list .chk-switch').bootstrapToggle({
            on: 'Yes',
            off: 'No',
            onstyle: 'success',
            offstyle: 'danger',
            width: 70,
            size: 'sm'
        });
    }, 100);
}

function showScrollToTop() {
    $(window).scroll(function () {
        if ($(this).scrollTop() > 50) {
            $('#iBulkEditFloat').fadeIn();
            $('#back-to-top').fadeIn();
        } else {
            $('#iBulkEditFloat').fadeOut();
            $('#back-to-top').fadeOut();
        }
    });
    // scroll body to 0px on click
    $('#back-to-top').click(function () {
        $('body,html').animate({
            scrollTop: 0
        }, 400);
        return false;
    });
}

function loadResults() {

    $('#pnlUpdateProgress').show();

    treeList.constructTreeView(onTreeNodeClick);

    let searchCriteria = {
        //recStatusCds: ['Form|001'] //No need to select only formulary for now
    }

    treeList.loadData('/Formulary/LoadFormularyList', searchCriteria,
        () => {
            //$('#frmlist-spinner').show();
            $('#frmlist-tree').hide();
        },
        (isSuccess, data) => {
            if (isSuccess) {
                updatePagination(data);
                //$('#frmlist-spinner').hide();
                $('#pnlUpdateProgress').hide();
                $('#frmlist-tree').show();
            }
            else {
                $('#pnlUpdateProgress').hide();
                $('#frmlist-tree').show();
            }
        });
}

function bulkEditSelectorClose(updateBulkEdit) {
    var tree = $.ui.fancytree.getTree("#treetable");
    let selectedDatas = {};

    let hasSelectedData = true;
    var selectedDataString = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_IDs);

    if (!selectedDataString) hasSelectedData = false;

    if (hasSelectedData)
        selectedDatas = JSON.parse(selectedDataString);

    if (!selectedDatas || Object.keys(selectedDatas).length == 0) hasSelectedData = false;

    if (!hasSelectedData) {
        sessionStorage.removeItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_IDs);
        sessionStorage.removeItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL);
        sessionStorage.removeItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_STATUS);

        tree.visit(function (treenode) {
            treenode.setSelected(false);
            treenode.render();
            addOrRemoveSelectedNodes(null, null, treenode);
        });

        return;
    }

    if (!updateBulkEdit) {
        tree.visit(function (treenode) {
            treenode.setSelected(selectedDatas[treenode.data.formularyVersionId] ? true : false);
            treenode.render();
        });
        return;
    }

    let selectedFormularyIds = [];

    Object.keys(selectedDatas).forEach(function (itemKey, index) {
        selectedFormularyIds.push(selectedDatas[itemKey].formularyVersionId);
    });

    if (updateBulkEdit) {

        $('#pnlUpdateProgress').show();

        let url = "/Formulary/GetProductTypeBulkEditPartial";
        let args = {
            formularyVersionIds: JSON.stringify(selectedFormularyIds),
            productType: sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL),
            status: sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_STATUS)
        };

        ajaxLoad(url, args, $("#inspectorcomp"),
            (data) => {
                $('#pnlUpdateProgress').hide();
                $('#editMedModal').modal('show');
                //$("#inspectorcomp").show();
                //$("#inspectorcomp").html(data);
                //$("#ArchiveReasonshow").hide();
                //$("#lblArchivestatuserror").hide();
                //$('#btnsShowinspector').show();
                $('#btnUpdateCustomMed').show();
                $('#btnUpdateCustomMedDisabled').hide();

            },
            (err) => {
                $('#pnlUpdateProgress').hide();
                console.error(err);
            });
    }

}

//function getAssignableStatuses(currentStatus) {
//    if (!currentStatus || currentStatus === 'Archived') return [];
//    switch (currentStatus) {
//        case 'Draft':
//            return [{ val: '002', text: 'Approve' }];
//        case 'Approved':
//            return [{ val: '003', text: 'Active' }];
//        case 'Active':
//            return [{ val: '005', text: 'Inactive' }, { val: '004', text: 'Archive' }];
//        case 'Inactive':
//            return [{ val: '003', text: 'Active' }, { val: '004', text: 'Archive' }];
//        default:
//            return [];
//    }
//}

function onTreeNodeClick(event, data) {
    let target = $.ui.fancytree.getEventTargetType(event.originalEvent);

    if (target != 'checkbox' && target != 'expander') {
        if (!data || !data.node || !data.node.data || !data.node.data.formularyVersionId) return false;
        editFormularyNode(data.node);
    }
};

function editFormularyNode(node) {

    $('#pnlUpdateProgress').show();

    let url = "/Formulary/LoadFormularyDetails";
    let args = {
        formularyVersionId: node.data.formularyVersionId
    };

    ajaxPost(url, args,
        (data) => {
            $('#pnlUpdateProgress').hide();
            $('#editMedModal').modal('show');
            //$("#inspectorcomp").show();
            $("#inspectorcomp").html(data);
            //$("#ArchiveReasonshow").hide();
            //$("#lblArchivestatuserror").hide();
            //$('#btnsShowinspector').show();
            $('#btnUpdateCustomMed').show();
            $('#btnUpdateCustomMedDisabled').hide();

        },
        (err) => {
            $('#pnlUpdateProgress').hide();
            console.error(err);
        });
}

function displayFilterResults() {

    $('#pnlFilterResults').html('');
    let selected = [];
    let searchCriteria = {};

    $('#lstBoxFilterStatuses option:selected').each(function () {
        selected.push($(this).text());
    });

    let searchText = $('#searchtext').val();
    let hideArchived = $('#chkSearchArchive').is(':checked');

    //if (searchText == "") {
    //    toastr.info('Please enter search text');
    //    return false;
    //}

    if (searchText != "") {
        if (searchText.length < 3) {
            toastr.info('Please enter at least 3 characters to search');
            return false;
        } else {
            searchCriteria['Search Text'] = searchText;
        }
    }

    if (hideArchived) {
        searchCriteria['Hide Archived'] = true;
    }

    if (selected && selected.length > 0) {
        searchCriteria['Status Code'] = selected.join(', ');
    }

    let searchCriteriaKeys = Object.keys(searchCriteria);

    if (searchCriteriaKeys && searchCriteriaKeys.length > 0) {
        searchCriteriaKeys.forEach((item, index) => {
            $('<div/>', { class: 'chip m-1', text: `${item} : ${searchCriteria[item]}` }).appendTo('#pnlFilterResults');
        });
    }

    return true;
}

function callSearchFormularies() {
    //$('#inspectorcomp').html('');
    //$("#inspectorcomp").hide();
    //$('#btnsShowinspector').hide();


    let searchText = $('#searchtext').val();
    let multiSelectSearch = $('#lstBoxFilterStatuses').val().toString();
    let hideArchived = $('#chkSearchArchive').is(':checked');

    if (searchText && searchText.length < 3) {
        toastr.info('Enter at-least 3 search text characters.');
        return false;
    }

    //Not required to have this validation now
    //if (searchText == "" && (multiSelectSearch.includes('Form|002') || (!multiSelectSearch.includes('Form|001') && !multiSelectSearch.includes('Form|002')))) {
    //    toastr.error('Please enter search text');
    //    return false;
    //}


    let searchCriteria = {
        searchTerm: searchText,
        hideArchived: hideArchived,
        recStatusCds: multiSelectSearch
    };

    treeList.loadData('/Formulary/LoadFormularyList', searchCriteria,
        () => {
            $('#lnkCollapseAll').removeClass('fa-caret-down').addClass('fa-caret-right');
            $('#pnlUpdateProgress').show();
            $('#frmlist-tree').hide();

            if (searchText || multiSelectSearch || hideArchived) {
                $('#pnlFilterResults-row').show();
            }
            else {
                $('#pnlFilterResults-row').hide();
            }
        },
        (isSuccess, data) => {
            if (isSuccess) {
                updatePagination(data);
                $('#pnlUpdateProgress').hide();
                $('#frmlist-tree').show();
            } else {
                $('#pnlUpdateProgress').hide();
                $('#frmlist-tree').show();
            }
        });
}

function updatePagination(data) {

    if (!data) return;

    let totalRecords = data.totalRecords || 0;
    let pageSize = data.pageSize || 0;

    $('#pnlFormulariesPagination').data('total-records', totalRecords);
    $('#pnlFormulariesPagination').data('page-size', pageSize);
    $('#pnlFormulariesPagination').pagination('updateItems', totalRecords);
    $('#pnlFormulariesPagination').pagination('updateItemsOnPage', pageSize);
}

function showFormularyCntrlTooltip(item) {
    $(item).data('html', true);
    $(item).data('title', $(item).data('tooltip-content'));
    $(item).tooltip('toggle');
}

function contextMenuCallbackHandler(key, options, node, currentLevelSelected) {
    if (key && key === 'quit') return true;
    let currentNode = node;
    sessionStorage.setItem("BULK_SEL_TARGET_SEL_LEVEL", '');

    if (currentNode && currentNode.data) {
        if (currentNode.data.Level == 'VTM') {
            if (key === 'DelVMP') {
                currentNode.visit(vmpNode => vmpNode.setSelected(false));
            } else if (key === 'DelAMP') {
                currentNode.visit(vmpNode => {
                    if (vmpNode) {
                        vmpNode.visit(ampNode => ampNode.setSelected(false));
                    }
                });
            } else {
                sessionStorage.setItem("BULK_SEL_TARGET_SEL_LEVEL", key);
                if (currentNode.isExpanded())
                    currentNode.setExpanded(false);
                setTimeout(() => { currentNode.setExpanded(true); }, 10);
            }
        } else if (currentNode.data.Level == 'VMP') {
            if (key === 'DelAMP') {
                currentNode.visit(ampNode => ampNode.setSelected(false));
            } else {
                sessionStorage.setItem("BULK_SEL_TARGET_SEL_LEVEL", key);
                if (currentNode.isExpanded())
                    currentNode.setExpanded(false);
                setTimeout(() => { currentNode.setExpanded(true); }, 10);
            }
        }
    }
}

function formularyListNodeExpandHandler(event, data) {

    let targetLevelSelected = sessionStorage.getItem("BULK_SEL_TARGET_SEL_LEVEL");

    if (!targetLevelSelected) return;

    switch (data.node.data.Level) {
        case "VTM":
            if (!targetLevelSelected || !(targetLevelSelected === "AMP" || targetLevelSelected === "VMP")) break;

            if (data.node.children && data.node.children.length > 0) {
                $.each(data.node.children, function (index, vmpNode) {
                    if (targetLevelSelected === "AMP") {

                        if (vmpNode && vmpNode.children && vmpNode.children.length > 0) {
                            $.each(vmpNode.children, function (index, ampNode) {
                                let currentLevelSelected = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL);
                                let currentRecordStatusSelected = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_STATUS);
                                if (!ampNode.unselectable && (!currentLevelSelected || currentLevelSelected == "AMP") && (!currentRecordStatusSelected || currentRecordStatusSelected == ampNode.data.recordstatus.description)) {
                                    ampNode.setSelected(true);
                                    //sessionStorage.setItem("BULK_SEL_TARGET_SEL_LEVEL", "");
                                }
                            });
                        } else {
                            vmpNode.setExpanded(true);
                        }
                    }
                    else if (targetLevelSelected === "VMP") {
                        let currentLevelSelected = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL);
                        if (!vmpNode.unselectable && (!currentLevelSelected || currentLevelSelected == "VMP")) {
                            vmpNode.setSelected(true);
                        }
                    }
                });
            }
            break;
        case "VMP":
            if (!targetLevelSelected || targetLevelSelected !== "AMP") break;
            if (data.node.children && data.node.children.length > 0) {
                $.each(data.node.children, function (index, ampNode) {
                    let currentLevelSelected = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL);
                    let currentRecordStatusSelected = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_STATUS);
                    if (!ampNode.unselectable && (!currentLevelSelected || currentLevelSelected == "AMP") && (!currentRecordStatusSelected || currentRecordStatusSelected == ampNode.data.recordstatus.description)) {
                        ampNode.setSelected(true);
                        //sessionStorage.setItem("BULK_SEL_TARGET_SEL_LEVEL", "");
                    }
                });
            }
            break;
    }

    $('#lnkCollapseAll').removeClass('fa-caret-right').addClass('fa-caret-down');
}

function addOrRemoveSelectedNodes(event, data, otherNode) {
    var tree = $.ui.fancytree.getTree("#treetable");

    var selNodeData = {};

    let currentNode = (data && data.node) ? data.node : (otherNode ? otherNode : {});

    let selectedRecordsAsString = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_IDs);
    if (selectedRecordsAsString)
        selNodeData = JSON.parse(selectedRecordsAsString);

    if (currentNode.selected) {
        selNodeData[currentNode.data.formularyVersionId] = { code: currentNode.key, title: currentNode.title, formularyVersionId: currentNode.data.formularyVersionId };
        toastr.info('Added for bulk edit');
    }
    else {
        if (selNodeData[currentNode.data.formularyVersionId]) {
            delete selNodeData[currentNode.data.formularyVersionId];
        }
        toastr.info('Removed from bulk edit');
    }

    if (!selNodeData || Object.keys(selNodeData).length == 0) {
        $('#lblBulkEditNumber, #lblBulkEditNumberFloat').html('0');
        $('#iBulkEdit, #iBulkEditFloat').removeClass('enabled').addClass('disabled');
        sessionStorage.removeItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL);
        sessionStorage.removeItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_IDs);
        sessionStorage.removeItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_STATUS);
        tree.visit(function (treenode) {
            treenode.unselectable = false;
            treenode.render();
        });
    } else {
        $('#lblBulkEditNumber, #lblBulkEditNumberFloat').html(Object.keys(selNodeData).length);
        $('#iBulkEdit, #iBulkEditFloat').removeClass('disabled').addClass('enabled');
        sessionStorage.setItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_IDs, JSON.stringify(selNodeData));
        sessionStorage.setItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL, currentNode.data.Level);
        sessionStorage.setItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_STATUS, currentNode.data.recordstatus.description);
        tree.visit(function (treenode) {
            treenode.unselectable = ((currentNode.data.Level != treenode.data.Level) || (currentNode.data.recordstatus.description != treenode.data.recordstatus.description)) ? true : false;
            treenode.render();
        });
    }
}

// EDIT: Let's cover URL fragments (i.e. #page-3 in the URL).
// More thoroughly explained (including the regular expression) in: 
// https://github.com/bilalakil/bin/tree/master/simplepagination/page-fragment/index.html

// We'll create a function to check the URL fragment
// and trigger a change of page accordingly.
function checkFragment() {
    // If there's no hash, treat it like page 1.
    var hash = window.location.hash || "#page-1";

    // We'll use a regular expression to check the hash string.
    hash = hash.match(/^#page-(\d+)$/);

    if (hash) {
        // The `selectPage` function is described in the documentation.
        // We've captured the page number in a regex group: `(\d+)`.
        $("#historyPagination").pagination("selectPage", parseInt(hash[1]));
    }
};

function getHistoryOfFormularies(url, args, beforeLoad, afterLoad) {
    if (beforeLoad) {
        beforeLoad();
    }
    ajaxGetJson(url, args,
        (data) => {
            if (afterLoad) {
                afterLoad(true);
            }

            if (!data || data.length == 0) return;

            var str = [];

            data.forEach((item) => {
                if (item.status == "Active" || item.status == "") {
                    var htmlString = "<tr><td>" + item.dateTime + "</td><td>" + item.user + "</td><td>" + item.name + " (" + (item.productType) + ")" + "</td><td>" + item.status + "</td><td><button type='button' class='btn btn-secondary' onclick='GetPreviousFormularyDetails(\"" + item.previousFormularyVersionId + "\", \"" + item.currentFormularyVersionId + "\");GetCurrentFormularyDetails(\"" + item.previousFormularyVersionId + "\", \"" + item.currentFormularyVersionId + "\");'>Properties Changed</button></td></tr>";
                    str.push(htmlString);
                }
                if (item.status == "Draft") {
                    var htmlString = "<tr><td>" + item.dateTime + "</td><td>" + item.user + "</td><td>" + item.name + " (" + (item.productType) + ")" + "</td><td>" + item.status + "</td><td>Imported</td></tr>";
                    str.push(htmlString);
                }
                if (item.status == "Deleted") {
                    var htmlString = "<tr><td>" + item.dateTime + "</td><td>" + item.user + "</td><td>" + item.name + " (" + (item.productType) + ")" + "</td><td>" + item.status + "</td><td>Deleted</td></tr>";
                    str.push(htmlString);
                }
            });

            $('#historyOutput').html(str);

            //applyPagingToHistoryTable();
            $("#tblHistory").fancyTable({
                sortable: false,
                pagination: true,
                perPage: 15,
                globalSearch: false
            });

            $('#pnlUpdateProgress').hide();
        });
}

function applyPagingToHistoryTable() {
    var tblHistory = $('#tblHistory tbody tr');

    var numItems = tblHistory.length;
    var perPage = 15;

    // Only show the first 2 (or first `per_page`) items initially.
    tblHistory.slice(perPage).hide();

    // Now setup the pagination using the `.pagination-page` div.
    $("#historyPagination").pagination({
        items: numItems,
        itemsOnPage: perPage,
        cssStyle: "light-theme",

        // This is the actual page changing functionality.
        onPageClick: function (pageNumber) {
            // We need to show and hide `tr`s appropriately.
            var showFrom = perPage * (pageNumber - 1);
            var showTo = showFrom + perPage;

            // We'll first hide everything...
            tblHistory.hide()
                // ... and then only show the appropriate rows.
                .slice(showFrom, showTo).show();
        }
    });

    // We'll call this function whenever back/forward is pressed...
    $(window).bind("popstate", checkFragment);

    // ... and we'll also call it when the page has loaded
    // (which is right now).
    checkFragment();
}

function GetPreviousFormularyDetails(previousFormularyVersionId, currentFormularyVersionId) {
    $('#pnlUpdateProgress').show();

    let url = "/Formulary/LoadHistoryFormularyDetails";
    let args = {
        previousFormularyVersionId: previousFormularyVersionId,
        currentFormularyVersionId: currentFormularyVersionId,
        previousOrCurrent: "previous"
    };

    ajaxPost(url, args,
        (data) => {
            $('#historyModal').modal('hide');
            $('#pnlUpdateProgress').hide();
            $('#historyMedModal').modal('show');
            $("#previous").html(data);
        },
        (err) => {
            $('#pnlUpdateProgress').hide();
            console.error(err);
        });
}

function GetCurrentFormularyDetails(previousFormularyVersionId, currentFormularyVersionId) {
    $('#pnlUpdateProgress').show();

    let url = "/Formulary/LoadHistoryFormularyDetails";
    let args = {
        previousFormularyVersionId: previousFormularyVersionId,
        currentFormularyVersionId: currentFormularyVersionId,
        previousOrCurrent: "current"
    };

    ajaxPost(url, args,
        (data) => {
            $('#historyModal').modal('hide');
            $('#pnlUpdateProgress').hide();
            $('#historyMedModal').modal('show');
            $("#current").html(data);
        },
        (err) => {
            $('#pnlUpdateProgress').hide();
            console.error(err);
        });
}

function UpdateFormulary() {
    let formularySaveObj = new studio.FormularySave('#editMedForm', '#Edit_Formulary');

    formularySaveObj.update(
        () => {
            $('#btnUpdateCustomMed').hide();
            $('#btnUpdateCustomMedDisabled').show();
            $('#pnlUpdateProgress').show();
        },
        (data, formInput) => {
            let isBulkEdit = (formInput['IsBulkEdit'] && (formInput['IsBulkEdit'] == true || formInput['IsBulkEdit'] == 'True'))

            if (data != null) {
                $('#inspectorcomp').html(data);
                $('#btnUpdateCustomMed').show();
                $('#btnUpdateCustomMedDisabled').hide();
                $('#pnlUpdateProgress').hide();
                $('#editMedModalBody').animate({
                    scrollTop: 0
                }, 'fast');
            }
            else {
                $('#inspectorcomp').html('');
                $('#btnUpdateCustomMed').show();
                $('#btnUpdateCustomMedDisabled').hide();
                $('#pnlUpdateProgress').hide();
                $('#editMedModal').modal('hide');
                if (isBulkEdit) { //Saved successfully - so refresh tree
                    sessionStorage.removeItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_IDs);
                    sessionStorage.removeItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL);
                    sessionStorage.removeItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_STATUS);
                    $('#lblBulkEditNumber, #lblBulkEditNumbeFloat').html('0');
                    $('#iBulkEdit, #iBulkEditFloat').removeClass('enabled').addClass('disabled');
                }
                callSearchFormularies();
            }
        },
        (err, formInput) => {
            console.error(err);
            $('#inspectorcomp').html('');
            $('#btnUpdateCustomMed').show();
            $('#btnUpdateCustomMedDisabled').hide();
            $('#pnlUpdateProgress').hide();
            $('#editMedModalBody').animate({
                scrollTop: 0
            }, 'fast');
        });
}