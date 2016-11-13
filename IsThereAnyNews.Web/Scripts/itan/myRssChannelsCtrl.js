angular.module("itan")
.controller("itan.MyRssChannelsCtrl", function ($scope, $http) {
    $scope.channels = {
        loaded: false,
        list: {},
        current: {}
    };

    $scope.channel = {
        loaded: false,
        entries: {}
    };

    $scope.maxHeight = function () {
        var h = document.documentElement.clientHeight -
            $(".navbar-fixed-top").height() -
            $(".navbar-fixed-bottom").height() -
            20; // missing px somewhere :)
        return h;
    };

    $(window).on("resize.doResize", function () {
        $scope.$apply(function () {
            $scope.setHeights();
        });
    });

    $scope.setHeights = function () {
        var h = $(".height");
        h.height($scope.maxHeight());
        h[0].style.overflowY = "auto";
        h[1].style.overflowY = "auto";
        h[0].style.overflowX = "hidden";
        h[1].style.overflowX = "hidden";
    }

    $scope.loadChannels = function () {
        $http.get("/RssChannel/MyChannelList")
       .success(function (data) {
           $scope.channels.list = data;
           $scope.channels.loaded = true;
       });
    }

    $scope.isCurrent = function (channel) {
        var x = channel === $scope.channels.current;
        if (x) {
            return "btn-info";
        }
        return "";
    }

    $http.get("/RssChannel/MyChannelList")
        .success(function (data) {
            $scope.channels.list = data;
            $scope.channels.loaded = true;
            $scope.setHeights();
        });

    $scope.onChannelClick = function (channel) {
        $scope.channels.current = channel;
        $http.get("/Stream/ReadAjax?streamType=" + channel.StreamType + "&id=" + channel.Id)
            .success(function (data) {
                $scope.channel.loaded = true;
                $scope.channel.entries = data;
                $scope.setHeights();
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
                Id: entries.SubscriptionId
            }

        };
        $http(httpOptions)
        .success(function () {
            $scope.channels.current.Count = 0;
        });
    }

    $scope.markReadWithEvent = function (streamType, item) {
        var httpOptions = {
            method: 'POST',
            url: "/Stream/MarkReadWithEvent",
            data: {
                StreamType: streamType,
                Id: item.Id,
                DisplayedItems: item.Id
            }
        };
        $http(httpOptions)
            .success(function () {
                if (!item.wasRead) {
                    $scope.channels.current.Count--;
                }
                item.wasRead = true;
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

