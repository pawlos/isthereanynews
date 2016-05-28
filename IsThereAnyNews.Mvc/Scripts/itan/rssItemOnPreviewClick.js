(function($) {
    var onSuccessShowPreviewText = function() {};
    var onErrorShowPreviewText = function() {};

    var onShowPreviewText = function(e) {
        var element = $(e.currentTarget);

        var ajaxOptions = {
            url: element.data("url-mark-as-read"),
            method: "POST",
            success: onSuccessShowPreviewText,
            error: onErrorShowPreviewText
        }

        $.ajax(ajaxOptions);
    }

    var onDocumentReady = function() {
        $(".nocss-rsschannel-container").on("click", ".nocss-show-preview-text", onShowPreviewText);
    }

    $(document).ready(onDocumentReady);
})(jQuery);