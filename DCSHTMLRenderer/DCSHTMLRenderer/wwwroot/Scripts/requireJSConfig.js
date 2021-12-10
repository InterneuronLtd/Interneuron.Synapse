requirejs.config({
    baseUrl: './Scripts',
    paths: {
        "jQuery": "../../lib/jquery/jquery.min",
        "moment": "../../lib/moment.js/moment.min",
        "bootstrap": "https://maxcdn.bootstrapcdn.com/bootstrap/3.4.0/js/bootstrap.min.js",
        "bootstrap-datetime-picker": "../../lib/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js"
    },

    deps: ['formmodel'],

    urlArgs: "t=20160320000000" //flusing cache, do not use in production
});