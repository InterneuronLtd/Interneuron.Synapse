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
define(["require", "exports", "../Enumerations/ElementType", "./SectionElement", "../Enumerations/CssClassNames", "../JSONMap/ValidationResult", "../Enumerations/Comparator", "../Enumerations/ValidationTypes", "../JSONMap/FieldValue", "../JSONMap/DisplayRules", "../formmodel"], function (require, exports, ElementType_1, SectionElement_1, CssClassNames_1, ValidationResult_1, Comparator_1, ValidationTypes_1, FieldValue_1, DisplayRules_1, formmodel_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var TextBox = /** @class */ (function (_super) {
        __extends(TextBox, _super);
        function TextBox(id, defaultValue) {
            var _this = _super.call(this) || this;
            _this.wrapperElement = document.createElement("div");
            _this.domElement = document.createElement("input");
            _this.errorLabel = document.createElement("label");
            _this.fieldValidations = new Array();
            _this.displayRules = new DisplayRules_1.DisplayRules();
            if (typeof id != undefined && id) {
                _this.id = id;
                _this.domElement.id = id;
            }
            if (typeof defaultValue != undefined && defaultValue) {
                _this.defaultvalue = defaultValue;
                _this.setDefaultValue(defaultValue);
            }
            return _this;
        }
        TextBox.prototype.setDefaultValue = function (value) {
            if (typeof value != undefined && value) {
                this.defaultvalue = value;
            }
            if (typeof this.defaultvalue != undefined && this.defaultvalue) {
                this.domElement.value = this.defaultvalue;
            }
        };
        TextBox.prototype.getMarkup = function () {
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
        TextBox.prototype.getValue = function () {
            return [new FieldValue_1.FieldValue(this.domElement.value, this.domElement.value)];
        };
        TextBox.prototype.getControlType = function () {
            return ElementType_1.ElementType.TextBox;
        };
        TextBox.prototype.validate = function (type) {
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
                        else if (this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.ValueCompare) {
                            result.concat(this.valueCompare(this.fieldValidations[i]));
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
                        else if (type == ValidationTypes_1.ValidationTypes.ValueCompare && this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.ValueCompare) {
                            result.concat(this.valueCompare(this.fieldValidations[i]));
                        }
                        else if (type == ValidationTypes_1.ValidationTypes.MaxLength && this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.MaxLength) {
                            result.concat(this.maxLength(this.fieldValidations[i]));
                        }
                }
            if (!result.result) {
                this.errorLabel.classList.add(CssClassNames_1.CssClassNames["text-danger"]);
                this.errorLabel.innerHTML = result.errorMsg;
                //this.observerForValidationError.classList.add(CssClassNames["text-danger"])
            }
            else {
                this.errorLabel.classList.add(CssClassNames_1.CssClassNames["text-danger"]);
                this.errorLabel.innerHTML = "";
                // this.observerForValidationError.classList.remove(CssClassNames["text-danger"])
            }
            this.validationStatus = result.result;
            this.enclosingSection.setTitleValidationError();
            return result;
        };
        //validators
        TextBox.prototype.required = function (rule) {
            // var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.Required);
            if (rule) {
                if (this.getValue() && this.getValue()[0].text != null && this.getValue()[0].text.trim() != "")
                    return new ValidationResult_1.ValidationResult(true);
                else
                    return new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
            }
            return new ValidationResult_1.ValidationResult(true);
        };
        TextBox.prototype.minLength = function (rule) {
            //  var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.MinLength);
            if (rule) {
                if (this.getValue() && this.getValue()[0].text.length >= rule.minLength)
                    return new ValidationResult_1.ValidationResult(true);
                else
                    return new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
            }
            return new ValidationResult_1.ValidationResult(true);
        };
        TextBox.prototype.maxLength = function (rule) {
            //var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.MaxLength);
            if (rule) {
                if (this.getValue() && this.getValue()[0].text.length <= rule.maxLength)
                    return new ValidationResult_1.ValidationResult(true);
                else
                    return new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
            }
            return new ValidationResult_1.ValidationResult(true);
        };
        TextBox.prototype.valueCompare = function (rule) {
            //var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.ValueCompare);
            var result = new ValidationResult_1.ValidationResult(true);
            if (rule) {
                var valueToCompare = rule.compareValue;
                if (this.getValue && this.getValue()[0].text.trim() != "") {
                    if (rule.comparator == Comparator_1.Comparators.EqualsTo) {
                        if (parseFloat(this.getValue()[0].text.trim()) != valueToCompare[0])
                            result = new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
                    }
                    else if (rule.comparator == Comparator_1.Comparators.LessThan) {
                        if (!(parseFloat(this.getValue()[0].text.trim()) < valueToCompare[0]))
                            result = (new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg));
                    }
                    else if (rule.comparator == Comparator_1.Comparators.GreaterThan) {
                        if (!(parseFloat(this.getValue()[0].text.trim()) > valueToCompare[0]))
                            result = (new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg));
                    }
                    else if (rule.comparator == Comparator_1.Comparators.LessThanOrEqualTo) {
                        if (!(parseFloat(this.getValue()[0].text.trim()) <= valueToCompare[0]))
                            result = (new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg));
                    }
                    else if (rule.comparator == Comparator_1.Comparators.GreaterThanOrEqualTo) {
                        if (!(parseFloat(this.getValue()[0].text.trim()) >= valueToCompare[0]))
                            result = (new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg));
                    }
                    else if (rule.comparator == Comparator_1.Comparators.Between) {
                        if (!(valueToCompare[0] <= parseFloat(this.getValue()[0].text.trim()) && valueToCompare[1] >= parseFloat(this.getValue()[0].text.trim())))
                            result = (new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg));
                    }
                }
            }
            return result;
        };
        TextBox.prototype.regex = function (rule) {
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
        TextBox.prototype.onFocusChange = function () {
            //Required Validation on change
            this.validate();
        };
        TextBox.prototype.onCharChange = function () {
            this.validate(ValidationTypes_1.ValidationTypes.MinLength).result &&
                this.validate(ValidationTypes_1.ValidationTypes.MaxLength).result;
        };
        TextBox.prototype.setDisplay = function (visibile) {
            if (!visibile) {
                if (this.getDisplay())
                    this.clear();
                this.wrapperElement.classList.add(CssClassNames_1.CssClassNames["d-none"]);
            }
            else
                this.wrapperElement.classList.remove(CssClassNames_1.CssClassNames["d-none"]);
        };
        TextBox.prototype.registerEvent = function (event, callback, el) {
            this.domElement.addEventListener(event, function (e) { return callback(el); });
        };
        TextBox.prototype.getDisplay = function () {
            return !this.wrapperElement.classList.contains(CssClassNames_1.CssClassNames["d-none"]);
        };
        TextBox.prototype.clear = function () {
            this.domElement.value = "";
        };
        return TextBox;
    }(SectionElement_1.SectionElement));
    exports.TextBox = TextBox;
});
//# sourceMappingURL=TextBox.js.map