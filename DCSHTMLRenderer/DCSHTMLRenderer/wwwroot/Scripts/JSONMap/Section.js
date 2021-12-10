define(["require", "exports", "../Enumerations/CssClassNames", "../Enumerations/ValidationTypes"], function (require, exports, CssClassNames_1, ValidationTypes_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Section = /** @class */ (function () {
        function Section() {
            this.elements = new Array();
            this.domElement = document.createElement("div");
            this.sectionCollapsableDiv = document.createElement("div");
            this.sectionHeaderDiv = document.createElement("div");
            this.sectionTitleDiv = document.createElement("div");
            this.sectionDescriptionDiv = document.createElement("div");
            this.sectionBannerDiv = document.createElement("div");
            this.lblTitle = document.createElement("label");
        }
        Section.prototype.generateMarkup = function () {
            this.domElement.id = this.id;
            this.domElement.className = CssClassNames_1.CssClassNames.divSectionWrapper;
            this.sectionCollapsableDiv.id = this.id.concat("-divCollapsable");
            this.sectionCollapsableDiv.className = CssClassNames_1.CssClassNames.divCollasibleSection;
            //bootstrap collapse
            this.sectionHeaderDiv.setAttribute("data-toggle", "collapse");
            this.sectionHeaderDiv.setAttribute("data-target", "#".concat(this.sectionCollapsableDiv.id));
            this.sectionHeaderDiv.style.cursor = "pointer";
            this.sectionTitleDiv.className = CssClassNames_1.CssClassNames.divSectionTitle;
            // let lblTitle = document.createElement("label");
            this.lblTitle.classList.add(CssClassNames_1.CssClassNames.h5);
            // lblTitle.classList.add(CssClassNames["text-primary"]);
            // lblTitle.classList.add(CssClassNames["font-weight-light"]);
            if (this.elements.filter(function (e) { return e.fieldValidations && e.fieldValidations.filter(function (i) { return i.validationtype == ValidationTypes_1.ValidationTypes.Required; }).length > 0; }).length > 0) {
                this.lblTitle.innerHTML = this.title + " *";
            }
            else {
                this.lblTitle.innerHTML = this.title;
            }
            var lblDesc = document.createElement("label");
            lblDesc.classList.add(CssClassNames_1.CssClassNames["text-muted"]);
            lblDesc.classList.add(CssClassNames_1.CssClassNames["font-italic"]);
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
        };
        Section.prototype.setTitleValidationError = function () {
            if (this.elements.filter(function (e) { return e.validationStatus == false; }).length > 0) {
                this.lblTitle.classList.add(CssClassNames_1.CssClassNames["text-danger"]);
            }
            else {
                this.lblTitle.classList.remove(CssClassNames_1.CssClassNames["text-danger"]);
            }
        };
        Section.prototype.getMarkup = function () {
            this.generateMarkup();
            return this.domElement;
        };
        return Section;
    }());
    exports.Section = Section;
});
//# sourceMappingURL=Section.js.map