(function ($) {

    var openOriginalStory = function (element) {
        var url = element.data("url-full-entry");
        window.open(url, '_blank');
    }

    var sendMarkAsClickedRequest = function (element) {
        var url = element.data("url-mark-as-clicked");

        var ajaxOptions = {
            method: "POST",
            url: url
        }
        $.ajax(ajaxOptions);
    }

    var onPreviewTextClick = function (e) {
        e.preventDefault();
        var element = $(e.currentTarget);

        openOriginalStory(element);
        sendMarkAsClickedRequest(element);
    }



    var onDocumentReady = function () {
        $(".nocss-rsschannel-container").on("click", ".nocss-preview-text", onPreviewTextClick);
    }

    $(document).ready(onDocumentReady);
})(jQuery);