(function ($) {
    var onSuccessRssChannelClick = function (response) {
        $(".nocss-rsschannel-container").html(response);
    };
    var onErrorRssChannelClick = function () { };

    var onRssChannelClick = function (e) {
        e.preventDefault();

        var href = $(e.currentTarget);


        var ajaxOptions = {
            url: href.data("url-read-rss"),
            method: "GET",
            type: "HTML",
            success: onSuccessRssChannelClick,
            error: onErrorRssChannelClick
        }
        window.history.pushState("test", "url", href.attr("href"));
        $.ajax(ajaxOptions);
    }

    var onDocumentReady = function () {
        $(".nocss-mychannel-list").on("click", ".nocss-rsschannel", onRssChannelClick);
    }

    $(document).ready(onDocumentReady);
})(jQuery);