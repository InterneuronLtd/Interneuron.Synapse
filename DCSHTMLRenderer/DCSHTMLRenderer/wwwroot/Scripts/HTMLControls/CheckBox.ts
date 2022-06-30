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
﻿import { ElementType } from "../Enumerations/ElementType";
import { SectionElement } from "./SectionElement";
import { OptionListStatement } from "../JSONMap/OptionListStatement";
import { CssClassNames } from "../Enumerations/CssClassNames";
import { FieldValidation } from "../JSONMap/FieldValidation";
import { ValidationTypes } from "../Enumerations/ValidationTypes";
import { ValidationResult } from "../JSONMap/ValidationResult";
import { FieldValue } from "../JSONMap/FieldValue";
import { FormHelpers } from "../formmodel";

export class Checkbox extends SectionElement {
    wrapperElement = document.createElement("div");
    domElement = document.createElement("div");
    errorLabel = document.createElement("label");
    inputElement: HTMLInputElement;
    labelElement: HTMLLabelElement;
    fieldValidations = new Array<FieldValidation>();

    constructor();
    constructor(id: string, optionList: OptionListStatement[], defaultValue?: string[]);
    constructor(id?: string, optionList?: OptionListStatement[], defaultValue?: string[]) {
        super();

        if (id != null) {
            this.id = id;
            this.domElement.id = id;
        }

        if (typeof optionList != "undefined" && optionList) {
            for (var i = 0; i < optionList.length; i++) {
                this.addItem(id, optionList[i].optiontext, optionList[i].optionval);
            }
            this.setDefaultValue(defaultValue);
        }
    }

    addItem(name: string, text: string, value: string): void {
        let inputWrapper = document.createElement("div");

        this.inputElement = document.createElement("input");
        this.inputElement.type = "checkbox";
        this.inputElement.name = name;
        this.inputElement.value = value;
        this.inputElement.className = CssClassNames["custom-control-input"];
        this.inputElement.id = name.concat(Math.floor((Math.random() * 10000000000) + 1).toString());

        this.labelElement = document.createElement("label");
        this.labelElement.className = CssClassNames["custom-control-label"];
        this.labelElement.htmlFor = this.inputElement.id;
        this.labelElement.innerHTML = text;
        this.labelElement.style.fontSize = "15px";
        this.labelElement.style.cursor = "pointer";

        inputWrapper.appendChild(this.inputElement);
        inputWrapper.appendChild(this.labelElement);
        inputWrapper.className = CssClassNames["custom-control-div-checkbox"];

        this.domElement.appendChild(inputWrapper);
    }

    setDefaultValue(value?: string[]): void {
        var chkbx = this.domElement.children;
        if (typeof value != undefined && value) {
            this.defaultvalue = value;
        }

        if (typeof this.defaultvalue != undefined && this.defaultvalue) {
            for (var i = 0; i < chkbx.length; i++) {
                if (chkbx[i].firstChild.nodeName == "INPUT") {
                    for (var j = 0; j < this.defaultvalue.length; j++) {
                        if ((<HTMLInputElement>chkbx[i].firstChild).value == this.defaultvalue[j]) {
                            (<HTMLInputElement>chkbx[i].firstChild).checked = true;
                        }
                    }
                }
            }
        }
    }

    getValue(): FieldValue[] {
        var chkbx = this.domElement.children;
        let selectedVal = new Array<FieldValue>();
        for (var i = 0; i < chkbx.length; i++) {
            if (chkbx[i].nodeName == "DIV" && chkbx[i].firstChild.nodeName == "INPUT") {
                if ((<HTMLInputElement>chkbx[i].firstChild).checked) {
                    selectedVal.push(new FieldValue(
                        (<HTMLInputElement>chkbx[i].firstChild).value,
                        (<HTMLInputElement>chkbx[i].firstChild).value
                    ));
                }
            }
        }

        return selectedVal.length == 0 ? [new FieldValue("", "")] : selectedVal;
    }

    getMarkup(): HTMLDivElement {
        var chkbl = this.domElement.children;
        if (this.fieldValidations && this.fieldValidations.length > 0) {
            for (var i = 0; i < chkbl.length; i++) {
                if (chkbl[i].nodeName == "DIV" && chkbl[i].firstChild.nodeName == "INPUT") {
                    chkbl[i].firstChild.addEventListener("blur", (e: Event) => this.onChange());
                    chkbl[i].firstChild.addEventListener("change", (e: Event) => this.onChange());
                }
            }
        }

        this.domElement.insertBefore(this.addQuestionLabel(), this.domElement.childNodes[0] || null);
        this.domElement.className = CssClassNames["form-group"];
        this.domElement.appendChild(this.errorLabel);
        FormHelpers.setVisibility(this);
        return this.domElement;
    }

    getControlType(): ElementType {
        return ElementType.CheckBoxList;
    }

    validate();
    validate(type: ValidationTypes);
    validate(type?: ValidationTypes): ValidationResult {
        var result = new ValidationResult(true);
        if (this.getDisplay())
            if (!type) {
                //eval all available validations for this element
                for (var i = 0; i < this.fieldValidations.length; i++) {
                    if (this.fieldValidations[i].validationtype == ValidationTypes.Required) {
                        result = this.required(this.fieldValidations[i]);
                    }
                }
            }
            else {
                for (var i = 0; i < this.fieldValidations.length; i++)
                    if (type == ValidationTypes.Required && this.fieldValidations[i].validationtype == ValidationTypes.Required) {
                        result = this.required(this.fieldValidations[i]);

                    }
            }
        if (!result.result) {
            this.errorLabel.classList.add(CssClassNames["text-danger"]);
            this.errorLabel.innerHTML = result.errorMsg;
        } else {
            this.errorLabel.classList.add(CssClassNames["text-danger"]);
            this.errorLabel.innerHTML = "";
        }

        this.validationStatus = result.result;
        this.enclosingSection.setTitleValidationError();

        return result;
    }

    required(rule: FieldValidation): ValidationResult {
        if (rule) {
            if (typeof this.getValue() != undefined && this.getValue().length > 0 && this.getValue()[0].value != "") {

                return new ValidationResult(true);
            }
            else
                return new ValidationResult(false, rule.validationErrorMsg);
        }
        return new ValidationResult(true);
    }

    onChange() {
        this.validate(ValidationTypes.Required);
    }

    setDisplay(visibile: boolean) {
        if (!visibile) {
            if (this.getDisplay())
                this.clear();
            this.domElement.classList.add(CssClassNames["d-none"]);
        }
        else
            this.domElement.classList.remove(CssClassNames["d-none"]);
    }

    getDisplay(): boolean {
        return !this.domElement.classList.contains(CssClassNames["d-none"]);
    }

    registerEvent(event: string, callback: Function, el: SectionElement) {
        var chkbl = this.domElement.children;
        for (var i = 0; i < chkbl.length; i++) {
            if (chkbl[i].nodeName == "DIV" && chkbl[i].firstChild.nodeName == "INPUT") {
                chkbl[i].firstChild.addEventListener(event, (e: Event) => callback(el));
            }
        }
    }

    clear() {
        var chkbx = this.domElement.children;
        let selectedVal = new Array<FieldValue>();
        for (var i = 0; i < chkbx.length; i++) {
            if (chkbx[i].nodeName == "DIV" && chkbx[i].firstChild.nodeName == "INPUT") {
                (<HTMLInputElement>chkbx[i].firstChild).checked = false;
            }
        }
    }
}