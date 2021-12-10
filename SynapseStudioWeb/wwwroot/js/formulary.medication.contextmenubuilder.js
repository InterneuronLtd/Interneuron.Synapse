class FormularyMedicationContextMenuItemsBuilder {
    constructor(menuObj, node) {
        this.menuObj = menuObj || {};
        this.node = node;

        this.menuObj.items = this.menuObj.items || {};
    }

    buildMenu() {
        if (!this.node || !this.node.data) return this.menuObj;
        if (this.node.data.Level == 'VTM')
            this.buildVTMMenu();
        else if (this.node.data.Level == 'VMP')
            this.buildVMPMenu();
        else if (this.node.data.Level == 'AMP')
            this.buildAMPMenu();

        return this.menuObj;
    }

    buildVTMMenu() {
        //All VMPs already selected
        //-No option to select all vmp
        //All AMPs already selected
        //-No option to select all amp
        //VMP not selectable
        //-No option to select all vmp
        //AMP not selectable
        //-No option to select all amp

        //let allVMPNodesSelected = true;
        let allAMPNodesSelected = true;
        //let anyVMPNodeSelected = false;
        let anyAMPNodeSelected = false;
        //let currentLevelSelected = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL);

        if (this.node.children && this.node.children.length > 0) {
            $.each(this.node.children, function (key, childNode) {
                //if (!childNode.isSelected() && allVMPNodesSelected) {
                //    allVMPNodesSelected = false;
                //}
                //if (childNode.isSelected() && !anyVMPNodeSelected) {
                //    anyVMPNodeSelected = true;
                //}
                if (childNode.children && childNode.children.length > 0) {
                    $.each(childNode.children, function (key, grandChildNode) {
                        if (!grandChildNode.isSelected() && allAMPNodesSelected) {
                            allAMPNodesSelected = false;
                        }
                        if (grandChildNode.isSelected() && !anyAMPNodeSelected) {
                            anyAMPNodeSelected = true;
                        }
                    });
                } else {
                    allAMPNodesSelected = false;
                }
            });
        } else {
            //allVMPNodesSelected = false;
            allAMPNodesSelected = false;
        }

        //let allowVMPSelection = !allVMPNodesSelected && (!currentLevelSelected || currentLevelSelected == 'VMP');
        let allowAMPSelection = !allAMPNodesSelected;

        //if (allowVMPSelection) {
        //    this.menuObj.items['VMP'] = {
        //        name: 'Select all VMP records',
        //        icon: function (opt, $itemElement, itemKey, item) {
        //            $itemElement.html('<span><img src="../img/Capsule.svg" class="fancytree-icon mx-1" title="Select all VMP records" alt="">' + item.name + '</span>');
        //            return 'context-menu-icon-updated';
        //        }
        //    };
        //}

        if (allowAMPSelection) {
            this.menuObj.items['AMP'] = {
                name: 'Select all AMP records',
                icon: function (opt, $itemElement, itemKey, item) {
                    $itemElement.html('<span><img src="../img/ActualMedicinalProduct.svg" class="fancytree-icon mx-1" title="Select all AMP records" alt="">' + item.name + '</span>');
                    return 'context-menu-icon-updated';
                }
            };
        }
        this.menuObj.items['sep1'] = '-------------';
        //if (anyVMPNodeSelected) {
        //    this.menuObj.items['DelVMP'] = {
        //        name: 'De-select all VMP records',
        //        icon: function (opt, $itemElement, itemKey, item) {
        //            $itemElement.html('<span><img src="../img/Capsule.svg" class="fancytree-icon mx-1" title="Select all VMP records" alt="">' + item.name + '</span>');
        //            return 'context-menu-icon-updated';
        //        }
        //    };
        //}

        if (anyAMPNodeSelected) {
            this.menuObj.items['DelAMP'] = {
                name: 'De-select all AMP records',
                icon: function (opt, $itemElement, itemKey, item) {
                    $itemElement.html('<span><img src="../img/ActualMedicinalProduct.svg" class="fancytree-icon mx-1" title="Select all AMP records" alt="">' + item.name + '</span>');
                    return 'context-menu-icon-updated';
                }
            };
        }
        //if (anyVMPNodeSelected || anyAMPNodeSelected)
        if (anyAMPNodeSelected)
            this.menuObj.items['sep2'] = '-------------';

        this.menuObj.items['quit'] = {
            name: 'Quit'
        };

    }

    buildVMPMenu() {
        //All AMPs already selected
        //-No option to select all amp
        //AMP not selectable
        //-No option to select all amp

        let allAMPNodesSelected = true;
        let anyAMPNodeSelected = false;
        //let currentLevelSelected = sessionStorage.getItem(studio.Storage.STORAGE_KEYS.FORMULARY_BULK_SELECT_LEVEL);

        if (this.node.children && this.node.children.length > 0) {
            $.each(this.node.children, function (key, childNode) {
                if (!childNode.isSelected() && allAMPNodesSelected) {
                    allAMPNodesSelected = false;
                }
                if (childNode.isSelected() && !anyAMPNodeSelected) {
                    anyAMPNodeSelected = true;
                }
            });
        } else {
            allAMPNodesSelected = false;
        }

        let allowAMPSelection = !allAMPNodesSelected;

        if (allowAMPSelection) {
            this.menuObj.items['AMP'] = {
                name: 'Select all AMP records',
                icon: function (opt, $itemElement, itemKey, item) {
                    $itemElement.html('<span><img src="../img/Capsule.svg" class="fancytree-icon mx-1" title="Select all AMP records" alt="">' + item.name + '</span>');
                    return 'context-menu-icon-updated';
                }
            };
        }
        this.menuObj.items['sep1'] = '-------------';

        if (anyAMPNodeSelected) {
            this.menuObj.items['DelAMP'] = {
                name: 'De-select all AMP records',
                icon: function (opt, $itemElement, itemKey, item) {
                    $itemElement.html('<span><img src="../img/ActualMedicinalProduct.svg" class="fancytree-icon mx-1" title="Select all AMP records" alt="">' + item.name + '</span>');
                    return 'context-menu-icon-updated';
                }
            };

            this.menuObj.items['sep2'] = '-------------';
        }

        this.menuObj.items['quit'] = {
            name: 'Quit'
        };
    }

    buildAMPMenu() {
        this.menuObj.items['quit'] = {
            name: 'Quit'
        };
    }
}