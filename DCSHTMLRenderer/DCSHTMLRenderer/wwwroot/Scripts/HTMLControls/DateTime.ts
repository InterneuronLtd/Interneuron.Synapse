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
import { FieldValue } from "../JSONMap/FieldValue";
import { ValidationTypes } from "../Enumerations/ValidationTypes";
import { ValidationResult } from "../JSONMap/ValidationResult";
import { FieldValidation } from "../JSONMap/FieldValidation";
import { Comparators } from "../Enumerations/Comparator";
import { CompareDateValues } from "../Enumerations/CompareDateValues";
import { FormHelpers } from "../formmodel";

export class Datetime extends SectionElement {
    wrapperElement = document.createElement("div");
    domElement = document.createElement("input");
    errorLabel = document.createElement("label");
    fieldValidations = new Array<FieldValidation>();

    constructor();
    constructor(id: string, defaultValue?: string);
    constructor(id?: string, defaultValue?: string) {
        super();
        this.domElement.type = "text";
        if (typeof id != undefined && id) {
            this.id = id;
            this.domElement.id = id;
            this.domElement.name = id;
        }
        if (typeof defaultValue != undefined && defaultValue) {
            this.setDefaultValue(defaultValue);
        }
        this.domElement.classList.add("datetime");
        this.wrapperElement.appendChild(this.domElement);
    }

    setDefaultValue(value?: string): void {
        if (typeof value != undefined && value) {
            this.defaultvalue = this.getStringDate(value);
        }
        if (typeof this.defaultvalue != undefined && this.defaultvalue) {
            this.domElement.value = this.defaultvalue;
        }
    }

    getStringDate(isoDate: string): string {
        var date = new Date(isoDate);

        var day = date.getDate() < 10 ? '0' + date.getDate() : '' + date.getDate();
        var month = (date.getMonth() + 1) < 10 ? '0' + (date.getMonth() + 1) : '' + (date.getMonth() + 1);
        var hour = date.getHours() < 10 ? '0' + date.getHours() : '' + date.getHours();
        var minutes = date.getMinutes() < 10 ? '0' + date.getMinutes() : '' + date.getMinutes();

        return day + '/' + month + '/' + date.getFullYear() + ' ' + hour + ':' + minutes;
    }

    getValue(): FieldValue[] {
        if (typeof this.domElement.value != undefined && this.domElement.value) {
            var dateparts = this.domElement.value.split(' ');
            const [day, month, year] = dateparts[0].split("/");
            const [hour, minute] = dateparts[1].split(":");
            //var inputDate = new Date(parseInt(year), parseInt(month) - 1, parseInt(day), parseInt(hour), parseInt(minute));

            return [new FieldValue(
                `${year}-${month}-${day}T${hour}:${minute}:00`,
                `${year}-${month}-${day}T${hour}:${minute}:00`
            )];
        }

        return [new FieldValue("", "")];
    }

    getMarkup(): HTMLDivElement {
        // *** Comment Start *** 
        //This line is required for rendering calendar div. Should not be required once proper bootstrap css is applied to the wrapper div
        this.wrapperElement.style.position = "relative";
        // *** Comment End ***

        if (this.fieldValidations && this.fieldValidations.length > 0) {
            this.domElement.addEventListener("blur", (e: Event) => this.onFocusChange());
            this.domElement.addEventListener("input", (e: Event) => this.onCharChange());
        }

        this.wrapperElement.appendChild(this.addQuestionLabel());
        this.wrapperElement.appendChild(this.domElement);
        this.wrapperElement.appendChild(this.errorLabel);

        this.wrapperElement.className = CssClassNames["form-group"];
        this.domElement.classList.add(CssClassNames["form-control"]);
        FormHelpers.setVisibility(this);
        return this.wrapperElement;
    }

    getControlType(): ElementType {
        return ElementType.DateTime;
    }

    compareValue(comparator: Comparators, dateComponentLow: CompareDateValues, compareValueLow: string, dateComponentHigh: CompareDateValues, compareValueHigh: string): boolean {
        var isSuccessful = true;

        var inputValue = this.getValue()[0].text;
        var now = new Date();

        if (inputValue && inputValue != "") {
            var inputMilliseconds = new Date(inputValue).getTime();
            var compareMillisecondsLow: number;
            var compareMillisecondsHigh: number;

            if (dateComponentLow == CompareDateValues.SECONDS) {
                compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds() + parseInt(compareValueLow)).getTime();
            }
            else if (dateComponentLow == CompareDateValues.MINUTES) {
                compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes() + parseInt(compareValueLow)).getTime();
            }
            else if (dateComponentLow == CompareDateValues.HOURS) {
                compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours() + parseInt(compareValueLow)).getTime();
            }
            else if (dateComponentLow == CompareDateValues.DAYS) {
                compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth(), now.getDate() + parseInt(compareValueLow)).getTime();
            }
            else if (dateComponentLow == CompareDateValues.WEEKS) {
                compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth(), now.getDate() + (parseInt(compareValueLow) * 7)).getTime();
            }
            else if (dateComponentLow == CompareDateValues.MONTHS) {
                compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth() + parseInt(compareValueLow), now.getDate()).getTime();
            }
            else if (dateComponentLow == CompareDateValues.YEARS) {
                compareMillisecondsLow = new Date(now.getFullYear() + parseInt(compareValueLow), now.getMonth(), now.getDate()).getTime();
            }
            else {
                compareMillisecondsLow = new Date(dateComponentLow).getTime();
            }

            if (dateComponentHigh != null && dateComponentHigh != undefined) {
                if (dateComponentHigh == CompareDateValues.SECONDS) {
                    compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds() + parseInt(compareValueHigh)).getTime();
                }
                else if (dateComponentHigh == CompareDateValues.MINUTES) {
                    compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes() + parseInt(compareValueHigh)).getTime();
                }
                else if (dateComponentHigh == CompareDateValues.HOURS) {
                    compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours() + parseInt(compareValueHigh)).getTime();
                }
                else if (dateComponentHigh == CompareDateValues.DAYS) {
                    compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth(), now.getDate() + parseInt(compareValueHigh)).getTime();
                }
                else if (dateComponentHigh == CompareDateValues.WEEKS) {
                    compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth(), now.getDate() + (parseInt(compareValueHigh) * 7)).getTime();
                }
                else if (dateComponentHigh == CompareDateValues.MONTHS) {
                    compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth() + parseInt(compareValueHigh), now.getDate()).getTime();
                }
                else if (dateComponentHigh == CompareDateValues.YEARS) {
                    compareMillisecondsHigh = new Date(now.getFullYear() + parseInt(compareValueHigh), now.getMonth(), now.getDate()).getTime();
                }
                else {
                    compareMillisecondsHigh = new Date(dateComponentHigh).getTime();
                }
            }

            if (comparator == Comparators.EqualsTo) {
                // input should be equal to the value to compare
                if (!(inputMilliseconds == compareMillisecondsLow))
                    isSuccessful = false;
            }
            else if (comparator == Comparators.NotEqualTo) {
                // input should not be equal to the value to compare
                if (!(inputMilliseconds != compareMillisecondsLow))
                    isSuccessful = false;
            }
            else if (comparator == Comparators.LessThan) {
                // input should be less than the value to compare
                if (!(inputMilliseconds < compareMillisecondsLow))
                    isSuccessful = false;
            }
            else if (comparator == Comparators.LessThanOrEqualTo) {
                // input should be less than or equal to the value to compare
                if (!(inputMilliseconds <= compareMillisecondsLow))
                    isSuccessful = false;
            }
            else if (comparator == Comparators.GreaterThan) {
                // input should be greater than the value to compare
                if (!(inputMilliseconds > compareMillisecondsLow))
                    isSuccessful = false;
            }
            else if (comparator == Comparators.GreaterThanOrEqualTo) {
                // input should be greater than or equal to the value to compare
                if (!(inputMilliseconds >= compareMillisecondsLow))
                    isSuccessful = false;
            }
            else if (comparator == Comparators.Between) {
                // input should be between the low and high value (inclusive)
                if (!(inputMilliseconds >= compareMillisecondsLow && inputMilliseconds <= compareMillisecondsHigh))
                    isSuccessful = false;
            }
        }

        return isSuccessful;
    }

    validate(type?: ValidationTypes): ValidationResult {
        var result = new ValidationResult(true);
        if (this.getDisplay())
            if (!type) {
                //eval all available validations for this element
                for (var i = 0; i < this.fieldValidations.length; i++) {
                    if (this.fieldValidations[i].validationtype == ValidationTypes.Required) {
                        result.concat(this.required(this.fieldValidations[i]));
                    }
                    else if (this.fieldValidations[i].validationtype == ValidationTypes.ValueCompare) {
                        result.concat(this.valueCompare(this.fieldValidations[i]));
                    }
                }
            }
            else {
                for (var i = 0; i < this.fieldValidations.length; i++) {
                    if (type == ValidationTypes.Required && this.fieldValidations[i].validationtype == ValidationTypes.Required) {

                        result.concat(this.required(this.fieldValidations[i]));
                    }
                    else if (type == ValidationTypes.ValueCompare && this.fieldValidations[i].validationtype == ValidationTypes.ValueCompare) {
                        result.concat(this.valueCompare(this.fieldValidations[i]));
                    }
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

    getDisplay(): boolean {
        return !this.wrapperElement.classList.contains(CssClassNames["d-none"]);
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

    valueCompare(rule: FieldValidation): ValidationResult {
        var isValid = true;

        if (rule) {
            var inputValue = this.getValue()[0].text.trim().substring(0, 19);

            if (inputValue && inputValue != "") {
                var compareValueLow = rule.compareValue.length != 0 ? "" + rule.compareValue[0] : null;
                var compareValueHigh = rule.compareValue.length == 2 ? "" + rule.compareValue[1] : null;

                isValid = this.compareValue(rule.comparator, rule.compareDateValue[0], compareValueLow, rule.compareDateValue[1], compareValueHigh);
            }
        }

        return isValid ? (new ValidationResult(isValid)) : (new ValidationResult(isValid, rule.validationErrorMsg));
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
    clear() {

        this.domElement.value = "";
    }
}