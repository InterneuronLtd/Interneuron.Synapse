/// <reference path="oidc-client.js" />



//set OIDC logger parameters
Oidc.Log.logger = window.console;
Oidc.Log.level = Oidc.Log.INFO;


var mgr = new Oidc.UserManager(config);

mgr.events.addUserLoaded(function (user) {
    console.log("User loaded");

});

mgr.events.addUserUnloaded(function () {
    console.log("User logged out locally");

});

mgr.events.addAccessTokenExpiring(function () {
    console.log("Access token is expiring...");
});
mgr.events.addSilentRenewError(function (err) {
    console.log("Silent renew error: " + err.message);
});
mgr.events.addUserSignedOut(function () {
    console.log("User signed out of SIS");
});
mgr.events.addAccessTokenExpired(function (user) { logout(); });

function login() {
    mgr.signinRedirect();

}

function logout() {
    mgr.signoutRedirect();
}

function revoke() {
    mgr.revokeAccessToken();
}

if (window.location.pathname.toLowerCase().endsWith("logout.aspx") && !window.location.href.includes("oidccallback"))
    logout();


var currentUser;
mgr.getUser().then(function (user) {
    if (user) {
        //currentUser = user;
        //get a new token even if one exists in localstorage as this page does not check aspnet sessions state. 
        if (!window.location.href.includes("oidccallback"))
            login();
    }
    else {
        currentUser = null;
        if (!window.location.href.includes("oidccallback"))
            login();
    }
});
