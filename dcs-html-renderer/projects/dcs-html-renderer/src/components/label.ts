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
import { ValidationResult } from "../model/validation-result";
import { FieldValue } from "../model/field-value";
import { CssClassNames } from "../enumeration/css-class-names";
import { FormHelpers } from "./form-model";

export class Label extends SectionElement {
    wrapperElement = document.createElement("div");
    domElement = document.createElement("label");

    constructor();
    constructor(id: string, value?: string);
    constructor(id?: string, value?: string) {
        super();
        if (typeof id != undefined && id) {
            this.id = id;
            this.domElement.id = id;
        }
        if (typeof value != undefined && value) {
            this.defaultvalue = value;
            this.setDefaultValue(value);
        }
    }

    setDefaultValue(value?: string): void {
        if (typeof value != undefined && value)
            this.defaultvalue = value;
        if (typeof this.defaultvalue != undefined && this.defaultvalue)
            this.domElement.innerHTML = this.defaultvalue;
    }

    getMarkup(): HTMLDivElement {
        this.wrapperElement.appendChild(this.addQuestionLabel());
        this.wrapperElement.appendChild(this.domElement);
        FormHelpers.setVisibility(this);
        //this.domElement.disabled = this.disabled;
        return this.wrapperElement;
    }

    getControlType(): ElementType {
        return ElementType.Label;
    }

    getValue(): FieldValue[] {
        return [new FieldValue("", "")];
    }

    validate(): ValidationResult {
        return new ValidationResult(true);
    }

    setDisplay(visibile: boolean) {
        if (!visibile)
            this.wrapperElement.classList.add(CssClassNames["d-none"]);
        else
            this.wrapperElement.classList.remove(CssClassNames["d-none"]);
    }

    registerEvent(event: string, callback: Function, el: SectionElement) {
        this.domElement.addEventListener(event, (e: Event) => callback(el));
    }

}
