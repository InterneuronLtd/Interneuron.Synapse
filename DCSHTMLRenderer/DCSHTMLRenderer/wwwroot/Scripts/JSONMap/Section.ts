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
﻿import { CssClassNames } from "../Enumerations/CssClassNames";
import { SectionElement } from "../HTMLControls/SectionElement";
import { ValidationTypes } from "../Enumerations/ValidationTypes";
import { Form } from "./Form";

export class Section {
    //Attributes to set Section properties for section objects 

    id: string;
    title: string;
    banner: string;
    desciption: string;
    displayorder: number;
    elements = new Array<SectionElement>();
    domElement = document.createElement("div");
    sectionCollapsableDiv = document.createElement("div");
    sectionHeaderDiv = document.createElement("div");
    sectionTitleDiv = document.createElement("div");
    sectionDescriptionDiv = document.createElement("div");
    sectionBannerDiv = document.createElement("div");
    lblTitle = document.createElement("label");
    enclosingForm: Form;

    constructor() {

    }

    private generateMarkup(): void {

        this.domElement.id = this.id;

        this.domElement.className = CssClassNames.divSectionWrapper;

        this.sectionCollapsableDiv.id = this.id.concat("-divCollapsable");
        this.sectionCollapsableDiv.className = CssClassNames.divCollasibleSection;

        //bootstrap collapse
        this.sectionHeaderDiv.setAttribute("data-toggle", "collapse");
        this.sectionHeaderDiv.setAttribute("data-target", "#".concat(this.sectionCollapsableDiv.id))
        this.sectionHeaderDiv.style.cursor = "pointer";

        this.sectionTitleDiv.className = CssClassNames.divSectionTitle;

        // let lblTitle = document.createElement("label");
        this.lblTitle.classList.add(CssClassNames.h5);
        // lblTitle.classList.add(CssClassNames["text-primary"]);
        // lblTitle.classList.add(CssClassNames["font-weight-light"]);

        if (this.elements.filter(e => e.fieldValidations && e.fieldValidations.filter(i => i.validationtype == ValidationTypes.Required).length > 0).length > 0) {
            this.lblTitle.innerHTML = this.title + " *";
        } else {
            this.lblTitle.innerHTML = this.title;
        }


        let lblDesc = document.createElement("label");
        lblDesc.classList.add(CssClassNames["text-muted"]);
        lblDesc.classList.add(CssClassNames["font-italic"]);
        lblDesc.innerHTML = this.desciption;

        //create banner
        this.sectionBannerDiv = document.createElement("div");
        this.sectionBannerDiv.innerHTML = this.banner;

        this.elements.sort(function (a, b) {
            return a.displayOrder - b.displayOrder;
        });

        this.sectionCollapsableDiv.appendChild(document.createElement("hr"));


        for (var i in this.elements) {
            this.elements[i].enclosingSection = this;
            this.sectionCollapsableDiv.appendChild(this.elements[i].getMarkup());
        }


        this.sectionTitleDiv.appendChild(this.lblTitle);
        this.sectionDescriptionDiv.appendChild(lblDesc);

        this.sectionHeaderDiv.appendChild(this.sectionTitleDiv);
        this.sectionHeaderDiv.appendChild(this.sectionDescriptionDiv);


        this.domElement.appendChild(this.sectionHeaderDiv);
        //  this.domElement.appendChild(this.sectionBannerDiv);
        this.domElement.appendChild(this.sectionCollapsableDiv);

        //Alternate impl
        //might be slower when things get complex
        //this.domElement.innerHTML = "<p><strong>Title</strong></p><p><em>Description </em></p><hr /><p>&nbsp;</p>";

    }
    setTitleValidationError() {
        if (this.elements.filter(e => e.validationStatus == false).length > 0) {
            this.lblTitle.classList.add(CssClassNames["text-danger"])
        } else {
            this.lblTitle.classList.remove(CssClassNames["text-danger"])
        }
    }

    getMarkup(): HTMLDivElement {
        this.generateMarkup();
        return this.domElement;
    }
}