var f;

$(document).ready(function () {
    $("#wait").hide();

    $.ajax({
        type: "GET",
        url: "https://localhost:44307/form/2/per1/enc1/observationid/obs1",
        //contentType: 'application/json',
        //data: JSON.stringify(formData),
        success: function (formJson) {
            require(['./formmodel'], function (form) {

                //var obj = JSON.parse(document.getElementById("json").innerText.replace(/\"\\/g, "\\").replace(/&quot;/g, '"').replace(/\"\"/g, '"'));
                
                f = form.FormHelpers.deserialize(JSON.parse(formJson));
                $("#form").append(f.getMarkup());

                $('.date').datetimepicker({ format: 'DD/MM/YYYY' }); $('.datetime').datetimepicker({ format: 'DD/MM/YYYY HH:mm' });
            });
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            $("#Error").show();
            $("#wait").hide();
        }
    });
});

function submitForm() {

    var formData = new Object();
    var doSubmit = true;
    formData.form_id = f.id;
    formData.forminstance_id = f.forminstance_id;
    formData.person_id = '12345';
    formData.encounter_id = '98765';
    formData.formcontext = '12345';
    formData.formparameters = f.formparameters;
    formData.destinationentities = f.destinationentities;
    console.log(f.formparameters);
    console.log(f.destinationentities);
    formData.formsections = [];

    for (var i = 0; i < f.sections.length; i++) {
        var formSection = new Object();
        formSection.section_id = f.sections[i].id;
        formSection.title = f.sections[i].title;
        formSection.sectionfields = [];
        for (var j = 0; j < f.sections[i].elements.length; j++) {
            var sectionField = new Object();
            sectionField.field_id = f.sections[i].elements[j].id;
            sectionField.fieldcontroltypename = f.sections[i].elements[j].getControlType();
            sectionField.fieldlabeltext = f.sections[i].elements[j].labelText;
            sectionField.fielddata = f.sections[i].elements[j].getValue();

            sectionField.fieldvalidation = f.sections[i].elements[j].validate();
            doSubmit = doSubmit && sectionField.fieldvalidation.result;
            //console.log(f.sections[i].elements[j].getControlType());
            //console.log(f.sections[i].elements[j].validate().result);

            formSection.sectionfields.push(sectionField);
        }
        formData.formsections.push(formSection);
    }

    console.log(JSON.stringify(formData));
    if (doSubmit) {
        $("#wait").show();
        $("#form").hide();
        $.ajax({
            type: "POST",
            url: "https://localhost:44307/FormResponse",
            contentType: 'application/json',
            data: JSON.stringify(formData),
            success: function () {
                $("#FormSubmitted").show();
                $("#wait").hide();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                $("#Error").show();
                $("#wait").hide();
            }
        });
    }
}