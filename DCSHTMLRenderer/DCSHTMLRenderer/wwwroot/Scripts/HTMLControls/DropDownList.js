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
define(["require", "exports", "../Enumerations/ElementType", "./SectionElement", "../Enumerations/CssClassNames", "../JSONMap/ValidationResult", "../Enumerations/ValidationTypes", "../JSONMap/FieldValue", "../formmodel"], function (require, exports, ElementType_1, SectionElement_1, CssClassNames_1, ValidationResult_1, ValidationTypes_1, FieldValue_1, formmodel_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var DropDownList = /** @class */ (function (_super) {
        __extends(DropDownList, _super);
        function DropDownList(id, optionList, defaultValue) {
            var _this = _super.call(this) || this;
            _this.wrapperElement = document.createElement("div");
            _this.domElement = document.createElement("select");
            _this.errorLabel = document.createElement("label");
            _this.fieldValidations = new Array();
            if (typeof id != undefined && id) {
                _this.id = id;
                _this.domElement.id = id;
            }
            _this.addItem("Please Select", "Please Select");
            if (typeof optionList != undefined && optionList) {
                for (var i = 0; i < optionList.length; i++) {
                    _this.addItem(optionList[i].optiontext, optionList[i].optionval);
                }
                _this.setDefaultValue(defaultValue);
            }
            return _this;
        }
        DropDownList.prototype.addItem = function (text, value) {
            this.optionelement = document.createElement("option");
            this.optionelement.value = value;
            this.optionelement.text = text;
            this.domElement.options.add(this.optionelement);
        };
        DropDownList.prototype.setDefaultValue = function (value) {
            if (typeof value != undefined && value)
                this.defaultvalue = value;
            if (typeof this.defaultvalue != undefined && this.defaultvalue) {
                for (var i = 0; i < this.domElement.length; i++) {
                    if (this.domElement.options[i].value == this.defaultvalue)
                        this.domElement.options[i].selected = true;
                }
            }
        };
        DropDownList.prototype.getMarkup = function () {
            var _this = this;
            if (this.fieldValidations && this.fieldValidations.length > 0) {
                this.domElement.addEventListener("blur", function (e) { return _this.onFocusChange(); });
                this.domElement.addEventListener("change", function (e) { return _this.onFocusChange(); });
            }
            this.wrapperElement.appendChild(this.addQuestionLabel());
            this.wrapperElement.appendChild(this.domElement);
            this.wrapperElement.appendChild(this.errorLabel);
            this.wrapperElement.className = CssClassNames_1.CssClassNames["form-group"];
            this.domElement.className = CssClassNames_1.CssClassNames["form-control"];
            formmodel_1.FormHelpers.setVisibility(this);
            return this.wrapperElement;
        };
        DropDownList.prototype.getValue = function () {
            return [new FieldValue_1.FieldValue(this.domElement.options[this.domElement.selectedIndex].text, this.domElement.value)];
        };
        DropDownList.prototype.getControlType = function () {
            return ElementType_1.ElementType.DropDownList;
        };
        DropDownList.prototype.validate = function (type) {
            var result = new ValidationResult_1.ValidationResult(true);
            if (this.getDisplay())
                if (!type) {
                    //eval all available validations for this element
                    for (var i = 0; i < this.fieldValidations.length; i++)
                        if (this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.Required) {
                            result.concat(this.required(this.fieldValidations[i]));
                        }
                }
                else {
                    for (var i = 0; i < this.fieldValidations.length; i++)
                        if (type == ValidationTypes_1.ValidationTypes.Required && this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.Required) {
                            result.concat(this.required(this.fieldValidations[i]));
                        }
                }
            if (!result.result) {
                this.errorLabel.classList.add(CssClassNames_1.CssClassNames["text-danger"]);
                this.errorLabel.innerHTML = result.errorMsg;
            }
            else {
                this.errorLabel.classList.add(CssClassNames_1.CssClassNames["text-danger"]);
                this.errorLabel.innerHTML = "";
            }
            this.validationStatus = result.result;
            this.enclosingSection.setTitleValidationError();
            return result;
        };
        //validators
        DropDownList.prototype.required = function (rule) {
            // var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.Required);
            if (rule) {
                if (this.getValue() && this.getValue()[0].text.trim().toLowerCase() != "please select")
                    return new ValidationResult_1.ValidationResult(true);
                else
                    return new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
            }
            return new ValidationResult_1.ValidationResult(true);
        };
        //events
        DropDownList.prototype.onFocusChange = function () {
            //Required Validation on change
            this.validate();
        };
        DropDownList.prototype.setDisplay = function (visibile) {
            if (!visibile) {
                if (this.getDisplay())
                    this.clear();
                this.wrapperElement.classList.add(CssClassNames_1.CssClassNames["d-none"]);
            }
            else
                this.wrapperElement.classList.remove(CssClassNames_1.CssClassNames["d-none"]);
        };
        DropDownList.prototype.registerEvent = function (event, callback, el) {
            this.domElement.addEventListener(event, function (e) { return callback(el); });
        };
        DropDownList.prototype.getDisplay = function () {
            return !this.wrapperElement.classList.contains(CssClassNames_1.CssClassNames["d-none"]);
        };
        DropDownList.prototype.clear = function () {
            this.domElement.selectedIndex = 0;
        };
        return DropDownList;
    }(SectionElement_1.SectionElement));
    exports.DropDownList = DropDownList;
});
//# sourceMappingURL=DropDownList.js.map