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
﻿import { OptionListStatement } from "../JSONMap/OptionListStatement";
import { FieldValidation } from "../JSONMap/FieldValidation";
import { FieldCssStyle } from "../JSONMap/FieldCssStyle";
import { ScoringModel } from "../JSONMap/ScoringModel";
import { DisplayRules } from "../JSONMap/DisplayRules";
import { Section } from "../JSONMap/Section";
import { ValidationTypes } from "../Enumerations/ValidationTypes";
import { ElementType } from "../Enumerations/ElementType";
import { FieldValue } from "../JSONMap/FieldValue";

export abstract class SectionElement {
    id: string;
    controlTypeId: string;
    controlTypeName: string;
    labelText = "this is a placeholder";
    dataType: string;
    fieldvalue: any;
    defaultvalue: any;
    displayOrder: number;
    displayOnLoad: boolean;
    defaultvaluestatement: string;
    validationStatus: boolean;
    cssStyles: FieldCssStyle[];
    //nrar optionListMethod: string;
    optionliststatement: OptionListStatement[];
    fieldValidations: FieldValidation[];
    enclosingSection: Section;


    //nrar optionselectedflag: string;
    //nrar optiondeselectedflag: string;
    //nrar optionselectedflagsmall: string;
    //nrar  optionselectedflaglarge: string;
    scoringModel: ScoringModel[];
    displayRules: DisplayRules;
    domElement: HTMLElement;
    wrapperElement: HTMLElement;
    constructor() {
    }
    addQuestionLabel(): HTMLLabelElement {
        var label = document.createElement("label");

        if (this.fieldValidations && this.fieldValidations.filter(i => i.validationtype == ValidationTypes.Required).length > 0) {
            label.innerHTML = this.labelText + " *";
        } else {
            label.innerHTML = this.labelText;
        }

        return label;
    }

    abstract getMarkup(): HTMLElement;
    abstract getControlType(): ElementType;
    abstract getValue(): FieldValue[];
    abstract registerEvent(event: string, callback: Function, el: SectionElement);
    abstract setDisplay(visible: boolean);
}