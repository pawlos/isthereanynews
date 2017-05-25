angular
    .module("itan")
    .factory("entriesService", ['entriesApi', function (entriesApi) {
        return {
            markEntriesRead: function (model, entries) {
                entriesApi.markEntriesRead(entries.streamType, entries.displayedRss, entries.subscriptionId, function () {
                    model.channels.current.count = 0;
                    model.channel.entries.rssEntryToReadViewModels = [];
                });
            },
            markReadWithEvent: function (model, streamType, item) {
                if (!item.isRead) {
                    entriesApi.markReadWithEvent(streamType, item.rssEntryViewModel.id, item.rssEntryViewModel.subscriptionId,
                        function () {
                            if (!item.isRead) {
                                model.channels.current.count--;
                            }
                            item.isRead = true;
                        });
                }
            },
            onArticleBodyClicked: function (streamType, item) {
                entriesApi.onArticleBodyClicked(streamType, item.rssEntryViewModel.id, item.rssEntryViewModel.subscriptionId, function () {
                    window.open(item.rssEntryViewModel.url, "_blank");
                });
            },
            onThumbsUpClicked: function ($http, streamType, id) {
                entriesApi.onThumbsUpClicked(id, function () {});
            },
            onThumbsDownClick: function ($http, streamType, id) {
                entriesApi.onThumbsDownClick(id, function () {});
            },
            onMarkUnreadClicked: function ($http, streamType, id) {
                entriesApi.onMarkUnreadClicked(id, function () {});
            },
            onShareClicked: function ($http, streamType, id) {
                entriesApi.onShareClicked(id, function () {});
            },
            onCommentsClicked: function ($http, streamType, id) {
                entriesApi.onCommentsClicked(id, function () {});
            },
            onReadLaterClicked: function ($http, streamType, id) {
                entriesApi.onReadLaterClicked(id, function () {});
            }
        }
    }]);