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
﻿import { ElementType } from "../Enumerations/ElementType";
import { SectionElement } from "./SectionElement";
import { CssClassNames } from "../Enumerations/CssClassNames";
import { ValidationResult } from "../JSONMap/ValidationResult";
import { FieldValidation } from "../JSONMap/FieldValidation";
import { Comparators } from "../Enumerations/Comparator";
import { ValidationTypes } from "../Enumerations/ValidationTypes";
import { FieldValue } from "../JSONMap/FieldValue";
import { Form } from "../JSONMap/Form";
import { DisplayRules } from "../JSONMap/DisplayRules";
import { Checkbox } from "./CheckBox";
import { FormHelpers } from "../formmodel";

export class TextBox extends SectionElement {

    wrapperElement = document.createElement("div");
    domElement = document.createElement("input");
    errorLabel = document.createElement("label");
    fieldValidations = new Array<FieldValidation>();
    displayRules = new DisplayRules();

    constructor();
    constructor(id: string, defaultValue?: string);
    constructor(id?: string, defaultValue?: string) {
        super();
        if (typeof id != undefined && id) {
            this.id = id;
            this.domElement.id = id;
        }
        if (typeof defaultValue != undefined && defaultValue) {
            this.defaultvalue = defaultValue;
            this.setDefaultValue(defaultValue);
        }
    }

    setDefaultValue(value?: string): void {
        if (typeof value != undefined && value) {
            this.defaultvalue = value;
        }
        if (typeof this.defaultvalue != undefined && this.defaultvalue) {
            this.domElement.value = this.defaultvalue;
        }
    }

    getMarkup(): HTMLDivElement {

        if (this.fieldValidations && this.fieldValidations.length > 0) {
            this.domElement.addEventListener("blur", (e: Event) => this.onFocusChange());
            this.domElement.addEventListener("input", (e: Event) => this.onCharChange());
        }
        this.wrapperElement.appendChild(this.addQuestionLabel());
        this.wrapperElement.appendChild(this.domElement);
        this.wrapperElement.appendChild(this.errorLabel);
        this.wrapperElement.className = CssClassNames["form-group"];
        this.domElement.className = CssClassNames["form-control"];
        FormHelpers.setVisibility(this);
        return this.wrapperElement;
    }

    getValue(): FieldValue[] {
        return [new FieldValue(
            this.domElement.value,
            this.domElement.value
        )];
    }

    getControlType(): ElementType {
        return ElementType.TextBox;
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
                    else if (this.fieldValidations[i].validationtype == ValidationTypes.MinLength) {
                        result.concat(this.minLength(this.fieldValidations[i]));
                    }
                    else if (this.fieldValidations[i].validationtype == ValidationTypes.Regex) {
                        result.concat(this.regex(this.fieldValidations[i]));
                    }
                    else if (this.fieldValidations[i].validationtype == ValidationTypes.ValueCompare) {
                        result.concat(this.valueCompare(this.fieldValidations[i]));
                    }
                    else if (this.fieldValidations[i].validationtype == ValidationTypes.MaxLength) {
                        result.concat(this.maxLength(this.fieldValidations[i]));
                    }
            }
            else {
                for (var i = 0; i < this.fieldValidations.length; i++)
                    if (type == ValidationTypes.Required && this.fieldValidations[i].validationtype == ValidationTypes.Required) {

                        result.concat(this.required(this.fieldValidations[i]));
                    }
                    else if (type == ValidationTypes.MinLength && this.fieldValidations[i].validationtype == ValidationTypes.MinLength) {
                        result.concat(this.minLength(this.fieldValidations[i]));
                    }
                    else if (type == ValidationTypes.Regex && this.fieldValidations[i].validationtype == ValidationTypes.Regex) {
                        result.concat(this.regex(this.fieldValidations[i]));
                    }
                    else if (type == ValidationTypes.ValueCompare && this.fieldValidations[i].validationtype == ValidationTypes.ValueCompare) {
                        result.concat(this.valueCompare(this.fieldValidations[i]));
                    }
                    else if (type == ValidationTypes.MaxLength && this.fieldValidations[i].validationtype == ValidationTypes.MaxLength) {
                        result.concat(this.maxLength(this.fieldValidations[i]));
                    }
            }
        if (!result.result) {
            this.errorLabel.classList.add(CssClassNames["text-danger"]);
            this.errorLabel.innerHTML = result.errorMsg;
            //this.observerForValidationError.classList.add(CssClassNames["text-danger"])
        } else {
            this.errorLabel.classList.add(CssClassNames["text-danger"]);
            this.errorLabel.innerHTML = "";
            // this.observerForValidationError.classList.remove(CssClassNames["text-danger"])
        }
        this.validationStatus = result.result
        this.enclosingSection.setTitleValidationError();
        return result;
    }

    //validators
    required(rule: FieldValidation): ValidationResult {
        // var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.Required);
        if (rule) {
            if (this.getValue() && this.getValue()[0].text != null && this.getValue()[0].text.trim() != "")
                return new ValidationResult(true);
            else
                return new ValidationResult(false, rule.validationErrorMsg);
        }
        return new ValidationResult(true);
    }
    minLength(rule: FieldValidation): ValidationResult {
        //  var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.MinLength);
        if (rule) {
            if (this.getValue() && this.getValue()[0].text.length >= rule.minLength)
                return new ValidationResult(true);
            else
                return new ValidationResult(false, rule.validationErrorMsg);
        }
        return new ValidationResult(true);
    }
    maxLength(rule: FieldValidation): ValidationResult {
        //var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.MaxLength);
        if (rule) {
            if (this.getValue() && this.getValue()[0].text.length <= rule.maxLength)
                return new ValidationResult(true);
            else
                return new ValidationResult(false, rule.validationErrorMsg);
        }
        return new ValidationResult(true);
    }
    valueCompare(rule: FieldValidation): ValidationResult {
        //var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.ValueCompare);
        var result = new ValidationResult(true);
        if (rule) {
            var valueToCompare = rule.compareValue;
            if (this.getValue && this.getValue()[0].text.trim() != "") {
                if (rule.comparator == Comparators.EqualsTo) {
                    if (parseFloat(this.getValue()[0].text.trim()) != valueToCompare[0])
                        result = new ValidationResult(false, rule.validationErrorMsg);
                }
                else if (rule.comparator == Comparators.LessThan) {
                    if (!(parseFloat(this.getValue()[0].text.trim()) < valueToCompare[0]))
                        result = (new ValidationResult(false, rule.validationErrorMsg));
                }
                else if (rule.comparator == Comparators.GreaterThan) {
                    if (!(parseFloat(this.getValue()[0].text.trim()) > valueToCompare[0]))
                        result = (new ValidationResult(false, rule.validationErrorMsg));
                }
                else if (rule.comparator == Comparators.LessThanOrEqualTo) {
                    if (!(parseFloat(this.getValue()[0].text.trim()) <= valueToCompare[0]))
                        result = (new ValidationResult(false, rule.validationErrorMsg));

                }
                else if (rule.comparator == Comparators.GreaterThanOrEqualTo) {
                    if (!(parseFloat(this.getValue()[0].text.trim()) >= valueToCompare[0]))
                        result = (new ValidationResult(false, rule.validationErrorMsg));
                }
                else if (rule.comparator == Comparators.Between) {
                    if (!(valueToCompare[0] <= parseFloat(this.getValue()[0].text.trim()) && valueToCompare[1] >= parseFloat(this.getValue()[0].text.trim())))
                        result = (new ValidationResult(false, rule.validationErrorMsg));
                }
            }
        }
        return result;
    }
    regex(rule: FieldValidation): ValidationResult {
        //var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.Regex);
        if (rule) {
            let rExp = new RegExp(rule.fieldValidationPattern);
            if (this.getValue() && (this.getValue()[0].text == "" || rExp.test(this.getValue()[0].text.trim())))
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
    onCharChange() {
        this.validate(ValidationTypes.MinLength).result &&
            this.validate(ValidationTypes.MaxLength).result;

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
        this.domElement.value = "";
    }

}