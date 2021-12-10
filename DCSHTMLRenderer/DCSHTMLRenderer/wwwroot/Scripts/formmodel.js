define(["require", "exports", "./JSONMap/FieldValidation", "./JSONMap/Form", "./JSONMap/Section", "./Enumerations/ElementType", "./HTMLControls/TextBox", "./HTMLControls/DropDownList", "./HTMLControls/Radio", "./HTMLControls/CheckBox", "./HTMLControls/TextArea", "./HTMLControls/DateTime", "./HTMLControls/Calendar", "./HTMLControls/Label", "./JSONMap/DisplayRules", "./JSONMap/ConditionGroup", "./JSONMap/Condition", "./Enumerations/Comparator", "./Enumerations/LogicOperator"], function (require, exports, FieldValidation_1, Form_1, Section_1, ElementType_1, TextBox_1, DropDownList_1, Radio_1, CheckBox_1, TextArea_1, DateTime_1, Calendar_1, Label_1, DisplayRules_1, ConditionGroup_1, Condition_1, Comparator_1, LogicOperator_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var FormHelpers;
    (function (FormHelpers) {
        function pushValidation(element, validations) {
            if (validations)
                for (var i = 0; i < validations.length; i++) {
                    var validation = new FieldValidation_1.FieldValidation();
                    validation.validationtype = validations[i].validationtype;
                    validation.comparator = validations[i].comparator;
                    validation.compareField = validations[i].compareField;
                    validation.compareDateValue = validations[i].compareDateValue;
                    validation.compareValue = validations[i].compareValue;
                    validation.minLength = validations[i].minLength;
                    validation.maxLength = validations[i].maxLength;
                    validation.fieldValidationPattern = validations[i].fieldvalidationpattern;
                    validation.validationErrorMsg = validations[i].validationerrormsg;
                    element.fieldValidations.push(validation);
                }
        }
        function pushDisplayRules(element, displayRules) {
            if (displayRules) {
                var conditionGroups = displayRules.conditiongroup;
                element.displayRules = new DisplayRules_1.DisplayRules();
                element.displayRules.conditiongroups = new Array();
                for (var i = 0; i < conditionGroups.length; i++) {
                    var cg = new ConditionGroup_1.Conditiongroup();
                    cg.conditions = new Array();
                    cg.logicoperator = conditionGroups[i].logicoperator;
                    var conditions = conditionGroups[i].condition;
                    for (var i = 0; i < conditions.length; i++) {
                        var c = new Condition_1.Condition();
                        c.compareField_id = conditions[i].field_id;
                        c.compareoperator = conditions[i].compareoperator;
                        c.comparetype = conditions[i].comparetype;
                        c.comparedatevalue = conditions[i].comparedatevalue;
                        c.comparevalue = conditions[i].comparevalue;
                        c.logicOperator = conditions[i].logicoperator;
                        c.order = conditions[i].order;
                        cg.conditions.push(c);
                    }
                    element.displayRules.conditiongroups.push(cg);
                }
            }
        }
        function initializeVisitility(f) {
            var elArray;
            for (var i = 0; i < f.sections.length; i++) {
                for (var j = 0; j < f.sections[i].elements.length; j++) {
                    var el = f.sections[i].elements[j];
                    if (el.displayRules != undefined) {
                        elArray = new Array();
                        var conditiongroups = el.displayRules.conditiongroups;
                        if (conditiongroups != undefined)
                            for (var k = 0; k < conditiongroups.length; k++) {
                                var conditions = conditiongroups[k].conditions;
                                for (var l = 0; l < conditions.length; l++) {
                                    var condition = conditions[l];
                                    var e = f.sections.filter(function (sec) { return sec.elements.filter(function (el) { return el.id == condition.compareField_id; })[0]; })[0].elements.filter(function (el) { return el.id == condition.compareField_id; })[0];
                                    if (e) {
                                        if (elArray.indexOf(e.id) == -1) {
                                            if (e.getControlType() == ElementType_1.ElementType.Date || e.getControlType() == ElementType_1.ElementType.DateTime)
                                                e.registerEvent("blur", setVisibility, el);
                                            else
                                                e.registerEvent("change", setVisibility, el);
                                        }
                                        elArray.push(e.id);
                                    }
                                }
                            }
                    }
                }
            }
        }
        function setVisibility(element) {
            var f = element.enclosingSection.enclosingForm;
            var result = true;
            var loc = LogicOperator_1.LogicOperator.And;
            var locg = LogicOperator_1.LogicOperator.And;
            if (element.displayRules != undefined) {
                var conditiongroups = element.displayRules.conditiongroups;
                if (conditiongroups != undefined)
                    for (var cg = 0; cg < conditiongroups.length; cg++) {
                        var conditions = conditiongroups[cg].conditions;
                        for (var c = 0; c < conditions.length; c++) {
                            var condition = conditions[c];
                            var e = f.sections.filter(function (sec) { return sec.elements.filter(function (el) { return el.id == condition.compareField_id; })[0]; })[0].elements.filter(function (el) { return el.id == condition.compareField_id; })[0];
                            var values = condition.comparevalue;
                            var selectedValues = e.getValue();
                            if (e.getControlType() == ElementType_1.ElementType.CheckBoxList) {
                                if (condition.compareoperator == Comparator_1.Comparators.Filled) {
                                    if (loc == LogicOperator_1.LogicOperator.And)
                                        result = result && selectedValues.length > 0 && selectedValues[0].value != "";
                                    else
                                        result = result || selectedValues.length > 0 && selectedValues[0].value != "";
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.EqualsTo) {
                                    if (Array.isArray(values) && JSON.stringify(selectedValues.map(function (a) { return a.value; }).sort()) != JSON.stringify(values.sort())) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.NotEqualTo) {
                                    var tresult = false;
                                    if (selectedValues[0].value != "") {
                                        tresult = true;
                                    }
                                    if (Array.isArray(values) && JSON.stringify(selectedValues.map(function (a) { return a.value; }).sort()) == JSON.stringify(values.sort())) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = tresult && false;
                                        else
                                            result = tresult || false;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = tresult && true;
                                        else
                                            result = tresult || true;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.Contains) {
                                    var tresult = true;
                                    if (Array.isArray(values))
                                        for (var j = 0; j < values.length; j++) {
                                            if (selectedValues.map(function (a) { return a.value; }).filter(function (item) { return item == values[j]; }).length == 0)
                                                tresult = tresult && false;
                                        }
                                    if (loc == LogicOperator_1.LogicOperator.And)
                                        result = result && tresult;
                                    else
                                        result = result || tresult;
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.DoesNotContain) {
                                    var tresult = false;
                                    if (selectedValues[0].value != "") {
                                        tresult = true;
                                        if (Array.isArray(values))
                                            for (var k = 0; k < values.length; k++) {
                                                if (selectedValues.map(function (a) { return a.value; }).filter(function (item) { return item == values[k]; }).length > 0)
                                                    tresult = tresult && false;
                                            }
                                    }
                                    if (loc == LogicOperator_1.LogicOperator.And)
                                        result = result && tresult;
                                    else
                                        result = result || tresult;
                                }
                            }
                            if (e.getControlType() == ElementType_1.ElementType.RadioButtonList) {
                                if (condition.compareoperator == Comparator_1.Comparators.Filled) {
                                    if (loc == LogicOperator_1.LogicOperator.And)
                                        result = result && selectedValues.length > 0 && selectedValues[0].value != "";
                                    else
                                        result = result || selectedValues.length > 0 && selectedValues[0].value != "";
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.EqualsTo) {
                                    if (Array.isArray(values) && JSON.stringify(selectedValues.map(function (a) { return a.value; }).sort()) != JSON.stringify(values.sort())) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.NotEqualTo) {
                                    var tresult = false;
                                    if (selectedValues[0].value != "")
                                        tresult = true;
                                    if (Array.isArray(values))
                                        for (var k = 0; k < values.length; k++) {
                                            if (selectedValues.map(function (a) { return a.value; }).filter(function (item) { return item == values[k]; }).length > 0)
                                                tresult = tresult && false;
                                        }
                                    if (loc == LogicOperator_1.LogicOperator.And)
                                        result = result && tresult;
                                    else
                                        result = result || tresult;
                                }
                            }
                            else if (e.getControlType() == ElementType_1.ElementType.DropDownList) {
                                if (condition.compareoperator == Comparator_1.Comparators.Filled) {
                                    if (loc == LogicOperator_1.LogicOperator.And)
                                        result = result && selectedValues.length > 0 && selectedValues[0].value.trim().toLowerCase() != "please select";
                                    else
                                        result = result || selectedValues.length > 0 && selectedValues[0].value.trim().toLowerCase() != "please select";
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.EqualsTo) {
                                    if (Array.isArray(values) && JSON.stringify(selectedValues.map(function (a) { return a.value; }).sort()) != JSON.stringify(values.sort())) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.NotEqualTo) {
                                    var tresult = false;
                                    if (selectedValues[0].value.trim().toLowerCase() != "please select")
                                        if (Array.isArray(values)) {
                                            tresult = true;
                                            for (var k = 0; k < values.length; k++) {
                                                if (selectedValues.map(function (a) { return a.value; }).filter(function (item) { return item == values[k]; }).length > 0)
                                                    tresult = tresult && false;
                                            }
                                        }
                                    if (loc == LogicOperator_1.LogicOperator.And)
                                        result = result && tresult;
                                    else
                                        result = result || tresult;
                                }
                            }
                            else if (e.getControlType() == ElementType_1.ElementType.TextBox || e.getControlType() == ElementType_1.ElementType.TextArea) {
                                if (condition.compareoperator == Comparator_1.Comparators.Filled) {
                                    if (loc == LogicOperator_1.LogicOperator.And)
                                        result = result && selectedValues.length > 0 && selectedValues[0].value != "";
                                    else
                                        result = result || selectedValues.length > 0 && selectedValues[0].value != "";
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.EqualsTo) {
                                    if (Array.isArray(values) && selectedValues[0].value != values[0]) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.NotEqualTo) {
                                    var tresult = false;
                                    if (selectedValues[0].value != "")
                                        tresult = true;
                                    if (Array.isArray(values) && selectedValues[0].value == values[0]) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = tresult && false;
                                        else
                                            result = tresult || false;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = tresult && true;
                                        else
                                            result = tresult || true;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.Contains) {
                                    if (Array.isArray(values) && selectedValues[0].value.indexOf(values[0]) == -1) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.DoesNotContain) {
                                    var tresult = false;
                                    if (selectedValues[0].value != "")
                                        tresult = true;
                                    if (Array.isArray(values) && selectedValues[0].value.indexOf(values[0]) != -1) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.GreaterThan) {
                                    if (Array.isArray(values) && parseFloat(selectedValues[0].value) > parseFloat(values[0])) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.GreaterThanOrEqualTo) {
                                    if (Array.isArray(values) && parseFloat(selectedValues[0].value) >= parseFloat(values[0])) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.LessThan) {
                                    if (Array.isArray(values) && parseFloat(selectedValues[0].value) < parseFloat(values[0])) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                }
                                else if (condition.compareoperator == Comparator_1.Comparators.LessThanOrEqualTo) {
                                    if (Array.isArray(values) && parseFloat(selectedValues[0].value) <= parseFloat(values[0])) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                }
                            }
                            else if (e.getControlType() == ElementType_1.ElementType.Date) {
                                if (condition.compareoperator == Comparator_1.Comparators.Filled) {
                                    if (loc == LogicOperator_1.LogicOperator.And)
                                        result = result && selectedValues.length > 0 && selectedValues[0].value != "";
                                    else
                                        result = result || selectedValues.length > 0 && selectedValues[0].value != "";
                                }
                                else if (Array.isArray(values)) {
                                    if ((condition.compareoperator == Comparator_1.Comparators.EqualsTo
                                        || condition.compareoperator == Comparator_1.Comparators.Between
                                        || condition.compareoperator == Comparator_1.Comparators.GreaterThan
                                        || condition.compareoperator == Comparator_1.Comparators.GreaterThanOrEqualTo
                                        || condition.compareoperator == Comparator_1.Comparators.LessThan
                                        || condition.compareoperator == Comparator_1.Comparators.LessThanOrEqualTo
                                        || condition.compareoperator == Comparator_1.Comparators.NotEqualTo)
                                        && e.compareValue(condition.compareoperator, condition.comparedatevalue[0], values[0], condition.comparedatevalue[1], values[1])) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                }
                            }
                            else if (e.getControlType() == ElementType_1.ElementType.DateTime) {
                                if (condition.compareoperator == Comparator_1.Comparators.Filled) {
                                    if (loc == LogicOperator_1.LogicOperator.And)
                                        result = result && selectedValues.length > 0 && selectedValues[0].value != "";
                                    else
                                        result = result || selectedValues.length > 0 && selectedValues[0].value != "";
                                }
                                else if (Array.isArray(values)) {
                                    if ((condition.compareoperator == Comparator_1.Comparators.EqualsTo
                                        || condition.compareoperator == Comparator_1.Comparators.Between
                                        || condition.compareoperator == Comparator_1.Comparators.GreaterThan
                                        || condition.compareoperator == Comparator_1.Comparators.GreaterThanOrEqualTo
                                        || condition.compareoperator == Comparator_1.Comparators.LessThan
                                        || condition.compareoperator == Comparator_1.Comparators.LessThanOrEqualTo
                                        || condition.compareoperator == Comparator_1.Comparators.NotEqualTo)
                                        && e.compareValue(condition.compareoperator, condition.comparedatevalue[0], values[0], condition.comparedatevalue[1], values[1])) {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && true;
                                        else
                                            result = result || true;
                                    }
                                    else {
                                        if (loc == LogicOperator_1.LogicOperator.And)
                                            result = result && false;
                                        else
                                            result = result || false;
                                    }
                                }
                            }
                            loc = condition.logicOperator;
                        }
                        locg = conditiongroups[cg].logicoperator;
                    }
                element.setDisplay(result);
            }
        }
        FormHelpers.setVisibility = setVisibility;
        function deserialize(obj) {
            var f = new Form_1.Form();
            f.id = obj.form.form_id;
            f.forminstance_id = obj.form.forminstance_id;
            f.name = obj.form.formname;
            f.description = obj.form.description;
            f.formparameters = obj.form.formparameters;
            f.destinationentities = obj.form.destinationentities;
            for (var i in obj.form.formsections) {
                var s = new Section_1.Section();
                s.elements = [];
                s.id = obj.form.formsections[i].formsection_id;
                s.title = obj.form.formsections[i].title;
                s.enclosingForm = f;
                if (typeof obj.form.formsections[i].desciption != undefined && obj.form.formsections[i].desciption != null) {
                    s.desciption = obj.form.formsections[i].desciption;
                }
                else {
                    s.desciption = "";
                }
                s.banner = obj.form.formsections[i].banner;
                s.displayorder = parseInt(obj.form.formsections[i].displayorder);
                // ..initialize other section attributes                
                for (var j in obj.form.formsections[i].sectionfields) {
                    var field = obj.form.formsections[i].sectionfields[j];
                    if (field.fieldcontroltypename == ElementType_1.ElementType.TextBox) {
                        var df = field.defaultvalue;
                        if (typeof field.fieldvalue != undefined && field.fieldvalue) {
                            df = field.fieldvalue[0];
                        }
                        else {
                            if (typeof field.defaultvalue != undefined && field.defaultvalue) {
                                df = field.defaultvalue[0];
                            }
                        }
                        var tb = new TextBox_1.TextBox(field.field_id, df);
                        tb.labelText = field.fieldlabeltext;
                        tb.displayOrder = field.fielddisplayorder;
                        // ..initialize other field attributes
                        pushValidation(tb, field.fieldvalidations);
                        pushDisplayRules(tb, field.displayrules);
                        tb.enclosingSection = s;
                        s.elements.push(tb);
                    }
                    if (field.fieldcontroltypename == ElementType_1.ElementType.DropDownList) {
                        //create element with id and options
                        var ddl = new DropDownList_1.DropDownList(field.field_id, field.optionliststatement);
                        //set question label text
                        ddl.labelText = field.fieldlabeltext;
                        ddl.displayOrder = field.fielddisplayorder;
                        pushValidation(ddl, field.fieldvalidations);
                        //set default value
                        if (typeof field.fieldvalue != undefined && field.fieldvalue) {
                            ddl.defaultvalue = field.fieldvalue[0];
                        }
                        else {
                            if (typeof field.defaultvalue != undefined && field.defaultvalue) {
                                ddl.defaultvalue = field.defaultvalue[0];
                            }
                        }
                        ddl.setDefaultValue();
                        // ..initialize other field attributes
                        pushDisplayRules(ddl, field.displayrules);
                        ddl.enclosingSection = s;
                        s.elements.push(ddl);
                    }
                    if (field.fieldcontroltypename == ElementType_1.ElementType.RadioButtonList) {
                        var rbl = new Radio_1.Radio(field.field_id, field.optionliststatement);
                        rbl.id = field.field_id;
                        rbl.labelText = field.fieldlabeltext;
                        rbl.displayOrder = field.fielddisplayorder;
                        pushValidation(rbl, field.fieldvalidations);
                        if (typeof field.fieldvalue != undefined && field.fieldvalue) {
                            rbl.defaultvalue = field.fieldvalue;
                        }
                        else {
                            if (typeof field.defaultvalue != undefined && field.defaultvalue) {
                                rbl.defaultvalue = field.defaultvalue;
                            }
                        }
                        rbl.setDefaultValue();
                        pushDisplayRules(rbl, field.displayrules);
                        rbl.enclosingSection = s;
                        s.elements.push(rbl);
                    }
                    if (field.fieldcontroltypename == ElementType_1.ElementType.CheckBoxList) {
                        var chkbl = new CheckBox_1.Checkbox(field.field_id, field.optionliststatement);
                        chkbl.id = field.field_id;
                        chkbl.labelText = field.fieldlabeltext;
                        chkbl.displayOrder = field.fielddisplayorder;
                        pushValidation(chkbl, field.fieldvalidations);
                        if (typeof field.fieldvalue != undefined && field.fieldvalue) {
                            chkbl.defaultvalue = field.fieldvalue;
                        }
                        else {
                            if (typeof field.defaultvalue != undefined && field.defaultvalue) {
                                chkbl.defaultvalue = field.defaultvalue;
                            }
                        }
                        chkbl.setDefaultValue();
                        pushDisplayRules(chkbl, field.displayrules);
                        chkbl.enclosingSection = s;
                        s.elements.push(chkbl);
                    }
                    if (field.fieldcontroltypename == ElementType_1.ElementType.TextArea) {
                        var ta = new TextArea_1.Textarea(field.field_id);
                        ta.id = field.field_id;
                        ta.labelText = field.fieldlabeltext;
                        ta.displayOrder = field.fielddisplayorder;
                        pushValidation(ta, field.fieldvalidations);
                        if (typeof field.fieldvalue != undefined && field.fieldvalue) {
                            ta.defaultvalue = field.fieldvalue;
                        }
                        else {
                            if (typeof field.defaultvalue != undefined && field.defaultvalue) {
                                ta.defaultvalue = field.defaultvalue;
                            }
                        }
                        ta.setDefaultValue();
                        pushDisplayRules(ta, field.displayrules);
                        ta.enclosingSection = s;
                        s.elements.push(ta);
                    }
                    if (field.fieldcontroltypename == ElementType_1.ElementType.DateTime) {
                        var dttm = new DateTime_1.Datetime(field.field_id, field.defaultvalue);
                        dttm.labelText = field.fieldlabeltext;
                        dttm.displayOrder = field.fielddisplayorder;
                        pushValidation(dttm, field.fieldvalidations);
                        // ..initialize other field attributes
                        pushDisplayRules(dttm, field.displayrules);
                        dttm.enclosingSection = s;
                        s.elements.push(dttm);
                    }
                    if (field.fieldcontroltypename == ElementType_1.ElementType.Date) {
                        var dt = new Calendar_1.Calendar(field.field_id, field.defaultvalue);
                        dt.labelText = field.fieldlabeltext;
                        dt.displayOrder = field.fielddisplayorder;
                        pushValidation(dt, field.fieldvalidations);
                        // ..initialize other field attributes
                        pushDisplayRules(dt, field.displayrules);
                        dt.enclosingSection = s;
                        s.elements.push(dt);
                    }
                    if (field.fieldcontroltypename == ElementType_1.ElementType.Label) {
                        var lbl = new Label_1.Label(field.field_id, field.defaultvalue);
                        lbl.labelText = field.fieldlabeltext;
                        lbl.displayOrder = field.fielddisplayorder;
                        // ..initialize other field attributes
                        lbl.enclosingSection = s;
                        s.elements.push(lbl);
                    }
                    // field loop ends here
                }
                f.sections.push(s);
                // section loop ends
            }
            initializeVisitility(f);
            return f;
        }
        FormHelpers.deserialize = deserialize;
    })(FormHelpers = exports.FormHelpers || (exports.FormHelpers = {}));
});
//# sourceMappingURL=formmodel.js.map