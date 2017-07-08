angular
    .module("itan")
    .factory("entriesService", ['entriesApi', function (entriesApi) {
        return {
            markEntriesRead: function (model, entries) {
                var x = _.reject(model.channel.entries.rssEntryToReadViewModels, function (i) { return i.rssEntryViewModel.isRead });
                var y = _.map(x, function (i) { return i.rssEntryViewModel.id });
                entriesApi.markEntriesRead(entries.streamType, y, entries.subscriptionId, function () {
                    model.channels.current.count -= y.length;
                    model.channel.entries.rssEntryToReadViewModels = [];
                });
            },
            markReadWithEvent: function (model, streamType, item) {
                if (!item.rssEntryViewModel.isRead) {
                    entriesApi.markReadWithEvent(streamType, item.rssEntryViewModel.id, item.rssEntryViewModel.subscriptionId,
                        function () {
                            if (!item.rssEntryViewModel.isRead) {
                                model.channels.current.count--;
                            }
                            item.rssEntryViewModel.isRead = true;
                        });
                }
            },
            onArticleBodyClicked: function (streamType, item) {
                entriesApi.onArticleBodyClicked(streamType, item.rssEntryViewModel.id, item.rssEntryViewModel.subscriptionId, function () {
                    window.open(item.rssEntryViewModel.url, "_blank");
                });
            },
            onThumbsUpClicked: function ($http, streamType, id) {
                entriesApi.onThumbsUpClicked(id, function () { });
            },
            onThumbsDownClick: function ($http, streamType, id) {
                entriesApi.onThumbsDownClick(id, function () { });
            },
            onMarkUnreadClicked: function ($http, streamType, id) {
                entriesApi.onMarkUnreadClicked(id, function () { });
            },
            onShareClicked: function ($http, streamType, id) {
                entriesApi.onShareClicked(id, function () { });
            },
            onCommentsClicked: function ($http, streamType, id) {
                entriesApi.onCommentsClicked(id, function () { });
            },
            onReadLaterClicked: function ($http, streamType, id) {
                entriesApi.onReadLaterClicked(id, function () { });
            }
        }
    }]);