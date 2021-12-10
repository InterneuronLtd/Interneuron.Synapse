function namespace(namespaceString) {
    var parts = namespaceString.split('.'),
        parent = window,
        currentPart = '';

    for (var i = 0, length = parts.length; i < length; i++) {
        currentPart = parts[i];
        parent[currentPart] = parent[currentPart] || {};
        parent = parent[currentPart];
    }

    return parent;
}

function checkForMaxLength(selector) {
    var limit = parseInt($(selector).data('maxlength'));
    var text = $(selector).val();
    var chars = text.length;

    if (chars > limit) {
        var newText = text.substr(0, limit);
        $(selector).val(newText).change();
    }
}

(function ($) {
    var restrictToMaxLength = function(e) {
        var limit = parseInt($(this).data('maxlength'));
        var text = $(this).val();
        var len = text.length;
        if(len > limit) {
            $(this).val(text.substr(0,limit));
        }
    };
    $(function () {
        $('body').on('keyup','textarea.chkmaxlength',restrictToMaxLength);
        $('body').on('blur','textarea.chkmaxlength',restrictToMaxLength);

        setTimeout(function() {
            if (typeof(window.successMessage) != 'undefined') {
                if(window.urlToRedirect !== undefined && window.urlToRedirect!='') {
                    $.alert(window.successMessage, function() {
                        window.location = window.urlToRedirect;
                    });
                } else {
                    $.alert(window.successMessage);
                }
            }
            if (typeof(window.customMessage) != 'undefined') {
                if(window.urlToRedirect !== undefined && window.urlToRedirect!='') {
                    $.alert(window.customMessage, function() {
                        window.location = window.urlToRedirect;
                    });
                } else {
                    $.alert(window.customMessage);    
                }
                
            }
        }, 100);
        
        try {
            $(':text,select', 'div.contentWrapper,div.contentWrapper3').get(0).focus();   
        }
        catch(acold){}

    });

    // Used for showing dropdown tooltips
    function displayDropDownTooltip(control) {
        control.title = $('option:selected', control).text();
        $('option', control).each(function () {
            this.title = this.text;
        });
    }

    $('body').on('mouseover', 'select.show-tooltip', function() {
        var $this = $(this);
        if(!$this.data('tooltipattached'))
        {
            displayDropDownTooltip(this);
            $this.data('tooltipattached', true);
        }
    });

    $('body').on('change','select.show-tooltip',function() {
        this.title = $('option:selected', $(this)).text();
        displayDropDownTooltip(this);
    });

})(jQuery);