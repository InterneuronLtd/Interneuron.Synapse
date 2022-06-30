//BEGIN LICENSE BLOCK 
//Interneuron Synapse

//Copyright(C) 2022  Interneuron CIC

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
//END LICENSE BLOCK 
﻿import { SectionElement } from "./HTMLControls/SectionElement";
import { FieldValidation } from "./JSONMap/FieldValidation";
import { Form } from "./JSONMap/Form";
import { Section } from "./JSONMap/Section";
import { ElementType } from "./Enumerations/ElementType";
import { TextBox } from "./HTMLControls/TextBox";
import { DropDownList } from "./HTMLControls/DropDownList";
import { Radio } from "./HTMLControls/Radio";
import { Checkbox } from "./HTMLControls/CheckBox";
import { Textarea } from "./HTMLControls/TextArea";
import { Datetime } from "./HTMLControls/DateTime";
import { Calendar } from "./HTMLControls/Calendar";
import { Label } from "./HTMLControls/Label";
import { DisplayRules } from "./JSONMap/DisplayRules";
import { Conditiongroup } from "./JSONMap/ConditionGroup";
import { Condition } from "./JSONMap/Condition";
import { Comparators } from "./Enumerations/Comparator";
import { LogicOperator } from "./Enumerations/LogicOperator";

export module FormHelpers {
    function pushValidation(element: SectionElement, validations: any): void {
        if (validations)
            for (var i = 0; i < validations.length; i++) {
                var validation = new FieldValidation();
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
    function pushDisplayRules(element: SectionElement, displayRules: any) {
        if (displayRules) {
            var conditionGroups = displayRules.conditiongroup;
            element.displayRules = new DisplayRules();
            element.displayRules.conditiongroups = new Array<Conditiongroup>();
            for (var i = 0; i < conditionGroups.length; i++) {
                var cg = new Conditiongroup();
                cg.conditions = new Array<Condition>();
                cg.logicoperator = conditionGroups[i].logicoperator;
                var conditions = conditionGroups[i].condition;
                for (var i = 0; i < conditions.length; i++) {
                    var c = new Condition();
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
    function initializeVisitility(f: Form) {
        var elArray: string[];
        for (var i = 0; i < f.sections.length; i++) {
            for (var j = 0; j < f.sections[i].elements.length; j++) {
                var el = f.sections[i].elements[j];

                if (el.displayRules != undefined) {
                    elArray = new Array<string>();
                    var conditiongroups = el.displayRules.conditiongroups;
                    if (conditiongroups != undefined)
                        for (var k = 0; k < conditiongroups.length; k++) {
                            var conditions = conditiongroups[k].conditions;
                            for (var l = 0; l < conditions.length; l++) {
                                var condition = conditions[l];
                                var e = f.sections.filter(sec => sec.elements.filter(el => el.id == condition.compareField_id)[0])[0].elements.filter(el => el.id == condition.compareField_id)[0];
                                if (e) {
                                    if (elArray.indexOf(e.id) == -1) {
                                        if (e.getControlType() == ElementType.Date || e.getControlType() == ElementType.DateTime)
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
    export function setVisibility(element: SectionElement) {
        var f = element.enclosingSection.enclosingForm;
        var result = true;
        var loc = LogicOperator.And;
        var locg = LogicOperator.And;
        if (element.displayRules != undefined) {
            var conditiongroups = element.displayRules.conditiongroups;
            if (conditiongroups != undefined)
                for (var cg = 0; cg < conditiongroups.length; cg++) {
                    var conditions = conditiongroups[cg].conditions;
                    for (var c = 0; c < conditions.length; c++) {
                        var condition = conditions[c];
                        var e = f.sections.filter(sec => sec.elements.filter(el => el.id == condition.compareField_id)[0])[0].elements.filter(el => el.id == condition.compareField_id)[0];
                        var values = condition.comparevalue;
                        var selectedValues = e.getValue();

                        if (e.getControlType() == ElementType.CheckBoxList) {
                            if (condition.compareoperator == Comparators.Filled) {
                                if (loc == LogicOperator.And)
                                    result = result && selectedValues.length > 0 && selectedValues[0].value != "";
                                else
                                    result = result || selectedValues.length > 0 && selectedValues[0].value != "";
                            }
                            else if (condition.compareoperator == Comparators.EqualsTo) {
                                if (Array.isArray(values) && JSON.stringify(selectedValues.map(a => a.value).sort()) != JSON.stringify(values.sort())) {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                }
                                else {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                }
                            }
                            else if (condition.compareoperator == Comparators.NotEqualTo) {
                                var tresult = false
                                if (selectedValues[0].value != "") {
                                    tresult = true;
                                }
                                if (Array.isArray(values) && JSON.stringify(selectedValues.map(a => a.value).sort()) == JSON.stringify(values.sort())) {
                                    if (loc == LogicOperator.And)
                                        result = tresult && false;
                                    else
                                        result = tresult || false;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = tresult && true;
                                    else
                                        result = tresult || true;
                                }

                            }
                            else if (condition.compareoperator == Comparators.Contains) {
                                var tresult = true;
                                if (Array.isArray(values))
                                    for (var j = 0; j < values.length; j++) {
                                        if (selectedValues.map(a => a.value).filter(item => item == values[j]).length == 0)
                                            tresult = tresult && false;
                                    }
                                if (loc == LogicOperator.And)
                                    result = result && tresult;
                                else
                                    result = result || tresult;
                            }
                            else if (condition.compareoperator == Comparators.DoesNotContain) {
                                var tresult = false;
                                if (selectedValues[0].value != "") {
                                    tresult = true;
                                    if (Array.isArray(values))
                                        for (var k = 0; k < values.length; k++) {
                                            if (selectedValues.map(a => a.value).filter(item => item == values[k]).length > 0)
                                                tresult = tresult && false;
                                        }
                                }
                                if (loc == LogicOperator.And)
                                    result = result && tresult;
                                else
                                    result = result || tresult;

                            }
                        }
                        if (e.getControlType() == ElementType.RadioButtonList) {
                            if (condition.compareoperator == Comparators.Filled) {
                                if (loc == LogicOperator.And)
                                    result = result && selectedValues.length > 0 && selectedValues[0].value != "";
                                else
                                    result = result || selectedValues.length > 0 && selectedValues[0].value != "";
                            }
                            else if (condition.compareoperator == Comparators.EqualsTo) {
                                if (Array.isArray(values) && JSON.stringify(selectedValues.map(a => a.value).sort()) != JSON.stringify(values.sort())) {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;

                                }
                            }
                            else if (condition.compareoperator == Comparators.NotEqualTo) {
                                var tresult = false;
                                if (selectedValues[0].value != "")
                                    tresult = true;
                                if (Array.isArray(values))
                                    for (var k = 0; k < values.length; k++) {
                                        if (selectedValues.map(a => a.value).filter(item => item == values[k]).length > 0)
                                            tresult = tresult && false;
                                    }
                                if (loc == LogicOperator.And)
                                    result = result && tresult;
                                else
                                    result = result || tresult;
                            }
                        }
                        else if (e.getControlType() == ElementType.DropDownList) {
                            if (condition.compareoperator == Comparators.Filled) {
                                if (loc == LogicOperator.And)
                                    result = result && selectedValues.length > 0 && selectedValues[0].value.trim().toLowerCase() != "please select";
                                else
                                    result = result || selectedValues.length > 0 && selectedValues[0].value.trim().toLowerCase() != "please select";
                            }
                            else if (condition.compareoperator == Comparators.EqualsTo) {
                                if (Array.isArray(values) && JSON.stringify(selectedValues.map(a => a.value).sort()) != JSON.stringify(values.sort())) {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                }
                            }
                            else if (condition.compareoperator == Comparators.NotEqualTo) {
                                var tresult = false;
                                if (selectedValues[0].value.trim().toLowerCase() != "please select")
                                    if (Array.isArray(values)) {
                                        tresult = true;
                                        for (var k = 0; k < values.length; k++) {
                                            if (selectedValues.map(a => a.value).filter(item => item == values[k]).length > 0)
                                                tresult = tresult && false;
                                        }
                                    }
                                if (loc == LogicOperator.And)
                                    result = result && tresult;
                                else
                                    result = result || tresult;
                            }
                        }
                        else if (e.getControlType() == ElementType.TextBox || e.getControlType() == ElementType.TextArea) {
                            if (condition.compareoperator == Comparators.Filled) {
                                if (loc == LogicOperator.And)
                                    result = result && selectedValues.length > 0 && selectedValues[0].value != "";
                                else
                                    result = result || selectedValues.length > 0 && selectedValues[0].value != "";
                            }
                            else if (condition.compareoperator == Comparators.EqualsTo) {
                                if (Array.isArray(values) && selectedValues[0].value != values[0]) {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                }
                                else {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                }
                            }
                            else if (condition.compareoperator == Comparators.NotEqualTo) {
                                var tresult = false;
                                if (selectedValues[0].value != "")
                                    tresult = true;

                                if (Array.isArray(values) && selectedValues[0].value == values[0]) {
                                    if (loc == LogicOperator.And)
                                        result = tresult && false;
                                    else
                                        result = tresult || false;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = tresult && true;
                                    else
                                        result = tresult || true;
                                }
                            }
                            else if (condition.compareoperator == Comparators.Contains) {
                                if (Array.isArray(values) && selectedValues[0].value.indexOf(values[0]) == -1) {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                }
                            }
                            else if (condition.compareoperator == Comparators.DoesNotContain) {
                                var tresult = false;

                                if (selectedValues[0].value != "")
                                    tresult = true;
                                if (Array.isArray(values) && selectedValues[0].value.indexOf(values[0]) != -1) {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                }
                            }
                            else if (condition.compareoperator == Comparators.GreaterThan) {
                                if (Array.isArray(values) && parseFloat(selectedValues[0].value) > parseFloat(values[0])) {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                }
                            }
                            else if (condition.compareoperator == Comparators.GreaterThanOrEqualTo) {
                                if (Array.isArray(values) && parseFloat(selectedValues[0].value) >= parseFloat(values[0])) {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                }
                            }
                            else if (condition.compareoperator == Comparators.LessThan) {
                                if (Array.isArray(values) && parseFloat(selectedValues[0].value) < parseFloat(values[0])) {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                }
                            }
                            else if (condition.compareoperator == Comparators.LessThanOrEqualTo) {
                                if (Array.isArray(values) && parseFloat(selectedValues[0].value) <= parseFloat(values[0])) {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                }
                            }
                        }
                        else if (e.getControlType() == ElementType.Date) {
                            if (condition.compareoperator == Comparators.Filled) {
                                if (loc == LogicOperator.And)
                                    result = result && selectedValues.length > 0 && selectedValues[0].value != "";
                                else
                                    result = result || selectedValues.length > 0 && selectedValues[0].value != "";
                            }
                            else if (Array.isArray(values)) {
                                if ((condition.compareoperator == Comparators.EqualsTo
                                    || condition.compareoperator == Comparators.Between
                                    || condition.compareoperator == Comparators.GreaterThan
                                    || condition.compareoperator == Comparators.GreaterThanOrEqualTo
                                    || condition.compareoperator == Comparators.LessThan
                                    || condition.compareoperator == Comparators.LessThanOrEqualTo
                                    || condition.compareoperator == Comparators.NotEqualTo)
                                    && (e as Calendar).compareValue(condition.compareoperator,
                                        condition.comparedatevalue[0],
                                        values[0],
                                        condition.comparedatevalue[1],
                                        values[1])) {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                } else {
                                    if (loc == LogicOperator.And)
                                        result = result && false;
                                    else
                                        result = result || false;
                                }
                            }
                        }
                        else if (e.getControlType() == ElementType.DateTime) {
                            if (condition.compareoperator == Comparators.Filled) {
                                if (loc == LogicOperator.And)
                                    result = result && selectedValues.length > 0 && selectedValues[0].value != "";
                                else
                                    result = result || selectedValues.length > 0 && selectedValues[0].value != "";
                            }
                            else if (Array.isArray(values)) {
                                if ((condition.compareoperator == Comparators.EqualsTo
                                    || condition.compareoperator == Comparators.Between
                                    || condition.compareoperator == Comparators.GreaterThan
                                    || condition.compareoperator == Comparators.GreaterThanOrEqualTo
                                    || condition.compareoperator == Comparators.LessThan
                                    || condition.compareoperator == Comparators.LessThanOrEqualTo
                                    || condition.compareoperator == Comparators.NotEqualTo)
                                    && (e as Datetime).compareValue(condition.compareoperator,
                                        condition.comparedatevalue[0],
                                        values[0],
                                        condition.comparedatevalue[1],
                                        values[1])) {
                                    if (loc == LogicOperator.And)
                                        result = result && true;
                                    else
                                        result = result || true;
                                } else {
                                    if (loc == LogicOperator.And)
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
    export function deserialize(obj: any): Form {
        var f = new Form();
        f.id = obj.form.form_id;
        f.forminstance_id = obj.form.forminstance_id;
        f.name = obj.form.formname;
        f.description = obj.form.description;
        f.formparameters = obj.form.formparameters;
        f.destinationentities = obj.form.destinationentities;

        for (var i in obj.form.formsections) {
            var s = new Section();
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
                if (field.fieldcontroltypename == ElementType.TextBox) {
                    var df = field.defaultvalue;
                    if (typeof field.fieldvalue != undefined && field.fieldvalue) {
                        df = field.fieldvalue[0];
                    }
                    else {
                        if (typeof field.defaultvalue != undefined && field.defaultvalue) {
                            df = field.defaultvalue[0];
                        }
                    }
                    var tb = new TextBox(field.field_id, df); 
                    tb.labelText = field.fieldlabeltext;
                    tb.displayOrder = field.fielddisplayorder;
                    // ..initialize other field attributes
                    pushValidation(tb, field.fieldvalidations);
                    pushDisplayRules(tb, field.displayrules)
                    tb.enclosingSection = s;
                    s.elements.push(tb);

                }

                if (field.fieldcontroltypename == ElementType.DropDownList) {
                    //create element with id and options
                    var ddl = new DropDownList(field.field_id, field.optionliststatement);
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
                    pushDisplayRules(ddl, field.displayrules)

                    ddl.enclosingSection = s;
                    s.elements.push(ddl);
                }

                if (field.fieldcontroltypename == ElementType.RadioButtonList) {
                    var rbl = new Radio(field.field_id, field.optionliststatement);
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
                    pushDisplayRules(rbl, field.displayrules)

                    rbl.enclosingSection = s;
                    s.elements.push(rbl);
                }
                if (field.fieldcontroltypename == ElementType.CheckBoxList) {

                    var chkbl = new Checkbox(field.field_id, field.optionliststatement);
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
                    pushDisplayRules(chkbl, field.displayrules)

                    chkbl.enclosingSection = s;
                    s.elements.push(chkbl);
                }
                if (field.fieldcontroltypename == ElementType.TextArea) {

                    var ta = new Textarea(field.field_id);
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
                    pushDisplayRules(ta, field.displayrules)

                    ta.enclosingSection = s;
                    s.elements.push(ta);
                }
                if (field.fieldcontroltypename == ElementType.DateTime) {
                    var dttm = new Datetime(field.field_id, field.defaultvalue);
                    dttm.labelText = field.fieldlabeltext;
                    dttm.displayOrder = field.fielddisplayorder;
                    pushValidation(dttm, field.fieldvalidations);
                    // ..initialize other field attributes
                    pushDisplayRules(dttm, field.displayrules)

                    dttm.enclosingSection = s;
                    s.elements.push(dttm);
                }
                if (field.fieldcontroltypename == ElementType.Date) {
                    var dt = new Calendar(field.field_id, field.defaultvalue);
                    dt.labelText = field.fieldlabeltext;
                    dt.displayOrder = field.fielddisplayorder;
                    pushValidation(dt, field.fieldvalidations);
                    // ..initialize other field attributes
                    pushDisplayRules(dt, field.displayrules)

                    dt.enclosingSection = s;
                    s.elements.push(dt);
                }
                if (field.fieldcontroltypename == ElementType.Label) {
                    var lbl = new Label(field.field_id, field.defaultvalue);
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
}