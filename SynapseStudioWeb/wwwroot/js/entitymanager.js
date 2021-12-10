//import { Toast } from "../lib/bootstrap/dist/js/bootstrap.bundle";

var onLocalNamespaceSuccess = function () {
    $("#localNamespaceList").load("GetLocalNamespaceList", function () {
        $.notify("Local Namespace has been added", "success");
        $("#LocalNamespaceName").val("");
        $("#LocalNamespaceDescription").val("");
    });
};
var onLocalNamespaceFailed = function (context) {
    alert(context);
};
var onRelationSuccess = function (context) {
    if (context === "You are unable to create a relation to the same entity" || context ==="The relation that you are trying to create already exists for this entity") {
        $.notify(context, "warning");
    } else {
        $("#relationList").load("GetRelationList?id=" + context, function () {
            $.notify("Relation has has been added", "success");
            $("#AttributeName").val("");
        });
    }
};
var onRelationFailed = function (context) {
    alert(context);
};
var onAttributeSuccess = function (context) {
    var data = JSON.parse(JSON.stringify(context));
    if (data.message) {
        $.notify(context.message, "Error");
    } else {
        $("#attributeList").load("GetAttributeList?id=" + context, function () {
            $.notify("Attribute has been added", "success");
            $("#AttributeName").val("");
        });
    }
};
var onAttributeFailed = function (context) {
    alert(context);
};
var onSuccess = function (context) {
    window.location = "EntityManagerAttribute?id=" + context;
};

var onFailed = function (context) {
    console.log('On Failed');
    console.dir(context);
    //toastr['error']('Are you the 6 fingered man?');
    //alert(context);
};
$(document).ready(function () {
    // Entity Manager
    $("#btn-enable-drop").click(function () {
        if (confirm('Are you sure that you want to delete this entity? This cannot be undone!!')) {
            $("#btn-enable-drop").hide();
            $("#btn-delete-entity").show();
            return true;
        } else {
            return false;
        }
    });
    $("#btn-delete-relation").click(function () {
        if (confirm('Are you sure that you want to delete this relation? This cannot be undone!!')) {

            return true;
        } else {
            return false;
        }
    });
    
    $("#SynapseNamespaceId").change(function () {
        $.getJSON("GetEntityJsonList?synapsenamespaceid=" + $(this).val(), function (data) {
            $("#EntityId").html("");
            $("#EntityId").append('<option value="">Please Select...</option>');
            $.each(data, function (i, entity) {
                $("#EntityId").append('<option value="' + entity.entityId + '">' + entity.entityName + '</option>');
            })
        });
    });
    $("#entitylist").load("GetEntityList?synapsenamespaceid=" + $("#EntityId").val(), function () {
        $('#entitylist').fadeIn('slow');
        var entity = $("#EntityId option:selected").text();
        $("#entityname").text(entity);
        $("#btn-add-entity").val("New " + entity + " entity");
    });

    var entity = $("#EntityId").find("option:selected").text();
    $("#entityname").text(entity);
    $("#btn-add-entity").text("New " + entity + " entity");

    //$("#btn-add-entity").attr("href", "EntityManagerNew?id=" + $("#EntityId").val());
    if (entity === "extended" || entity === "local" || entity === "meta")
        $("#btn-add-entity").attr("href", "EntityManager" + entity + "New?id=" + $("#EntityId").val());
    else
        $("#btn-add-entity").attr("href", "EntityManagerNew?id=" + $("#EntityId").val());

    $("#EntityId").on("change", function () {
        console.log($(this).find("option:selected").text());
        $("#entitylist").load("GetEntityList?synapsenamespaceid=" + $(this).val(), function () {
            $('#entitylist').fadeIn('slow');
            var entity = $("#EntityId").find("option:selected").text();
            $("#entityname").text(entity);
            $("#btn-add-entity").text("New " + entity + " entity");
            if (entity === "extended" || entity === "local" || entity === "meta")
                $("#btn-add-entity").attr("href", "EntityManager" + entity + "New?id=" + $("#EntityId").val());
            else
                $("#btn-add-entity").attr("href", "EntityManagerNew?id=" + $("#EntityId").val());

        });
    });

});