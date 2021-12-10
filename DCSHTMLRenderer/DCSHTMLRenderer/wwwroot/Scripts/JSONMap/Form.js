define(["require", "exports", "../Enumerations/CssClassNames"], function (require, exports, CssClassNames_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Form = /** @class */ (function () {
        function Form() {
            this.scores = new Array();
            this.domelement = document.createElement("div");
            this.sectionsWrapperDiv = document.createElement("div");
            this.formNameLabel = document.createElement("label");
            this.formDescLabel = document.createElement("label");
            this.formSubmitButton = document.createElement("button");
            this.formSubmitButtonWrapper = document.createElement("div");
            this.sections = new Array();
            this.sectionsWrapperDiv.className = CssClassNames_1.CssClassNames["col-lg-6"];
        }
        Form.prototype.genereateMarkup = function () {
            //sort array based on displayOrder
            this.sections.sort(function (a, b) {
                return a.displayorder - b.displayorder;
            });
            for (var i in this.sections) {
                this.sectionsWrapperDiv.appendChild(this.sections[i].getMarkup());
            }
            this.formNameLabel.innerHTML = this.name;
            this.formNameLabel.classList.add(CssClassNames_1.CssClassNames.h3);
            this.formNameLabel.classList.add(CssClassNames_1.CssClassNames["text-primary"]);
            this.formNameLabel.classList.add(CssClassNames_1.CssClassNames["font-weight-bold"]);
            this.formNameLabel.classList.add(CssClassNames_1.CssClassNames["col-lg-12"]);
            this.formDescLabel.innerHTML = this.description;
            this.formDescLabel.classList.add(CssClassNames_1.CssClassNames["text-primary"]);
            this.formDescLabel.classList.add(CssClassNames_1.CssClassNames["font-italic"]);
            this.formDescLabel.classList.add(CssClassNames_1.CssClassNames["col-lg-12"]);
            //this.formSubmitButtonWrapper.className = CssClassNames["col-lg-6"];
            this.formSubmitButtonWrapper.classList.add(CssClassNames_1.CssClassNames["float-right"]);
            this.formSubmitButtonWrapper.classList.add(CssClassNames_1.CssClassNames["p-md-2"]);
            this.formSubmitButton.innerText = "Submit";
            this.formSubmitButton.classList.add(CssClassNames_1.CssClassNames.btn);
            this.formSubmitButton.classList.add(CssClassNames_1.CssClassNames["btn-primary"]);
            this.formSubmitButton.setAttribute("onclick", "submitForm()");
            this.formSubmitButtonWrapper.appendChild(this.formSubmitButton);
            this.sectionsWrapperDiv.appendChild(this.formSubmitButtonWrapper);
            this.domelement.appendChild(this.formNameLabel);
            this.domelement.appendChild(this.formDescLabel);
            this.domelement.appendChild(this.sectionsWrapperDiv);
            // this.domelement.appendChild(this.formSubmitButtonWrapper);
        };
        Form.prototype.getMarkup = function () {
            this.genereateMarkup();
            return this.domelement;
        };
        return Form;
    }());
    exports.Form = Form;
});
//# sourceMappingURL=Form.js.map