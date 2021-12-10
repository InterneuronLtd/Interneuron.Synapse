let glyph_opts = {
    preset: "awesome5",
    map: {
    }
};
let nodesAddedForEdit = [];

let DATA_SOURCE = [
    {
        title: 'Abatacept', key: '1', data: { Level: 'VTM', recordstatus: { description: 'Draft', isDuplicate: false } }, expanded: false,
        children: [
            {
                title: 'Abatacept 125mg / 1ml solution for injection pre- filled disposable devices', key: '1.1', data: { Level: 'VMP', recordstatus: { description: 'Draft', isDuplicate: false } }, expanded: false,
                children: [
                    { title: 'Orencia ClickJect 125mg/1ml solution for injection pre-filled pens (Bristol-Myers Squibb Pharmaceuticals Ltd)', key: '1.1.1', data: { Level: 'AMP', recordstatus: { description: 'Draft', isDuplicate: false } } }
                ]
            }]
    },
    {
        title: 'Baricitinib', key: '2', data: { Level: 'VTM', recordstatus: { description: 'Draft', isDuplicate: false } }, expanded: false,
        children: [
            {
                title: 'Baricitinib 4mg tablets', key: '2.1', data: { Level: 'VMP', recordstatus: { description: 'Draft', isDuplicate: false } }, expanded: false,
                children: [
                    { title: 'Olumiant 4mg tablets (Eli Lilly and Company Ltd)', key: '2.1.1', data: { Level: 'AMP', recordstatus: { description: 'Draft', isDuplicate: false } } }
                ]


            },
            {
                title: 'Baricitinib 2mg tablets', key: '2.2', data: { Level: 'VMP', recordstatus: { description: 'Draft', isDuplicate: false } }, expanded: false,
                children: [
                    { title: 'Barium Sulphate 94% powder for rectal suspension', key: '2.2.1', data: { Level: 'AMP', recordstatus: { description: 'Draft', isDuplicate: false } } }
                ]
            }]
    },
    {
        title: 'Diltiazem', key: '3', data: { Level: 'VTM', recordstatus: { description: 'Draft', isDuplicate: false } }, expanded: false,
        children: [
            {
                title: 'Diltiazem 60mg modified-release tablets', key: '3.1', data: { Level: 'VMP', recordstatus: { description: 'Draft', isDuplicate: false } }, expanded: false,
                children: [
                    { title: 'Tildiem 60mg modified-release tablets (Waymade Healthcare Plc)', key: '3.1.1', data: { Level: 'AMP', recordstatus: { description: 'Draft', isDuplicate: false } } },
                    { title: 'Tildiem 60mg modified-release tablets (DE Pharmaceuticals)', key: '3.1.2', data: { Level: 'AMP', recordstatus: { description: 'Draft', isDuplicate: false } } },
                    { title: 'Tildiem 60mg modified-release tablets (Dowelhurst Ltd)', key: '3.1.3', data: { Level: 'AMP', recordstatus: { description: 'Draft', isDuplicate: false } } }
                ]
            },
            {
                title: 'Diltiazem 2% cream', key: '3.2', data: { Level: 'VMP', recordstatus: { description: 'Draft', isDuplicate: false } }, expanded: false
            }]
    }
];

$("#treetable").fancytree({
    extensions: ["glyph", "table"],
    checkbox: true,
    icon: function (event, data) {
        switch (data.node.data.Level) {
            case "VTM":
                return "Medication type not defined.svg";
            case "VMP":
                return "Capsule.svg";
            case "AMP":
                return "Actual Medicinal Product.svg";
        }
    },
    imagePath: "/img/",
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
    source: DATA_SOURCE,
    table: {
        checkboxColumnIdx: 0,
        nodeColumnIdx: 1
    },
    activate: function (event, data) {
    },
    lazyLoad: (event, data) => {
        this.loadChildren(data);
    },
    select: function (event, data) {
        
        if (data.node.selected) {

            if (data.node.data.Level == 'VTM' || data.node.data.Level == 'VMP') {
                toastr.error('Error: At a time you cannot edit records of different levels. AMP level records are already in queue for edit');
                return;
            }

            nodesAddedForEdit.push(data);
            $('#lblBulkEditNumber').html(nodesAddedForEdit.length);

            let tree = $.ui.fancytree.getTree("#treetable");
            tree.visit(function (treenode) {
                treenode.unselectable = data.node.data.Level != treenode.data.Level ? true : false;
                treenode.render();
            });

            toastr.info('Record added for bulk edit');

        } else {
           
            nodesAddedForEdit.pop(data);
            $('#lblBulkEditNumber').html(nodesAddedForEdit.length);
            toastr.info('Record removed');

            if (!nodesAddedForEdit || nodesAddedForEdit.length == 0) {
                let tree = $.ui.fancytree.getTree("#treetable");
                tree.visit(function (treenode) {
                    treenode.unselectable = false;
                    treenode.render();
                });
                return;
            }
        }
        // Get a list of all selected nodes, and convert to a key array:
        //var selKeys = $.map(data.tree.getSelectedNodes(), function (node) {
        //    return node.key;
        //});

        //$("#inspectortext").text(selKeys.join(", "));
    },
    click: function (event, data) {
        //let targetType = $.ui.fancytree.getEventTargetType(event.originalEvent);
        //let target = $.ui.fancytree.getEventTarget(event.originalEvent);

        //if (targetType === "checkbox") {
        //    debugger;
        //    let $input = $(event.originalEvent.target);
        //    if ($input.is(':checked')) {
        //        nodesAddedForEdit.push(data);
        //        toastr.info('Record added to bucket for edit');
        //    } else {
        //        nodesAddedForEdit.pop(data);
        //        toastr.info('Record removed from bucket');
        //    }
        //}
        //if (nodeClick) nodeClick(event, data);
    },
    renderColumns: function (event, data) {
        var node = data.node,
            $tdList = $(node.tr).find(">td");
        //$tdList.eq(0).text(node.getIndexHier());
        if (node.data.recordstatus.isDuplicate === true) {
            $tdList.eq(1).addClass("bg-warning text-white");
            node.setExpanded(true);
        }
        //$tdList.eq(2).text(node.data.recordstatus.description);
        $tdList.eq(2).html("<a href='#' class='status' name='recordstatus'>"+ node.data.recordstatus.description +"</a>");
    }
});


$('#iBulkEdit').click(function () {
    $('#bulkEditModal').modal('show');

    //$(`#bulkEditModal :checkbox`).bootstrapToggle('destroy');

    //setTimeout(() => {
    //    $(`#bulkEditModal :checkbox`).bootstrapToggle({
    //        on: 'Yes',
    //        off: 'No',
    //        onstyle: 'success',
    //        offstyle: 'danger',
    //        width: 50,
    //        size: 'sm'
    //    });
    //}, 100);
});

$("#treetable").on("click", "i[name=node_edit]", function (e) {
    var node = $.ui.fancytree.getNode(e),
        $input = $(e.target);

    e.stopPropagation();  // prevent fancytree activate for this row
    $('#inspectorModalEditor').modal('show');
    //if ($input.is(":checked")) {
    //    alert("like " + node);
    //} else {
    //    alert("dislike " + node);
    //}
});

$("#treetable").on("click", "i[name=node_delete]", function (e) {
    e.stopPropagation();  // prevent fancytree activate for this row
    $('#deleteFormulary').modal('show');
});

//$("#treetable").on("click", "i[name=node_change_status]", function (e) {
//    e.stopPropagation();  // prevent fancytree activate for this row
//    $('#changestatus').modal('show');
//});

$('#customMed').click(function () {
    $('#customMedModal').modal('show');
});

$("#treetable").on("click", 'a[name=recordstatus]', function (e) {
    e.stopPropagation();  // prevent fancytree activate for this row
    $('#changestatus').modal('show');
});

$('#iDeSelectAll').click(function (e) {
    let tree = $.ui.fancytree.getTree("#treetable");
    nodesAddedForEdit = [];
    tree.visit(function (node) {
        node.setSelected(false);
    });
});


setTimeout(() => {
    $(`#chkSearchArchive`).bootstrapToggle({
        on: 'Yes',
        off: 'No',
        onstyle: 'success',
        offstyle: 'danger',
        width: 70,
        size: 'sm'
    });
}, 100);

$('#lblFilter').click(function () {
    $('#pnlFilterContainer').toggle();
    $('#pnlFilterResults').toggle();
});

$('#btnApplyFilter').click(function () {
    $('#pnlFilterContainer').hide();
    $('#pnlFilterResults').show();
});

$('#lnkCollapseAll').click(function () {
    let tree = $.ui.fancytree.getTree("#treetable");
    tree.visit(function (node) {
        node.setExpanded(false);
    });
});

$(document).on('click', '#btnApproveBulkEdit', function () {
    $('#bulkEditModal').modal('hide');
    setTimeout(() => {
        $('#bulkEditAMPModalEditor').modal('show');
    },50);
});