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
    var Textarea = /** @class */ (function (_super) {
        __extends(Textarea, _super);
        function Textarea(id, defaultValue) {
            var _this = _super.call(this) || this;
            _this.wrapperElement = document.createElement("div");
            _this.domElement = document.createElement("textarea");
            _this.errorLabel = document.createElement("label");
            _this.fieldValidations = new Array();
            if (typeof id != undefined && id) {
                _this.id = id;
                _this.domElement.id = id;
            }
            if (typeof defaultValue != undefined && defaultValue) {
                _this.defaultvalue = defaultValue;
                _this.setDefaultValue(defaultValue);
            }
            _this.domElement.rows = 4;
            _this.domElement.cols = 50;
            // this.domElement.className = CssClassNames["form-control"];
            _this.wrapperElement.appendChild(_this.domElement);
            return _this;
        }
        Textarea.prototype.setDefaultValue = function (value) {
            if (typeof value != undefined && value) {
                this.defaultvalue = value;
            }
            if (typeof this.defaultvalue != undefined && this.defaultvalue) {
                this.domElement.value = this.defaultvalue;
            }
        };
        Textarea.prototype.getMarkup = function () {
            var _this = this;
            if (this.fieldValidations && this.fieldValidations.length > 0) {
                this.domElement.addEventListener("blur", function (e) { return _this.onFocusChange(); });
                this.domElement.addEventListener("input", function (e) { return _this.onCharChange(); });
            }
            this.wrapperElement.appendChild(this.addQuestionLabel());
            this.wrapperElement.appendChild(this.domElement);
            this.wrapperElement.appendChild(this.errorLabel);
            this.wrapperElement.className = CssClassNames_1.CssClassNames["form-group"];
            this.domElement.className = CssClassNames_1.CssClassNames["form-control"];
            formmodel_1.FormHelpers.setVisibility(this);
            return this.wrapperElement;
        };
        Textarea.prototype.getValue = function () {
            return [new FieldValue_1.FieldValue(this.domElement.value, this.domElement.value)];
        };
        Textarea.prototype.getControlType = function () {
            return ElementType_1.ElementType.TextArea;
        };
        Textarea.prototype.validate = function (type) {
            var result = new ValidationResult_1.ValidationResult(true);
            if (this.getDisplay())
                if (!type) {
                    //eval all available validations for this element
                    for (var i = 0; i < this.fieldValidations.length; i++)
                        if (this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.Required) {
                            result.concat(this.required(this.fieldValidations[i]));
                        }
                        else if (this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.MinLength) {
                            result.concat(this.minLength(this.fieldValidations[i]));
                        }
                        else if (this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.Regex) {
                            result.concat(this.regex(this.fieldValidations[i]));
                        }
                        else if (this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.MaxLength) {
                            result.concat(this.maxLength(this.fieldValidations[i]));
                        }
                }
                else {
                    for (var i = 0; i < this.fieldValidations.length; i++)
                        if (type == ValidationTypes_1.ValidationTypes.Required && this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.Required) {
                            result.concat(this.required(this.fieldValidations[i]));
                        }
                        else if (type == ValidationTypes_1.ValidationTypes.MinLength && this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.MinLength) {
                            result.concat(this.minLength(this.fieldValidations[i]));
                        }
                        else if (type == ValidationTypes_1.ValidationTypes.Regex && this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.Regex) {
                            result.concat(this.regex(this.fieldValidations[i]));
                        }
                        else if (type == ValidationTypes_1.ValidationTypes.MaxLength && this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.MaxLength) {
                            result.concat(this.maxLength(this.fieldValidations[i]));
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
        Textarea.prototype.required = function (rule) {
            // var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.Required);
            if (rule) {
                if (this.getValue() && this.getValue()[0].text != null && this.getValue()[0].text.trim() != "")
                    return new ValidationResult_1.ValidationResult(true);
                else
                    return new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
            }
            return new ValidationResult_1.ValidationResult(true);
        };
        Textarea.prototype.minLength = function (rule) {
            //  var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.MinLength);
            if (rule) {
                if (this.getValue() && this.getValue()[0].text.replace(/\n/g, "").length >= rule.minLength)
                    return new ValidationResult_1.ValidationResult(true);
                else
                    return new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
            }
            return new ValidationResult_1.ValidationResult(true);
        };
        Textarea.prototype.maxLength = function (rule) {
            //var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.MaxLength);
            if (rule) {
                if (this.getValue() && this.getValue()[0].text.length <= rule.maxLength)
                    return new ValidationResult_1.ValidationResult(true);
                else
                    return new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
            }
            return new ValidationResult_1.ValidationResult(true);
        };
        Textarea.prototype.regex = function (rule) {
            //var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.Regex);
            if (rule) {
                var rExp = new RegExp(rule.fieldValidationPattern);
                if (this.getValue() && (this.getValue()[0].text == "" || rExp.test(this.getValue()[0].text.trim())))
                    return new ValidationResult_1.ValidationResult(true);
                else
                    return new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
            }
            return new ValidationResult_1.ValidationResult(true);
        };
        //events
        Textarea.prototype.onFocusChange = function () {
            //Required Validation on change
            this.validate();
        };
        Textarea.prototype.onCharChange = function () {
            this.validate(ValidationTypes_1.ValidationTypes.MinLength).result &&
                this.validate(ValidationTypes_1.ValidationTypes.MaxLength).result;
        };
        Textarea.prototype.setDisplay = function (visibile) {
            if (!visibile) {
                if (this.getDisplay())
                    this.clear();
                this.wrapperElement.classList.add(CssClassNames_1.CssClassNames["d-none"]);
            }
            else
                this.wrapperElement.classList.remove(CssClassNames_1.CssClassNames["d-none"]);
        };
        Textarea.prototype.registerEvent = function (event, callback, el) {
            this.domElement.addEventListener(event, function (e) { return callback(el); });
        };
        Textarea.prototype.getDisplay = function () {
            return !this.wrapperElement.classList.contains(CssClassNames_1.CssClassNames["d-none"]);
        };
        Textarea.prototype.clear = function () {
            this.domElement.value = "";
        };
        return Textarea;
    }(SectionElement_1.SectionElement));
    exports.Textarea = Textarea;
});
//# sourceMappingURL=TextArea.js.map