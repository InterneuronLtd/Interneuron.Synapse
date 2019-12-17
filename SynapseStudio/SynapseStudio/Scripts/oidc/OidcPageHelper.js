/// <reference path="oidc-client.js" />


//set OIDC logger parameters
Oidc.Log.logger = window.console;
Oidc.Log.level = Oidc.Log.INFO;

//instantiate oidcmanager object
var mgr = new Oidc.UserManager(config);

mgr.events.addUserLoaded(function (user) {
    console.log("User loaded from silent renew: re-setting global auth header");

    //check if user is authenticated if not redirect to login page // this happens when page loads
    mgr.getUser().then(function (user) {
        if (user) {
            console.log("new access token :");
            console.log(user.access_token);
            at = user.access_token;

            // Set global Authorization header for all subsequent ajax xhr requests.
            $.ajaxSetup({
                beforeSend: function (xhr) {
                    console.log("global auth header set");
                    xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
                }
            });
        }
        else {
            console.log("user not found after silent renew: xhrs might fail")
            //window.location = "SessionExpired.html";
        }
    });


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
var at;

function logout() {
    mgr.signoutRedirect();
}
//check if user is authenticated if not redirect to login page // this happens when page loads
mgr.getUser().then(function (user) {
    if (user) {
        console.log("user is logged in");
        console.log(user.access_token);
        at = user.access_token;

        // Set global Authorization header for all subsequent ajax xhr requests.
        $.ajaxSetup({
            beforeSend: function (xhr) {
                console.log("global auth header set");
                xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
            }
        });
    }
    else {
        console.log("user not logged in: xhrs might fail")
        //window.location = "SessionExpired.html";
    }
});

