
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

function processAutoCompletes(formInput, rootElSel) {

    $(`${rootElSel} input.studio-autocomplete`).each(function (index, elItem) {

        let data = $(this).data();
        let elementName = elItem.name;
        let isSingleSelect = data.singleselect;
        let selectedData = data.selected;

        if (isSingleSelect) {
            formInput[elementName] = '';
            if (Array.isArray(selectedData) && selectedData.length > 0) {
                formInput[elementName] = {
                    id: selectedData[0].id, name: selectedData[0].name, source: selectedData[0].datasource, sourcecolor: selectedData[0].color, isreadonly: selectedData[0].readonly
                };
            }
        } else {
            formInput[elementName] = [];
            if (Array.isArray(selectedData) && selectedData.length > 0) {
                $.each(selectedData, (indx, item) => {
                    formInput[elementName].push({
                        id: item.id, name: item.name, source: item.datasource, sourcecolor: item.color, isreadonly: item.readonly
                    });
                });
            }
        }
    });
}

function processIdentificationCodes(formInput, rootElSel) {
    let identificationPanelId = `${rootElSel} #IdentificationCodeContainer .identificationCode`;

    let identifications = $(identificationPanelId);
    if (!identifications || identifications.length == 0) return;

    $(identificationPanelId).each(function (index, elItem) {

        var additionalCode = $(`${rootElSel} .identificationCode .additional-code-text`)[index].value;
        var additionalCodeSystem = $(`${rootElSel} .identificationCode .additional-code-system`)[index].selectedOptions[0].value;
        let additionalCodeDesc = $(`${rootElSel} .identificationCode .additional-code-desc`)[index].value;
        let additionalCodeSource = $(`${rootElSel} .identificationCode .source`)[index].value;
        let additionalCodeType = $(`${rootElSel} .identificationCode .code-type`)[index].value;

        var FormularyIdentificationCodes = {};

        FormularyIdentificationCodes["AdditionalCode"] = additionalCode;
        FormularyIdentificationCodes["AdditionalCodeSystem"] = additionalCodeSystem;
        FormularyIdentificationCodes["AdditionalCodeDesc"] = additionalCodeDesc;
        FormularyIdentificationCodes["Source"] = additionalCodeSource;
        FormularyIdentificationCodes["CodeType"] = additionalCodeType;

        if (formInput["FormularyIdentificationCodes"] !== undefined) {
            if (!formInput["FormularyIdentificationCodes"].push) {
                formInput["FormularyIdentificationCodes"] = [formInput["FormularyIdentificationCodes"]];
            }
            formInput["FormularyIdentificationCodes"].push(FormularyIdentificationCodes || '');
        }
        else {
            formInput["FormularyIdentificationCodes"] = [FormularyIdentificationCodes] || '';
        }
    });
}

function processAdditionalCodes(formInput, rootElSel) {
    let panelId = `${rootElSel} #AdditionalCodeContainer .additionalCode`;

    let dataList = $(panelId);
    if (!dataList || dataList.length == 0) return;

    $(panelId).each(function (index, elItem) {

        let additionalCodeCntrl = $(`${rootElSel} .additionalCode .additional-code-text`);

        //If additional code is present then enabled - else disabled template
        if (additionalCodeCntrl && additionalCodeCntrl.length > 0) {

            let additionalCode = additionalCodeCntrl[index].value;
            let additionalCodeSystem = $(`${rootElSel} .additionalCode .additional-code-system`)[index].selectedOptions[0].value;
            let additionalCodeDesc = $(`${rootElSel} .additionalCode .additional-code-desc`)[index].value;
            let additionalCodeSource = $(`${rootElSel} .additionalCode .source`)[index].value;
            let additionalCodeType = $(`${rootElSel} .additionalCode .code-type`)[index].value;

            var FormularyClassificationCodes = {};

            FormularyClassificationCodes["AdditionalCode"] = additionalCode;
            FormularyClassificationCodes["AdditionalCodeSystem"] = additionalCodeSystem;
            FormularyClassificationCodes["AdditionalCodeDesc"] = additionalCodeDesc;
            FormularyClassificationCodes["Source"] = additionalCodeSource;
            FormularyClassificationCodes["CodeType"] = additionalCodeType;

            if (formInput["FormularyClassificationCodes"] !== undefined) {
                if (!formInput["FormularyClassificationCodes"].push) {
                    formInput["FormularyClassificationCodes"] = [formInput["FormularyClassificationCodes"]];
                }
                formInput["FormularyClassificationCodes"].push(FormularyClassificationCodes || '');
            }
            else {
                formInput["FormularyClassificationCodes"] = [FormularyClassificationCodes] || '';
            }
        }
    });
}

function processCustomWarnings(formInput, rootElSel) {

    let panelId = `${rootElSel} #CustomWarningContainer .custom-warning`;

    let controlsList = $(panelId);
    if (!controlsList || controlsList.length == 0) return;

    $(panelId).each(function (index, elItem) {
        let warningText = $($(`${rootElSel}  .custom-warning textarea`)[index]).val();
        let needResponse = $($(`${rootElSel} .custom-warning input`)[index]).is(':checked');

        var CustomWarnings = {};

        CustomWarnings["Warning"] = warningText;
        CustomWarnings["NeedResponse"] = needResponse;

        if (formInput["CustomWarnings"] !== undefined) {
            if (!formInput["CustomWarnings"].push) {
                formInput["CustomWarnings"] = [formInput["CustomWarnings"]];
            }
            formInput["CustomWarnings"].push(CustomWarnings || '');
        }
        else {
            formInput["CustomWarnings"] = [CustomWarnings] || '';
        }
    });
}

function processReminders(formInput, rootElSel) {

    let panelId = `${rootElSel} #ReminderContainer .reminder`;

    let controlsList = $(panelId);
    if (!controlsList || controlsList.length == 0) return;

    $(panelId).each(function (index, elItem) {
        let reminder = $($(`${rootElSel}  .reminder textarea`)[index]).val();
        let duration = $($(`${rootElSel}  .reminder .duration`)[index]).val();
        let active = $($(`${rootElSel} .reminder .rem-active`)[index]).is(':checked');

        var Reminders = {};

        Reminders["Reminder"] = reminder;
        Reminders["Duration"] = duration;
        Reminders["Active"] = active;

        if (formInput["Reminders"] !== undefined) {
            if (!formInput["Reminders"].push) {
                formInput["Reminders"] = [formInput["Reminders"]];
            }
            formInput["Reminders"].push(Reminders || '');
        }
        else {
            formInput["Reminders"] = [Reminders] || '';
        }
    });
}

function processEndorsements(formInput, rootElSel) {

    let panelId = `${rootElSel} #EndorsementContainer .endorsement`;

    let controlsList = $(panelId);
    if (!controlsList || controlsList.length == 0) return;

    $(panelId).each(function (index, elItem) {
        let endorsementVal = $($(`${rootElSel}  .endorsement textarea`)[index]).val();

        if (formInput["Endorsements"] !== undefined) {
            if (!formInput["Endorsements"].push) {
                formInput["Endorsements"] = [formInput["Endorsements"]];
            }
            formInput["Endorsements"].push(endorsementVal || '');
        }
        else {
            formInput["Endorsements"] = [endorsementVal] || '';
        }
    });
}

function processIngredients(formInput, rootElSel) {
    let rootElName = rootElSel.substring(1);

    $(`${rootElSel} .ingredient`).each(function (index, elItem) {

        var Ingredients = {};
        let basisOfPharmaceuticalStrength = null;
        let selectedIngredient = $($(`${rootElSel} .ingredient input[id^='${rootElName}_Ingredient']`)[index]).data('selected');

        let selectedStrengthValueNumeratorUnit = $($(`${rootElSel} .ingredient input[id^='${rootElName}_StrengthValueNumeratorUnit']`)[index]).data('selected');
        let selectedStrengthValueDenominatorUnit = $($(`${rootElSel} .ingredient input[id^='${rootElName}_StrengthValueDenominatorUnit']`)[index]).data('selected');

        var strengthValueNumerator = $(`${rootElSel} .ingredient input[id^='${rootElName}_StrengthValNumerator']`)[index].value;
        var strengthValueDenominator = $(`${rootElSel} .ingredient input[id^='${rootElName}_StrengthValDenominator']`)[index].value;
        if ($(`${rootElSel} .ingredient select`)[index].selectedOptions.length > 0) {
            basisOfPharmaceuticalStrength = $(`${rootElSel} .ingredient select`)[index].selectedOptions[0].value;
        }
        else {
            let hdnBasisOfPharmaStrnCntrl = $(`${rootElSel} .ingredient input[id^='${rootElName}_hdnBasisOfPharmaceuticalStrength']`);
            if (hdnBasisOfPharmaStrnCntrl && hdnBasisOfPharmaStrnCntrl[index]) {
                basisOfPharmaceuticalStrength = $(hdnBasisOfPharmaStrnCntrl[index]).val();
            }
        }


        if (selectedIngredient && selectedIngredient.length > 0) {
            Ingredients["Ingredient"] = { id: selectedIngredient[0].id, name: selectedIngredient[0].name };
        }

        if (selectedStrengthValueNumeratorUnit && selectedStrengthValueNumeratorUnit.length > 0) {
            Ingredients["StrengthValueNumeratorUnit"] = { id: selectedStrengthValueNumeratorUnit[0].id, name: selectedStrengthValueNumeratorUnit[0].name };
        }

        if (selectedStrengthValueDenominatorUnit && selectedStrengthValueDenominatorUnit.length > 0) {
            Ingredients["StrengthValueDenominatorUnit"] = { id: selectedStrengthValueDenominatorUnit[0].id, name: selectedStrengthValueDenominatorUnit[0].name };
        }

        Ingredients["StrengthValNumerator"] = strengthValueNumerator;
        Ingredients["StrengthValDenominator"] = strengthValueDenominator;
        Ingredients["BasisOfPharmaceuticalStrength"] = basisOfPharmaceuticalStrength;

        if (formInput["Ingredients"] !== undefined) {
            if (!formInput["Ingredients"].push) {
                formInput["Ingredients"] = [formInput["Ingredients"]];
            }
            formInput["Ingredients"].push(Ingredients || '');
        }
        else {
            formInput["Ingredients"] = [Ingredients] || '';
        }

    });
}

function processExcipients(formInput, rootElSel) {
    let rootElName = rootElSel.substring(1);

    $(`${rootElSel} .excipient`).each(function (index, elItem) {

        var Excipients = {};

        let selectedIngredient = $($(`${rootElSel} .excipient input[id^='${rootElName}_Excipient_Ingredient']`)[index]).data('selected');

        let selectedStrengthUnit = $($(`${rootElSel} .excipient input[id^='${rootElName}_Excipient_StrengthUnit']`)[index]).data('selected');

        var strengthValue = $(`${rootElSel} .excipient input[id^='${rootElName}_Excipient_Strength']`)[index].value;

        if (selectedIngredient && selectedIngredient.length > 0) {
            Excipients["Ingredient"] = { id: selectedIngredient[0].id, name: selectedIngredient[0].name };
        }

        if (selectedStrengthUnit && selectedStrengthUnit.length > 0) {
            Excipients["StrengthUnit"] = { id: selectedStrengthUnit[0].id, name: selectedStrengthUnit[0].name };
        }
        Excipients["Strength"] = strengthValue;

        if (formInput["Excipients"] !== undefined) {
            if (!formInput["Excipients"].push) {
                formInput["Excipients"] = [formInput["Excipients"]];
            }
            formInput["Excipients"].push(Excipients || '');
        }
        else {
            formInput["Excipients"] = [Excipients] || '';
        }

    });
}


var studio = studio || {};

studio.FormularySave = studio.FormularySave || class {
    constructor(formElSelector, rootElSelector) {
        this._formElSelector = formElSelector;
        this._rootElSelector = rootElSelector;
    }

    update(beforeSaveFn, saveSuccessFn, saveFailFn) {

        $(this._formElSelector).find('input:text').each(function () {
            this.value = $(this).val().trim();
        });

        let formInput = $(this._formElSelector).serializeObject();

        if (beforeSaveFn) beforeSaveFn();

        processAutoCompletes.bind(this)(formInput, this._rootElSelector);
        processIdentificationCodes.bind(this)(formInput, this._rootElSelector);
        processAdditionalCodes.bind(this)(formInput, this._rootElSelector);
        processCustomWarnings.bind(this)(formInput, this._rootElSelector);
        processReminders.bind(this)(formInput, this._rootElSelector);
        processEndorsements.bind(this)(formInput, this._rootElSelector);
        processIngredients.bind(this)(formInput, this._rootElSelector);
        processExcipients.bind(this)(formInput, this._rootElSelector);

        let isBulkEdit = (formInput['IsBulkEdit'] && (formInput['IsBulkEdit'] == true || formInput['IsBulkEdit'] == 'True'))
        let url = isBulkEdit ? 'Formulary/UpdateBulkFormulary': 'Formulary/UpdateFormulary';

        ajaxPost(url, formInput,
            (data) => {
                if (saveSuccessFn) saveSuccessFn(data, formInput);
            },
            (err) => {
                if (saveFailFn) saveFailFn(err, formInput);
            });
    }

    create(beforeSaveFn, saveSuccessFn, saveFailFn) {

        $(this._formElSelector).find('input:text').each(function () {
            this.value = $(this).val().trim();
        });

        let formInput = $(this._formElSelector).serializeObject();

        if (beforeSaveFn) beforeSaveFn();

        processAutoCompletes.bind(this)(formInput, this._rootElSelector);
        processIdentificationCodes.bind(this)(formInput, this._rootElSelector);
        processAdditionalCodes.bind(this)(formInput, this._rootElSelector);
        processCustomWarnings.bind(this)(formInput, this._rootElSelector);
        processReminders.bind(this)(formInput, this._rootElSelector);
        processEndorsements.bind(this)(formInput, this._rootElSelector);
        processIngredients.bind(this)(formInput, this._rootElSelector);
        processExcipients.bind(this)(formInput, this._rootElSelector);

        ajaxPost('Formulary/CreateFormulary', formInput,
            (data) => {
                if (saveSuccessFn) saveSuccessFn(data);
            },
            (err) => {
                if (saveFailFn) saveFailFn(err);
            });
    }
}
