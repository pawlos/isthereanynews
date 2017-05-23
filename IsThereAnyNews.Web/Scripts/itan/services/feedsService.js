angular
    .module("itan")
    .factory("feedsService", ['feedsApi', function (feedsApi) {
        return {
            onClick: function (model, feed, callback) {
                feedsApi.loadFeedChannel(feed.id, function (data) {
                    callback();
                    model.entries = data.rssEntryViewModels;
                });
            },
            loadFeeds: function (model) {
                feedsApi.loadFeeds(function (data) {
                    model.feeds = data.allChannels;
                    model.feedsLoaded = true;
                });
            },
            isCurrent: function (model, feed) {
                var x = model.currentFeed === feed;
                if (x) {
                    return "btn-info";
                }
                return "";
            },
            buttonSubscriptionClass: function (feed) {
                return feed.isSubscribed ? "btn-danger" : "btn-primary";
            },
            onSubscribeClick: function (model, feedId, isSubscribed) {
                if (isSubscribed) {
                    this.unsubscribe(model, feedId);
                } else {
                    this.subscribe(model, feedId);
                }
            },
            subscribe: function (model, feedId) {
                feedsApi.subscribe(feedId, function () {
                    model.currentFeed.isSubscribed = true;
                });
            },
            unsubscribe: function (model, feedId) {
                feedsApi.unsubscribe(feedId, function () {
                    model.currentFeed.isSubscribed = false;
                });
            },
            loadMoreFeeds: function (model) {
                var toSkip = model.feeds.length;
                var feeds = feedsApi.loadMoreFeeds(toSkip, function (loadedFeeds) {
                    model.feeds = model.feeds.concat(loadedFeeds.allChannels);
                });
            }
        };
    }]);