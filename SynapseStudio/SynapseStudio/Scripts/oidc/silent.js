var mgr = new Oidc.UserManager(config);
mgr.signinSilentCallback().then(function (user) {  });