(function ($) {
    var onPreviewTextClick = function (e) {
        e.preventDefault();
        var element = $(e.currentTarget);
        var url = element.data("url-full-entry");
        window.open(url, '_blank');
    }

    var onDocumentReady = function () {
        $(".nocss-rsschannel-container").on("click", ".nocss-preview-text", onPreviewTextClick);
    }

    $(document).ready(onDocumentReady);
})(jQuery);