var onBaseviewNamespaceSuccess = function () {
    $("#BaseviewNamespaceList").load("GetBaseviewNamespaceList", function () {
        $.notify("Baseview Namespace has been added", "success");
        $("#BaseviewNamespace").val("");
        $("#BaseviewNamespaceDescription").val("");
    });
};
var onBaseviewNamespaceFailed = function (context) {
    alert(context);
};
var onBaseviewSQLSuccess = function (context) {
    if (context.indexOf("Re-Validate") != -1) {
        $.notify("Baseview has been validated", "success");
        $("#btn-sql-validate").hide();
        $("#btn-sql-create").show();
    } else if (context.indexOf("Recreate Baseview") != -1) {
        $.notify("Baseview has been re created", "success");
    } else {
        $.notify(context, { autoHide: false, className: "error" });
    }
};
var onBaseviewSQLFailed = function (context) {
    alert(context);
};
function onBaseviewStart() {
    $("#BaseviewName").removeAttr("disabled");
    $("#BaseviewSQL").removeAttr("disabled");
    return true;
};
var onBaseviewSuccess = function (data) {
    console.log(data);
    if (data.indexOf("Error") !== -1)
        $.notify(data, "error");
    else if (data === "OK") {
        $.notify("Baseview validated successfully", "success");
        $("#BaseviewName").attr("disabled", "disabled");
        $("#BaseviewSQL").attr("disabled", "disabled");
        $("#btn-validate").hide();
        $("#btn-save-cancal").show();
        $("#btn-submit-save").show();
    }
    else {
        $.notify("Baseview has been added", "success");
        $("#BaseviewName").val("");
        $("#BaseviewDesc").val("");
        $("#BaseviewSQL").val("");
        $("#BaseviewName").removeAttr("disabled");
        $("#BaseviewSQL").removeAttr("disabled");
        $("#btn-validate").show();
        $("#btn-save-cancal").hide();
        $("#btn-submit-save").hide();
    }
};
var onBaseviewFailed = function (context) {
    alert(context);
};

$(document).ready(function () {

    // base view
    $("#baseViewlist").load("GetBaseViewList?id=" + $("#Baseview_id").val(), function () {
        var baseviewname = $("#Baseview_id option:selected").text();
        $("#baseviewname").text(baseviewname);
        $("#btn-add-baseview").val("New " + baseviewname + " Baseview");
    });
    var baseviewname = $("#Baseview_id").find("option:selected").text();
    $("#baseviewname").text(baseviewname);
    $("#btn-add-baseview").text("New " + baseviewname + " Baseview");
    $("#btn-add-baseview").attr("href", "BaseViewManagerNew?id=" + $("#Baseview_id").val());
    $("#Baseview_id").on("change", function () {
        $("#baseViewlist").load("GetBaseViewList?id=" + $(this).val(), function () {
            var baseviewname = $("#Baseview_id option:selected").text();
            $("#baseviewname").text(baseviewname);
            $("#btn-add-baseview").text("New " + baseviewname + " Baseview");
            $("#btn-add-baseview").attr("href", "BaseViewManagerNew?id=" + $("#Baseview_id").val());
        });
    });
    $("#btn-save-cancal").click(function () {
        $("#BaseviewName").val("");
        $("#BaseviewDesc").val("");
        $("#BaseviewSQL").val("");
        $("#BaseviewName").removeAttr("disabled");
        $("#BaseviewSQL").removeAttr("disabled");
        $("#btn-validate").show();
        $("#btn-save-cancal").hide();
        $("#btn-submit-save").hide();
    });
    $("#btn-enable-baseview").click(function () {
        if (confirm('Are you sure that you want to delete this baseview? This cannot be undone!!')) {
            $("#btn-enable-baseview").hide();
            $("#btn-delete-baseview").show();
            return true;
        } else {
            return false;
        }

    });
    $("#btn-re-create-baseview").click(function () {
        $.getJSON("BaseViewManagerReCreate", function () {
            $.notify("Baseview has been created", "success");
        });
    });
});
