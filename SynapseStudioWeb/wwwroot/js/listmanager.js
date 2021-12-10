var onListNamespaceSuccess = function () {
    $("#ListNamespaceList").load("GetListNamespaceList", function () {
        $.notify("List Namespace has been added", "success");
        $("#ListNamespace").val("");
        $("#ListNamespaceDescription").val("");
    });
};
var onListNamespaceFailed = function (context) {
    alert(context);
};

var onListOptionSuccess = function (context) {
    window.location = "ListOptionCollectionView?id=" + context;
};

var onListOptionFailed = function (context) {
    alert(context);
};
var onListQuestionSuccess = function (context) {
    window.location = "ListManagerList";
};

var onListQuestionFailed = function (context) {
    alert(context);
};
var onListManagerSuccess = function (context) {
    window.location = "ListManagerList";
};

var onListManagerFailed = function (context) {
    $.notify(context, "error");
};
function removePersona(id) {
    $.get("DeletePersona?id=" + id, function (data) {
        $("#list_persona").html(data);
        bindPersonDropdown();
    });
}
function bindPersonDropdown() {
    if ($("#BaseViewId").val() && $("#BaseViewId").val() != "0") {
        $.getJSON("BaseViewFieldsJson?id=" + $("#BaseViewId").val(), function (data) {
            $("#BaseviewFieldId").html("");
            $("#BaseviewFieldId").append('<option value="0">Please Select...</option>');
            $.each(data, function (i, entity) {
                $("#BaseviewFieldId").append('<option value="' + entity.value + '">' + entity.text + '</option>');
            });
        });
    }
    $.getJSON("PesonaJson", function (data) {
        $("#PersonaContextFieldId").html("");
        $("#PersonaContextFieldId").append('<option value="0">Please Select...</option>');
        $.each(data, function (i, entity) {
            $("#PersonaContextFieldId").append('<option value="' + entity.value + '">' + entity.text + '</option>');
        });
    });
}
$(document).ready(function () {
    $("#btn-delete").click(function () {
        $.getJSON("ListOptionDelete?id=" + $("#OptionID").val() + "&optionCollection=" + $("#OptionCollectionID").val(), function (data) {
            window.location = "/List/ListOptionCollectionView?id=" + data;
        });
    });
    $("#btn-AddPersona").click(function () {
        var ptext = $("#PersonaContextFieldId option:selected").text();
        $.get("AddPersona?id=" + $("#PersonaContextFieldId").val() + "&dispayname=" + ptext + "&baseview=" + $("#BaseviewFieldId").val() + "&list_id=" + $("#list_id").val(), function (data) {
            $("#list_persona").html(data);
            bindPersonDropdown();
        });

    });



    // list manager
    $("#managerlist").load("GetListManager?id=" + $("#EntityId").val(), function () {
        var entity = $("#EntityId option:selected").text();
        $("#listname").text(entity);
        $("#btn-add-list").text("New " + entity + " List");
        $("#btn-add-list").attr("href", "../List/ListManagerNew?id=" + $("#EntityId").val());
    });

    $("#EntityId").on("change", function () {
        $("#managerlist").load("GetListManager?id=" + $(this).val(), function () {
            var entity = $("#EntityId option:selected").text();
            $("#listname").text(entity);
            $("#btn-add-list").text("New " + entity + " List");
            $("#btn-add-list").attr("href", "../List/ListManagerNew?id=" + $("#EntityId").val());
        });
    });

    $.getJSON("BaseViewJsonList?id=" + $("#BaseViewNamespaceId").val(), function (data) {
        console.log(data);
        $("#BaseViewId").html("");
        $("#BaseViewId").append('<option value="0">Please Select...</option>');
        $.each(data, function (i, entity) {
            $("#BaseViewId").append('<option value="' + entity.baseview_id + '">' + entity.baseviewname + '</option>');
        });
        bindPersonDropdown();
    });

    $("#BaseViewNamespaceId").change(function () {
        $.getJSON("BaseViewJsonList?id=" + $(this).val(), function (data) {
            $("#BaseViewId").html("");
            console.log(data);
            $("#BaseViewId").append('<option value="0">Please Select...</option>');
            $.each(data, function (i, entity) {
                $("#BaseViewId").append('<option value="' + entity.baseview_id + '">' + entity.baseviewname + '</option>');
            });
        });
    });

    $("#BaseViewId").change(function () {
        $.getJSON("DateContextJsonList?id=" + $("#BaseViewId").val(), function (data) {
            $("#DateContextField").html("");
            $("#DateContextField").append('<option value="0">Please Select...</option>');
            $.each(data, function (i, entity) {
                $("#DateContextField").append('<option value="' + entity.attributename + '">' + entity.attributename + '</option>');
            });
        });
    });

    $("#DefaultContextId").change(function () {
        $.getJSON("BaseViewContextFieldsJson?id=" + $("#BaseViewId").val(), function (data) {
            $("#BaseviewFieldId,#MatchContextFieldId,#DefaultSortColumnId,#PatientBannerFieldId,#RowCSSFieldId,#SnapshotLine1Id,#SnapshotLine2Id,#SnapshotBadgeId").html("");
            $("#MatchContextFieldId,#DefaultSortColumnId,#PatientBannerFieldId,#RowCSSFieldId,#SnapshotLine1Id,#SnapshotLine2Id,#SnapshotBadgeId").append('<option value="0">Please Select...</option>');
            $.each(data, function (i, entity) {
                $("#BaseviewFieldId,#MatchContextFieldId,#DefaultSortColumnId,#PatientBannerFieldId,#RowCSSFieldId,#SnapshotLine1Id,#SnapshotLine2Id,#SnapshotBadgeId").append('<option value="' + entity.attributename + '">' + entity.attributename + '</option>');
            });
        });
    });

    CheckQuestionTypeSelection($("#QuestionTypeId").val());
    $("#QuestionTypeId").change(function () {
        CheckQuestionTypeSelection($(this).val());
    });
    $("#OptionTypeId").change(function () {
        CheckOptionTypeSelection();
    });

});

function CheckQuestionTypeSelection(id) {

    $("#fgLabelText").hide();
    $("#fgCustomHTML").hide();
    $("#fgDefaultValueDate").hide();
    $("#fgDefaultValueText").hide();
    $("#pnlOptions").hide();
    $("#fgCustomHTMLAlt").hide();
    $('#LabelText').rules('add', { required: true });
    $('#CustomHTML').rules('remove', 'required');
   

    //this.lblCustomHTML.Text = "* * enter the custom HTML that you wish to display";

    switch (id) {
        case "bbc7acbc-b968-4dad-b9d2-ee22ce943a35":  //"Text Box (Limit 255)"                1
            $("#fgLabelText").show();
            $("#fgCustomHTML").hide();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").show();
            $("#pnlOptions").hide();

            break;
        case "feb547a3-3b84-40c7-8007-547c9fe267e9":  //"Text Area (No limit)"                2
            $("#fgLabelText").show();
            $("#fgCustomHTML").hide();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").show();
            $("#pnlOptions").hide();
            break;
        case "3aa99ab6-9df6-4c3a-a966-6cc51ce1a3bf":  //"HTML Tag (Label, Custom HTML)"       3
            $("#fgLabelText").hide();
            $("#fgCustomHTML").show();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").hide();
            $("#pnlOptions").hide();
            $('#LabelText').rules('remove', 'required');
            $('#CustomHTML').rules('add', { required: true });
            break;
        case "fc1f2643-b491-4889-8d1a-910619b65722":  //"Drop Down List"                      4
            $("#fgLabelText").show();
            $("#fgCustomHTML").hide();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").hide();
            $("#pnlOptions").show();
            CheckOptionTypeSelection();
            break;

        case "ca1f1b24-b490-4e57-8921-9f680819e47c": // "Radio Button List"
            $("#fgLabelText").show();
            $("#fgCustomHTML").hide();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").show();
            $("#pnlOptions").show();
            CheckOptionTypeSelection();
            break;

        case "71490eff-a54b-455a-86b1-a4d5ab676f32": // "Radio Button Image List"
            $("#fgLabelText").show();
            $("#fgCustomHTML").hide();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").show();
            $("#pnlOptions").show();
            CheckOptionTypeSelection();
            break;

        case "3d236e17-e40e-472d-95a5-5e45c5e02faf":  //"Check Box List"                      5
            $("#fgLabelText").show();
            $("#fgCustomHTML").hide();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").show();
            $("#pnlOptions").show();
            CheckOptionTypeSelection();
            break;
        case "6c166d07-53d0-4cd3-80f4-801cadcc88eb":  //"Calendar Control"                    6
            $("#fgLabelText").show();
            $("#fgCustomHTML").hide();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").hide();
            $("#pnlOptions").hide();
            break;
        case "83d4fd68-ac33-4996-bd2a-8b6338526520":  //"Time Picker"                         7
            $("#fgLabelText").show();
            $("#fgCustomHTML").hide();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").hide();
            $("#pnlOptions").hide();
            break;
        case "4f31c02d-fa36-4033-8977-8f25bef33d52":  //"Auto-complete Selection List"        8
            $("#fgLabelText").show();
            $("#fgCustomHTML").hide();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").show();
            $("#pnlOptions").show();
            CheckOptionTypeSelection();
            break;
        case "164c31d5-d32e-4c97-91d6-a0d01822b9b6":  //"Single Checkbox (Binary)"        9

            $("#fgLabelText").show();
            $("#fgCustomHTML").show();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").hide();
            $("#pnlOptions").hide();
            $("#fgCustomHTMLAlt").show();
            //this.lblCustomHTML.Text = "* enter the custom html you want to display if the value has been selected";
            break;
        case "221ca4a0-3a39-42ff-a0f4-885ffde0f0bd":  //"Checkbox Image (Binary)"        10
            this.fgLabelText.Visible = true;
            this.fgCustomHTML.Visible = true;
            this.fgCustomHTMLAlt.Visible = true;
            this.fgDefaultValueDate.Visible = false;
            this.fgDefaultValueText.Visible = false;
            this.pnlOptions.Visible = false;
            $("#fgLabelText").show();
            $("#fgCustomHTML").show();
            $("#fgDefaultValueDate").hide();
            $("#fgDefaultValueText").hide();
            $("#pnlOptions").hide();
            $("#fgCustomHTMLAlt").show();
            //this.lblCustomHTML.Text = "* enter the custom html you want to display if the value has been selected";
            break;

        default:

            break;

    }
}
function CheckOptionTypeSelection() {
    $("#fgOptionCollection").hide();
    $("#fgOptionSQLStatement").hide();
    var op = $("#OptionTypeId").val();
    switch (op) {
        case "e9e6feda-f02d-4388-8c5b-9fc97558c684": //  "Internal Option Collection"
            $("#fgOptionCollection").show();
            $("#fgOptionSQLStatement").hide();
            break;
        case "638dadd6-fca7-4f9b-b25f-692c45172524": //  "Custom SQL Staetement"
            $("#fgOptionCollection").hide();
            $("#fgOptionSQLStatement").show();
            break;
        default:
            break;
    }

    $('#OptionTypeId').rules('remove', 'required');
    $('#OptionSQLStatement').rules('remove', 'required');

    if ($('#QuestionTypeId').val() === "fc1f2643-b491-4889-8d1a-910619b65722" ||
        $('#QuestionTypeId').val() === "3d236e17-e40e-472d-95a5-5e45c5e02faf" ||
        $('#QuestionTypeId').val() === "4f31c02d-fa36-4033-8977-8f25bef33d52" ||
        $('#QuestionTypeId').val() === "ca1f1b24-b490-4e57-8921-9f680819e47c" ||
        $('#QuestionTypeId').val() === "71490eff-a54b-455a-86b1-a4d5ab676f32"
    ) {
        if ($("#OptionTypeId").val() === "e9e6feda-f02d-4388-8c5b-9fc97558c684")//Internal Option Collection
        {
            $('#OptionTypeId').rules('add', { required: true });
        }


        if ($("#OptionTypeId").val() === "638dadd6-fca7-4f9b-b25f-692c45172524") //Custom SQL Statement
        {
            $('#OptionSQLStatement').rules('add', { required: true });
        }
    }

}
function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

