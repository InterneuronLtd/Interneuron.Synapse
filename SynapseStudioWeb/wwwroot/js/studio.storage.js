var studio = studio || {};

studio.Storage = studio.Storage || {};

studio.Storage.STORAGE_KEYS = {
    FORMULARY_BULK_SELECT_IDs: "FORMULARY_BULK_SELECT_IDs",
    FORMULARY_BULK_SELECT_IDs_TEMP: "FORMULARY_BULK_SELECT_IDs_TEMP",
    FORMULARY_BULK_SELECT_LEVEL: "FORMULARY_BULK_SELECT_LEVEL",
    FORMULARY_BULK_SELECT_STATUS: "FORMULARY_BULK_SELECT_STATUS"
};

studio.Storage
/*
studio.Storage = studio.Storage || (function () {

    var STORAGE_KEYS = {
        FORMULARY_BULK_SELECT_IDs: "FORMULARY_BULK_SELECT_IDs",
        FORMULARY_BULK_SELECT_LEVEL: "FORMULARY_BULK_SELECT_LEVEL"
    };

    
    class StudioStorage {
        static addToSession(keyName, data) {
            if (!keyName) return;
            sessionStorage.setItem(keyName, data);
        }

        static getFromSession(keyName) {
            if (!keyName) return null;
            sessionStorage.getItem(keyName);
        }

        static removeFromSession(keyName) {
            if (!keyName) return;
            sessionStorage.removeItem(keyName);
        }

        static clearSession() {
            sessionStorage.clear();
        }

        static addToLocalStorage(keyName, data) {
            if (!keyName) return;
            localStorage.setItem(keyName, data);
        }

        static getFromLocalStorage(keyName) {
            if (!keyName) return null;
            localStorage.getItem(keyName);
        }


        static removeFromLocalStorage(keyName) {
            if (!keyName) return;
            localStorage.removeItem(keyName);
        }

        static clearLocalStorage() {
            localStorage.clear();
        }
    }

    return {
        StudioStorage: StudioStorage, STORAGE_KEYS: STORAGE_KEYS
    };
})();
*/