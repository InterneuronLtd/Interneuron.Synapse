(function ($) {

    jQuery.fn.center = function () {
        this.css("position", "absolute");
        this.css("top", Math.max(0, (($(window).height() - this.outerHeight()) / 2) +
            $(window).scrollTop()) + "px");
        this.css("left", Math.max(0, (($(window).width() - this.outerWidth()) / 2) +
            $(window).scrollLeft()) + "px");
        return this;
    };

    $.confirm = function (message, callback) {
        $.dialog(message, 'confirm', callback);
    };

    $.confirmWithMessagearea = function (message, textAreaMaxlength, callback, canCloseFn) {
        $.dialogMessageArea(message, 'confirm', textAreaMaxlength, callback, canCloseFn);
    };

    $.alert = function (message, callback) {
        $.dialog(message, 'alert', callback);
    };

    $.dialog = function (message, type, callbackfn) {
        var dialogBody = '<div class="confirmationDelete js-dialog" style="display: none; "> ' +
            '<p>{message}</p>' +
            '<br /><br />' +
            '<input type="button" id="dialog-yesbtn" class="saveBtn confirm" value="Yes"/>' +
            '<input type="button" id="dialog-nobtn" class="saveBtn confirm" value="No"/>' +
            '<input type="button" id="dialog-okay" class="saveBtn alert" value="Ok"/>' +
            '</div>';


        var $dialog = $(dialogBody.replace('{message}', message));
        $('input', $dialog).not('.' + type).remove();

        var overlayBody = '<div id="dialog-overlay" class="overlay" style="display: block;"><div>';
        var $overlay = $(overlayBody);

        var $otherOverlays = $('.overlay');

        if ($otherOverlays.length > 0) {
            $overlay.css('z-index', 10000);
            $dialog.css('z-index', 10200);
        }

        var callback = callbackfn;
        $('input', $dialog).click(function () {
            $dialog.remove();
            $overlay.remove();
            if (callback)
                callback($(this).val().toLowerCase());
        });
        $('body').append($overlay.css('height', $(document).height())).append($dialog);

        setTimeout(function () {
            $dialog.center();
            $dialog.show();
        }, 50);

        $(window).bind('resize', function () {
            $dialog.center({ transition: 300 });
        });

    };

    $.dialogMessageArea = function (message, type, maxlength, callbackfn, canCloseFn) {

        var dialogBody = '<div class="confirmationDelete js-dialogMessageArea" style="display: none; "> ' +
            '<p>{message}</p>' +
            '<br /><br />' +
            '<div id="dialog-error-message" class="validation-dialog-errors"></div>' +
            '<div style="word-wrap: break-word">' +
            '<textarea cols="50" data-maxlength=' + maxlength + ' id="dialog-Comment" rows="5"></textarea>' +
            '</div>' +
            '<br /><br />' +
            '<input type="button" id="dialog-yesbtn" class="saveBtn confirm" value="Yes"/>' +
            '<input type="button" id="dialog-nobtn" class="saveBtn confirm" value="No"/>' +
            '<input type="button" id="dialog-okay" class="saveBtn alert" value="Ok"/>' +
            '</div>';

       

        var $dialog = $(dialogBody.replace('{message}', message));
        $('input', $dialog).not('.' + type).remove();
        
        $('#dialog-Comment', $dialog).keyup(function () {
            checkForMaxLength(this);
        });

        var overlayBody = '<div id="dialog-overlay" class="overlay" style="display: block;"><div>';
        var $overlay = $(overlayBody);

        var $otherOverlays = $('.overlay');

        if ($otherOverlays.length > 0) {
            $overlay.css('z-index', 1000);
            $dialog.css('z-index', 1020);
        }

        var callback = callbackfn;
        $('input', $dialog).click(function () {
            var command = $(this).val().toLowerCase();
            var inputText = $('#dialog-Comment', $dialog).val();
            if (canCloseFn) {
                var msg = canCloseFn(command, inputText);
                if (msg && msg != '') {
                    $('#dialog-error-message').html(msg);
                    return false;
                }
            }
            $dialog.remove();
            $overlay.remove();

            if (callback)
                callback(command, inputText);
        });

        $('body').append($overlay.css('height', $(document).height())).append($dialog);

        setTimeout(function () {
            $dialog.center();
            $dialog.show();
        }, 50);

        $(window).bind('resize', function () {
            $dialog.center({ transition: 300 });
        });

    };
})(jQuery)