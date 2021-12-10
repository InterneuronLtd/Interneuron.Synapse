var studio = studio || {};

studio.AutoComplete = studio.AutoComplete || (function () {

    let defaultProps = {
        minChars: 2,
        noResultsText: 'No Results',
        theme: 'facebook',
        zindex: 10000,
        allowFreeTagging: false,
        preventDuplicates: true,
        disabled: false
    };

    class StudioAutoComplete {
        constructor(selector, data, dataAsFn, existingdata, additionalProps) {
            this.data = data;
            this.selector = selector;
            this.dataAsFn = dataAsFn;
            this.$thisSelector = $(this.selector);
            this.existingdata = existingdata ? existingdata : this.$thisSelector.data('existingdata');

            if (!additionalProps) {
                additionalProps = {};
            }

            let isSingleSelect = this.$thisSelector.data('singleselect');
            let isDisabled = this.$thisSelector.data('isdisabled');

            if (isSingleSelect) {
                additionalProps['tokenLimit'] = 1;
            }

            if (isDisabled) {
                additionalProps['disabled'] = true;
            }

            this.props = Object.assign({}, defaultProps, additionalProps);

            //console.log('Existing Data for :' + this.$thisSelector);
            //console.log(this.existingdata);

            if (this.existingdata) {
                this.props['prePopulate'] = this.existingdata;
            }

            //console.log('isdisabled:');
            //console.log(this.$thisSelector.data('isdisabled'));
            if (this.$thisSelector.data('isdisabled') === true || this.$thisSelector.data('isdisabled') == 'true' || this.$thisSelector.data('isdisabled') == 'True') {
                this.props['disabled'] = true;
            }

            this.props['onAdd'] = (item) => {
                //this.$thisSelector.data('selected', this.$thisSelector.tokenInput("get"));
                this.setDatafromSelection();
                if (this.onAdd) {
                    this.onAdd(item);
                }
                //console.log(this.$thisSelector.data('selected'));
            };

            this.props['onDelete'] = (item) => {
                this.setDatafromSelection();
                if (this.onRemove) {
                    this.onRemove(item);
                }
                //console.log(this.$thisSelector.data('selected'));
            };

            this.props['tokenFormatter'] = (item) => {
                if (item) {
                    if (item.readonly) {
                        if (item.color) {
                            return `<li style="background-color:${item.color};" title="${item.datasource ? item.datasource : ''}"><p>${item.name}</p></li>`;
                        }
                        return `<li style="background-color:lightgray;" title="${item.datasource ? item.datasource:''}"><p>${item.name}</p></li>`;
                    } else if (item.color) {
                        return `<li style="background-color:${item.color};" title="${item.datasource ? item.datasource : ''}"><p>${item.name}</p></li>`;
                    } else {
                        return `<li style="background-color:#eff2f7;" title="${item.datasource ? item.datasource : ''}"><p>${item.name}</p></li>`
                    }
                    return ``;
                }
            };
        }

        setDatafromSelection() {
            this.$thisSelector.data('selected', this.$thisSelector.tokenInput("get"));
            this.$thisSelector.val(this.$thisSelector.tokenInput("get"));
            this.$thisSelector.blur();
            //var self = this;
            //setTimeout(function () { this.$thisSelector.focus(); $(self).focus();  }, 100);
        }

        init() {
            let queryUrl = this.$thisSelector.data('query-url');
            let inputSrc = this.$thisSelector.data('input-src');
            if (queryUrl) {
                this.$thisSelector.tokenInput(queryUrl, this.props);
            } else if (this.dataAsFn != null) {
                this.data = this.dataAsFn();
                this.$thisSelector.tokenInput(this.data, this.props);
            } else if (inputSrc) {
                this.$thisSelector.tokenInput(inputSrc, this.props);
            } else {
                this.$thisSelector.tokenInput(this.data, this.props);
            }

            //Add when prepoulated
            //this.$thisSelector.data('selected', this.$thisSelector.tokenInput("get"));
            this.setDatafromSelection();
        }


        add(item) {
            if (item) {
                this.$thisSelector.tokenInput("add", item);
            }
        }
        removeById(idInput) {
            if (item) {
                this.$thisSelector.tokenInput("remove", { id: idInput });
            }
            //this.$thisSelector.data('selected', this.$thisSelector.tokenInput("get"));
            this.setDatafromSelection();
        }
        removeByName(nameInput) {
            if (item) {
                this.$thisSelector.tokenInput("remove", { name: nameInput });
            }
            //this.$thisSelector.data('selected', this.$thisSelector.tokenInput("get"));
            this.setDatafromSelection();

        }
        clear() {
            this.$thisSelector.tokenInput("clear");
            //this.$thisSelector.data('selected', this.$thisSelector.tokenInput("get"));
            this.setDatafromSelection();
        }
        getSelectedData() {
            this.$thisSelector.tokenInput("get");
        }
    }

    return StudioAutoComplete;
})();