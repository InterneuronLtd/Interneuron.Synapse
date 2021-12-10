var onLocatorDeviceSuccess = function (context) {
    window.location = "LocatorBoardDeviceList";
};

var onLocatorDeviceFailed = function (context) {
    alert(context);
};

var onBoardManagerSuccess = function (context) {
    window.location = "BoardManagerList";
};

var onBoardManagerFailed = function (context) {
    alert(context);
};
 
$(document).ready(function () {    
    $("#ListId").change(function () {
        $.getJSON("ListBaseViewContextFieldsJson?id=" + $("#ListId").val(), function (data) {
            $("#ListLocationField").html("");
            $("#ListLocationField").append('<option value="">Please Select...</option>');
            $.each(data, function (i, entity) {
                $("#ListLocationField").append('<option value="' + entity.attributename + '">' + entity.attributename + '</option>');
            })
        });
    });
    $("#BaseViewNamespaceId").change(function () {
        $.getJSON("BaseViewJsonList?id=" + $(this).val(), function (data) {
            $("#BaseViewId").html("");
            $("#BaseViewId").append('<option value="">Please Select...</option>');
            $("#locatorBaseView").html("");
            $("#locatorBaseView").append('<option value="">Please Select...</option>');
            $.each(data, function (i, entity) {
                $("#BaseViewId").append('<option value="' + entity.baseview_id + '">' + entity.baseviewname + '</option>');
                $("#locatorBaseView").append('<option value="' + entity.baseview_id + '">' + entity.baseviewname + '</option>');
            });
        });
    });

    $("#BaseViewId").change(function () {
        $.getJSON("BaseViewContextFieldsJson?id=" + $("#BaseViewId").val(), function (data) {
            $("#PersonIDField,#EncounterIDField,#WardField,#BedField,#TopField,#TopLeftField,#TopRightField,#MiddleField,#MiddleLeftField,#MiddleRightField,#BottomField,#BottomLeftField,#BottomRightField").html("");
            $("#PersonIDField,#EncounterIDField,#WardField,#BedField,#TopField,#TopLeftField,#TopRightField,#MiddleField,#MiddleLeftField,#MiddleRightField,#BottomField,#BottomLeftField,#BottomRightField").append('<option value="">Please Select...</option>');
            $.each(data, function (i, entity) {
                $("#PersonIDField,#EncounterIDField,#WardField,#BedField,#TopField,#TopLeftField,#TopRightField,#MiddleField,#MiddleLeftField,#MiddleRightField,#BottomField,#BottomLeftField,#BottomRightField").append('<option value="' + entity.attributename + '">' + entity.attributename + '</option>');
            });
        });
    });    

    $("#btn-delete-bed-board").click(function () {
        if (confirm('Are you sure that you want to delete this board?')) {
            $.getJSON("../Board/DeleteLocatorBoard?locatorBoardId=" + $(this).attr("data-id"), function () {
                window.location = "../Board/BoardManagerList";
            });
        }
        else {
            return false;
        }
    });
    $("#locatorBaseView").change(function () {
        $("#LocationIDField,#Heading,#TopLeftField,#TopRightField").html("");
        $("#LocationIDField,#Heading,#TopLeftField,#TopRightField").append('<option value="">Please Select...</option>');
        if ($(this).val() !== "") {
            console.log("field loaded");
            $.getJSON("../Board/BaseViewContextFieldsJson?id=" + $(this).val(), function (data) {
                $.each(data, function (i, entity) {
                    $("#LocationIDField,#Heading,#TopLeftField,#TopRightField").append('<option value="' + entity.attributename + '">' + entity.attributename + '</option>');
                })
            });
        }
    });
});