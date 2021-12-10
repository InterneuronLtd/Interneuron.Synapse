var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
define(["require", "exports", "../Enumerations/ElementType", "./SectionElement", "../JSONMap/ValidationResult", "../JSONMap/FieldValue", "../Enumerations/CssClassNames", "../formmodel"], function (require, exports, ElementType_1, SectionElement_1, ValidationResult_1, FieldValue_1, CssClassNames_1, formmodel_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Label = /** @class */ (function (_super) {
        __extends(Label, _super);
        function Label(id, value) {
            var _this = _super.call(this) || this;
            _this.wrapperElement = document.createElement("div");
            _this.domElement = document.createElement("label");
            if (typeof id != undefined && id) {
                _this.id = id;
                _this.domElement.id = id;
            }
            if (typeof value != undefined && value) {
                _this.defaultvalue = value;
                _this.setDefaultValue(value);
            }
            return _this;
        }
        Label.prototype.setDefaultValue = function (value) {
            if (typeof value != undefined && value)
                this.defaultvalue = value;
            if (typeof this.defaultvalue != undefined && this.defaultvalue)
                this.domElement.innerHTML = this.defaultvalue;
        };
        Label.prototype.getMarkup = function () {
            this.wrapperElement.appendChild(this.addQuestionLabel());
            this.wrapperElement.appendChild(this.domElement);
            formmodel_1.FormHelpers.setVisibility(this);
            return this.wrapperElement;
        };
        Label.prototype.getControlType = function () {
            return ElementType_1.ElementType.Label;
        };
        Label.prototype.getValue = function () {
            return [new FieldValue_1.FieldValue("", "")];
        };
        Label.prototype.validate = function () {
            return new ValidationResult_1.ValidationResult(true);
        };
        Label.prototype.setDisplay = function (visibile) {
            if (!visibile)
                this.wrapperElement.classList.add(CssClassNames_1.CssClassNames["d-none"]);
            else
                this.wrapperElement.classList.remove(CssClassNames_1.CssClassNames["d-none"]);
        };
        Label.prototype.registerEvent = function (event, callback, el) {
            this.domElement.addEventListener(event, function (e) { return callback(el); });
        };
        return Label;
    }(SectionElement_1.SectionElement));
    exports.Label = Label;
});
//# sourceMappingURL=Label.js.map