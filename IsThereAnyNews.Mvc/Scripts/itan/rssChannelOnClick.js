(function ($) {
    var onSuccessRssChannelClick = function (response) {
        $(".nocss-rsschannel-container").html(response);
        $('.collapsible').collapsible({
            accordion: false // A setting that changes the collapsible behavior to expandable instead of the default accordion style
        });
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