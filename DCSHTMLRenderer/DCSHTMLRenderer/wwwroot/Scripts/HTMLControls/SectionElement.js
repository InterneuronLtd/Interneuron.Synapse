define(["require", "exports", "../Enumerations/ValidationTypes"], function (require, exports, ValidationTypes_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var SectionElement = /** @class */ (function () {
        function SectionElement() {
            this.labelText = "this is a placeholder";
        }
        SectionElement.prototype.addQuestionLabel = function () {
            var label = document.createElement("label");
            if (this.fieldValidations && this.fieldValidations.filter(function (i) { return i.validationtype == ValidationTypes_1.ValidationTypes.Required; }).length > 0) {
                label.innerHTML = this.labelText + " *";
            }
            else {
                label.innerHTML = this.labelText;
            }
            return label;
        };
        return SectionElement;
    }());
    exports.SectionElement = SectionElement;
});
//# sourceMappingURL=SectionElement.js.map