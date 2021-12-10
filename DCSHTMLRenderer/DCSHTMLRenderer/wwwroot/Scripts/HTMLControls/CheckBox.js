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
define(["require", "exports", "../Enumerations/ElementType", "./SectionElement", "../Enumerations/CssClassNames", "../Enumerations/ValidationTypes", "../JSONMap/ValidationResult", "../JSONMap/FieldValue", "../formmodel"], function (require, exports, ElementType_1, SectionElement_1, CssClassNames_1, ValidationTypes_1, ValidationResult_1, FieldValue_1, formmodel_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Checkbox = /** @class */ (function (_super) {
        __extends(Checkbox, _super);
        function Checkbox(id, optionList, defaultValue) {
            var _this = _super.call(this) || this;
            _this.wrapperElement = document.createElement("div");
            _this.domElement = document.createElement("div");
            _this.errorLabel = document.createElement("label");
            _this.fieldValidations = new Array();
            if (id != null) {
                _this.id = id;
                _this.domElement.id = id;
            }
            if (typeof optionList != "undefined" && optionList) {
                for (var i = 0; i < optionList.length; i++) {
                    _this.addItem(id, optionList[i].optiontext, optionList[i].optionval);
                }
                _this.setDefaultValue(defaultValue);
            }
            return _this;
        }
        Checkbox.prototype.addItem = function (name, text, value) {
            var inputWrapper = document.createElement("div");
            this.inputElement = document.createElement("input");
            this.inputElement.type = "checkbox";
            this.inputElement.name = name;
            this.inputElement.value = value;
            this.inputElement.className = CssClassNames_1.CssClassNames["custom-control-input"];
            this.inputElement.id = name.concat(Math.floor((Math.random() * 10000000000) + 1).toString());
            this.labelElement = document.createElement("label");
            this.labelElement.className = CssClassNames_1.CssClassNames["custom-control-label"];
            this.labelElement.htmlFor = this.inputElement.id;
            this.labelElement.innerHTML = text;
            this.labelElement.style.fontSize = "15px";
            this.labelElement.style.cursor = "pointer";
            inputWrapper.appendChild(this.inputElement);
            inputWrapper.appendChild(this.labelElement);
            inputWrapper.className = CssClassNames_1.CssClassNames["custom-control-div-checkbox"];
            this.domElement.appendChild(inputWrapper);
        };
        Checkbox.prototype.setDefaultValue = function (value) {
            var chkbx = this.domElement.children;
            if (typeof value != undefined && value) {
                this.defaultvalue = value;
            }
            if (typeof this.defaultvalue != undefined && this.defaultvalue) {
                for (var i = 0; i < chkbx.length; i++) {
                    if (chkbx[i].firstChild.nodeName == "INPUT") {
                        for (var j = 0; j < this.defaultvalue.length; j++) {
                            if (chkbx[i].firstChild.value == this.defaultvalue[j]) {
                                chkbx[i].firstChild.checked = true;
                            }
                        }
                    }
                }
            }
        };
        Checkbox.prototype.getValue = function () {
            var chkbx = this.domElement.children;
            var selectedVal = new Array();
            for (var i = 0; i < chkbx.length; i++) {
                if (chkbx[i].nodeName == "DIV" && chkbx[i].firstChild.nodeName == "INPUT") {
                    if (chkbx[i].firstChild.checked) {
                        selectedVal.push(new FieldValue_1.FieldValue(chkbx[i].firstChild.value, chkbx[i].firstChild.value));
                    }
                }
            }
            return selectedVal.length == 0 ? [new FieldValue_1.FieldValue("", "")] : selectedVal;
        };
        Checkbox.prototype.getMarkup = function () {
            var _this = this;
            var chkbl = this.domElement.children;
            if (this.fieldValidations && this.fieldValidations.length > 0) {
                for (var i = 0; i < chkbl.length; i++) {
                    if (chkbl[i].nodeName == "DIV" && chkbl[i].firstChild.nodeName == "INPUT") {
                        chkbl[i].firstChild.addEventListener("blur", function (e) { return _this.onChange(); });
                        chkbl[i].firstChild.addEventListener("change", function (e) { return _this.onChange(); });
                    }
                }
            }
            this.domElement.insertBefore(this.addQuestionLabel(), this.domElement.childNodes[0] || null);
            this.domElement.className = CssClassNames_1.CssClassNames["form-group"];
            this.domElement.appendChild(this.errorLabel);
            formmodel_1.FormHelpers.setVisibility(this);
            return this.domElement;
        };
        Checkbox.prototype.getControlType = function () {
            return ElementType_1.ElementType.CheckBoxList;
        };
        Checkbox.prototype.validate = function (type) {
            var result = new ValidationResult_1.ValidationResult(true);
            if (this.getDisplay())
                if (!type) {
                    //eval all available validations for this element
                    for (var i = 0; i < this.fieldValidations.length; i++) {
                        if (this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.Required) {
                            result = this.required(this.fieldValidations[i]);
                        }
                    }
                }
                else {
                    for (var i = 0; i < this.fieldValidations.length; i++)
                        if (type == ValidationTypes_1.ValidationTypes.Required && this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.Required) {
                            result = this.required(this.fieldValidations[i]);
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
        Checkbox.prototype.required = function (rule) {
            if (rule) {
                if (typeof this.getValue() != undefined && this.getValue().length > 0 && this.getValue()[0].value != "") {
                    return new ValidationResult_1.ValidationResult(true);
                }
                else
                    return new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
            }
            return new ValidationResult_1.ValidationResult(true);
        };
        Checkbox.prototype.onChange = function () {
            this.validate(ValidationTypes_1.ValidationTypes.Required);
        };
        Checkbox.prototype.setDisplay = function (visibile) {
            if (!visibile) {
                if (this.getDisplay())
                    this.clear();
                this.domElement.classList.add(CssClassNames_1.CssClassNames["d-none"]);
            }
            else
                this.domElement.classList.remove(CssClassNames_1.CssClassNames["d-none"]);
        };
        Checkbox.prototype.getDisplay = function () {
            return !this.domElement.classList.contains(CssClassNames_1.CssClassNames["d-none"]);
        };
        Checkbox.prototype.registerEvent = function (event, callback, el) {
            var chkbl = this.domElement.children;
            for (var i = 0; i < chkbl.length; i++) {
                if (chkbl[i].nodeName == "DIV" && chkbl[i].firstChild.nodeName == "INPUT") {
                    chkbl[i].firstChild.addEventListener(event, function (e) { return callback(el); });
                }
            }
        };
        Checkbox.prototype.clear = function () {
            var chkbx = this.domElement.children;
            var selectedVal = new Array();
            for (var i = 0; i < chkbx.length; i++) {
                if (chkbx[i].nodeName == "DIV" && chkbx[i].firstChild.nodeName == "INPUT") {
                    chkbx[i].firstChild.checked = false;
                }
            }
        };
        return Checkbox;
    }(SectionElement_1.SectionElement));
    exports.Checkbox = Checkbox;
});
//# sourceMappingURL=CheckBox.js.map