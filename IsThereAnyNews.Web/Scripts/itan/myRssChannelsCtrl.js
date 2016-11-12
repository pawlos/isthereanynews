angular.module("itan")
.controller("itan.MyRssChannelsCtrl", function ($scope, $http) {
    $scope.channels = {
        loaded: false
    };

    $scope.channel = {
        loaded: false
    };

    $scope.loadChannels = function () {
        $http.get("/RssChannel/MyChannelList")
       .success(function (data) {
           $scope.channels.list = data;
           $scope.channels.loaded = true;
       });
    }

    $http.get("/RssChannel/MyChannelList")
        .success(function (data) {
            $scope.channels.list = data;
            $scope.channels.loaded = true;
        });

    $scope.onChannelClick = function (streamType, subscriptionId) {
        $http.get("/Stream/ReadAjax?streamType=" + streamType + "&id=" + subscriptionId)
            .success(function (data) {
                $scope.channel.loaded = true;
                $scope.channel.entries = data;
                $(".nocss-rss-item-list")
               .collapse({
                   toggle: false
               });
            });
    };

    $scope.markEntriesRead = function (entries) {
        var ids = entries.RssEntryToReadViewModels.map(function (e) {
            return e.Id;
        });
        var httpOptions = {
            method: 'POST',
            url: "/Stream/MarkEntriesRead",
            data: {
                StreamType: entries.StreamType,
                DisplayedItems: ids.join(),
                Id:entries.SubscriptionId
            }

        };
        $http(httpOptions);
    }

    $scope.markReadWithEvent = function (streamType, id) {
        var httpOptions = {
            method: 'POST',
            url: "/Stream/MarkReadWithEvent",
            data: {
                StreamType: streamType,
                Id: id,
                DisplayedItems:id
            }
        };
        $http(httpOptions)
            .success(function () {
            });
    };

    $scope.onArticleBodyClicked = function (streamType, id, url) {
        var httpOptions = {
            method: 'POST',
            url: "/Stream/MarkClickedWithEvent",
            data: {
                Id: id
            }
        };
        $http(httpOptions)
            .success(function () {
            });
        window.open(url, "_blank");
    };

    $scope.onThumbsUpClicked = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/RssItemAction/VoteUp",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };
    $scope.onThumbsDownClick = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/RssItemAction/VoteDown",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };

    $scope.onMarkUnreadClicked = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/RssItemAction/MarkNotRead",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };

    $scope.onShareClicked = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/RssItemAction/Share",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };
    $scope.onCommentsClicked = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/RssItemAction/AddComment",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };

    $scope.onReadLaterClicked = function (streamType, id) {
        var httpOptions = {
            method: "POST",
            url: "/RssItemAction/AddToReadLater",
            data: {
                streamType: streamType,
                id: id
            }
        };
        $http(httpOptions);
    };

    $scope.onAddChannelClicked = function (newChannel) {
        var options = {
            method: "POST",
            url: "/RssChannel/AddChannel",
            data: {
                RssChannelLink: newChannel.link,
                RssChannelName: newChannel.title
            }
        };
        $http(options)
            .success(function () {
                $scope.loadChannels();
            });
    };

});

