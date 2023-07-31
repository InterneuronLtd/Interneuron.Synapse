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
﻿import { FormParameter } from "./FormParameter";
import { FormScores } from "./FormScores";
import { CssClassNames } from "../Enumerations/CssClassNames";
import { Section } from "./Section";
import { DestinationEntity } from "./DestinationEntity";

export class Form {
    public id: string;
    forminstance_id: string;
    name: string;
    description: string;
    activeFrom: Date;
    activeTo: Date;
    active: boolean;
    category: string;
    context: string;
    formparameters: FormParameter[];
    destinationentities: DestinationEntity[];
    scores = new Array<FormScores>();
    sections: Section[];
    domelement = document.createElement("div");
    sectionsWrapperDiv = document.createElement("div");
    formNameLabel = document.createElement("label");
    formDescLabel = document.createElement("label");
    formSubmitButton = document.createElement("button");
    formSubmitButtonWrapper = document.createElement("div");

    constructor() {
        this.sections = new Array<Section>();
        this.sectionsWrapperDiv.className = CssClassNames["col-lg-6"];
    }

    public genereateMarkup(): void {
        //sort array based on displayOrder
        this.sections.sort(function (a, b) {
            return a.displayorder - b.displayorder;
        });

        for (var i in this.sections) {
            this.sectionsWrapperDiv.appendChild(this.sections[i].getMarkup());
        }

        this.formNameLabel.innerHTML = this.name;
        this.formNameLabel.classList.add(CssClassNames.h3);
        this.formNameLabel.classList.add(CssClassNames["text-primary"]);
        this.formNameLabel.classList.add(CssClassNames["font-weight-bold"])
        this.formNameLabel.classList.add(CssClassNames["col-lg-12"]);

        this.formDescLabel.innerHTML = this.description;
        this.formDescLabel.classList.add(CssClassNames["text-primary"]);
        this.formDescLabel.classList.add(CssClassNames["font-italic"]);
        this.formDescLabel.classList.add(CssClassNames["col-lg-12"]);


        //this.formSubmitButtonWrapper.className = CssClassNames["col-lg-6"];
        this.formSubmitButtonWrapper.classList.add(CssClassNames["float-right"]);
        this.formSubmitButtonWrapper.classList.add(CssClassNames["p-md-2"]);



        this.formSubmitButton.innerText = "Submit";
        this.formSubmitButton.classList.add(CssClassNames.btn);
        this.formSubmitButton.classList.add(CssClassNames["btn-primary"]);
        this.formSubmitButton.setAttribute("onclick", "submitForm()");



        this.formSubmitButtonWrapper.appendChild(this.formSubmitButton);

        this.sectionsWrapperDiv.appendChild(this.formSubmitButtonWrapper);


        this.domelement.appendChild(this.formNameLabel);
        this.domelement.appendChild(this.formDescLabel);

        this.domelement.appendChild(this.sectionsWrapperDiv);
        // this.domelement.appendChild(this.formSubmitButtonWrapper);
    }

    public getMarkup(): HTMLDivElement {
        this.genereateMarkup();
        return this.domelement;
    }
}