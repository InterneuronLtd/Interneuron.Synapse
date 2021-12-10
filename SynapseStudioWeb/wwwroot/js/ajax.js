// ajax.js

//highlight animation
(function ($) {
    $.fn.glow = function () {
        this.animate({ backgroundColor: "#ffffcc" }, 1).animate({ backgroundColor: "#EDEEEF" }, 1500);
    };
})(jQuery);

//var port = $(location).attr('protocol');
//var host = $(location).attr('host');
//var basePath = (typeof(appSettings) != 'undefined' && appSettings.AppPath) || '';

var basePath = (typeof (AppPath) != 'undefined' && AppPath) || '';

var buildUrl = function (actionurl) {
    return actionurl[0] != '/' ? basePath + "/" + actionurl : basePath + actionurl;
};

var ajaxLoad = function (url, args, $container, onSuccess, onError) {
    $.get(buildUrl(url),
        args,
        function (data, status, xhr) {
            if ($container)
                $container.html(data);
            if (status == 'success' && onSuccess)
                onSuccess(data);
            if (status == 'error') {
                console.log(data);
                if (xhr && xhr.responseJSON && xhr.status == 307)
                    window.location.href = xhr.responseJSON.url;
                else if (onError) onError(data);
            }
        },
        'html'
    );
};

var ajaxPost = function (url, args, onSuccess, onError) {
    var fullUrl = buildUrl(url);

    $.post(
        fullUrl,
        args,
        function (data, status, xhr) {
            if (status == 'success' && onSuccess)
                onSuccess(data);
            if (status == 'error') {
                console.log(data);
                if (xhr && xhr.responseJSON && xhr.status == 307)
                    window.location.href = xhr.responseJSON.url;
                else if (onError) onError(data);
            }
        });
};

var ajaxGetJson = function (url, args, onSuccess, onError) {

    var fullUrl = buildUrl(url);
    $.ajax({
        url: fullUrl,
        dataType: 'json',
        data: args,
        success: onSuccess,
        error: function (xhr, status, err) {
            console.log(err);
            if (xhr && xhr.responseJSON && xhr.status == 307)
                window.location.href = xhr.responseJSON.url;
            else if (onError) onError(err);
        }
    });
};

    var ajaxPostJson = function (url, args, onSuccess, onError) {

        var fullUrl = buildUrl(url);
        $.ajax({
            type: 'POST',
            url: fullUrl,
            dataType: 'json',
            data: args,
            success: onSuccess,
            error: function (xhr, status, err) {
                console.log(err);
                if (xhr && xhr.responseJSON && xhr.status == 307)
                    window.location.href = xhr.responseJSON.url;
                else if (onError) onError(err);
            }
        });
};