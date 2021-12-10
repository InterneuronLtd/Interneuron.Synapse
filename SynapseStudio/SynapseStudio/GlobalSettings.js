
var GlobalServiceURL = 'SYNAPSE_DYNAMIC_URI/';

var config = {

    //configure authority and client

    authority: "SYNAPSE_IDENTITY_URI",
    client_id: "SynapseStudio",
    redirect_uri: window.location.origin + "/callback.aspx",
    post_logout_redirect_uri: window.location.origin + "/logout.aspx?oidccallback=true",

    response_type: "id_token token",
    scope: "openid dynamicapi.read",

    //set default authentication provider for this client
    //acr_values: "idp:ADFS",

    //load userinfo from user info end point
    //disabled as SIS will copy userinfo into access token
    loadUserInfo: false,

    // This will get a new access_token via an iframe 60 secs before the old token is going to expire
    automaticSilentRenew: true,
    silent_redirect_uri: window.location.origin + "/SilentRenew.aspx",

    // will revoke access tokens at logout time
    revokeAccessTokenOnSignout: true,

    filterProtocolClaims: false,
    accessTokenExpiringNotificationTime: 60

};

