(function ($) {
    var onSuccessFormSubmit = function () {
        $(".nocss-rsschannel-container .nocss-rss-item-list").html("");

    };
    var onErrorFormSubmit = function () { };

    var onFormSubmit = function (e) {
        e.preventDefault();
        var element = $(e.currentTarget);

        var ajaxOptions = {
            url: element.attr("action"),
            method: "POST",
            data: element.serialize(),
            success: onSuccessFormSubmit,
            error: onErrorFormSubmit
        }

        $.ajax(ajaxOptions);
    }

    var onDocumentReady = function () {
        $(".nocss-rsschannel-container").on("submit", ".nocss-mark-channel-read", onFormSubmit);
    }

    $(document).ready(onDocumentReady);
})(jQuery);