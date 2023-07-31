//BEGIN LICENSE BLOCK 
//Interneuron Synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

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
import { ElementType } from "../enumeration/element-type";
import { SectionElement } from "./section-element";
import { OptionList } from "../model/option-list";
import { CssClassNames } from "../enumeration/css-class-names";
import { FieldValidation } from "../model/field-validation";
import { ValidationResult } from "../model/validation-result";
import { ValidationTypes } from "../enumeration/validation-types";
import { FieldValue } from "../model/field-value";
import { FormHelpers } from "./form-model";

export class DropDownList extends SectionElement {

    wrapperElement = document.createElement("div");
    domElement = document.createElement("select");
    optionelement: HTMLOptionElement;
    errorLabel = document.createElement("label");
    fieldValidations = new Array<FieldValidation>();

    constructor(id?: string, optionList?: OptionList[], defaultValue?: string, disabled?: boolean) {
        super();
        if (typeof id != undefined && id) {
            this.id = id;
            this.domElement.id = id;

        }
        this.addItem("Please Select", "Please Select")
        if (typeof optionList != undefined && optionList) {
            for (var i = 0; i < optionList.length; i++) {
                this.addItem(optionList[i].optiontext, optionList[i].optionval);
            }
            this.setDefaultValue(defaultValue);
        }
        if (typeof disabled != undefined && disabled != null) {
            this.disabled = disabled;
        }
    }

    addItem(text: string, value: string): void {
        this.optionelement = document.createElement("option");
        this.optionelement.value = value;
        this.optionelement.text = text;
        this.domElement.options.add(this.optionelement);
    }

    setDefaultValue(value?: string): void {
        if (typeof value != undefined && value)
            this.defaultvalue = value;

        if (typeof this.defaultvalue != undefined && this.defaultvalue) {
            for (var i = 0; i < this.domElement.length; i++) {
                if (this.domElement.options[i].value == this.defaultvalue)
                    this.domElement.options[i].selected = true;
            }
        }
    }

    getMarkup(): HTMLDivElement {
        if (this.fieldValidations && this.fieldValidations.length > 0) {
            this.domElement.addEventListener("blur", (e: Event) => this.onFocusChange());
            this.domElement.addEventListener("change", (e: Event) => this.onFocusChange());
        }
        this.wrapperElement.appendChild(this.addQuestionLabel());
        this.wrapperElement.appendChild(this.domElement);
        this.wrapperElement.appendChild(this.errorLabel);
        this.wrapperElement.className = CssClassNames["form-group"];
        this.domElement.className = CssClassNames["form-control"];
        FormHelpers.setVisibility(this);
        this.domElement.disabled = this.disabled;
        return this.wrapperElement;
    }

    getValue(): FieldValue[] {
        return [new FieldValue(
            this.domElement.options[this.domElement.selectedIndex].text,
            this.domElement.value
        )];
    }

    getControlType(): ElementType {
        return ElementType.DropDownList;
    }

    validate(type?: ValidationTypes): ValidationResult {
        var result = new ValidationResult(true);
        if (this.getDisplay())
            if (!type) {
                //eval all available validations for this element
                for (var i = 0; i < this.fieldValidations.length; i++)
                    if (this.fieldValidations[i].validationtype == ValidationTypes.Required) {
                        result.concat(this.required(this.fieldValidations[i]));
                    }
            }
            else {
                for (var i = 0; i < this.fieldValidations.length; i++)
                    if (type == ValidationTypes.Required && this.fieldValidations[i].validationtype == ValidationTypes.Required) {
                        result.concat(this.required(this.fieldValidations[i]));
                    }
            }
        if (!result.result) {
            this.errorLabel.classList.add(CssClassNames["text-danger"]);
            this.errorLabel.innerHTML = result.errorMsg;

        } else {
            this.errorLabel.classList.add(CssClassNames["text-danger"]);
            this.errorLabel.innerHTML = "";
        }
        this.validationStatus = result.result
        this.enclosingSection.setTitleValidationError();
        return result;
    }

    //validators
    required(rule: FieldValidation): ValidationResult {
        // var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.Required);
        if (rule) {
            if (this.getValue() && this.getValue()[0].text.trim().toLowerCase() != "please select")
                return new ValidationResult(true);
            else
                return new ValidationResult(false, rule.validationErrorMsg);
        }
        return new ValidationResult(true);
    }

    //events
    onFocusChange() {
        //Required Validation on change
        this.validate();
    }

    setDisplay(visibile: boolean) {
        if (!visibile) {
            if (this.getDisplay())
                this.clear();
            this.wrapperElement.classList.add(CssClassNames["d-none"]);
        }
        else
            this.wrapperElement.classList.remove(CssClassNames["d-none"]);
    }

    registerEvent(event: string, callback: Function, el: SectionElement) {
        this.domElement.addEventListener(event, (e: Event) => callback(el));
    }

    getDisplay(): boolean {
        return !this.wrapperElement.classList.contains(CssClassNames["d-none"]);
    }
    clear() {
        this.domElement.selectedIndex = 0;
    }
}
