

$(document).ready(function () {
    var mgr = new Oidc.UserManager({});


    mgr.signinRedirectCallback().then(function (user) {
        // console.log(user);
        document.getElementById('hTkn').value = user.access_token;
        window.history.replaceState({},
            window.document.title,
            window.location.origin + window.location.pathname);
        document.getElementById('btnPostback').click();

    });

});