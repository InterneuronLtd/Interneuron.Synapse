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
define(["require", "exports", "../Enumerations/ElementType", "./SectionElement", "../Enumerations/CssClassNames", "../JSONMap/FieldValue", "../Enumerations/ValidationTypes", "../JSONMap/ValidationResult", "../Enumerations/Comparator", "../Enumerations/CompareDateValues", "../formmodel"], function (require, exports, ElementType_1, SectionElement_1, CssClassNames_1, FieldValue_1, ValidationTypes_1, ValidationResult_1, Comparator_1, CompareDateValues_1, formmodel_1) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var Datetime = /** @class */ (function (_super) {
        __extends(Datetime, _super);
        function Datetime(id, defaultValue) {
            var _this = _super.call(this) || this;
            _this.wrapperElement = document.createElement("div");
            _this.domElement = document.createElement("input");
            _this.errorLabel = document.createElement("label");
            _this.fieldValidations = new Array();
            _this.domElement.type = "text";
            if (typeof id != undefined && id) {
                _this.id = id;
                _this.domElement.id = id;
                _this.domElement.name = id;
            }
            if (typeof defaultValue != undefined && defaultValue) {
                _this.setDefaultValue(defaultValue);
            }
            _this.domElement.classList.add("datetime");
            _this.wrapperElement.appendChild(_this.domElement);
            return _this;
        }
        Datetime.prototype.setDefaultValue = function (value) {
            if (typeof value != undefined && value) {
                this.defaultvalue = this.getStringDate(value);
            }
            if (typeof this.defaultvalue != undefined && this.defaultvalue) {
                this.domElement.value = this.defaultvalue;
            }
        };
        Datetime.prototype.getStringDate = function (isoDate) {
            var date = new Date(isoDate);
            var day = date.getDate() < 10 ? '0' + date.getDate() : '' + date.getDate();
            var month = (date.getMonth() + 1) < 10 ? '0' + (date.getMonth() + 1) : '' + (date.getMonth() + 1);
            var hour = date.getHours() < 10 ? '0' + date.getHours() : '' + date.getHours();
            var minutes = date.getMinutes() < 10 ? '0' + date.getMinutes() : '' + date.getMinutes();
            return day + '/' + month + '/' + date.getFullYear() + ' ' + hour + ':' + minutes;
        };
        Datetime.prototype.getValue = function () {
            if (typeof this.domElement.value != undefined && this.domElement.value) {
                var dateparts = this.domElement.value.split(' ');
                var _a = dateparts[0].split("/"), day = _a[0], month = _a[1], year = _a[2];
                var _b = dateparts[1].split(":"), hour = _b[0], minute = _b[1];
                //var inputDate = new Date(parseInt(year), parseInt(month) - 1, parseInt(day), parseInt(hour), parseInt(minute));
                return [new FieldValue_1.FieldValue(year + "-" + month + "-" + day + "T" + hour + ":" + minute + ":00", year + "-" + month + "-" + day + "T" + hour + ":" + minute + ":00")];
            }
            return [new FieldValue_1.FieldValue("", "")];
        };
        Datetime.prototype.getMarkup = function () {
            var _this = this;
            // *** Comment Start *** 
            //This line is required for rendering calendar div. Should not be required once proper bootstrap css is applied to the wrapper div
            this.wrapperElement.style.position = "relative";
            // *** Comment End ***
            if (this.fieldValidations && this.fieldValidations.length > 0) {
                this.domElement.addEventListener("blur", function (e) { return _this.onFocusChange(); });
                this.domElement.addEventListener("input", function (e) { return _this.onCharChange(); });
            }
            this.wrapperElement.appendChild(this.addQuestionLabel());
            this.wrapperElement.appendChild(this.domElement);
            this.wrapperElement.appendChild(this.errorLabel);
            this.wrapperElement.className = CssClassNames_1.CssClassNames["form-group"];
            this.domElement.classList.add(CssClassNames_1.CssClassNames["form-control"]);
            formmodel_1.FormHelpers.setVisibility(this);
            return this.wrapperElement;
        };
        Datetime.prototype.getControlType = function () {
            return ElementType_1.ElementType.DateTime;
        };
        Datetime.prototype.compareValue = function (comparator, dateComponentLow, compareValueLow, dateComponentHigh, compareValueHigh) {
            var isSuccessful = true;
            var inputValue = this.getValue()[0].text;
            var now = new Date();
            if (inputValue && inputValue != "") {
                var inputMilliseconds = new Date(inputValue).getTime();
                var compareMillisecondsLow;
                var compareMillisecondsHigh;
                if (dateComponentLow == CompareDateValues_1.CompareDateValues.SECONDS) {
                    compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds() + parseInt(compareValueLow)).getTime();
                }
                else if (dateComponentLow == CompareDateValues_1.CompareDateValues.MINUTES) {
                    compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes() + parseInt(compareValueLow)).getTime();
                }
                else if (dateComponentLow == CompareDateValues_1.CompareDateValues.HOURS) {
                    compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours() + parseInt(compareValueLow)).getTime();
                }
                else if (dateComponentLow == CompareDateValues_1.CompareDateValues.DAYS) {
                    compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth(), now.getDate() + parseInt(compareValueLow)).getTime();
                }
                else if (dateComponentLow == CompareDateValues_1.CompareDateValues.WEEKS) {
                    compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth(), now.getDate() + (parseInt(compareValueLow) * 7)).getTime();
                }
                else if (dateComponentLow == CompareDateValues_1.CompareDateValues.MONTHS) {
                    compareMillisecondsLow = new Date(now.getFullYear(), now.getMonth() + parseInt(compareValueLow), now.getDate()).getTime();
                }
                else if (dateComponentLow == CompareDateValues_1.CompareDateValues.YEARS) {
                    compareMillisecondsLow = new Date(now.getFullYear() + parseInt(compareValueLow), now.getMonth(), now.getDate()).getTime();
                }
                else {
                    compareMillisecondsLow = new Date(dateComponentLow).getTime();
                }
                if (dateComponentHigh != null && dateComponentHigh != undefined) {
                    if (dateComponentHigh == CompareDateValues_1.CompareDateValues.SECONDS) {
                        compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes(), now.getSeconds() + parseInt(compareValueHigh)).getTime();
                    }
                    else if (dateComponentHigh == CompareDateValues_1.CompareDateValues.MINUTES) {
                        compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours(), now.getMinutes() + parseInt(compareValueHigh)).getTime();
                    }
                    else if (dateComponentHigh == CompareDateValues_1.CompareDateValues.HOURS) {
                        compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth(), now.getDate(), now.getHours() + parseInt(compareValueHigh)).getTime();
                    }
                    else if (dateComponentHigh == CompareDateValues_1.CompareDateValues.DAYS) {
                        compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth(), now.getDate() + parseInt(compareValueHigh)).getTime();
                    }
                    else if (dateComponentHigh == CompareDateValues_1.CompareDateValues.WEEKS) {
                        compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth(), now.getDate() + (parseInt(compareValueHigh) * 7)).getTime();
                    }
                    else if (dateComponentHigh == CompareDateValues_1.CompareDateValues.MONTHS) {
                        compareMillisecondsHigh = new Date(now.getFullYear(), now.getMonth() + parseInt(compareValueHigh), now.getDate()).getTime();
                    }
                    else if (dateComponentHigh == CompareDateValues_1.CompareDateValues.YEARS) {
                        compareMillisecondsHigh = new Date(now.getFullYear() + parseInt(compareValueHigh), now.getMonth(), now.getDate()).getTime();
                    }
                    else {
                        compareMillisecondsHigh = new Date(dateComponentHigh).getTime();
                    }
                }
                if (comparator == Comparator_1.Comparators.EqualsTo) {
                    // input should be equal to the value to compare
                    if (!(inputMilliseconds == compareMillisecondsLow))
                        isSuccessful = false;
                }
                else if (comparator == Comparator_1.Comparators.NotEqualTo) {
                    // input should not be equal to the value to compare
                    if (!(inputMilliseconds != compareMillisecondsLow))
                        isSuccessful = false;
                }
                else if (comparator == Comparator_1.Comparators.LessThan) {
                    // input should be less than the value to compare
                    if (!(inputMilliseconds < compareMillisecondsLow))
                        isSuccessful = false;
                }
                else if (comparator == Comparator_1.Comparators.LessThanOrEqualTo) {
                    // input should be less than or equal to the value to compare
                    if (!(inputMilliseconds <= compareMillisecondsLow))
                        isSuccessful = false;
                }
                else if (comparator == Comparator_1.Comparators.GreaterThan) {
                    // input should be greater than the value to compare
                    if (!(inputMilliseconds > compareMillisecondsLow))
                        isSuccessful = false;
                }
                else if (comparator == Comparator_1.Comparators.GreaterThanOrEqualTo) {
                    // input should be greater than or equal to the value to compare
                    if (!(inputMilliseconds >= compareMillisecondsLow))
                        isSuccessful = false;
                }
                else if (comparator == Comparator_1.Comparators.Between) {
                    // input should be between the low and high value (inclusive)
                    if (!(inputMilliseconds >= compareMillisecondsLow && inputMilliseconds <= compareMillisecondsHigh))
                        isSuccessful = false;
                }
            }
            return isSuccessful;
        };
        Datetime.prototype.validate = function (type) {
            var result = new ValidationResult_1.ValidationResult(true);
            if (this.getDisplay())
                if (!type) {
                    //eval all available validations for this element
                    for (var i = 0; i < this.fieldValidations.length; i++) {
                        if (this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.Required) {
                            result.concat(this.required(this.fieldValidations[i]));
                        }
                        else if (this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.ValueCompare) {
                            result.concat(this.valueCompare(this.fieldValidations[i]));
                        }
                    }
                }
                else {
                    for (var i = 0; i < this.fieldValidations.length; i++) {
                        if (type == ValidationTypes_1.ValidationTypes.Required && this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.Required) {
                            result.concat(this.required(this.fieldValidations[i]));
                        }
                        else if (type == ValidationTypes_1.ValidationTypes.ValueCompare && this.fieldValidations[i].validationtype == ValidationTypes_1.ValidationTypes.ValueCompare) {
                            result.concat(this.valueCompare(this.fieldValidations[i]));
                        }
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
        Datetime.prototype.getDisplay = function () {
            return !this.wrapperElement.classList.contains(CssClassNames_1.CssClassNames["d-none"]);
        };
        //validators
        Datetime.prototype.required = function (rule) {
            // var rule = this.fieldValidations.filter(item => item.validationtype == ValidationTypes.Required);
            if (rule) {
                if (this.getValue() && this.getValue()[0].text != null && this.getValue()[0].text.trim() != "")
                    return new ValidationResult_1.ValidationResult(true);
                else
                    return new ValidationResult_1.ValidationResult(false, rule.validationErrorMsg);
            }
            return new ValidationResult_1.ValidationResult(true);
        };
        Datetime.prototype.valueCompare = function (rule) {
            var isValid = true;
            if (rule) {
                var inputValue = this.getValue()[0].text.trim().substring(0, 19);
                if (inputValue && inputValue != "") {
                    var compareValueLow = rule.compareValue.length != 0 ? "" + rule.compareValue[0] : null;
                    var compareValueHigh = rule.compareValue.length == 2 ? "" + rule.compareValue[1] : null;
                    isValid = this.compareValue(rule.comparator, rule.compareDateValue[0], compareValueLow, rule.compareDateValue[1], compareValueHigh);
                }
            }
            return isValid ? (new ValidationResult_1.ValidationResult(isValid)) : (new ValidationResult_1.ValidationResult(isValid, rule.validationErrorMsg));
        };
        //events
        Datetime.prototype.onFocusChange = function () {
            //Required Validation on change
            this.validate();
        };
        Datetime.prototype.onCharChange = function () {
            this.validate(ValidationTypes_1.ValidationTypes.MinLength).result &&
                this.validate(ValidationTypes_1.ValidationTypes.MaxLength).result;
        };
        Datetime.prototype.setDisplay = function (visibile) {
            if (!visibile) {
                if (this.getDisplay())
                    this.clear();
                this.wrapperElement.classList.add(CssClassNames_1.CssClassNames["d-none"]);
            }
            else
                this.wrapperElement.classList.remove(CssClassNames_1.CssClassNames["d-none"]);
        };
        Datetime.prototype.registerEvent = function (event, callback, el) {
            this.domElement.addEventListener(event, function (e) { return callback(el); });
        };
        Datetime.prototype.clear = function () {
            this.domElement.value = "";
        };
        return Datetime;
    }(SectionElement_1.SectionElement));
    exports.Datetime = Datetime;
});
//# sourceMappingURL=DateTime.js.map