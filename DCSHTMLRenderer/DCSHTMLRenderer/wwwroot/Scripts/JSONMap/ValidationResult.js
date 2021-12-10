define(["require", "exports"], function (require, exports) {
    "use strict";
    Object.defineProperty(exports, "__esModule", { value: true });
    var ValidationResult = /** @class */ (function () {
        function ValidationResult(result, errormsg) {
            this.result = result;
            this.errorMsg = errormsg;
        }
        ValidationResult.prototype.concat = function (result) {
            this.result = this.result && result.result;
            if (!result.result)
                if (this.errorMsg != undefined)
                    this.errorMsg += result.errorMsg + document.createElement("br").outerHTML;
                else
                    this.errorMsg = result.errorMsg + document.createElement("br").outerHTML;
        };
        return ValidationResult;
    }());
    exports.ValidationResult = ValidationResult;
});
//# sourceMappingURL=ValidationResult.js.map