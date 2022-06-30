var selectedItems = [];

class medicationListTree {

    constructTreeView() {
        let glyph_opts = {
            preset: "awesome5",
            map: {
            }
        };

        $("#tree2").fancytree({
            extensions: ["glyph"],
            source: [],
            checkbox: true,
            selectMode: 3, //2, //default value
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
            renderNode: function (event, data) {
                // Optionally tweak data.node.span
                //              $(data.node.span).text(">>" + data.node.title);
                let nodeVal = data.node;
                if (nodeVal && nodeVal.data) {
                    nodeVal.unselectable = false;
                    //nodeVal.addClass("bg-success");
                    let recStatusCode = nodeVal.data['recStatusCode'];
                    let nodeLevel = nodeVal.data['Level'];
                    if (nodeLevel == 'AMP' && recStatusCode === '001') {
                        nodeVal.unselectable = true;
                        var $span = $(nodeVal.span);
                        if ($span) $span.find("> span.fancytree-title").addClass('dmdDraftMedColor');
                    }
                }

                //if (node && (recStatusCode === '001')) {
                //    node.addClass("bg-success");
                //    node.unselectable = true;
                //    //var $span = $(node.span);
                //    //$span.find("> span.fancytree-title").addClass('text-muted');
                //    node.render();
                //}
            },
            imagePath: "../img/",
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
            select: function (event, data) {
                if (data.node.isSelected()) {
                    let nodeDataVal = data.node.data;
                    //if (nodeDataVal && (nodeDataVal.Level == 'VTM' || nodeDataVal.Level == 'VMP' ||
                    //    (nodeDataVal.Level == 'AMP' && nodeDataVal.recStatusCode == '001'))) {
                    //    data.node.setSelected(false);
                    //}
                    if (nodeDataVal && (nodeDataVal.Level == 'VTM' || nodeDataVal.Level == 'VMP')) {
                        data.node.setSelected(false);
                    }
                }
                if (data.options.selectMode == 3) {
                    //let selectedNodes = data.tree.getSelectedNodes();
                    data.options.selectMode = 2;
                    //for (var i = 0; i < selectedNodes.length; i++) {
                    //    let selectedNode = selectedNodes[i];
                    //    let nodeDataVal = selectedNode.data;
                    //    if (nodeDataVal && (nodeDataVal.Level == 'VTM' || nodeDataVal.Level == 'VMP' ||
                    //        (nodeDataVal.Level == 'AMP' && nodeDataVal.recStatusCode == '001'))) {
                    //        selectedNode.setSelected(false);
                    //    }
                    //}
                    data.options.selectMode = 3;
                }
            },
            glyph: glyph_opts,
            expand: function (event, data) {
                formularyMedicationNodeExpandHandler(event, data);
            },
            //collapse: function (event, data) {
            //    let allNodeCollapsed = true;
            //    var tree = data.tree;
            //    tree.visit((treenode) => {
            //        allNodeCollapsed = allNodeCollapsed && !treenode.isExpanded();
            //    });
            //    //if (allNodeCollapsed)
            //        //$('#lnkCollapseAll').removeClass('fa-caret-down').addClass('fa-caret-right');
            //},
            click: function (event, data) {
                sessionStorage.setItem("DMD_SEL_TARGET_LEVEL", "");
            }
        });
    }

}

$.contextMenu({
    selector: "#tree2 span.fancytree-title",
    build: function ($trigger, e) {

        let node = $.ui.fancytree.getNode($trigger);

        let medMenuObj = {};
        medMenuObj.callback = (key, options) => {
            medicationContextMenuCallbackHandler(key, options, node);
        };

        medMenuObj.items = {};

        medMenuObj = new FormularyMedicationContextMenuItemsBuilder(medMenuObj, node).buildMenu();

        return medMenuObj;
    }
});

function medicationContextMenuCallbackHandler(key, options, node) {
    if (key && key === 'quit') return true;
    let currentNode = node;
    sessionStorage.setItem("DMD_SEL_TARGET_LEVEL", '');

    if (currentNode && currentNode.data) {
        if (currentNode.data.Level == 'VTM') {
            if (key === 'DelAMP') {
                currentNode.visit(vmpNode => {
                    if (vmpNode) {
                        vmpNode.visit(ampNode => ampNode.setSelected(false));
                    }
                });
            } else {
                sessionStorage.setItem("DMD_SEL_TARGET_LEVEL", key);

                if (currentNode.isExpanded())
                    currentNode.setExpanded(false);

                setTimeout(() => {
                    currentNode.setExpanded(true);
                    currentNode.children.forEach((vmpNode) => {
                        vmpNode.setExpanded(true);
                    });
                }, 500);
            }
        } else if (currentNode.data.Level == 'VMP') {
            if (key === 'DelAMP') {
                currentNode.visit(ampNode => ampNode.setSelected(false));
            } else {
                sessionStorage.setItem("DMD_SEL_TARGET_LEVEL", key);

                if (currentNode.isExpanded())
                    currentNode.setExpanded(false);

                setTimeout(() => { currentNode.setExpanded(true); }, 500);
            }
        }
    }
}

function formularyMedicationNodeExpandHandler(event, data) {

    let targetLevelSelected = sessionStorage.getItem("DMD_SEL_TARGET_LEVEL");

    if (!targetLevelSelected) return;

    switch (data.node.data.Level) {
        case "VTM":
            if (!targetLevelSelected || !(targetLevelSelected === "AMP" || targetLevelSelected === "VMP")) break;

            if (data.node.children && data.node.children.length > 0) {
                $.each(data.node.children, function (index, vmpNode) {
                    if (targetLevelSelected === "AMP") {
                        if (vmpNode && vmpNode.children && vmpNode.children.length > 0) {
                            $.each(vmpNode.children, function (index, ampNode) {
                                if (!ampNode.unselectable) {
                                    ampNode.setSelected(true);
                                }
                            });
                        }
                        else {
                            vmpNode.setExpanded(true);
                        }
                    }
                });
            }
            break;
        case "VMP":
            if (!targetLevelSelected || targetLevelSelected !== "AMP") break;

            if (data.node.children && data.node.children.length > 0) {
                $.each(data.node.children, function (index, ampNode) {
                    if (!ampNode.unselectable) {
                        ampNode.setSelected(true);
                    }
                });
            }
            break;
    }
}

$(document).ready(function () {

    sessionStorage.setItem("DMD_SEL_TARGET_LEVEL", "");

    $('#medModal').keypress((event) => {
        if (event.keyCode == 13) {
            $('#btnSearchDMDMedication').click();
        }
    });

    if (selectedItems.length <= 0) {
        $("#btnImport").attr("disabled", true);
    }
    else {
        $("#btnImport").attr("disabled", false);
    }

    $("#btnImport").show();
    $("#btnImportDisabled").hide();

    let treeList = new medicationListTree();

    treeList.constructTreeView();

    $('#chkImportAutoSelect').click(function () {
        let isAutoSelectChecked = $(this).is(":checked");
        let tree = $.ui.fancytree.getTree("#tree2");

        if (isAutoSelectChecked) {
            tree.setOption("selectMode", 3);
        }
        else {
            tree.setOption("selectMode", 2);
        }
    });


    $('#btnSyncPendingDMDs').on('click', function () {
        let searchText = $('#txtSearchDMDMed').val();

        if (searchText && searchText.length <= 2) return;

        searchText = searchText.replace(/[^\w\s%.\-/+*,]/gi, '');

        let searchCriteria = {
            searchTxt: searchText
        };

        $('#modal-spinner').show();
        $('#tree2').hide();

        ajaxPostJson('/Formulary/LoadSyncPendingDMDList/', searchCriteria,
            (data) => {
                $('#modal-spinner').hide();
                $('#tree2').show();

                let tree = $.ui.fancytree.getTree("#tree2");
                tree.reload(data);
            },
            err => {
                console.err(err);
                toastr.error(err);
                $('#modal-spinner').hide();
                $('#tree2').show();
            });
    });


    $("#btnSearchDMDMedication").on('click', function () {
        let searchText = $('#txtSearchDMDMed').val();

        if (searchText.length > 2) {
            searchText = searchText.replace(/[^\w\s%.\-/+*,]/gi, '');

            if (searchText != "") {
                let searchCriteria = {
                    searchTxt: searchText
                };

                getTreeData(searchCriteria,
                    () => {
                        $('#modal-spinner').show();
                        $('#tree2').hide();
                    },
                    (isSuccess, err) => {
                        //markRecordsInFormularies(searchText);
                        //getFormularies('Formulary/GetLatestFormulariesHeaderOnly', null,
                        getFormularies('Formulary/SearchFormularyByName', { q: searchText },
                            () => {
                                $('#modal-spinner').show();
                                $('#tree2').hide();
                            },
                            (isSuccess, err) => {
                                $('#modal-spinner').hide();
                                $('#tree2').show();
                            });
                        $('#modal-spinner').hide();
                        $('#tree2').show();
                    });
            }
        }
    });

    $("#btnAddMed").on('click', function () {

        var tree = $.ui.fancytree.getTree("#tree2");
        var selectedNodes = tree.getSelectedNodes();

        if (!selectedNodes || selectedNodes.length == 0) return;

        let ampNodes = selectedNodes.filter(rec => rec.data && rec.data.Level === 'AMP');

        if (!ampNodes || ampNodes.length == 0) return;

        for (let ampIndex = 0; ampIndex < ampNodes.length; ampIndex++) {

            let nodeSelected = ampNodes[ampIndex];

            if (selectedItems && selectedItems.length > 0) {
                let hasRecordAlready = selectedItems.some(rec => rec.Key === nodeSelected.key);
                if (hasRecordAlready) continue;
            }

            let selectedItem = { 'Key': nodeSelected.key, 'Title': nodeSelected.title, 'Level': 'AMP' };
            let parentItems = [];

            let selectedNodesParents = getSelectedNodesWithParents([nodeSelected], true);

            if (selectedNodesParents && selectedNodesParents.length > 0) {
                for (let selIndex = 0; selIndex < selectedNodesParents.length; selIndex++) {
                    let parentNodeSelected = selectedNodesParents[selIndex];
                    let parentItem = { 'Key': parentNodeSelected.key, 'Title': parentNodeSelected.title, 'Level': parentNodeSelected.data.Level };
                    parentItems.push(parentItem);
                }
            }
            selectedItem['Parents'] = parentItems;
            selectedItems.push(selectedItem);
        }

        //let selectedNodesParents = getSelectedNodesWithParents(ampNodes, true);

        //for (var i = 0; i < selectedNodesWithParents.length; i++) {
        //    let nodeSelected = selectedNodesWithParents[i];
        //    var addedItems = selectedItems.filter(x => x.Key == nodeSelected.key);

        //    if (addedItems.length == 0) {
        //        let amps = 
        //        let level = nodeSelected.data ? nodeSelected.data.Level : '';
        //        let parentLevel = nodeSelected.parent && nodeSelected.parent.data ? nodeSelected.parent.data.Level : '';
        //        let parentKey = nodeSelected.parent ? nodeSelected.parent.key : '';

        //        selectedItems.push({ 'Key': nodeSelected.key, 'Title': nodeSelected.title, 'Level': level, 'ParentLevel': parentLevel, 'ParentKey': parentKey });
        //    }
        //}

        selectedItems.sort(function (a, b) {
            if (a.Title < b.Title) { return -1; }
            if (a.Title > b.Title) { return 1; }
            return 0;
        });

        addRemoveCheckboxesFromDiv(selectedItems);

        if (selectedItems.length <= 0) {
            $("#btnImport").attr("disabled", true);
        }
        else {
            $("#btnImport").attr("disabled", false);
        }
    });



    $("#btnRemoveMed").on('click', function () {
        getAllCheckedCheckboxesAndRemoveFromArray();
        addRemoveCheckboxesFromDiv(selectedItems);
    });

    $("#btnImport").on('click', function () {

        $.confirm('Are you sure you want to import the selected ones?', (action) => {
            if (action === 'yes') {
                importDMDs();
            }
        });
    });

    $("#medModal").on("hidden.bs.modal", function () {
        selectedItems = [];
        /*
         * Medication.js should be within the modal 
         * - To Be refactored and the below lines to be uncommented 
         * - Probable candidates for memory leak
        $('#medModal #chkImportAutoSelect').off();
        $("#medModal #btnSearchDMDMedication").off();
        $("#medModal #btnAddMed").off();
        $("#medModal #btnRemoveMed").off();
        $("#medModal #btnImport").off();
        */
        $("#txtSearchDMDMed").val("");
        $('#dvSelectedNodes').empty();
        var tree = $.ui.fancytree.getTree("#tree2");
        tree.setOption("selectMode", 3);//2);
        tree.clear();
    });

    getDMDVersion('Formulary/GetDMDVersion', null, null, null);
});

function importDMDs() {
    let meds = [];

    for (var i = 0; i < selectedItems.length; i++) {
        let selectedItem = selectedItems[i];
        meds.push(selectedItem.Key);

        if (selectedItem.Parents && selectedItem.Parents.length > 0) {
            for (let parentIndex = 0; parentIndex < selectedItem.Parents.length; parentIndex++) {
                let parentItem = selectedItem.Parents[parentIndex];
                meds.push(parentItem.Key);
            }
        }
    }

    $('#pnlUpdateProgress').show();
    $("#btnImport").hide();
    $("#btnImportDisabled").show();

    let args = { 'meds': meds };

    ajaxPostJson('/Formulary/ImportMeds', args,
        (data) => {
            if (!data) {//Due to some error
                $("#btnImport").show();
                $("#btnImportDisabled").hide();
                $('#pnlUpdateProgress').hide();
            } else {//no error
                $('#pnlUpdateProgress').hide();
                $('#medModal').modal('hide');
                if (callSearchFormularies) callSearchFormularies();
            }
        },
        (err) => {
            console.error(err);
            $("#btnImport").show();
            $("#btnImportDisabled").hide();
            $('#pnlUpdateProgress').hide();
        });
}

function getSelectedNodesWithParents(selectedNodes, excludeCurrentNode) {
    if (!selectedNodes || selectedNodes.length == 0) return null;

    let allSelected = excludeCurrentNode ? [] : selectedNodes;
    for (selIndex = 0; selIndex < selectedNodes.length; selIndex++) {
        var currentSelNode = selectedNodes[selIndex];
        if (currentSelNode.parent != null) {
            currentSelNode.visitParents((parentNode) => {
                if (parentNode && parentNode.data && (parentNode.data.Level == 'VTM' || parentNode.data.Level == 'VMP' || parentNode.data.Level == 'AMP'))
                    allSelected.push(parentNode);

                //if (parentNode.parent != null) {
                //    parentNode.visitParents((parentOfParentNode) => {
                //        if (parentOfParentNode && parentOfParentNode.data && (parentOfParentNode.data.Level == 'VTM' || parentOfParentNode.data.Level == 'VMP' || parentOfParentNode.data.Level == 'AMP'))
                //            allSelected.push(parentOfParentNode);
                //    });
                //}
            });
        }
    }
    return allSelected;
}

function getAllCheckedCheckboxesAndRemoveFromArray() {
    var checks = document.querySelectorAll('#dvSelectedNodes input[type="checkbox"]');
    for (var i = 0; i < checks.length; i++) {
        var check = checks[i];
        if (check.checked) {
            selectedItems = removeItemAll(selectedItems, check.value);
        }
    }
    if (selectedItems.length <= 0) {
        $("#btnImport").attr("disabled", true);
    }
    else {
        $("#btnImport").attr("disabled", false);
    }
}

function removeItemAll(arr, value) {
    var i = 0;
    while (i < arr.length) {
        if (arr[i].Key === value) {
            arr.splice(i, 1);
        } else {
            ++i;
        }
    }
    return arr;
}

function addRemoveCheckboxesFromDiv(selectedItems) {
    $('#dvSelectedNodes').empty();
    var dv = document.getElementById("dvSelectedNodes");

    for (var i = 0; i < selectedItems.length; i++) {
        let selectedItem = selectedItems[i];
        var chk = document.createElement('input');
        var lbl = document.createElement('label');
        var linebreak = document.createElement("br");

        chk.setAttribute('type', 'checkbox');
        chk.setAttribute('value', selectedItem.Key);
        chk.setAttribute('id', 'chk' + selectedItem.Key);
        chk.setAttribute('class', 'form-check-input');

        //lbl.setAttribute('for', 'chk' + selectedItem.Key);
        lbl.setAttribute('class', 'form-check-label');

        let title = selectedItem.Title;
        if (selectedItem.Parents && selectedItem.Parents.length > 0) {
            for (let parentIndex = 0; parentIndex < selectedItem.Parents.length; parentIndex++) {
                let parent = selectedItem.Parents[parentIndex];
                title = `${title} [${parent.Level}:- ${parent.Title}]`;
            }
        }
        lbl.appendChild(document.createTextNode(title));

        dv.appendChild(chk);
        dv.appendChild(lbl);
        dv.appendChild(linebreak);
    }
}

function getTreeData(args, beforeLoad, afterLoad) {
    if (beforeLoad) {
        beforeLoad();
    }
    ajaxPostJson('/Formulary/LoadDMDList/', args,
        (data) => {
            if (afterLoad) {
                afterLoad(true);
            }
            var tree = $.ui.fancytree.getTree("#tree2");
            tree.reload(data);
        });
}

function getDMDVersion(url, args, beforeLoad, afterLoad) {
    if (beforeLoad) {
        beforeLoad();
    }
    ajaxGetJson(url, args,
        (data) => {
            if (afterLoad) {
                afterLoad(true);
            }
            if (!data || data.length == 0) return;

            //var str = data.replace("nhsbsa_dmd_", "").substr(0, data.indexOf("_") - 1);

            var str = data;

            $('#dmdVersion').html(str);
        });
}

function getFormularies(url, args, beforeLoad, afterLoad) {
    if (beforeLoad) {
        beforeLoad();
    }
    ajaxGetJson(url, args,
        (data) => {
            if (afterLoad) {
                afterLoad(true);
            }
            if (!data || data.length == 0) return;

            var recordsInDraft = getRecordsInDraft(data);

            let tree = $.ui.fancytree.getTree("#tree2");

            for (let resIndex = 0; resIndex < data.length; resIndex++) {
                let dataItem = data[resIndex];
                let codeVal = dataItem.code || '';
                //let recStatusCode = dataItem.recStatusCode;
                let node = tree.getNodeByKey(codeVal);

                if (node) {
                    node.addClass("dmdMedBackground");
                    if (node.data)
                        node.data['recStatusCode'] = recordsInDraft[codeVal] || '';
                }

                //if (node && (recStatusCode === '001')) {
                //    node.addClass("bg-success");
                //    node.unselectable = true;
                //    //var $span = $(node.span);
                //    //$span.find("> span.fancytree-title").addClass('text-muted');
                //    node.render();
                //}


                //if (data[i].children.length > 0) {
                //    for (var j = 0; j < data[i].children.length; j++) {
                //        if (tree.getNodeByKey(data[i].children[j].key) != null) {
                //            tree.getNodeByKey(data[i].children[j].key).addClass("bg-success");
                //        }
                //        if (data[i].children[j].children.length > 0) {
                //            for (var k = 0; k < data[i].children[j].children.length; k++) {
                //                if (tree.getNodeByKey(data[i].children[j].children[k].key) != null) {
                //                    tree.getNodeByKey(data[i].children[j].children[k].key).addClass("bg-success");
                //                }
                //            }
                //        }
                //    }
                //}

            }
        });
}

function getRecordsInDraft(data) {
    let recordsInDraft = {};
    if (!data || data.length == 0) return recordsInDraft;

    var draftRecs = data.filter(rec => rec.recStatusCode === '001');
    if (!draftRecs || draftRecs.length == 0) return recordsInDraft;

    for (let draftRecIndex = 0; draftRecIndex < draftRecs.length; draftRecIndex++) {
        let draftRec = draftRecs[draftRecIndex];
        recordsInDraft[draftRec.code] = '001';
    }

    return recordsInDraft;
}