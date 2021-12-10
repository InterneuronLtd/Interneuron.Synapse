//BEGIN LICENSE BLOCK 
//Interneuron Synapse

//Copyright(C) 2021  Interneuron CIC

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
import { CssClassNames } from "../enumeration/css-class-names";
import { ValidationResult } from "../model/validation-result";
import { FieldValidation } from "../model/field-validation";
import { Comparators } from "../enumeration/comparator";
import { ValidationTypes } from "../enumeration/validation-types";
import { FieldValue } from "../model/field-value";
import { DisplayRules } from "../model/display-rules";
import { FormHelpers } from "./form-model";

export class GenericHtmlControl extends SectionElement {

  wrapperElement = document.createElement("div");
  domElement = document.createElement("div");
  //errorLabel = document.createElement("label");
  //fieldValidations = new Array<FieldValidation>();
  displayRules = new DisplayRules();

  constructor(id?: string, defaultValue?: string, disabled?: boolean) {
    super();
    if (typeof id != undefined && id) {
      this.id = id;
      this.domElement.id = id;
    }
    if (typeof defaultValue != undefined && defaultValue) {
      this.defaultvalue = defaultValue;
      this.setDefaultValue(defaultValue);
    }
    if (typeof disabled != undefined && disabled != null) {
      this.disabled = disabled;
    }
  }

  setDefaultValue(value?: string): void {
    if (typeof value != undefined && value) {
      this.defaultvalue = value;
    }
    if (typeof this.defaultvalue != undefined && this.defaultvalue) {
      this.domElement.innerHTML = this.defaultvalue;
    }
  }

  getMarkup(): HTMLDivElement {
    this.wrapperElement.appendChild(this.addQuestionLabel());
    this.wrapperElement.appendChild(this.domElement);
    this.wrapperElement.className = CssClassNames["form-group"];
    //this.domElement.className = CssClassNames["form-control"];
    FormHelpers.setVisibility(this);
    return this.wrapperElement;
  }

  getValue(): FieldValue[] {
    return [new FieldValue(
      this.domElement.innerHTML,
      this.domElement.innerHTML
    )];
  }

  getControlType(): ElementType {
    return ElementType.GenericHTMLControl;
  }

  setDisplay(visibile: boolean) {
    if (!visibile) {
      this.wrapperElement.classList.add(CssClassNames["d-none"]);
    }
    else
      this.wrapperElement.classList.remove(CssClassNames["d-none"]);
  }

  registerEvent(event: string, callback: Function, el: SectionElement) {
    this.domElement.addEventListener(event, (e: Event) => callback(el));
  }

  validate() {
    return true;
  }
}
